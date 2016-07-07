using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeciesStatPanel : MonoBehaviour
{
    public Text statName;
    public GameObject[] statPoints;

    public void DisplaySpecies(Species species, int num)
    {
        if (species is PlantSpecies)
            DisplayPlant((PlantSpecies)species, num);
        else
            DisplayAnimal((AnimalSpecies)species, num);
    }

    void DisplayPlant(PlantSpecies species, int num)
    {
        switch (num)
        {
            case 0:
                statName.text = "Size";
                DisplayStatPoints((int)species.size);
                break;
            case 1:
                statName.text = "Reproduction";
                DisplayStatPoints(species.GetReproduction());
                break;
            case 2:
                statName.text = "Req. Soil Quality";
                DisplayStatPoints(Mathf.RoundToInt(statPoints.Length * species.minSoilQuality));
                break;
            case 3:
                statName.text = "Req. Moisture";
                DisplayStatPoints(Mathf.RoundToInt(statPoints.Length * species.minMoistureQuality));
                break;
            case 4:
                statName.text = "Max Moisture";
                DisplayStatPoints(Mathf.RoundToInt(statPoints.Length * species.maxMoistureQuality));
                break;
            case 5:
                statName.text = "Max Altitude";
                DisplayStatPoints(Mathf.RoundToInt(PlantSpeciesConfig.instance.heightDisplayCurve.Evaluate(species.maxHeightQuality)));
                break;

        }
    }

    void DisplayAnimal(AnimalSpecies species, int num)
    {
        switch (num)
        {
            case 0:
                statName.text = "Size";
                DisplayStatPoints((int)species.size);
                break;
            case 1:
                statName.text = "Reproduction";
                DisplayStatPoints(species.numChildren * 3);
                break;
            case 2:
                statName.text = "Hunger";
                DisplayStatPoints(Mathf.RoundToInt(species.rateOfStarving * AnimalSpeciesConfig.instance.uiStarving));
                break;
            case 3:
                statName.text = "Sight";
                DisplayStatPoints(species.sight);
                break;
            case 4:
                statName.text = "Eat Speed";
                DisplayStatPoints(Mathf.RoundToInt(species.rateOfEating * AnimalSpeciesConfig.instance.uiEating));
                break;
            case 5:
                statName.text = "";
                DisplayStatPoints(0);
                break;

        }
    }


    void DisplayStatPoints(int numToDisplay)
    {
        for (int i = 0; i < statPoints.Length; i++)
        {
            if (i < numToDisplay)
                statPoints[i].SetActive(true);
            else
                statPoints[i].SetActive(false);
        }
    }
}
