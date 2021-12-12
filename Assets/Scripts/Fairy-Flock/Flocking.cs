using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : MonoBehaviour
{
    public FlockManager flockManager;
    float speed;
    bool turnBack;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(flockManager.speedMin, flockManager.speedMax);
    }

    // Update is called once per frame
    void Update()
    {
        // set bounds for where the fairies can fly
        Bounds bounds = new Bounds(flockManager.transform.position, flockManager.flyDistance);

        // if the position of the current fairy is not in the bounds set turnBack to true
        if (!bounds.Contains(transform.position))
        {
            turnBack = true;
        }
        else
        {
            turnBack = false;
        }

        // if turnBack is true, get fairy back to the center
        if (turnBack)
        {
            Vector3 currentDirection = flockManager.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentDirection), flockManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            FlockRules();
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void FlockRules()
    {
        GameObject[] fairyHolder;
        fairyHolder = flockManager.fairyArray;

        Vector3 averageCenterPoint = Vector3.zero;
        Vector3 averageAvoidancePoint = Vector3.zero;

        float averageSpeed = 0.01f;
        float neighborDistance;
        int fairyNumber = 0;

        foreach(GameObject fairy in fairyHolder)
        {
            // if current fairy is not this current fairy
            if(fairy != this.gameObject)
            {
                neighborDistance = Vector3.Distance(fairy.transform.position, this.transform.position); // get distance between the current loop fairy and this fairy

                // if this distance is less than the distance that fairies should have between them, add this distance to the average center point and increase the fairy number by 1 for calculating the average after
                if(neighborDistance <= flockManager.distanceBetween)
                {
                    averageCenterPoint += fairy.transform.position;
                    fairyNumber++;

                    // if distance is less than distanceToAvoid, then fairy should avoid its neighbor 
                    if (neighborDistance < flockManager.distanceToAvoid)
                    {
                        averageAvoidancePoint = averageAvoidancePoint + (this.transform.position - fairy.transform.position);
                    }

                    // get current flocking and add its speed to average speed
                    Flocking thisFlocking = fairy.GetComponent<Flocking>();
                    averageSpeed = averageSpeed + thisFlocking.speed;
                }
            }
        }

        // calculate the averages by deviding by the fairy number
        if(fairyNumber > 0)
        {
            averageCenterPoint = (averageCenterPoint / fairyNumber) + (flockManager.goalPosition - this.transform.position); // add goal position to the average center point
            averageSpeed = averageSpeed / fairyNumber;

            // calculate the direction that fairy should travel
            Vector3 currentDirection = (averageCenterPoint + averageAvoidancePoint) - transform.position;
            if(currentDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentDirection), flockManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
