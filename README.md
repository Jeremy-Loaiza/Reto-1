
F2X - Backend Sistema de Recaudos
Descripción
Este repositorio contiene el Backend (API REST) del proyecto F2X - Sistema de Recaudos. Proporciona los servicios necesarios para la gestión y consulta de recaudos almacenados en base de datos SQL Server.
Repositorio:
https://github.com/Jeremy-Loaiza/Reto-1
________________________________________
Tecnologías utilizadas
•	ASP.NET Web API
•	C#
•	Entity Framework
•	SQL Server
•	.NET Framework 4.7+
________________________________________
Descarga del proyecto
Con Git
git clone https://github.com/Jeremy-Loaiza/Reto-1.git
Sin Git
1.	Ir al repositorio
2.	Clic en Code → Download ZIP
3.	Descomprimir la carpeta
________________________________________


Requisitos
Obligatorios
•	Visual Studio Code
•	SQL Server
•	SQL Server Management Studio
•	.NET Framework 4.7 o superior
Recomendados
•	Git
________________________________________
Paquetes NuGet necesarios
Estos paquetes son requeridos para el correcto funcionamiento del API:
Paquete	Función
EntityFramework	ORM y acceso a base de datos
Microsoft.AspNet.WebApi	Framework del API
Microsoft.AspNet.WebApi.Core	Núcleo Web API
Microsoft.AspNet.WebApi.Cors	Habilita CORS
System.Data.SqlClient	Conectividad SQL Server
Newtonsoft.Json	Manejo de JSON
Instalación desde Consola NuGet
En Visual Studio abrir:
Herramientas → Administrador de paquetes NuGet → Consola
Ejecutar:
Install-Package EntityFramework
Install-Package Microsoft.AspNet.WebApi
Install-Package Microsoft.AspNet.WebApi.Core
Install-Package Microsoft.AspNet.WebApi.Cors
Install-Package System.Data.SqlClient
Install-Package Newtonsoft.Json
________________________________________

Configuración de base de datos
1.	Abrir SQL Server Management Studio
2.	Crear base de datos:
RecaudosDB
3.	Abrir archivo appsettings.json y configurar:
<connectionStrings>
  <add name="RecaudosDB"
       connectionString="Server=.;Database=RecaudosDB;Trusted_Connection=True;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
Si usas usuario y contraseña:
Server=.;Database=RecaudosDB;User Id=USUARIO;Password=CLAVE;
________________________________________
Cómo ejecutar
1.	Abrir la terminal de Visual Studio Code
2.	Ejecutar el comando: dotnet run

El API quedará disponible en:
http://localhost:PUERTO/api/recaudos
________________________________________
Funcionalidades del API
•	Listado de recaudos
•	Filtros por criterios
•	Paginación
•	Importación de datos
________________________________________


Problemas comunes
Error de conexión SQL
•	Verificar SQL Server activo
•	Revisar cadena de conexión
________________________________________
Autor
Jeremy Loaiza
Reto F2X
________________________________________
Guardar como:
README.md

