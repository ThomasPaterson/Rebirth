using UnityEngine;
using System.Collections;

public class BlockConfig : MonoBehaviour
{
    public static BlockConfig instance;

    public float sandSoilThreshold = 0.2f;
    public float sandMoistureThreshold = 0.2f;
    public int sandHeightThreshold = 2;

    public float snowMoistureThreshold = 0.9f;
    public int snowHeightThreshold = 2;

    public float rockSoilThreshold = 0.2f;
    public float rockMoistureThreshold = 0.2f;

    public float earthyRockSoilThreshold = 0.3f;
    public float earthyRockMoistureThreshold = 0.8f;

    public float darkGrassSoilThreshold = 0.6f;
    public float darkGrassMoistureThreshold = 0.8f;

    public float aridGrassSoilThreshold = 0.4f;
    public float aridGrassMoistureThreshold = 0.2f;

    public float dirtSoilThreshold = 0.4f;

    void Awake()
    {
        instance = this;
    }

    public Block DetermineChoice(GridLocation loc)
    {
        if (loc.IsWater())
            return new BlockWater();

        float soilQuality = loc.GetSoilQuality();
        float moisture = loc.GetMoisture();
        int height = loc.GetHeight();

        if (soilQuality < sandSoilThreshold && moisture < sandMoistureThreshold && height < TerrainManager.instance.waterLevel + sandHeightThreshold)
            return new BlockSand();
        else if (moisture < snowMoistureThreshold && height > TerrainManager.instance.maxHeight - snowHeightThreshold)
            return new BlockSnow();
        else if (moisture >= snowMoistureThreshold && height > TerrainManager.instance.maxHeight - snowHeightThreshold)
            return new Block();
        else if (soilQuality < rockSoilThreshold && moisture < rockMoistureThreshold)
            return new Block();
        else if (soilQuality < earthyRockSoilThreshold && moisture > earthyRockMoistureThreshold)
            return new BlockEarthy();
        else if (soilQuality > darkGrassSoilThreshold && moisture > darkGrassMoistureThreshold)
            return new BlockDarkGrass();
        else if (moisture < aridGrassMoistureThreshold)
            return new BlockAridGrass();
        else if (soilQuality < dirtSoilThreshold)
            return new BlockDirt();
        else
            return new BlockGrass();
    }
}

