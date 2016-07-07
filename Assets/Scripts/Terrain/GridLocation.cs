using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GridLocation
{

    public int x { get; private set; }
    public int y { get; private set; }
    public Vector2 pos;
    public List<IGridOccupier> occupiers = new List<IGridOccupier>();

    private Vector3 worldPos;
    private bool hasTree;

    public GridLocation(int x, int y)
    {
        this.x = x;
        this.y = y;
        pos = new Vector2((float)x, (float)y);

        worldPos = new Vector3(x, TerrainManager.instance.heightMap[x, y] + 0.5f, y);
        worldPos.Scale(World.instance.meshScale);
    }

    public void Occupy(IGridOccupier newOccupier)
    {
        if (newOccupier == null)
            return;

        else if (newOccupier.loc != null)
            newOccupier.loc.Leave(newOccupier);

        occupiers.Add(newOccupier);

        newOccupier.loc = this;

        if (newOccupier.GetGameObject().GetComponent<Plant>() != null)
            if (newOccupier.GetGameObject().GetComponent<Plant>().species.type == PlantSpecies.Type.Tree)
                hasTree = true;
    }

    public void Leave(IGridOccupier leaving)
    {
        occupiers.Remove(leaving);

        if (leaving.GetSpecies() is PlantSpecies)
        {
            foreach (IGridOccupier occupier in occupiers)
                if (occupier.GetGameObject().GetComponent<Plant>() != null)
                    if (occupier.GetGameObject().GetComponent<Plant>().species.type == PlantSpecies.Type.Tree)
                        hasTree = true;
        }

        
    }

    public Vector3 GetPosition()
    {
        return worldPos;
    }

    public float GetSoilQuality()
    {
        return TerrainManager.instance.soilQualityMap[x, y];
    }

    public float GetMoisture()
    {
        return TerrainManager.instance.moistureMap[x, y];
    }

    public int GetHeight()
    {
        return TerrainManager.instance.heightMap[x, y];
    }

    public bool IsWater()
    {
        return TerrainManager.instance.ocean[x, y] || TerrainManager.instance.river[x, y];
    }

    public bool HasTree()
    {
        return hasTree;
    }

}
