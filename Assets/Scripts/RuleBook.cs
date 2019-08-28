﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RuleBook : MonoBehaviour
{
    // The grand variable, that in the end, deterines if the sign is lying
    private bool signIsLying, leftSafe, middleSafe, rightSafe;

    // If the value of the door is altered directly by a condition. If these are false, they will become opposite of the door with the sign
    private bool leftTouched, middleTouched, rightTouched;

    private MasterLogic ML;

    // Delegate so we can store methods
    public delegate bool RuleMethod();

    // Which rules are in this game
    public List<int> rulesInThisGame;

    // Rulebook containing our rules
    public List<RuleMethod> rules = new List<RuleMethod>() { };

    // Add each rule that is defined at the bottom of this script. Use this as an example
    public void AddRules()
    {
        rules.Clear();
        rules.Add(LyingIfRat);
        rules.Add(DripLeftDeath);
        rules.Add(EnterMidRightDeath);
        rules.Add(EvenRocksLying);
        rules.Add(MultipleThreeRocksTruthful);
        rules.Add(MoreShroomsRatsMiddleDeath);
    }

    // Randomly generates 2-4 rules to start
    public void StartRules()
    {
        //int howMany = Mathf.RoundToInt(UnityEngine.Random.Range(2, 4));
        int howMany = UnityEngine.Random.Range(2, 5);
        for (int i = 0; i < howMany; i++)
        {
            rulesInThisGame.Add(Mathf.RoundToInt(UnityEngine.Random.Range(0, rules.Count)));
        }

    }

    /// <summary>
    /// Calls each rule, which will modify signIsLying and also return whether or not the rule was true
    /// (currently I can't think of a use for this, but futureproofing never hurts)
    /// </summary>
    public void RunThroughRules()
    {
        ML = GetComponent<MasterLogic>();
        signIsLying = false;
        leftSafe = false; middleSafe = false; rightSafe = false;
        for (int i = 0; i < rules.Count; i++)
        {
            rules[i]();
        }

        FinalizeRoomStats();
    }

    /// <summary>
    /// Taking all variables, sends back the room info
    /// </summary>
    public void FinalizeRoomStats()
    {
        ML.currentRoom.signLying = signIsLying;
        DetermineSafeDoors();
    }

    // Sets the safe rooms in the current room
    private void DetermineSafeDoors()
    {
        int doorWithSign = ML.currentRoom.signLocation;

        ML.currentRoom.safeDoors = new bool[3] { false, false, false };

        // If a door isn't affected directly, it is set to the opposite of what the sign says
        bool untouchedDoor;
        if (signIsLying)
        {
            untouchedDoor = ML.currentRoom.signSaysSafe;
        }
        else
        {
            untouchedDoor = !ML.currentRoom.signSaysSafe;
        }

        print("untouched doors should be: " + untouchedDoor);

        // Hard code the doors that weren't affected by logic to be the opposite of what the sign said
        if (!leftTouched && doorWithSign != 0) { leftSafe = untouchedDoor; print("left door not touched"); }
        if (!middleTouched && doorWithSign != 1) { middleSafe = untouchedDoor; print("middle door not touched"); }
        if (!rightTouched && doorWithSign != 2) { rightSafe = untouchedDoor; print("right door not touched"); }

        bool[] touchedStates = new bool[3] { leftTouched, middleTouched, rightTouched };
        // If the sign is lying and the door with the sign isn't hard coded
        if (ML.currentRoom.signLying && touchedStates[doorWithSign] == false)
        {
            // Set the marked door to be the opposite of what the sign says
            SetDoor(doorWithSign, !ML.currentRoom.signSaysSafe);
            print("sign is lying, door " + doorWithSign + " is set to " + !ML.currentRoom.signSaysSafe);
        }
        // If the sign is not lying and the door with the sign isn't hard coded
        else if (!ML.currentRoom.signLying && touchedStates[doorWithSign] == false)
        {
            // Set the marked door to be what the sign says
            SetDoor(doorWithSign, ML.currentRoom.signSaysSafe);
            print("sign is truthful, door " + doorWithSign + " is set to " + ML.currentRoom.signSaysSafe);
        }
        // The marked door was changed by a hard coded rule (example, sign is above left door saying safe but there's dripping meaning left door is deadly)
        else if (touchedStates[doorWithSign] == true && CheckDoorOpposite(doorWithSign) == true)
        {
            // Note: this does not apply if the hard code is the same as the sign, for example, if a truthful sign says "death" above a tunnel that is hard coded to be death, nothign happens
            print("The marked door has been hard forced to opposite, flip everything");
            // Flip other two doors and the sign state
            signIsLying = !signIsLying;
            FlipOtherDoors(doorWithSign);
        }

        print("setting the room to these: " + leftSafe + ", " + middleSafe + ", " + rightSafe);
        ML.currentRoom.safeDoors[0] = leftSafe;
        ML.currentRoom.safeDoors[1] = middleSafe;
        ML.currentRoom.safeDoors[2] = rightSafe;
    }

    /// <summary>
    /// Shows if the marked door's state (safe/death) is opposite of what the sign says it is
    /// </summary>
    /// <param name="doorNum">The door number (0, 1, 2)</param>
    /// <returns>If the doors are opposite, used to check if a full flip is needed</returns>
    private bool CheckDoorOpposite(int doorNum)
    {
        bool signState;
        if (ML.currentRoom.signLying)
            signState = !ML.currentRoom.signSaysSafe;
        else
            signState = ML.currentRoom.signSaysSafe;

        switch (doorNum)
        {
            case 0:
                if (leftSafe != signState)
                    return true;
                break;
            case 1:
                if (middleSafe != signState)
                    return true;
                break;
            case 2:
                if (rightSafe != signState)
                    return true;
                break;
        }
        return false;
    }

    private void SetDoor(int doorNum, bool doorState)
    {
        switch (doorNum)
        {
            case 0:
                leftSafe = doorState;
                break;
            case 1:
                middleSafe = doorState;
                break;
            case 2:
                rightSafe = doorState;
                break;
        }
    }

    private void FlipOtherDoors(int doorNum)
    {
        switch (doorNum)
        {
            case 0:
                middleSafe = !middleSafe;
                rightSafe = !rightSafe;
                break;
            case 1:
                leftSafe = !leftSafe;
                rightSafe = !rightSafe;
                break;
            case 2:
                leftSafe = !leftSafe;
                middleSafe = !middleSafe;
                break;
        }
    }

    #region ALL RULES

    // NOTE: Rules which directly change the state of the sign should occur early, so that it can be more complex with rules that mention specific doors

    // If there is at least one rat, the sign is lying
    public bool LyingIfRat()
    {
        if (ML.currentRoom.numRats > 0)
        {
            signIsLying = true;
            print("There is a rat, sign is lying");
            return true;
        }
        return false;
    }

    // If there is water in the room, the left door is deadly
    public bool DripLeftDeath()
    {
        if (ML.currentRoom.numWater > 0)
        {
            print("There is dripping, left is deadly");
            leftSafe = false;
            leftTouched = true;
            return true;
        }
        return false;
    }

    // If you just entered from the middle, the right door is deadly
    public bool EnterMidRightDeath()
    {
        if (ML.currentRoom.entranceDoor == 2)
        {
            print("Came from the middle, right is deadly");
            rightSafe = false;
            rightTouched = true;
            return true;
        }
        return false;
    }

    // If there is an even number of rocks, the sign is lying
    public bool EvenRocksLying()
    {
        int rocks = ML.currentRoom.numRocks;
        if (rocks > 0 && rocks % 2 == 0)
        {
            print("Even number of rocks, sign is lying");
            signIsLying = true;
            return true;
        }
        return false;
    }

    // If there are a multiple of three rocks in the room, then the sign is truthful
    public bool MultipleThreeRocksTruthful()
    {
        int rocks = ML.currentRoom.numRocks;
        if (rocks > 0 && rocks % 3 == 0)
        {
            print("Multiple of 3 rocks, sign is truthful");
            signIsLying = false;
            return true;
        }
        return false;
    }

    // If there are both mushrooms and rats, but more mushrooms, the middle is not safe
    private bool MoreShroomsRatsMiddleDeath()
    {
        int rats = ML.currentRoom.numRats;
        int shrooms = ML.currentRoom.numShrooms;
        if (rats > 0 && shrooms > rats)
        {
            print("More shrooms than rats, middle is not safe");
            middleSafe = false;
            middleTouched = true;
            return true;
        }
        return false;
    }
    #endregion
}
