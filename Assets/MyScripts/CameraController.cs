using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
   
    public Vector3 cameraOffset = new Vector3(0,4.0f, -10.0f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
       
    }

    // Update is called once per frame  
    void Update()
    {

    }

    private void LateUpdate()
    {
        // Positioning camera in Isometrci view.
        transform.position = player.transform.position + cameraOffset;

        // Code reference: https://stackoverflow.com/questions/56382451/how-to-rotate-camera-around-player-in-unity-c-sharp
        Vector3 offset = Quaternion.AngleAxis(player.transform.rotation.eulerAngles.y * player.GetComponent<PlayerController>().turningSpeed, Vector3.up) * cameraOffset;
        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform.position);


    }
}
//Quaternion.AngleAxis(player.transform.rotation.eulerAngles.y * player.GetComponent<PlayerController>().turningSpeed, Vector3.up) * cameraOffset; --> Use of last multiplication?