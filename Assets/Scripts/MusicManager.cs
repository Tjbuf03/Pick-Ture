using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    //void Awake()
    //{
       // if (instance == null)
        //{
        //    instance = this;
         //   DontDestroyOnLoad(gameObject); // Make this GameObject persist across scenes
        //}
        //else
        //{
        //    Destroy(gameObject); // Destroy duplicate instances
        //}
    //}
    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (!audio.isPlaying)
        {
            audio.Play();
        }
    }

}

