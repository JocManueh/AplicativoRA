using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public TreeManager treeManager;
    public Animator anim;
    public GameObject gameOverButton; // Arrastra el botón aquí en el Inspector

    private bool isDead = false;
    private Vector3 leftPos = new Vector3(-1f, 0, 0); // Ajusta estas posiciones
    private Vector3 rightPos = new Vector3(1f, 0, 0);

    void Update()
    {
        if (isDead) return;

        if (Input.GetMouseButtonDown(0))
        {
            bool touchedLeft = Input.mousePosition.x < Screen.width / 2;

            // Mover leñador
            transform.localPosition = touchedLeft ? leftPos : rightPos;
            transform.localScale = new Vector3(touchedLeft ? 1 : -1, 1, 1); // Voltear para mirar al árbol

            anim.SetTrigger("Chop");
            string bottomLogTag = treeManager.ChopBottomLog();

            // Verificar si te golpeó una rama
            if ((touchedLeft && bottomLogTag == "LeftBranch") || (!touchedLeft && bottomLogTag == "RightBranch"))
            {
                Die();
            }
        }
    }

    void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        if (gameOverButton != null) gameOverButton.SetActive(true); // Muestra botón reiniciar
    }
}