# Relic Rush — Player Controller GDD

**Version**: 0.1
**Date**: 2026-03-11
**Status**: Redesign (extends existing PlayerController.cs)
**Dependencies**: Obstacle System, Procedural Generation, Input System, Biome System

---

## 1. Overview

The player controller defines how the explorer moves through a biome run.
The player is stationary on the Z axis — the world scrolls toward them.
Control is lateral (left/right lane switching), vertical up (jump), and
vertical down (slide/crouch). A state machine governs which actions are
available in each state. Controls are designed for both PC (keyboard) and
mobile (swipe gestures) with equivalent feel and responsiveness on both
platforms.

---

## 2. Player Fantasy

The player should feel agile, responsive, and in control — like a trained
explorer who instinctively ducks, leaps, and sidesteps without breaking
stride. Every input should respond immediately. No input should feel "sticky"
or delayed. Near-misses should feel earned, not lucky.

---

## 3. Detailed Rules

### 3.1 Movement Model

The player GameObject is fixed on the Z axis. Chunks scroll toward the player
on the Z axis at a speed controlled by LevelGenerator. The player moves only
on the X axis (lane switching) and Y axis (jump/slide).

The play area is divided into 3 implicit lanes:
- Left lane:   X = -laneOffset
- Centre lane: X = 0
- Right lane:  X = +laneOffset

The player is not snapped to lanes — movement is continuous and physics-based.
The xClamp prevents the player from leaving the path boundaries.

### 3.2 Player States

The player is always in exactly one of the following states:

| State   | Entry Condition                      | Exit Condition                  |
|---------|--------------------------------------|---------------------------------|
| Running | Default / landing / slide end        | Jump input, Slide input, or Hit |
| Jumping | Jump input while Grounded            | Lands on ground                 |
| Sliding | Slide input while Running or Jumping | Slide duration expires          |
| Hit     | Collision with obstacle (cooldown)   | Hit animation completes         |

**State rules:**
- Jump is only available from Running state (not from Sliding)
- Slide is available from Running or Jumping (mid-air slide = fast descent)
- Lateral movement (left/right) is available in ALL states
- Hit state does not block movement — player can still steer during hit recovery
- States cannot be stacked; entering a new state cancels the previous one
  (except Hit, which overlays on top of the current movement state)

### 3.3 Jump

- Player applies an upward impulse force to the Rigidbody on jump input
- Gravity pulls the player back down naturally
- One jump allowed per grounded contact (no double jump)
- Jump arc is parabolic (physics-driven, not animated)
- Left/right steering remains active during jump
- Camera FOV increases slightly on jump initiation (speed feel reinforcement)
- Ground detection uses a short downward Raycast from the player's feet

**Jump and obstacles:**
- Jumping clears ground-level obstacles (barrels, roots, low walls)
- Jumping does not clear overhead obstacles (hanging beams, archways)
- Some paths are only accessible by jumping (elevated branching route)

### 3.4 Slide

- On slide input, the player's collider height is halved (crouching hitbox)
- Slide lasts for a fixed duration (slideDuration)
- After duration expires, collider returns to full height automatically
- Slide cannot be cancelled early by the player
- If slide input is given mid-air (Jumping state), it triggers a fast descent:
  - Adds downward force to Rigidbody, increasing fall speed
  - Enters Sliding state on landing immediately
- Left/right steering remains active during slide

**Slide and obstacles:**
- Sliding passes under overhead obstacles (beams, archways, low ceilings)
- Sliding does not clear ground-level obstacles
- Some paths are only accessible by sliding (low tunnel branching route)

### 3.5 Collision Behaviour (existing, preserved)

- Collision with an obstacle triggers the Hit state
- Speed reduction is applied to chunk scroll speed (not player lateral speed)
- Collision cooldown prevents repeated rapid hits from the same obstacle
- Hit animation plays; player retains lateral control during recovery
- Negative pickup effects (biome-specific) are handled separately by the
  Gadget/Pickup System, not by this controller

### 3.6 PC Controls

| Action    | Primary        | Alternative  |
|-----------|----------------|--------------|
| Move Left  | A              | Left Arrow   |
| Move Right | D              | Right Arrow  |
| Jump       | Space          | Up Arrow / W |
| Slide      | Left Shift / S | Down Arrow   |

### 3.7 Mobile Controls

| Action     | Gesture     |
|------------|-------------|
| Move Left  | Swipe Left  |
| Move Right | Swipe Right |
| Jump       | Swipe Up    |
| Slide      | Swipe Down  |

**Mobile design rules:**
- Swipe detection uses a minimum swipe distance threshold to prevent
  accidental inputs from small finger movements
- Swipe direction is determined at gesture end, not during gesture
- Simultaneous lateral + vertical swipes resolve to the dominant axis
  (larger delta wins)
- No on-screen buttons — swipe-only controls for clean visual field

### 3.8 Hitbox Specifications

| State   | Collider Height | Collider Centre (Y offset) |
|---------|-----------------|----------------------------|
| Running | Full (1.8 units) | 0.9                       |
| Jumping | Full (1.8 units) | dynamic (follows Y pos)   |
| Sliding | Half (0.9 units) | 0.45                      |

Collider width and depth remain constant across all states.

### 3.9 Path Branching Interaction

When a branching route fork appears in a chunk:
- **Elevated path** (upper route): requires a jump to reach the entry ramp.
  Higher risk (more obstacles), higher reward (more artifacts / gadget fragments).
- **Low path** (tunnel route): requires a slide to enter the tunnel entrance.
  Narrower, darker, fewer pickups but fewer obstacles.
- **Centre path** (default): always accessible with no special input.
  Balanced obstacle density and pickup rate.

The player's state at the fork point determines which path they can access.
If no special input is made, the player follows the centre path.

---

## 4. Formulas

### Lateral Movement
```
newX = currentX + (input.x × lateralMoveSpeed × fixedDeltaTime)
newX = Clamp(newX, -xClamp, +xClamp)
```

### Jump Impulse
```
jumpForce = Mathf.Sqrt(2 × gravity × jumpHeight)
Applied as: rigidbody.AddForce(Vector3.up × jumpForce, ForceMode.Impulse)
```

### Fast Descent (mid-air slide)
```
rigidbody.AddForce(Vector3.down × fastDescentForce, ForceMode.Impulse)
```

### Ground Detection
```
isGrounded = Physics.Raycast(
    feet position,
    Vector3.down,
    groundCheckDistance,
    groundLayer
)
```

---

## 5. Edge Cases

| Scenario | Behaviour |
|----------|-----------|
| Jump input while already Jumping | Ignored |
| Slide input while already Sliding | Ignored (resets duration — to be playtested) |
| Jump input while Sliding | Ignored (slide must complete first) |
| Hit during Jump | Hit overlay applies; jump arc continues uninterrupted |
| Hit during Slide | Hit overlay applies; slide continues uninterrupted |
| Player reaches xClamp boundary | Movement stops on that axis; other axes unaffected |
| Landing on a sloped surface | Raycast ground check must account for surface normal tolerance |
| Slide ends while under a low obstacle | Collider expansion deferred until space is clear (overlap check before restoring) |
| Fast descent on ground (swipe down while grounded) | Ignored — only triggers from Jumping state |

---

## 6. Dependencies

| System | Dependency Type |
|--------|-----------------|
| InputManager | Provides Move (Vector2), Jump, Slide input events |
| LevelGenerator | Controls chunk scroll speed (affected by collision) |
| Obstacle System | Defines obstacle hitbox sizes that the player collider must clear |
| Biome System | May apply movement modifiers (e.g., reduced lateral speed in mud biome) |
| Gadget / Pickup System | Negative pickup effects may temporarily modify controls |
| CameraController | Reacts to speed changes and jump for FOV adjustment |
| Animator | Driven by player state (Running, Jumping, Sliding, Hit) |

---

## 7. Tuning Knobs

All values are serialized in the Inspector for rapid iteration.

| Parameter | Suggested Default | Notes |
|-----------|-------------------|-------|
| lateralMoveSpeed | 5.0 | Units/sec lateral movement |
| xClamp | 3.0 | Half-width of play area |
| jumpHeight | 2.0 | Target apex height in units |
| gravity | 9.81 | Can override Physics gravity per-scene |
| slideDuration | 0.8s | Duration of slide state |
| fastDescentForce | 15.0 | Downward impulse for mid-air slide |
| groundCheckDistance | 0.15 | Raycast length for ground detection |
| collisionCooldown | 1.0s | Minimum time between obstacle hits |
| speedChangeDuration | 2.0s | Duration of speed reduction from hit |
| adjustChangeMoveSpeedAmount | -2.0 | Speed reduction per hit |

---

## 8. Acceptance Criteria

- [ ] Player can move left and right continuously; X position is clamped
- [ ] Jump input from Running state launches player upward with correct arc
- [ ] Player cannot jump while already airborne
- [ ] Slide reduces collider height; player passes under overhead obstacles
- [ ] Slide cannot be triggered from Sliding state (no re-trigger)
- [ ] Mid-air slide input triggers fast descent and enters Sliding on land
- [ ] Left/right movement works in all states (Running, Jumping, Sliding, Hit)
- [ ] Hit state triggers animation and speed reduction without blocking movement
- [ ] PC controls (WASD + Space + Shift) all function correctly
- [ ] Mobile swipe controls map correctly to all four actions
- [ ] Dominant-axis swipe resolution works on mobile (no ambiguous diagonals)
- [ ] Elevated path is accessible only by jumping at fork point
- [ ] Tunnel path is accessible only by sliding at fork point
- [ ] Centre path is always accessible without special input
- [ ] Collider correctly restores after slide only when overhead space is clear
- [ ] All tuning values are serialized and adjustable without code changes
