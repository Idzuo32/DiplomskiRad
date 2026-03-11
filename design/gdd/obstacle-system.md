# Relic Rush — Obstacle System GDD

**Version**: 0.1
**Date**: 2026-03-11
**Status**: Redesign (replaces current ObstacleSpawner random placement model)
**Dependencies**: Player Controller, Procedural Generation, Biome System, Pool Manager

---

## 1. Overview

The obstacle system delivers a fair, readable, and varied stream of hazards
as the player runs through a biome. It uses a hybrid model: a library of
hand-authored obstacle patterns is assembled procedurally at runtime, with
randomness applied to pattern selection and spacing — never to individual
obstacle placement within a pattern. Each biome has its own pattern library
and obstacle prefab set. Difficulty scales by promoting harder patterns and
tightening inter-pattern spacing as the run progresses.

---

## 2. Player Fantasy

Every obstacle death should feel like the player's fault, not the game's.
The obstacle system exists to create tension, near-misses, and moments of
skilled navigation — not to randomly kill the player. Obstacles should feel
like traps set by the environment, consistent with the "you disturbed the
relic" narrative.

---

## 3. Detailed Rules

### 3.1 Obstacle Types

Six obstacle types cover the full input space of the player controller:

| Type | Required Action | Height | Lanes Blocked | Example (Forest) |
|------|----------------|--------|---------------|------------------|
| Ground Blocker | Dodge left or right | Ground level | 1 lane | Log, rock |
| Double Blocker | Dodge into the one free lane | Ground level | 2 lanes | Two logs side-by-side |
| Overhead | Slide | Head height | Full width | Low branch, fallen archway |
| Aerial | Jump | Knee height | 1–2 lanes | Root ridge, low wall |
| Moving | Time-based dodge | Ground level | Travels across lanes | Rolling barrel, swinging pendulum |
| Gauntlet | Sequence of inputs | Mixed | Varies | See Section 3.3 |

**Rules:**
- At least one lane is always free for Ground Blocker and Double Blocker
- Overhead obstacles always span the full path width (no lane escape — must slide)
- Aerial obstacles always have a jump clearance of at least jumpHeight + 0.3 units
- Moving obstacles have a visible, predictable travel path and fixed period
- No obstacle type requires an input not yet introduced in the current mission

### 3.2 Pattern Library

A pattern is a hand-authored sequence of 1–5 obstacles with defined:
- Obstacle types and their lane assignments
- Z spacing between obstacles within the pattern (in world units)
- Difficulty rating: Easy / Medium / Hard
- Required player actions (used to gate patterns by mission progression)
- Biome tags (which biomes this pattern belongs to)

Patterns are stored as ScriptableObjects. Each biome has a curated pattern
library. The system selects patterns randomly from the library, filtered by
the current difficulty tier.

**Example patterns (Forest biome):**

| Pattern ID | Sequence | Difficulty | Actions Required |
|------------|----------|------------|-----------------|
| F_E_01 | Ground Blocker (left) | Easy | Dodge right |
| F_E_02 | Ground Blocker (right) | Easy | Dodge left |
| F_E_03 | Aerial (centre) | Easy | Jump |
| F_M_01 | Ground Blocker (left) → Ground Blocker (right) | Medium | Dodge right, then left |
| F_M_02 | Double Blocker (left+centre) | Medium | Dodge right |
| F_M_03 | Aerial (centre) → Ground Blocker (right) | Medium | Jump, then dodge left |
| F_H_01 | Ground Blocker (left) → Overhead → Ground Blocker (right) | Hard | Dodge right, slide, dodge left |
| F_H_02 | Moving (left-to-right) → Ground Blocker (centre) | Hard | Time dodge + lane change |
| F_H_03 | Double Blocker (right+centre) → Aerial (left) | Hard | Dodge left, jump |

MVP target: 6–10 patterns per difficulty tier per biome (18–30 patterns per biome).

### 3.3 Gauntlet Patterns

Gauntlets are extended sequences (3–5 obstacles) that form a mini-challenge.
They are treated as a single pattern selection. Rules:

- Gauntlets only appear from mid-mission difficulty onward (never in the
  opening 30 seconds of a run)
- Gauntlets are always preceded by a 2-second gap (breathing room)
- Gauntlets are always followed by a 3-second gap (recovery room)
- Each obstacle in a gauntlet individually satisfies telegraph timing rules

### 3.4 Pattern Selection Logic

At runtime, the spawner selects the next pattern using this logic:

```
1. Determine current difficulty tier (Easy / Medium / Hard) from DifficultyRamp
2. Filter pattern library: biome match + difficulty tier match + actions unlocked
3. Exclude the last N patterns from selection (prevent immediate repetition, N = 2)
4. Select randomly from filtered pool
5. Schedule pattern spawn at current inter-pattern interval
```

**Safety rules (applied before spawning):**
- Never select two consecutive Overhead patterns (player cannot stand up between slides)
- Never select a pattern that requires an action not yet introduced in the mission
- If filtered pool is empty (e.g., biome has no Hard patterns yet), fall back to Medium

### 3.5 Difficulty Progression

Difficulty is controlled by two variables that change at checkpoints:

| Variable | Easy | Medium | Hard |
|----------|------|--------|------|
| Pattern tier | Easy only | Easy + Medium | All tiers |
| Inter-pattern gap | 3.0s | 2.0s | 1.2s |
| Moving obstacle period | Slow (3s) | Medium (2s) | Fast (1.2s) |

Tier transitions:
- Checkpoint 1–2: Easy only
- Checkpoint 3–4: Easy + Medium (weighted 70/30)
- Checkpoint 5+: All tiers (weighted 30/50/20)

This replaces the current exponential decay of obstacleSpawnTime, which is
replaced by the inter-pattern gap controlled per difficulty tier.

### 3.6 Telegraph Rules

Every obstacle must be visible to the player with enough lead time to react.
Lead time is calculated from the spawn Z position to the player Z position,
divided by the current chunk scroll speed.

| Scroll Speed | Minimum Lead Time | Minimum Spawn Distance |
|-------------|-------------------|----------------------|
| ≤ 8 m/s | 1.5s | 12 units |
| 8–12 m/s | 2.0s | 20 units |
| > 12 m/s | 2.5s | 30+ units |

The spawn Z offset is dynamically calculated each spawn based on current speed
to always meet the minimum lead time. This replaces the current fixed spawn Z.

**Additional telegraph rules:**
- Moving obstacles have a visible start position and travel path before
  they enter the danger zone
- All obstacle prefabs have a consistent visual silhouette that communicates
  their type (tall = ground blocker, wide = overhead, etc.)
- Biome-specific obstacles use materials/colours consistent with their biome

### 3.7 Biome Obstacle Sets

Each biome provides its own prefab set and pattern library. Obstacle types
remain consistent across biomes; only the prefabs and patterns differ.

| Biome | Ground Blocker | Overhead | Aerial | Moving |
|-------|---------------|----------|--------|--------|
| Forest | Log, boulder | Fallen branch, vine curtain | Root ridge | Rolling log |
| Temple | Stone block, statue | Low archway, ceiling slab | Pressure plate ridge | Swinging pendulum |
| Lava | Lava rock, cooled pillar | Smoke vent | Lava crack ridge | Falling stalactite |

### 3.8 Object Pooling

The existing PoolManager integration is preserved. Each obstacle prefab has a
pre-warmed pool. Obstacles are returned to the pool when they pass the player
via the existing ObstacleDestroy trigger volume.

Pattern-level pooling: all obstacles in a pattern are requested from the pool
at pattern spawn time, not individually timed — this prevents mid-pattern
pool stalls.

---

## 4. Formulas

### Dynamic Spawn Distance
```
minSpawnDistance = currentScrollSpeed × minimumLeadTime
spawnZ = playerZ + minSpawnDistance
```

### Inter-Pattern Interval
```
interPatternGap = baseGapForTier - (checkpointCount × gapReductionPerCheckpoint)
interPatternGap = Max(interPatternGap, minimumGap)
```

### Pattern Internal Spacing
```
obstacleSpacing = baseObstacleSpacing / currentScrollSpeed × referenceSpeed
```
Keeps intra-pattern spacing feeling consistent regardless of scroll speed.

### Difficulty Tier Weights
```
easyWeight   = Max(0, 1.0 - (checkpoint / easyFadeCheckpoint))
mediumWeight = Bell curve peaking at checkpoint 4
hardWeight   = Min(1.0, checkpoint / hardStartCheckpoint)
```
Weights are normalised before random selection.

---

## 5. Edge Cases

| Scenario | Behaviour |
|----------|-----------|
| Pattern pool exhausted (all filtered patterns recently used) | Extend exclusion window gradually until a pattern is available |
| Scroll speed changes mid-pattern (obstacle hit) | Already-spawned obstacles continue; spawn distance recalculated for next pattern |
| Player enters branching path during a pattern | Pattern obstacles on the non-taken path are returned to pool immediately |
| Moving obstacle period < player reaction window | Period clamped to minimum reaction window at all speeds |
| Gauntlet spawned immediately after another gauntlet | Safety rule prevents this; always enforces 3s recovery gap |
| Mission introduces jump for the first time | All patterns requiring jump are gated behind this mission's intro checkpoint |

---

## 6. Dependencies

| System | Dependency Type |
|--------|-----------------|
| Player Controller | Defines available actions; gates pattern requirements |
| Procedural Generation | Checkpoints trigger difficulty tier changes |
| Biome System | Provides active biome's prefab set and pattern library |
| Pool Manager | Obstacle instantiation and recycling |
| Difficulty Ramp GDD | Defines checkpoint intervals and tier transition thresholds |
| LevelGenerator | Provides current scroll speed for dynamic spawn distance |

---

## 7. Tuning Knobs

| Parameter | Suggested Default | Notes |
|-----------|-------------------|-------|
| easyInterPatternGap | 3.0s | Time between patterns at Easy tier |
| mediumInterPatternGap | 2.0s | Time between patterns at Medium tier |
| hardInterPatternGap | 1.2s | Time between patterns at Hard tier |
| minimumGap | 0.8s | Floor — never tighter than this |
| minLeadTimeSlow | 1.5s | Lead time at ≤ 8 m/s |
| minLeadTimeMid | 2.0s | Lead time at 8–12 m/s |
| minLeadTimeFast | 2.5s | Lead time at > 12 m/s |
| patternExclusionWindow | 2 | Recent patterns excluded from re-selection |
| gauntletPreGap | 2.0s | Breathing gap before a gauntlet |
| gauntletPostGap | 3.0s | Recovery gap after a gauntlet |
| easyFadeCheckpoint | 4 | Checkpoint where Easy weight reaches 0 |
| hardStartCheckpoint | 5 | Checkpoint where Hard patterns begin |

---

## 8. Acceptance Criteria

- [ ] At least one lane is always free for Ground Blocker and Double Blocker patterns
- [ ] Overhead obstacles always span the full path width
- [ ] Aerial obstacles always have sufficient jump clearance
- [ ] No pattern requiring jump appears before jump is introduced in the mission
- [ ] Dynamic spawn Z correctly scales with current scroll speed
- [ ] Player always has minimum lead time at all scroll speeds
- [ ] No two consecutive Overhead patterns are selected
- [ ] Pattern exclusion window prevents immediate repetition
- [ ] Difficulty tier transitions at correct checkpoint thresholds
- [ ] Gauntlets are always preceded and followed by enforced gap
- [ ] Moving obstacles have a visible, predictable travel path
- [ ] All obstacles are returned to pool correctly via ObstacleDestroy
- [ ] Each biome renders only its own prefab set
- [ ] Pattern library ScriptableObjects are editable without code changes
- [ ] Difficulty weights are tunable without code changes
