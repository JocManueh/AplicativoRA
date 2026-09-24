using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TreeManager treeManager;
    public Animator anim;

    private bool isDead = false;

    // Posiciones pegadas al tronco (Reducidas para que no se aleje tanto)
    private Vector3 leftPos = new Vector3(-0.25f, 0, 0);
    private Vector3 rightPos = new Vector3(0.25f, 0, 0);

    void Update()
    {
        if (isDead) return;

        // Detectar toque en la pantalla
        if (Input.GetMouseButtonDown(0))
        {
            bool touchedLeft = Input.mousePosition.x < Screen.width / 2;

            if (touchedLeft)
            {
                transform.localPosition = leftPos;
                // Rotación limpia mirando hacia la derecha (hacia el árbol)
                transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                transform.localPosition = rightPos;
                // Rotación limpia mirando hacia la izquierda (hacia el árbol)
                transform.localRotation = Quaternion.Euler(0, -90, 0);
            }

            if (anim != null) anim.SetTrigger("Chop");

            string bottomLogTag = treeManager.ChopBottomLog();

            // Si hay rama en nuestro lado, morimos
            if ((touchedLeft && bottomLogTag == "LeftBranch") || (!touchedLeft && bottomLogTag == "RightBranch"))
            {
                Die();
            }
        }
    }

    void Die()
    {
        isDead = true;
        if (anim != null) anim.SetTrigger("Die");

        GameObject btn = GameObject.Find("RestartButton");
        if (btn != null)
        {
            btn.SetActive(true);
        }
    }
}