using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenericEnemy : MonoBehaviour
{
    public float aggroRange;
    private GameObject target;
    private Rigidbody2D rb;

    public LayerMask terrainMask;
    public LayerMask playerMask;

    int counter = 0;


    private void Start()
    {
        target = MoveScript.instance.gameObject;
        rb = GetComponent<Rigidbody2D>();
    }

    public event Action<Vector2> enemyAIUpdate;

    private void FixedUpdate()
    {
        if (counter <= 4)
        {
            counter++;
            return;
        }
        else
        {
            counter = 0;
        }
        Vector2 dir = target.transform.position - transform.position;
        if(dir.magnitude <= aggroRange)
        {
            enemyAIUpdate?.Invoke(dir);
        }
    }

    public Rigidbody2D RigidBody()
    {
        return rb;
    }
}
