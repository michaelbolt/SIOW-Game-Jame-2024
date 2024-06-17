using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    public float speedGain = 0.5f;
    bool lookatPlayer = false;
    bool attackPlayer = false;
    public GameObject player;
    Vector3 enemyPosition;
    Quaternion enemyQuat;
    Vector3 playerPosition;
    Quaternion playerQuat;
    float timeSeen = 0;
    Rigidbody enemyRb;
    float enemyDistance;

    // Start is called once at beginning
    private void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        if (PlayerDetectionSystem.seePlayer)
        {
            lookatPlayer = true;

        } else
        {
            lookatPlayer = false;
            timeSeen = 0;
            attackPlayer = false;
            print("EnemySystem: Chillin' ");
        }
        transform.GetPositionAndRotation(out enemyPosition, out enemyQuat);
        player.transform.GetPositionAndRotation(out playerPosition, out playerQuat);
        Vector3 deltaPosition =  playerPosition - enemyPosition;
        enemyDistance = deltaPosition.magnitude;
        if (lookatPlayer)
        {
            transform.LookAt(player.transform);
            timeSeen = timeSeen + Time.deltaTime;
            print("EnemySystem: Transform Enemy to Look At Player! Time = " + timeSeen.ToString());

            if (timeSeen >= 3)
            {
                attackPlayer = true;
            }
        }
        if (attackPlayer)
        {
            float enemySpeedx;
            enemySpeedx = deltaPosition.x * speedGain;
            float enemySpeedz;
            enemySpeedz = deltaPosition.z * speedGain;
            print("EnemySystem: Attacking! Speed =  " + enemySpeedx.ToString());
            //enemyRb.velocity.Set(enemySpeed, 0, 0);
            transform.position = new Vector3(transform.position.x + (enemySpeedx * Time.deltaTime),
                transform.position.y, transform.position.z + (enemySpeedz * Time.deltaTime));
        }
    }
}
