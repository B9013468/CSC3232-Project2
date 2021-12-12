using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles Camera movement making sure it always follows player */
public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform playerView;

    // Update is called once per frame
    void Update()
    {
        transform.position = playerView.position; // set camera pisition to playerView's position
    }
}
