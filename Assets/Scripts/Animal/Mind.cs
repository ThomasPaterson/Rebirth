using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mind : MonoBehaviour
{
    public List<UtilityGenerator.Desire> priorites;
    public int range;
    public UtilityGenerator target;
    public bool threatenedBy;
    public UtilityGenerator.Desire currentDesire;

    private Animal animal;

    void Awake()
    {
        animal = GetComponent<Animal>();
    }

    public void AddPreferenceForTree()
    {
        if (!priorites.Contains(UtilityGenerator.Desire.EatTree))
        {
            int bushIndex = priorites.IndexOf(UtilityGenerator.Desire.EatBush);
            int grassIndex = priorites.IndexOf(UtilityGenerator.Desire.EatGrass);

            int indexToUse = Mathf.Max(bushIndex, grassIndex);

           // if (indexToUse)
        }
    }
  
    public void Think()
    {
        List<UtilityGenerator> potential = new List<UtilityGenerator>();

        foreach (GridLocation loc in GridManager.instance.GetBox(GetComponent<IGridOccupier>().loc, range))
        {
            if (loc == null)
                continue;

            foreach (IGridOccupier occupier in loc.occupiers)
                potential.Add(occupier.GetGenerator());
               
        }

        target = DetermineBestTarget(potential);

        if (target is AnimalUtilityGenerator && animal.species == target.GetComponent<Animal>().species)
            target.GetComponent<Mind>().target = GetComponent<AnimalUtilityGenerator>();


    }

    UtilityGenerator DetermineBestTarget(List<UtilityGenerator> potential)
    {
        List<UtilityGenerator> matching = new List<UtilityGenerator>();

        foreach (UtilityGenerator.Desire desire in priorites)
        {
            currentDesire = desire;

            if (CheckSkipDesire(desire))
                continue;

            if (desire == UtilityGenerator.Desire.Mate && !GetComponent<Animal>().CanBreed())
                continue;

            //return the current target over anything else matched
            if (target != null && target.CheckSatisfies(desire, animal) && potential.Contains(target))
                return target;

            foreach (UtilityGenerator util in potential)
            { 
                if (util == null || util.gameObject == gameObject)
                    continue;

                if (util.CheckSatisfies(desire, animal))
                    matching.Add(util);
            }

            if (matching.Count > 0)
                return matching[Random.Range(0, matching.Count)];

            matching = new List<UtilityGenerator>();
        }

        return null;
    }

    bool CheckSkipDesire(UtilityGenerator.Desire desire)
    {
        switch (desire)
        {
            case UtilityGenerator.Desire.EatBush:
                return animal.currentHunger > animal.species.hunger * 0.9f;
            case UtilityGenerator.Desire.EatGrass:
                return animal.currentHunger > animal.species.hunger * 0.9f;
            case UtilityGenerator.Desire.EatMeat:
                return animal.currentHunger > animal.species.hunger * 0.9f;
            case UtilityGenerator.Desire.EatTree:
                return animal.currentHunger > animal.species.hunger * 0.9f;
            case UtilityGenerator.Desire.Hide:
                return animal.IsSafe();
            case UtilityGenerator.Desire.RunAway:
                return animal.IsSafe();
            default:
                return false;
        }
    }



}

