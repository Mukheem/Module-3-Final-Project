using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
   
    public Vector3 cameraOffset = new Vector3(0, 2.83f, -5.65f);


    /*
     * This method holds the Camera at Isometric view and gives a smoother transition when player is moved around.
     */
    private void LateUpdate()
    {
        // Positioning camera in Isometrci view.
        transform.position = player.transform.position + cameraOffset;

    }
}
