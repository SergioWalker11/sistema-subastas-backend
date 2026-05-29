using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Servicios;

public class ServicioAutenticacion : IServicioAutenticacion
{
    private readonly IRepositorioUsuarios _repositorioUsuarios;
    private readonly IConfiguration _configuracion;

    public ServicioAutenticacion(IRepositorioUsuarios repositorioUsuarios, IConfiguration configuracion)
    {
        _repositorioUsuarios = repositorioUsuarios;
        _configuracion = configuracion;
    }

    public async Task<AuthRespuestaDTO> LoginAsync(LoginDTO dto)
    {
        var usuario = await _repositorioUsuarios.ObtenerPorCorreoAsync(dto.Correo);
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Contrasena, usuario.ContrasenaHash))
            throw new UnauthorizedAccessException("Credenciales invalidas");

        return GenerarRespuesta(usuario);
    }

    public async Task<AuthRespuestaDTO> RegistroAsync(RegistroDTO dto)
    {
        var existente = await _repositorioUsuarios.ObtenerPorCorreoAsync(dto.Correo);
        if (existente != null)
            throw new InvalidOperationException("El correo ya esta registrado");

        var usuario = new Usuario
        {
            NombreCompleto = dto.NombreCompleto,
            Correo = dto.Correo,
            ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
            RolId = dto.RolId
        };

        usuario = await _repositorioUsuarios.CrearAsync(usuario);
        return GenerarRespuesta(usuario);
    }

    private AuthRespuestaDTO GenerarRespuesta(Usuario usuario)
    {
        var clave = _configuracion["Jwt:Clave"] ?? "ClaveSuperSecretaParaDesarrolloLocal12345678";
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(ClaimTypes.Name, usuario.NombreCompleto),
            new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "comprador")
        };

        var token = new JwtSecurityToken(
            issuer: _configuracion["Jwt:Emisor"] ?? "SistemaSubasta",
            audience: _configuracion["Jwt:Audiencia"] ?? "SistemaSubastaClient",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave)),
                SecurityAlgorithms.HmacSha256)
        );

        return new AuthRespuestaDTO
        {
            UsuarioId = usuario.Id,
            NombreCompleto = usuario.NombreCompleto,
            Correo = usuario.Correo,
            Rol = usuario.Rol?.Nombre ?? "comprador",
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
    }
}
