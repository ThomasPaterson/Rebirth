using UnityEngine;
using System.Collections;

public class DefaultPlantSpecies : DefaultSpecies
{
    public PlantSpecies plantSpecies;

    public override Species GetDefaultSpecies()
    {
        return new PlantSpecies(plantSpecies);
    }
}
