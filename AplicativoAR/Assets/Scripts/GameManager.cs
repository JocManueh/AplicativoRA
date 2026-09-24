using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Paneles UI (arrastra los GameObjects del Canvas)")]
    public GameObject menuPanel;
    public GameObject gameOverPanel;
    public GameObject hudPanel; // opcional: puntaje, instrucciones, etc.

    [Header("Referencias de juego")]
    public PlaneManager planeManager;
    public TreeManager treeManager;
    public PlayerController playerController;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip loseSound;

    public enum GameState { Menu, Playing, GameOver }
    public GameState CurrentState { get; private set; } = GameState.Menu;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ShowMenu();
    }

    private void ShowMenu()
    {
        CurrentState = GameState.Menu;

        if (menuPanel) menuPanel.SetActive(true);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (hudPanel) hudPanel.SetActive(false);

        if (playerController) playerController.enabled = false;
    }

    // Engancha esto al OnClick del boton "Start" del menu
    public void OnStartPressed()
    {
        if (menuPanel) menuPanel.SetActive(false);
        if (hudPanel) hudPanel.SetActive(true);

        CurrentState = GameState.Playing;

        // Habilita que PlaneManager empiece a buscar un plano y colocar el arbol/leñador
        if (planeManager) planeManager.EnablePlacement();
    }

    // Engancha esto al OnClick del boton "Salir" del menu
    public void OnExitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Llamado por PlaneManager en cuanto coloca el arbol en el plano detectado
    public void OnGameplayReady()
    {
        if (playerController)
        {
            playerController.ResetPlayer();
            playerController.enabled = true;
        }
    }

    // Llamado por PlayerController cuando el leñador muere
    public void OnPlayerDied()
    {
        CurrentState = GameState.GameOver;

        if (playerController) playerController.enabled = false;
        if (gameOverPanel) gameOverPanel.SetActive(true);

        PlaySound(loseSound);
    }

    // Engancha esto al OnClick del boton "Reintentar" del panel de derrota
    public void OnRestartPressed()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);

        // No hace falta volver a escanear el plano: solo reiniciamos el arbol y al jugador
        if (treeManager) treeManager.ResetTree();
        if (playerController)
        {
            playerController.ResetPlayer();
            playerController.enabled = true;
        }

        CurrentState = GameState.Playing;
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}