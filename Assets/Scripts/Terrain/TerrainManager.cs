using UnityEngine;
using System.Collections;
using SimplexNoise;
using System.Linq;
using System.Collections.Generic;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager instance;

    public int gridDiameter = 160;

    public float heightMapFreq1 = 0.001f;
    public float heightMapFreq2 = 0.001f;
    public float height1Amplitude = 30f;
    public float height2Amplitude = 30f;
    public float mountainCutoff = 0.5f;
    public float minHeight = 2f;
    public float moistureMapFreq1 = 0.001f;
    public float moistureMapFreq2 = 0.001f;
    public float moisture1Amplitude = 0.7f;
    public float moisture2Amplitude = 0.3f;
    public float soilQualityFreq1 = 0.001f;
    public float soilQualityFreq2 = 0.001f;
    public float soil1Amplitude = 0.7f;
    public float soil2Amplitude = 0.3f;
    public float waterCutoff = 0.2f;
    public float minDistanceBetweenRivers;
    public int numRivers = 5;
    public float riverMinCutoff = 0.5f;
    public float riverMoistureBonus = 0.1f;
    public float riverSoilBonus = 0.1f;
    public AnimationCurve edgeCurve;
    public float randomSeed;

    public int[,] heightMap { get; private set; }
    public float[,] moistureMap { get; private set; }
    public float[,] soilQualityMap { get; private set; }
    public bool[,] river { get; private set; }
    public bool[,] ocean { get; private set; }
    public int waterLevel;
    public int maxHeight;
    public int riverMin;

    private List<Vector2> riverPotential = new List<Vector2>();
    private Vector2 center;
    private float maxDistance;


    void Awake()
    {
        center = new Vector2(gridDiameter / 2f, gridDiameter / 2f);
        maxDistance = Vector2.Distance(center, new Vector2(0f, gridDiameter / 2f));
        randomSeed = Random.value * randomSeed;
    }

    void Start()
    {
        instance = this;
        GenerateMaps();
        DetermineWaterLevel();
        GridManager.instance.Init();
        World.instance.GenerateWorld();
    }

    void DetermineWaterLevel()
    {
        ocean = new bool[gridDiameter, gridDiameter];
        river = new bool[gridDiameter, gridDiameter];

        List<int> heights = new List<int>();

        for (int x = 0; x < gridDiameter; x++)
            for (int y = 0; y < gridDiameter; y++)
                if (heightMap[x,y] > 0)
                    heights.Add(heightMap[x, y]);

        heights.Sort();

        waterLevel = heights[Mathf.FloorToInt(heights.Count * waterCutoff)];
        riverMin = heights[Mathf.FloorToInt(heights.Count * riverMinCutoff)];
        maxHeight = heights[heights.Count - 1];

        for (int x = 0; x < gridDiameter; x++)
        {
            for (int y = 0; y < gridDiameter; y++)
            {
                if (heightMap[x, y] <= waterLevel)
                    ocean[x, y] = true;
                else if (heightMap[x, y] >= riverMin)
                    riverPotential.Add(new Vector2((float)x, (float)y));
            } 
        }

        GenerateRivers();
    }

    void GenerateRivers()
    {
        List<Vector2> riverChoices = new List<Vector2>();
        List<Vector2> filledRiver = new List<Vector2>();

        for (int i = 0; i < numRivers; i++)
        {
            Vector2 riverChoice = ChooseRiver(riverChoices);
            riverChoices.Add(riverChoice);
            List<Vector2> spots = RiverGenerator.GenerateRiver(riverChoice);
            filledRiver.AddRange(spots);

            foreach (Vector2 spot in spots)
            {
                riverPotential.Remove(spot);
                river[(int)spot.x, (int)spot.y] = true;
                heightMap[(int)spot.x, (int)spot.y] = heightMap[(int)spot.x, (int)spot.y] - 1;
            }
        }

        foreach (Vector2 riverLoc in filledRiver)
        {
            moistureMap[(int)riverLoc.x + 1,(int)riverLoc.y] = moistureMap[(int)riverLoc.x + 1, (int)riverLoc.y] + riverMoistureBonus;
            moistureMap[(int)riverLoc.x - 1, (int)riverLoc.y] = moistureMap[(int)riverLoc.x - 1, (int)riverLoc.y] + riverMoistureBonus;
            moistureMap[(int)riverLoc.x, (int)riverLoc.y + 1] = moistureMap[(int)riverLoc.x, (int)riverLoc.y + 1] + riverMoistureBonus;
            moistureMap[(int)riverLoc.x, (int)riverLoc.y - 1] = moistureMap[(int)riverLoc.x, (int)riverLoc.y - 1] + riverMoistureBonus;
        //    moistureMap[(int)riverLoc.x + 1, (int)riverLoc.y+1] = moistureMap[(int)riverLoc.x + 1, (int)riverLoc.y+1] + riverMoistureBonus;
        //    moistureMap[(int)riverLoc.x - 1, (int)riverLoc.y-1] = moistureMap[(int)riverLoc.x - 1, (int)riverLoc.y-1] + riverMoistureBonus;
         //   moistureMap[(int)riverLoc.x-1, (int)riverLoc.y + 1] = moistureMap[(int)riverLoc.x-1, (int)riverLoc.y + 1] + riverMoistureBonus;
        //    moistureMap[(int)riverLoc.x+1, (int)riverLoc.y - 1] = moistureMap[(int)riverLoc.x+1, (int)riverLoc.y - 1] + riverMoistureBonus;

            soilQualityMap[(int)riverLoc.x + 1, (int)riverLoc.y] = soilQualityMap[(int)riverLoc.x + 1, (int)riverLoc.y] + riverSoilBonus;
            soilQualityMap[(int)riverLoc.x - 1, (int)riverLoc.y] = soilQualityMap[(int)riverLoc.x - 1, (int)riverLoc.y] + riverSoilBonus;
            soilQualityMap[(int)riverLoc.x, (int)riverLoc.y + 1] = soilQualityMap[(int)riverLoc.x, (int)riverLoc.y + 1] + riverSoilBonus;
            soilQualityMap[(int)riverLoc.x, (int)riverLoc.y - 1] = soilQualityMap[(int)riverLoc.x, (int)riverLoc.y - 1] + riverSoilBonus;
           /// soilQualityMap[(int)riverLoc.x + 1, (int)riverLoc.y + 1] = soilQualityMap[(int)riverLoc.x + 1, (int)riverLoc.y + 1] + riverSoilBonus;
           // soilQualityMap[(int)riverLoc.x - 1, (int)riverLoc.y - 1] = soilQualityMap[(int)riverLoc.x - 1, (int)riverLoc.y - 1] + riverSoilBonus;
           // soilQualityMap[(int)riverLoc.x - 1, (int)riverLoc.y + 1] = soilQualityMap[(int)riverLoc.x - 1, (int)riverLoc.y + 1] + riverSoilBonus;
          //  soilQualityMap[(int)riverLoc.x + 1, (int)riverLoc.y - 1] = soilQualityMap[(int)riverLoc.x + 1, (int)riverLoc.y - 1] + riverSoilBonus;

        }
    }

   

    Vector2 ChooseRiver(List<Vector2> currentChoices)
    {
        if (currentChoices.Count == 0)
            return riverPotential[Random.Range(0, riverPotential.Count)];
        
        Vector2 bestSpot = riverPotential[0];
        List<Vector2> legal = new List<Vector2>();
        legal.Add(bestSpot);

        foreach (Vector2 possible in riverPotential)
        {
            float score = CompareToExisting(possible, currentChoices);
            if (score > minDistanceBetweenRivers)
                legal.Add(possible);
        }

        return legal[Random.Range(0, legal.Count)];
    }

    float CompareToExisting(Vector2 pos, List<Vector2> currentChoices)
    {
        float total = 0f;

        foreach (Vector2 vec in currentChoices)
            total += Vector2.Distance(pos, vec);

        return total;
    }

    void GenerateMaps()
    {
        heightMap = ConvertToInt(GenerateHeightMap(heightMapFreq1, heightMapFreq2, height1Amplitude, height2Amplitude, mountainCutoff));
        moistureMap = GenerateMap(moistureMapFreq1, moistureMapFreq2, moisture1Amplitude, moisture2Amplitude);
        soilQualityMap = GenerateMap(soilQualityFreq1, soilQualityFreq2, soil1Amplitude, soil2Amplitude);

    }

    float[,] GenerateMap(float freq1, float freq2,  float scale1 = 1f, float scale2 = 1f)
    {
        float[,] newMap = new float[gridDiameter, gridDiameter];
        float total = 0f;
        float counted = 0f;
        for (int x = 0; x < gridDiameter; x++)
        {
            for (int y = 0; y < gridDiameter; y++)
            {
                float result1 = (Noise.Generate((x + randomSeed) * freq1, (y + randomSeed) * freq1) + 1f)/2f * scale1;
                float result2 = (Noise.Generate((x + randomSeed) * freq2, (y + randomSeed) * freq2) + 1f)/ 2f * scale2;
                float edgeScale = edgeCurve.Evaluate(1f - Vector2.Distance(new Vector2((float)x, (float)y), center) / maxDistance);
                newMap[x, y] = (result1 + result2) * edgeScale;

                if (edgeScale > 0.9f)
                {
                    total += newMap[x, y];
                    counted++;
                }
                
            }
        }

        total /= counted;
        Debug.Log("Average is: " + total.ToString());


        return newMap;
    }

    float[,] GenerateHeightMap(float freq1, float freq2, float scale1 = 0.5f, float scale2 = 0.5f, float heightCutoff = 0.5f)
    {
        float[,] newMap = new float[gridDiameter, gridDiameter];

        for (int x = 0; x < gridDiameter; x++)
        {
            for (int y = 0; y < gridDiameter; y++)
            {
                float result1 = (Noise.Generate((x + randomSeed) * freq1, (y + randomSeed) * freq1) + 1f) / 2f * scale1;
                float noiseResult = (Noise.Generate((x + randomSeed) * freq2, (y + randomSeed) * freq2) + 1f) / 2f;
                noiseResult = noiseResult > heightCutoff ? noiseResult - heightCutoff  : 0f;
                float result2 = noiseResult * scale2;

                float edgeScale = edgeCurve.Evaluate(1f-Vector2.Distance(new Vector2((float)x, (float)y), center) / maxDistance);

                newMap[x, y] = (result1 + result2 + minHeight) * edgeScale;
            }
        }

        return newMap;
    }

    int[,] ConvertToInt(float[,] floatArray)
    {

        int[,] newArr = new int[gridDiameter, gridDiameter];

        for (int x = 0; x < gridDiameter; x++)
            for (int y = 0; y < gridDiameter; y++)
                newArr[x, y] = Mathf.FloorToInt(floatArray[x, y]);

        return newArr;

    }

    public int GetHeight(Vector2 loc)
    {
        return GetHeight((int)loc.x, (int)loc.y);
    }

    public int GetHeight(int x, int y)
    {
        return heightMap[x, y];
    }


}
