using UnityEngine;
using System.Collections;

public class LoadLevelAdditively : MonoBehaviour
{

    public float timeToWait = 4f;
    public string sceneName;

	// Use this for initialization
	IEnumerator Start ()
    {

        while (timeToWait > 0f)
            yield return null;

        Application.LoadLevel(sceneName);

    }

    void Update()
    {
        timeToWait -= Time.deltaTime;
    }
	
}
