using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Actions of the NavMesh Agent
public abstract class GOAP_Action : MonoBehaviour
{
    // Atributes of each action
    public string actionName = "Action Name"; // name of the Action
    public GameObject target; // location of where that action is going to take place
    public GameObject endingTarget; // location of where the action is going to end
    public string targetTag; // the tag of the location of where the action is going to take place
    public float cost = 1.0f; // cost of the action
    public float duration = 0; // duration of the action 
    public WorldState[] preConditions; // pre condition for the action to start
    public WorldState[] afterEffects; // the after effects of the action
    public NavMeshAgent agent; // agent that is going to accomplish that action
    public GameObject weaponToGet; // weapon for agent to get from shelf
    public GameObject weaponToLeave; // weapon for agent to leave on the table
    public string textNo1;
    public string textNo2;

    public Dictionary<string, int> preconditions; // dictionary of preconditions
    public Dictionary<string, int> effects; // dictionary of after effects

    public WorldStates agentState; // state of the agent

    public bool running = false; // shows if action is running or not

    public GOAP_Action()
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    public void Awake()
    {
        // set agent as this agent
        agent = this.gameObject.GetComponent<NavMeshAgent>();

        // add preconditions
        if(preConditions != null)
        {
            foreach(WorldState state in preConditions)
            {
                preconditions.Add(state.key, state.value);
            }
        }

        // add after effects
        if (afterEffects != null)
        {
            foreach (WorldState state in afterEffects)
            {
                effects.Add(state.key, state.value);
            }
        }
    }

    // see if goal is achievable
    public bool IsAchievable()
    {
        return true;
    }

    // see if goal meets all preconditions
    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach(KeyValuePair<string, int> precondition in preconditions)
        {
            if (!conditions.ContainsKey(precondition.Key))
            {
                return false;
            }
        }
        return true;
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();

}
