# Workflow Engine API

An ASP .NET Core Minimal-API that lets you **define** state-machine workflows in memory (persisted to JSON) and **run** instances through them. Supports multi-state transitions and runtime enable/disable of states and actions.

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
| **State**         | `Domain.Entities.State`                | `Id`, `Name`, `IsStart`, `IsEnd`, `Enabled`, `Enable()`, `Disable()`                       |
| **Action**        | `Domain.Entities.Action`               | `Id`, `Name`, `FromStateIds` (multiple), `ToStateId`, `Enabled`, `Enable()`, `Disable()`   |
| **Definition**    | `Domain.Entities.WorkflowDefinition`   | `Id`, `Name`, `States: List<State>`, `Actions: List<Action>`                               |
| **Instance**      | `Domain.Entities.WorkflowInstance`     | `Id`, `DefinitionId`, `CurrentStateId`, `CreatedAt`, `History: List<InstanceHistoryEntry>` |
| **History Entry** | `Domain.Entities.InstanceHistoryEntry` | `Id`, `InstanceId`, `ActionId`, `PerformedAt`                                              |

---

## API Endpoints

All endpoints are wired up in `Api/Extensions/ApplicationBuilderExtensions.cs`.

### Definitions

| Method | Route                                             | Description                       |
| ------ | ------------------------------------------------- | --------------------------------- |
| POST   | `/definitions`                                    | Create a new workflow definition  |
| GET    | `/definitions`                                    | List all definitions              |
| GET    | `/definitions/{defId}`                            | Get a specific definition         |
| PATCH  | `/definitions/{defId}/states/{stateId}/disable`   | Disable a state in a definition   |
| PATCH  | `/definitions/{defId}/states/{stateId}/enable`    | Enable a state in a definition    |
| PATCH  | `/definitions/{defId}/actions/{actionId}/disable` | Disable an action in a definition |
| PATCH  | `/definitions/{defId}/actions/{actionId}/enable`  | Enable an action in a definition  |

### Instances

| Method | Route                                    | Description                           |
| ------ | ---------------------------------------- | ------------------------------------- |
| POST   | `/definitions/{defId}/instances`         | Start a new instance for a definition |
| GET    | `/instances`                             | List all instances                    |
| GET    | `/instances/{instId}`                    | Get instance state & history          |
| POST   | `/instances/{instId}/actions/{actionId}` | Execute an action on an instance      |

---

## Validation Rules

Enforced in the **domain entities**:

1. **Definition creation** (`WorkflowDefinition.AddState` & `AddAction`):

   - No duplicate state IDs
   - Exactly one `IsStart == true`
   - Each action’s **all** `FromStateIds` must exist in `States`
   - Each action’s `ToStateId` must exist in `States`

2. **Instance start** (`new WorkflowInstance(definition)`):

   - Must have exactly one start state
   - Sets `CurrentStateId` to that state

3. **Action execution** (`WorkflowInstance.ExecuteAction`):

   - Action must be `Enabled`
   - Current state must be found and `Enabled`
   - `CurrentStateId` must be contained in the action’s `FromStateIds`
   - Throws `InvalidOperationException` on violations

4. **Service-level guards** (`WorkflowService`):

   - Missing definition or instance → `KeyNotFoundException`
   - Action/state not in definition → `KeyNotFoundException`
   - Disabled action/state → `InvalidOperationException`

---

## Persistence

- **In-memory store** backed by `Infrastructure.Storage.InMemoryStore<T>`
- **JSON snapshot** on every `AddAsync`/`UpdateAsync` to `definitions.json` and `instances.json`
- **No EF Core usage** at runtime; all EF configuration classes have been stubbed or removed

---

## Design & Readability

- **Layered Architecture**

  - **Domain**: Entities & business rules
  - **Application**: DTOs, AutoMapper profiles, service orchestration
  - **Infrastructure**: In-memory store, repository implementations
  - **API**: Minimal-API endpoints, DI setup, exception handling

- **Modularity**

  - `ServiceCollectionExtensions` wires services and repos
  - `ApplicationBuilderExtensions` wires HTTP endpoints

- **Strong Typing**

  - GUIDs everywhere, record types for DTOs, no magic strings

---

## Maintainability & Pragmatism

- **Simple Abstraction**: Single reusable in-memory store, not over-engineered
- **Swappable Persistence**: Replace in-memory repos with EF-backed ones if needed
- **Concise Startup**: `Program.cs` remains minimal

---

## Running & Testing

1. **Restore & build**

   ```bash
   dotnet clean && dotnet build
   ```

2. **Run the API**

   ```bash
   cd Api
   dotnet run
   ```

3. **Swagger UI**
   Browse to `http://localhost:5082/swagger` to explore and test all endpoints interactively.

4. **Manual Tests**

   - **Create** a definition with multi-state transitions
   - **List** definitions and verify fields (including `Enabled`)
   - **Start** an instance and **execute** actions
   - **Disable** a state/action, attempt invalid transition → expect 400/500 with clear message
   - **Enable** it back and verify normal flow

---

> Feel free to raise issues or suggest enhancements in the repository’s issue tracker. Enjoy building your workflows!
