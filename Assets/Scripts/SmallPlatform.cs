using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using Unity.AI.Navigation;

public class SmallPlatform : MonoBehaviour
{
    public NavMeshSurface surface;
    GameObject[] platforms;
    bool nearPlatform = false;

    void Start()
    {
        platforms = GameObject.FindGameObjectsWithTag("SmallPlatform");
    }

    void Update()
    {
        surface.BuildNavMesh(); // update navmesh each frame so it can build along with platform movement

        // for each moving platform except the current one if its distance from another platfom is less than 6 then switch nearPlatform to true
        foreach( GameObject platform in platforms)
        {
            // if there is a platform near change nearPlatform to true and break from loop, so it doesnt change back to false
            if((transform.position - platform.transform.position).magnitude < 6 && !platform.Equals(this.gameObject))
            {
                nearPlatform = true;
                break;
            }
            else if((transform.position - platform.transform.position).magnitude > 6 && !platform.Equals(this.gameObject))
            {
                nearPlatform = false;
            }
        }

        Debug.Log("Near: "+ nearPlatform);
    }

    // when enemy or player enter platform, make them platoform's children
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.transform.parent = null;
            other.gameObject.transform.parent = transform;
        }
    }

    // when enemy or player exit platform, remove them from platform's children, except if the current platform is near to other (this way we avoid of player or enemy from exiting the platforms when switching from one to another)
    void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy")) && !nearPlatform)
        {
            other.gameObject.transform.parent = null;
        }
    }
}
