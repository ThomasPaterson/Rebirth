using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TraitDisplay : MonoBehaviour, IPointerClickHandler
{
    public Text traitName;
    public Text traitDescription;
    public Image selectedDisplay;

    public Trait trait;

    public void Init(Trait trait)
    {
        this.trait = trait;
        traitName.text = trait.traitName;
        traitDescription.text = trait.description;
        selectedDisplay.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Select();
    }

    public void Select()
    {
        GetComponentInParent<SelectTraitPanel>().SetCurrentTraitDisplay(this);
        selectedDisplay.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        selectedDisplay.gameObject.SetActive(false);
    }

}
