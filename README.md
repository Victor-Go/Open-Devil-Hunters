# Devil Hunters

## Executive Summary

**Devil Hunters** is a high-performance, top-down Action Roguelike built in Unity, now available on [Steam](https://store.steampowered.com/app/2306050). 

Engineered from the ground up for scale, the codebase demonstrates a mature, strictly decoupled architecture that prioritizes predictable state mutation and zero-allocation frame cycles. This repository serves as a technical portfolio showcasing enterprise-grade design patterns applied within a real-time engine environment.

## Technical Highlights

The engine architecture is defined by the following core design choices, implemented to handle high-density entity requirements without performance degradation:

*   **State-Driven Decoupling:** Implemented a custom Flux/Redux-inspired state management system (`StoreManager`) to strictly separate Game Logic, View, and Data layers. 
*   **Zero-GC in Hot Paths:** The physical movement layer (`MovementControl`) is engineered to bypass generic boxing allocations. By utilizing direct memory and array references for high-frequency updates, the system guarantees zero heap allocation per frame during the `FixedUpdate` cycle.
*   **High-Performance Spatial Queries:** Utilized pre-allocated buffers (`Collider2D[]`) and `Physics2D.OverlapCircleNonAlloc` exclusively across all combat systems to eliminate memory churn.
*   **Advanced Object Pooling:** Built a robust, string-mapped Object Pool that manages thousands of concurrent entities without runtime instantiation, eliminating closure-based memory leaks via method groups.

## Project Structure & Navigation

Reviewers can find the implementation of the core architectural pillars in the following locations:

*   **State Management:** `Assets/Code/Scripts/Src/Store/` — The Redux-style store, actions, and state definitions.
*   **Zero-GC Logic:** `Assets/Code/Scripts/Behaviour/Character/Player/MovementControl.cs` — Direct memory position updates.
*   **Pooling System:** `Assets/Code/Scripts/Src/ObjectPool.cs` — The core entity lifecycle manager.
*   **NonAlloc Physics:** `Assets/Code/Scripts/Behaviour/Skill/Bullets/ArtilleryShell.cs` — Implementation of pre-allocated buffer spatial queries.

> For a deep dive into the design philosophy and extensibility, see [Architecture & Development Guide](Architecture_And_Development_Guide.md).

## Marketplace

*   **Steam:** [Purchase and Play Devil Hunters](https://store.steampowered.com/app/2306050)

## License & Copyright

This repository follows a **"Split-License"** model to balance open-source technical contribution with commercial asset protection:

*   **Source Code:** All C# scripts and logic within this repository are licensed under the **GNU General Public License v3.0 (GPL-3.0)**. 
*   **Artistic Assets:** All visual assets (Sprites, Animations, UI designs) and audio assets (Music, Sound Effects) are **Proprietary and Copyright (c) 2026 Victor Yeung**. 
    *   **Strictly Prohibited:** Any secondary use, redistribution, modification, re-uploading, or incorporation of these artistic assets into other projects (commercial or non-commercial) is strictly prohibited without explicit written consent from the author.
    *   **Permitted Use:** These assets are provided solely for the purpose of compiling and running this specific project for technical review and educational assessment.

## Getting Started

### Prerequisites
*   **Unity 2022.3 LTS** (or newer)
*   **Git LFS** (Required to fetch binary metadata)

### Opening the Project
1.  Clone the repository to your local machine.
2.  Open **Unity Hub**.
3.  Click **Add Project from disk** and select the root directory.
4.  Open the main level scene located in `Assets/Scenes/`.
5.  Press **Play** to run the game.