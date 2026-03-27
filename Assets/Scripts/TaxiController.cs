using UnityEngine;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(Rigidbody))]
public class TaxiController : MonoBehaviour
{
    [Header("CONFIGURACIÓN DEL TAXI")]
    public float speed = 10f;      
    public float turnSpeed = 100f; 
    //***Para powerup aumento de velocidad
    private float originalSpeed; // Guarda velocidad normal
    private Coroutine speedCoroutine; // controla el temporizador
    //***hasta aca powerup aumento velocidad

    private Rigidbody rb;
    private Vector2 inputVector;   

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Baja el centro de masa para que no vuelque en las curvas
        rb.centerOfMass = new Vector3(0, -0.5f, 0); 
        //***Para powerup aumento de velocidad
        originalSpeed = speed;
        //***hasta aca powerup aumento velocidad
    }

    // llama a esta funcion automaticamente al tocar WASD
    public void OnMove(InputValue value)
    {
        // Guardamos el vector (X es Izquierda/Derecha, Y es Arriba/Abajo)
        inputVector = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        MoveTaxi();
        TurnTaxi();
    }

    private void MoveTaxi()
    {
        // inputVector.y es el acelerador (W/S)
        Vector3 moveDirection = transform.forward * inputVector.y * speed;
        
        // Aplica velocidad manteniendo la gravedad intacta
        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
    }

    private void TurnTaxi()
    {
        // Solo giramos si el carro esta avanzando o retrocediendo
        if (inputVector.y != 0)
        {
            // inputVector.x volante (A/D)
            float turnAmount = inputVector.x * turnSpeed * Time.fixedDeltaTime;
            
            // Si es reversa, invierte el volante para que sea natural
            if (inputVector.y < 0) turnAmount *= -1; 
            
            Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
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
