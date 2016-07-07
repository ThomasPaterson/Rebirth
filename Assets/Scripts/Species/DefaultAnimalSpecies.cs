using UnityEngine;
using System.Collections;

public class DefaultAnimalSpecies : DefaultSpecies
{
    public AnimalSpecies animalSpecies;

    public override Species GetDefaultSpecies()
    {
        return new AnimalSpecies(animalSpecies);
    }

}
