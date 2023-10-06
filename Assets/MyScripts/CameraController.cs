using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
   
    private Vector3 cameraOffset = new Vector3(-2.24f, 3.15f, 7.82f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
        // Positioning camera in Isometrci view.
        transform.position = player.transform.position + cameraOffset;

        Vector3 relativePos = player.transform.position - transform.position;
        Quaternion reotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = reotation;

        //Debug.Log(player.transform.rotation.eulerAngles.y);
        //Vector3 offset = Quaternion.AngleAxis(player.transform.rotation.eulerAngles.y * player.GetComponent<PlayerController>().turningSpeed, Vector3.up) * cameraOffset;
        //transform.position = player.transform.position + offset;
        transform.LookAt(player.transform.position);
    }

    // Update is called once per frame  
    void Update()
    {

    }

    private void LateUpdate()
    {
       // transform.position = player.transform.position + cameraOffset;
       
       
    }
}
//Quaternion.AngleAxis(player.transform.rotation.eulerAngles.y * player.GetComponent<PlayerController>().turningSpeed, Vector3.up) * cameraOffset; --> Use of last multiplication?