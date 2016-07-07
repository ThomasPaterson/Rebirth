using UnityEngine;
using System.Collections;
using System;

public class MeatUtilityGenerator : UtilityGenerator, IGridOccupier
{
    public float timeToLivePerSize = 2f;
    public GridLocation loc { get; set; }

    private AnimalSpecies species;
    private float timeLeft;
    private float foodLeft;

    public void Init(GridLocation newLoc, AnimalSpecies species)
    {
        this.species = species;
        timeLeft = species.size * timeToLivePerSize;
        foodLeft = species.size;
        MoveTo(newLoc);

        foreach (GridLocation loc in GridManager.instance.GetBox(newLoc, 1))
        {
            foreach (IGridOccupier occupier in loc.occupiers)
            {
                GameObject occ = occupier.GetGameObject();

                if (occ.GetComponent<Animal>() != null && occ.GetComponent<Mind>().currentDesire == Desire.EatMeat)
                    occ.GetComponent<Mind>().target = this;
            }
        }  
                
    }

    public bool CanEnter(GridLocation newLoc)
    {
        return true;
    }

    public UtilityGenerator GetGenerator()
    {
        return this;
    }

    public Species GetSpecies()
    {
        return null;
    }

    public Vector3 MoveLocation(GridLocation newLoc)
    {
        return newLoc.GetPosition();
    }

    public void MoveTo(GridLocation newLoc, bool animate = false)
    {
        transform.position = MoveLocation(newLoc);
        newLoc.Occupy(this);
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft < 0 || foodLeft < 0)
        {
            loc.Leave(this);
            Destroy(gameObject);
        }
    }

    public override void InteractWith(Animal other)
    {
        float amountEaten = Mathf.Min(other.species.rateOfEating * other.species.size, foodLeft);
        foodLeft -= amountEaten;

        other.currentHunger += amountEaten;

        if (species.HasTrait(Trait.Type.Poisonous) && !other.species.HasTrait(Trait.Type.PoisonResistant))
            other.currentHealth -= amountEaten;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

}
