# Issues Propuestos

## Issue 1 — Implementar capa de autorización con Identity

**Objetivo**
Implementar la capa de autorización basada en Identity, alineada con la arquitectura actual (Clean + CQRS).

**Alcance**
- Crear proyecto/capa de autorización (o módulo dentro de Infrastructure si aplica).
- Configurar Identity y sus stores.
- Definir modelo de usuario, roles y claims básicos.
- Configuración en DI y appsettings.
- Endpoints mínimos (registro/login/roles) si aplica al diseño.
- Integración con políticas de autorización en controllers.

**Criterios de aceptación**
- Identity configurado y funcional.
- Se puede autenticar un usuario y autorizar por rol/política.
- Documentación básica de configuración (README o notas).
- Tests básicos o verificación manual documentada.

**Notas**
- Se hará junto con el equipo (no implementar todavía).

---

## Issue 2 — Implementar servicio de mensajería de correo

**Objetivo**
Agregar un servicio de envío de correo desacoplado del dominio, integrable con futuros casos de uso.

**Alcance**
- Definir interfaz en Application (IEmailSender u equivalente).
- Implementación en Infrastructure (SMTP o proveedor externo).
- Configuración por appsettings (host, puerto, usuario, etc).
- Registro en DI.
- Manejo de errores y reintentos simples si aplica.

**Criterios de aceptación**
- Servicio de email configurado y usable.
- Ejemplo de envío (endpoint o caso de uso de prueba).
- Documentación mínima de configuración.
- Tests básicos o verificación manual documentada.

**Notas**
- Se hará más adelante, no bloquear trabajo actual.


---
---

# Comandos para crear PRs desde la terminal (gh)

## PR 1 — Authorization (Issue #1)
```bash
git checkout -b feature/authorization-layer
git push -u origin feature/authorization-layer

gh pr create --base main --head feature/authorization-layer `
  --title "Authorization layer (Identity)" `
  --body "Resolves #1"
```

## PR 2 — Email (Issue #2)
```bash
git checkout -b feature/email-messaging
git push -u origin feature/email-messaging

gh pr create --base main --head feature/email-messaging `
  --title "Email messaging service" `
  --body "Resolves #2"
```

**Nota**
Si tu rama base no es `main`, reemplázala por `master` o `dev` según corresponda.
