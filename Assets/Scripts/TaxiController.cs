using UnityEngine;
using UnityEngine.InputSystem; // Necesario para InputValue

[RequireComponent(typeof(Rigidbody))]
public class TaxiController : MonoBehaviour
{
    [Header("CONFIGURACIÓN DEL TAXI")]
    public float speed = 20f;      // ¡Recuerda subirlo en el Inspector!
    public float turnSpeed = 100f; 

    private Rigidbody rb;
    private Vector2 inputVector;   // Aquí guardaremos el WASD

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Bajamos el centro de masa para que no vuelque en las curvas
        rb.centerOfMass = new Vector3(0, -0.5f, 0); 
    }

    // EL SECRETO: Unity llama a esta función automáticamente cuando tocas WASD
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
        // inputVector.y es nuestro acelerador (W/S)
        Vector3 moveDirection = transform.forward * inputVector.y * speed;
        
        // Aplicamos velocidad manteniendo la gravedad intacta
        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
    }

    private void TurnTaxi()
    {
        // Solo giramos si el carro está avanzando o retrocediendo
        if (inputVector.y != 0)
        {
            // inputVector.x es nuestro volante (A/D)
            float turnAmount = inputVector.x * turnSpeed * Time.fixedDeltaTime;
            
            // Si vamos en reversa, invertimos el volante para que sea natural
            if (inputVector.y < 0) turnAmount *= -1; 
            
            Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }
}
