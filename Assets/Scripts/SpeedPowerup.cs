using UnityEngine;

public class SpeedPowerup : MonoBehaviour
{
    [Header("CONFIGURACIÓN DEL TURBO")]
    public float speedMultiplier = 1.5f; // Multiplica velocidad x 1.5
    public float boostDuration = 3f;     // Dura 3 segundos

    private void OnTriggerEnter(Collider other)
    {
        // Buscamos si el obj tiene el script del Taxi
        CarController taxi = other.GetComponent<CarController>();

        if (taxi != null)
        {
            // Le activa el turbo
            taxi.ActivateSpeedBoost(speedMultiplier, boostDuration);

            // Destruye el prefab
            Destroy(gameObject);
        }
    }
}
