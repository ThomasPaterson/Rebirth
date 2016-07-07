using UnityEngine;
using System.Collections;

public class PlantUtilityGenerator : UtilityGenerator
{
    private Plant plant;

    void Awake()
    {
        plant = GetComponent<Plant>();
    }


    public override void InteractWith(Animal animal)
    {
        float amountEatten = plant.Eat(animal.species.rateOfEating * animal.species.size);
        animal.currentHunger += amountEatten;
        animal.currentHunger = Mathf.Min(animal.species.hunger, animal.currentHunger);

        if (plant.species.HasTrait(Trait.Type.Poisonous) && !animal.species.HasTrait(Trait.Type.PoisonResistant))
            animal.currentHealth -= amountEatten;

    }
}
