# FaroPets — Docker y visualización en tiempo real

Este entorno ejecuta la aplicación **ASP.NET Core MVC** y SQL Server en contenedores. La aplicación queda disponible en el navegador y `dotnet watch` vigila los cambios de código.

> **Compatibilidad:** el proyecto original usa .NET 5 y Entity Framework Core 5. .NET 5 está fuera de soporte. Esta configuración reproduce la versión existente para desarrollo; planifica actualizar el proyecto a una versión LTS compatible antes de exponerlo a Internet.

## Requisitos

- Docker Desktop instalado y ejecutándose.
- Docker Compose v2.
- En Windows, abre PowerShell en la carpeta `PAGINA TALLER DE PROYECTO`.

## Iniciar

1. Copia `.env.example` a `.env` y configura una contraseña local fuerte para `SA_PASSWORD`. No publiques `.env`.
2. Desde esta carpeta, ejecuta:

   ```powershell
   Copy-Item .env.example .env
   # Edita .env y reemplaza SA_PASSWORD por una contraseña fuerte.
   docker compose up --build
   ```

3. Abre **http://localhost:8080**. El contenedor de la aplicación se reconstruye cuando cambias archivos C#; los cambios de vistas y archivos estáticos se sirven desde el volumen montado. Si el navegador no se actualiza solo, recarga la página.

## Base de datos

SQL Server corre en el servicio `sqlserver`, con datos persistidos en el volumen `faropets-sql-data`. El entorno usa la base `Faropets` y una cadena de conexión inyectada por variables de entorno; no necesita modificar el `appsettings.json` que apunta al servidor SQL de tu equipo.

Una vez que los contenedores estén arriba, aplica las migraciones existentes para crear el esquema:

```powershell
docker compose exec app dotnet ef database update
```

Si la aplicación no arranca por errores de conexión durante el primer inicio, espera a que SQL Server esté saludable y reinicia el servicio web:

```powershell
docker compose restart app
```

> La base de datos de este contenedor es nueva y no incluye los datos de tu SQL Server local. No se copian ni publican datos personales. Los scripts SQL del repositorio pueden requerir pasos adicionales si contienen tablas fuera de las migraciones de EF Core.

## Comandos útiles

```powershell
# Ver estado
docker compose ps

# Ver logs en vivo
docker compose logs -f app
docker compose logs -f sqlserver

# Detener los servicios, conservando los datos
docker compose down

# Borrar también los datos locales de SQL Server (acción destructiva)
docker compose down -v
```

## Qué significa “tiempo real”

- **Vista en vivo local:** disponible en `http://localhost:8080` mientras Docker Desktop y los contenedores estén ejecutándose.
- **Cambios de código:** `dotnet watch` reinicia/recarga la aplicación al detectar cambios; quizá tengas que actualizar el navegador.
- **Enlace público:** Docker por sí solo no publica la aplicación en Internet. Para una demo pública se requiere desplegarla en un host compatible y configurar la base de datos, secretos y HTTPS.
