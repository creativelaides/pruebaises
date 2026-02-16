# AGENT.md - Project Context

## Directory Overview

This directory contains the complete planning and documentation for a technical test submission. The project is a full-stack web application designed to monitor and visualize electricity tariffs in Colombia, using public data from `datos.gov.co`.

The documentation outlines two versions of the project:
1.  **Minimal Version**: A core, functional application achievable in 2-3 days.
2.  **Full Version**: A complete, feature-rich system with a more complex architecture.

The proposed technology stack is:
*   **Backend**: .NET with Clean Architecture
*   **Frontend**: Angular (Modular Architecture)
*   **Database**: PostgreSQL

## Key Files

*   `PRUEBA_TECNICA_FULLSTACK.md`: The original problem statement for the technical test. It describes the requirements for the ETL process, a REST API, and a frontend interface.

*   `Diagrams/`: This folder contains Mermaid diagrams that visually represent the system.
    *   `diagrama-arquitectura.mermaid`: High-level architecture of the Frontend, Backend, and external services.
    *   `diagrama-clases.mermaid`: Detailed class diagram for the .NET backend.
    *   `diagrama-flujo.mermaid`: A flowchart illustrating user interaction and the ETL process flow.
    *   `diagrama-secuencia-etl.mermaid`: A sequence diagram detailing the step-by-step ETL process from the admin's action to data being stored in the database.

*   `Visuals/`: Contains images related to the project, such as a visual representation of the ETL process and screenshots of the data source website.

## Usage

This directory serves as the blueprint for the software development project. The contained documents should be used to understand:
*   The project's goals and requirements.
*   The planned technical implementation.
*   The system's architecture and data flow.

