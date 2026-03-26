# 2D Platformer

Um jogo de plataforma 2D desenvolvido em Unity, com mecânicas clássicas de movimento, coleta de itens, combate e progressão por níveis.

![Unity Version](https://img.shields.io/badge/Unity-6000.4.0f1-blue?logo=unity)
![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-yellow)
![License](https://img.shields.io/badge/License-MIT-green)

---

## 🎮 Sobre o Jogo

**2D Platformer** é um jogo de plataforma side-scrolling onde o jogador controla um personagem que deve navegar por fases desafiadoras, coletar itens, derrotar inimigos e alcançar o objetivo final.

### Características Principais
- Sistema de movementação fluida com suporte a corrida, pulo e wall slide
- Mecânicas de combate e dano
- Sistema de vidas e health
- Coletáveis (moedas, vidas extras, health, chaves)
- Inimigos com comportamentos diferentes (terrestres e voadores)
- Sistema de checkpoints
- Plataformas animadas
- UI completa (pontuação, vidas, health, high score)
- Suporte a múltiplas cenas (menu principal, níveis)

---

## 🛠️ Tecnologias Utilizadas

- **Unity**: 6000.4.0f1 (Unity 6.0)
- **Linguagem**: C#
- **Input System**: Nova API de Input da Unity
- **Rendering**: Universal Render Pipeline (URP)
- **Text**: TextMesh Pro
- **Audio**: Sistema de áudio nativo da Unity
- **Sistema de Física**: Physics2D
- **Tilemaps**: Sistema de Tilemaps da Unity

---

## 📁 Estrutura do Projeto

```
2DPlatformer/
├── Assets/
│   ├── Art/                          # Arte do jogo
│   │   ├── Animation/               # Animações de sprites
│   │   ├── Collectables/            # Sprites de itens coletáveis
│   │   ├── Enemies/                 # Sprites de inimigos
│   │   ├── Environment/             # Sprites de cenário
│   │   ├── Player/                  # Sprites do jogador
│   │   └── UI Elements/             # Elementos de UI
│   ├── Audio/                       # Arquivos de áudio
│   │   ├── Jingles/                 # Músicas de vitória/derrota
│   │   ├── Music/                   # Trilha sonora
│   │   └── SFX/                     # Efeitos sonoros
│   ├── Prefabs/                     # Prefabs do jogo
│   │   ├── Enemies/
│   │   ├── Environment/
│   │   ├── Managers/
│   │   ├── PickUps/
│   │   ├── Player/
│   │   ├── UI/
│   │   └── Utilities/
│   ├── Scripts/                     # Código fonte
│   │   ├── Camera/                  # Controle de câmera
│   │   ├── Checkpoint/              # Sistema de checkpoints
│   │   ├── Enemy/                   # Lógica de inimigos
│   │   ├── Environment/             # Plataformas, objetos ambientais
│   │   ├── Health&Damage/           # Sistema de dano e vida
│   │   ├── Keys&Doors/              # Sistema de chaves e portas
│   │   ├── Pickups/                 # Itens coletáveis
│   │   ├── Player/                  # Controles do jogador
│   │   ├── UI/                      # Interface do usuário
│   │   └── Utility/                 # Scripts utilitários
│   ├── Input/                       # Configurações de input
│   ├── Physics Materials/           # Materiais de física
│   ├── Resources/                   # Recursos dinâmicos
│   ├── TextMesh Pro/                # Fontes e configurações TMP
│   ├── _Scenes/                     # Cenas do jogo
│   │   ├── Level1.unity             # Primeiro nível
│   │   └── MainMenu.unity           # Menu principal
│   └── Imported Custom Asset Packages/
│       └── Developer Console/       # Console de desenvolvimento
├── Library/                          # Cache do Unity (não versionar)
├── Logs/                            # Logs do Unity
├── Packages/                        # Pacotes Unity
├── ProjectSettings/                 # Configurações do projeto
├── UserSettings/                    # Configurações do usuário
├── *.sln                            # Solution do Visual Studio
├── opencode.json                    # Configuração Claude Code
└── README.md                        # Este arquivo

```

---

## 🎯 Mecânicas do Jogo

### Controles do Jogador
- **Move**: setas direcionais ou A/D
- **Pulo**: Espaço ou botão de ação
- **Correr**: Shift (movimento mais rápido)
- **Wall Slide**: Perto de parede durante pulo
- **Interagir**: E (para portas e objetos interativos)

### Sistemas de Jogo

| Sistema | Descrição | Scripts |
|---------|-----------|---------|
| **Player** | Movimentação, pulo, wall slide, animações | `PlayerMovement.cs`, `PlayerAnimator.cs`, `PlayerChilder.cs` |
| **Câmera** | Segue o jogador com拖拽 limits | `CameraController.cs` |
| **Inimigos** | Dois tipos: terrestre e voador | `WalkingEnemy.cs`, `FlyingEnemy.cs`, `EnemyBase.cs`, `EnemyAnimator.cs` |
| **Dano/Vida** | Sistema de health com invulnerabilidade | `Health.cs`, `Damage.cs`, `Head.cs`, `DeathEffectAnimationHandler.cs` |
| **Coletáveis** | Moedas, vidas, health, chaves, objetivo | `ScorePickup.cs`, `HealthPickup.cs`, `ExtraLifePickup.cs`, `KeyPickup.cs`, `GoalPickup.cs` |
| **Checkpoints** | Pontos de respawn automático | `Checkpoint.cs`, `CheckpointTracker.cs` |
| **Portas/Chaves** | Sistema de acesso com chaves | `Door.cs`, `KeyRing.cs` |
| **Plataformas** | Plataformas animadas | `PlatformAnimator.cs`, `WaypointMover.cs` |
| **UI** | HUD com pontuação, vidas, health | `UIManager.cs`, `ScoreDisplay.cs`, `LivesDisplay.cs`, `HealthDisplay.cs` |
| **Gestão de Nível** | Carregamento de cenas, reset | `LevelManager.cs`, `LevelSwitcher.cs` |
| **Game Manager** | Estado global do jogo | `GameManager.cs` |

---

## 🚀 Como Executar

### Requisitos do Sistema
- **Unity**: 6000.4.0f1 (Unity 6.0) ou superior
- **Sistema Operacional**: Windows, macOS ou Linux
- **GPU**: Qualquer GPU com suporte a OpenGL 3.0+ / DirectX 11+
- **RAM**: Mínimo de 4GB (recomendado 8GB+)

### Passos para Execução

1. **Abrir o Projeto**
   ```bash
   - Abra o Unity Hub
   - Clique em "Add Project"
   - Selecione a pasta D:/Unity_Projects/2DPlatformer
   - Clique em "Open"
   ```

2. **Aguardar Compilação**
   - O Unity vai importar e compilar todos os assets
   - Isso pode levar alguns minutos na primeira vez

3. **Executar o Jogo**
   - Abra a cena `Assets/_Scenes/MainMenu.unity` (ou Level1.unity)
   - Clique no botão **Play** no topo do editor Unity
   - Ou pressione **Ctrl+P** (Windows/Linux) / **Cmd+P** (macOS)

4. **Build para Executável** (opcional)
   - `File → Build Settings`
   - Adicione a cena desejada
   - Selecione plataforma (Windows, macOS, Linux)
   - Clique em **Build and Run**

---

## 🎨 Recursos de Arte

- **Sprites**: Arte pixel art em Assets/Art/
- **Tilemaps**: Sistema de tiles para cenários
- **Animações**: Spritesheets com sprites individuais e animações
- **Cores**: Paleta vibrante estilo retro
- **Resolução**: Game desenvolvido para resolução base 1920x1080 (upscaling)

---

## 📊 Progresso do Projeto

✅ **Implementado:**
- [x] Menu principal funcional
- [x] Sistema de input completo
- [x] Movimentação do jogador (run, jump, wall slide)
- [x] Sistema de animações do jogador
- [x] Câmera com acompanhamento
- [x] Inimigos básicos (terrestre e voador)
- [x] Sistema de coleta de itens
- [x] Sistema de health e dano
- [x] Sistema de vidas e respawn
- [x] Checkpoints
- [x] Portas e sistema de chaves
- [x] Plataformas animadas
- [x] UI completa (HUD e menus)
- [x] Sistema de pontuação e high score
- [x] Transições entre cenas
- [x] Sons e música

🔄 **Em Desenvolvimento:**
- [ ] Mais níveis
- [ ] Mais tipos de inimigos
- [ ] Power-ups
- [ ] Bosses
- [ ] Cutscenes
- [ ] Mais tipos de plataformas

---

## 🧪 Testes

O projeto inclui funcionalidades de debugging:
- **Console de Desenvolvedor**: Comandos disponíveis via console
  - `loadscene <nome>` - Carrega uma cena específica
  - `sethhealth <valor>` - Define a saúde do jogador
  - `listscenes` - Lista todas as cenas disponíveis

Para ativar o console de desenvolvedor, pressione a tecla **~** (til) durante o jogo.

---

## 📝 Configuração do Projeto

### Layers
- `Player` (layer 8) - Camada do jogador
- `Ground` (layer 7) - Camada do chão e plataformas
- `Enemy` (layer 9) - Camada dos inimigos
- `Feet` (layer 10) - Detectores de chão

### Tags
- `Player` - Objeto jogador
- `MainCamera` - Câmera principal
- `Feet` - Pontos de detecção de chão

### Input Actions
O projeto usa o novo Input System da Unity com os seguintes action maps:
- `Player` - Controles do jogador (move, jump, run)
- `UI` - Navegação na interface

---

## 📄 Scripts Principais

### Player
- `PlayerMovement.cs` - Física e controle do jogador
- `PlayerAnimator.cs` - Controlador de animações
- `PlayerChilder.cs` - Gerencia objetos filhos do player

### Inimigos
- `EnemyBase.cs` - Classe base para todos os inimigos
- `WalkingEnemy.cs` - Inimigo que anda entre waypoints
- `FlyingEnemy.cs` - Inimigo que voa em padrões

### UI
- `UIManager.cs` - Gerenciador principal da interface
- `ScoreDisplay.cs` - Exibição da pontuação
- `LivesDisplay.cs` - Exibição das vidas
- `HealthDisplay.cs` - Exibição da saúde

---

## 🤝 Contribuindo

Este é um projeto pessoal/educacional. Se deseja contribuir:

1. Faça um fork do repositório
2. Crie uma branch para sua feature (`git checkout -b feature/NovaFeature`)
3. Faça commit das mudanças (`git commit -m 'Adiciona nova feature'`)
4. Faça push para a branch (`git push origin feature/NovaFeature`)
5. Abra um Pull Request

---

## 📜 Licença

Este projeto está licenciado sob a Licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

---

## 👨‍💻 Autor

Desenvolvido por: Jhonni Vieceli

---

## 🔗 Links Úteis

- [Documentação Unity](https://docs.unity3d.com/Manual/index.html)
- [Unity Learn](https://learn.unity.com/)
- [Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.4/manual/)
- [TextMesh Pro](https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.0/manual/)

---

## 📌 Notas

- **Versionamento**: Este projeto usa Git para controle de versão
- **Unity Version**: 6000.4.0f1 (Unity 6.0)
- **Target Platform**: Windows ( construível para outras plataformas)
- **Source Control**: Configurado para Unity com arquivos .meta versionados

---

**Divirta-se jogando! 🎮**
