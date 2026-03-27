using UnityEngine;
using UnityEngine.InputSystem; // Requerido para el nuevo Input System

public class CarController : MonoBehaviour
{
    [Header("MOVEMENT")]
    public float speed = 15f;
    public float turnSpeed = 100f;

    // Referencias al Nuevo Input System (Cambiado a InputSystem_Actions)
    private InputSystem_Actions inputActions;
    private Vector2 moveInputVector;

    // Powerup variables
    private float originalSpeed; 
    private Coroutine speedCoroutine; 

    private void Awake()
    {
        // Inicializamos las acciones usando el nombre exacto del Asset de Unity 6
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        // Activamos el mapa de acciones "Player"
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        // Desactivamos para evitar errores cuando el objeto no esté activo
        inputActions.Player.Disable();
    }

    void Start()
    {
        // Guardamos la velocidad inicial
        originalSpeed = speed;
    }

    void Update()
    {
        // Leemos el valor del Vector2 del Input Action "Move"
        // Asegúrate de que en tu Asset el Action Map se llame 'Player' y la Acción 'Move'
        moveInputVector = inputActions.Player.Move.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Move();
        Turn();
    }

    void Move()
    {
        // moveInputVector.y es el eje vertical (W/S o flechas Arriba/Abajo)
        transform.Translate(Vector3.forward * moveInputVector.y * speed * Time.fixedDeltaTime);
    }

    void Turn()
    {
        // moveInputVector.x es el eje horizontal (A/D o flechas Izquierda/Derecha)
        transform.Rotate(Vector3.up * moveInputVector.x * turnSpeed * Time.fixedDeltaTime);
    }

    // --- Lógica de PowerUp (Intacta para no romper tus mecánicas) ---
    public void ActivateSpeedBoost(float multiplier, float duration)
    {
        if (speedCoroutine != null) StopCoroutine(speedCoroutine);
        speedCoroutine = StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }

    private System.Collections.IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        speed = originalSpeed * multiplier;
        Debug.Log("¡Turbo activado! Nueva velocidad: " + speed);

        yield return new WaitForSeconds(duration); 

        speed = originalSpeed;
        Debug.Log("Turbo apagado. Velocidad normal: " + speed);
    }
}