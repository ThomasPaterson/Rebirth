using UnityEngine;
using System.Collections;

public class AnimalUtilityGenerator : UtilityGenerator
{
    private Animal animal;

    void Awake()
    {
        animal = GetComponent<Animal>();
    }

    public override bool CheckSpecific(Desire desire, Animal checker)
    {
        if (desire == Desire.Mate)
            return animal.CanBreed();
        else if (desire == Desire.RunAway || desire == Desire.RunAway)
            return IsThreat(animal, checker);
        else if (desire == Desire.EatMeat)
            return IsThreat(checker, animal);
        else
            return true;
    }


    public override void InteractWith(Animal other)
    {
        if (other.species == animal.species && other.CanBreed() && animal.CanBreed())
        {
            animal.GiveBirth();
            other.Impregnate();
        }
        else if (other.species != animal.species && other.species.type == AnimalSpecies.Type.Carnivore)
        {
            if (other.species.HasTrait(Trait.Type.Claws))
                animal.currentHealth = 0f;
            else
                animal.currentHealth -= other.species.size * other.species.rateOfEating;
        }
    }

    bool IsThreat(Animal attacker, Animal defender)
    {
        if (attacker.species == defender.species)
            return false;

        if (defender.species.HasTrait(Trait.Type.Vicious))
            return false;

        if (attacker.species.HasTrait(Trait.Type.Vicious))
            return true;


        return attacker.species.GetCombatThreat() > defender.species.GetCombatThreat() * AnimalSpeciesConfig.instance.defenderThreatMod;
    }
}
