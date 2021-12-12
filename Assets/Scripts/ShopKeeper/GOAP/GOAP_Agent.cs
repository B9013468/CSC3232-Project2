using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

// create sub-goals of goals, for more complex goals
public class SubGoal
{
    public Dictionary<string, int> subgoals;
    public bool remove;
    
    public SubGoal(string s, int i, bool r)
    {
        subgoals = new Dictionary<string, int>();
        subgoals.Add(s, i);
        remove = r; // set to true if we want to remove that sub-goal after its completion, or false if thi sub-goal is going to reoccur many times
    }
}

// set Agent's behaviour
public class GOAP_Agent : MonoBehaviour
{
    public List<GOAP_Action> actions = new List<GOAP_Action>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    GOAP_Planner planner;
    Queue<GOAP_Action> actionQueue;
    public GOAP_Action currentAction;
    SubGoal currentGoal;

    GameObject[] weaponsToGet;
    GameObject[] weaponsToLeave;

    public Text speechText;

    void Awake()
    {
        // gets arrays of all weapons on the shelf and on the table
        weaponsToGet = GameObject.FindGameObjectsWithTag("WeaponToGet");
        weaponsToLeave = GameObject.FindGameObjectsWithTag("WeaponToLeave");
        RestartWeaponsToLeave();
    }

    public void Start()
    {
        GOAP_Action[] actionArray = this.GetComponents<GOAP_Action>(); // create an array of all actions on the current agent

        // add all actions in the array in the actions list
        foreach(GOAP_Action action in actionArray)
        {
            actions.Add(action);
        }
    }

    bool invoked = false;

    // restart planning system for current action
    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }
    
    void LateUpdate()
    {

        // if there is already an action which is running, move agent torwards the goal
        if (currentAction != null && currentAction.running)
        {
            if(currentAction.agent.hasPath && currentAction.agent.remainingDistance < 1f)
            {
                // if action is not yet invoked, invoke it for a duration equal to current action's duration by after calling CompleteAction method
                if (!invoked)
                {
                    Invoke("PickWeapon", currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }

        // if agent has currently no plans to work on, create new planner
        if(planner == null || actionQueue == null)
        {
            planner = new GOAP_Planner();

            // sort through goals, putting them from most important to least important
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach(KeyValuePair<SubGoal, int> goal in sortedGoals)
            {
                actionQueue = planner.plan(actions, goal.Key.subgoals, null);
                if(actionQueue != null)
                {
                    currentGoal = goal.Key;
                    break;
                }
            }
        }

        // if there is already an action queue but it is empty, meaning that it has ran out of things to do, remove current goal and set planner to null
        if(actionQueue != null && actionQueue.Count == 0)
        {
            if (currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }
            planner = null;
        }

        // if there is an action queue and there are still actions left in the queue, then take off the top action of the queue and continue from the next action
        if(actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform())
            {
                // if current target or target's tag is null, find next target by using their tag
                if(currentAction.target == null && currentAction.targetTag != "")
                {
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                }

                // if there is a current target set running to true and set agent to current destination and restart weapon from table
                if(currentAction.target!= null)
                {
                    currentAction.running = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                    speechText.text = currentAction.textNo1;
                    RestartWeaponsToLeave();
                }
            }
            else
            {
                actionQueue = null;
            }
        }
    }

    // picks weapon from the shelf
    void PickWeapon()
    {
        RestartWeaponsToGet();
        currentAction.weaponToGet.SetActive(false);
        currentAction.agent.SetDestination(currentAction.endingTarget.transform.position);
        Invoke("LeaveWeapon", currentAction.duration);
    }

    // restarts weapons on the shelf
    void RestartWeaponsToGet()
    {
        foreach (GameObject weapon in weaponsToGet)
        {
            weapon.SetActive(true);
        }
    }
    // restarts weapons on the table
    void RestartWeaponsToLeave()
    {
       foreach (GameObject weapon in weaponsToLeave)
        {
            weapon.SetActive(false);
        }
    }

    // leaves weapon on the table
    void LeaveWeapon()
    {
        speechText.text = currentAction.textNo2;
        currentAction.weaponToLeave.SetActive(true);
        Invoke("CompleteAction", currentAction.duration);
    }
}
