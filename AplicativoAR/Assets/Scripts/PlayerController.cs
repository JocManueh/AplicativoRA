using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para reiniciar la escena

public class PlayerController : MonoBehaviour
{
    public TreeManager treeManager;
    public Animator anim;

    private bool isDead = false;
    private Vector3 leftPos = new Vector3(-0.5f, 0, 0); // Ajusta la posición izquierda
    private Vector3 rightPos = new Vector3(0.5f, 0, 0); // Ajusta la posición derecha

    void Update()
    {
        if (isDead) return;

        // Detectar toque en la pantalla del celular
        if (Input.GetMouseButtonDown(0))
        {
            bool touchedLeft = Input.mousePosition.x < Screen.width / 2;

            // Mover leñador y voltearlo
            transform.localPosition = touchedLeft ? leftPos : rightPos;
            transform.localScale = new Vector3(touchedLeft ? 1 : -1, 1, 1);

            if (anim != null) anim.SetTrigger("Chop"); // Animación de talar

            string bottomLogTag = treeManager.ChopBottomLog(); // Cortar el árbol

            // Si el tronco de abajo tiene una rama de nuestro lado, perdemos
            if ((touchedLeft && bottomLogTag == "LeftBranch") || (!touchedLeft && bottomLogTag == "RightBranch"))
            {
                Die();
            }
        }
    }

    void Die()
    {
        isDead = true;
        if (anim != null) anim.SetTrigger("Die"); // Animación de morir

        // BUSCAR EL BOTÓN EN LA ESCENA Y MOSTRARLO
        GameObject btn = GameObject.Find("RestartButton"); // Busca el botón por su nombre
        if (btn != null)
        {
            btn.SetActive(true); // Lo hace visible
        }
    }
}