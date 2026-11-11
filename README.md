## 📂 Repository Structure

```text
Assets/
├── _Project/
│   ├── Animations/         # All .anim and Animator Controllers
│   ├── Art/                # 3D Models, Materials, Textures
│   ├── Audio/              # Sound effects and music
│   ├── Prefabs/            # ALL game objects must become prefabs!
│   │   ├── Enemies/
│   │   ├── Player/
│   │   └── Environment/
│   ├── Scenes/
│   │   ├── MainMenu.unity
│   │   ├── Level_01.unity
│   │   └── TestScenes/     # Personal sandboxes (e.g., Dawid_Test.unity)
│   ├── Scripts/
│   │   ├── Core/           # GameManager, RevolutionController
│   │   ├── Player/         # PlayerMovement, WeaponController
│   │   ├── Enemies/        # EnemyAI, WaypointPatrol
│   │   └── UI/             # UIManager, Menus
│   └── Settings/           # Input Actions, Physics Materials
└── Plugins/                # Let Unity put 3rd party assets here
```

## ⚠️ The 3 Golden Rules for Unity + GitHub

1. **The `.gitignore` is Mandatory:**
   Before your very first commit, you must include a standard Unity `.gitignore` file at the root of the repository. You should **only** commit the `Assets` and `ProjectSettings` folders. Never commit the `Library`, `Logs`, `obj`, or `Temp` folders.

2. **Always Commit `.meta` Files:**
   Every file and folder in Unity generates a `.meta` file (e.g., `PlayerMovement.cs` generates `PlayerMovement.cs.meta`). This file stores the GUID that Unity uses to link scripts to GameObjects. **If you commit a file, you must commit its matching `.meta` file**, otherwise references will break for everyone else.

3. **Never Work in the Same Scene:**
   Unity Scenes are massive text files. If two people edit `Level_01.unity` at the same time, Git will create an unresolvable merge conflict, and someone's work will be lost.

# Time Revolution - Project Roadmap & Task Distribution

This roadmap is designed for a fast, 3-week development cycle. Since the team is strong in CS but new to Unity, **simplicity is key**. We will focus on a Minimum Viable Product (MVP) first, ensuring the core time loop and basic combat work before adding complex movement or extra weapons.

## 💡 Feature Ideas & Brainstorming

_Items checked [x] are confirmed for the MVP. Items unchecked [ ] are stretch goals._

**Core Mechanics & Time Manipulation**

- [x] **Time Rewind:** Player can reverse their position/state (already prototyped).
- [x] **Loop-Based Levels:** A strict timer (e.g., 60s) that resets the level if the goal isn't reached.
- [ ] **Checkpoint System:** The 60s loop is divided into goals; failing rolls back to the last checkpoint instead of the very beginning.

**Player Movement (Ultrakill-inspired)**

- [x] **Momentum Physics:** Rigidbody-based movement with friction, air control, and soft speed caps.
- [ ] **Player Dash:** Quick horizontal burst of speed.
- [ ] **Slide:** Crouch-gliding to maintain momentum and shrink hitbox.
- [ ] **Wall Jump:** Bouncing off walls for vertical mobility.

**Combat & Enemies**

- [x] **Basic Hitscan Weapon:** Instant-hit pistol/rifle.
- [x] **Basic Melee/Chaser Enemy:** Simple AI that runs at the player using NavMesh.
- [ ] **Ranged Enemy:** AI that stops to shoot projectiles at the player.
- [ ] **Multiple Weapons:** Ability to find and equip different guns in the level.

**Environment & VFX**

- [x] **Matrix-Style Environment:** (Chosen for simplicity). Simple glowing grids, floating cubes, and plain backgrounds. Highly stylized, minimal texture work.
- [x] **Time-Warp VFX:** Screen distortion or color shift when rewinding time.
- [ ] **Dynamic Player VFX:** Movement dust, speed lines during high-speed movement.
- [ ] **Destructible Environment:** Shooting specific glass or explosive barrels.

**UI / UX**

- [x] **Core HUD:** Health bar, ammo counter, and crosshair.
- [ ] **The "Time Clock":** A prominent, stylized clock tied to the loop and rewind power.
- [ ] **Hit Markers & Damage Numbers:** Popping out of enemies for arcade feel.

## 🌍 Environment Ideas

To save time on 3D modeling and texturing, we need to pick a theme that relies on simple geometry:

1. **In the Matrix (Recommended):** The absolute easiest for a short deadline. Simple glowing grids, floating cubes, and a plain background. Highly stylized without needing realistic textures.
2. **Prison:** Simple corridors, repeating cell blocks, and concrete textures.
3. **Destroyed City:** Harder to make look good quickly, requires a lot of rubble and broken geometry.

---

## 👨‍💻 Team Responsibilities & Core Scripts (Simplified)

### Dawid (Player Controller, Rewind, & Weapons)

**Focus:** Physics, game feel, and the core "Time" mechanic.

- `PlayerMovement.cs`: Handles Rigidbody WASD, jumping, and friction. _(Stretch: Add Dash, Slide, Wall Jump here later)._
- `RevolutionController.cs`: The time-rewind mechanic (recording positions and replaying them).
- `Weapon.cs`: A single, simple hitscan shooting script. _(Stretch: Expand to `WeaponManager` if we add multiple guns)._

### Ivan (Enemy Systems & AI)

**Focus:** Giving the player something to shoot at using simple logic.

- `EnemyAI.cs`: Simple Unity NavMesh script. If player is close -> Chase. If player is visible -> Shoot/Attack.
- `Health.cs`: A universal script attached to enemies (and the player) to handle taking damage and dying.

### Hedi (VFX & 3D Level Design)

**Focus:** Building the arena and making the game look impactful.

- `LevelBuilder`: (Not a script, but Editor work) Blocking out the chosen environment using Unity ProBuilder or basic cubes.
- `SimpleVFX.cs`: A script to spawn particle prefabs (sparks, blood, time-glitches) at a specific location when called by other scripts.
- `LevelTrigger.cs`: Invisible boxes in the level that load the next scene or spawn enemies when the player walks into them.

### Mitica (User Interface & Game Loop)

**Focus:** The 60s timer, checkpoints, and making the screen look like a game.

- `GameLoopManager.cs`: The most important script. Tracks the 60s timer, resets the scene if time runs out, and manages the Checkpoint data.
- `UIManager.cs`: Updates the UI text/bars. Reads data from the Player and GameLoopManager.
- `ClockUI.cs`: _(Stretch)_ Specifically handles animating the visual clock on the screen based on time left.

---

## 📅 Plan of Action (3-Week Timeline)

**Rule #1:** Do not start on Phase 2 until Phase 1 is 100% playable.

| Phase                                  | Goal                        | Key Tasks & Scripts                                                                                                                                                                                                    | Who |
| :------------------------------------- | :-------------------------- | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :-- |
| **Phase 1: MVP (Week 1)**              | A playable, ugly prototype. | - Basic movement & shooting (`PlayerMovement`, `Weapon`)<br>- Rewind works smoothly (`RevolutionController`)<br>- One dumb enemy that follows you (`EnemyAI`)<br>- 60s Timer that resets the scene (`GameLoopManager`) | All |
| **Phase 2: Core Loop (Week 2)**        | It feels like a real game.  | - Build the actual Level geometry (Matrix/Prison)<br>- Add UI: Health bars, ammo, basic timer text (`UIManager`)<br>- Add Win/Loss conditions & Checkpoints<br>- Enemies can now damage the player                     | All |
| **Phase 3: Polish & Stretch (Week 3)** | Making it cool.             | - Advanced Movement (Slide, Dash, Wall Jump)<br>- VFX (Muzzle flashes, sparks, time-warp effects)<br>- Cool UI Clock and Hit Markers<br>- Extra weapons if time permits                                                | All |

## 📅 Feature Implementation Table

This table breaks down the work into 3 Phases. **Phase 1 (MVP)** is mandatory for the presentation. **Phase 2 (Polish)** makes the game feel good. **Phase 3 (Stretch Goals)** are extras we add only if time permits.

| Feature / Module             | Priority / Phase  | Sub-features                                                              | Included Files / Assets                                    | Assignee(s)    |
| :--------------------------- | :---------------- | :------------------------------------------------------------------------ | :--------------------------------------------------------- | :------------- |
| **Basic Movement & Physics** | Phase 1 (MVP)     | Rigidbody Setup, WASD, Jump, Ground Detection, Camera Input               | `PlayerMovement.cs`, `PlayerInputHandler.cs`               | Dawid          |
| **Time Loop Manager**        | Phase 1 (MVP)     | 60s Timer, Game Over trigger, Scene Reloading                             | `GameLoopManager.cs`                                       | Mitica         |
| **Basic Combat**             | Phase 1 (MVP)     | Raycast shooting, damage calculation, player/enemy death                  | `WeaponController.cs`, `PlayerHealth.cs`, `EnemyHealth.cs` | Dawid & Ivan   |
| **Enemy AI System**          | Phase 1 (MVP)     | NavMesh baking, Patrol state, Chase player state                          | `EnemyAI.cs`, `WaypointPatrol.cs`                          | Ivan           |
| **Time Rewind**              | Phase 2 (Polish)  | Record position/velocity arrays, trigger rewind state, ghost momentum fix | `RevolutionController.cs`                                  | Dawid          |
| **Level Blockout**           | Phase 2 (Polish)  | Arena design, wall placement, colliders, basic Matrix materials           | `LevelTrigger.cs`, Matrix Materials                        | Hedi & Mitica  |
| **Core VFX & Feedback**      | Phase 2 (Polish)  | Muzzle flashes, hit particles, rewind screen filter                       | `VFXManager.cs`, Post-Processing Profile                   | Hedi           |
| **HUD & Menus**              | Phase 2 (Polish)  | Canvas setup, Health/Ammo UI, Main Menu scene, Pause Menu                 | `UIManager.cs`, `MainMenuController.cs`                    | Mitica         |
| **Advanced Movement**        | Phase 3 (Stretch) | Sliding, Dashing, Wall-Jumping, Jump Buffering                            | `PlayerMovement.cs` (Updates)                              | Dawid          |
| **Advanced Level/Loop**      | Phase 3 (Stretch) | Checkpoint logic, time penalty vs full reset                              | `CheckpointManager.cs`                                     | Mitica & Dawid |
| **Extra Weapons & AI**       | Phase 3 (Stretch) | Weapon pickups, Ranged enemies, Destructible props                        | `WeaponPickup.cs`, `DestructibleObject.cs`                 | Ivan & Hedi    |

# Unity Project File Structure & Git Guidelines

To prevent merge conflicts and keep the project organized, we use a **Type-Based** file structure rather than grouping folders by person. All project-specific files should be placed inside the `_Project` folder to separate our work from third-party plugins.
   - **The Workflow:** Work in your own personal sandbox scene in `Scenes/TestScenes/`. Build your feature (e.g., a new enemy or a UI canvas), turn it into a **Prefab**, and push the Prefab to GitHub. Then, one designated person places that Prefab into the main level scene.
