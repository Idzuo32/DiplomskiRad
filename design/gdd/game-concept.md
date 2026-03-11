# Relic Rush — Game Concept Document

**Version**: 0.1 (Draft)
**Date**: 2026-03-11
**Engine**: Unity 6.3.10f1 URP
**Platform**: PC + Mobile (optimized for both)
**Genre**: Narrative Escape Runner

---

## 1. Overview

Relic Rush is a 3D narrative escape runner in which the player controls an
Indiana Jones-inspired artifact explorer navigating dangerous, exotic locations
after disturbing a sacred relic. Each run is a self-contained escape mission:
the player picks up a relic, the environment reacts (traps trigger, the location
begins to collapse or pursue), and they must reach the exit alive within the time
limit. Success reveals the collected artifact and its story. Failure sends the
player back to try again. Progression is driven by a biome-based world map, a
gadget/pickup system, and a growing artifact museum that rewards both skill and
curiosity.

---

## 2. Player Fantasy

The player should feel like a brilliant, daring explorer who is always one step
ahead of disaster — but only just. Each run evokes:

- **Adventure**: Beautiful, exotic biomes reward the player visually as they run
  through them. The world feels alive and worth exploring.
- **Urgency**: Something is always chasing. The environment is hostile and
  reacting to the player's presence.
- **Mastery**: Near-misses, gadget timing, and path choices make the player feel
  skilled, not lucky.
- **Discovery**: Each collected artifact tells a story. The museum grows. There
  is always something new to find.
- **Replayability**: "I can do better." Each escape invites improvement — better
  time, more artifacts collected, higher star rating.

---

## 3. Core Loop

### Micro-loop (per obstacle, ~1–3 seconds)
React to incoming obstacle → choose lane / jump / slide → near-miss or collect
pickup → feel the feedback (visual, audio, haptic).

### Meso-loop (per mission, 3–10 minutes)
Enter biome → pick up relic → chaser activates → run through procedurally
arranged chunks → collect gadget fragments + artifacts → reach exit →
success screen (artifact reveal + star rating) or failure screen
(retry with motivation).

### Macro-loop (across sessions)
Complete missions → earn coins + artifacts → unlock next biome on world map →
expand artifact museum → unlock skins, characters, new biomes.

---

## 4. Game Pillars

**1. Adventure First**
Every system, visual, and sound should reinforce the feeling of an epic
expedition. Biomes are beautiful. Artifacts have stories. The world map is an
adventure journal, not a menu.

**2. Fair Challenge**
Obstacles are always readable and avoidable. Difficulty ramps through increased
speed, obstacle density, and pattern complexity — never through unfair
randomness. The player dies because of their choices, not the game's.

**3. Rewarding Mastery**
Multiple paths, gadget timing, and fragment collection create a skill ceiling
worth reaching. A perfect run should feel distinctly different from a survival
run. Star ratings and score comparisons surface this improvement.

**4. Bite-Sized Depth**
Sessions are 3–10 minutes. The meta-progression is meaningful but never
overwhelming. A player with 5 minutes and a player with an hour both find
satisfying loops.

**5. Living World**
The artifact museum grows. The world map fills in. The game should feel like
it is being explored and uncovered over time, not depleted.

---

## 5. Target Player

**Primary**: Casual-to-midcore mobile and PC players, ages 16–35, who enjoy
skill-based runners with a strong aesthetic identity and a sense of progression.
Fans of Indiana Jones, Uncharted, and games like Temple Run or Alto's Adventure.

**Secondary**: Portfolio and game jam audience — developers and designers
evaluating the project's systems depth and visual quality.

**Motivations** (Bartle/BrainHex):
- Achiever: High score, star ratings, mission completion
- Explorer: Biome discovery, artifact lore, hidden paths
- Collector: Artifact museum, skin/character unlocks

---

## 6. Biomes

Each biome is an expedition site with a unique visual theme, chaser, obstacle
set, and negative pickup type. Biomes unlock sequentially on the world map.

| # | Biome | Chaser | Atmosphere | MVP |
|---|-------|--------|------------|-----|
| 1 | Forest | Pack of wolves | Lush, misty, serene then dark | ✅ |
| 2 | Temple | Collapsing ceiling / boulder | Ancient, trap-laden, dramatic | ✅ |
| 3 | Lava | Lava flow | Intense, red-orange, apocalyptic | ✅ |
| 4 | Desert | Sandstorm / rival explorer | Vast, sun-bleached, mirage effects | — |
| 5 | Beach/Sea | Tidal wave / kraken | Coastal, blue, stormy | — |
| 6 | Town | City guards / angry mob | Crowded, colourful, chaotic | — |
| 7 | Dungeon | Awakened monster | Narrow, dark, torchlit | — |

Each biome contains 3–5 missions of increasing difficulty. All missions in a
biome must be completed to unlock the next biome.

---

## 7. Key Systems

| System | Description | Status |
|--------|-------------|--------|
| Player Controller | Lane switching (left/right), jump, slide | Partial (L/R exists) |
| Procedural Generation | Chunk-based level assembly with checkpoints | Implemented |
| Obstacle System | Static, moving, pattern obstacles — biome-specific | Partial (redesign needed) |
| Gadget / Pickup System | Fragment-based activation (3–5 parts), Indiana Jones themed | Planned |
| Negative Pickups | Biome-specific curses, visually distinct, avoidable | Planned |
| Biome System | Visual theme, chaser, hazard set, ambient audio per biome | Planned |
| Mission System | 3–5 missions per biome, 1–3 star rating per mission | Planned |
| Meta-Progression | Coin economy → upgrades; artifact currency → content unlocks | Planned |
| Artifact Museum | Main menu collection screen with lore per artifact | Planned |
| World Map | Parchment-aesthetic map, locked (muted) / unlocked (color) biomes | Planned |
| Save System | Per-mission best score; unlock state; museum collection | Partial |
| Multiple Paths | Branching routes with risk/reward differentiation | Planned |

---

## 8. Gadgets (Pickup System)

Gadgets are the Indiana Jones-flavored power-up system. Each gadget requires
collecting 3–5 fragments scattered through the level before it can be activated
via a UI tap/press. Fragments do not persist on run failure.

| Gadget | Fragments | Effect | Duration | Theme |
|--------|-----------|--------|----------|-------|
| Whip | 3 | Clear next obstacle automatically + speed burst | Instant | Field tool |
| Shield (Fedora) | 3 | Negates one obstacle hit or negative pickup | Until triggered | Protection |
| Torch | 4 | Reveals hidden path + highlights artifact locations | 12s | Exploration |
| Map | 5 | Slows time briefly (bullet-time dodge window) | 6s | Knowledge |

---

## 9. Negative Pickups

Visually distinct from positive pickups (red/dark glow vs. gold). Clearly
telegraphed. Avoidable through skilled lane choice. Appear from mid-mission
onward; frequency increases with difficulty. The Shield gadget negates one
negative pickup effect.

| Pickup | Biome | Effect | Duration |
|--------|-------|--------|----------|
| Cursed Idol | Temple | Reverse controls | 5s |
| Mud Pit | Forest | Speed reduction | 4s |
| Heat Shimmer | Lava | Blurred vision | 3s |
| Bandit's Trap | Town | Timer -5s | Instant |

---

## 10. Scope Tiers

### MVP (Portfolio / Initial Release)
- 3 biomes: Forest, Temple, Lava
- 3 missions per biome (9 total)
- 4 gadgets
- 3–4 negative pickup types
- Coin economy + basic upgrade tree (3–5 upgrades)
- Artifact museum (9 artifacts with lore)
- World map screen
- Jump + Slide controls
- Single character
- PC + Mobile builds

### V1.0
- +2 biomes (Desert, Beach/Sea)
- +2 missions per biome
- 2 unlockable characters / skins
- Mission system with 3-star rating
- Expanded upgrade tree
- 2+ branching path types per chunk set

### Full Vision
- All 7 biomes
- 5 missions per biome (35 total)
- 4+ characters
- Full artifact museum (35 artifacts)
- Seasonal/event missions
- Leaderboards

---

## 11. Art Direction (Reference)

- **Style**: Low-to-mid poly, clean silhouettes, vibrant per-biome palette
- **UI**: Warm, parchment-textured world map; clean HUD with fragment counters
- **Tone**: Arcade-casual with cinematic moments (escape sequence, win screen)
- **Performance target**: 60fps on mid-range mobile, 120fps on PC

---

## 12. Open Questions / Future Decisions

- [ ] Specific character design and name for the protagonist
- [ ] Whether negative pickups appear in Forest (biome 1) or only from Temple onward
- [ ] Exact upgrade tree contents
- [ ] Audio direction (orchestral adventure vs. upbeat arcade)
