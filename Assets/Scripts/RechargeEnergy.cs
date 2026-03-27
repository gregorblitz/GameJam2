using UnityEngine;
using UnityEngine.UI; // para modificar la barra de UI

public class BatteryManager : MonoBehaviour
{
    [Header("Configuración de la Batería")]
    public float maxBattery = 100f;
    public float currentBattery;
    public float drainRate = 5f; // ratio de perdida de bateria por segundo
    public float rechargeAmount = 25f; // cantidad que recarga al agarrar un PowerUp

    [Header("Interfaz y Luces")]
    public Image batteryBar; // Arrastrar imagen de la UI
    public Light carHeadlights; // Arrastrar la luz del taxi

    void Start()
    {
        // Empezamos con bateria full
        currentBattery = maxBattery;
    }

    void Update()
    {
        // Bateria se va gastando con el tiempo
        if (currentBattery > 0)
        {
            currentBattery -= drainRate * Time.deltaTime;
            UpdateUI();

            // Si la bateria llega a 0, apagamos las luces
            if (currentBattery <= 0)
            {
                currentBattery = 0;
                if (carHeadlights != null) carHeadlights.enabled = false;
                
                // Falta por definir algo aca, pero se deja espacio por si algo
                Debug.Log("¡Luces apagadas! El monstruo se acerca...");
            }
        }
    }

    // Recargar la bateria
    public void RechargeBattery()
    {
        currentBattery += rechargeAmount;
        
        // Evitamos que pase del 100%
        if (currentBattery > maxBattery) 
        {
            currentBattery = maxBattery;
        }

        // prende luces si estaban apagadas
        if (carHeadlights != null && !carHeadlights.enabled)
        {
            carHeadlights.enabled = true;
        }

        UpdateUI();
        Debug.Log("¡Bateria recargada!");
    }

    // Actualiza la barra visual en la pantalla
    private void UpdateUI()
    {
        if (batteryBar != null)
        {
            // fillAmount requiere un valor entre 0 y 1, por eso dividimos
            batteryBar.fillAmount = currentBattery / maxBattery;
        }
    }

    // Detecta cuando el taxi choca con una bateria en la calle
    private void OnTriggerEnter(Collider other)
    {
        // compara si tag es BatteryPowerUp 
        if (other.CompareTag("BatteryPowerUp"))
        {
            RechargeBattery();
            Destroy(other.gameObject); // Destruye el objeto
        }
    }
}
