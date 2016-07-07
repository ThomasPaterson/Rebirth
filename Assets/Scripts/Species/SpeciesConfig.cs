using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SpeciesConfig : MonoBehaviour
{
    public abstract List<GameObject> GetPrefabs();

    public abstract Species GetSpecies(GameObject prefab);

    public abstract List<Trait> GetTraits(Species species);


}
