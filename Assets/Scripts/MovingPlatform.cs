using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 StartPoint = new Vector3(-5, 0, 0);
    [SerializeField] private Vector3 EndPoint = new Vector3(5, 0, 0);
    [SerializeField] private float speed = 2.0f;

    private bool moveToEnd = true;

    void FixedUpdate()
    {
        Vector3 target = moveToEnd ? EndPoint : StartPoint;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            moveToEnd = !moveToEnd;
        }
    }
}
