using System.Collections;
using UnityEngine;

public class Rikayon : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;
    public Transform player; // Taxi

    [Header("Movimiento")]
    public float speed = 5f;

    //****Se añade para que se reponga luego de un ataque
    [Header("Ataque")]
    public float attackRecoveryTime = 2f; // Segundos que tarda el bicho en volver a moverse tras atacar
    public float attackRange = 3.5f;      // NUEVO: Distancia a la que el bicho ataca (ajustable en el Inspector)

    private bool isChasing = false;
    private bool isAttacking = false;
    //****

    void Start()
    {
        // ¡El bicho empieza a perseguir INMEDIATAMENTE al ser activado!
        isChasing = true;
        animator.SetBool("isWalking", true);
    }

    // Nota: ¡Si en algún momento planeas apagar y volver a prender al bicho varias veces 
    // en la misma partida, cambia "void Start()" por "void OnEnable()"!

    void Update()
    {
        // Si está atacando, no se mueve
        if (!isChasing || isAttacking) return;

        // Verifica distancia evita el error de colisión de Unity
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer(player.gameObject);
            return; // Detiene el Update para que no siga caminando este frame
        }

        ChasePlayer();
    }

    void ChasePlayer()
    {
        // Dirección hacia el jugador (sin inclinación)
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        // Rotar hacia el jugador
        transform.rotation = Quaternion.LookRotation(direction);

        // Moverse hacia el jugador
        transform.position += direction * speed * Time.deltaTime;
    }

    //  DETECTAR COLISIÓN CON EL TAXI 
    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.CompareTag("Player"))
        {
            AttackPlayer();
        }*/
        //**** Si choca contra el Taxi Y no esta atacando
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            AttackPlayer(collision.gameObject);
        }
        //****
    }

    /*void AttackPlayer()
    {
        if (isAttacking) return;

        isAttacking = true;

        // Detener movimiento
        animator.SetBool("isWalking", false);

        // Activar animación de ataque
        animator.SetTrigger("Attack");

        // Aquí luego puedes poner Game Over
        Debug.Log("Jugador atrapado");
    }*/

    //****Mejora ataque bicho
    void AttackPlayer(GameObject taxi)
    {
        isAttacking = true;

        // Detiene movimiento
        animator.SetBool("isWalking", false);

        // Activa animación de ataque
        animator.SetTrigger("Attack");

        // --- APLICA DAÑO AL TAXI ---
        PlayerHealth taxiHealth = taxi.GetComponent<PlayerHealth>();
        if (taxiHealth != null)
        {
            taxiHealth.LoseLife(); // Le quitamos una vida
        }
        else 
        {
            Debug.LogWarning("¡El bicho atrapó algo, pero no encontró el script PlayerHealth en el Taxi!");
        }

        Debug.Log("¡Jugador atrapado por el bicho!");

        // Inicia corrutina para que el monstruo vuelva a la normalidad
        StartCoroutine(RecoverFromAttack());
    }

    // Temporizador para que monstruo vuelva a perseguir al player si aun quedan vidas
    IEnumerator RecoverFromAttack()
    {
        // Espera a que termine animación de ataque y un poquito de cooldown
        yield return new WaitForSeconds(attackRecoveryTime);

        // El bicho vuelve a la normalidad
        isAttacking = false;
        animator.SetBool("isWalking", true); // Vuelve a caminar
    }
    //****
}