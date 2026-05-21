# 🧪 Scripts de Prueba para Bug #17356

## Script 1: Probar con `dotnet run` (WORKAROUND RECOMENDADO)

```powershell
# Test 1: dotnet run - DEBERÍA FUNCIONAR PERFECTO
Write-Host "=== TEST 1: dotnet run (sin CLI de Aspire) ===" -ForegroundColor Cyan
dotnet run --project Aspire/Aspire.AppHost/Aspire.AppHost.csproj

# Esperar 180 segundos manualmente
# Presionar Ctrl+C cuando quieras detener
# ✅ ÉXITO: Dashboard accesible en https://localhost:17113
# ❌ FALLO: Error antes de 180s
```

## Script 2: Probar AppHost Minimal (SOLO PostgreSQL + API)

```powershell
# Test 2: AppHost Minimal - aislar Angular
Write-Host "=== TEST 2: AppHost Minimal (sin Angular, sin pgAdmin) ===" -ForegroundColor Cyan
dotnet run --project Aspire/Aspire.AppHost.Minimal/Aspire.AppHost.Minimal.csproj

# Esperar 180 segundos manualmente
# ✅ ÉXITO: Funciona → Angular es la causa del bug
# ❌ FALLO: También timeout → problema es el CLI/Named Pipes
```

## Script 3: Probar con `aspire run` (REPRODUCE EL BUG)

```powershell
# Test 3: aspire run - REPRODUCE EL BUG
Write-Host "=== TEST 3: aspire run (reproduce bug #17356) ===" -ForegroundColor Yellow
aspire run

# ❌ ESPERADO: Error después de ~120s
# "The JSON-RPC connection with the remote party was lost"
```

## Script 4: Verificar Logs Más Recientes

```powershell
# Ver último log del CLI
$latestLog = Get-ChildItem -Path "$env:USERPROFILE\.aspire\logs" -Filter "cli_*.log" | 
    Sort-Object LastWriteTime -Descending | 
    Select-Object -First 1

Write-Host "=== Último log: $($latestLog.Name) ===" -ForegroundColor Cyan
Get-Content $latestLog.FullName -Tail 50

# Buscar errores específicos
Write-Host "`n=== Errores encontrados ===" -ForegroundColor Red
Get-Content $latestLog.FullName | Select-String -Pattern "error|Error|lost|timeout|exception" -Context 2
```

## Script 5: Matar Procesos de Aspire Atascados

```powershell
# Limpiar procesos de Aspire atascados
Write-Host "=== Matando procesos de Aspire... ===" -ForegroundColor Yellow
Get-Process | Where-Object { 
    $_.ProcessName -like "*aspire*" -or 
    $_.ProcessName -like "*Aspire*" 
} | Stop-Process -Force

Get-Process | Where-Object { 
    $_.ProcessName -eq "dotnet" -and 
    $_.MainWindowTitle -like "*Aspire*" 
} | Stop-Process -Force

Write-Host "Procesos limpiados." -ForegroundColor Green
```

## Ejecutar Todos los Tests en Secuencia

```powershell
# Script completo de debugging
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Debugging Bug #17356 - Aspire CLI   " -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# 1. Limpiar procesos
& "$PSScriptRoot\test-aspire-bug.ps1" -Clean

# 2. Test con dotnet run (debería funcionar)
Write-Host "`n[TEST 1] dotnet run - DEBERÍA FUNCIONAR" -ForegroundColor Green
Start-Sleep -Seconds 2
# Ejecutar manualmente y observar

# 3. Test con AppHost Minimal
Write-Host "`n[TEST 2] AppHost Minimal - AISLAR ANGULAR" -ForegroundColor Green
Start-Sleep -Seconds 2
# Ejecutar manualmente y observar

# 4. Ver logs
Write-Host "`n[TEST 3] Ver logs recientes" -ForegroundColor Green
Get-ChildItem -Path "$env:USERPROFILE\.aspire\logs" -Filter "cli_*.log" | 
    Sort-Object LastWriteTime -Descending | 
    Select-Object -First 3 | 
    ForEach-Object {
        Write-Host "  $($_.Name) - $($_.Length) bytes - $($_.LastWriteTime)"
    }
```

---

## Comandos Rápidos

```powershell
# Workaround diario - usar dotnet run
cd C:\Users\P335\Documents\Velaides\pruebaises
dotnet run --project Aspire/Aspire.AppHost/Aspire.AppHost.csproj

# Ver logs en tiempo real
Get-Content "$env:USERPROFILE\.aspire\logs\cli_*.log" -Tail 10 -Wait

# Ver dashboard de Aspire
Start-Process "https://localhost:17113"
```

---

## Criterios de Éxito

| Test | Resultado Esperado | Indica |
|------|-------------------|--------|
| `dotnet run` | ✅ Funciona 180s+ | Bug es del CLI de Aspire |
| `aspire run` | ❌ Error ~120s | Confirma bug #17356 |
| AppHost Minimal + `dotnet run` | ✅ Funciona | Angular es causa secundaria |
| AppHost Minimal + `aspire run` | ❌ Error ~120s | CLI es causa principal |

---

## Próximos Pasos

1. **Usar `dotnet run`** para desarrollo diario (workaround efectivo)
2. **Esperar Aspire 13.4+** para fix oficial
3. **Monitorear issue #17356** en GitHub
4. **Considerar contribuir fix** al repo de Aspire si es crítico
