#Sistema de Gestión de Productos - ASISYA
Sistema completo de gestión de productos y categorías desarrollado con arquitectura limpia, diseñado para alta escalabilidad y rendimiento en el manejo de grandes volúmenes de datos.


## Decisiones Arquitectonicas

- Mantenibilidad: Separación clara de responsabilidades facilita cambios futuros
- Escalabilidad: Cada capa puede evolucionar independientemente
- Testabilidad: El dominio puro sin dependencias externas facilita las pruebas unitarias

##  Arquitectura

El proyecto implementa **Domain-Driven Design (DDD)** con una clara separación de responsabilidades en capas, garantizando alta cohesión y bajo acoplamiento.

## Patrones de Diseño

### 1. Repository Pattern
**Propósito**: Abstracción de la capa de datos

**Ventajas**:
- Desacoplamiento del ORM
- Facilita testing con mocks
- Centraliza lógica de acceso a datos

### 2. Unit of Work Pattern
**Propósito**: Manejo transaccional de múltiples operaciones

**Ventajas**:
- Garantiza atomicidad en operaciones complejas
- Reduce viajes a la BD
- Mantiene consistencia de datos

### 3. Specification Pattern
**Propósito**: Encapsular lógica de consultas reutilizable


**Ventajas**:
- Reutilización de consultas complejas
- Testeable independientemente
- Composición de criterios


## CQRS (Command Query Responsibility Segregation)
Propósito: Separar operaciones de lectura y escritura

## Escalabilidad en Cloud

### Estrategias de Escalabilidad Horizontal

#### 1. Frontend (React)

# Opción A: AWS ECS Fargate
service:
  type: Fargate
  desiredCount: 3  # Mínimo 3 instancias
  autoscaling:
    minTasks: 3
    maxTasks: 20
    targetCPU: 70%
    targetMemory: 80%
  loadBalancer:
    type: Application Load Balancer
    healthCheck: /index.html

# Opción B: S3 + CloudFront (Estático)
# Build optimizado
npm run build

# Deploy a S3
aws s3 sync build/ s3://shop-frontend-bucket --delete

# Invalidar caché de CloudFront
aws cloudfront create-invalidation --distribution-id XXX --paths "/*"

Ventajas:
- CDN global (baja latencia)
- Escalado automático ilimitado
- Costo muy bajo
- HTTPS incluido

#### 2. Backend (.NET API)

# AWS ECS con Auto Scaling
{
  "family": "shop-backend",
  "taskRoleArn": "arn:aws:iam::xxx:role/ecsTaskRole",
  "networkMode": "awsvpc",
  "containerDefinitions": [{
    "name": "backend-api",
    "image": "shop-backend:latest",
    "cpu": 512,
    "memory": 1024,
    "portMappings": [{
      "containerPort": 80,
      "protocol": "tcp"
    }],
    "environment": [
      { "name": "ASPNETCORE_ENVIRONMENT", "value": "Production" },
      { "name": "ConnectionStrings__DefaultConnection", "value": "xxx" }
    ],
    "logConfiguration": {
      "logDriver": "awslogs",
      "options": {
        "awslogs-group": "/ecs/shop-backend",
        "awslogs-region": "us-east-1",
        "awslogs-stream-prefix": "ecs"
      }
    }
  }]
}

# Auto Scaling Policy
{
  "TargetTrackingScalingPolicyConfiguration": {
    "TargetValue": 70.0,
    "PredefinedMetricSpecification": {
      "PredefinedMetricType": "ECSServiceAverageCPUUtilization"
    },
    "ScaleOutCooldown": 60,
    "ScaleInCooldown": 300
  }
}

Comportamiento:
- CPU > 70% → Agregar instancia (en 60 seg)
- CPU < 70% → Eliminar instancia (en 5 min)
- Rango: 2-10 instancias

#### 3. Base de Datos (PostgreSQL)

# Configuración principal
resource "aws_db_instance" "primary" {
  identifier     = "shop-db-primary"
  engine         = "postgres"
  engine_version = "15.3"
  instance_class = "db.r6g.xlarge"
  
  multi_az = true
  
  allocated_storage     = 100
  max_allocated_storage = 1000
  storage_type          = "gp3"
  
  backup_retention_period = 7
  backup_window           = "03:00-04:00"
  
  performance_insights_enabled = true
}

# Read Replicas para lectura
resource "aws_db_instance" "read_replica_1" {
  identifier          = "shop-db-read-1"
  replicate_source_db = aws_db_instance.primary.id
  instance_class      = "db.r6g.large"
}

Beneficios:
- Alta disponibilidad con Multi-AZ
- Escalado de lectura con Read Replicas
- Backups automáticos y Performance Insights habilitado



## Instalación y Ejecución

### Docker Compose

```bash
# 2. Levantar todos los servicios desde la carpeta raíz
docker-compose up -d --build

# 3. Verificar que los servicios están corriendo
docker-compose ps

# Deberáas ver:
# shop-db        postgres:15    Up (healthy)   5432/tcp, 0.0.0.0:5431->5432/tcp
# shop-backend   ...            Up             0.0.0.0:5000->80/tcp
# shop-frontend  ...            Up             0.0.0.0:3000->80/tcp

# 4. Ver logs en tiempo real
docker-compose logs -f

# 5. Acceder a la aplicación
# Frontend: http://localhost:3000
# Backend:  http://localhost:5000
# Swagger:  http://localhost:5000/swagger
```