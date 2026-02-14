# Plan de Desarrollo: Sistema de Tarifas Eléctricas EPM

## 📋 Descripción General

Sistema web para visualizar y entender las tarifas de energía eléctrica de EPM de forma didáctica. **Conecta directamente a los datos abiertos de GOV.CO** mediante ETL automatizado y presenta la información de manera accesible para usuarios no técnicos.

---

## 📑 Índice

1. [Versión FULL](#versión-full-completa) - Sistema completo con todas las funcionalidades
2. [Versión MINIMAL](#versión-minimal-clean-arch--modular) - Versión reducida pero profesional (2-3 días)

---

# VERSIÓN FULL (Completa)

## 🏗️ Arquitectura General

### Stack Tecnológico
- **Backend**: .NET 10 (Clean Architecture - 4 capas)
- **Frontend**: Angular 19 (Arquitectura Modular por Features)
- **Base de Datos**: PostgreSQL (Data Warehouse dimensional)
- **Fuente de Datos**: API GOV.CO Datos Abiertos

---

## 🗄️ Base de Datos: Modelo Dimensional

### Tabla de Hechos

**`fact_tarifas_electricas`**
```sql
CREATE TABLE fact_tarifas_electricas (
    tarifa_id SERIAL PRIMARY KEY,
    periodo_id INTEGER REFERENCES dim_periodo(periodo_id),
    operador_id INTEGER REFERENCES dim_operador(operador_id),
    nivel_id INTEGER REFERENCES dim_nivel_tension(nivel_id),
    
    fecha_actualizacion TIMESTAMP,
    año INTEGER NOT NULL,
    mes INTEGER NOT NULL,
    
    -- Métricas
    cu_total NUMERIC(10,4) NOT NULL,
    costo_compra_g NUMERIC(10,4),
    cargo_transporte_stn_tm NUMERIC(10,4),
    cargo_transporte_sdl_dm NUMERIC(10,4),
    margen_comercializacion NUMERIC(10,4),
    costo_perdidas_pr NUMERIC(10,4),
    restricciones_rm NUMERIC(10,4),
    cot NUMERIC(10,4),
    cfmj_gfact NUMERIC(10,4),
    
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

### Dimensiones

**`dim_periodo`**
```sql
CREATE TABLE dim_periodo (
    periodo_id SERIAL PRIMARY KEY,
    año INTEGER NOT NULL,
    mes INTEGER NOT NULL,
    nombre_mes VARCHAR(20),
    trimestre INTEGER,
    semestre INTEGER,
    fecha_inicio DATE,
    fecha_fin DATE
);
```

**`dim_operador`**
```sql
CREATE TABLE dim_operador (
    operador_id SERIAL PRIMARY KEY,
    nombre_operador VARCHAR(100) NOT NULL,
    departamento VARCHAR(100),
    municipio VARCHAR(100),
    codigo_divipola VARCHAR(10),
    entidad_territorial VARCHAR(100),
    sector VARCHAR(50)
);
```

**`dim_nivel_tension`**
```sql
CREATE TABLE dim_nivel_tension (
    nivel_id SERIAL PRIMARY KEY,
    codigo_nivel VARCHAR(20) NOT NULL,
    nombre_nivel VARCHAR(100),
    descripcion TEXT,
    tipo_servicio VARCHAR(50) -- Residencial/Comercial/Industrial
);
```

**`dim_componente_tarifa`**
```sql
CREATE TABLE dim_componente_tarifa (
    componente_id SERIAL PRIMARY KEY,
    nombre_componente VARCHAR(100) NOT NULL,
    codigo_componente VARCHAR(50) UNIQUE,
    descripcion_simple TEXT,
    descripcion_tecnica TEXT,
    analogia_explicativa TEXT,
    icono_representativo VARCHAR(50),
    color_visual VARCHAR(7),
    orden_visualizacion INTEGER
);
```

### Tablas de Control

**`etl_log`**
```sql
CREATE TABLE etl_log (
    log_id SERIAL PRIMARY KEY,
    fecha_ejecucion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    tipo_proceso VARCHAR(50), -- Extract/Transform/Load
    estado VARCHAR(20), -- Success/Failed/Warning
    registros_procesados INTEGER,
    registros_exitosos INTEGER,
    registros_fallidos INTEGER,
    mensaje_error TEXT,
    duracion_segundos NUMERIC(10,2),
    usuario_ejecucion VARCHAR(100)
);
```

**`data_source_metadata`**
```sql
CREATE TABLE data_source_metadata (
    metadata_id SERIAL PRIMARY KEY,
    url_fuente TEXT,
    fecha_ultima_actualizacion_fuente TIMESTAMP,
    fecha_ultima_extraccion TIMESTAMP,
    numero_registros_fuente INTEGER,
    checksum_datos VARCHAR(255),
    version_dataset VARCHAR(50)
);
```

---

## 🔧 Backend: Clean Architecture (.NET 10)

### Estructura Completa

```
src/
├── TarifasEPM.Domain/
│   ├── Entities/
│   │   ├── FactTarifaElectrica.cs
│   │   ├── DimPeriodo.cs
│   │   ├── DimOperador.cs
│   │   ├── DimNivelTension.cs
│   │   ├── DimComponenteTarifa.cs
│   │   └── EtlLog.cs
│   ├── ValueObjects/
│   │   ├── Periodo.cs
│   │   ├── CostoComponente.cs
│   │   └── RangoFecha.cs
│   ├── Interfaces/
│   │   └── IRepository.cs
│   └── Enums/
│       ├── TipoComponente.cs
│       ├── EstadoEtl.cs
│       └── TipoServicio.cs
│
├── TarifasEPM.Application/
│   ├── DTOs/
│   │   ├── TarifaDTO.cs
│   │   ├── ComponenteTarifaDTO.cs
│   │   ├── FacturaSimuladaDTO.cs
│   │   ├── ComparacionHistoricaDTO.cs
│   │   └── GovCoResponseDTO.cs
│   ├── Interfaces/
│   │   ├── ITarifaService.cs
│   │   ├── IEtlService.cs
│   │   └── IFacturaService.cs
│   ├── Services/
│   │   ├── TarifaService.cs
│   │   ├── EtlService.cs
│   │   └── FacturaSimuladaService.cs
│   ├── Mappings/
│   │   └── AutoMapperProfile.cs
│   └── Validators/
│       └── TarifaValidator.cs
│
├── TarifasEPM.Infrastructure/
│   ├── Data/
│   │   ├── TarifasDbContext.cs
│   │   └── Configurations/
│   │       ├── FactTarifaConfiguration.cs
│   │       └── DimPeriodoConfiguration.cs
│   ├── Repositories/
│   │   ├── TarifaRepository.cs
│   │   └── EtlLogRepository.cs
│   ├── ExternalServices/
│   │   ├── GovCoApiClient.cs
│   │   └── HttpClientConfig.cs
│   ├── ETL/
│   │   ├── Extractors/
│   │   │   └── GovCoDataExtractor.cs
│   │   ├── Transformers/
│   │   │   ├── TarifaTransformer.cs
│   │   │   └── DataCleaner.cs
│   │   └── Loaders/
│   │       └── PostgresLoader.cs
│   └── Migrations/
│
└── TarifasEPM.WebApi/
    ├── Controllers/
    │   ├── TarifasController.cs
    │   ├── EtlController.cs
    │   ├── FacturaController.cs
    │   └── EducacionController.cs
    ├── Middleware/
    │   ├── ErrorHandlingMiddleware.cs
    │   └── LoggingMiddleware.cs
    ├── Program.cs
    └── appsettings.json
```

### Endpoints Completos

#### TarifasController
```csharp
GET    /api/tarifas                          // Listar todas (paginado)
GET    /api/tarifas/{id}                     // Por ID
GET    /api/tarifas/periodo/{año}/{mes}     // Por período
GET    /api/tarifas/componente/{tipo}       // Por componente
GET    /api/tarifas/comparacion             // Comparación histórica
GET    /api/tarifas/tendencias              // Análisis de tendencias
```

#### EtlController
```csharp
POST   /api/etl/ejecutar                    // ETL completo
POST   /api/etl/extract                     // Solo extracción
POST   /api/etl/transform                   // Solo transformación
POST   /api/etl/load                        // Solo carga
GET    /api/etl/status                      // Estado actual
GET    /api/etl/logs                        // Histórico
GET    /api/etl/logs/{id}                   // Detalle ejecución
```

#### FacturaController
```csharp
POST   /api/factura/simular                 // Simular factura
GET    /api/factura/desglose/{consumo}      // Desglose detallado
GET    /api/factura/ejemplo                 // Factura ejemplo
```

#### EducacionController
```csharp
GET    /api/educacion/componentes           // Todos los componentes
GET    /api/educacion/componente/{tipo}     // Un componente
GET    /api/educacion/analogias             // Todas las analogías
GET    /api/educacion/glosario              // Glosario términos
```

---

## 🎨 Frontend: Angular 19

### Estructura Completa

```
src/app/
├── core/
│   ├── guards/
│   │   └── auth.guard.ts
│   ├── interceptors/
│   │   ├── http-error.interceptor.ts
│   │   └── loading.interceptor.ts
│   ├── services/
│   │   ├── tarifa.service.ts
│   │   ├── etl.service.ts
│   │   └── educacion.service.ts
│   └── models/
│       ├── tarifa.model.ts
│       ├── componente-tarifa.model.ts
│       └── factura.model.ts
│
├── shared/
│   ├── components/
│   │   ├── header/
│   │   ├── footer/
│   │   ├── loading-spinner/
│   │   └── error-message/
│   ├── directives/
│   │   └── tooltip.directive.ts
│   └── pipes/
│       ├── currency-format.pipe.ts
│       └── month-name.pipe.ts
│
├── features/
│   ├── factura-simulada/
│   │   ├── components/
│   │   │   ├── factura-header/
│   │   │   ├── factura-desglose/
│   │   │   ├── factura-total/
│   │   │   └── consumo-input/
│   │   ├── pages/
│   │   │   └── factura-page/
│   │   ├── services/
│   │   │   └── factura.service.ts
│   │   └── factura.routes.ts
│   │
│   ├── educacion/
│   │   ├── components/
│   │   │   ├── componente-card/
│   │   │   ├── analogia-visual/
│   │   │   └── glosario-item/
│   │   ├── pages/
│   │   │   ├── educacion-page/
│   │   │   └── detalle-componente-page/
│   │   └── educacion.routes.ts
│   │
│   ├── dashboard/
│   │   ├── components/
│   │   │   ├── tarifa-chart/
│   │   │   ├── comparacion-card/
│   │   │   └── tendencia-graph/
│   │   ├── pages/
│   │   │   └── dashboard-page/
│   │   └── dashboard.routes.ts
│   │
│   └── admin-etl/
│       ├── components/
│       │   ├── etl-trigger-button/
│       │   ├── etl-log-table/
│       │   └── etl-status-badge/
│       ├── pages/
│       │   └── admin-page/
│       └── admin.routes.ts
│
└── app.routes.ts
```

---

# VERSIÓN MINIMAL (Clean Arch + Modular)

## ⏱️ Tiempo: 2-3 días
## 🔌 Conexión: API GOV.CO Real

---

## 🗄️ Base de Datos Simplificada

### Tabla Principal

```sql
CREATE TABLE tarifas_electricas (
    id SERIAL PRIMARY KEY,
    
    -- Temporal
    año INTEGER NOT NULL,
    mes INTEGER NOT NULL,
    periodo VARCHAR(7) NOT NULL,
    
    -- Entidad
    operador VARCHAR(100) DEFAULT 'EPM',
    nivel VARCHAR(50) NOT NULL,
    
    -- Componentes (9 campos)
    cu_total NUMERIC(10,4) NOT NULL,
    costo_compra_g NUMERIC(10,4),
    cargo_transporte_stn_tm NUMERIC(10,4),
    cargo_transporte_sdl_dm NUMERIC(10,4),
    margen_comercializacion NUMERIC(10,4),
    costo_perdidas_pr NUMERIC(10,4),
    restricciones_rm NUMERIC(10,4),
    cot NUMERIC(10,4),
    cfmj_gfact NUMERIC(10,4),
    
    -- Metadatos
    fecha_actualizacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    CONSTRAINT uk_periodo_nivel UNIQUE (periodo, nivel)
);

CREATE INDEX idx_periodo ON tarifas_electricas(periodo);
CREATE INDEX idx_nivel ON tarifas_electricas(nivel);
```

### Tabla Educativa

```sql
CREATE TABLE componentes_educacion (
    id SERIAL PRIMARY KEY,
    codigo VARCHAR(50) UNIQUE NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    descripcion_simple TEXT NOT NULL,
    analogia TEXT NOT NULL,
    icono VARCHAR(50),
    color VARCHAR(7),
    orden INTEGER
);

-- Seed Data
INSERT INTO componentes_educacion VALUES
(1, 'CU_TOTAL', 'Costo Total', 
 'Es todo lo que pagas en tu factura por cada kWh que consumes', 
 'Como el total de la cuenta del restaurante', 
 '💰', '#2E7D32', 1),
(2, 'COSTO_COMPRA', 'Compra de Energía', 
 'Lo que cuesta producir la electricidad en las plantas', 
 'Como el costo de cocinar la comida en el restaurante', 
 '⚡', '#1976D2', 2),
(3, 'TRANSPORTE_STN', 'Transporte Nacional', 
 'Mover la energía por torres de alta tensión desde la planta', 
 'Como el camión que lleva ingredientes al restaurante', 
 '🚚', '#F57C00', 3),
(4, 'TRANSPORTE_SDL', 'Distribución Local', 
 'Llevar la energía por los cables de tu barrio hasta tu casa', 
 'Como el delivery que lleva la comida a tu casa', 
 '🏘️', '#FBC02D', 4),
(5, 'COMERCIALIZACION', 'Comercialización', 
 'El servicio de leer tu contador, enviarte la factura y atenderte', 
 'Como el servicio y atención del restaurante', 
 '👨‍💼', '#7B1FA2', 5),
(6, 'PERDIDAS', 'Pérdidas Técnicas', 
 'Energía que se pierde en cables viejos o por conexiones ilegales', 
 'Como la comida que se desperdicia en la cocina', 
 '📉', '#C62828', 6);
```

### Tabla de Logs

```sql
CREATE TABLE etl_logs (
    id SERIAL PRIMARY KEY,
    fecha_ejecucion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    estado VARCHAR(20) NOT NULL,
    registros_procesados INTEGER,
    mensaje TEXT,
    duracion_segundos NUMERIC(10,2)
);
```

---

## 🔧 Backend Minimal

### Estructura (13 archivos)

```
src/
├── TarifasEPM.Domain/
│   ├── Entities/
│   │   ├── TarifaElectrica.cs           ⭐
│   │   ├── ComponenteEducacion.cs       ⭐
│   │   └── EtlLog.cs                    ⭐
│   └── Interfaces/
│       └── ITarifaRepository.cs         ⭐
│
├── TarifasEPM.Application/
│   ├── DTOs/
│   │   ├── TarifaDTO.cs                 ⭐
│   │   ├── FacturaSimuladaDTO.cs        ⭐
│   │   └── GovCoResponseDTO.cs          ⭐
│   ├── Interfaces/
│   │   ├── ITarifaService.cs            ⭐
│   │   └── IEtlService.cs               ⭐
│   └── Services/
│       ├── TarifaService.cs             ⭐
│       └── EtlService.cs                ⭐
│
├── TarifasEPM.Infrastructure/
│   ├── Data/
│   │   └── TarifasDbContext.cs          ⭐
│   ├── Repositories/
│   │   └── TarifaRepository.cs          ⭐
│   └── ExternalServices/
│       └── GovCoApiClient.cs            ⭐ (HttpClient)
│
└── TarifasEPM.WebApi/
    ├── Controllers/
    │   ├── TarifasController.cs         ⭐
    │   ├── FacturaController.cs         ⭐
    │   └── EtlController.cs             ⭐
    ├── Program.cs                        ⭐
    └── appsettings.json                  ⭐
```

### ETL con GOV.CO

**GovCoApiClient.cs**
```csharp
public class GovCoApiClient
{
    private readonly HttpClient _httpClient;
    private const string BASE_URL = "https://www.datos.gov.co/resource/";
    private const string DATASET_ID = "xxxx-xxxx"; // ID del dataset EPM
    
    public async Task<List<GovCoTarifaDTO>> ObtenerTarifasAsync()
    {
        var url = $"{BASE_URL}{DATASET_ID}.json";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<GovCoTarifaDTO>>(content);
    }
}
```

**EtlService.cs**
```csharp
public class EtlService : IEtlService
{
    private readonly GovCoApiClient _apiClient;
    private readonly ITarifaRepository _repository;
    
    public async Task<EtlResult> EjecutarEtlAsync()
    {
        var log = new EtlLog { Estado = "Running" };
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            // 1. EXTRACT: GOV.CO
            var datosExternos = await _apiClient.ObtenerTarifasAsync();
            
            // 2. TRANSFORM: Limpiar y mapear
            var tarifas = TransformarDatos(datosExternos);
            
            // 3. LOAD: PostgreSQL
            await _repository.BulkInsertAsync(tarifas);
            
            stopwatch.Stop();
            log.Estado = "Success";
            log.RegistrosProcesados = tarifas.Count;
            log.DuracionSegundos = stopwatch.Elapsed.TotalSeconds;
        }
        catch (Exception ex)
        {
            log.Estado = "Failed";
            log.Mensaje = ex.Message;
        }
        
        await _repository.GuardarLogAsync(log);
        return new EtlResult { Success = log.Estado == "Success" };
    }
}
```

### Endpoints Minimal

```csharp
// TarifasController
[HttpGet("actual")]
GetTarifaActual() // Tarifa del mes actual

[HttpGet("componentes")]
GetComponentesEducativos() // Explicaciones

// FacturaController
[HttpPost("simular")]
SimularFactura([FromBody] int consumoKwh) // Calcular factura

// EtlController
[HttpPost("ejecutar")]
EjecutarEtl() // Disparar ETL

[HttpGet("logs")]
GetEtlLogs() // Histórico

[HttpGet("status")]
GetStatus() // Estado último ETL
```

---

## 🎨 Frontend Minimal

### Estructura (10 componentes)

```
src/app/
├── core/
│   ├── services/
│   │   ├── tarifa.service.ts        ⭐
│   │   └── etl.service.ts           ⭐
│   └── models/
│       └── tarifa.model.ts          ⭐
│
├── shared/
│   └── components/
│       ├── navbar/                  ⭐
│       └── footer/                  ⭐
│
└── features/
    ├── factura/
    │   ├── pages/
    │   │   └── factura-page/        ⭐ (componente único)
    │   └── factura.routes.ts        ⭐
    │
    ├── educacion/
    │   ├── pages/
    │   │   └── educacion-page/      ⭐ (componente único)
    │   └── educacion.routes.ts      ⭐
    │
    └── admin/
        ├── pages/
        │   └── admin-page/          ⭐ (componente único)
        └── admin.routes.ts          ⭐
```

### Features

**Factura Page**
- Input: consumo kWh
- Botón: "Calcular"
- Card: desglose visual
- Tooltips educativos

**Educación Page**
- Grid de cards
- Cada componente: icono + nombre + descripción + analogía
- Sección "Analogía del Delivery"

**Admin Page**
- Botón: "Ejecutar ETL"
- Loading spinner
- Tabla: logs de ejecuciones
- Badges: Success/Failed/Running

---

## 📋 Checklist Desarrollo MINIMAL

### Día 1: Backend + ETL
- [ ] Crear proyecto .NET 10
- [ ] Configurar PostgreSQL
- [ ] Crear 3 tablas
- [ ] Entities + DbContext
- [ ] Repository
- [ ] **GovCoApiClient con HttpClient**
- [ ] **EtlService completo**
- [ ] 3 Controllers
- [ ] Seed componentes_educacion
- [ ] **Probar ETL en Swagger**

### Día 2: Frontend Base
- [ ] Crear proyecto Angular 19
- [ ] TailwindCSS
- [ ] tarifa.service.ts
- [ ] **etl.service.ts**
- [ ] Modelos
- [ ] Navbar + Footer
- [ ] Routing (3 rutas)
- [ ] Conectar con backend

### Día 3: Features
- [ ] Factura Page (UI + lógica)
- [ ] Educación Page (grid cards)
- [ ] **Admin Page (ETL + logs)**
- [ ] Testing completo
- [ ] README

---

## 🎨 Paleta de Colores

```css
:root {
  --primary: #1976D2;      /* Azul EPM */
  --secondary: #F57C00;    /* Naranja energía */
  --success: #2E7D32;      /* Verde */
  --danger: #C62828;       /* Rojo */
  --warning: #FBC02D;      /* Amarillo */
  --bg: #F5F5F5;           /* Gris claro */
  --text: #212121;         /* Negro */
}
```

---

## 🚀 Stack Tecnológico

| Componente | Tecnología |
|------------|------------|
| Backend | .NET 10 Web API |
| Frontend | Angular 19 Standalone |
| DB | PostgreSQL 15+ |
| ORM | EF Core 9 |
| HTTP Client Backend | HttpClient (.NET) |
| HTTP Client Frontend | HttpClient (Angular) |
| Styling | TailwindCSS |

---

## 🏁 Flujo de Uso

### Primera vez:
1. Admin → `/admin`
2. Click "Ejecutar ETL"
3. Sistema descarga de GOV.CO
4. Guarda en PostgreSQL
5. Muestra log: Success

### Usuario normal:
1. Usuario → `/factura`
2. Ingresa: "150 kWh"
3. Ve desglose completo
4. Tooltips educativos
5. Va a `/educacion` para aprender

### Mensual:
1. Admin ejecuta ETL
2. Datos actualizados
3. Usuarios ven nuevo mes

---

## 🔑 Diferencias MINIMAL vs FULL

| Feature | MINIMAL | FULL |
|---------|---------|------|
| Tablas DB | 3 | 8+ |
| Archivos Backend | 13 | 30+ |
| Componentes Frontend | 10 | 25+ |
| Endpoints | 6 | 15+ |
| ETL | Básico funcional | Complejo |
| Gráficos | No | Sí |
| Comparaciones | No | Sí |
| Auth | No | Sí |
| Tests | Básicos | Exhaustivos |

---

## ✅ Entregables MINIMAL

1. ✅ Backend Clean Architecture (4 capas)
2. ✅ **ETL conectado a GOV.CO**
3. ✅ PostgreSQL (3 tablas)
4. ✅ Frontend modular (3 features)
5. ✅ Factura simulada funcional
6. ✅ Educación con analogías
7. ✅ **Admin con ETL + logs**
8. ✅ README completo

---

**Versión**: 2.0 - MINIMAL con GOV.CO  
**Fecha**: Febrero 2025  
**Tiempo**: 2-3 días
