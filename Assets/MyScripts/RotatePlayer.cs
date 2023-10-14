using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayer : MonoBehaviour
{
    

    /*
     * This method keeps the player rotating during intro scene.
     */
    void Update()
    {
        transform.Rotate(0, 60 * Time.deltaTime,0 );
    }
}
