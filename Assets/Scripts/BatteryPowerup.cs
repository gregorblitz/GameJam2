using UnityEngine;

public class BatteryPowerUp : MonoBehaviour
{
    [Header("ENERGÍA A RECUPERAR")]
    public float rechargeAmount = 40f; // Recuperacion de luz al coger powerup

    // Se activa cuando taxi atraviesa a este objeto
    private void OnTriggerEnter(Collider other)
    {
        // Busca si el objeto tiene script de luces
        CarLightSystem lightSystem = other.GetComponent<CarLightSystem>();

        // Si lo tiene significa que es el taxi
        if (lightSystem != null)
        {
            // Recargamos la energia usando la funcion que ya estaba preparada
            lightSystem.AddEnergy(rechargeAmount);

            // Destruye la bateria de la escena
            Destroy(gameObject);
            
            Debug.Log("¡Batería recogida! Energía restaurada.");
        }
    }
}
