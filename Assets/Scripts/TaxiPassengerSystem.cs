using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaxiPassengerSystem : MonoBehaviour
{
    // --- NUEVO: TEXTO EN PANTALLA ---
    [Header("UI (Interfaz de Usuario)")]
    public TextMeshProUGUI scoreText; 
    // --------------------------------
    [Header("Modo Oscuro / La Horda")]
    public Light directionalLight;      
    public GameObject[] monsters;       
    public float spawnDistance = 15f;   
    public float monsterSeparation = 4f;

    [Header("Luces del Carro")]
    public CarLightSystem carLightSystem;  

    [Header("Referencias")]
    public ArrowIndicator arrow;         
    public Transform destinationMarker;  

    // --- SISTEMA ALEATORIO PASAJEROS ---
    [Header("Generación de Pasajeros")]
    [Tooltip("Arrastra aquí el PREFAB de tu pasajero")]
    public GameObject passengerPrefab;       
    [Tooltip("Puntos en la calle donde pueden aparecer esperando el taxi")]
    public List<Transform> spawnPoints;      
    [Tooltip("Puntos en la ciudad donde los pasajeros quieren bajarse")]
    public List<Transform> destinationPoints;
    // --------------------------------

    private Passenger currentPassenger;
    private int passengersPickedUpCount = 0; // Cuenta los que hemos recogido en total
    private int passengersDeliveredCount = 0;// Cuenta pasajeros entregados (Para los puntos)
    private bool hasPassenger = false;   

    void Start()
    {
        if (destinationMarker != null)
            destinationMarker.gameObject.SetActive(false);

        // Actualizamos el marcador a 0 apenas empieza el juego
        UpdateScoreUI();
        // Al iniciar el juego, crea el primer pasajero
        SpawnRandomPassenger(); 
    }

    void SpawnRandomPassenger()
    {
        // Verificación de seguridad
        if (spawnPoints.Count == 0 || destinationPoints.Count == 0 || passengerPrefab == null)
        {
            Debug.LogWarning("¡Faltan puntos o el Prefab del pasajero en el Inspector!");
            return;
        }

        // Eleccion de un punto de aparición al azar
        Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // Eleccion de destino al azar
        Transform randomDest = destinationPoints[Random.Range(0, destinationPoints.Count)];

        // clonacion pasajero en ese punto
        GameObject newPassenger = Instantiate(passengerPrefab, randomSpawn.position, randomSpawn.rotation);
        currentPassenger = newPassenger.GetComponent<Passenger>();

        // Inyectamos su destino deseado
        currentPassenger.destination = randomDest;

        // la flecha guía hacia él
        arrow.target = currentPassenger.transform;

        Debug.Log("Nuevo pasajero esperando en: " + randomSpawn.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Passenger") && !hasPassenger)
        {
            Passenger p = other.GetComponent<Passenger>();
            if (p == currentPassenger)
                PickUpPassenger(p);
        }

        if (other.CompareTag("Destination") && hasPassenger)
        {
            DropPassenger();
        }
    }

    void PickUpPassenger(Passenger passenger)
    {
        hasPassenger = true;
        passengersPickedUpCount++; // Sumamos 1 a nuestra cuenta histórica

        passenger.PickUp(); 

        if (destinationMarker != null && passenger.destination != null)
        {
            destinationMarker.position = passenger.destination.position;
            destinationMarker.gameObject.SetActive(true);
        }

        arrow.target = passenger.destination;
        Debug.Log("Pasajero recogido. Llevas en total: " + passengersPickedUpCount);

        // --- INVOCACIÓN DE LA HORDA ---
        // Si acabamos de recoger al SEGUNDO pasajero, apagamos las luces
        if (passengersPickedUpCount == 2)
        {
            if (carLightSystem != null)
                carLightSystem.AddEnergy(carLightSystem.maxEnergy);

            ActivateDarkMode();
        }
    }

    void DropPassenger()
    {
        hasPassenger = false;

        if (destinationMarker != null)
            destinationMarker.gameObject.SetActive(false);

        // Destruimos al pasajero viejo para que no se acumule basura en la memoria
        if (currentPassenger != null)
        {
            Destroy(currentPassenger.gameObject);
        }
        // --- SUMAR PUNTO AL ENTREGAR ---
        passengersDeliveredCount++; // suma 1 a los entregados
        UpdateScoreUI();            // Actualiza el texto en pantalla
        // --------------------------------------
        Debug.Log("Pasajero entregado. Generando el siguiente...");

        // Llamamos al ciclo de nuevo para que el juego sea infinito
        SpawnRandomPassenger(); 
    }
    // --- ACTUALIZAR LA PANTALLA ---
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Pasajeros: " + passengersDeliveredCount;
        }
    }
    // -------------------------------------------------

    void ActivateDarkMode()
    {
        Debug.Log("MODO OSCURO ACTIVADO - ¡ESCAPA DE LA HORDA!");

        if (directionalLight != null)
            directionalLight.intensity = 0f;

        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i] != null)
            {
                Vector3 baseSpawnPosition = transform.position + (transform.forward * spawnDistance);

                float lateralOffset = 0f;
                if (i == 1) lateralOffset = -monsterSeparation; 
                else if (i == 2) lateralOffset = monsterSeparation;  

                Vector3 finalPosition = baseSpawnPosition + (transform.right * lateralOffset);
                finalPosition.y = monsters[i].transform.position.y; 

                monsters[i].transform.position = finalPosition;
                monsters[i].transform.LookAt(transform.position); 
                monsters[i].SetActive(true); 
            }
        }
    }
}