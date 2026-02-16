# Prueba Técnica Desarrollador Junior FULLSTACK

## 1. Descripción de la prueba

Se requiere una solución tecnológica que permita visualizar o monitorear las tarifas de energía eléctrica de las comercializadoras en Colombia, consumiendo datos oficiales y presentándolos de forma organizada y sobre todo legible para cualquier usuario.

## 2. Requisitos técnicos

- **Lenguaje de programación**: Node.js, C#, Python, etc.
- **Base de datos**: A tu selección (MySQL, PostgreSQL, SQLite, MongoDB)
- **Arquitectura**: Separación de Backend y Frontend
- **Fuente de datos**: [datos.gov.co](https://www.datos.gov.co/Minas-y-Energ-a/Tarifas-y-Costos-de-Energ-a-El-ctrica-para-el-Merc/ytme-6qnu/about_data)
  - Es importante buscar un dataset de "Tarifas y Costos de Energía para el Mercado Regulado"

## 3. Componentes de la solución

### Proceso ETL (Backend)

- **Extracción**: Conectarse al API de datos.gov.co o procesar el archivo fuente de tarifas
- **Transformación**: Filtrar y normalizar la información extraída de la fuente de los datos
- **Cargue**: Guardar la información en una base de datos SQL o NoSQL. Al finalizar el proceso enviar un correo electrónico con la información que se cargó en información básica

### API REST

Crear los endpoints necesarios para:
- Mostrar la información
- Buscar por diferentes tipos
- Generar uno que permita disparar el proceso ETL

### Frontend

Desarrollar una interfaz sencilla (React, Angular, Vue, HTML/CSS/JS) que permita:
- Visualizar la información
- Buscar información
- Ver la última actualización

## 4. Entregables

- **Código fuente**: Cargar la solución en GitHub, GitLab, Bitbucket o cualquiera de preferencia
- **README.md**: Documentación del proyecto
- **Presentación**: Disponer de 20 minutos para presentar la prueba y explicar cada una de las partes que conlleva
