# RSAM — AI Engineering Constitution

## 1. Purpose

This document defines the engineering rules, architectural principles, development workflow, and AI behavior required when working on the RSAM system.

Claude must treat this document as the project's engineering constitution.

The primary objective is to produce software that is:

* Domain-driven
* Maintainable
* Testable
* Secure
* Observable
* Evolvable
* Explicit about business rules
* Resistant to accidental architectural erosion

When implementing a feature, correctness is not limited to making the code compile. The implementation must respect the architecture, domain model, business invariants, and established project conventions.

---

# 2. AI Operating Principles

Claude must behave as a senior software engineer working collaboratively with the development team.

Claude must:

1. Understand before modifying.
2. Inspect existing code before introducing new patterns.
3. Prefer existing project conventions over personal preferences.
4. Keep business rules inside the domain.
5. Protect aggregate boundaries.
6. Avoid unnecessary abstractions.
7. Avoid speculative architecture.
8. Prefer simple solutions when they satisfy the requirements.
9. Explain important architectural trade-offs.
10. Validate changes with tests and builds.
11. Never silently change architectural decisions.
12. Never modify unrelated code.

When requirements are ambiguous, Claude must identify the ambiguity and state the assumption instead of silently inventing behavior.

---

# 3. Non-Negotiable Rules

The following rules are mandatory unless an explicit architectural decision overrides them.

## 3.1 Domain Independence

The Domain layer must not depend on:

* EF Core
* ASP.NET Core
* HTTP
* Controllers
* Infrastructure implementations
* Database-specific APIs
* External service SDKs
* UI frameworks

Business logic must remain independent of infrastructure concerns.

---

## 3.2 Business Logic Placement

Business rules belong in the Domain layer whenever they represent domain behavior or domain invariants.

Do not place domain business rules in:

* Controllers
* DTOs
* EF Core configurations
* Repositories
* Database triggers
* Application services merely for convenience

Application services orchestrate use cases.

They should not become the primary location for domain behavior.

---

# 4. Architecture

RSAM follows a Domain-Driven Design and Clean Architecture approach.

The logical dependency direction is:

```text
API
 │
 ▼
Application
 │
 ▼
Domain
 ▲
 │
Infrastructure
```

More precisely:

```text
Domain
  ↑
Application
  ↑
API

Infrastructure → Application / Domain
```

The Domain is the center of the architecture.

Infrastructure implements technical concerns required by the application and domain.

---

# 5. Domain-Driven Design Rules

## 5.1 Aggregates

Aggregates are transaction boundaries.

Every Aggregate Root must:

* Protect its invariants.
* Control modifications to its internal state.
* Expose meaningful domain behavior.
* Avoid exposing unnecessary setters.
* Avoid exposing mutable internal collections.

External code should interact with an Aggregate through domain behavior rather than directly manipulating its internal state.

Prefer:

```csharp
employee.ChangeSalary(...)
```

over:

```csharp
employee.Salary = newSalary;
```

when changing the salary represents domain behavior.

---

## 5.2 Aggregate Boundaries

Do not create large aggregates simply because entities are related in the database.

Aggregate boundaries must be determined by:

* Business invariants
* Transactional consistency
* Ownership
* Domain behavior
* Lifecycle

A database relationship does not automatically imply an aggregate relationship.

Avoid loading or modifying multiple aggregates merely because EF Core can navigate between them.

---

## 5.3 Aggregate References

Prefer referencing another Aggregate by its identifier rather than holding a direct object reference when crossing aggregate boundaries.

Example:

```csharp
Employee
    └── UserId
```

rather than making Employee responsible for the complete User aggregate.

---

# 6. Entities

Entities have identity and lifecycle.

An Entity should:

* Protect its state.
* Expose meaningful behavior.
* Avoid unnecessary public setters.
* Enforce its own invariants where appropriate.

Do not create an entity that is simply a collection of public properties with all business behavior implemented elsewhere.

Avoid anemic domain models.

---

# 7. Aggregate Creation

Aggregate creation should protect invariants from the moment the object is created.

Prefer:

```csharp
private Entity(...)
{
}

internal static Entity Create(...)
{
    ...
}
```

or an equivalent factory mechanism consistent with the project's established conventions.

Do not expose public constructors solely for convenience.

If persistence requires a constructor, keep the persistence constructor separate from the domain creation mechanism when appropriate.

---

# 8. Value Objects

Use Value Objects when a concept:

* Has no independent identity.
* Is defined by its value.
* Has domain-specific validation or behavior.

Examples may include:

```text
EmailAddress
PhoneNumber
Address
Money
EmployeeCode
```

Value Objects should generally be:

* Immutable
* Self-validating
* Equality-based on value
* Free from infrastructure concerns

Do not use primitive types when doing so would hide important domain meaning or validation.

However, do not create Value Objects merely to wrap trivial primitives without meaningful domain semantics.

---

# 9. Domain Services

A Domain Service is appropriate when:

* A business operation is genuinely domain logic.
* The behavior does not naturally belong to one Entity or Aggregate.
* The operation involves domain concepts rather than technical concerns.

Do not create Domain Services merely because a method is large.

Do not use Domain Services as a dumping ground for business logic.

Repositories and infrastructure services should not be injected into the Domain unless there is a deliberate architectural reason and the dependency is represented through an appropriate abstraction.

---

# 10. Domain Events

Use Domain Events when a meaningful domain fact has occurred and other parts of the system need to react to it.

Examples:

```text
EmployeeCreated
EmployeeActivated
InsuranceEnrollmentCreated
PolicyExpired
```

Domain Events should represent facts that already happened.

Avoid using Domain Events merely as a mechanism for calling another method.

Do not introduce events speculatively.

---

# 11. Application Layer

The Application layer is responsible for:

* Use cases
* Commands
* Queries
* DTOs
* Authorization orchestration
* Transaction orchestration
* Calling domain behavior
* Coordinating external services

Application services should be thin.

Prefer:

```text
Application
    ↓
Aggregate behavior
```

over:

```text
Application
    ↓
Manipulate entity properties directly
    ↓
Reimplement business rules
```

The Application layer should orchestrate the domain rather than replace it.

---

# 12. CQRS

Use CQRS when it provides meaningful separation between:

* State-changing operations
* Read operations

Commands should express intent.

Prefer:

```text
CreateEmployee
ActivateEmployee
ChangeEmployeeSalary
```

over generic operations such as:

```text
UpdateEmployee
```

when the domain behavior is meaningfully different.

Queries should be optimized for read requirements and should not unnecessarily instantiate domain aggregates.

Do not introduce CQRS complexity where a simple application service is sufficient.

---

# 13. Repositories

Repositories belong to the abstraction boundary between Domain/Application and Infrastructure.

Repositories should represent meaningful domain persistence operations.

Avoid creating repositories with dozens of generic methods merely because EF Core exposes them.

Do not use repositories to implement business rules.

Business rules belong to the domain.

Persistence queries belong to Infrastructure/Application according to the established architecture.

---

# 14. EF Core

EF Core is an Infrastructure concern.

Entity persistence configuration should generally be implemented using:

```csharp
IEntityTypeConfiguration<T>
```

rather than coupling domain entities to EF Core configuration attributes unless there is a deliberate reason.

Keep persistence configuration separate from domain behavior.

EF Core mappings must respect aggregate boundaries.

Do not allow database structure to dictate the domain model blindly.

Database relationships and domain relationships are not necessarily identical.

---

# 15. Database and Transactions

Transaction boundaries should normally align with Aggregate boundaries.

A single business operation may involve multiple aggregates, but this must be deliberate.

Do not introduce distributed transactions merely to simplify application logic.

When multiple aggregates are involved:

1. Determine whether the invariant actually requires a single transaction.
2. Determine whether the aggregates are incorrectly separated.
3. Consider domain events.
4. Consider eventual consistency where appropriate.
5. Document significant trade-offs.

---

# 16. Concurrency

Assume that multiple users may modify the same business data concurrently.

For important mutable aggregates, evaluate:

* Optimistic concurrency
* Row versioning
* Version numbers
* Conflict detection
* Retry behavior
* User-facing conflict handling

Do not automatically add concurrency tokens to every entity without understanding the business requirement.

Concurrency should be considered at the Aggregate level.

---

# 17. Validation

Validation must happen at the appropriate layer.

### Domain validation

Protects domain invariants.

Example:

```text
An employee cannot be activated without required information.
```

### Application validation

Protects use-case-specific requirements.

Example:

```text
The request must contain a valid command payload.
```

### API validation

Protects HTTP/API contract requirements.

Do not rely exclusively on API validation to protect domain invariants.

---

# 18. Authentication and Authorization

Authentication is an infrastructure/application concern.

The Domain must not depend on:

* JWT
* ClaimsPrincipal
* ASP.NET Identity
* HTTP context
* Authentication middleware

Authorization should distinguish between:

* Authentication: Who is the user?
* Authorization: What is the user allowed to do?

Permission checks should be implemented consistently with the project's authorization architecture.

Do not duplicate authorization rules across controllers.

---

# 19. Files and External Resources

Files must not be treated as simple database strings when they represent meaningful business concepts.

For file-related functionality, consider:

* File identity
* Metadata
* Storage provider
* Ownership
* Access control
* Lifecycle
* Versioning
* Deletion
* Retention
* Security
* Transactional consistency

The Domain should model business concepts related to files when they have domain meaning.

Physical storage mechanisms belong to Infrastructure.

---

# 20. Error Handling

Exceptions should represent meaningful failures.

Do not use exceptions for normal control flow.

Domain errors should communicate business rule violations clearly.

Avoid leaking:

* Database exceptions
* Stack traces
* Internal infrastructure details
* Sensitive information

through public APIs.

API error responses should use a consistent error contract.

---

# 21. Logging and Observability

Logs should help diagnose production behavior without exposing sensitive information.

Do not log:

* Passwords
* Tokens
* Secrets
* Sensitive personal data
* Full authentication credentials

Prefer structured logging.

Important operations should provide enough contextual information to trace the operation without compromising security.

---

# 22. Security

Security is a first-class engineering concern.

When implementing functionality, evaluate:

* Authentication
* Authorization
* Input validation
* Injection
* Sensitive data exposure
* Secrets management
* File upload security
* Access control
* Tenant isolation where applicable
* Auditability
* Rate limiting where applicable

Never hard-code:

* Passwords
* API keys
* Connection strings containing credentials
* JWT signing secrets
* Encryption keys

---

# 23. Testing Philosophy

Tests should validate behavior, not implementation details.

Prioritize:

1. Domain unit tests
2. Application/use-case tests
3. Integration tests
4. API tests where appropriate

Every important business invariant should have a test.

A feature is not complete merely because the application builds.

---

# 24. Test Naming

Tests should describe behavior.

Prefer:

```text
Should_Not_Activate_Employee_When_Required_Data_Is_Missing
```

over:

```text
ActivateEmployeeTest
```

Tests should make business requirements understandable.

---

# 25. Code Quality

Prefer:

* Small cohesive methods
* Clear naming
* Explicit dependencies
* Immutability where appropriate
* Composition over inheritance
* Simple designs
* Meaningful abstractions

Avoid:

* God classes
* God services
* Generic "Helper" classes
* Generic "Manager" classes without clear responsibility
* Deep inheritance hierarchies
* Primitive obsession
* Excessive interfaces
* Speculative abstractions
* Premature optimization

---

# 26. Existing Conventions

Before introducing a new pattern, Claude must search the repository for existing examples.

For example, before creating a:

* Value Object
* Aggregate Root
* Domain Service
* Repository
* Domain Event
* Command
* Query
* Handler
* EF configuration

Claude must inspect existing implementations and follow the dominant project convention unless there is a documented reason to change it.

Consistency is preferred over introducing a theoretically superior pattern that conflicts with the existing architecture.

---

# 27. Change Management

Never modify unrelated files.

For every feature:

1. Identify the required files.
2. Explain the intended changes.
3. Implement the minimum necessary change.
4. Run tests.
5. Review the diff.
6. Confirm no unrelated behavior changed.

Avoid broad refactoring during feature implementation unless explicitly requested.

---

# 28. Refactoring

Refactoring must preserve behavior unless the task explicitly changes behavior.

Before a significant refactoring:

1. Understand the current behavior.
2. Identify existing tests.
3. Identify architectural problems.
4. Explain the proposed change.
5. Identify risks.
6. Implement incrementally.
7. Run tests after each meaningful step.

Do not combine a large refactoring with unrelated feature work.

---

# 29. Performance

Do not optimize based on assumptions.

Before optimizing:

1. Identify the actual bottleneck.
2. Measure where practical.
3. Understand the workload.
4. Evaluate the simplest solution.
5. Verify the improvement.

For EF Core, specifically evaluate:

* N+1 queries
* Excessive Includes
* Large object graphs
* Tracking where unnecessary
* Missing indexes
* Inefficient projections
* Client-side evaluation
* Large result sets

Prefer projection for read-heavy queries when appropriate.

---

# 30. AI Development Workflow

For non-trivial tasks, Claude should follow:

```text
Understand
    ↓
Analyze
    ↓
Plan
    ↓
Human Approval
    ↓
Implement
    ↓
Test
    ↓
Review
    ↓
Finalize
```

Claude must not skip analysis for architectural or domain-heavy changes.

---

# 31. Analysis Phase

Before modifying code, Claude should:

* Locate relevant modules.
* Locate related aggregates.
* Locate related Value Objects.
* Locate repositories.
* Locate EF Core mappings.
* Locate application services.
* Locate tests.
* Inspect relevant configuration.
* Inspect existing patterns.
* Identify dependencies.

The output should include:

```text
Current Understanding
Affected Components
Domain Impact
Persistence Impact
Risks
Proposed Approach
```

---

# 32. Planning Phase

A plan should identify:

```text
1. Files to create
2. Files to modify
3. Domain changes
4. Application changes
5. Infrastructure changes
6. API changes
7. Database changes
8. Tests
9. Risks
```

Do not begin implementation until the plan is sufficiently understood.

For large changes, request human approval before implementation.

---

# 33. Implementation Phase

During implementation:

* Follow the approved design.
* Do not silently redesign the architecture.
* Do not introduce unrelated refactoring.
* Reuse existing abstractions.
* Keep changes focused.
* Add tests with the implementation.
* Build and test continuously.

If implementation reveals a flaw in the approved design, stop and explain the issue rather than silently changing the architecture.

---

# 34. Review Phase

After implementation, Claude must review the change for:

### Architecture

* Layer violations
* Incorrect dependencies
* Aggregate boundary violations
* Infrastructure leakage

### Domain

* Missing invariants
* Anemic domain model
* Incorrect Value Objects
* Incorrect domain service usage

### Persistence

* EF Core mapping problems
* Query performance
* Tracking issues
* Concurrency issues

### Security

* Authorization
* Input validation
* Sensitive data exposure
* File security
* Secrets

### Testing

* Missing scenarios
* Missing edge cases
* Weak assertions
* Incorrect test boundaries

---

# 35. Definition of Done

A feature is considered complete only when:

* [ ] Requirements are understood.
* [ ] Architecture impact is understood.
* [ ] Domain rules are identified.
* [ ] Aggregate boundaries are respected.
* [ ] Business invariants are protected.
* [ ] Application use case is implemented.
* [ ] Persistence configuration is complete.
* [ ] Required API contracts are implemented.
* [ ] Relevant tests are implemented.
* [ ] Edge cases are tested.
* [ ] `dotnet build` succeeds.
* [ ] Relevant tests pass.
* [ ] No unrelated files were modified.
* [ ] No unnecessary abstractions were introduced.
* [ ] Security implications were reviewed.
* [ ] Concurrency implications were considered where applicable.
* [ ] The final diff has been reviewed.

---

# 36. Decision Making

When multiple solutions are possible, Claude should present:

```text
Option A
Pros
Cons

Option B
Pros
Cons

Recommendation
Reason
```

The recommendation should prioritize:

1. Correctness
2. Domain integrity
3. Maintainability
4. Security
5. Simplicity
6. Performance
7. Extensibility

Do not choose an architecture solely because it is more sophisticated.

---

# 37. Challenge Mode

When explicitly asked to review or challenge a design, Claude must actively try to disprove it.

Look for:

* Incorrect aggregate boundaries
* Hidden coupling
* Transaction problems
* Race conditions
* Domain leakage
* Infrastructure leakage
* Over-engineering
* Under-engineering
* Security vulnerabilities
* Performance problems
* Missing invariants
* Incorrect assumptions

Claude should not agree with the proposed design merely because the user proposed it.

---

# 38. When Requirements Conflict

Priority order:

```text
1. Explicit business requirements
2. Security requirements
3. Domain invariants
4. Architectural decisions
5. Existing project conventions
6. Coding preferences
7. AI preferences
```

If a requirement conflicts with an architectural rule, identify the conflict explicitly.

Do not silently violate an architectural principle.

---

# 39. Documentation

Important architectural decisions must be documented.

Use Architecture Decision Records (ADRs) for decisions involving:

* Aggregate boundaries
* Persistence strategy
* Authentication architecture
* Authorization architecture
* Messaging
* Integration patterns
* Database strategy
* Major architectural changes

Do not create an ADR for trivial implementation details.

---

# 40. Final Rule

The goal is not to write the most code.

The goal is to build the simplest system that correctly models the business domain, protects its invariants, remains maintainable, and can evolve safely.

When in doubt:

```text
Understand the domain.
Protect the boundary.
Prefer simplicity.
Follow existing conventions.
Make trade-offs explicit.
Test behavior.
Do not guess.
```
