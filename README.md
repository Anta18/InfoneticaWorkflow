# Workflow Engine API

An ASP .NET Core Minimal-API that lets you **define** state-machine workflows in memory (with optional JSON persistence) and **run** instances through them.

---

## Table of Contents

1. [Core Concepts & Domain Model](#core-concepts--domain-model)
2. [API Endpoints](#api-endpoints)
3. [Validation Rules](#validation-rules)
4. [Persistence](#persistence)
5. [Design & Readability](#design--readability)
6. [Maintainability & Pragmatism](#maintainability--pragmatism)
7. [Running & Testing](#running--testing)

---

## Core Concepts & Domain Model

| Concept           | Class                                  | Key Members                                                                                |
| ----------------- | -------------------------------------- | ------------------------------------------------------------------------------------------ |
| **State**         | `Domain.Entities.State`                | `Id`, `Name`, `IsStart`, `IsEnd`, `Enabled`                                                |
| **Action**        | `Domain.Entities.Action`               | `Id`, `Name`, `FromStateId`, `ToStateId`, `Enabled`                                        |
| **Definition**    | `Domain.Entities.WorkflowDefinition`   | `Id`, `Name`, `States: List<State>`, `Actions: List<Action>`                               |
| **Instance**      | `Domain.Entities.WorkflowInstance`     | `Id`, `DefinitionId`, `CurrentStateId`, `CreatedAt`, `History: List<InstanceHistoryEntry>` |
| **History Entry** | `Domain.Entities.InstanceHistoryEntry` | `Id`, `InstanceId`, `ActionId`, `PerformedAt`                                              |

---

## API Endpoints

> All endpoints are defined in `Api/Extensions/ApplicationBuilderExtensions.cs`.

| Area            | Action                             | HTTP                                          | Code Location                                            |
| --------------- | ---------------------------------- | --------------------------------------------- | -------------------------------------------------------- |
| **Definitions** | Create definition                  | `POST /definitions`                           | MapWorkflowEndpoints → `CreateDefinitionRequest` handler |
|                 | List all definitions               | `GET /definitions`                            | MapWorkflowEndpoints                                     |
|                 | Get one definition                 | `GET /definitions/{defId}`                    | MapWorkflowEndpoints                                     |
| **Instances**   | Start new instance                 | `POST /definitions/{defId}/instances`         | MapWorkflowEndpoints                                     |
|                 | List all instances                 | `GET /instances`                              | MapWorkflowEndpoints                                     |
|                 | Get one instance (state & history) | `GET /instances/{instId}`                     | MapWorkflowEndpoints                                     |
|                 | Execute action on instance         | `POST /instances/{instId}/actions/{actionId}` | MapWorkflowEndpoints                                     |

---

## Validation Rules

All of these checks live in your **domain entity** methods:

1. **Definition creation** (`WorkflowDefinition.AddState` & `AddAction`):

   - No duplicate state IDs
   - Exactly one `IsStart == true`
   - Both `FromStateId` and `ToStateId` must exist in `States`
     _(see Domain/Entities/WorkflowDefinition.cs)_

2. **Instance construction** (`new WorkflowInstance(def)`):

   - Must have exactly one start state; sets `CurrentStateId` to that

3. **Action execution** (`WorkflowInstance.ExecuteAction`):

   - Action must be `Enabled`
   - Current state must be found and `Enabled`
   - `action.FromStateId` must equal `CurrentStateId`
   - Throws clear `InvalidOperationException` messages on failure
     _(see Domain/Entities/WorkflowInstance.cs)_

4. **Service-level guards**:

   - Missing definition or instance → `KeyNotFoundException`
   - Action not in definition → `InvalidOperationException`
     _(see Application/Services/WorkflowService.cs)_

---

## Persistence

- **In‐memory store**:
  `Infrastructure.Storage.InMemoryStore<T>` uses a `ConcurrentDictionary<Guid,T>`.
- **Optional JSON snapshot**:
  On every `AddAsync` / `UpdateAsync` it `File.WriteAllText(...)` to `definitions.json` and `instances.json`.
- **No database**:
  All EF Core references / `WorkflowDbContext` have been removed.

---

## Design & Readability

- **Clear layers**:

  - **Domain**: Entities & business rules only
  - **Application**: DTOs, AutoMapper profiles, Service orchestration
  - **Infrastructure**: Repositories + InMemoryStore
  - **API**: Minimal endpoints, DI setup, exception handler

- **Modular**:

  - `ServiceCollectionExtensions` wires up repos & services
  - `ApplicationBuilderExtensions` wires endpoints

- **Strong typing**:

  - GUIDs everywhere, record types for DTOs, no magic strings

---

## Maintainability & Pragmatism

- **Light abstraction**: In-memory store is a single reusable class, but not over-engineered.
- **Easily swappable**: If you decide to add a real database, simply replace `InMemoryStore` + repos with `DbContext` + EF.
- **Extension methods** keep `Program.cs` extremely concise.

---

## Running & Testing

1. **Restore & build**:

   ```bash
   dotnet clean && dotnet build
   ```

2. **Run the API**:

   ```bash
   cd Api
   dotnet run
   ```

3. **Swagger UI**:
   Browse to `https://localhost:5001/swagger` (or `http://localhost:5000/swagger`).

4. **Postman / curl**:
   Follow the Postman collection and automated tests you already configured:

   - `POST /definitions` → returns `{ id: ... }`
   - `GET /definitions` → returns your definition
   - `POST /definitions/{id}/instances` → returns `{ instId: ... }`
   - `GET /instances/{instId}` → shows `currentStateId`, `history`
