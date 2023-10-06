using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private float horizontalMovement;
    private float forwardMovement;
    [SerializeField]
    private float movingSpeed = 1;
    public float turningSpeed = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        forwardMovement = Input.GetAxis("Vertical");

        transform.Translate(Vector3.forward * Time.deltaTime * movingSpeed * forwardMovement);
        transform.Rotate(Vector3.up,horizontalMovement * turningSpeed);
    }
}

// transform.Rotate(Vector3.up,horizontalMovement * turningSpeed * Time.deltaTime); -- Not working
// How to make the player stand on the ground/Snow
