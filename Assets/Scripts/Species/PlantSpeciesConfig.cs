using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlantSpeciesConfig : SpeciesConfig
{
    public static PlantSpeciesConfig instance;

    public PlantSpecies baseGrassSpecies;
    public List<GameObject> grassPrefabs;
    public List<Trait> grassTraits;
    public PlantSpecies baseBushSpecies;
    public List<GameObject> bushPrefabs;
    public List<Trait> bushTraits;
    public PlantSpecies baseTreeSpecies;
    public List<GameObject> treePrefabs;
    public List<Trait> treeTraits;

    public List<Trait> plantTraits;
    public AnimationCurve plantReproductionCurve;
    public AnimationCurve heightDisplayCurve;

    public List<GameObject> prefabs;


    void Awake()
    {
        instance = this;
        prefabs = new List<GameObject>();
        prefabs.AddRange(grassPrefabs);
        prefabs.AddRange(bushPrefabs);
        prefabs.AddRange(treePrefabs);
    }

    public override List<GameObject> GetPrefabs()
    {
            return prefabs;
    }

    public override Species GetSpecies(GameObject prefab)
    {
        if (prefab == null)
            return null;

        if (grassPrefabs.Contains(prefab))
            return baseGrassSpecies;
        else if (bushPrefabs.Contains(prefab))
            return baseBushSpecies;
        else if (treePrefabs.Contains(prefab))
            return baseTreeSpecies;
        else
            return null;
    }

    public override List<Trait> GetTraits(Species species)
    {
        List<Trait> traits = new List<Trait>(plantTraits);

        if (((PlantSpecies)species).type == PlantSpecies.Type.Bush)
            traits.AddRange(bushTraits);
        else if (((PlantSpecies)species).type == PlantSpecies.Type.Grass)
            traits.AddRange(grassTraits);
        else if (((PlantSpecies)species).type == PlantSpecies.Type.Tree)
            traits.AddRange(treeTraits);

        foreach (Trait t in species.traits)
            traits.Remove(t);

        return traits;
    }


}
