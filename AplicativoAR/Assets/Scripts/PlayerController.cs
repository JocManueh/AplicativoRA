using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TreeManager treeManager;
    public Animator anim;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip chopLeftSound;
    public AudioClip chopRightSound;

    private bool isDead = false;

    // Posiciones LOCALES al lado del árbol (X: -1.47 y 1.47)
    private Vector3 leftPos = new Vector3(-1.47f, 0f, 0f);
    private Vector3 rightPos = new Vector3(1.47f, 0f, 0f);

    // Rotaciones LOCALES correctas para tu modelo (X:0, Z:0)
    private Quaternion lookRight = Quaternion.Euler(0f, 90f, 0f);  // Mira al árbol desde la izquierda
    private Quaternion lookLeft = Quaternion.Euler(0f, -90f, 0f); // Mira al árbol desde la derecha

    void OnEnable()
    {
        ResetPlayer();
    }

    // Deja al leñador listo para una nueva partida (llamado por GameManager)
    public void ResetPlayer()
    {
        isDead = false;
        transform.localPosition = leftPos;
        transform.localRotation = lookRight;
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetMouseButtonDown(0))
        {
            // Divide la pantalla a la mitad para detectar toque izquierdo o derecho
            bool touchedLeft = Input.mousePosition.x < (Screen.width / 2f);
            HandleChop(touchedLeft);
        }
    }

    private void HandleChop(bool touchedLeft)
    {
        // 1) Movemos al leñador a su posición y rotación correspondiente
        transform.localPosition = touchedLeft ? leftPos : rightPos;
        transform.localRotation = touchedLeft ? lookRight : lookLeft;

        // 2) Revisamos el tronco de ABAJO ANTES de talarlo
        string currentBottomTag = treeManager.GetBottomLogTag();

        bool hitByBranch =
            (touchedLeft && currentBottomTag == "LeftBranch") ||
            (!touchedLeft && currentBottomTag == "RightBranch");

        if (hitByBranch)
        {
            Die();
            return;
        }

        // 3) Si esquivó la rama, se realiza la animación, sonido y corte
        if (anim != null) anim.SetTrigger("Chop");
        PlayChopSound(touchedLeft);
        treeManager.ChopBottomLog();
    }

    private void PlayChopSound(bool touchedLeft)
    {
        if (audioSource == null) return;

        AudioClip clip = touchedLeft ? chopLeftSound : chopRightSound;
        if (clip != null) audioSource.PlayOneShot(clip);
    }

    void Die()
    {
        isDead = true;
        if (anim != null) anim.SetTrigger("Die");

        if (GameManager.Instance != null)
            GameManager.Instance.OnPlayerDied();
    }
}