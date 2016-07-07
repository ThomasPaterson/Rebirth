using UnityEngine;
using System.Collections;

public class SpeciesButton : MonoBehaviour
{
    public PlantSpecies species;
    public int numToAdd = 30;

    public void AddSpecies()
    {

        int numLeft = numToAdd;
        int failures = 0;

        while (numLeft > 0 && failures < 30)
        {
            int x = Random.Range(0, TerrainManager.instance.gridDiameter);
            int y = Random.Range(0, TerrainManager.instance.gridDiameter);

            GridLocation loc = GridManager.instance.GetGridLocation(x, y);

            if (!loc.IsWater())
            {

                if (species.CanEnter(loc))
                {
                    SpeciesManager.instance.AddSpeciesMember(species, loc);
                    numLeft--;
                }
                else
                    failures++;
            }
        }
        
    }

}
