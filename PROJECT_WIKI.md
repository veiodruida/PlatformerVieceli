# 2D Platformer - Project Wiki

> **Versão do documento**: 1.0
> **Data de criação**: 2026-03-26
> **Engine**: Unity 6000.4.0f1
> **Render Pipeline**: URP 17.4.0

---

## 1. Visão Geral do Projeto

| Atributo | Valor |
|----------|-------|
| **Nome do Projeto** | 2D Platformer |
| **Versão do Unity** | 6000.4.0f1 (Unity 6) |
| **Render Pipeline** | Universal Render Pipeline (URP) 17.4.0 |
| **Sistema de Input** | Unity Input System (novo) |
| **Pacotes 2D** | com.unity.2d.animation, tilemap, sprteshape |
| **Gênero** | Platformer 2D |

### Descrição
Jogo de plataforma 2D com sistema de checkpoint, múltiplos tipos de inimigos, coletáveis, puzzles de chaves/portas, e UI completa com menus e pause.

---

## 2. Arquitetura do Sistema

### 2.1 Diagrama de Relações entre Sistemas

```
┌─────────────────────────────────────────────────────────────────────────┐
│                           GAME LOOP                                     │
└─────────────────────────────────────────────────────────────────────────┘
                                   │
        ┌──────────────────────────┼──────────────────────────┐
        ▼                          ▼                          ▼
┌───────────────────┐    ┌───────────────────┐    ┌───────────────────┐
│   PLAYER SYSTEM   │    │   ENEMY SYSTEM    │    │   PICKUP SYSTEM   │
│                   │    │                   │    │                   │
│ PlayerController  │    │ EnemyBase (abs)   │    │ Pickup (base)     │
│ GroundCheck       │    │ WalkingEnemy      │    │ ScorePickup       │
│ PlayerAnimator    │    │ FlyingEnemy       │    │ HealthPickup     │
│ Health            │    │ EnemyAnimator     │    │ KeyPickup         │
└───────────────────┘    └───────────────────┘    │ GoalPickup        │
        │                        │                  │ ExtraLifePickup   │
        ▼                        ▼                  └───────────────────┘
┌───────────────────┐    ┌───────────────────┐              │
│  KEYS & DOORS     │    │  CHECKPOINT       │              │
│                   │    │                   │              │
│ KeyRing (static)  │    │ Checkpoint        │              │
│ Door              │    │ CheckpointTracker │              │
└───────────────────┘    └───────────────────┘              ▼
                                      │            ┌───────────────────┐
                                      │            │   ENVIRONMENT     │
        ┌─────────────────────────────┼────────────┤                   │
        ▼                             ▼            │ PlatformAnimator  │
┌───────────────────┐    ┌───────────────────┐    │ WaypointMover     │
│  GAME MANAGER     │    │    UI MANAGER     │    │ PlayerChilder     │
│  (Singleton)      │    │   (Singleton)      │    │ CameraController  │
│                   │    │                   │    └───────────────────┘
│ Score/HighScore   │    │ Page Management   │
│ Game State        │    │ Pause System      │
│ Victory/GameOver  │    │ Effects           │
└───────────────────┘    └───────────────────┘
```

### 2.2 Padrões de Projeto Usados

| Padrão | Aplicação |
|--------|-----------|
| **Singleton** | GameManager, UIManager |
| **Static Class** | KeyRing, CheckpointTracker, LevelManager |
| **Herança OOP** | Pickup → (ScorePickup, HealthPickup, KeyPickup, GoalPickup, ExtraLifePickup) |
| **Herança OOP** | EnemyBase (abstract) → (WalkingEnemy, FlyingEnemy) |
| **Component-Based** | Arquitetura padrão Unity |
| **Observer** | UIelement ← GameManager (UpdateUIElements) |

### 2.3 Convenções de Código

- **Classes**: PascalCase (ex: `PlayerController`, `GameManager`)
- **Variáveis públicas**: PascalCase com `[Tooltip]` para documentação
- **Métodos**: PascalCase
- **Headers**: `[Header("Nome")]` para agrupar campos no Inspector
- **Comentários**: XML comments `/// <summary>` para classes públicas

---

## 3. Estrutura de Arquivos

```
Assets/
├── _Scenes/                          # Cenas do jogo
│   ├── Level1.unity
│   └── MainMenu.unity
│
├── Scripts/                          # Scripts C# do projeto
│   ├── Player/
│   │   ├── PlayerController.cs
│   │   ├── PlayerAnimator.cs
│   │   └── GroundCheck.cs
│   │
│   ├── Enemy/
│   │   ├── EnemyBase.cs              # Classe abstrata base
│   │   ├── WalkingEnemy.cs
│   │   ├── FlyingEnemy.cs
│   │   └── EnemyAnimator.cs
│   │
│   ├── Pickups/
│   │   ├── Pickup.cs                 # Classe base
│   │   ├── ScorePickup.cs
│   │   ├── HealthPickup.cs
│   │   ├── KeyPickup.cs
│   │   ├── GoalPickup.cs
│   │   └── ExtraLifePickup.cs
│   │
│   ├── Health&Damage/
│   │   ├── Health.cs
│   │   ├── Damage.cs
│   │   ├── Head.cs
│   │   └── DeathEffectAnimationHandler.cs
│   │
│   ├── Keys&Doors/
│   │   ├── KeyRing.cs                # Static class
│   │   └── Door.cs
│   │
│   ├── Environment/
│   │   ├── PlatformAnimator.cs
│   │   ├── WaypointMover.cs
│   │   └── PlayerChilder.cs
│   │
│   ├── Checkpoint/
│   │   ├── Checkpoint.cs
│   │   └── CheckpointTracker.cs      # Static class
│   │
│   ├── Camera/
│   │   └── CameraController.cs
│   │
│   ├── UI/
│   │   ├── UIManager.cs              # Singleton
│   │   ├── UIPage.cs
│   │   ├── LevelLoadButton.cs
│   │   ├── QuitGameButton.cs
│   │   ├── CursorChanger.cs
│   │   ├── HighlightFix.cs
│   │   └── UIelement/
│   │       ├── UIelement.cs         # Classe base
│   │       ├── ScoreDisplay.cs
│   │       ├── HealthDisplay.cs
│   │       ├── LivesDisplay.cs
│   │       └── HighScoreDisplay.cs
│   │
│   └── Utility/
│       ├── GameManager.cs            # Singleton
│       ├── LevelManager.cs           # Static class
│       ├── LevelSwitcher.cs
│       ├── TimedObjectDestroyer.cs
│       ├── ScreenshotUtility.cs
│       └── PlayerPrefsResetter.cs
│
├── Prefabs/                          # Prefabs do jogo
│   ├── Player/
│   │   └── Player.prefab
│   │
│   ├── Enemies/
│   │   ├── Basic/
│   │   │   ├── BasicWalkingEnemy.prefab
│   │   │   ├── BasicToughWalkingEnemy.prefab
│   │   │   └── BasicStaticEnemy.prefab
│   │   ├── SmartWalkingEnemy.prefab
│   │   └── WaypointEnemyParent.prefab
│   │
│   ├── Environment/
│   │   ├── Platforms/
│   │   │   ├── Static/ (Small, Medium, Large)
│   │   │   └── Moving/ (Small, Medium, Large)
│   │   ├── Doors/ (Red, Blue, Green, Pink, Orange, Grey, White)
│   │   ├── Checkpoint/
│   │   └── Partial/ (MovingPlatform, Waypoint)
│   │
│   ├── PickUps/
│   │   ├── ScorePickup.prefab
│   │   ├── HealthPickUp.prefab
│   │   ├── GoalPickup.prefab
│   │   ├── ExtraLifePickUp.prefab
│   │   └── Keys/ (todas as cores)
│   │
│   ├── UI/
│   │   ├── Canvases/ (Game UI Canvas, MainMenu UI Canvas)
│   │   ├── NumberTextDisplay.prefab
│   │   ├── PlayerImage.prefab
│   │   └── HealthImage.prefab
│   │
│   ├── Effects/
│   │   ├── WinLoseEffects/
│   │   ├── UIEffects/
│   │   ├── PickupEffects/
│   │   ├── HitEffects/
│   │   ├── DeathEffects/
│   │   └── CheckpointEffects/
│   │
│   ├── Managers/
│   │   ├── GameManager.prefab
│   │   └── UI Manager.prefab
│   │
│   ├── Music/
│   └── Utilities/
│
├── TileMapping/                     # Assets de tilemap
│   └── Pallets/
│       └── Full Tileset.prefab
│
└── Imported Custom Asset Packages/  # Assets de terceiros
    └── Developer Console/
```

---

## 4. Sistemas do Jogo

### 4.1 Player System

Responsável pelo controle e animação do personagem do jogador.

| Script | Descrição |
|--------|-----------|
| **PlayerController** | Gerencia movimento horizontal, pulo, detecção de solo, direção do sprite, e estados (Idle/Walk/Jump/Fall/Dead). Usa Unity Input System. |
| **GroundCheck** | Usa overlap de Collider2D para verificar se o player está em contato com camadas específicas ("Ground"). |
| **PlayerAnimator** | Lê o estado do PlayerController e define parâmetros booleanos no Animator (isIdle, isJumping, isFalling, isRunning, isDead). |

**Estados do Player (PlayerController.PlayerState)**:
- `Idle` - Player parado no chão
- `Walk` - Player se movendo no chão
- `Jump` - Player subindo (pulo ativo)
- `Fall` - Player caindo
- `Dead` - Player sem saúde

**Input Actions (Unity Input System)**:
- `moveAction` - Movimento horizontal (WASD / Arrow keys / Analog stick)
- `jumpAction` - Pulo (Space / South button)

---

### 4.2 Enemy System

Sistema orientado a objetos para diferentes tipos de inimigos.

| Script | Descrição |
|--------|-----------|
| **EnemyBase** | Classe abstrata base. Define `moveSpeed`, `enemyState` (Walking/Dead/Idle), e métodos virtuais `Start()`, `Update()`, `GetMovement()`, `MoveEnemy()`. |
| **WalkingEnemy** | Herda de EnemyBase. Anda em uma direção até bater em parede (wall test) ou borda (edge test), então vira. Requer GroundCheck para wall/edge detection. |
| **FlyingEnemy** | Herda de EnemyBase. Usa WaypointMover para mover-se entre waypoints. Sobrepõe Update para evitar movement padrão. |
| **EnemyAnimator** | Controla animações dos inimigos baseado no estado. |

**Estados do Inimigo (EnemyBase.EnemyState)**:
- `Walking` - Inimigo se movendo
- `Idle` - Inimigo parado
- `Dead` - Inimigo derrotado

---

### 4.3 Pickup System

Sistema de coletáveis com herança.

| Script | Descrição |
|--------|-----------|
| **Pickup** | Classe base. Detecta trigger com Player, cria efeito, destrói objeto. |
| **ScorePickup** | Herda de Pickup. Adiciona pontos via `GameManager.AddScore()`. |
| **HealthPickup** | Herda de Pickup. Cura o player via `Health.ReceiveHealing()`. |
| **KeyPickup** | Herda de Pickup. Adiciona key ID ao KeyRing. |
| **GoalPickup** | Herda de Pickup. Chama `GameManager.LevelCleared()` para vencer o nível. |
| **ExtraLifePickup** | Herda de Pickup. Adiciona vidas via `Health.AddLives()`. |

---

### 4.4 Health & Damage System

Sistema de saúde e aplicação de dano.

| Script | Descrição |
|--------|-----------|
| **Health** | Gerencia saúde, vidas, invencibilidade, respawn, morte. Suporta teamId para friendly fire prevention. |
| **Damage** | Aplica dano a componentes Health. Suporta trigger enter, trigger stay (DoT), e collision. |
| **DeathEffectAnimationHandler** | Ativa trigger "isDead" no Animator ao iniciar. |

**Configurações de Health**:
- `defaultHealth` - Saúde inicial
- `maximumHealth` - Saúde máxima
- `invincibilityTime` - Tempo de invencibilidade após dano
- `useLives` - Se usa sistema de vidas
- `respawnWaitTime` - Tempo antes de respawnar

---

### 4.5 Keys & Doors System

Sistema de puzzles com chaves e portas coloridas.

| Script | Descrição |
|--------|-----------|
| **KeyRing** | Static class. Gerencia HashSet de keyIDs coletados. Métodos: `AddKey()`, `HasKey(Door)`, `ClearKeyRing()`. |
| **Door** | Verifica se player tem a chave correta via KeyRing. Executa UnityEvents ao abrir/fechar. Suporta collider e trigger. |

**Fluxo**:
1. Player coleta KeyPickup → `KeyRing.AddKey(keyID)`
2. Player interage com Door → `Door.AttemptToOpen()`
3. Door verifica `KeyRing.HasKey(this)` → abre se verdadeiro

---

### 4.6 Environment System

Objetos ambientais do nível.

| Script | Descrição |
|--------|-----------|
| **PlatformAnimator** | Requer WaypointMover e Animator. Define parâmetro "isMoving" baseado no estado do mover. |
| **WaypointMover** | Move objeto entre waypoints. Suporta waitTime em cada waypoint. Properties: `waypoints`, `moveSpeed`, `waitTime`, `stopped`, `travelDirection`. |
| **PlayerChilder** | Faz player ser filho da plataforma quando em trigger (para movimento conjunto). Remove parent ao sair. |
| **CameraController** | Segue player com diferentes estilos: Locked, Overhead, DistanceFollow, OffsetFollow, BetweenTargetAndMouse. |

**Estilos de Câmera (CameraController.CameraStyles)**:
- `Locked` - Câmera fixa
- `Overhead` - Fica sobre o alvo
- `DistanceFollow` - Segue mantendo distância máxima
- `OffsetFollow` - Segue com offset fixo
- `BetweenTargetAndMouse` - Fica entre player e mouse

---

### 4.7 Checkpoint System

Sistema de save/respawn.

| Script | Descrição |
|--------|-----------|
| **Checkpoint** | Trigger que define posição de respawn. Ativa animação e efeito. |
| **CheckpointTracker** | Static class que mantém referência ao checkpoint atual. |

**Fluxo**:
1. Player entra em Checkpoint trigger
2. Checkpoint.SetRespawnPoint() atualiza posição
3. CheckpointTracker.currentCheckpoint指向 este checkpoint
4. Ao morrer, Health.Respawn() usa esta posição

---

### 4.8 UI System

Sistema completo de interface do usuário.

| Script | Descrição |
|--------|-----------|
| **UIManager** | Singleton. Gerencia páginas de UI, pause, efeitos de navegação/clique/voltar. |
| **UIPage** | Representa uma página de UI. Define defaultSelected para navigation. |
| **UIelement** | Classe base para elementos de UI que atualizam via UpdateUI(). |
| **ScoreDisplay** | Herda UIelement. Exibe score via GameManager.score. |
| **HealthDisplay** | Herda UIelement. Exibe health como imagens ou número. |
| **LivesDisplay** | Herda UIelement. Exibe vidas restantes. |
| **HighScoreDisplay** | Herda UIelement. Exibe high score. |
| **LevelLoadButton** | Botão para carregar cenas pelo nome. |
| **QuitGameButton** | Botão para sair do jogo (funciona em Editor e Build). |

---

### 4.9 Game Management System

Controle global do estado do jogo.

| Script | Descrição |
|--------|-----------|
| **GameManager** | Singleton. Gerencia score, high score, game state (victory/game over), persistência via PlayerPrefs. |
| **LevelManager** | Static class helper para mudança de cenas com Time.timeScale = 1. |

---

## 5. Referência de Scripts

### 5.1 Player Scripts

#### PlayerController.cs
**Caminho**: `Assets/Scripts/Player/PlayerController.cs`
**Descrição**: Controla movimento, pulo, direção do sprite e estados do player.
```
Variáveis públicas:
- groundCheck: GroundCheck - 检测地面
- spriteRenderer: SpriteRenderer - 控制翻转
- playerHealth: Health - 获取生命值
- movementSpeed: float (default: 4.0f)
- jumpPower: float (default: 10.0f)
- allowedJumps: int (default: 1)
- jumpDuration: float (default: 0.1f)
- jumpEffect: GameObject
- passThroughLayers: List<string>
- moveAction: InputAction
- jumpAction: InputAction

Propriedades:
- grounded: bool - 是否着地
- facing: PlayerDirection (Left/Right)
- state: PlayerState
```

#### GroundCheck.cs
**Caminho**: `Assets/Scripts/Player/GroundCheck.cs`
**Descrição**: Verifica se o objeto está em contato com camadas de solo.
```
Variáveis públicas:
- groundLayers: LayerMask
- groundCheckCollider: Collider2D
- landingEffect: GameObject

Métodos:
- CheckGrounded(): bool
- GetCollider(): void
```

#### PlayerAnimator.cs
**Caminho**: `Assets/Scripts/Player/PlayerAnimator.cs`
**Descrição**: Controla animações do player baseado no estado.
```
Variáveis públicas:
- playerController: PlayerController
- animator: Animator

Parâmetros do Animator:
- isIdle: bool
- isJumping: bool
- isFalling: bool
- isRunning: bool
- isDead: bool
```

---

### 5.2 Enemy Scripts

#### EnemyBase.cs
**Caminho**: `Assets/Scripts/Enemy/EnemyBase.cs`
**Descrição**: Classe abstrata base para todos os inimigos.
```
Variáveis públicas:
- moveSpeed: float (default: 2.0f)
- enemyState: EnemyState (Walking/Dead/Idle)

Métodos abstratos:
- Setup(): void
- GetMovement(): Vector3

Métodos virtuais:
- Update(): void
- MoveEnemy(Vector3): void
```

#### WalkingEnemy.cs
**Caminho**: `Assets/Scripts/Enemy/WalkingEnemy.cs`
**Descrição**: Inimigo que caminha até bater em parede ou borda.
```
Variáveis públicas:
- wallTestLeft: GroundCheck
- wallTestRight: GroundCheck
- leftEdge: GroundCheck
- rightEdge: GroundCheck
- walkDirection: WalkDirections (Right/Left/None)
- willTurnAroundAtEdge: bool
```

#### FlyingEnemy.cs
**Caminho**: `Assets/Scripts/Enemy/FlyingEnemy.cs`
**Descrição**: Inimigo voador que usa waypoints.
```
Variáveis públicas:
- waypointMover: WaypointMover

Métodos:
- CheckFlipSprite(): void
- SetStateInformation(): void
```

---

### 5.3 Pickup Scripts

#### Pickup.cs
**Caminho**: `Assets/Scripts/Pickups/Pickup.cs`
**Descrição**: Classe base para todos os coletáveis.
```
Variáveis públicas:
- pickUpEffect: GameObject

Métodos virtuais:
- DoOnPickup(Collider2D): void
```

#### ScorePickup.cs
**Caminho**: `Assets/Scripts/Pickups/ScorePickup.cs`
**Descrição**: Adiciona pontos ao ser coletado.
```
Variáveis públicas:
- scoreAmount: int (default: 1)
```

#### HealthPickup.cs
**Caminho**: `Assets/Scripts/Pickups/HealthPickup.cs`
**Descrição**: Cura o player.
```
Variáveis públicas:
- healingAmount: int (default: 1)
```

#### KeyPickup.cs
**Caminho**: `Assets/Scripts/Pickups/KeyPickup.cs`
**Descrição**: Adiciona chave ao KeyRing.
```
Variáveis públicas:
- keyID: int (default: 0)
```

#### GoalPickup.cs
**Caminho**: `Assets/Scripts/Pickups/GoalPickup.cs`
**Descrição**: Finaliza o nível.

#### ExtraLifePickup.cs
**Caminho**: `Assets/Scripts/Pickups/ExtraLifePickup.cs`
**Descrição**: Adiciona vida extra.
```
Variáveis públicas:
- extraLives: int (default: 1)
```

---

### 5.4 Health & Damage Scripts

#### Health.cs
**Caminho**: `Assets/Scripts/Health&Damage/Health.cs`
**Descrição**: Gerencia saúde, vidas, invencibilidade e morte.
```
Variáveis públicas:
- teamId: int (default: 0)
- defaultHealth: int (default: 1)
- maximumHealth: int (default: 1)
- currentHealth: int (default: 1)
- invincibilityTime: float (default: 3.0f)
- useLives: bool (default: false)
- currentLives: int (default: 3)
- maximumLives: int (default: 5)
- respawnWaitTime: float (default: 3.0f)
- deathEffect: GameObject
- hitEffect: GameObject
```

#### Damage.cs
**Caminho**: `Assets/Scripts/Health&Damage/Damage.cs`
**Descrição**: Aplica dano a componentes Health.
```
Variáveis públicas:
- teamId: int (default: 0)
- damageAmount: int (default: 1)
- destroyAfterDamage: bool (default: true)
- dealDamageOnTriggerEnter: bool (default: false)
- dealDamageOnTriggerStay: bool (default: false)
- dealDamageOnCollision: bool (default: false)
```

---

### 5.5 Keys & Doors Scripts

#### KeyRing.cs
**Caminho**: `Assets/Scripts/Keys&Doors/KeyRing.cs`
**Descrição**: Static class que gerencia chaves do player.
```
Métodos estáticos:
- AddKey(int): void
- HasKey(Door): bool
- ClearKeyRing(): void
```

#### Door.cs
**Caminho**: `Assets/Scripts/Keys&Doors/Door.cs`
**Descrição**: Porta que requer chave específica para abrir.
```
Variáveis públicas:
- doorID: int (default: 0)
- isOpen: bool (default: false)
- openEvent: UnityEvent
- closeEvent: UnityEvent
- doorOpenAndCloseEffect: GameObject
- doorLockedEffect: GameObject
```

---

### 5.6 Environment Scripts

#### WaypointMover.cs
**Caminho**: `Assets/Scripts/Environment/WaypointMover.cs`
**Descrição**: Move objeto entre waypoints com wait time.
```
Variáveis públicas:
- waypoints: List<Transform>
- moveSpeed: float (default: 1.0f)
- waitTime: float (default: 3.0f)
```

#### PlatformAnimator.cs
**Caminho**: `Assets/Scripts/Environment/PlatformAnimator.cs`
**Descrição**: Sincroniza animação com WaypointMover.

#### PlayerChilder.cs
**Caminho**: `Assets/Scripts/Environment/PlayerChilder.cs`
**Descrição**: Faz player seguir plataforma em movimento.

#### CameraController.cs
**Caminho**: `Assets/Scripts/Camera/CameraController.cs`
**Descrição**: Controla movimento da câmera.
```
Variáveis públicas:
- target: Transform
- cameraMovementStyle: CameraStyles
- maxDistanceFromTarget: float (default: 5.0f)
- cameraOffset: Vector2
- cameraZCoordinate: float (default: -10.0f)
```

---

### 5.7 Checkpoint Scripts

#### Checkpoint.cs
**Caminho**: `Assets/Scripts/Checkpoint/Checkpoint.cs`
**Descrição**: Define ponto de respawn.
```
Variáveis públicas:
- respawnLocation: Transform
- checkpointAnimator: Animator
- animatorActiveParameter: string
- checkpointActivationEffect: GameObject
```

#### CheckpointTracker.cs
**Caminho**: `Assets/Scripts/Checkpoint/CheckpointTracker.cs`
**Descrição**: Static class que mantém checkpoint atual.
```
Propriedade estática:
- currentCheckpoint: Checkpoint
```

---

### 5.8 UI Scripts

#### UIManager.cs
**Caminho**: `Assets/Scripts/UI/UIManager.cs`
**Descrição**: Singleton. Gerencia páginas de UI e pause.
```
Variáveis públicas:
- pages: List<UIPage>
- currentPage: int
- defaultPage: int
- pausePageIndex: int
- allowPause: bool
- navigationEffect: GameObject
- clickEffect: GameObject
- backEffect: GameObject
- pauseAction: InputAction
```

#### UIPage.cs
**Caminho**: `Assets/Scripts/UI/UIPage.cs`
**Descrição**: Página individual de UI.

#### UIelement.cs
**Caminho**: `Assets/Scripts/UI/UIelement/UIelement.cs`
**Descrição**: Classe base para elementos de UI.

#### ScoreDisplay.cs, HealthDisplay.cs, LivesDisplay.cs, HighScoreDisplay.cs
**Caminho**: `Assets/Scripts/UI/UIelement/`
**Descrição**: Displays específicos que herdam de UIelement.

#### LevelLoadButton.cs
**Caminho**: `Assets/Scripts/UI/LevelLoadButton.cs`
**Descrição**: Botão para carregar cenas.

#### QuitGameButton.cs
**Caminho**: `Assets/Scripts/UI/QuitGameButton.cs`
**Descrição**: Botão para sair do jogo.

---

### 5.9 Game Management Scripts

#### GameManager.cs
**Caminho**: `Assets/Scripts/Utility/GameManager.cs`
**Descrição**: Singleton principal. Gerencia estado do jogo.
```
Propriedades estáticas:
- score: int

Métodos estáticos:
- AddScore(int): void
- ResetScore(): void
- SaveHighScore(): void
- UpdateUIElements(): void
```

#### LevelManager.cs
**Caminho**: `Assets/Scripts/Utility/LevelManager.cs`
**Descrição**: Static class helper para mudança de cenas.
```
Métodos estáticos:
- LoadScene(string): void
```

---

## 6. Referência de Prefabs

### 6.1 Player
| Prefab | Caminho |
|--------|---------|
| Player | `Assets/Prefabs/Player/Player.prefab` |

### 6.2 Enemies
| Prefab | Caminho |
|--------|---------|
| BasicWalkingEnemy | `Assets/Prefabs/Enemies/Basic/BasicWalkingEnemy.prefab` |
| BasicToughWalkingEnemy | `Assets/Prefabs/Enemies/Basic/BasicToughWalkingEnemy.prefab` |
| BasicStaticEnemy | `Assets/Prefabs/Enemies/Basic/BasicStaticEnemy.prefab` |
| SmartWalkingEnemy | `Assets/Prefabs/Enemies/SmartWalkingEnemy.prefab` |
| WaypointEnemyParent | `Assets/Prefabs/Enemies/WaypointEnemyParent.prefab` |

### 6.3 Platforms
| Prefab | Caminho |
|--------|---------|
| SmallPlatformStatic | `Assets/Prefabs/Environment/Platforms/Static/SmallPlatformStatic.prefab` |
| PlatformMediumStatic | `Assets/Prefabs/Environment/Platforms/Static/PlatformMediumStatic.prefab` |
| LargeStaticPlatform | `Assets/Prefabs/Environment/Platforms/Static/LargeStaticPlatform.prefab` |
| SmallMovingPlatformParent | `Assets/Prefabs/Environment/Platforms/Moving/SmallMovingPlatformParent.prefab` |
| MediumMovingPlatformParent | `Assets/Prefabs/Environment/Platforms/Moving/MediumMovingPlatformParent.prefab` |
| LargeMovingPlatformParent | `Assets/Prefabs/Environment/Platforms/Moving/LargeMovingPlatformParent.prefab` |

### 6.4 Pickups
| Prefab | Caminho |
|--------|---------|
| ScorePickup | `Assets/Prefabs/PickUps/ScorePickup.prefab` |
| HealthPickUp | `Assets/Prefabs/PickUps/HealthPickUp.prefab` |
| GoalPickup | `Assets/Prefabs/PickUps/GoalPickup.prefab` |
| ExtraLifePickUp | `Assets/Prefabs/PickUps/ExtraLifePickUp.prefab` |
| KeyPickupRed | `Assets/Prefabs/PickUps/Keys/KeyPickupRed.prefab` |
| KeyPickupBlue | `Assets/Prefabs/PickUps/Keys/KeyPickupBlue.prefab` |
| KeyPickupGreen | `Assets/Prefabs/PickUps/Keys/KeyPickupGreen.prefab` |
| KeyPickupPink | `Assets/Prefabs/PickUps/Keys/KeyPickupPink.prefab` |
| KeyPickupOrange | `Assets/Prefabs/PickUps/Keys/KeyPickupOrange.prefab` |
| KeyPickupGrey | `Assets/Prefabs/PickUps/Keys/KeyPickupGrey.prefab` |
| KeyPickupWhiteB | `Assets/Prefabs/PickUps/Keys/KeyPickupWhiteB.prefab` |

### 6.5 Doors
| Prefab | Caminho |
|--------|---------|
| DoorRed | `Assets/Prefabs/Environment/Doors/DoorRed.prefab` |
| DoorBlue | `Assets/Prefabs/Environment/Doors/DoorBlue.prefab` |
| DoorGreen | `Assets/Prefabs/Environment/Doors/DoorGreen.prefab` |
| DoorPink | `Assets/Prefabs/Environment/Doors/DoorPink.prefab` |
| DoorOrange | `Assets/Prefabs/Environment/Doors/DoorOrange.prefab` |
| DoorGrey | `Assets/Prefabs/Environment/Doors/DoorGrey.prefab` |
| DoorWhite | `Assets/Prefabs/Environment/Doors/DoorWhite.prefab` |

### 6.6 UI
| Prefab | Caminho |
|--------|---------|
| Game UI Canvas | `Assets/Prefabs/UI/Canvases/Game UI Canvas.prefab` |
| MainMenu UI Canvas | `Assets/Prefabs/UI/Canvases/MainMenu UI Canvas.prefab` |
| Game UI Canvas End | `Assets/Prefabs/UI/Canvases/Game UI Canvas End.prefab` |
| NumberTextDisplay | `Assets/Prefabs/UI/NumberTextDisplay.prefab` |
| PlayerImage | `Assets/Prefabs/UI/PlayerImage.prefab` |
| HealthImage | `Assets/Prefabs/UI/HealthImage.prefab` |

### 6.7 Effects
| Prefab | Caminho |
|--------|---------|
| VictoryEffect | `Assets/Prefabs/Effects/WinLoseEffects/VictoryEffect.prefab` |
| GameOverEffect | `Assets/Prefabs/Effects/WinLoseEffects/GameOverEffect.prefab` |
| NavigationEffect | `Assets/Prefabs/Effects/UIEffects/NavigationEffect.prefab` |
| ClickEffect | `Assets/Prefabs/Effects/UIEffects/ClickEffect.prefab` |
| UIBackEffect | `Assets/Prefabs/Effects/UIEffects/UIBackEffect.prefab` |
| ScorePickupEffect | `Assets/Prefabs/Effects/PickupEffects/ScorePickupEffect.prefab` |
| HealthPickupEffect | `Assets/Prefabs/Effects/PickupEffects/HealthPickupEffect.prefab` |
| KeyPickupEffect | `Assets/Prefabs/Effects/PickupEffects/KeyPickupEffect.prefab` |
| ExtraLifePickupEffect | `Assets/Prefabs/Effects/PickupEffects/ExtraLifePickupEffect.prefab` |
| PlayerHurtEffect | `Assets/Prefabs/Effects/HitEffects/PlayerHurtEffect.prefab` |
| EnemyHitEffect | `Assets/Prefabs/Effects/HitEffects/EnemyHitEffect.prefab` |
| PlayerDeathEffect | `Assets/Prefabs/Effects/DeathEffects/PlayerDeathEffect.prefab` |
| CheckpointActivationEffect | `Assets/Prefabs/Effects/CheckpointEffects/CheckpointActivationEffect.prefab` |
| DoorOpenAndCloseEffect | `Assets/Prefabs/Effects/Door/DoorOpenAndCloseEffect.prefab` |
| DoorLockedEffect | `Assets/Prefabs/Effects/Door/DoorLockedEffect.prefab` |

### 6.8 Managers
| Prefab | Caminho |
|--------|---------|
| GameManager | `Assets/Prefabs/Managers/GameManager.prefab` |
| UI Manager | `Assets/Prefabs/Managers/UI Manager.prefab` |

### 6.9 Checkpoint
| Prefab | Caminho |
|--------|---------|
| Checkpoint | `Assets/Prefabs/Environment/Checkpoint/Checkpoint.prefab` |
| Checkpoint B | `Assets/Prefabs/Environment/Checkpoint/Checkpoint B.prefab` |

---

## 7. Cenas

### 7.1 Level1.unity
**Caminho**: `Assets/_Scenes/Level1.unity`
**Descrição**: Cena principal de gameplay.

**Elementos típicos esperados**:
- Tilemap com plataformas
- Player spawn point
- Inimigos vários
- Checkpoints
- Collectibles (score, health, keys)
- Portas coloridas
- Moving platforms
- Goal/Finish line
- UI Canvas com HUD (score, health, lives)
- GameManager na cena
- UI Manager na cena

### 7.2 MainMenu.unity
**Caminho**: `Assets/_Scenes/MainMenu.unity`
**Descrição**: Menu principal do jogo.

**Elementos típicos esperados**:
- Background/tema visual
- Título do jogo
- Botões: Play, Options, Credits, Quit
- MainMenu UI Canvas
- UI Manager
- Música ambiente

---

## 8. Input System

### 8.1 Mapa de Controles

| Ação | Input (Keyboard) | Input (Gamepad) | Script Referência |
|------|------------------|-----------------|-------------------|
| **Movimento Horizontal** | A/D ou ←/→ | Left Stick X | PlayerController.moveAction |
| **Pulo** | Space | South Button (A/X) | PlayerController.jumpAction |
| **Pause** | Escape | Start | UIManager.pauseAction |
| **Look/Mouse** | Mouse Position | Right Stick | CameraController.lookAction |

### 8.2 Configuração no Unity

Os Input Actions são configurados no Unity Input System e referenciados nos scripts via campos públicos do tipo `InputAction`.

---

## 9. PlayerPrefs

### 9.1 Keys Usadas

| Key | Tipo | Descrição |
|-----|------|-----------|
| `highscore` | int | Maior pontuação alcançada |
| `score` | int | Pontuação atual |
| `lives` | int | Vidas atuais do player |
| `health` | int | Saúde atual do player |

### 9.2 Acesso via Código

```csharp
// Leitura
PlayerPrefs.GetInt("highscore");
PlayerPrefs.GetInt("score");
PlayerPrefs.GetInt("lives");
PlayerPrefs.GetInt("health");

// Escrita
PlayerPrefs.SetInt("highscore", valor);
PlayerPrefs.SetInt("score", valor);
PlayerPrefs.SetInt("lives", valor);
PlayerPrefs.SetInt("health", valor);

// Verificação
PlayerPrefs.HasKey("highscore");
```

---

## 10. Apêndice: Code Snippets

### 10.1 Obter Referência ao Player

```csharp
// Via GameManager (recomendado)
GameObject player = GameManager.instance.player;

// Via Find
PlayerController playerController = FindObjectOfType<PlayerController>();
GameObject player = playerController.gameObject;

// Via Tag
GameObject player = GameObject.FindGameObjectWithTag("Player");
```

### 10.2 Adicionar Pontuação

```csharp
GameManager.AddScore(10);
```

### 10.3 Aplicar Dano

```csharp
// Em um script com referência ao Health
Health health = target.GetComponent<Health>();
if (health != null)
{
    health.TakeDamage(1);
}
```

### 10.4 Curar Player

```csharp
Health health = GameManager.instance.player.GetComponent<Health>();
if (health != null)
{
    health.ReceiveHealing(1);
}
```

### 10.5 Adicionar Vida

```csharp
Health health = GameManager.instance.player.GetComponent<Health>();
if (health != null)
{
    health.AddLives(1);
}
```

### 10.6 Mudar de Cena

```csharp
// Via LevelManager (reseta time scale)
LevelManager.LoadScene("Level1");

// Via SceneManager direto
SceneManager.LoadScene("Level1");

// Com loading aditivo
SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
```

### 10.7 Pausar/Despausar Jogo

```csharp
// Pausar
Time.timeScale = 0;

// Despausar
Time.timeScale = 1;
```

### 10.8 Acessar UI Manager

```csharp
UIManager uiManager = UIManager.instance;

// Ir para página específica
uiManager.GoToPage(0);

// Alternar pause
uiManager.TogglePause();

// Atualizar UI
uiManager.UpdateUI();
```

### 10.9 Usar KeyRing

```csharp
// Adicionar chave
KeyRing.AddKey(1); // doorID 1

// Verificar se tem chave
bool hasKey = KeyRing.HasKey(doorComponent);

// Limpar todas as chaves
KeyRing.ClearKeyRing();
```

### 10.10 Usar Checkpoint

```csharp
// Definir checkpoint (em Checkpoint.cs automaticamente)
Health playerHealth = collision.gameObject.GetComponent<Health>();
playerHealth.SetRespawnPoint(respawnLocation.position);

// Respawn manual
playerHealth.Respawn();
```

### 10.11 Verificar Estado do Jogo

```csharp
// Verificar se game over
if (GameManager.instance.gameIsOver)
{
    // Game over ativo
}

// Verificar se vitória
bool isWinnable = GameManager.instance.gameIsWinnable;
```

---

## 11. Notas para IAs

### 11.1 Ao Trabalhar com Este Projeto

1. **Sempre verificar se GameManager.instance não é null** antes de usar
2. **Use PlayerPrefs com cautela** - dados podem persistir entre sessões
3. **Input System** requer que Actions sejam habilitadas no OnEnable e desabilitadas no OnDisable
4. **Herança OOP**: não crie instâncias de classes abstratas (EnemyBase, Pickup)
5. **Static classes**: KeyRing, CheckpointTracker, LevelManager não precisam de instância

### 11.2 Erros Comuns a Evitar

1. Não referenciar `GameManager.instance` em Awake sem verificar se é null
2. Não chamar métodos de UI antes de UIManager.Start() completar
3. Não usar Time.deltaTime incorretamente em movimentos
4. Não esquecer de configurar Layers para GroundCheck

### 11.3 Convenções de Debug

- Use `Debug.Log()` para logging
- Use `Debug.LogWarning()` para avisos
- Use `Debug.LogError()` para erros

---

*Este documento foi gerado automaticamente para servir como referência para agentes de IA trabalhando com o projeto Unity 2D Platformer.*