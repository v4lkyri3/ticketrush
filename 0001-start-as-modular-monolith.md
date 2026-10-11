# ADR 0001: Start as a modular monolith, split into services later

- **Status:** Accepted
- **Date:** 2026-10-12
- **Deciders:** Project author

## Context

TicketRush is intended to demonstrate production-grade system design: caching, messaging, load balancing, and resilience patterns. The target architecture is microservices with Kafka between them. However, the domain rules (seat holds, booking states, pricing) are not yet proven, and distributed-systems problems (network failures, eventual consistency, message duplication) make early debugging expensive.

## Decision

1. Phases 1-2: build a **modular monolith**: one deployable, one database, but internal modules (Identity, Catalog, Booking, Payment, Notification) with explicit boundaries. Modules talk through interfaces, not by reaching into each other's tables.
2. Phase 3: **extract modules into services**, each with its own database, and replace in-process calls with Kafka events and REST.

## Alternatives considered

| Option | Why not |
|---|---|
| Microservices from day one | High upfront cost; boundaries are guessed before the domain is understood; slows down feedback |
| Permanent monolith | Doesn't demonstrate event-driven design, outbox, saga, or independent scaling, which are project goals |

## Consequences

**Positive**
- Domain logic and concurrency handling are validated with simple tests first.
- The split is driven by real seams, which makes a good interview story: *"I evolved the design and here is why."*
- Git history shows the evolution.

**Negative**
- Extraction work in Phase 3 (data migration, replacing in-process calls).
- Discipline needed to keep module boundaries clean (no cross-module DB joins).

## Guardrails

- Each module has its own folder, its own DbContext (or schema), and a public contract.
- No module references another module's internals; architecture tests (NetArchTest) enforce this.
- Cross-module reads/writes go through application-layer interfaces that can later be swapped for HTTP or Kafka.
