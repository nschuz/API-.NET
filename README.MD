# TiendaOnlineAPI

Este proyecto es una API para la gestión de una tienda online, desarrollada con .NET 8.0. La API incluye modelos de productos, pedidos y la relación entre ellos, permitiendo la gestión de inventario, órdenes de compra y más. La base de datos utilizada es SQL Server y todo el entorno está gestionado mediante Docker.

## Requisitos

- [Docker](https://www.docker.com/get-started) instalado en tu máquina.
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) (para desarrollo y ejecución local).
- Un editor de texto o IDE, como [Visual Studio Code](https://code.visualstudio.com/).

## Configuración del entorno

### 1. Clonar el repositorio

```bash
git clone https://github.com/usuario/TiendaOnlineAPI.git
cd TiendaOnlineAPI
```

### 2. Configuración del Docker Compose

El archivo `docker-compose.yml` define dos servicios:

- **db**: Un contenedor de SQL Server que expone el puerto 1433.
- **web**: La aplicación API que se construye y despliega usando .NET 8.0.

Aquí está el contenido del archivo `docker-compose.yml`:

```yaml
services:
  db:
    image: mcr.microsoft.com/mssql/server:2019-latest
    container_name: sql_server_db
    environment:
      SA_PASSWORD: 'root'
      ACCEPT_EULA: 'Y'
    ports:
      - '1433:1433'
    networks:
      - app-network
    healthcheck:
      test: ["CMD-SHELL", "/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'root' -Q 'SELECT 1'"]
      interval: 30s
      timeout: 10s
      retries: 5

  web:
    build:
      context: .
      dockerfile: deployment/Dockerfile
    container_name: tienda_online_api
    depends_on:
      - db
    environment:
      - ConnectionStrings__DefaultConnection=Server=db;Database=TiendaOnlineDB;User Id=sa;Password=root
    ports:
      - '8080:80'
    networks:
      - app-network
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:80"]
      interval: 30s
      timeout: 10s
      retries: 5

networks:
  app-network:
```

### 3. Dockerfile

Este es el archivo `Dockerfile` utilizado para construir la imagen de la API:

```dockerfile
# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./

RUN dotnet tool install --global dotnet-ef --version 8.0.10
ENV PATH="$PATH:/root/.dotnet/tools"

RUN dotnet publish -c Release -o out

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

RUN apt-get update && apt-get install -y --no-install-recommends libssl-dev libsqlite3-dev && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/out .

COPY deployment/entrypoint.sh ./

RUN chmod +x ./entrypoint.sh

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

EXPOSE 80

ENTRYPOINT ["./entrypoint.sh"]
```

### 4. Construir y ejecutar la aplicación

Para construir y ejecutar el proyecto con Docker, sigue estos pasos:

1. Construir los contenedores y servicios definidos en `docker-compose.yml`:

   ```bash
   docker-compose up --build
   ```

   Este comando levantará los servicios de base de datos SQL Server y la API.

2. Verificar que la aplicación está corriendo:

   Accede a la API en `http://localhost:8080`.

### 5. Ejecución de migraciones

Para crear la base de datos e implementar las migraciones:

```bash
docker exec -it tienda_online_api dotnet ef database update
```

### 6. Endpoints de la API

La API expone los siguientes endpoints:

- **Pedidos:**
  - **GET /pedidos**: Obtener la lista de pedidos.
  - **POST /pedidos**: Crear un nuevo pedido.
  - **GET /pedidos/{id}**: Obtener un pedido específico por ID.
  - **PUT /pedidos/{id}**: Actualizar un pedido por ID.
  - **DELETE /pedidos/{id}**: Eliminar un pedido por ID.

- **Productos:**
  - **GET /productos**: Obtener la lista de productos.
  - **POST /productos**: Crear un nuevo producto.
  - **GET /productos/{id}**: Obtener un producto específico por ID.
  - **PUT /productos/{id}**: Actualizar un producto por ID.
  - **DELETE /productos/{id}**: Eliminar un producto por ID.

- **Gestión de productos en un pedido:**
  - **GET /pedidos/{id}/productos**: Obtener la lista de productos en un pedido específico.
  - **POST /pedidos/{id}/productos**: Añadir productos a un pedido específico.

### 7. Detener los contenedores

Para detener la ejecución de los contenedores, usa:

```bash
docker-compose down
```

### 8. Solución de problemas

- **Problemas con la conexión a la base de datos**: Asegúrate de que el contenedor de la base de datos está corriendo y que las credenciales son correctas en el archivo `docker-compose.yml`.
- **Errores de construcción del contenedor**: Revisa el `Dockerfile` y verifica que todos los archivos necesarios están presentes en la carpeta de trabajo.
- **Puertos en uso**: Si los puertos 1433 o 8080 ya están en uso, modifica los puertos expuestos en el `docker-compose.yml`.

## Contribuciones

Si deseas contribuir al proyecto, sigue estos pasos:

1. Haz un _fork_ del repositorio.
2. Crea una rama para tu feature (`git checkout -b feature/nueva-funcionalidad`).
3. Realiza tus cambios y haz _commit_ (`git commit -am 'Agrega nueva funcionalidad'`).
4. Haz _push_ a la rama (`git push origin feature/nueva-funcionalidad`).
5. Abre un _pull request_.

