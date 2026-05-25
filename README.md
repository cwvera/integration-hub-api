# INTEGRATION HUB API

Backend de orquestación para la sincronización masiva de datos entre sistemas externos, desarrollado en .NET 9 siguiendo los estándares de **Clean Architecture** y **CQRS**.

## TECNOLOGÍAS UTILIZADAS

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core 9
- MediatR (CQRS)
- Parallel.ForEachAsync (Procesamiento Paralelo)
- OAuth 2.0 (DelegatingHandler para gestión de tokens)
- xUnit & Moq (Pruebas Unitarias)
- FluentAssertions
- Swagger / OpenAPI

## ARQUITECTURA

La solución está dividida en las siguientes capas:

- **Domain**: Entidades y lógica central del negocio.
- **Application**: Casos de uso, Commands, Queries y lógica de orquestación.
- **Infrastructure**: Implementación de persistencia, seguridad (OAuth) y adaptadores de integración.
- **WebApi**: Controladores versionados y configuración global.
- **Commons**: Objetos de respuesta y mensajes compartidos.

## PATRONES IMPLEMENTADOS

- **CQRS**: Separación de operaciones de lectura y escritura.
- **Strategy & Factory Pattern**: Resolución dinámica de adaptadores para múltiples sistemas (REST, SOAP).
- **DelegatingHandler**: Gestión transparente del ciclo de vida de tokens de acceso.
- **Parallel processing**: Procesamiento de lotes de forma asíncrona y paralela con control de concurrencia.

## CONFIGURACIÓN

1. Configurar la cadena de conexión en `src/IntegrationHub.WebApi/appsettings.json`.
2. Ejecutar las migraciones o asegurar que la base de datos existe (soporte para LocalDB por defecto).

## EJECUCIÓN

Desde la carpeta `src/`:

```bash
dotnet restore
dotnet build
dotnet run --project IntegrationHub.WebApi
```

Swagger estará disponible en: `https://localhost:{puerto}/swagger/index.html`

## PRUEBAS

Para ejecutar las pruebas unitarias:

```bash
dotnet test
```

---
*Desarrollado como parte de la Prueba Técnica FullStack NYXN.*
