using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpeciesCreationUI : MonoBehaviour, IPointerClickHandler
{
    public Image display;
    public int numToSpawn;
    public GameObject[] toDisable;
    public AudioClip successSound;
    public AudioClip failSound;

    private int numLeft;
    private Species currentSpecies;

    void OnEnable()
    {
        foreach (GameObject obj in toDisable)
            obj.SetActive(false);
    }

    void OnDisable()
    {
        foreach (GameObject obj in toDisable)
            obj.SetActive(true);
    }

    public void SetSpecies(Species species)
    {
        this.currentSpecies = species;
        numLeft = numToSpawn;
        display.sprite = species.GetIcon();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.pointerCurrentRaycast.screenPosition);

        RaycastHit hit;

       if ( Physics.Raycast(ray, out hit))
        {
            GridLocation locHit = GridManager.instance.GetLocFromWorld(hit.point);

            if (locHit != null && currentSpecies.CanEnter(locHit))
            {

                SpeciesManager.instance.AddSpeciesMember(currentSpecies, locHit);
                numLeft--;
                AudioSource.PlayClipAtPoint(successSound, Camera.main.transform.position);
            }
            else
                AudioSource.PlayClipAtPoint(failSound, Camera.main.transform.position);
        }

        

    }

}
