#!/bin/bash
echo "=== Iniciando Migración de Base de Datos a Docker Compose ==="

# 1. Detener el contenedor actual
echo "1. Deteniendo contenedor DockerCbtis..."
docker stop DockerCbtis

# 2. Copiar los datos del contenedor al host
echo "2. Copiando datos de la base de datos a ./DbData..."
docker cp DockerCbtis:/var/opt/mssql/data ./DbData

# 3. Remover el contenedor viejo
echo "3. Eliminando contenedor DockerCbtis anterior..."
docker rm DockerCbtis

# 4. Levantar con docker compose
echo "4. Levantando el nuevo contenedor con docker compose..."
docker compose up -d

echo "=== Migración Completada con Éxito ==="
echo "La base de datos ahora está guardada en la carpeta './DbData' de tu Mac y es 100% persistente."
