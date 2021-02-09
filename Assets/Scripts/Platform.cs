using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform Pos1, Pos2;
    public float Speed;
    public Transform StartPos;
    private Vector3 _nextPos;

    void Start()
    {
        _nextPos = StartPos.position;
    }

    
    void Update()
    {
        if (transform.position==Pos1.position)
        {
            _nextPos = Pos2.position;
        }

        if (transform.position==Pos2.position)
        {
            _nextPos = Pos1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, _nextPos, Speed * Time.deltaTime);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Pos1.position,Pos2.position);
    }
}
