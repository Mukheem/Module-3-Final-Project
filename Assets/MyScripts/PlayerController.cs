using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
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
    public ParticleSystem playerExplosionParticleFX;
    public ParticleSystem playerDirtSplatterParticlFX;
    private AudioSource gameBGM;
    public AudioClip playerDeathSound;
    private AudioSource playerAudioSource;
    private SpawnManager spawnManager;
    


    // Misellaneous
    public float gravityModifier;
    public ProgressBarCircle playerHealth;
    private int playerHealthCountdownTimeLimit = 1; //Every 8 seconds, Player looses his/her health.
    public bool isGameOver;


    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        Physics.gravity *= gravityModifier;

        playerHealth.BarValue = 99;//Health value Initialisation
        StartCoroutine("PlayerHealthCounter");

        isGameOver = false;

        gameBGM = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        playerAudioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        // Movement of the player
        horizontalMovement = Input.GetAxis("Horizontal");
        forwardMovement = Input.GetAxis("Vertical");

        transform.Translate(Vector3.forward * Time.deltaTime * movingSpeed * forwardMovement);
        transform.Rotate(Vector3.up, horizontalMovement * turningSpeed);




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
            playerHealth.BarBackGroundColor = Color.grey;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.StartsWith("Powerup"))
        {
            Destroy(other.gameObject);
            spawnManager.hasOnePowerupInScene = false; //Letting know SpawnManager that there is no powerup in the scene.
        }
    }
}

// How to make the player stand on the ground/Snow and make physics work
// Player Movement with Physics is not working
// transform.Rotate(Vector3.up,horizontalMovement * turningSpeed * Time.deltaTime); -- Not working
// Why do we need Input.GetAxis when we can use Input.getKeyDown(up arrow)
// Why/when do we use Public variable and drag + drop ; why do we use GetComponent<Type>();
