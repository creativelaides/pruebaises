# Guia de Presentacion Tecnica (20-25 min) - Prueba ISES

## Objetivo de la presentacion
Explicar de forma clara que resolviste el problema de negocio de tarifas electricas con una solucion full-stack, mostrando criterio tecnico de nivel Junior con buena estructura, decisiones razonables y dominio del flujo end-to-end.

## Mensaje principal que debes repetir
"La solucion permite consumir datos oficiales, transformarlos, almacenarlos, consultarlos y visualizarlos de forma segura y mantenible."

## Agenda sugerida (tiempos reales)
1. `00:00 - 02:00` Contexto y objetivo de negocio.
2. `02:00 - 06:00` Arquitectura general y decisiones tecnicas.
3. `06:00 - 10:00` Backend: capas, endpoints y seguridad.
4. `10:00 - 13:00` ETL y persistencia local en Docker.
5. `13:00 - 17:00` Frontend y flujo de usuario.
6. `17:00 - 20:00` Demo guiada.
7. `20:00 - 23:00` Testing, calidad y manejo de errores.
8. `23:00 - 25:00` Cierre, limites actuales y siguientes pasos.

## Guion detallado por bloque

## 1) Contexto y objetivo (`00:00 - 02:00`)
- Problema: tarifas electricas son datos tecnicos y dispersos; se requiere visualizacion y consulta clara.
- Fuente oficial: `datos.gov.co` (Socrata).
- Valor: centralizar consulta, simular factura y facilitar monitoreo.

Frase de cierre del bloque:
"El foco fue construir una base tecnica ordenada para crecer funcionalmente sin romper el sistema."

## 2) Arquitectura y decisiones (`02:00 - 06:00`)
- Muestra `Skills/Diagrams/diagrama-arquitectura.mermaid`.
- Explica separacion en capas (Domain, Application, Infrastructure, API, Frontend).
- Aclara punto clave: PostgreSQL no es "externo"; corre local en Docker (`Database/docker-compose.yml`), mientras que externo es Socrata y SMTP.
- Justifica decisiones:
- Clean Architecture para mantenibilidad.
- CQRS para separar lectura/escritura.
- Identity para autenticacion/autorizacion.

## 3) Backend (`06:00 - 10:00`)
- Ubicacion: `Backend/src`.
- Muestra rapidamente:
- `Domain`: entidades y reglas.
- `Application`: casos de uso y validaciones.
- `API`: controllers y contratos HTTP.
- Endpoints que debes mencionar:
- `api/tariffs` (CRUD + consultas).
- `api/invoices/simulate`.
- `api/etl/run`.
- `api/account/*`.
- Seguridad:
- Roles `Admin` y `Client`.
- Políticas y autorización en controladores.

## 4) ETL + Base de datos (`10:00 - 13:00`)
- Muestra `Skills/Diagrams/diagrama-secuencia-etl.mermaid`.
- Explica pipeline:
- Extract: Socrata API.
- Transform: mapeo y validaciones.
- Load: upsert en PostgreSQL.
- Muestra que hay trazabilidad con logs de ETL.
- Aclara infraestructura local:
- PostgreSQL + pgAdmin levantados por Docker en `Database/`.

## 5) Frontend (`13:00 - 17:00`)
- Ubicacion: `Frontend/src/app`.
- Explica features por modulo:
- `auth-feature`
- `tariffs-feature`
- `invoices-feature`
- `etl-feature`
- `admin-feature`
- Flujo funcional de usuario:
- Login.
- Consulta de tarifas.
- Simulacion de factura.
- (Admin) ejecucion ETL y prueba de correo.

## 6) Demo guiada (`17:00 - 20:00`)
- Orden recomendado para no fallar:
1. Levantar DB Docker.
2. Levantar API.
3. Levantar Frontend.
4. Login.
5. Consultar `latest` o listado de tarifas.
6. Ejecutar simulacion de factura.
7. (Opcional) correr ETL como Admin.
- Si el tiempo esta corto, prioriza:
1. Login.
2. Consulta tarifas.
3. Simulacion.

## 7) Calidad tecnica (`20:00 - 23:00`)
- Pruebas backend con xUnit + NSubstitute.
- Validaciones en Application (FluentValidation).
- Manejo de errores centralizado (middleware + excepciones de caso de uso).
- Configuracion desacoplada por `appsettings` y variables.

## 8) Cierre y roadmap (`23:00 - 25:00`)
- Lo entregado:
- Flujo full-stack funcional.
- Arquitectura limpia y escalable.
- Seguridad y ETL integrados.
- Mejoras que propones (mostrar criterio):
- Observabilidad (métricas/telemetría).
- Paginación/filtros avanzados en frontend.
- Jobs programados para ETL.
- Hardening de seguridad para producción.

Frase final sugerida:
"La solución cumple el objetivo funcional y deja una base sólida para evolucionar el producto con bajo riesgo técnico."

## Preguntas típicas y respuestas cortas
- "Por que Clean Architecture en una prueba junior?"
  Respuesta: para demostrar orden, separacion de responsabilidades y facilidad de pruebas.
- "Por que CQRS?"
  Respuesta: porque las consultas y comandos tienen reglas distintas y crecen de forma independiente.
- "Que pasa si falla Socrata?"
  Respuesta: ETL registra estado/error en log y no bloquea el resto de la API.
- "Que tan productivo es el sistema hoy?"
  Respuesta: funcional para entorno local y demo; faltan ajustes de hardening para produccion.

## Checklist previo (hoy en la noche)
- Base de datos arriba (`docker compose up -d`).
- API arriba sin errores.
- Frontend arriba.
- Credenciales de prueba listas.
- Un flujo de demo ensayado 1 vez con cronometro.
- Abrir de antemano:
- `README.md`
- `Summaries/APPLICATION_STRUCTURE_SUMMARY.md`
- `Summaries/DOMAIN_STRUCTURE_SUMMARY.md`
- Diagramas en `Skills/Diagrams/`.

## Plan B si algo falla en vivo
- Si cae frontend: demostrar endpoints desde OpenAPI/Scalar.
- Si falla ETL en vivo: mostrar diagrama + logs + endpoints de lectura.
- Si falla correo: explicar restriccion de entorno y mostrar configuracion SMTP.

## Regla para sonar senior sin exagerar
- Habla con datos concretos de tu repo.
- No prometas features no implementadas.
- Cuando algo no esta listo, dilo y propone siguiente paso tecnico.
