using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] powerups;
    public bool hasOnePowerupInScene = false;
    private List<int> powerupIndices = new List<int>();
    public TextMeshProUGUI clueText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PowerupInstantiator());

    }

    // CoRoutine which instatntiates powerups one after the other.
    private IEnumerator PowerupInstantiator()
    {
        int powerupLoopingVariable = 0; // variable to ensure that the coroutine is iterated powerups.length number of times.
        while(powerupLoopingVariable < powerups.Length)
        {
            // Condition to wait until is that the existing powerup is destroyed.
            yield return new WaitUntil(() => hasOnePowerupInScene == false);
            //Method which selects powerups randomly.
            selectPowerupRandomly();

            powerupLoopingVariable++; // Incrementing the loop.
        }
        
    }

    /*
     * selectPowerupRandomly -> This method takes the length of powerup gameobjects added in the inspector menu and randomly picks one integer.
     * It checks if the same integer is not picked before.
     *   
     * Switch Case is used to spawn the chosen powerup
     */
    private void selectPowerupRandomly()
    {
        int powerupIndex;
        
        do
        {
            powerupIndex = Random.Range(0, powerups.Length);

        } while (powerupIndices.Contains(powerupIndex) && (powerupIndices.Count < powerups.Length));
        powerupIndices.Add(powerupIndex);
        Debug.Log("PowerupIndex chosen is: "+powerupIndex);

        switch (powerupIndex)
        {
            case 0:
                instantiatePowerupOne();
                break;
            case 1:
                instantiatePowerupTwo();
                break;

            case 2:
                instantiatePowerupThree();
                break;
            case 3:
                instantiatePowerupFour();
                break;
            case 4:
                instantiatePowerupFive();
                break;
            
            default:
                Debug.Log("Default Powerup - Not needed as powerups are predefined.");
                break;
        }



    }

    private void instantiatePowerupOne()
    {
        float xAxisRangeStart = 105.0f;
        float xAxisRangeEnd = 121.0f;
        float zAxisRangeStart = -141.0f;
        float zAxisRangeEnd = -88.0f;
        float yAxisRangeStart = 34;
        float yAxisRangeEnd = 35;

        bool aptPositionFound = false;


        while (aptPositionFound == false)
        {
            Vector3 powerupOnePos = new Vector3(Random.Range(xAxisRangeStart, xAxisRangeEnd), Random.Range(yAxisRangeStart, yAxisRangeEnd), Random.Range(zAxisRangeStart, zAxisRangeEnd));
            if (Physics.CheckSphere(powerupOnePos, 0.2f))
            {
                Debug.Log("Oops...Some Objection noticed at - " + powerupOnePos);
            }
            else
            {
                GameObject spawnnedPowerup = Instantiate(powerups[0], powerupOnePos, transform.rotation);
                //Updating ClueText
                clueText.text = spawnnedPowerup.tag + " is out there for you to collect";
                aptPositionFound = true; // Bool condition to break the loop
                hasOnePowerupInScene = true; // Bool condition to stop and start spawnning powerups
            }
        }

    }

    private void instantiatePowerupTwo()
    {
        Vector3[] powerupTwoLocations = { new Vector3(98.43f,25.524f,-149.826f), new Vector3(83.15f, 17.08f, -137.57f),new Vector3(57.65f, 13.98f, -133.24f) };

        Vector3 powerupTwoPos = powerupTwoLocations[Random.Range(0, powerupTwoLocations.Length)];

        GameObject spawnnedPowerup = Instantiate(powerups[1], powerupTwoPos, transform.rotation);
        //Updating ClueText
        clueText.text = spawnnedPowerup.tag + " is out there for you to collect";

        hasOnePowerupInScene = true; // Bool condition to stop and start spawnning powerups
    }
    private void instantiatePowerupThree()
    {
        Vector3[] powerupThreeLocations = { new Vector3(30.8f,15.86f-131.63f), new Vector3(8.01f,11.37f,-109.38f), new Vector3(68.78f,13.15f,-83.79f) };

        Vector3 powerupThreePos = powerupThreeLocations[Random.Range(0, powerupThreeLocations.Length)];

        GameObject spawnnedPowerup = Instantiate(powerups[2], powerupThreePos, transform.rotation);
        //Updating ClueText
        clueText.text = spawnnedPowerup.tag + " is out there for you to collect";

        hasOnePowerupInScene = true; // Bool condition to stop and start spawnning powerups
    }
    private void instantiatePowerupFour()
    {
        Vector3[] powerupFourLocations = { new Vector3(23.564f,10.91f,-99.64f), new Vector3(41.86f,10.1f,-86.77f), new Vector3(28.07f,12.48f,-115.37f) };

        Vector3 powerupFourPos = powerupFourLocations[Random.Range(0, powerupFourLocations.Length)];

        GameObject spawnnedPowerup = Instantiate(powerups[3], powerupFourPos, transform.rotation);
        //Updating ClueText
        clueText.text = spawnnedPowerup.tag + " is out there for you to collect";

        hasOnePowerupInScene = true; // Bool condition to stop and start spawnning powerups
    }
    private void instantiatePowerupFive()
    {
        Vector3[] powerupFiveLocations = { new Vector3(102f,25.41f,-75.73f), new Vector3(77.4f,10.312f,-69.698f), new Vector3(39.77f,18.52f,-122.39f) };

        Vector3 powerupFivePos = powerupFiveLocations[Random.Range(0, powerupFiveLocations.Length)];

        GameObject spawnnedPowerup = Instantiate(powerups[4], powerupFivePos, transform.rotation);
        //Updating ClueText
        clueText.text = spawnnedPowerup.tag + " is out there for you to collect";

        hasOnePowerupInScene = true; // Bool condition to stop and start spawnning powerups
    }
}
