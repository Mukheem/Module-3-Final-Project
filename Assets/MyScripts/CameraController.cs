using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    //new Vector3(96.73f, 23.54f, -57.49f); 98.97789
    private Vector3 cameraOffset = new Vector3(96.73f, 23.54f, -57.49f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position + new Vector3(-2.24f, 3.15f, 7.82f);
        //transform.rotation = Quaternion.Euler(0,162.43f, 0);
    }
}
