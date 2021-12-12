using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldState
{
    public string key; // key indicates to world's state
    public int value; // associate key to a value if wanted to have multiple of the same actions (if value is below 0 it gets discarded from dictionary)
}

// using dictionary to handle the states, because dictionaries are easy to manipulate
public class WorldStates
{
    public Dictionary<string, int> states;

    public WorldStates()
    {
        states = new Dictionary<string, int>();
    }

    // asks if dictionary has a particular state in it
    public bool HasState(string key)
    {
        return states.ContainsKey(key);
    }

    //add state in the dictionary
    void AddState(string key, int value)
    {
        states.Add(key, value);
    }

    //remove state in the dictionary if it is in Dictionary
    public void RemoveState(string key)
    {
        if (states.ContainsKey(key))
        {
            states.Remove(key);
        }
    }

    // modify state, by adding to its value if it is already in Dictionary, discarding it if its value is below 0, or adding it in the Dictionary if it is not already in there 
    public void ModifyState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] += value;
            if(states[key] <= 0)
            {
                RemoveState(key);
            }
        }
        else
        {
            AddState(key, value);
        }
    }

    // set current's key value if it is already in Dictionary, else add key in Dictionary
    public void SetState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] = value;
        }
        else
        {
            states.Add(key, value);
        }
    }

    // return final Dictionary
    public Dictionary<string, int> GetStates()
    {
        return states;
    }
}
