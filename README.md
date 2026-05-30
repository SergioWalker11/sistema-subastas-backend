# 🚀 Sistema de Subastas en Línea — Backend

API REST desarrollada en **ASP.NET Core 8** para la gestión de una plataforma de subastas en línea. El sistema permite la publicación de productos, gestión de subastas en tiempo real, registro de pujas, procesamiento de pagos simulados, envío de notificaciones y administración de usuarios mediante roles.

---

## 📋 Características Principales

* ✅ Autenticación y autorización mediante JWT.
* ✅ Gestión de usuarios con roles (Administrador, Vendedor y Comprador).
* ✅ Creación y administración de productos.
* ✅ Gestión completa de subastas.
* ✅ Registro y validación de pujas.
* ✅ Simulación de pasarela de pagos.
* ✅ Sistema de notificaciones automáticas.
* ✅ Procesos automáticos para cierre de subastas y control de pagos vencidos.
* ✅ Arquitectura en capas.
* ✅ Cobertura mediante pruebas unitarias con TDD.

---

# 🛠️ Stack Tecnológico

| Categoría     | Tecnología              |
| ------------- | ----------------------- |
| Framework     | ASP.NET Core 8.0        |
| Lenguaje      | C# 12                   |
| ORM           | Entity Framework Core 8 |
| Base de Datos | PostgreSQL 16           |
| Autenticación | JWT (JSON Web Tokens)   |
| Seguridad     | BCrypt.Net-Next 4.0.3   |
| Documentación | Swagger / Swashbuckle   |
| Testing       | xUnit + Moq             |

---

# 🏗️ Arquitectura del Sistema

Controladores
      │
      ▼
Servicios
      │
      ▼
Repositorios
      │
      ▼
PostgreSQL

### Capas

#### Controladores

Gestionan las solicitudes HTTP y devuelven respuestas JSON estandarizadas.

####  Servicios

Implementan la lógica de negocio de la aplicación.

#### Repositorios

Encapsulan el acceso a datos mediante Entity Framework Core.

### DTOs

Permiten transferir información entre capas evitando exponer entidades directamente.

#### Interfaces

Facilitan la inversión de dependencias y la inyección de servicios.


#Estructura del Proyecto

SistemaSubastaBackend/

├── Controladores/
├── DTOs/
├── Interfaces/
├── Modelos/
├── Repositorios/
├── Servicios/
├── ServiciosExternos/
├── Utilidades/
├── Middleware/
├── Datos/
├── Migraciones/
├── Refactorizacion/
└── Program.cs

Descripción de Carpetas

| Carpeta           | Descripción                           |
| ----------------- | ------------------------------------- |
| Controladores     | Endpoints REST                        |
| DTOs              | Objetos de transferencia              |
| Interfaces        | Contratos de servicios y repositorios |
| Modelos           | Entidades del dominio                 |
| Repositorios      | Acceso a datos                        |
| Servicios         | Lógica de negocio                     |
| ServiciosExternos | Integraciones externas simuladas      |
| Utilidades        | Componentes reutilizables             |
| Middleware        | Manejo global de excepciones          |
| Datos             | DbContext y Seed de datos             |
| Migraciones       | Migraciones de EF Core                |
| Refactorizacion   | Evidencias de refactorización         |



# 🔐 Endpoints Principales

## Autenticación

| Método | Endpoint             |
| ------ | -------------------- |
| POST   | `/api/auth/login`    |
| POST   | `/api/auth/registro` |

## Subastas

| Método | Endpoint                             |
| ------ | ------------------------------------ |
| GET    | `/api/subastas`                      |
| GET    | `/api/subastas/{id}`                 |
| POST   | `/api/subastas`                      |
| GET    | `/api/subastas/vendedor/{id}`        |
| GET    | `/api/subastas/ganadas/{id}`         |
| GET    | `/api/subastas/pendientes-pago/{id}` |
| GET    | `/api/subastas/ventas/{id}`          |
| PATCH  | `/api/subastas/{id}/estado`          |

## Pujas

| Método | Endpoint                  |
| ------ | ------------------------- |
| POST   | `/api/pujas`              |
| GET    | `/api/pujas/subasta/{id}` |

## Pagos

| Método | Endpoint                  |
| ------ | ------------------------- |
| POST   | `/api/pagos`              |
| GET    | `/api/pagos/{id}`         |
| GET    | `/api/pagos/usuario/{id}` |

## Notificaciones

| Método | Endpoint                           |
| ------ | ---------------------------------- |
| GET    | `/api/notificaciones/usuario/{id}` |
| POST   | `/api/notificaciones`              |
| PATCH  | `/api/notificaciones/{id}/leida`   |

## Administración

| Método | Endpoint                            |
| ------ | ----------------------------------- |
| GET    | `/api/admin/usuarios`               |
| PATCH  | `/api/admin/usuarios/{id}/rol`      |
| PATCH  | `/api/admin/subastas/{id}/cancelar` |

---

# 🗄️ Modelo de Base de Datos

El sistema está compuesto por **9 tablas principales**:


roles
 └── usuarios
      ├── pujas
      ├── pagos
      ├── notificaciones
      └── subastas

categorias
 └── productos
       ├── subastas
       └── imagenes_producto


# 🔄 Flujo de Estados de una Subasta

ACTIVA
   │
   ▼
PENDIENTE_PAGO
   │
   ├──► VENDIDA
   │
   └──► INCUMPLIDA

ACTIVA
   │
   └──► CANCELADA


---

Componentes Reutilizables

| Componente             | Función                       |
| ---------------------- | ----------------------------- |
| AyudanteRespuestaAPI   | Estandarización de respuestas |
| ValidadorPujas         | Validación de reglas de pujas |
| ServicioPasarelaPagos  | Simulación de pagos           |
| ServicioNotificaciones | Generación de notificaciones  |
| MiddlewareExcepciones  | Manejo global de errores      |
| ServicioCierreSubastas | Cierre automático             |
| ServicioPagosVencidos  | Control de pagos pendientes   |


 Patrones de Diseño Aplicados

| Patrón               | Aplicación                   |
| -------------------- | ---------------------------- |
| Repository           | Acceso a datos               |
| DTO                  | Transferencia de información |
| Dependency Injection | Resolución de dependencias   |
| Strategy             | Validación de pujas y pagos  |
| State                | Estados de subasta           |
| Observer             | Sistema de notificaciones    |

Pruebas Unitarias (TDD)

El proyecto incluye **28 pruebas unitarias** desarrolladas utilizando:

* xUnit
* Moq



# 🔧 Refactorizaciones Aplicadas

### 1. Extract Class + Replace Magic Number

Refactorizacion/PagosRefactorizado.cs

### 2. Extract Method + Replace Nested Conditional

Refactorizacion/SubastasRefactorizado.cs

### 3. Consolidación de Manejo de Errores

Middleware/MiddlewareExcepciones.cs

Ejecución del Proyecto

## Requisitos

* .NET SDK 8.0 o superior
* PostgreSQL 16 o superior





