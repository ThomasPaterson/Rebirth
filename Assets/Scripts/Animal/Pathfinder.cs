using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Pathfinder : MonoBehaviour
{
    public float walkSpeed = 0.5f;

    private Mind mind;
    private Animal animal;
    private bool canLook = true;
 

    private List<GridLocation> movePath = new List<GridLocation>();

    
    void Start()
    {
        mind = GetComponent<Mind>();
        animal = GetComponent<Animal>();

        StartCoroutine(WalkOnPath());
    }

    IEnumerator WalkOnPath()
    {
        yield return null;

        while (true)
        {
            if (mind.target != null && mind.currentDesire == UtilityGenerator.Desire.RunAway)
            {
                Runaway();
                yield return new WaitForSeconds(walkSpeed * animal.GetRunSpeed());
            }
            else if (mind.target != null && mind.target.GetLocation() == animal.loc)
            {
                yield return new WaitForSeconds(walkSpeed);
            }
            else if (canLook && mind.target != null && (movePath.Count == 0 || movePath[0] != mind.target.GetLocation()))
            {
                FindPath();
                yield return new WaitForSeconds(walkSpeed);
                canLook = false;
            }
            else
            {
                if (movePath.Count > 0)
                {
                    MoveOnPath();
                    float time = mind.currentDesire == UtilityGenerator.Desire.EatMeat ? animal.GetChaseSpeed() : 1f;
                    yield return new WaitForSeconds(walkSpeed * time);
                }
                else
                {
                    MoveRandom();
                    yield return new WaitForSeconds(walkSpeed);
                }
            }
              
        }
    }

    void Runaway()
    {
        GridLocation[] locs = GridManager.instance.GetAdjacent(animal.loc);

        float maxDistance = float.MinValue;
        GridLocation best = null;

        foreach (GridLocation loc in locs)
        {
            if (!animal.CanEnter(loc))
                continue;

            float distance = Vector2.Distance(loc.GetPosition(), mind.target.GetLocation().GetPosition());

            if (distance > maxDistance)
            {
                maxDistance = distance;
                best = loc;
            }
        }

        movePath = new List<GridLocation>();

        if (best != null)
            animal.MoveTo(best);

    }

    public void FindPath()
    {
            movePath = FindPath(animal, mind.target.GetLocation());
    }

    public void SetCanSeek()
    {
        canLook = true;
    }

    void MoveOnPath()
    {
        if (animal.CanEnter(movePath[movePath.Count - 1]))
            animal.MoveTo(movePath[movePath.Count - 1]);

        movePath.RemoveAt(movePath.Count - 1);

        if (movePath.Count == 0 && mind.target != null && mind.target.GetLocation() != animal.loc)
            FindPath();
    }

    void MoveRandom()
    {
        int directionX = Mathf.RoundToInt(Random.Range(-1f, 1f)) + animal.loc.x;
        int directionY = Mathf.RoundToInt(Random.Range(-1f, 1f)) + animal.loc.y;

        GridLocation newLoc = GridManager.instance.GetGridLocation(directionX, directionY);

        if (animal.CanEnter(newLoc))
            animal.MoveTo(newLoc);
    }

    public static List<GridLocation> FindPath(Animal a, GridLocation target)
    {
        List<GridLocation> closedSet = new List<GridLocation>();
        List<GridLocation> openSet = new List<GridLocation>();
        openSet.Add(a.loc);
        Dictionary<GridLocation, GridLocation> travelled = new Dictionary<GridLocation, GridLocation>();
        Dictionary<GridLocation, float> gScore = new Dictionary<GridLocation, float>();
        gScore.Add(a.loc, 0f);
        Dictionary<GridLocation, float> fScore = new Dictionary<GridLocation, float>();
        fScore.Add(a.loc, Vector2.Distance(a.loc.pos, target.pos));

        while (openSet.Count > 0)
        {
            float bestScore = Mathf.Infinity;
            GridLocation current = null;

            foreach (GridLocation loc in openSet)
            {

                if (fScore[loc] < bestScore)
                {
                    bestScore = fScore[loc];
                    current = loc;
                }
            }

            if (current == target)
                return ReconstructPath(travelled, current);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (GridLocation loc in GridManager.instance.GetAdjacent(current))
            {
                if (loc == null || closedSet.Contains(loc))
                    continue;

                float tentativeScore = Vector2.Distance(current.pos, loc.pos) + gScore[current];

                if (!gScore.ContainsKey(loc))
                {
                    gScore.Add(loc, tentativeScore);

                    if (travelled.ContainsKey(loc))
                        travelled[loc] = current;
                    else
                        travelled.Add(loc, current);
                }
                    
                else if (gScore[loc] > tentativeScore)
                {
                    gScore[loc] = tentativeScore;

                    if (travelled.ContainsKey(loc))
                        travelled[loc] = current;
                    else
                        travelled.Add(loc, current);
                }
                    

                if (!openSet.Contains(loc))
                    openSet.Add(loc);

                if (fScore.ContainsKey(loc))
                    fScore[loc] = gScore[loc] + Vector2.Distance(target.pos, loc.pos);
                else
                    fScore.Add(loc, gScore[loc] + Vector2.Distance(target.pos, loc.pos));
            }
        }

        return null;
    }

    static List<GridLocation> ReconstructPath(Dictionary<GridLocation, GridLocation> travelled, GridLocation last)
    {
        List<GridLocation> path = new List<GridLocation>();
        path.Add(last);

        GridLocation current = last;

        while (travelled.Keys.ToList().Contains(current))
        {
            current = travelled[current];
            path.Add(current);
        }

        return path;
    }


}
