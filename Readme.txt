Acerca del API

------------------------------------------------------------------------------------------------------------------------------------
Comandos básicos
------------------------------------------------------------------------------------------------------------------------------------

# desde la raíz de la solución, para restarurar y compilar la solución
dotnet restore
dotnet build
dotnet clean

# ejecutar la API (Kestrel) leyendo ApiBase.Web como proyecto de inicio
dotnet run --project ApiBase.Web

# hot-reload para desarrollo
dotnet watch --project ApiBase.Web run

------------------------------------------------------------------------------------------------------------------------------------
Instalación de paquetes por proyecto
------------------------------------------------------------------------------------------------------------------------------------

dotnet list package

dotnet add ApiBase.DAL package Microsoft.EntityFrameworkCore --version 8.*
dotnet add ApiBase.DAL package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.*
dotnet add ApiBase.DAL package Microsoft.EntityFrameworkCore.Design --version 8.*

dotnet add ApiBase.Web package Swashbuckle.AspNetCore
dotnet add ApiBase.Web package Microsoft.EntityFrameworkCore.Design --version 8.*
dotnet add ApiBase.Web package Microsoft.AspNetCore.OpenApi --version 8.*
dotnet add ApiBase.Web package Microsoft.AspNetCore.Authentication.JwtBearer  --version 8.*


------------------------------------------------------------------------------------------------------------------------------------
Arquitectura y Convenciones de la solución
------------------------------------------------------------------------------------------------------------------------------------

Esta solución sigue una arquitectura en tres capas básicas, que favorece la separación de responsabilidades, la mantenibilidad y la escalabilidad de la aplicación. Cada capa es un proyecto independiente dentro de la solución:

DAL (Data Access Layer) → ApiBase.DAL
Contiene el acceso a datos. Incluye el DbContext y las entidades generadas mediante Entity Framework Core (Database First).
Esta capa se comunica directamente con la base de datos PostgreSQL y nunca debe contener lógica de negocio.

BLL (Business Logic Layer) → ApiBase.BLL
Contiene la lógica de negocio de la aplicación. Define interfaces y servicios que implementan reglas y procesos.
Esta capa utiliza las entidades del DAL, pero abstrae las operaciones a través de servicios que luego consume la capa Web.

WEB (Presentation/API Layer) → ApiBase.Web
Expone la aplicación al exterior a través de una API RESTful.
Contiene controladores, ViewModels/DTOs y la configuración de middlewares (ej. Swagger, autenticación, logging).
Esta capa recibe peticiones HTTP, valida la entrada y delega la lógica al BLL.



------------------------------------------------------------------------------------------------------------------------------------
Convenciones para Scaffold de bases de datos
------------------------------------------------------------------------------------------------------------------------------------

Para mantener consistencia al generar modelos desde la base de datos con Entity Framework Core, se establecen las siguientes convenciones:

1. Instalación de herramientas
Después de instalar el SDK de .NET, es necesario instalar la herramienta dotnet-ef de manera global.
Utiliza una terminal de línea de comandos (CMD o PowerShell) y ejecuta la siguiente instrucción: dotnet tool install --global dotnet-ef

2. Ejemplo de ejecución de Scaffold:
dotnet ef dbcontext scaffold "Host=localhost;Database=BD_Universidad;Username=UsuarioSistema;Password=Sistema1234" Npgsql.EntityFrameworkCore.PostgreSQL --context BD_UniversidadContext --output-dir Modelos_BD_Universidad --project ApiBase.DAL --startup-project ApiBase.Web
dotnet ef dbcontext scaffold "Host=dpg-d3e293idbo4c7390crm0-a.virginia-postgres.render.com;Database=bd_universidad;Username=usuariosistema;Password=MnPk07UzRzEFDEpSNy0v8oEg4hj5M1kl" Npgsql.EntityFrameworkCore.PostgreSQL --context BD_UniversidadContext --output-dir Modelos_BD_Universidad --project ApiBase.DAL --startup-project ApiBase.Web --schema public --use-database-names --force

3. Convenciones de nombres:
--Context : Debe de ser igual al nombre de la base de datos + la palabra "Context"
--ouput-dir: Debe de ser igual a la palabra "Modelos_" + el nombre de la base de datos.
--project : Siempre debe de apuntar al proyecto o capa DAL (acceso a datos)
--startup-project : Debe de apuntar al proyecto WEB (capa de presentación/API).

4. En producción, las claves/contraseñas que se utilicen para realizar diferentes conexiones deben ir en variables de entorno o Azure Key Vault, no en texto plano.
