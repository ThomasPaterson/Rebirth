using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{

    public static GridManager instance;

    public GridLocation[,] grid;

    void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        grid = new GridLocation[TerrainManager.instance.gridDiameter, TerrainManager.instance.gridDiameter];

        for (int x = 0; x < TerrainManager.instance.gridDiameter; x++)
            for (int y = 0; y < TerrainManager.instance.gridDiameter; y++)
                grid[x, y] = new GridLocation(x, y);
    }

    public List<GridLocation> GetBox(GridLocation center, int radius)
    {
        List<GridLocation> locations = new List<GridLocation>();

        for (int x = center.x - radius; x < center.x + radius; x++)
            for (int y = center.y - radius; y < center.y + radius; y++)
                locations.Add(GetGridLocation(x, y));

        return locations;
    }

    public GridLocation GetLocFromWorld(Vector3 worldPos)
    {
        return GetGridLocation(Mathf.RoundToInt(worldPos.x - 2f), Mathf.RoundToInt(worldPos.z + 2f));
    }

    public GridLocation GetGridLocation(Vector2 loc)
    {
        return GetGridLocation((int)loc.x, (int)loc.y);
    }

    public GridLocation GetGridLocation(int x, int y)
    {
        if (x < 0 || y < 0 || x >= TerrainManager.instance.gridDiameter || y >= TerrainManager.instance.gridDiameter)
            return null;
        else
            return grid[x, y];
    }

    public GridLocation[] GetAdjacent(GridLocation loc)
    {
        GridLocation[] adjacent = new GridLocation[4];

        adjacent[0] = GetGridLocation(loc.pos + Vector2.up);
        adjacent[1] = GetGridLocation(loc.pos + Vector2.right);
        adjacent[2] = GetGridLocation(loc.pos + Vector2.down);
        adjacent[3] = GetGridLocation(loc.pos + Vector2.left);

        return adjacent;
    }
}
