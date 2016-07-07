using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UtilityGenerator : MonoBehaviour
{
    public enum Desire { EatGrass = 0, EatBush = 1, EatTree = 2, EatMeat = 3, Mate = 4, RunAway = 5, Hide = 6}

    public List<Desire> canSatisfy = new List<Desire>();

    public bool CheckSatisfies(Desire desire, Animal checker)
    {
        if (!canSatisfy.Contains(desire))
            return false;
        else
            return CheckSpecific(desire, checker);
    }

    public virtual bool CheckSpecific(Desire desire, Animal checker)
    {
        return true;
    }

    public GridLocation GetLocation()
    {
        return GetComponent<IGridOccupier>().loc;
    }

    public virtual void InteractWith(Animal animal)
    {

    }

}
