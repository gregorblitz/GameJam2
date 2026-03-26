using UnityEngine;
using UnityEngine.SceneManagement; // Para reiniciar el nivel si mueres

public class PlayerHealth : MonoBehaviour
{
    [Header("SISTEMA DE VIDAS")]
    public int maxLives = 3;
    public int currentLives;

    void Start()
    {
        // Vidas iniciales al max
        currentLives = maxLives;
    }

    // se llama cuando el monstruo ataque
    public void LoseLife()
    {
        currentLives--;
        Debug.Log("¡El monstruo te ataco! Vidas restantes: " + currentLives);

        // Aqui se puede a;adir efectos de sonido, etc

        if (currentLives <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("¡GAME OVER! El monstruo destruyo el taxi.");
        
        // Se puede reiniciar la escena actual
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}
