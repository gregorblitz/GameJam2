using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("MOVEMENT")]
    public float speed = 15f;
    public float turnSpeed = 100f;

    //***Para powerup aumento de velocidad
    private float originalSpeed; // Guarda velocidad normal
    private Coroutine speedCoroutine; // controla el temporizador
    //***hasta aca powerup aumento velocidad

    private float moveInput;
    private float turnInput;
    void Start()
    {
        // Guardamos la velocidad inicial al comenzar
        originalSpeed = speed;
    }
    void Update()
    {
        // Input de teclado
        moveInput = Input.GetAxis("Vertical");   // W/S
        turnInput = Input.GetAxis("Horizontal"); // A/D
    }

    void FixedUpdate()
    {
        Move();
        Turn();
    }

    void Move()
    {
        // Movimiento hacia adelante
        transform.Translate(Vector3.forward * moveInput * speed * Time.fixedDeltaTime);
    }

    void Turn()
    {
        // Giro del carro
        transform.Rotate(Vector3.up * turnInput * turnSpeed * Time.fixedDeltaTime);
    }
    //*** Función que llama el PowerUp
    public void ActivateSpeedBoost(float multiplier, float duration)
    {
        // Si ya tiene un turbo activo, reinicia para que no se buguee
        if (speedCoroutine != null) StopCoroutine(speedCoroutine);
        
        // Inicia el temporizador
        speedCoroutine = StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }

    // El temporizador
    private System.Collections.IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        
        speed = originalSpeed * multiplier; // Multiplicamos la velocidad
        Debug.Log("¡Turbo activado! Nueva velocidad: " + speed);

        yield return new WaitForSeconds(duration); 

        speed = originalSpeed; // Devuelve velocidad a la normalidad
        Debug.Log("Turbo apagado. Velocidad normal: " + speed);
    }
    //***hasta aca powerup aumento velocidad
}