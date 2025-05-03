# Jogo-2d-fecaf

1. PlayerController.cs (Controle da Raposa)
Funções Principais:

Movimento Horizontal:

Usa Input.GetAxisRaw para detectar direção (esquerda/direita)

Aplica velocidade através do Rigidbody2D

Pulo:

Verifica chão com OverlapCircle no objeto groundCheck

Implementa coyote time (tempo extra para pular ao sair da plataforma)

Aplica força vertical no Rigidbody2D

Flip do Sprite:

Inverte escala (transform.localScale.x) ao mudar de direção

Variáveis Importantes:

csharp
public float moveSpeed = 5f;
public float jumpForce = 12f;
public Transform groundCheck;
public LayerMask groundLayer;
private bool isGrounded;
2. Coin.cs (Moedas Coletáveis)
Funções Principais:

Detecção de Colisão:

Usa OnTriggerEnter2D para registrar contato com o jogador

Feedback Visual/Sonoro:

Toca AudioClip ao ser coletada

Instancia partículas (ParticleSystem) antes de ser destruída

Atualização de Pontuação:

Acessa o GameManager para incrementar contador

Variáveis Importantes:

csharp
public int value = 1;
public AudioClip collectSound;
public ParticleSystem collectEffect;
3. EnemyPatrol.cs (Inimigos Patrulheiros)
Funções Principais:

Patrulha Automática:

Movimentação horizontal constante com Translate

Detecção de Bordas:

Usa Raycast para verificar chão e paredes

Inverte direção (Flip) ao detectar obstáculos

Dano ao Jogador:

Verifica colisão via OnCollisionEnter2D

Variáveis Importantes:

csharp
public float speed = 3f;
public float rayDistance = 1f;
public Transform edgeCheck;
private bool movingRight;
4. CameraFollow.cs (Câmera que Segue o Jogador)
Funções Principais:

Seguimento Suave:

Usa Vector3.SmoothDamp para movimento fluido

Limites de Cena:

Restringe área visível com valores mínimos/máximos em X e Y

Variáveis Importantes:

csharp
public Transform target;
public float smoothSpeed = 0.125f;
public Vector2 minBounds, maxBounds;
5. GameManager.cs (Gerenciador Global)
Funções Principais:

Controle de Estado:

Gerencia vida do jogador, moedas coletadas e cenas

UI:

Atualiza textos (TextMeshProUGUI) de pontuação/vida

Singleton Pattern:

Garante única instância com DontDestroyOnLoad

Variáveis Importantes:

csharp
public static GameManager Instance;
public TextMeshProUGUI coinText;
private int coins;
6. Interactable.cs (Objetos Interativos)
Funções Principais:

Detecção de Ação:

Checa input (ex: tecla "E") próximo ao objeto

Eventos Customizáveis:

Dispara ações via UnityEvent (ex: abrir baú, ativar diálogo)

Variáveis Importantes:

csharp
public KeyCode interactKey = KeyCode.E;
public UnityEvent onInteract;
Estrutura de Pastas Recomendada:
Assets/
├─ Scripts/
│  ├─ Player/
│  ├─ Enemies/
│  ├─ Systems/
├─ Prefabs/
├─ Audio/
├─ Art/
Cada script foi projetado para ser modular e de fácil ajuste via Inspector. Para dúvidas específicas sobre implementação de algum deles, posso elaborar exemplos mais detalhados
