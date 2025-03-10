using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    Vector3 startPos;
    public float moveDistance = 10f;
    public float movingSpeed;

    private void Start()
    {
        startPos = transform.position;
    }
    private void Update()
    {
        Move_Platform();
    }

    void Move_Platform()
    {
        float newX = startPos.x + Mathf.PingPong(Time.time * movingSpeed, moveDistance);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && collision.transform.position.y > transform.position.y)
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
