using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement Variables
    private float horizontalMovement;
    private float forwardMovement;
    public float movingSpeed = 1;
    public float turningSpeed = 1;

    // Component Variables
    private Animator playerAnimator;
    private Rigidbody playerRigidBody;
    private CharacterController playerCharacterController;

    //Miscellaneous
    public float gravityModifier;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody>();
        playerCharacterController = GetComponent<CharacterController>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement of the player
        horizontalMovement = Input.GetAxis("Horizontal");
        forwardMovement = Input.GetAxis("Vertical");

        Vector3 newMovementPos = new Vector3(horizontalMovement, 0, forwardMovement);

        //transform.Translate(Vector3.forward * Time.deltaTime * movingSpeed * forwardMovement);
        transform.Rotate(Vector3.up, horizontalMovement * turningSpeed);

        //playerCharacterController.Move(newMovementPos * Time.deltaTime * movingSpeed);
        //playerRigidBody.MovePosition(transform.position + newMovementPos);
        //playerRigidBody.AddForce(Vector3.forward * movingSpeed, ForceMode.Acceleration);

        playerRigidBody.velocity = new Vector3(horizontalMovement, 0, forwardMovement);

        // This condition controls the animation of the player while moving.
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        { 
            //playerRigidBody.MovePosition(transform.position + newMovementPos);
            //playerRigidBody.AddForce(Vector3.forward * movingSpeed, ForceMode.Impulse);
            playerAnimator.SetBool("Static_b", false);
            playerAnimator.SetFloat("Speed_f", movingSpeed);
           
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)){
            playerAnimator.SetBool("Static_b", true);
            playerAnimator.SetFloat("Speed_f", 0);
        }
       
        
    }
}

// transform.Rotate(Vector3.up,horizontalMovement * turningSpeed * Time.deltaTime); -- Not working
// How to make the player stand on the ground/Snow
// Why do we need Input.GetAxis when we can use Input.getKeyDown(up arrow)
