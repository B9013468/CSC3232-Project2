using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// nodes for our graph of actions, helping to link different actions together
public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public GOAP_Action action;

    public Node(Node parent, float cost, Dictionary<string, int> allstates, GOAP_Action action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allstates);
        this.action = action;
    }
}

// Planner checks if all the actions that are given are achievable, depending on the preconditions that are given, and plans the order that are going to happen, creating branches and nodes
public class GOAP_Planner
{
    // return a queue of all the actions
    public Queue<GOAP_Action> plan(List<GOAP_Action> actions, Dictionary<string, int> goal, WorldStates states)
    {
        List<GOAP_Action> usableActions = new List<GOAP_Action>();

        // add action if it is achievable
        foreach (GOAP_Action action in actions)
        {
            if (action.IsAchievable())
            {
                usableActions.Add(action);
            }
        }

        List<Node> leaves = new List<Node>();

        // create starting node, with no parent, 0 cost, list of all the world states, and no action
        Node start = new Node(null, 0, GOAP_World.Instance.GetWorld().GetStates(), null);

        // return true or false based on if a path is found or not
        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            Debug.Log("No Path Found!!!");
            return null;
        }

        // find cheapest leaf, by using cost of each action
        Node cheapest = null;
        foreach(Node leaf in leaves)
        {
            if(cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if(leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }
        }

        // resulting plan
        List<GOAP_Action> result = new List<GOAP_Action>();
        Node node = cheapest;
        while (node != null)
        {
            if(node.action != null)
            {
                result.Insert(0, node.action);
            }
            node = node.parent;
        }

        // create and return new queue of actions in currrent plan
        Queue<GOAP_Action> queue = new Queue<GOAP_Action>();
        foreach(GOAP_Action action in result)
        {
            queue.Enqueue(action);
        }

        // print all actions in current plan
        Debug.Log("The Plan is: ");
        foreach(GOAP_Action action in queue)
        {
            Debug.Log("Q: " + action.actionName);
        }

        return queue;
    }

    // return true or false based of if a path is found or not, considering the preconditions and after-effects of each action 
    private bool BuildGraph(Node parent, List<Node> leaves, List<GOAP_Action> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false; // initialize foundPath to true
        foreach(GOAP_Action action in usableActions)
        {
            if (action.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach(KeyValuePair<string, int> effects in action.effects)
                {
                    if (!currentState.ContainsKey(effects.Key))
                    {
                        currentState.Add(effects.Key, effects.Value);
                    }
                }

                // create next node
                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                // if goal is achieved, then path is found
                if(GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else // else go to next node
                {
                    List<GOAP_Action> subset = ActionSubset(usableActions, action);  // creating a subset of the usable actions, so to take out the current action node (that makes sure that our recursion will have an end in some point)
                    bool found = BuildGraph(node, leaves, subset, goal); // using recursion, by caling again BuildGraph for the next node
                    if (found)
                    {
                        foundPath = true;
                    }
                }
            }
        }

        return foundPath;
    }

    //check if goal has been achieved
    private bool GoalAchieved(Dictionary<string, int> goals, Dictionary<string, int> state)
    {
        // if the goal does not exist in the after effets then return false else return true
        foreach(KeyValuePair<string, int> goal in goals)
        {
            if (!state.ContainsKey(goal.Key))
            {
                return false;
            }
        }
        return true;
    }

    // gets rid of the current action node, by returning a new subset without the current one in it
    private List<GOAP_Action> ActionSubset(List<GOAP_Action> actions, GOAP_Action currentAction)
    {
        List<GOAP_Action> subset = new List<GOAP_Action>();
        foreach(GOAP_Action action in actions)
        {
            // if action is not equal to current action we want to remove, add it to new subset (that way the new subset will contain all actions, except the current one, which will be ignored)
            if (!action.Equals(currentAction))
            {
                subset.Add(action);
            }
        }
        return subset;
    }
}
