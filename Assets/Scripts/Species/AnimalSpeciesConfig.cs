using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalSpeciesConfig : SpeciesConfig
{
    public static AnimalSpeciesConfig instance;

    public AnimalSpecies smallHerbivoreSpecies;
    public List<GameObject> smallHerbivorePrefabs;
    public List<Trait> smallHerbivoreTraits;
    public AnimalSpecies largeHerbivoreSpecies;
    public List<GameObject> largeHerbivorePrefabs;
    public List<Trait> largeHerbivoreTraits;
    public AnimalSpecies carnivoreSpecies;
    public List<GameObject> carnivorePrefabs;
    public List<Trait> carnivoreTraits;

    public List<Trait> animalTraits;
    public AnimationCurve animalReproductionCurve;

    public List<GameObject> prefabs;
    public float birthPercent;
    public AnimationCurve lifeTimeCurve;
    public float defenderThreatMod = 0.8f;
    public GameObject meatCubePrefab;
    public float uiStarving = 100f;
    public float uiEating = 30f;


    void Awake()
    {
        instance = this;
        prefabs = new List<GameObject>();
        prefabs.AddRange(smallHerbivorePrefabs);
        prefabs.AddRange(largeHerbivorePrefabs);
        prefabs.AddRange(carnivorePrefabs);
    }

    public override List<GameObject> GetPrefabs()
    {
            return prefabs;
    }

    public override Species GetSpecies(GameObject prefab)
    {
        if (prefab == null)
            return null;

        if (smallHerbivorePrefabs.Contains(prefab))
            return smallHerbivoreSpecies;
        else if (largeHerbivorePrefabs.Contains(prefab))
            return largeHerbivoreSpecies;
        else if (carnivorePrefabs.Contains(prefab))
            return carnivoreSpecies;
        else
            return null;
    }

    public override List<Trait> GetTraits(Species species)
    {
        List<Trait> traits = new List<Trait>(animalTraits);

        if (((AnimalSpecies)species).type == AnimalSpecies.Type.SmallHerbivore)
            traits.AddRange(smallHerbivoreTraits);
        else if (((AnimalSpecies)species).type == AnimalSpecies.Type.LargeHerbivore)
            traits.AddRange(largeHerbivoreTraits);
        else if (((AnimalSpecies)species).type == AnimalSpecies.Type.Carnivore)
            traits.AddRange(carnivoreTraits);

        foreach (Trait t in species.traits)
            traits.Remove(t);

        return traits;
    }


}
