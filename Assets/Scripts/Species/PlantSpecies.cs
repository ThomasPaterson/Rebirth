using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlantSpecies : Species
{
    public enum Type { Grass = 0, Bush = 1, Tree = 2}

    public float size;
    public float foodGrowthRate;
    public float minSoilQuality;
    public float minMoistureQuality;
    public float maxMoistureQuality;
    public float maxHeightQuality;
    public int spreadDistance;
    public int minDistance = 1;
    public float startingFood;
    public int numSeeds;

    public Type type;

    public PlantSpecies(PlantSpecies toClone)
    {
        prefab = toClone.prefab;
        traits = new System.Collections.Generic.List<Trait>(toClone.traits);
        size = toClone.size;
        foodGrowthRate = toClone.foodGrowthRate;
        minSoilQuality = toClone.minSoilQuality;
        minMoistureQuality = toClone.minMoistureQuality;
        maxMoistureQuality = toClone.maxMoistureQuality;
        maxHeightQuality = toClone.maxHeightQuality;
        spreadDistance = toClone.spreadDistance;
        minDistance = toClone.minDistance;
        startingFood = toClone.startingFood;
        numSeeds = toClone.numSeeds;
        type = toClone.type;
    }

    public static bool LowerPriority(PlantSpecies checking, PlantSpecies competitor)
    {
        if (checking == competitor)
            return true;

        if (checking.HasTrait(Trait.Type.Weed))
            return false;

        if ((int)checking.type < (int)competitor.type)
            return true;

        if ((int)checking.type == (int)competitor.type)
        {
            float checkingScore = 0;

            if (checking.minSoilQuality > competitor.minSoilQuality)
                checkingScore++;
            else if (checking.minSoilQuality < competitor.minSoilQuality)
                checkingScore--;

            if (checking.minMoistureQuality > competitor.minMoistureQuality)
                checkingScore++;
            else if (checking.minMoistureQuality < competitor.minMoistureQuality)
                checkingScore--;

            if (checking.maxHeightQuality < competitor.maxHeightQuality)
                checkingScore++;
            else if (checking.maxHeightQuality > competitor.maxHeightQuality)
                checkingScore--;

            if (checkingScore <= 0)
                return true;
        }

        return false;
    }

    public override bool CanEnter(GridLocation newLoc)
    {
        if (newLoc.IsWater())
            return false;
        if (newLoc.GetSoilQuality() < minSoilQuality)
            return false;
        else if (newLoc.GetMoisture() < minMoistureQuality)
            return false;
        else if (newLoc.GetHeight() > TerrainManager.instance.maxHeight + maxHeightQuality)
            return false;

        foreach (IGridOccupier occupier in newLoc.occupiers)
            if (occupier.GetSpecies() is PlantSpecies)
                if (PlantSpecies.LowerPriority(this, (PlantSpecies)occupier.GetSpecies()))
                    return false;

        foreach (GridLocation loc in GridManager.instance.GetBox(newLoc, minDistance))
        {
            if (loc == newLoc)
                continue;

            foreach (IGridOccupier occupier in loc.occupiers)
                if (occupier.GetSpecies() is PlantSpecies)
                    if (((PlantSpecies)occupier.GetSpecies()).type == type || PlantSpecies.LowerPriority(this, (PlantSpecies)occupier.GetSpecies()))
                        return false;
        }

        return true;
    }

    public int GetReproduction()
    {
        float time = size / foodGrowthRate;
        float score = PlantSpeciesConfig.instance.plantReproductionCurve.Evaluate(time);
        return Mathf.FloorToInt(score);

    }

    protected override void ProcessTrait(Trait t, float direction)
    {
        switch (t.effect)
        {
            case Trait.Type.ExtraAltitude:
                maxHeightQuality += 2 * direction;
                break;
            case Trait.Type.ExtraArid:
                minMoistureQuality -= 0.2f * direction;
                break;
            case Trait.Type.ExtraFood:
                foodGrowthRate += 0.2f * direction;
                break;
            case Trait.Type.ExtraWater:
                minMoistureQuality += 0.2f * direction;
                maxMoistureQuality += 0.2f * direction;
                break;
            case Trait.Type.Travelling:
                spreadDistance += 2 * (int)direction;
                break;
            case Trait.Type.Hardy:
                minSoilQuality -= 0.2f * direction;
                break;

        }
    }

}
