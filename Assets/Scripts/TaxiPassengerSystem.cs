using System.Collections.Generic;
using UnityEngine;

public class TaxiPassengerSystem : MonoBehaviour
{
    [Header("Modo Oscuro")]
    public Light directionalLight;      // Luz global del sol
    public GameObject monster;          // Monstruo que se activa en el modo oscuro

    public float spawnDistance = 15f;   // Cantidad de metros detras del taxi en el que aparecera el bicho

    [Header("Luces del Carro")]
    public CarLightSystem carLightSystem;  // Referencia al sistema de luces del carro

    [Header("Referencias")]
    public ArrowIndicator arrow;         // Flecha indicadora
    public Transform destinationMarker;  // Marcador visual de destino

    [Header("Pasajeros")]
    public List<Passenger> passengers;   // Lista de pasajeros

    private Passenger currentPassenger;
    private int currentIndex = 0;
    private bool hasPassenger = false;   // Si actualmente hay un pasajero a bordo

    void Start()
    {
        // Ocultar marcador al inicio
        if (destinationMarker != null)
            destinationMarker.gameObject.SetActive(false);

        SetNextPassenger(); // Activar primer pasajero
    }

    /// <summary>
    /// Activa al siguiente pasajero en la lista
    /// </summary>
    void SetNextPassenger()
    {
        if (currentIndex >= passengers.Count)
        {
            Debug.Log("No hay más pasajeros");
            arrow.target = null;
            return;
        }

        currentPassenger = passengers[currentIndex];
        currentPassenger.gameObject.SetActive(true);

        // Flecha apunta al pasajero
        arrow.target = currentPassenger.transform;

        Debug.Log("Nuevo pasajero activado");
    }

    private void OnTriggerEnter(Collider other)
    {
        // RECOGER PASAJERO
        if (other.CompareTag("Passenger") && !hasPassenger)
        {
            Passenger p = other.GetComponent<Passenger>();
            if (p == currentPassenger)
                PickUpPassenger(p);
        }

        // ENTREGAR PASAJERO
        if (other.CompareTag("Destination") && hasPassenger)
        {
            DropPassenger();
        }
    }

    /// <summary>
    /// Función al recoger un pasajero
    /// </summary>
    void PickUpPassenger(Passenger passenger)
    {
        hasPassenger = true;
        passenger.PickUp();

        // Activar marcador de destino
        if (destinationMarker != null && passenger.destination != null)
        {
            destinationMarker.position = passenger.destination.position;
            destinationMarker.gameObject.SetActive(true);
        }

        // Flecha apunta al destino
        arrow.target = passenger.destination;

        Debug.Log("Pasajero recogido");

        // Si es el segundo pasajero (índice 1)
        if (currentIndex == 1)
        {
            // Recargar luces del carro al máximo
            if (carLightSystem != null)
                carLightSystem.AddEnergy(carLightSystem.maxEnergy);

            // Activar modo oscuro
            ActivateDarkMode();
        }
    }

    /// <summary>
    /// Función al entregar un pasajero
    /// </summary>
    void DropPassenger()
    {
        hasPassenger = false;

        if (destinationMarker != null)
            destinationMarker.gameObject.SetActive(false);

        currentIndex++; // Pasar al siguiente pasajero
        Debug.Log("Pasajero entregado");

        SetNextPassenger(); // Activar siguiente pasajero
    }

    /// <summary>
    /// Modo oscuro: apaga luz global y activa el monstruo
    /// </summary>
    void ActivateDarkMode()
    {
        Debug.Log("MODO OSCURO ACTIVADO");

        // Apagar luz del sol
        if (directionalLight != null)
            directionalLight.intensity = 0f;

        // Activar monstruo
        /*if (monster != null)
            monster.SetActive(true);
        */
        // --- APARICION DEL MONSTRUO CERCA DEL TAXI ---
        if (monster != null)
        {
            // Calcula posicion detrás del taxi
            // transform.position es el taxi, transform.forward es hacia adelante (+ adelante del carro - detras)
            Vector3 spawnPosition = transform.position + (transform.forward * spawnDistance);
            
            // Mantener la altura Y original del monstruo para que no aparezca enterrado o flotando
            spawnPosition.y = monster.transform.position.y;

            // Transportar al monstruo a esa posicion
            monster.transform.position = spawnPosition;
            
            // El monstruo mira directamente al taxi al aparecer
            monster.transform.LookAt(transform.position);

            // Activa la bestia!
            monster.SetActive(true);
        }
    }
}