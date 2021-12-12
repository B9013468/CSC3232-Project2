using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableAnimation : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 rotationAngle;
    public float rotationSpeed;

    AudioSource collectSound;

    // Use this for initialization
    void Start()
    {
        collectSound = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>(); // get gameobject with needed collect sound
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.up * 5, ForceMode.Impulse); // add force upwards to jump 
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);  
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            collectSound.Play();
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // enable isKinematic and isTrigger only after touches ground
        if (collision.gameObject.CompareTag("Floor"))
        {
            rb.isKinematic = true;
            GetComponent<Collider>().isTrigger = true;
        }
    }
}
