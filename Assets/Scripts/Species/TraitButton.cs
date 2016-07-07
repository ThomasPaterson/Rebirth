using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TraitButton : MonoBehaviour
{
    public Text nameText;
    public Text buttonText;
    

    private Trait trait;

    public void DisplaySpecies(Species species, int index)
    {
        trait = species.GetTrait(index);
        DisplayTrait();
    }


    void DisplayTrait()
    {
        if (trait == null)
        {
            nameText.text = "Add New Trait";
            buttonText.text = "+";
        }
        else
        {
            nameText.text = trait.traitName;
            buttonText.text = "-";
        }
    }
	
}
