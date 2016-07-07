using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Plant : MonoBehaviour, IGridOccupier, ISpeciesInstance
{
    public Texture lowFood;
    public Texture medFood;
    public Texture highFood;
    public Sprite icon;

    public GridLocation loc { get;  set; }
    public PlantSpecies species { get; set; }

    public float currentFood = 0f;
    private List<GameObject> children = new List<GameObject>();
    private bool dead;

    public bool CanEnter(GridLocation newLoc)
    {
        return species.CanEnter(newLoc);
    }

    public void Init(Species species)
    {
        this.species = (PlantSpecies)species;
        currentFood = this.species.startingFood;
        SetAppearance();
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

    public void HandleInstance()
    {
        for (int i = children.Count - 1; i >= 0; i--)
            if (children[i] == null)
                children.RemoveAt(i);

        currentFood = Mathf.Min(currentFood + species.foodGrowthRate * UnityEngine.Random.Range(0.5f, 1.5f), species.size);

        if (currentFood == species.size && children.Count < species.numSeeds)
            SpreadSeeds();

        SetAppearance();
    }

    void SpreadSeeds()
    {
        if (species.HasTrait(Trait.Type.Weed))
        {
            foreach (GridLocation nextTo in GridManager.instance.GetAdjacent(loc))
            {
                foreach (IGridOccupier occ in nextTo.occupiers)
                {
                    if (occ.GetSpecies() is PlantSpecies && occ.GetSpecies() != species)
                    {
                        occ.GetGameObject().GetComponent<Plant>().currentFood -= species.size * 2f;
                    }
                }
            }
        }

        List<GridLocation> potential = GridManager.instance.GetBox(loc, species.spreadDistance);
        bool success = false;

        for (int i = 0; i < Mathf.Min(species.numSeeds, potential.Count); i++)
        {
            int toUse = UnityEngine.Random.Range(0, potential.Count);

            if (CanEnter(potential[toUse]))
            {
                children.Add(SpeciesManager.instance.AddSpeciesMember(species, potential[toUse]));
                potential.RemoveAt(toUse);
                success = true;

                if (!SpeciesManager.playingSpreadSeed)
                {
                    AudioManager.instance.PlaySound(AudioManager.instance.sproutSounds, gameObject);
                    SpeciesManager.playingSpreadSeed = true;
                }
                   
            }
            
        }

        if (success)
            currentFood = species.size * 0.5f;
    }

    void SetAppearance()
    {
        if (currentFood < 0.2f && lowFood != null)
            GetComponentInChildren<Renderer>().material.mainTexture = lowFood;
        else if (currentFood < 0.5f && medFood != null)
            GetComponentInChildren<Renderer>().material.mainTexture = medFood;
        else if (currentFood >= 0.5f && highFood != null)
            GetComponentInChildren<Renderer>().material.mainTexture = highFood;
    }

    public Species GetSpecies()
    {
        return species;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public bool CompareType(ISpeciesInstance otherInst)
    {
        if (otherInst.GetSpecies() is PlantSpecies)
            if (((PlantSpecies)otherInst.GetSpecies()).type == species.type)
                return true;

        return false;
        
    }

    public UtilityGenerator GetGenerator()
    {
        if (!dead)
            return GetComponent<UtilityGenerator>();
        else
            return null;
    }

    public float Eat(float amount)
    {
        float amountEaten = Mathf.Min(amount, currentFood);

        currentFood -= amount;


        if (currentFood < 0f)
            Die();
        else
            SetAppearance();

        return amountEaten;
    }

    public void Die()
    {
        dead = true;
        loc.Leave(this);
        species.RemoveInstance(this);
        Destroy(gameObject);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}

