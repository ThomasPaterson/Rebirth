using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Animal : MonoBehaviour, ISpeciesInstance, IGridOccupier
{
    public Sprite icon;

    public GridLocation loc { get; set; }
    public AnimalSpecies species { get; set; }
    public float currentHealth;
    public float currentHunger;
    public float lifetime;

    private Mind mind;
    private bool dead;

    void Awake()
    {
        mind = GetComponent<Mind>();
    }

    public bool CanEnter(GridLocation newLoc)
    {
        return species.CanEnter(newLoc);
    }

    public bool CompareType(ISpeciesInstance otherInst)
    {
        return otherInst.GetSpecies() == species;
    }

    public UtilityGenerator GetGenerator()
    {
        if (!dead)
            return GetComponent<UtilityGenerator>();
        else
            return null;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public Species GetSpecies()
    {
        return species;
    }

    public void HandleInstance()
    {
        currentHunger -= species.rateOfStarving * species.size;
        lifetime -= SpeciesManager.instance.animalProcTime;

        if (currentHunger <= species.hunger * -1f || lifetime <= 0 || currentHealth <= 0)
            Die();

        if (!dead)
        {
            if (CanAct())
            {
                Act();

                if (UnityEngine.Random.value < 0.2f)
                    mind.Think();
            }       
            else
            {
                mind.Think();
                GetComponent<Pathfinder>().SetCanSeek();
            }
        }
        
        
        
    }

    public void Init(Species species)
    {
        this.species = (AnimalSpecies)species;
        this.currentHealth = this.species.size;
        this.currentHunger = 0f;
        this.lifetime = AnimalSpeciesConfig.instance.lifeTimeCurve.Evaluate(this.species.size);
        GetComponent<Pathfinder>().walkSpeed = this.species.speed;

        if ((species.HasTrait(Trait.Type.Climbing) || species.HasTrait(Trait.Type.LongNecked)) && this.species.type != AnimalSpecies.Type.Carnivore)
            GetComponent<Mind>().AddPreferenceForTree();

        if (species.HasTrait(Trait.Type.Huge))
        {
            Transform t = GetComponentInChildren<Renderer>().transform;
            t.localScale = t.localScale * 1.2f;
        }

        if (species.HasTrait(Trait.Type.Tiny))
        {
            Transform t = GetComponentInChildren<Renderer>().transform;
            t.localScale = t.localScale * 0.8f;
        }
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

    bool CanAct()
    {
        return (mind.target != null && mind.target.GetLocation() == loc);        
    }

    void Act()
    {
        mind.target.InteractWith(this);
    }

    void Die()
    {
        GameObject meat = Instantiate(AnimalSpeciesConfig.instance.meatCubePrefab) as GameObject;
        meat.GetComponent<MeatUtilityGenerator>().Init(loc, species);

        AudioManager.instance.PlaySound(AudioManager.instance.dieSound, gameObject);

        dead = true;
        loc.Leave(this);
        species.RemoveInstance(this);
        Destroy(gameObject);

        
    }

    public bool CanBreed()
    {
        return currentHunger >= species.hunger * AnimalSpeciesConfig.instance.birthPercent;
    }

    public void GiveBirth()
    {
        List<GridLocation> potential = GridManager.instance.GetBox(loc, 2);

        for (int i = 0; i < Mathf.Min(species.numChildren, potential.Count); i++)
        {
            int toUse = UnityEngine.Random.Range(0, potential.Count);

            if (CanEnter(potential[toUse]))
            {
                SpeciesManager.instance.AddSpeciesMember(species, potential[toUse]);
                potential.RemoveAt(toUse);
            }

        }
        mind.target = null;
        currentHunger = 0f;

        if (!SpeciesManager.playingMating)
        {
            AudioManager.instance.PlaySound(AudioManager.instance.matingSound, gameObject);
            SpeciesManager.playingMating = true;
        }
    }

    public void Impregnate()
    {
        mind.target = null;
        currentHunger = 0f;
    }

    public float GetRunSpeed()
    {
        return 1.6f * (species.HasTrait(Trait.Type.LongLegs) ? 0.5f : 1f);
    }

    public float GetChaseSpeed()
    {
        return 0.4f * (species.HasTrait(Trait.Type.LongLegs) ? 0.5f : 1f);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public bool IsSafe()
    {
        if (species.HasTrait(Trait.Type.Climbing) && loc.HasTree())
            return true;
        else
            return false;
    }
}
