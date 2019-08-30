﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public bool isDamaged = false;
	public int numFlares = 2;

    public string playerRules = "";

    public float timeRemaining;

	//Rocks V Stalactites V etc....
	private string key = "0v0v0v0v0";

    [SerializeField]
    private GameObject damaged;

    //Add more lists of the objects as needed to be spawned
    [SerializeField]
	private List<GameObject> rocks, mushrooms, rats, webs, gems;

    private List<List<GameObject>> allProps;

	[SerializeField]
	private List<GameObject> spawnPoints;
	[SerializeField]
	List<Vector2> TransformPoints;
	List<Vector2> availableTransformPoints = new List<Vector2>();

    // Spawn point remaining in this room
    private List<GameObject> availableSpawnPoints = new List<GameObject>();
    // TODO: Individual spawn points

    // Backpack Spawn
    [SerializeField]
    List<Vector2> backpackTransformPoints = new List<Vector2>();

    [SerializeField]
    private List<GameObject> backPackSpawnPoints;
    [SerializeField]
    private GameObject backPackObject;

    // Rat Spawn
    [SerializeField]
    List<Vector2> ratTransformPoints = new List<Vector2>();

    [SerializeField]
    private List<GameObject> ratSpawn;
    [SerializeField]
    private GameObject donavan;

    // Stalagmite Spawn
    [SerializeField]
    List<Vector2> stalagmiteTransformPoints = new List<Vector2>();

    [SerializeField]
    private List<GameObject> stalagmiteSpawn;
    [SerializeField]
    private List<GameObject> stalagmite;

    // Crack Spawn
    [SerializeField]
    List<Vector2> crackTransformPoints = new List<Vector2>();

    [SerializeField]
    private List<GameObject> crackSpawn;
    [SerializeField]
    private List<GameObject> crack;

    // Mushroom Spawn
    [SerializeField]
    List<Vector2> shroomTransfromPoints = new List<Vector2>();

    [SerializeField]
    private List<GameObject> shroomSpawn;
    [SerializeField]
    private List<GameObject> shrooms;

    // Gem Spawn
    [SerializeField]
    List<Vector2> gemTransformPoints = new List<Vector2>();

    [SerializeField]
    private List<GameObject> gemSpawn;
    [SerializeField]
    private List<GameObject> gem;

    // Rock Spawn
    [SerializeField]
    List<Vector2> rockTransformPoints = new List<Vector2>();

    [SerializeField]
    private List<GameObject> rockSpawn;
    [SerializeField]
    private List<GameObject> rock;

    // Web Spawn
    [SerializeField]
    List<Vector2> webTransformPoints = new List<Vector2>();

    [SerializeField]
    private List<GameObject> webSpawn;
    [SerializeField]
    private GameObject web;

    // The player is currently in a trap QTE
    public bool currentlyTrapped;

    //This controlls the number of things the key will be able to generate. Decided by list of things like rocks and stalactites
    int maxNumberOfThings = 2, difficultyLevels = 1, minNumberOfThings = 0;

	// Start is called before the first frame update
	void Start()
    {
        isDamaged = false;
        currentlyTrapped = false;
		damaged = GameObject.Find("Damage");
        damaged.SetActive(false);
        allProps = new List<List<GameObject>>() { };
		//for(int i = 0; i < spawnPoints.Count;i++)
		//{
		//	availableSpawnPoints.Add(spawnPoints[i]);
		//}
		for (int i = 0; i < spawnPoints.Count; i++)
		{
			TransformPoints.Add(spawnPoints[i].transform.position);
		}
		for (int i = 0; i < TransformPoints.Count; i++)
		{
			availableTransformPoints.Add(spawnPoints[i].transform.position);
		}
		for (int i = 0; i < backPackSpawnPoints.Count; i++)
		{
			backpackTransformPoints.Add(backPackSpawnPoints[i].transform.position);
		}

        // Donavan spawns
        for (int i = 0; i < ratSpawn.Count; i++)
        {
            ratTransformPoints.Add(ratSpawn[i].transform.position);
        }

        // Stalagmite spawns
        for(int i = 0; i < stalagmiteSpawn.Count; i++)
        {
            stalagmiteTransformPoints.Add(stalagmiteSpawn[i].transform.position);
        }

        // Crack spawns
        for(int i = 0; i < crackSpawn.Count; i++)
        {
            crackTransformPoints.Add(crackSpawn[i].transform.position);
        }

        // Shroom spawns
        for(int i = 0; i < shroomSpawn.Count; i++)
        {
            shroomTransfromPoints.Add(shroomSpawn[i].transform.position);
        }

        // Gem spawns
        for(int i = 0; i < gemSpawn.Count; i++)
        {
            gemTransformPoints.Add(gemSpawn[i].transform.position);
        }

        // Rock spawns
        for(int i = 0; i < rockSpawn.Count; i++)
        {
            rockTransformPoints.Add(rockSpawn[i].transform.position);
        }

        // Web spawns
        for(int i = 0; i < webSpawn.Count; i++)
        {
            webTransformPoints.Add(webSpawn[i].transform.position);
        }

        //	foreach (GameObject point in availableSpawnPoints)
        //   {
        //       DontDestroyOnLoad(point);
        //   }

        //   foreach(GameObject itemPoint in backPackSpwnPoints)
        //   {
        //       DontDestroyOnLoad(itemPoint);
        // }


        if (instance == null)
		{
			instance = this;
		}
		//Debugging Purposes
		Debug.Log("GameStart");
		DontDestroyOnLoad(instance);
        allProps.Add(rocks);
        allProps.Add(mushrooms);
        allProps.Add(rats);
        allProps.Add(webs);
        allProps.Add(gems);

		//Debug.Log(key);
        //KeyUpdate();
        //GenerateNewKey();
		//Debug.Log(key);
	}

    private void Update()
    {
		if(damaged == null)
		{
			damaged = GameObject.Find("Damage");
		}
		if(damaged != null)
		 damaged.SetActive(isDamaged);
    }
    //call this on the completion of a room to update the key
    void KeyUpdate()
	{
        int numberOfThings = Random.Range(minNumberOfThings, maxNumberOfThings + 1);
		List<int> keyPart = new List<int>();
		for (int i = 0; i < numberOfThings; i++)
		{
			//This adds the more to the part in the key that we are accessing
			keyPart.Add(int.Parse(key.Split('v')[i]) + difficultyLevels);
		}
		//This creates the new key
		key = keyPart[0].ToString() + 'v' + keyPart[1].ToString();
	}

	public string GenerateNewKey()
	{
		int numberOfThings = Random.Range(minNumberOfThings, maxNumberOfThings + 1);
		string newKey = "";
		for (int i = 0; i < allProps.Count; i++)
		{
			// If we still have objects to place, then place a random number of this object
			if (numberOfThings > 0)
			{
				int numberOfThisProp;
				if (i == allProps.Count)
				{
					// This is the last spot, dump the rest of the things here
					numberOfThisProp = numberOfThings;
				}
				else
				{
					numberOfThisProp = Random.Range(0, numberOfThings + 1);
					newKey += numberOfThisProp.ToString() + "v";
					numberOfThings -= numberOfThisProp;
				}
				// Place the item in the room
				print("Attempting to place with key: " + newKey);
				PlaceItems(allProps[i], numberOfThisProp);
			}
			else
			{
				newKey += 0.ToString() + "v";
			}
		}
		print("Built Key: " + newKey);
		for (int i = 0; i < TransformPoints.Count; i++)
		{
			availableTransformPoints.Add(TransformPoints[i]);
		}
			

        // Will an item spawn in this room?
        CheckSpawnItems();

        return newKey;
    }

    public void CheckSpawnItems()
    {
        int rollItem = Random.Range(1, 11);
        int itemsToSpawn = 0;
        // 1-2 spawns one backpack (20%)
        if (rollItem < 3)
        {
            itemsToSpawn = 1;
        }
        // 10 spawns 2 backpacks (10%)
        else if (rollItem == 10)
        {
            itemsToSpawn = 2;
        }

        if (itemsToSpawn > 0)
        {
            // Pick a random spawn point and place a pack there
            GameObject pack = Instantiate(backPackObject, backpackTransformPoints[Random.Range(0, backpackTransformPoints.Count)], Quaternion.identity);
            pack.transform.SetParent(gameObject.transform);

            // Probably not the right spot to put these...
            GameObject rat = Instantiate(donavan, ratTransformPoints[Random.Range(0, ratTransformPoints.Count)], Quaternion.identity);
            rat.transform.SetParent(gameObject.transform);

            GameObject spike = Instantiate(stalagmite[Random.Range(0, stalagmite.Count)], stalagmiteTransformPoints[Random.Range(0, stalagmiteTransformPoints.Count)], Quaternion.identity);
            spike.transform.SetParent(gameObject.transform);

            GameObject wallCrack = Instantiate(crack[Random.Range(0, crack.Count)], crackTransformPoints[Random.Range(0, crackTransformPoints.Count)], Quaternion.identity);
            wallCrack.transform.SetParent(gameObject.transform);

            GameObject fungi = Instantiate(shrooms[Random.Range(0, shrooms.Count)], shroomTransfromPoints[Random.Range(0, shroomTransfromPoints.Count)], Quaternion.identity);
            fungi.transform.SetParent(gameObject.transform);

            GameObject gemz = Instantiate(gem[Random.Range(0, gem.Count)], gemTransformPoints[Random.Range(0, gemTransformPoints.Count)], Quaternion.identity);
            gemz.transform.SetParent(gameObject.transform);

            GameObject rok = Instantiate(rock[Random.Range(0, rock.Count)], rockTransformPoints[Random.Range(0, rockTransformPoints.Count)], Quaternion.identity);
            rok.transform.SetParent(gameObject.transform);

            GameObject webz = Instantiate(web, webTransformPoints[Random.Range(0, webTransformPoints.Count)], Quaternion.identity);
            webz.transform.SetParent(gameObject.transform);
        }
    }

    public void UpgradeKey()
    {
        minNumberOfThings += 1;
        maxNumberOfThings += 1;
        if (maxNumberOfThings >= spawnPoints.Count)
        {
            maxNumberOfThings = spawnPoints.Count;
        }
        if (minNumberOfThings >= maxNumberOfThings)
        {
            minNumberOfThings = maxNumberOfThings;
        }
        print("Min: " + minNumberOfThings + " Max: " + maxNumberOfThings);
        print("Number of things in spawnPoints is: " + spawnPoints.Count);
    }

    // Place items on the random spawnpoints
	void PlaceItems(List<GameObject> propList, int howMany)
	{
        if (howMany > 0)
        {
            // Place the item at the point
            for (int i = 0; i < howMany; i++)
            {
                if (availableTransformPoints.Count > 0 && propList.Count > 0)
                {
                    // Pick a random available spawn point to place it at
                    int randomPoint = Random.Range(0, availableSpawnPoints.Count);

                    // Pick a random prefab from the proplist
                    int randomProp = Random.Range(0, propList.Count);

                    // Place the item at the point
                    print("Attempting to place prop number: " + randomProp + " at spawnPoint " + randomPoint);
                    GameObject prop = Instantiate(propList[randomProp], availableTransformPoints[randomPoint], Quaternion.identity);
                    prop.transform.SetParent(gameObject.transform);

                    // Remove this spawn point from the list
                    availableTransformPoints.RemoveAt(randomPoint);
                }
            }
        }
	}

    public void DestroyProps()
    {
        List<GameObject> children = new List<GameObject>() { };
        foreach(Transform child in gameObject.transform)
        {
            children.Add(child.gameObject);
        }

        foreach(GameObject prop in children)
        {
            Destroy(prop);
        }
    }
}
