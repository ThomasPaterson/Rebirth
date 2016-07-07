using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RiverGenerator
{
    private enum CanEnter { Lower, Higher, Same, Ocean}

    public static List<Vector2> GenerateRiver(Vector2 choice)
    {
        List<Vector2> choices = new List<Vector2>();
        choices.Add(choice);
        bool canMove = true;

        while (canMove)
        {
            Vector2 newPos = FindAdjacentLowest(choices);

            if (newPos != Vector2.one * -1f)
                choices.Insert(0, newPos);
            else
                canMove = false;
        }

        return choices;
    }

    static Vector2 FindAdjacentLowest(List<Vector2> current)
    {
        List<Vector2> ocean = new List<Vector2>();
        List<Vector2> lower = new List<Vector2>();
        List<Vector2> higher = new List<Vector2>();
        List<Vector2> same = new List<Vector2>();

        AddToResults(ocean, lower, higher, same, FindAdjacent(current[0], current[0] + Vector2.up, current), current[0] + Vector2.up);
        AddToResults(ocean, lower, higher, same, FindAdjacent(current[0], current[0] + Vector2.down, current), current[0] + Vector2.down);
        AddToResults(ocean, lower, higher, same, FindAdjacent(current[0], current[0] + Vector2.right, current), current[0] + Vector2.right);
        AddToResults(ocean, lower, higher, same, FindAdjacent(current[0], current[0] + Vector2.left, current), current[0] + Vector2.left);

        if (ocean.Count > 0)
            return Vector2.one * -1f;
        else if (lower.Count > 0)
            return ChooseBest(lower, current);
        else if (same.Count > 0)
            return ChooseBest(same, current);
        else
            return Vector2.one * -1f;


    }

    static Vector2 ChooseBest(List<Vector2> potential, List<Vector2> current)
    {
        if (current.Count < 2)
            return potential[Random.Range(0, potential.Count)];

        Vector2 lastDir = (current[0] - current[1]);

        List<Vector2> best = new List<Vector2>();

        foreach (Vector2 pot in potential)
            if (Vector2.Dot(pot, lastDir) > 0f)
                best.Add(pot);

        if (best.Count > 0)
            return best[Random.Range(0, best.Count)];

        return potential[Random.Range(0, potential.Count)];
    }

    static void AddToResults(List<Vector2> ocean, List<Vector2> lower, List<Vector2> higher, List<Vector2> same, CanEnter result, Vector2 target)
    {

        if (result == CanEnter.Same)
            same.Add(target);
        else if (result == CanEnter.Lower)
            lower.Add(target);
        else if (result == CanEnter.Higher)
            higher.Add(target);
        else
            ocean.Add(target);
    }

    static CanEnter FindAdjacent(Vector2 start, Vector2 pos, List<Vector2> current)
    {
        if (current.Contains(pos))
            return CanEnter.Higher;

        if (pos.x < 0 || pos.x >= TerrainManager.instance.gridDiameter || pos.y < 0 || pos.y >= TerrainManager.instance.gridDiameter)
            return CanEnter.Ocean;

        if (TerrainManager.instance.ocean[(int)pos.x, (int)pos.y])
            return CanEnter.Ocean;

        int startHeight = TerrainManager.instance.GetHeight(start);
        int targetHeight = TerrainManager.instance.GetHeight(pos);

        if (targetHeight < startHeight)
            return CanEnter.Lower;
        else if (targetHeight == startHeight)
            return CanEnter.Same;
        else
            return CanEnter.Higher;
    }


}
