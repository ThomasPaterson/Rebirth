using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeciesManager : MonoBehaviour
{
    public static bool playingSpreadSeed = false;
    public static bool playingMating = false;

    public static SpeciesManager instance;

    public List<Species> animals;
    public List<Species> vegetation;

    public GameObject plantPrefab;
    public GameObject animalPrefab;
    public GameObject creationUI;

    public float vegetationProcTime = 1f;
    public float animalProcTime = 0.5f;

    void Awake()
    {
        instance = this;

        animals = new List<Species>();
        vegetation = new List<Species>();
    }

    void LateUpdate()
    {
        playingSpreadSeed = false;
        playingMating = false;
    }

    IEnumerator Start()
    {
        while (GridManager.instance == null)
            yield return null;

        StartCoroutine(HandleVegetation());
        StartCoroutine(HandleAnimal());
    }

    public void AddVegetation(Species species)
    {
        vegetation.Add(species);
    }

    public void AddAnimal(Species species)
    {
        animals.Add(species);
    }

    public GameObject AddSpeciesMember(Species species, GridLocation loc)
    {
        if (species is PlantSpecies && !vegetation.Contains(species))
                AddVegetation(species);

        if (species is AnimalSpecies && !animals.Contains(species))
            AddAnimal(species);

        GameObject speciesMember = (GameObject)Instantiate(species.prefab);
        species.AddMember(speciesMember.GetComponent<ISpeciesInstance>());
        speciesMember.GetComponent<ISpeciesInstance>().Init(species);
        speciesMember.GetComponent<IGridOccupier>().MoveTo(loc);
        return speciesMember;
    }

    IEnumerator HandleVegetation()
    {
        while (true)
        {
            float timeLeft = vegetationProcTime;

            while (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                yield return null;
            }

            foreach (Species species in vegetation)
                species.ProcessSpecies();
        }
    }

    IEnumerator HandleAnimal()
    {
        while (true)
        {
            float timeLeft = animalProcTime;

            while (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                yield return null;
            }

            foreach (Species species in animals)
                species.ProcessSpecies();
        }
    }

    public void CreateInitialSpecies(Species newSpecies)
    {
        creationUI.SetActive(true);
        creationUI.GetComponent<SpeciesCreationUI>().SetSpecies(newSpecies);
    }

}
