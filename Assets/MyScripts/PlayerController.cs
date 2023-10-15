using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    /*
    * Global Variable declarations
    */
    //Player Movement Variables
    private float horizontalMovement;
    private float forwardMovement;
    public float movingSpeed = 40000;
    public float turningSpeed = 0.27f;
    public float jumpSpeed = 80000;
    private float jumpRestTimer = 3f;


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




    // Miscellaneous
    public float gravityModifier;
    public ProgressBarCircle playerHealth;
    private int playerHealthCountdownTimeLimit = 8; //Every 8 seconds, Player looses his/her health.
    public bool isGameOver;
    public bool canPlayerJump;
    public float velocityLimit = 2.0f; // TO control player speed when he is in the air.
    private int appleHealth = 15;
    private int milkHealth = 25;
    private int cookieHealth = 10;
    private int bigbottleHealth = 30;
    private int moneyHealth = 25;
    public int objectsCollected;
    public TextMeshProUGUI clueText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI jumpIndicator;
    public Button restartGameButton;
    [SerializeField]
    private TextMeshProUGUI floatingText;



    // Start is called before the first frame update
    void Start()
    {

        //Player Start Position and rotation
        transform.position = new Vector3(98.0f, 24f, -65f);
        this.transform.Rotate(0, 130, 0);

        //Component initializations
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        gameBGM = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        playerAudioSource = GetComponent<AudioSource>();

        // Player variables initializations
        playerAnimator = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerHealth.BarValue = 99;//Health value Initialisation


        // Starting the health Counter
        StartCoroutine("PlayerHealthCounter");

        //Miscellaneous
        isGameOver = false;
        canPlayerJump = true;
        jumpIndicator.gameObject.SetActive(true);

        //GUI Text related initializations
        objectsCollected = 0;
        clueText.text = "Go, get all the objects before health runs out.";

       

        //Disabling the powerup ParticleFX so that it does not keep on playing.
        playerPowerupFX.transform.parent.gameObject.SetActive(false);

    }

    private void FixedUpdate()
    {
        //Making sure that Indicator is always showing right color
        HealthColurIndicator();
    }

    // Update is called once per frame
    void Update()
    {
        // Controls all the player movements and also player movement particle FX
        PlayerMovementControls();

        
        // Keeps checking if health is <50. If yes, then Speed is reduced also the Jump wait time increases
        ReduceSpeed();

        //Restart Game
        if (Input.GetKey(KeyCode.R))
        {
            RestartGame();
        }



    }

    /*
     * This method controls all the player movements and related particle FX
     */
    private void PlayerMovementControls()
    {
        // Movement of the player
        horizontalMovement = Input.GetAxis("Horizontal");
        forwardMovement = Input.GetAxis("Vertical");

        // This condition controls the rotation of the player while moving.
        transform.Rotate(Vector3.up, horizontalMovement * turningSpeed);

        // This condition controls the movement of the player while moving.
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            // This condition is to slowdown the speed of the palyer when he is in the air
            if (playerRB.velocity.magnitude < velocityLimit && !canPlayerJump)
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
        if (Input.GetKey(KeyCode.Space) && canPlayerJump)
        {
            canPlayerJump = false;
            jumpIndicator.gameObject.SetActive(false);
            StartCoroutine(JumpCounter());
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

            playerDirtSplatterParticlFX.Stop(); // Dirt Splatter Stop
        }


    }

    // CO-Routine to handle health Counter
    IEnumerator PlayerHealthCounter()
    {
        while (playerHealth.BarValue > 0)
        {

            yield return new WaitForSeconds(playerHealthCountdownTimeLimit);
            playerHealth.BarValue -= 1;
            HealthColurIndicator();

            //If Player's health is Zero then he dies 
            if (playerHealth.BarValue == 0)
            {
                PlayerDeath(false);
            }

        }

    }


    /* Player's death and other rituals are handled.
     * This method takes a parameter which decides if player has reached goal or died abruptly.
     * 
     * IsSuccessfulDeath - True when palyer reaches goal; false if he dies abrubptly.
     */
    private void PlayerDeath(bool isSuccessfulDeath)
    {
        // GameOver - Set to true irrespective of isSuccessfulDeath
        isGameOver = true;

        if (isSuccessfulDeath)
        {
            Destroy(this.gameObject, 0.25f);
        }
        else
        {

            // Play death Aimation
            playerAnimator.SetBool("Death_b", true);
            playerAnimator.SetInteger("DeathType_int", 2);
            // Turning off the low-health indicating sound.
            playerHealth.repeat = false;
            playerHealth.sound = null;
            // Playing Player Death Explosion FX - Visual
            playerExplosionParticleFX.Play();
            // Playing Player Death Sound FX - Audio
            playerAudioSource.PlayOneShot(playerDeathSound, 1.0f);
            //Make player's health to Zero as soon as he dies
            playerHealth.BarValue = 0;

        }
        // Stop BGM
        gameBGM.Stop();
        //Deactivate Floating Text on Player's head upon palyer's death
        floatingText.gameObject.SetActive(false);
        // GameOver - text displayed irrespective of isSuccessfulDeath and after all the animations/sounds are played.
        gameOverText.gameObject.SetActive(true);
        // Restart Game - button displayed irrespective of isSuccessfulDeath and after all the animations/sounds are played.
        restartGameButton.gameObject.SetActive(true);


    }

    // Health Bar color codes and conditions
    private void HealthColurIndicator()
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


    /*
     * This method handles all the operations when player colldes with different objects like powerups,DyingZone and safehouse in the scene
     */
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("Game Object Name is - " + other.gameObject.name);
        if (other.tag.StartsWith("Powerup"))
        {
            //Letting know SpawnManager that there is no powerup in the scene.
            spawnManager.hasOnePowerupInScene = false; 
            //Enabling the powerup ParticleFX
            playerPowerupFX.transform.parent.gameObject.SetActive(true);
            playerPowerupFX.gameObject.SetActive(true);
             // Playing Player Powerup Collection Visual FX - Visual
            playerPowerupFX.Play();
            // Playing Player Powerup Collection Sound FX - Audio
            playerAudioSource.PlayOneShot(playerPowerupCollectionSound, 1.0f);
            //Add powerup to Object collection and show score
            IncrementObjectCollectionScoreAndShow();

            
            Debug.Log(other.tag + " collected...");
            

            if (other.tag.Contains("Apple"))
            {
                playerHealth.BarValue += appleHealth;
                Destroy(other.gameObject);
            }
            else if (other.tag.Contains("Milk"))
            {
                playerHealth.BarValue += milkHealth;
                Destroy(other.gameObject);
            }
            else if (other.tag.Contains("Cookie"))
            {
                playerHealth.BarValue += cookieHealth;
                Destroy(other.gameObject);
            }
            else if (other.tag.Contains("Bottle"))
            {
                playerHealth.BarValue += bigbottleHealth;
                Destroy(other.gameObject);
            }
            else if (other.tag.Contains("Money"))
            {
                playerHealth.BarValue += moneyHealth;
                Destroy(other.gameObject);
            }

        }

        if (other.CompareTag("DyingZone"))
        {
            //Updating ClueText
            clueText.text = "You died falling off island";
            Debug.Log("Player died falling off island");
            PlayerDeath(false);
        }
        if (other.CompareTag("SafeHouse") && objectsCollected == 5)
        {
            //Updating ClueText
            clueText.text = "Goal achieved";
            PlayerDeath(true);
        }
    }

    /*
     * This methods drains down the health of the player to as minimum as 20% as long as the player is in contact with poisonous gas
     */
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Poison") && playerHealth.BarValue > 20)
        {
            playerHealth.BarValue -= 1;
        }
    }

    /*
     * This co-routine handles the jumping mechanism. Player can't jump until the said time is met
     */
    private IEnumerator JumpCounter()
    {

        yield return new WaitForSeconds(jumpRestTimer);
        canPlayerJump = true;
        jumpIndicator.gameObject.SetActive(true);
    }


    /*
     * This methods increments the floating text which shows the remaining number of objects to be collected
     */
    private void IncrementObjectCollectionScoreAndShow()
    {
        Debug.Log("Objects collected before -" + objectsCollected);
        if(objectsCollected != 5)
            objectsCollected = objectsCollected + 1;
        Debug.Log("Objects collected after -" + objectsCollected);

        floatingText.text = (objectsCollected).ToString() + "/5";

        // When all the objects are collected hint box is updated with the message so that player moves towards safehouse.
        if (objectsCollected == 5)
        {
            //Updating ClueText to - reach Safe house
            clueText.text = "Bravo...Now,Reach SafeHouse before health runs out.";
        }
    }

    // Restarts game at any point while player is playing the game.
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /*
     * This method decreases the speed of the player also the jump wait time increases when health runs to < 50%
     */
    private void ReduceSpeed()
    {
        if (playerHealth.BarValue < 50)
        {
            movingSpeed = 20000;
            jumpRestTimer = 4.5f;
        }
        else {

            movingSpeed = 40000;
        jumpRestTimer = 3.0f;
        }
    }
}
