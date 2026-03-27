using System.Collections;
using UnityEngine;

public class Rikayon : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;
    public Transform player;

    [Header("Movimiento")]
    public float speed = 5f;

    [Header("Inicio")]
    public float delayBeforeChase = 5f;

    [Header("Ataque")]
    public float attackRecoveryTime = 2f;
    public float attackRange = 3.5f;

    [Header("PELIGRO UI")]
    public GameObject dangerFlash;
    public float dangerDistance = 12f;
    public float flickerSpeed = 8f;

    [Header("HORDA (TELETRANSPORTE)")]
    public float maxDistanceToTeleport = 50f;
    public float minRadius = 15f;
    public float maxRadius = 25f;

    private bool isChasing = false;
    private bool isAttacking = false;

    private CanvasGroup dangerCanvas;

    void Start()
    {
        StartCoroutine(StartChase());
    }

    void OnEnable()
    {
        if (dangerFlash == null)
            dangerFlash = GameObject.Find("DangerFlash");

        if (dangerFlash != null)
        {
            dangerCanvas = dangerFlash.GetComponent<CanvasGroup>();

            if (dangerCanvas == null)
                dangerCanvas = dangerFlash.AddComponent<CanvasGroup>();

            dangerCanvas.alpha = 0f; // 🔥 inicia limpio
        }
    }

    IEnumerator StartChase()
    {
        yield return new WaitForSeconds(delayBeforeChase);

        isChasing = true;
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        if (!isChasing || isAttacking) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 🔥 Efecto pantalla roja
        UpdateDangerEffect(distanceToPlayer);

        // 🔥 Teletransporte de horda
        if (distanceToPlayer >= maxDistanceToTeleport)
        {
            TriggerHordeTeleport();
            return;
        }

        // 🔥 Ataque
        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer(player.gameObject);
            return;
        }

        // 🔥 Persecución
        ChasePlayer();
    }

    void UpdateDangerEffect(float distance)
    {
        if (dangerCanvas == null) return;

        float normalized = Mathf.Clamp01(1 - (distance / dangerDistance));
        float alpha = normalized;

        // Parpadeo cerca
        if (distance < dangerDistance * 0.5f)
        {
            float flicker = Mathf.Sin(Time.time * flickerSpeed) * 0.5f + 0.5f;
            alpha *= flicker;
        }

        dangerCanvas.alpha = alpha;
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        transform.rotation = Quaternion.LookRotation(direction);
        transform.position += direction * speed * Time.deltaTime;
    }

    // 🔥 ACTIVAR TELETRANSPORTE DE TODA LA HORDA
    void TriggerHordeTeleport()
    {
        Rikayon[] horda = FindObjectsOfType<Rikayon>();

        for (int i = 0; i < horda.Length; i++)
        {
            horda[i].ExecuteTeleport(i, horda.Length);
        }

        Debug.Log("¡La horda rodea al jugador!");
    }

    // 🔥 TELETRANSPORTE EN FORMACIÓN CIRCULAR
    public void ExecuteTeleport(int index, int total)
    {
        float angle = (360f / total) * index;

        Vector3 direction = new Vector3(
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0,
            Mathf.Cos(angle * Mathf.Deg2Rad)
        );

        float radius = Random.Range(minRadius, maxRadius);

        Vector3 newPosition = player.position + direction * radius;
        newPosition.y = transform.position.y;

        transform.position = newPosition;

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            AttackPlayer(collision.gameObject);
        }
    }

    void AttackPlayer(GameObject taxi)
    {
        isAttacking = true;

        animator.SetBool("isWalking", false);
        animator.SetTrigger("Attack");

        PlayerHealth taxiHealth = taxi.GetComponent<PlayerHealth>();
        if (taxiHealth != null)
        {
            taxiHealth.LoseLife();
        }

        Debug.Log("¡Jugador atrapado!");

        StartCoroutine(RecoverFromAttack());
    }

    IEnumerator RecoverFromAttack()
    {
        yield return new WaitForSeconds(attackRecoveryTime);

        isAttacking = false;
        animator.SetBool("isWalking", true);
    }
}