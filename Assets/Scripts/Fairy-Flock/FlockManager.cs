using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject player;
    public GameObject fairy;
    public int numberOfFairies = 50;
    public GameObject[] fairyArray;
    public Vector3 flyDistance = new Vector3(1, 0.5f, 1);
    public Vector3 goalPosition;
    public float distanceToAvoid = 4;

    [Range(0.0f, 10.0f)]
    public float speedMin;
    [Range(0.0f, 10.0f)]
    public float speedMax;

    [Range(0f, 5f)]
    public float distanceBetween;
    [Range(0.0f, 10.0f)]
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        fairyArray = new GameObject[numberOfFairies];

        for(int i = 0; i < numberOfFairies; i++)
        {
            Vector3 position = this.transform.position + new Vector3(Random.Range(-flyDistance.x, flyDistance.x), Random.Range(-flyDistance.y, flyDistance.y), Random.Range(-flyDistance.z, flyDistance.z));

            fairyArray[i] = (GameObject)Instantiate(fairy, position, Quaternion.identity);

            fairyArray[i].GetComponent<Flocking>().flockManager = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        goalPosition = this.transform.position;
    }
}
