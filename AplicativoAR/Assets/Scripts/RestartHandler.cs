using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartHandler : MonoBehaviour
{
    // Esta función se ejecutará cuando toques el botón
    public void RestartGame()
    {
        // Recarga la escena actual limpiando todo para volver a jugar
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
