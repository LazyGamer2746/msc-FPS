using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float HP = 25f;
    public float damage = 10f;
    public float attackInterval = 1f;

    private float attackTimer = 0f;

    private NavMeshAgent agent;
    private Transform player;
    private PlayerHealth playerHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (player == null) return;

        // Always follow player
        agent.SetDestination(player.position);

        attackTimer += Time.deltaTime;
    }

    public void TakeDamage(float dmg)
    {
        HP -= dmg;
        if (HP <= 0)
        {
            PlayerHealth.OnEnemyKilled();  // <-- for adrenaline + last stand revive
            Destroy(gameObject);
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (attackTimer >= attackInterval)
            {
                playerHealth.TakeDamage(damage);
                attackTimer = 0f;
            }
        }
    }
}

