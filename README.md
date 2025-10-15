# Librería API

API REST para la gestión de autores, libros y préstamos 

## Configuración inicial

1. Clonar el repositorio y restaurar dependencias:

   git clone <url-del-repositorio>
   cd Libreria
   dotnet restore

2. Revisar/ajustar la cadena de conexión en `Libreria.Api/appsettings.json` bajo la clave `ConnectionStrings:DefaultConnection` para que apunte a su instancia de SQL Server.
3. Establecer las variables de entorno si prefiere no exponer credenciales en el archivo `appsettings.json`. Cualquier valor presente en variables de entorno `ConnectionStrings__DefaultConnection` o `JwtSettings__*` tendrá prioridad sobre el archivo de configuración.

## Importar la base de datos

La solución incluye una migración inicial en el proyecto `Libreria.Infrastructure`. Para crear la base de datos y aplicar el esquema:


dotnet ef database update \
  --project Libreria.Infrastructure \
  --startup-project Libreria.Api

## Ejecución de la API

Para compilar y ejecutar el proyecto web:

dotnet run --project Libreria.Api


## Generar un token JWT

1. **Registrar usuario (opcional si ya existe):**

   POST /api/Auth/register
   {
     "username": "admin",
     "password": "P@ssw0rd!",
     "role": "Admin"
   }

   El validador asegura que el rol sea `Admin` o `User` y que el nombre de usuario no exista previamente.

2. **Iniciar sesión:**

   POST /api/Auth/login
   {
     "username": "admin",
     "password": "P@ssw0rd!"
   }

   La respuesta contiene `token`, `expiration`, `username` y `role`. Copie el valor de `token`.

3. **Consumir endpoints protegidos:**
   Envíe el encabezado `Authorization: Bearer <token>` en cada petición que requiera autenticación. Los controladores validan el rol mediante `ClaimsPrincipal` cuando sea necesario.

dotnet test
