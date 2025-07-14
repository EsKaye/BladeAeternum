# BladeAeternum Script Reference

<details>
<summary><strong>CombatCore (BladeAeternum.Combat)</strong></summary>

**Purpose:** Handles core combat mechanics: slashing, blocking, and timing-based parries. Modular and event-driven for expansion.

**Serialized Fields:**
- `ParrySystem parrySystem` (private)

**Events:**
- `OnSlash` (Action)
- `OnBlock` (Action)
- `OnParryAttempt` (Action&lt;bool&gt;)

**Public Methods:**
- `void Slash()` – Perform a slash attack, triggers OnSlash.
- `void Block()` – Perform a block, triggers OnBlock.
- `void TryParry(float inputTime)` – Attempt a parry, triggers OnParryAttempt.

</details>

<details>
<summary><strong>ParrySystem (BladeAeternum.Combat)</strong></summary>

**Purpose:** Handles timing-based parry detection and triggers parry effects.

**Serialized Fields:**
- `float perfectParryWindow` (private)

**Events:**
- `OnParrySuccess` (Action)
- `OnParryFail` (Action)

**Public Properties:**
- `float LastAttackTime` (get/set)

**Public Methods:**
- `bool AttemptParry(float inputTime)` – Returns true if parry is successful, triggers events.

</details>

<details>
<summary><strong>GameManager (BladeAeternum.CoreManagers)</strong></summary>

**Purpose:** Manages the overall duel flow: intro, fight, and outcome. Singleton pattern.

**Enums:**
- `DuelState { Intro, Fight, Outcome }`

**Events:**
- `OnIntroStart` (Action)
- `OnFightStart` (Action)
- `OnDuelEnd` (Action&lt;bool&gt;)

**Public Properties:**
- `DuelState CurrentState` (get)
- `static GameManager Instance` (get)

**Public Methods:**
- `void StartDuel()` – Sets state to Intro, triggers OnIntroStart.
- `void BeginFight()` – Sets state to Fight, triggers OnFightStart.
- `void EndDuel(bool playerWon)` – Sets state to Outcome, triggers OnDuelEnd.

</details>

<details>
<summary><strong>WeaponUnlocker (BladeAeternum.CoreManagers)</strong></summary>

**Purpose:** Unlocks new blades upon player victory.

**Events:**
- `OnBladeUnlocked` (Action&lt;string&gt;)

**Public Methods:**
- `void UnlockBlade(string bladeName)` – Unlocks a blade, triggers OnBladeUnlocked.

</details>

<details>
<summary><strong>HUDController (BladeAeternum.UI)</strong></summary>

**Purpose:** Controls the Heads-Up Display (HUD) for health, stamina, and blade energy.

**Serialized Fields:**
- `Slider healthBar` (private)
- `Slider staminaBar` (private)
- `Slider bladeEnergyBar` (private)

**Events:**
- `OnHealthChanged` (Action&lt;float&gt;)
- `OnStaminaChanged` (Action&lt;float&gt;)
- `OnBladeEnergyChanged` (Action&lt;float&gt;)

**Public Methods:**
- `void UpdateHealth(float value)` – Updates health bar, triggers OnHealthChanged.
- `void UpdateStamina(float value)` – Updates stamina bar, triggers OnStaminaChanged.
- `void UpdateBladeEnergy(float value)` – Updates blade energy bar, triggers OnBladeEnergyChanged.

</details>

<details>
<summary><strong>LoreManager (BladeAeternum.Lore)</strong></summary>

**Purpose:** Delivers pre-duel lore messages and narrative context.

**Serialized Fields:**
- `string[] preDuelMessages` (private)

**Events:**
- `OnShowMessage` (Action&lt;string&gt;)

**Public Methods:**
- `void ShowPreDuelMessage()` – Shows the current pre-duel message, triggers OnShowMessage.

</details>

<details>
<summary><strong>EnemyAI (BladeAeternum.Characters)</strong></summary>

**Purpose:** Simple pattern-based boss combat AI.

**Serialized Fields:**
- `CombatCore combatCore` (private)

**Events:**
- `OnPatternStart` (Action)

**Public Methods:**
- `void StartPattern()` – Starts the attack pattern coroutine, triggers OnPatternStart.

</details> 