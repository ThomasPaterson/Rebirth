using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpeciesCreationPanel : MonoBehaviour
{
    public Species currentSpecies;
    public Image display;
    public Button floraButton;
    public Button faunaButton;
    public List<GameObject> targetPrefabs;
    public GameObject targetPrefab;
    public GameObject traitDisplayPanel;

    private SpeciesConfig currentConfig;
    private int floraIndex;
    private int faunaIndex;
    private bool initialized;

    void Start()
    {
        initialized = true;
        SetToFlora();
    }


    void OnEnable()
    {
        if (initialized)
            Setup(); 
    }

    void Setup()
    {
        targetPrefab = targetPrefabs[currentConfig is PlantSpeciesConfig ? floraIndex : faunaIndex];
        currentSpecies = targetPrefab.GetComponent<DefaultSpecies>().GetDefaultSpecies();
        currentSpecies.traits = new List<Trait>();
        DisplaySpecies();
    }

    public void DisplaySpecies()
    {
        SpeciesStatPanel[] statPanels = GetComponentsInChildren<SpeciesStatPanel>(true);

        for (int i = 0; i < statPanels.Length; i++)
            statPanels[i].DisplaySpecies(currentSpecies, i);

        TraitButton[] traitButtons = GetComponentsInChildren<TraitButton>(true);

        for (int i = 0; i < traitButtons.Length; i++)
            traitButtons[i].DisplaySpecies(currentSpecies, i);

        if (targetPrefab == null)
            display.sprite = null;
        else
            display.sprite = targetPrefab.GetComponentInChildren<ISpeciesInstance>().GetIcon();
    }

    public List<Trait> GetPossibleTraits()
    {
        return currentConfig.GetTraits(currentSpecies);
    }

    public void ClickTrait(int index)
    {
        if (currentSpecies.GetTrait(index) == null)
        {
            traitDisplayPanel.SetActive(true);
            traitDisplayPanel.GetComponent<SelectTraitPanel>().Init(this);
        }
        else
        { 
            currentSpecies.RemoveTrait(index);
            DisplaySpecies();
        }
            

    }

    public void ConfirmCreateSpecies()
    {
        currentSpecies.prefab = targetPrefab;

        if (currentSpecies is PlantSpecies)
            SpeciesManager.instance.CreateInitialSpecies(new PlantSpecies((PlantSpecies)currentSpecies));
        else
            SpeciesManager.instance.CreateInitialSpecies(new AnimalSpecies((AnimalSpecies)currentSpecies));

        currentSpecies = null;
        gameObject.SetActive(false);
    }

    public void ChangePrefab(int direction)
    {
        if (currentConfig is PlantSpeciesConfig)
        {
            floraIndex += direction;

            if (floraIndex < 0)
                floraIndex = targetPrefabs.Count - 1;
            else if (floraIndex >= targetPrefabs.Count)
                floraIndex = 0;
        }
        else
        {
            faunaIndex += direction;

            if (faunaIndex < 0)
                faunaIndex = targetPrefabs.Count - 1;
            else if (faunaIndex >= targetPrefabs.Count)
                faunaIndex = 0;
        }

        Setup();
    }

    public void SetToFlora()
    {
        currentConfig = PlantSpeciesConfig.instance;
        targetPrefabs = currentConfig.GetPrefabs();
        Setup();
        floraButton.GetComponent<Button>().interactable = false;
        faunaButton.GetComponent<Button>().interactable = true;
    }

    public void SetToFauna()
    {
        currentConfig = AnimalSpeciesConfig.instance;
        targetPrefabs = currentConfig.GetPrefabs();
        Setup();
        floraButton.GetComponent<Button>().interactable = true;
        faunaButton.GetComponent<Button>().interactable = false;
    }
}
