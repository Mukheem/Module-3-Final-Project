using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] powerups;
    public bool hasOnePowerupInScene = false;
    private List<int> powerupIndices = new List<int>();

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
                break;

            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            default:
                print("Incorrect intelligence level.");
                break;
        }



    }

    private void instantiatePowerupOne()
    {
        float xAxisRangeStart = 87.0f;
        float xAxisRangeEnd = 103.0f;
        float zAxisRangeStart = -116.0f;
        float zAxisRangeEnd = -92.0f;
        float yAxisRangeStart = 24;
        float yAxisRangeEnd = 25;

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
                Instantiate(powerups[0], powerupOnePos, transform.rotation);
                aptPositionFound = true; // Bool condition to break the loop
                hasOnePowerupInScene = true; // Bool condition to stop and start spawnning powerups
            }
        }

    }
}
