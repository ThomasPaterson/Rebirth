using UnityEngine;
using System.Collections;

[System.Serializable]
public class AnimalSpecies : Species
{
    public enum Type { SmallHerbivore, LargeHerbivore, Carnivore}

    public Type type;

    public int numChildren;
    public float hunger;
    public float speed;
    public int size;
    public int sight;
    public float rateOfEating;
    public float rateOfStarving;

    public AnimalSpecies(AnimalSpecies toClone)
    {
        prefab = toClone.prefab;
        traits = new System.Collections.Generic.List<Trait>(toClone.traits);
        size = toClone.size;
        numChildren = toClone.numChildren;
        type = toClone.type;
        hunger = toClone.hunger;
        speed = toClone.speed;
        size = toClone.size;
        sight = toClone.sight;
        rateOfEating = toClone.rateOfEating;
        rateOfStarving = toClone.rateOfStarving;
    }

    public override bool CanEnter(GridLocation newLoc)
    {
        return !newLoc.IsWater();
    }

    public float GetCombatThreat()
    {
        return (float)size;
    }

    protected override void ProcessTrait(Trait t, float direction)
    {
        switch (t.effect)
        {
            case Trait.Type.Fertile:
                numChildren += 1 * (int)direction;
                break;
            case Trait.Type.SlowEater:
                rateOfEating -= 0.1f * direction;
                break;
            case Trait.Type.SlowMetabolism:

                if (Mathf.Sign(direction) == 1f)
                    rateOfStarving /= 2f;
                else
                    rateOfStarving *= 2f;
                break;
            case Trait.Type.Huge:
                size += 1 * (int)direction;
                break;
            case Trait.Type.Tiny:
                size -= 1 * (int)direction;
                break;


        }
    }

}
