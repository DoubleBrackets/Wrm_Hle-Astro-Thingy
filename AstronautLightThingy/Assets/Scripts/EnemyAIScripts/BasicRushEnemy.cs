using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRushEnemy : MonoBehaviour
{

    public float moveForce;
    public float maxVel;
    private GenericEnemy enemyBase;

    private void Awake()
    {
        enemyBase = GetComponentInParent<GenericEnemy>();
        enemyBase.enemyAIUpdate += RushEnemyUpdate;
        moveForce += Random.Range(-0.5f, 1f);
        maxVel += Random.Range(0.5f, 0.5f);
    }

    void RushEnemyUpdate(Vector2 dir)
    {
        Rigidbody2D rb = enemyBase.RigidBody();
        rb.AddForce(dir.normalized * moveForce*5);
        if (rb.velocity.magnitude > maxVel)
            rb.velocity = rb.velocity.normalized * maxVel;
    }

}
