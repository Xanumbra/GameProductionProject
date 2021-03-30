using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;

public enum Resources
{
    darkMatter, //#3
    spacePig,   //#4
    water,      //#4
    metal,      //#3
    energy,     //#4
    sun         //#1  --> always on index 9
}

[System.Serializable]
public struct PlanetResourcesDict
{
    public GameObject planetPrefab;
    public Resources resource;
}

public class MapGenerator : NetworkBehaviour
{
    private static MapGenerator _instance;
    public static MapGenerator Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    public HexGrid hexGrid;
    public List<PlanetResourcesDict> planetPrefabs;

    private List<Resources> initialResources = new List<Resources>
    {
        Resources.darkMatter,
        Resources.darkMatter,
        Resources.darkMatter,
        Resources.spacePig,
        Resources.spacePig,
        Resources.spacePig,
        Resources.spacePig,
        Resources.water,
        Resources.water,
        Resources.water,
        Resources.water,
        Resources.metal,
        Resources.metal,
        Resources.metal,
        Resources.energy,
        Resources.energy,
        Resources.energy,
        Resources.energy,
        Resources.sun
    };
    private List<Resources> shuffledResources = new List<Resources>();

    private int seed = 2345;

    private int[] numbers = { 2, 3, 3, 4, 4, 5, 5, 9, 9, 10, 10, 11, 11, 12 };
    private int[] numbers68 = { 6, 6, 8, 8 };
    private int[,] gamefield = {
                    {0, 0, 1, 1, 1},
                     {0, 1, 1, 1, 1},
                      {1, 1, 0, 1, 1},
                       {1, 1, 1, 1, 0},
                        {1, 1, 1, 0, 0}
    };

    private List<GameObject> spawnedPlanets = new List<GameObject>();

    [Server]
    public int GeneratePlanets(string seedInput)
    {
        Random.InitState(1);

        try
        {
            seed = System.Int32.Parse(seedInput);
            Debug.Log("User seed: " + seedInput);
        }
        catch
        {
            Debug.Log("No or invalid seed entered!");
            seed = Random.Range(1000, 10000000);
            Debug.Log("Random Seed: " + seed);
        }

        Random.InitState(seed);
        ShuffleRessourceList();

        if (spawnedPlanets.Count > 0) RpcClearPlanets();

        RpcSpawnPlanets(shuffledResources);

        return seed;
    }


    [Server]
    void ShuffleRessourceList()
    {
        int n = initialResources.Count;
        shuffledResources = new List<Resources>(initialResources);

        while (n > 0)
        {
            n--;
            int k = Random.Range(0, n-1);
            Resources value = shuffledResources[k];
            shuffledResources[k] = shuffledResources[n];
            shuffledResources[n] = value;
        }

        var sunIndex = shuffledResources.IndexOf(Resources.sun);
        shuffledResources[sunIndex] = shuffledResources[9];
        shuffledResources[9] = Resources.sun;
    }

    [ClientRpc]
    void RpcSpawnPlanets(List<Resources> resources)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            var planet = Instantiate(planetPrefabs.Where(dict => dict.resource == resources[i]).Select(dict => dict.planetPrefab).ElementAt(0), hexGrid.cells[i].transform);
            spawnedPlanets.Add(planet);
        }
    }

    [ClientRpc]
    void RpcClearPlanets()
    {
        foreach (var cell in hexGrid.cells)
        {
            Destroy(cell.transform.GetChild(0).gameObject);
        }
    }

    void InitNumbers68()
    {
        var i = Random.Range(0, 4);
        var j = Random.Range(0, 4);

        while (gamefield[i,j] != 1)
        {
            i = Random.Range(0, 4);
            j = Random.Range(0, 4);
        }

        gamefield[i, j] = numbers68[0];
        Debug.Log($"Fill cell {i}-{j} with {numbers[0]}");


        for (int n = 1; n <= 4; n++)
        {
            i = Random.Range(0, 4);
            j = Random.Range(0, 4);

            // ENDLESS LOOP WEGEN SEED --> LÖSUNG FINDEN
            while (gamefield[i,j] != 1 || 
                (i > 0 && (gamefield[i-1, j] == 6 || gamefield[i-1, j] == 8)) 
                || ((i > 0 && j< 4) && (gamefield[i - 1, j + 1] == 6 || gamefield[i - 1, j + 1] == 8))
                || (j > 0 && (gamefield[i, j - 1] == 6 || gamefield[i, j - 1] == 8)) 
                || (j < 4 && (gamefield[i, j + 1] == 6 || gamefield[i, j + 1] == 8)) 
                || ((i < 4 && j > 0) && (gamefield[i + 1, j - 1] == 6 || gamefield[i + 1, j - 1] == 8)) 
                || (i < 4 && (gamefield[i + 1, j] == 6 || gamefield[i + 1, j] == 8)))
            {
                Debug.Log($"Neighbor collision at index: {i}-{j}");
                i = Random.Range(0, 4);
                j = Random.Range(0, 4);
            }

            gamefield[i, j] = numbers68[n];
            Debug.Log($"Fill cell {i}-{j} with {numbers68[n]}");
        }
    }

    void InitNumbersOthers()
    {
        ShuffleNumbers();

        int n = 0;
        for (int i = 0; i < gamefield.GetLength(0); i++)
        {
            for (int j = 0; j < gamefield.GetLength(1); j++)
            {
                if (gamefield[i,j] != 1)
                {
                    gamefield[i, j] = numbers[n];
                    n++;
                }
            }
        }
    }

    void ShuffleNumbers()
    {
        int n = numbers.Length;
        while (n > 0)
        {
            n--;
            int k = Random.Range(0, n - 1);
            int value = numbers[k];
            numbers[k] = numbers[n];
            numbers[n] = value;
        }
    }

    void PrintRessources()
    {
        Debug.Log("-------- Ressources --------");
        string s = "";

        foreach (var r in initialResources)
        {
            s += r.ToString();
            s += "-";
        }

        Debug.Log(s);
        Debug.Log("-------- ------ --------");
    }

    void PrintNumbers()
    {
        Debug.Log("-------- Numbers --------");

        string s = "";
        foreach (var n in gamefield)
        {
            s += n;
        }

        Debug.Log(s);
    }
}
