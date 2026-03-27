using UnityEngine;
using UnityEngine.SceneManagement; // Para reiniciar el nivel si mueres

public class PlayerHealth : MonoBehaviour
{
    [Header("SISTEMA DE VIDAS")]
    public int maxLives = 3;
    public int currentLives;

    [Header("UI VIDAS")]
    public GameObject life1;
    public GameObject life2;
    public GameObject life3;

    void Start()
    {
        currentLives = maxLives;

        //  Auto-asignar si no se arrastraron
        if (life1 == null) life1 = GameObject.Find("Life_1");
        if (life2 == null) life2 = GameObject.Find("Life_2");
        if (life3 == null) life3 = GameObject.Find("Life_3");
    }

    /// <summary>
    /// Se llama cuando el jugador pierde una vida
    /// </summary>
    public void LoseLife()
    {
        currentLives--;
        Debug.Log("¡El monstruo te atacó! Vidas restantes: " + currentLives);

        UpdateLivesUI();

        if (currentLives <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Actualiza los corazones en pantalla
    /// </summary>
    void UpdateLivesUI()
    {
        // Apagar corazones según vidas
        if (currentLives < 3 && life3 != null)
            life3.SetActive(false);

        if (currentLives < 2 && life2 != null)
            life2.SetActive(false);

        if (currentLives < 1 && life1 != null)
            life1.SetActive(false);
    }

    /// <summary>
    /// Cuando el jugador se queda sin vidas
    /// </summary>
    void Die()
    {
        Debug.Log("¡GAME OVER! El monstruo destruyó el taxi.");

        // Reiniciar escena después de un pequeño delay (opcional)
        Invoke(nameof(RestartScene), 1.5f);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}