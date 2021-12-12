using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [SerializeField]
    float speed = 5.00f;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        Rigidbody b = GetComponent<Rigidbody>();
        Vector3 velocity = b.velocity;

        if (Input.GetKey(KeyCode.A))
        {
            velocity.x = -speed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            velocity.x = speed;
        }

        if (Input.GetKey(KeyCode.W))
        {
            velocity.z = speed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            velocity.z = -speed;
        }

        b.velocity = velocity;
    }
}
