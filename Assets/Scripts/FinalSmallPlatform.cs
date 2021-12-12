using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSmallPlatform : MonoBehaviour
{
    // when enemy or player enter platform, make them platoform's children
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.transform.parent = null;
            other.gameObject.transform.parent = transform;
        }
    }

    // when enemy or player exit platform, remove them from platform's children
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.transform.parent = null;
        }
    }
}
