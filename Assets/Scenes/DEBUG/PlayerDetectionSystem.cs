using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectionSystem : MonoBehaviour
{
    static public bool seePlayer;

    private void OnTriggerEnter(Collider collisionObject)
    {
        if(collisionObject.name == "Player")
        {
            print("Player Detected by Enemy Vision System");
            seePlayer = true;
        }
    }
    private void OnTriggerExit(Collider collisionObject)
    {
        // TODO: There could be something we check on the player when perhaps the enemy can follow even outside the visionsphere
        seePlayer = false;
        print("Player Out of Sight by Enemy Vision System");

    }
}
    
