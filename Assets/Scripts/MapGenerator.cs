using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;
using TMPro;



[System.Serializable]
public struct PlanetResourcesDict
{
    public GameObject planetPrefab;
    public Enums.Resources resource;
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

    private List<Enums.Resources> initialResources = new List<Enums.Resources>
    {
        Enums.Resources.darkMatter,
        Enums.Resources.darkMatter,
        Enums.Resources.darkMatter, 
        Enums.Resources.spacePig,
        Enums.Resources.spacePig,
        Enums.Resources.spacePig,
        Enums.Resources.spacePig,
        Enums.Resources.water,
        Enums.Resources.water,
        Enums.Resources.water,
        Enums.Resources.water,
        Enums.Resources.metal,
        Enums.Resources.metal,
        Enums.Resources.metal,
        Enums.Resources.energy,
        Enums.Resources.energy,
        Enums.Resources.energy,
        Enums.Resources.energy,
        Enums.Resources.sun
    };
    private List<Enums.Resources> shuffledResources = new List<Enums.Resources>();

    private int seed = 2345;

    private int[] initialNumbers = { 2, 3, 3, 4, 4, 5, 5, 9, 9, 10, 10, 11, 11, 12 };
    private int[] shuffledNumbers = new int[14];
    private int[] numbers68 = { 6, 6, 8, 8 };
    private Dictionary<int, List<int>> neighborCells = new Dictionary<int, List<int>>
    {
        { 0, new List<int> {1, 3, 4} },
        { 1, new List<int> {0, 2, 4, 5} },
        { 2, new List<int> {1, 5, 6} },
        { 3, new List<int> {0, 4, 7, 8} },
        { 4, new List<int> {0, 1, 3, 5, 8, 9} },
        { 5, new List<int> {1, 2, 4, 6, 9, 10} },
        { 6, new List<int> {2, 5, 10, 11} },
        { 7, new List<int> {3, 8, 12} },
        { 8, new List<int> {3, 4, 7, 9, 12, 13} },
        //{ 9, new List<int> {1, 3, 4} },
        { 10, new List<int> {5, 6, 9, 11, 14, 15} },
        { 11, new List<int> {6, 10, 15} },
        { 12, new List<int> {7, 8, 13, 16} },
        { 13, new List<int> {8, 9, 12, 14, 16, 17} },
        { 14, new List<int> {9, 10, 13, 15, 17, 18} },
        { 15, new List<int> {10, 11, 14, 18} },
        { 16, new List<int> {12, 13, 17} },
        { 17, new List<int> {13, 14, 16, 18} },
        { 18, new List<int> {14, 15, 17} },

    };

    private int[] finalNumbers = new int[19];

    private List<GameObject> spawnedPlanets = new List<GameObject>();


    void OnEnable()
    {
        //InitNumbers68();
        //InitNumbersOthers();
        //PrintNumbers();
    }

    [Server]
    public int GenerateMap(string seedInput)
    {
        Random.InitState(System.Environment.TickCount);

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
        InitNumbers68();
        InitNumbersOthers();

        if (spawnedPlanets.Count > 0) RpcClearPlanets();
        RpcSpawnPlanets(shuffledResources, finalNumbers);

        return seed;
    }

    [Server]
    void ShuffleRessourceList()
    {
        int n = initialResources.Count;
        shuffledResources = new List<Enums.Resources>(initialResources);

        while (n > 0)
        {
            n--;
            int k = Random.Range(0, n-1);
            Enums.Resources value = shuffledResources[k];
            shuffledResources[k] = shuffledResources[n];
            shuffledResources[n] = value;
        }

        var sunIndex = shuffledResources.IndexOf(Enums.Resources.sun);
        shuffledResources[sunIndex] = shuffledResources[9];
        shuffledResources[9] = Enums.Resources.sun;
    }

    [ClientRpc]
    void RpcSpawnPlanets(List<Enums.Resources> resources, int[] numbers)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            var planet = Instantiate(planetPrefabs.Where(dict => dict.resource == resources[i]).Select(dict => dict.planetPrefab).ElementAt(0), hexGrid.cells[i].transform);
            spawnedPlanets.Add(planet);
            hexGrid.cells[i].cellResourceType = resources[i];
            try
            {
                planet.GetComponentInChildren<TMP_Text>().text = numbers[i].ToString();

                if (numbers[i] == 6 || numbers[i] == 8)
                {
                    planet.GetComponentInChildren<TMP_Text>().color = Color.red;
                }
            }
            catch
            {
                //Debug.Log("No text at sun");
            }
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

    [Server]
    void InitNumbers68()
    {
        finalNumbers = new int[19];
        var index = 0;

        bool collision = false;
        for (int i = 0; i < numbers68.Length; i++)
        {
            do
            {
                index = Random.Range(0, 18);
                collision = false;

                if (index != 9 && finalNumbers[index] == 0)
                {
                    foreach (var cell in neighborCells[index])
                    {
                        if (finalNumbers[cell] == 6 || finalNumbers[cell] == 8)
                        {
                            collision = true;
                            break;
                        }
                    }
                }
                else collision = true;

            } while (collision);

            finalNumbers[index] = numbers68[i];
        }
    }

    [Server]
    void InitNumbersOthers()
    {
        ShuffleNumbers();

        int n = 0;
        for (int i = 0; i < finalNumbers.Length; i++)
        {
            if (finalNumbers[i] == 0 && i != 9)
            {
                finalNumbers[i] = shuffledNumbers[n];
                n++;
            }
        }
    }

    [Server]
    void ShuffleNumbers()
    {
        int n = initialNumbers.Length;
        initialNumbers.CopyTo(shuffledNumbers, 0);
        while (n > 0)
        {
            n--;
            int k = Random.Range(0, n - 1);
            int value = shuffledNumbers[k];
            shuffledNumbers[k] = shuffledNumbers[n];
            shuffledNumbers[n] = value;
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
        foreach (var n in finalNumbers)
        {
            s += n;
            s += "-";
        }

        Debug.Log(s);
    }
}
