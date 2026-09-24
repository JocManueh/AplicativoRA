using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TreeManager treeManager;
    public Animator anim;

    private bool isDead = false;

    // Posiciones LOCALES al lado del árbol (Ajusta el número si se acerca o aleja mucho)
    private Vector3 leftPos = new Vector3(-1.47f, 0, 0);
    private Vector3 rightPos = new Vector3(1.47f, 0, 0);

    // Rotaciones LOCALES para mirar hacia el tronco
    private Quaternion lookRight = Quaternion.Euler(0, 90, 0);
    private Quaternion lookLeft = Quaternion.Euler(0, -90, 0);

    void Start()
    {
        // Al iniciar, colocarse a la izquierda mirando al árbol
        transform.localPosition = leftPos;
        transform.localRotation = lookRight;
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetMouseButtonDown(0))
        {
            bool touchedLeft = Input.mousePosition.x < Screen.width / 2;

            if (touchedLeft)
            {
                transform.localPosition = leftPos;
                transform.localRotation = lookRight;
            }
            else
            {
                transform.localPosition = rightPos;
                transform.localRotation = lookLeft;
            }

            if (anim != null) anim.SetTrigger("Chop");

            string bottomLogTag = treeManager.ChopBottomLog();

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