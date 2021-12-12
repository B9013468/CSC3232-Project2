using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Use for enemy's hp bar */
public class CanvasView3D : MonoBehaviour
{
    public Transform camera;

    void LateUpdate()
    {
        transform.LookAt(transform.position + camera.forward); // point canvas to the direction of the camera
    }
}
