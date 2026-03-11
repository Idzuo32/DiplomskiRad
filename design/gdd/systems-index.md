# Relic Rush — Systems Index

**Version**: 0.1
**Date**: 2026-03-11
**Reference**: See game-concept.md for full vision and scope tiers.

This document is the master list of all game systems. Each system links to its
dedicated GDD. Design work should proceed in priority order.

---

## Core Systems (MVP Critical)

| Priority | System | GDD File | Status |
|----------|--------|----------|--------|
| 1 | Player Controller (jump + slide) | `player-controller.md` | Partial — redesign needed |
| 2 | Obstacle System | `obstacle-system.md` | Partial — redesign needed |
| 3 | Procedural Generation | `proc-gen.md` | Implemented — document only |
| 4 | Biome System | `biome-system.md` | Planned |
| 5 | Gadget / Pickup System | `gadget-system.md` | Planned |
| 6 | Negative Pickups | `negative-pickups.md` | Planned |
| 7 | Mission System | `mission-system.md` | Planned |
| 8 | Timer & Difficulty Ramp | `difficulty-ramp.md` | Partial — redesign needed |

## Meta-Progression Systems (MVP Critical)

| Priority | System | GDD File | Status |
|----------|--------|----------|--------|
| 9 | Coin Economy & Upgrade Tree | `economy.md` | Planned |
| 10 | Artifact System | `artifact-system.md` | Planned |
| 11 | World Map | `world-map.md` | Planned |
| 12 | Artifact Museum | `artifact-museum.md` | Planned |
| 13 | Save System | `save-system.md` | Partial — expand needed |

## Presentation Systems (MVP)

| Priority | System | GDD File | Status |
|----------|--------|----------|--------|
| 14 | Multiple Paths / Branching Routes | `path-system.md` | Planned |
| 15 | Win / Escape Screen | `win-screen.md` | Planned |
| 16 | HUD & UI | `hud-ui.md` | Partial |

## V1.0 Systems (Post-MVP)

| Priority | System | GDD File | Status |
|----------|--------|----------|--------|
| — | Character / Skin Unlocks | `character-system.md` | Planned |
| — | Leaderboards | `leaderboards.md` | Planned |
| — | Biomes 4–7 | (per-biome files) | Planned |

---

## Design Order Rationale

Systems 1–3 are redesigned first because they are the foundation everything
else runs on. Biome System (4) must be designed before individual biome GDDs
are written. Gadget System (5) before Negative Pickups (6) since gadgets are
the counter-mechanic. Mission System (7) before Economy (9) since missions
define what earns rewards.
