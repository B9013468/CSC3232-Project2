using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GOAP_World
{

    private static readonly GOAP_World instance = new GOAP_World();

    private static WorldStates world;

    // Dictionary with all the states
    static GOAP_World()
    {
        world = new WorldStates();
    }


    public static GOAP_World Instance
    {
        get { return instance; }
    }

    // return status of the world
    public WorldStates GetWorld()
    {
        return world;
    }
}
