# Architecture & Development Guide

## The Architecture Dilemma & Solution

The central architectural challenge in building Devil Hunters was reconciling "State-Driven Decoupling" with the "High-Performance Unity Lifecycle." 

In enterprise software, the Flux/Redux pattern is the gold standard for predictable state mutation. By isolating state into distinct stores and requiring all changes to flow through an event bus (`StoreManager`), the Game Logic, View, and Data layers remain strictly decoupled. This prevents tangled `MonoBehaviour` dependencies and allows the application to scale infinitely. 

However, Unity operates on a high-frequency, frame-based lifecycle (`Update` and `FixedUpdate`). In C#, wrapping structurally typed value data (like a `PlayerPositionData` struct) into a generic event interface (`IActionData`) forces a boxing allocation. When the engine dispatches position updates up to 144 times a second per entity, this pure Redux approach generates massive memory allocations on the managed heap, leading to Garbage Collection (GC) spikes and frame stuttering.

### Pragmatic Architecture

To solve this, the architecture makes an intentional compromise. The pure Redux pattern is strictly enforced for low-frequency, discrete events (e.g., leveling up, inventory changes, UI state). 

Conversely, for continuous high-frequency data streams like the physical movement layer, the architecture intentionally bypasses the generic event bus interface. The `MovementControl` layer holds direct references to the unboxed `playerPositions` array backing the store. This allows the system to mutate state by directly writing vector data into pre-allocated memory. The result is a system that maintains the global visibility and structural integrity of the Redux pattern while executing at zero bytes of allocation per frame.

## Execution Flow

Data flows unidirectionally through the engine:

1. **Input / Physics Engine:** The user provides input via the hardware, or the Unity `FixedUpdate` loop triggers a physics callback.
2. **Action Dispatch:** The controller components generate a domain-specific Action (e.g., `SET_COOLING_COUNTDOWN`).
3. **StoreManager (Event Bus):** The action and its payload are dispatched to the `StoreManager`.
4. **Reducers (Stores):** The corresponding `IStore` implementation calculates the new State based on the incoming Action and writes it.
5. **Observers (View):** UI components and visual renderers subscribed to the `StoreManager` receive an `OnStoreChanged` callback and reactively update their presentation to match the new State.

## Extensibility Guide (Onboarding)

The decoupled architecture allows contributors to add new content without modifying the core engine or touching internal state machines. 

### Adding a New Enemy Type

1. **Define Configurations:** Open the `EnemyConfigurations` file located in `Assets/Code/Scripts/Src/Configurations`. Add a new configuration entry defining the enemy's Health Points, Speed, and Hit Points.
2. **Create the Prefab:** Duplicate a base enemy prefab in the Unity Editor. Attach the generic `GeneralEnemy` (or create a specific implementation inheriting from `Enemy`) component. 
3. **Bind Configuration:** Assign the newly created configuration ID to the enemy component in the inspector.
4. **Register with the Object Pool:** Ensure the new prefab is registered in the global string-mapped Object Pool initialization to allow zero-allocation spawning during gameplay.
5. **Update Factory:** Register the spawn conditions in `EnemyFactoryConfigurations` to dictate when and where the spawner should yield the new enemy.

### Adding a New Player Ability

1. **Create the Skill Script:** Create a new class inheriting from `PlayerActiveSkill`. Implement the `Launch(int playerNumber)` method using method groups and the `Scheduling` utility to execute the ability logic over time. Do not use lambda closures.
2. **Define the Data Payload:** If the skill requires custom configurations, create a new class inheriting from `ActiveSkillConfigurations`.
3. **Hook into the Event Bus:** If the skill interacts with the player's status (e.g., granting a temporary speed boost), dispatch the appropriate action (e.g., `PlayerStore_SET_MOVING_SPEED`) to the `StoreManager` rather than modifying the `PlayerControl` component directly.
4. **Register the Prefab & Configuration:** Assign the script to a new skill prefab and map it inside the `SkillConfigurations` registry. The generic Input layer will automatically trigger your skill's `Launch` method when the player equips the skill and presses the assigned input binding.
