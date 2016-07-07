using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectTraitPanel : MonoBehaviour
{
    public List<Trait> traits;
    public GameObject traitDisplayPrefab;
    public GameObject grid;

    private SpeciesCreationPanel creationPanel;
    private TraitDisplay currentSelected;

    void OnDisable()
    {
        foreach (TraitDisplay display in GetComponentsInChildren<TraitDisplay>())
            Destroy(display.gameObject);
    }


    public void Init(SpeciesCreationPanel creationPanel)
    {
        foreach (TraitDisplay display in GetComponentsInChildren<TraitDisplay>())
            Destroy(display.gameObject);

        this.creationPanel = creationPanel;
        traits = creationPanel.GetPossibleTraits();
        SetupTraits();
    }

    void SetupTraits()
    {

        foreach (Trait t in traits)
        {
            GameObject traitDisplay = Instantiate(traitDisplayPrefab) as GameObject;
            traitDisplay.transform.SetParent(grid.transform, false);
            traitDisplayPrefab.GetComponent<TraitDisplay>().Init(t);
        }
    }

    public void SetCurrentTraitDisplay(TraitDisplay selected)
    {
        if (currentSelected != selected && currentSelected != null)
            currentSelected.Deselect();

        currentSelected = selected;
    }

    public void Confirm()
    {
        if (currentSelected != null)
            creationPanel.currentSpecies.AddTrait(currentSelected.trait);

        creationPanel.DisplaySpecies();
        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }

}
