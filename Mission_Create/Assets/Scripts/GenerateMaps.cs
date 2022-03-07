using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMaps : MonoBehaviour
{


    public GameObject map;
    [Header("For ship prefabs")]
    public GameObject smallShip;
    public GameObject mediumShip;
    public GameObject largeShip;
    
    
    public int numLayers = 10;
    [Tooltip("The fix station that has update")]
    public int upStationIndex = 4;
    [Tooltip("The fix station that has shop")]
    public int shopIndex = 5;
    public int difficultyUpTimes = 2;
    [HideInInspector]
    public int[] difficultyUpIndex;

    //might not be of much use, just record keeping
    private int[] mapsEachLayer;

    [Header("how many maps per layer")]
    public int maxMapPerLayer = 4;
    public int minMapPerLayer = 2;


    [Header("Ship number bounds")]
    public int minLargeShip = 0;
    public int minMediumShip = 1;
    public int minSmallShip = 2;

    
    public int maxLargeShip = 1;
    public int maxMediumShip = 2;
    public int maxSmallShip = 4;

    [Header("Ship reward")]
    public int rewardSmallShip = 100;
    public int rewardMediumShip = 500;
    public int rewardLargeShip = 1000;


    

    // 2D array used to store all map class instances.
    public GameObject[][] layers;
    private int currDifficulty = 0;
   

    [Tooltip("Choose to have random number of upstation through out map")]
    public bool randUpStation = true;
    public bool randShop = true;
    [Tooltip("Randomly generate upgrade stations other beside the station on layer 4")]
    public int numUpStation = 0;
    public int maxUpStation = 2;
    public int minUpStation = 0;
    private int[] shopLocations;
    private int[] upLocations;

    private int[] shopLayers;
    private int[] upLayers;

    private int lastNumber;
    
    // Start is called before the first frame update
    void Start()
    {
        layers = new GameObject[numLayers][];
        mapsEachLayer = new int[numLayers];
        difficultyUpIndex = new int[difficultyUpTimes];

        for (int i = 0; i < difficultyUpIndex.Length; i++)
        {
            int section = numLayers / difficultyUpTimes;
            difficultyUpIndex[i] = UnityEngine.Random.Range(section * i, section * (i + 1));
            
            //Debug.Log("Difficulty up index is: " + difficultyUpIndex[i]);
        }

        for (int i = 0; i < numLayers; i++)
        {
            int mapInLayer = UnityEngine.Random.Range(minMapPerLayer, maxMapPerLayer);
            
            mapsEachLayer[i] = mapInLayer;
            layers[i] = new GameObject[mapInLayer];
            
        }
        if (randUpStation)
        {
            numUpStation = UnityEngine.Random.Range(minUpStation, maxUpStation);
        }
        //TODO: if random location of shop or upgrade is desired can be set up here. 




        for (int i = 0; i < numLayers; i++)
        {
            //used to increment difficulty level
            for (int j = 0; j < difficultyUpIndex.Length; j++)
            {
                if (i == difficultyUpIndex[j])
                {
                    currDifficulty += 1;
                }
            }
            //hard coded shop and upgrade location
            if (i == shopIndex)
            {
                Debug.Log("rewrite layer: " + i);
                layers[i] = new GameObject[1];
                layers[i][0] = Instantiate(map);
                layers[i][0].GetComponent<map>().isShop = true;
            }
            else if (i == upStationIndex)
            {
                Debug.Log("rewrite layer: " + i);
                layers[i] = new GameObject[1];
                layers[i][0] = Instantiate(map);
                layers[i][0].GetComponent<map>().isUpgradeStation = true;
            }

            else {
                for (int j = 0; j < layers[i].Length; j++)
                {

                    layers[i][j] = Instantiate(map);
                    layers[i][j].GetComponent<map>().difficultyLevel = currDifficulty;

                    int numSmall = UnityEngine.Random.Range(minSmallShip, maxSmallShip);
                    int numMid = UnityEngine.Random.Range(minMediumShip, maxMediumShip);
                    int numLarge = UnityEngine.Random.Range(minLargeShip, maxLargeShip);
                    layers[i][j].GetComponent<map>().smallShip = numSmall;
                    layers[i][j].GetComponent<map>().mediumShip = numMid;
                    layers[i][j].GetComponent<map>().largeShip = numLarge;

                    layers[i][j].GetComponent<map>().moneyReward = numSmall * rewardSmallShip + numMid * rewardMediumShip +
                        numLarge * rewardLargeShip;
                    layers[i][j].GetComponent<map>().healthReward = (int)(layers[i][j].GetComponent<map>().moneyReward / 10);
                }
            }
        }
        //Debug.Log("layer 7, map 2's enemy: " + layers[6][1].GetComponent<map>().moneyReward);

        
        
    }
    int GetRandom(int min, int max)
    {
        int rand = Random.Range(min, max);
        while (rand == lastNumber)
            rand = Random.Range(min, max);
        lastNumber = rand;
        return rand;
    }

}
