using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView3d : MonoBehaviour
{
    public float sensX = 1f;
    public float sensY = 1f;
    public float adjuster = 1f;

    float xRot;
    float yRot;

    [SerializeField] Camera playerCamera;
    [SerializeField] Transform weapon;
    [SerializeField] Transform orientation;

    float xMouse;
    float yMouse;

    void Update()
    {
        if (!Levels.GamePaused)
        {
            Cursor.lockState = CursorLockMode.Locked; // lock cursor on the center of the screen
            Cursor.visible = false; // hide cursor

            xMouse = Input.GetAxisRaw("Mouse X"); // get mouse x
            yMouse = Input.GetAxisRaw("Mouse Y"); // get mouse y

            yRot += xMouse * sensX * adjuster; // use xMouse because when rotate on the y-axis we rotate horizontally (use addition for normal rotation) (use substraction for inverted rotation)
            xRot -= yMouse * sensY * adjuster; // use yMouse because when rotate on the x-axis we rotate vertically (use substraction for normal rotation) (use addition for inverted rotation)

            xRot = Mathf.Clamp(xRot, -90f, 90f); // clamp x-rotation to -90 and 90 degrees so the player cannot look too far up or down

            playerCamera.transform.localRotation = Quaternion.Euler(xRot, yRot, 0); // pass to camera y and x rotation (view uses 2d, x & y)

            orientation.transform.rotation = Quaternion.Euler(0, yRot, 0); // pass to player only yRot because we want him to rotate only on y-axis
        }
        else
        {
            Cursor.lockState = CursorLockMode.None; // lock cursor on the center of the screen
            Cursor.visible = true; // hide cursor
        }
        
    }
}
