## ECOMMERCE SALES CARS

Aplicación web desarrollada con **React** en el frontend y **C# (ASP.NET Core Web API)** en el backend, que permite, crear usuario, publicar vehículos, realizar ofertas, gestionar ofertas, gestionar compras/ventas y filtrar por distintas características.

## Descripción general

Este proyecto permite a los usuarios:
- Crear una cuenta.
- Loguearse en la aplicación.
- Crear publicaciones de vehículos con imágenes e información detallada.
- Ver detalles de cada publicación.
- Ofertar en publicaciones de otros usuarios.
- Aceptar o rechazar ofertas recibidas.
- Ver todas sus publicaciones, ofertas realizadas y compras/ventas.
- Filtrar publicaciones por marca, año y precio.
- Navegar en una interfaz completamente responsive.

## Tecnologías utilizadas

**Frontend:**
- React
- HTML / CSS / JavaScript
- Axios

**Backend:**
- C# / ASP.NET Core Web API
- Entity Framework Core
- SQL Server

**Otros:**
- Git / GitHub
- Entity Framework Core Migrations
- Visual Studio / Visual Studio Code

## Instalación y ejecución

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/RodrigoCorrea31/E-CommerceSalesCars.git

## Backend (.NET Web API)

Entrar a la carpeta del backend:

cd ECommerceSalesCars

Restaurar las dependencias:

dotnet restore


Configurar la cadena de conexión mediante User Secrets:

dotnet user-secrets set "ConnectionStrings:StrConn" "Server=TU_SERVIDOR;Database=ECommerceSalesCarsBD;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False"
Reemplazá TU_SERVIDOR por el nombre de tu instancia local de SQL Server.

(Opcional) Crear la base de datos y aplicar migraciones:

dotnet ef database update

Ejecutar el proyecto:

dotnet run


Abrir la documentación de la API:

https://localhost:7253/swagger

## Frontend (React)

Entrar a la carpeta del frontend:

cd ../ecommerce-autos


Instalar dependencias:

npm install


Crear un archivo .env en la raíz del frontend con la URL del backend:

REACT_APP_API_URL=https://localhost:7253


Ejecutar la aplicación:

npm run dev


Abrir en el navegador:

http://localhost:5173



## Autor
Rodrigo Correa

Correo electrónico: rodrigocorreamuse@gmail.com

Mi perfil de LinkedIn: [LinkedIn](https://www.linkedin.com/in/rodrigo-correa-b868b1368/)
