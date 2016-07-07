using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour
{
    public string sceneToLoad;

	public void Load()
    {
        Application.LoadLevel(sceneToLoad);
    }
}
