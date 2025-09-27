# Unity Game Development Guidelines

This document outlines **best practices and coding standards** for Unity projects using C#. It is designed for both human developers and AI coding agents (JetBrains AI Assistant, Junie AI) to ensure **consistency, readability, maintainability, and performance** across Unity projects.

---

## 📑 Table of Contents
1. [Core Coding Standards](#core-coding-standards)
2. [MonoBehaviour Lifecycle](#monobehaviour-lifecycle)
3. [Component & Script Design](#component--script-design)
4. [Performance Optimization](#performance-optimization)
5. [Debugging & Logging](#debugging--logging)
6. [Documentation & Comments](#documentation--comments)
7. [Asset Management](#asset-management)
8. [UI Development](#ui-development)
9. [Audio Management](#audio-management)
10. [Animation](#animation)
11. [Input Systems](#input-systems)
12. [Networking](#networking)
13. [Memory & Jobs System](#memory--jobs-system)
14. [Shaders & Rendering](#shaders--rendering)
15. [Testing & Validation](#testing--validation)
16. [Version Control](#version-control)
17. [Editor & Tooling](#editor--tooling)
18. [Developer Workflow](#developer-workflow)
19. [Examples & Templates](#examples--templates)
20. [Common Pitfalls](#common-pitfalls)

---

## Core Coding Standards
- Follow **Microsoft’s C# conventions** (PascalCase for public, camelCase for private).
- One script = one class. File name must match class name.
- Use **namespaces aligned with folder structure** (`ProjectName.Systems`).
- Avoid magic numbers/strings. Use constants or `[SerializeField]`.
- Explicitly set **Script Execution Order** in Project Settings if needed.
- Cache references in `Awake` or `Start` — avoid expensive calls in `Update`.

**Checklist:** ✅ Naming follows C# conventions ✅ No magic numbers ✅ No `FindObjectOfType` in Update ✅ Script name matches class

---

## MonoBehaviour Lifecycle
- **Awake** → Cache references.
- **Start** → Initialize logic dependent on other components.
- **Update/FixedUpdate** → Keep lightweight; prefer coroutines/events.
- Use `[SerializeField]` for private fields visible in Inspector.
- Use `[RequireComponent]` to enforce dependencies.
- Expose runtime data via **properties**, not public fields.

---

## Component & Script Design
- Apply **Single Responsibility Principle**.
- Use **events/delegates** for loose coupling.
- Always **cache components** instead of repeated `GetComponent`.
- Add `[Tooltip]` for all Inspector-exposed fields.
- Avoid serializing large, complex structures.

---

## Performance Optimization
- **Object Pooling** for frequently spawned/destroyed objects.
- Use **coroutines** for timed logic instead of counters in Update.
- Physics → Use **FixedUpdate** with `Time.fixedDeltaTime`.
- Use **batching & material instancing** for rendering.
- Profile regularly using Unity Profiler, Memory Profiler, Frame Debugger.

---

## Debugging & Logging
- Always include **context in logs** (`gameObject.name`, `this`).
- Use `Debug.LogError` for critical, `Debug.LogWarning` for non-critical.
- Use `#if UNITY_EDITOR` for debug-only code.
- Use `Debug.Assert` for assumptions.

---

## Documentation & Comments
- Comment **intent**, not obvious behavior.
- Use `// noop` for intentionally empty methods.
- Add **XML documentation** for public classes/methods.

---

## Asset Management
- Use **ScriptableObjects** for shared data.
- Prefer **Addressables** over `Resources.Load`.
- Use **prefabs** for reusable GameObjects.
- Keep prefab hierarchies clean and consistent.

---

## UI Development
- Use **CanvasScaler** with reference resolution for multiple screen sizes.
- Use **TextMeshPro** instead of legacy `Text`.
- Only one **EventSystem** per scene.
- Use **Layout Groups** instead of manual positioning.
- Optimize UI → Minimize overdraw, use `CanvasGroup` for visibility.
- Ensure **keyboard/gamepad accessibility**.

---

## Audio Management
- Use one `AudioSource` per GameObject for looping sounds.
- Pool `AudioSource` for one-shot effects.
- Store clips in assets or ScriptableObjects.
- Use compressed formats for music, WAV for SFX.
- Implement **centralized AudioManager** for volume control.
- Always stop looping audio in `OnDestroy`.

---

## Animation
- Use `[RequireComponent(typeof(Animator))]`.
- Cache Animator in `Awake`.
- Use **triggers** for one-time animations.
- Use **Animation Events** for callbacks.
- Use **Blend Trees** for smooth transitions.
- Use **AnimatorOverrideController** for dynamic swaps.

---

## Input Systems
- Use the **Input System package** (not legacy `Input`).
- Define input in `InputActionAsset`.
- Use callbacks (`performed`, `canceled`) instead of polling.
- Support multiple devices (keyboard, gamepad, touch).
- Validate unhandled input in dev builds.

---

## Networking
- Use **Unity Netcode for GameObjects**, Unity Transport, or Mirror.
- Sync states efficiently — avoid spamming Update.
- Implement **latency compensation** for gameplay.
- Define clear **server/client responsibilities**.

---

## Memory & Jobs System
- Avoid excessive heap allocations in hot paths.
- Prefer `structs` for small value types.
- Use **Unity Job System + Burst** for heavy calculations.
- Be mindful mixing `async/await` with Unity’s main thread.

---

## Shaders & Rendering
- Use **Shader Graph** where possible, HLSL for advanced cases.
- Follow consistent naming conventions.
- Use **material instancing** to reduce draw calls.
- Avoid editing materials at runtime.

---

## Testing & Validation
- Use **NUnit** for unit tests (non-MonoBehaviour).
- Place tests in `Editor/` folder.
- Validate scene setups in `Awake`/`Start`.

---

## Version Control
- `.gitignore` → Exclude `Library/`, `Temp/`, `*.csproj`.
- Use **YAML serialization** for scenes & prefabs.
- Use **Unity Smart Merge**.
- Write descriptive commit messages.

---

## Editor & Tooling
- Place all **editor scripts** in `Editor/` folders.
- Use **custom inspectors** for complex components.
- Use attributes (`[Range]`, `[ContextMenu]`) for usability.
- Avoid heavy `[ExecuteInEditMode]` usage.

---

## Developer Workflow
- Follow **branching strategy** (Gitflow or trunk-based).
- Require **code reviews** for all merges.
- Run profiler before final commit.
- Keep commit history clean and meaningful.

---

## Examples & Templates
- Provide **boilerplate MonoBehaviour** with proper setup.
- Example UI prefab with clean hierarchy.
- Example AudioManager with pooling.

---

## Common Pitfalls
❌ Don’t use `Update()` for timers — use coroutines.
❌ Don’t hardcode layer names/tags.
❌ Don’t use `GameObject.Find` or `FindObjectOfType` at runtime.
❌ Don’t make fields public just for Inspector visibility — use `[SerializeField]`.
❌ Don’t modify shared materials at runtime.

---

# ✅ Final Notes
- Keep scripts small, focused, and self-explanatory.
- Optimize early for clarity, later for performance.
- Always profile before optimizing.
- Maintain consistency across the entire project.

