using UnityEngine;

public class CarLightSystem : MonoBehaviour
{
    [Header("REFERENCIAS DE LUCES")]
    // Asigna aquí las dos farolas del carro desde el inspector
    public Light leftLight;
    public Light rightLight;

    [Header("ENERGÍA")]
    public float maxEnergy = 100f;      // Energía máxima
    public float currentEnergy;         // Energía actual

    [Header("CONSUMO")]
    public float drainRate = 5f;        // Energía que se consume por segundo

    [Header("CONFIGURACIÓN DE LUZ")]
    public float maxIntensity = 2f;     // Intensidad máxima de la luz
    public float minIntensity = 0f;     // Intensidad mínima (cuando se apaga)

    public float maxRange = 25f;        // Alcance máximo de la luz
    public float minRange = 5f;         // Alcance mínimo

    [Header("EFECTOS")]
    public bool enableFlicker = true;   // Activar parpadeo cuando la energía es baja
    public float flickerThreshold = 0.2f; // % de energía para empezar a parpadear

    void Start()
    {
        // Al iniciar, la luz comienza con energía completa
        currentEnergy = maxEnergy;
    }

    void Update()
    {
        DrainEnergy();
        UpdateLights();
    }

    /// <summary>
    /// Reduce la energía con el tiempo
    /// </summary>
    void DrainEnergy()
    {
        currentEnergy -= drainRate * Time.deltaTime;

        // Evita que se pase de los límites
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }

    /// <summary>
    /// Actualiza intensidad, alcance y efectos de las luces
    /// </summary>
    void UpdateLights()
    {
        // Normalizamos la energía (0 a 1)
        float normalized = currentEnergy / maxEnergy;

        // Calculamos intensidad y alcance según la energía
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, normalized);
        float range = Mathf.Lerp(minRange, maxRange, normalized);

        // Aplicamos valores a ambas luces
        leftLight.intensity = intensity;
        rightLight.intensity = intensity;

        leftLight.range = range;
        rightLight.range = range;

        //  Parpadeo cuando la energía está baja
        if (enableFlicker && normalized < flickerThreshold && currentEnergy > 0)
        {
            float flicker = Random.Range(0.8f, 1.2f);

            leftLight.intensity *= flicker;
            rightLight.intensity *= flicker;
        }

        //  Apagar completamente si no hay energía
        if (currentEnergy <= 0)
        {
            leftLight.enabled = false;
            rightLight.enabled = false;
        }
        else
        {
            leftLight.enabled = true;
            rightLight.enabled = true;
        }
    }

    /// <summary>
    /// Función para recargar energía (usar con pickups)
    /// </summary>
    public void AddEnergy(float amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }

    /// <summary>
    /// (Opcional) Función para quitar energía (por efectos negativos)
    /// </summary>
    public void RemoveEnergy(float amount)
    {
        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }
}