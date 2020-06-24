﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour 
{
    public Animator animator;

    [Tooltip("Speed of the enemy")]
    public float MovementSpeed;

    [Tooltip("Damage on player on touch")]
    public int ContactDamage;

    [Tooltip("Health of the enemy")]
    public int HealthPoint;

    [Tooltip("Score reward for destorying enemy")]
    public int ScoreReward;

    [Tooltip("Sound upon death")]
    public AudioClip DeathAudioClip;

    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;

    // Use this for initialization
    void Start () {
        playerTransform = GameObject.Find("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (playerTransform != null && !GameManager.Instance.isGameOver)
        {
            navMeshAgent.SetDestination(playerTransform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bullet")) {
            BulletScript bulletScript = collision.gameObject.GetComponent<BulletScript>();
            HealthPoint -= bulletScript.BulletDamage;

            Destroy(bulletScript.gameObject);

            if (HealthPoint <= 0) {
                Dead();
            }
        }
    }

    private void Dead() {
        GameManager.Instance.UpdateScore(ScoreReward, DeathAudioClip);
        Destroy(gameObject);
    }
}
