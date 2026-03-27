using UnityEngine;
using UnityEngine.UI;

public class CarLightSystem : MonoBehaviour
{
    [Header("REFERENCIAS DE LUCES")]
    public Light leftLight;   // Luz izquierda
    public Light rightLight;  // Luz derecha

    [Header("ENERGÍA")]
    public float maxEnergy = 100f;
    public float currentEnergy;

    [Header("CONSUMO")]
    public float drainRate = 5f;

    [Header("CONFIGURACIÓN DE LUZ")]
    public float maxIntensity = 2f;
    public float minIntensity = 0f;
    public float maxRange = 25f;
    public float minRange = 5f;

    [Header("EFECTOS")]
    public bool enableFlicker = true;
    public float flickerThreshold = 0.2f;

    [Header("UI")]
    public Image energyFill; // Barra de energía (Energy_Fill)

    void Start()
    {
        // Iniciar con energía completa
        currentEnergy = maxEnergy;

        //  Si no se asignó en el inspector, buscar automáticamente
        if (energyFill == null)
        {
            GameObject bar = GameObject.Find("Energy_Fill");
            if (bar != null)
            {
                energyFill = bar.GetComponent<Image>();
            }
            else
            {
                Debug.LogWarning("No se encontró Energy_Fill en la escena");
            }
        }
    }

    void Update()
    {
        DrainEnergy();
        UpdateLights();
    }

    /// <summary>
    /// Reduce energía con el tiempo
    /// </summary>
    void DrainEnergy()
    {
        currentEnergy -= drainRate * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }

    /// <summary>
    /// Actualiza luces y UI
    /// </summary>
    void UpdateLights()
    {
        float normalized = currentEnergy / maxEnergy;

        // Intensidad y alcance según energía
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, normalized);
        float range = Mathf.Lerp(minRange, maxRange, normalized);

        leftLight.intensity = intensity;
        rightLight.intensity = intensity;

        leftLight.range = range;
        rightLight.range = range;

        // Parpadeo si está bajo
        if (enableFlicker && normalized < flickerThreshold && currentEnergy > 0)
        {
            float flicker = Random.Range(0.8f, 1.2f);
            leftLight.intensity *= flicker;
            rightLight.intensity *= flicker;
        }

        // Encendido/apagado
        bool isOn = currentEnergy > 0;
        leftLight.enabled = isOn;
        rightLight.enabled = isOn;

        //  Actualizar barra UI
        if (energyFill != null)
        {
            energyFill.fillAmount = normalized;

            // BONUS: cambio de color según energía
            if (normalized > 0.6f)
                energyFill.color = Color.green;
            else if (normalized > 0.3f)
                energyFill.color = Color.yellow;
            else
                energyFill.color = Color.red;
        }
    }

    /// <summary>
    /// Recargar energía
    /// </summary>
    public void AddEnergy(float amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }

    /// <summary>
    /// Quitar energía
    /// </summary>
    public void RemoveEnergy(float amount)
    {
        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }
}