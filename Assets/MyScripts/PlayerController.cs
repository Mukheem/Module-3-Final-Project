using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    //Movement Variables
    private float horizontalMovement;
    private float forwardMovement;
    public float movingSpeed = 2000;
    public float turningSpeed = 0.5f;
    public float jumpSpeed = 50;
    

    // Component Variables
    private Animator playerAnimator;
    public ParticleSystem playerExplosionParticleFX;
    public ParticleSystem playerDirtSplatterParticlFX;
    public ParticleSystem playerPowerupFX;
    private AudioSource gameBGM;
    public AudioClip playerDeathSound;
    public AudioClip playerPowerupCollectionSound;
    private AudioSource playerAudioSource;
    private SpawnManager spawnManager;
    private Rigidbody playerRB;
    


    // Misellaneous
    public float gravityModifier;
    public ProgressBarCircle playerHealth;
    private int playerHealthCountdownTimeLimit = 8; //Every 8 seconds, Player looses his/her health.
    public bool isGameOver;
    public bool isPlayerOnGround;
    public float velocityLimit = 2.0f; // TO control player speed when he is in the air.
    private int appleHealth = 15;
    private int milkHealth = 25;
    private int cookieHealth = 10;
    private int bigbottleHealth = 30;
    private int moneyHealth = 25;


    // Start is called before the first frame update
    void Start()
    {
        //Player Start Position 
       //s transform.position = new Vector3(98.0f, 24f, -65f);
        playerAnimator = GetComponent<Animator>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        Physics.gravity *= gravityModifier;
        playerRB = GetComponent<Rigidbody>();

        playerHealth.BarValue = 99;//Health value Initialisation
        StartCoroutine("PlayerHealthCounter");

        isGameOver = false;

        gameBGM = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        playerAudioSource = GetComponent<AudioSource>();

        //Disabling the powerup ParticleFX
        playerPowerupFX.transform.parent.gameObject.SetActive(false);

    }


    
    // Update is called once per frame
    void Update()
    {
        
        // Movement of the player
        horizontalMovement = Input.GetAxis("Horizontal");
        forwardMovement = Input.GetAxis("Vertical");

        // This condition controls the rotation of the player while moving.
        transform.Rotate(Vector3.up, horizontalMovement * turningSpeed);

        //Making sure that Indicator is always showing right color
        healthColurIndicator();

        //if (Input.GetKey(KeyCode.Space) && isPlayerOnGround && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
        //{
        //    Vector3 moveNew = new Vector3(transform.position.x,transform.position.y,transform.position.z);
        //    Debug.Log("Inside Combination");
        //    isPlayerOnGround = false;
        //    playerRB.AddForce(transform.up * Time.deltaTime * jumpSpeed, ForceMode.Impulse);

        //    transform.Translate(moveNew.x, moveNew.y,moveNew.z-1);
        //    playerRB.AddForce(0, jumpSpeed, 0 * Time.deltaTime, ForceMode.Impulse);
        //    //playerRB.AddForce(transform.forward * Time.deltaTime * 3000, ForceMode.Acceleration);
        //}


        // This condition controls the movement of the player while moving.
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                // This condition is to slowdown the speed of the palyer when he is in the air
                if (playerRB.velocity.magnitude < velocityLimit && !isPlayerOnGround)
                {
                    playerRB.AddForce(transform.forward * Time.deltaTime * movingSpeed, ForceMode.Force);
                    playerRB.AddForce(Vector3.down * Time.deltaTime * movingSpeed, ForceMode.Force);
                }
                else
                    playerRB.AddForce(transform.forward * Time.deltaTime * movingSpeed, ForceMode.Force);

        }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                playerRB.AddForce(-transform.forward * Time.deltaTime * movingSpeed, ForceMode.Force);
            }

            // This condition controls the Jump of the player while moving.
            if (Input.GetKeyUp(KeyCode.Space) && isPlayerOnGround)
            {
                isPlayerOnGround = false;
                playerRB.AddForce(transform.up * Time.deltaTime * jumpSpeed, ForceMode.Impulse);
            }

            // This condition controls the animation and also DirtSPlatter of the player while moving.
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                playerAnimator.SetBool("Static_b", false);
                playerAnimator.SetFloat("Speed_f", movingSpeed);

                playerDirtSplatterParticlFX.Play(); // Dirst Splatter Play

            }
            else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
            {
                playerAnimator.SetBool("Static_b", true);
                playerAnimator.SetFloat("Speed_f", 0);

                playerDirtSplatterParticlFX.Stop(); // Dirst Splatter Stop
            }
        

      

    }

    // CO-Routine to handle health Counter
    IEnumerator PlayerHealthCounter()
    {
        while (playerHealth.BarValue > 0)
        {

            yield return new WaitForSeconds(playerHealthCountdownTimeLimit);
            playerHealth.BarValue -= 1;
            healthColurIndicator();

            //If Player's health is Zero then he dies 
            if (playerHealth.BarValue == 0)
            {
                PlayerDeath();
            }

        }

    }

    // Player's death and other rituals are handled.
    private void PlayerDeath()
    {
        // GameOver - Set to true
        isGameOver = true;
        // Play death Aimation
        playerAnimator.SetBool("Death_b", true);
        playerAnimator.SetInteger("DeathType_int", 1);
        // Turning off the low-health indicating sound.
        playerHealth.repeat = false;
        playerHealth.sound = null;
        // Stop BGM
        gameBGM.Stop();
        // Playing Player Death Explosion FX - Visual
        playerExplosionParticleFX.Play(); 
        // Playing Player Death Sound FX - Audio
        playerAudioSource.PlayOneShot(playerDeathSound,1.0f);
        

        //Destroy Player
        //Destroy(this.gameObject, 2.20f);
    }

    // Health Bar color codes and conditions
    private void healthColurIndicator()
    {
        
        if (playerHealth.BarValue > 75 && playerHealth.BarValue < 91)
        {
            playerHealth.BarBackGroundColor = Color.green; // Olive Green

        }
        else if (playerHealth.BarValue > 50 && playerHealth.BarValue < 76)
        {
            playerHealth.BarBackGroundColor = new Color(0, 174, 215, 255); // Sky Blue
        }
        else if (playerHealth.BarValue > 35 && playerHealth.BarValue < 51)
        {
            playerHealth.BarBackGroundColor = new Color(244, 255, 0, 255); // Yellow
        }
        else if (playerHealth.BarValue > 20 && playerHealth.BarValue < 36)
        {
            playerHealth.BarBackGroundColor = Color.magenta; // magenta/Dark Pink
        }
        else if (playerHealth.BarValue < 21)
        {
            playerHealth.BarBackGroundColor = Color.red; // Red
        }
        else
        {
            playerHealth.BarBackGroundColor = Color.blue;
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("Game Object Name is - " + other.gameObject.name);
        if (other.tag.StartsWith("Powerup"))
        {
            //Enabling the powerup ParticleFX
            playerPowerupFX.transform.parent.gameObject.SetActive(true);
            
            spawnManager.hasOnePowerupInScene = false; //Letting know SpawnManager that there is no powerup in the scene.
            // Playing Player Powerup Collection Visual FX - Visual
            playerPowerupFX.Play();
            // Playing Player Powerup Collection Sound FX - Audio
            playerAudioSource.PlayOneShot(playerPowerupCollectionSound, 1.0f);

            if (other.gameObject.name.Contains("Apple"))
            {
                playerHealth.BarValue += appleHealth;
                Destroy(other.gameObject);
            }
            else if (other.gameObject.name.Contains("Milk"))
            {
                playerHealth.BarValue += milkHealth;
                Destroy(other.gameObject);
            }
            else if (other.gameObject.name.Contains("Cookie"))
            {
                playerHealth.BarValue += cookieHealth;
                Destroy(other.gameObject);
            }
            else if (other.gameObject.name.Contains("BigBottle"))
            {
                playerHealth.BarValue += bigbottleHealth;
                Destroy(other.gameObject);
            }
            else if (other.gameObject.name.Contains("10000dol"))
            {
                playerHealth.BarValue += moneyHealth;
                Destroy(other.gameObject);
            }

        }

        if (other.gameObject.name.Contains("Snow"))
        {
            Debug.Log("Is On Ground");
            isPlayerOnGround = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Poison") && playerHealth.BarValue > 20)
        {
            playerHealth.BarValue -= 1;
        }
    }

}

//particle fx when taking powerups



// How to make the player stand on the ground/Snow and make physics work
// Player Movement with Physics is not working
// transform.Rotate(Vector3.up,horizontalMovement * turningSpeed * Time.deltaTime); -- Not working
// Why do we need Input.GetAxis when we can use Input.getKeyDown(up arrow)
// Why/when do we use Public variable and drag + drop ; why do we use GetComponent<Type>();

//Change BGM if needed and increase the volume back
