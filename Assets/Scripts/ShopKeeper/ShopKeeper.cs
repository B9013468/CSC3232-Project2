using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : GOAP_Agent
{
    public int stars = 0;
    public int diamonds = 0;

    string weaponPicked;

    // Start is called before the first frame update
    void Start()
    {
        // add greet player action in the subgoal list
        base.Start();
        SubGoal subGoal1 = new SubGoal("greetPlayer", 1, true);
        goals.Add(subGoal1, 3);
    }

    //find which weapon can the player buy, based on the total diamonds and stars collected so far
    public void FindWeapon()
    {
        diamonds = ShopScript.totalDiamonds;
        stars = ShopScript.totalStars;
        if (stars >= 6 && diamonds >= 600)
        {
            weaponPicked = "pickWeapon6";
        }
        else if (stars >= 5 && diamonds >= 500)
        {
            weaponPicked = "pickWeapon5";
        }
        else if (stars >= 4 && diamonds >= 400)
        {
            weaponPicked = "pickWeapon4";
        }
        else if (stars >= 3 && diamonds >= 300)
        {
            weaponPicked = "pickWeapon3";
        }
        else if (stars >= 2 && diamonds >= 200)
        {
            weaponPicked = "pickWeapon2";
        }
        else if (stars >= 1 && diamonds >= 100)
        {
            weaponPicked = "pickWeapon1";
        }
        else
        {
            weaponPicked = "noneWeapon";
        }

        // add pick weapon action in the subgoal list (each pick weapon action has as preconditions two actions, check diamonds and check stars, where the shop keeper checks if the diamonds and the stars are real and counts them)
        SubGoal subGoal2 = new SubGoal(weaponPicked, 1, true);
        goals.Add(subGoal2, 3);
    }
}
