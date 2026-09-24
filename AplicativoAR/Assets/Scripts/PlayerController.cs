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

    // Posiciones LOCALES al lado del arbol (ajusta el numero si se acerca o aleja mucho)
    private Vector3 leftPos = new Vector3(-1.47f, 0, 0);
    private Vector3 rightPos = new Vector3(1.47f, 0, 0);

    // Rotaciones LOCALES para mirar hacia el tronco
    private Quaternion lookRight = Quaternion.Euler(0, 90, 0);
    private Quaternion lookLeft = Quaternion.Euler(0, -90, 0);

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
            bool touchedLeft = Input.mousePosition.x < Screen.width / 2;
            HandleChop(touchedLeft);
        }
    }

    private void HandleChop(bool touchedLeft)
    {
        // 1) Primero movemos al leñador al lado donde tocaron: esto es lo que hace que
        //    "si le doy a la derecha, el leñador pase a la derecha a talar".
        transform.localPosition = touchedLeft ? leftPos : rightPos;
        transform.localRotation = touchedLeft ? lookRight : lookLeft;

        // 2) Revisamos el tronco de ABAJO tal como esta, ANTES de talarlo. Ese es el
        //    tronco que le pega al leñador si tiene una rama de su mismo lado.
        //    (El bug original talaba primero y revisaba el tronco siguiente, por eso
        //    la muerte pasaba "un tronco tarde".)
        string currentBottomTag = treeManager.GetBottomLogTag();

        bool hitByBranch =
            (touchedLeft && currentBottomTag == "LeftBranch") ||
            (!touchedLeft && currentBottomTag == "RightBranch");

        if (hitByBranch)
        {
            Die();
            return;
        }

        // 3) Si esquivo la rama a tiempo, recien ahi se tala el tronco.
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