using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip matingSound;
    public AudioClip dieSound;
    public AudioClip spawnSound;
    public AudioClip[] sproutSounds;
    public AudioClip biteSound;
    public float cameraCutoff;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        

    }

    public void PlaySound(AudioClip clip, GameObject obj)
    {
        if (clip == null)
            return;

        if (CameraControls.instance.currentZoom > cameraCutoff)
            return;

        Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position);

        if (pos.x < 0f || pos.y < 0f || pos.x > 1f || pos.y > 1f)
            return;

        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 0.2f);
    }

    public void PlaySound(AudioClip[] clips, GameObject obj)
    {
        if (CameraControls.instance.currentZoom > cameraCutoff)
            return;

        Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position);

        if (pos.x < 0f || pos.y < 0f || pos.x > 1f || pos.y > 1f)
            return;

        AudioSource.PlayClipAtPoint(clips[Random.Range(0, clips.Length)], Camera.main.transform.position, 0.1f);
    }
}
