using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    private EnemyControl _enemy;
    private Rigidbody2D _physics;

    void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyControl>();
        _physics = GetComponent<Rigidbody2D>();
        _physics.AddForce(_enemy.GetDirection()*1000);
    }
}
