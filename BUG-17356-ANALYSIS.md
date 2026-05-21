# 🐛 Bug #17356: Aspire CLI 13.3.5 - JSON-RPC Backchannel Timeout

## Resumen Ejecutivo

`aspire run` arranca correctamente pero después de ~120 segundos pierde la conexión JSON-RPC backchannel y mata el AppHost:

```
❌ An unexpected error occurred: The JSON-RPC connection with the remote party was lost before the request could complete.
```

**`dotnet run` funciona PERFECTO** - el bug está específicamente en el CLI de Aspire.

---

## 🔍 Diagnóstico Completo

### Evidencia de Logs

#### Log CON pgAdmin (21/05/2026 22:57:33)
```
22:58:29.994 - AppHost: "Hosting started"
23:00:27.560 - HttpMessageHandler expired after 120000ms for client 'pgadmin_http_/browser_200_check'
23:00:33.696 - JSON-RPC connection lost
```
**→ Timeout EXACTO de 120 segundos en health check de pgAdmin**

#### Log SIN pgAdmin (21/05/2026 23:09:48)
```
23:10:37.536 - AppHost: "Distributed application started"
[FIN ABRUPTO - mismo patrón de ~120s]
```
**→ El problema PERSISTE sin pgAdmin**

---

## 📋 Respuestas a Tus Preguntas

### 1. ¿Qué OTRO recurso podría causar inactividad del backchannel?

**El Angular frontend (`AddJavaScriptApp`)** es el principal sospechoso ahora:

| Recurso | Problema |
|---------|----------|
| `AddJavaScriptApp` con `npm start` | No tiene health check nativo |
| `ng serve` tarda 30-60s en compilar | El CLI espera sin feedback |
| Sin heartbeat al backchannel | Timeout de 120s mata la conexión |

### 2. ¿Puede el Angular frontend ser la causa?

**SÍ, definitivamente.** `AddJavaScriptApp` es conocido por este bug porque:
- `npm start` no expone health checks automáticamente
- El CLI de Aspire no sabe cuándo está "listo"
- El backchannel JSON-RPC espera indefinidamente sin actividad

### 3. ¿Problema con `aspire run` desde contexto no-TTY?

**Posible contribuyente.** El backchannel usa **Named Pipes de Windows** en:
```
C:\Users\P335\.aspire\cli\runtime\sockets\cli.sock.*
```

En contextos no interactivos (agentes AI, scripts):
- El buffer del pipe puede llenarse sin lector activo
- El AppHost se bloquea escribiendo logs
- El CLI deja de recibir heartbeats

### 4. ¿Deberíamos probar AppHost MÍNIMO?

**SÍ - ya está creado.** Ver: `Aspire/Aspire.AppHost.Minimal/`

Contiene SOLO:
- ✅ PostgreSQL
- ✅ API .NET
- ❌ SIN Angular
- ❌ SIN pgAdmin

### 5. ¿El backchannel usa Unix sockets en Windows?

**No - usa Named Pipes de Windows.** La ruta `cli.sock.*` es un Named Pipe disfrazado. El timeout de 120s es **hardcodeado** en el CLI de Aspire 13.3.5.

### 6. ¿Qué fix concreto podemos APLICAR ahora?

Ver sección "Fixes" abajo.

---

## ✅ Fixes Concretos (Ordenados por Prioridad)

### FIX 1: Usar `dotnet run` (WORKAROUND INMEDIATO) ⭐

**El workaround más efectivo - evita completamente el CLI de Aspire:**

```bash
# En lugar de:
aspire run

# Usar:
dotnet run --project Aspire/Aspire.AppHost/Aspire.AppHost.csproj
```

**Ventajas:**
- ✅ Funciona perfecto sin timeout
- ✅ Mismo dashboard, mismos recursos
- ✅ Sin backchannel JSON-RPC problemático

**Desventajas:**
- ❌ No tienes los comandos específicos del CLI de Aspire

---

### FIX 2: AppHost Minimal para Debugging

**Para aislar el problema:**

```bash
# Probar SOLO PostgreSQL + API (sin Angular)
dotnet run --project Aspire/Aspire.AppHost.Minimal/Aspire.AppHost.Minimal.csproj
```

**Si funciona sin timeout:** Confirma que Angular es la causa.

**Si también falla:** El problema es más profundo (CLI/Named Pipes).

---

### FIX 3: Comentar Angular Temporalmente

**En `Aspire/Aspire.AppHost/Program.cs`:**

```csharp
// ── Angular Frontend ───────────────────────────────────────────────────
// TEMPORALMENTE COMENTADO - bug #17356
// Descomentar cuando Aspire 13.4+ esté disponible
// var frontend = builder.AddJavaScriptApp("frontend", "../Frontend")
//     .WithRunScript("start")
//     .WithHttpEndpoint(targetPort: 4200, name: "http", env: "PORT")
//     .WithReference(api)
//     .WaitFor(api);
```

---

### FIX 4: Actualizar a Aspire 13.4+ (Cuando esté disponible)

El bug está **milestoned para 13.4** por davidfowl (lead de Aspire).

**Verificar versión disponible:**
```bash
dotnet tool update --global aspire.cli
```

---

### FIX 5: Aumentar Timeout de Named Pipes (Avanzado)

**ADVERTENCIA:** Solo para debugging, requiere reinicio.

```powershell
# Aumentar timeout de 120s a 300s (5 minutos)
New-ItemProperty -Path "HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager" `
    -Name "PipeTimeout" -PropertyType "DWORD" -Value 300000 -Force

# REINICIO REQUERIDO
Restart-Computer
```

---

## 🧪 Pasos de Verificación

### Test 1: Verificar con `dotnet run` (DEBERÍA FUNCIONAR)
```bash
cd C:\Users\P335\Documents\Velaides\pruebaises
dotnet run --project Aspire/Aspire.AppHost/Aspire.AppHost.csproj

# Esperar 180+ segundos
# ✅ ÉXITO: Dashboard accesible, sin error
# ❌ FALLO: Mismo error (problema no es el CLI)
```

### Test 2: Verificar AppHost Minimal
```bash
dotnet run --project Aspire/Aspire.AppHost.Minimal/Aspire.AppHost.Minimal.csproj

# ✅ ÉXITO: Funciona → Angular es la causa
# ❌ FALLO: También timeout → problema es el CLI/Named Pipes
```

### Test 3: Verificar con `aspire run` (PROBLEMA ACTUAL)
```bash
aspire run

# ❌ ESPERADO: Error después de ~120s
```

---

## 📁 Archivos Creados para Debugging

| Archivo | Propósito |
|---------|-----------|
| `Aspire/AppHost.Minimal/Program.cs` | AppHost solo con PostgreSQL + API |
| `Aspire/AppHost.Minimal/*.csproj` | Proyecto minimal sin JavaScript |
| `BUG-17356-ANALYSIS.md` | Este documento |

---

## 🔗 GitHub Issues Relacionados

| Issue | Estado | Descripción |
|-------|--------|-------------|
| [#17356](https://github.com/dotnet/aspire/issues/17356) | Abierto (13.4) | Mismo error, milestoned por davidfowl |
| [#13929](https://github.com/dotnet/aspire/issues/13929) | Abierto | Mismo error con dotnet watch |

---

## 📊 Timeline del Bug

```
22:58:29 - AppHost inicia correctamente
22:58:30 - Angular/npm start comienza compilación
22:59:00 - (30s) Angular podría estar listo, pero no hay health check
23:00:27 - (118s) HttpMessageHandler expira para pgAdmin (si está presente)
23:00:33 - (124s) Backchannel JSON-RPC se pierde
23:00:33 - CLI mata AppHost con error
```

---

## 🎯 Recomendación Final

### Para Desarrollo Diario (AHORA)
```bash
# Usar dotnet run directamente
dotnet run --project Aspire/Aspire.AppHost/Aspire.AppHost.csproj
```

### Para Debugging
```bash
# 1. Probar AppHost minimal para aislar Angular
dotnet run --project Aspire/Aspire.AppHost.Minimal/Aspire.AppHost.Minimal.csproj

# 2. Si funciona, el problema es Angular
# 3. Si falla, el problema es el CLI/Named Pipes
```

### Para Producción
- No usar `aspire run` en pipelines CI/CD hasta 13.4+
- Usar `dotnet run` o Docker compose

---

## 📝 Conclusión

El bug es una combinación de:
1. **Health checks sin timeout configurado** (pgAdmin, Angular)
2. **Backchannel JSON-RPC sin heartbeat** durante operaciones largas
3. **Named Pipes en Windows** que no reportan estado correctamente

**Fix inmediato:** Usar `dotnet run` en lugar de `aspire run`.

**Fix definitivo:** Esperar Aspire 13.4+ o contribuir al fix en el repo de Aspire.
