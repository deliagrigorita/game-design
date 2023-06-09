using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance{get; private set;}
    public Sound[] sounds;

    private void Awake() {
        Instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            
        }
    }

    private void Start() {
        // Create a temporary reference to the current scene.
         Scene currentScene = SceneManager.GetActiveScene ();
 
         // Retrieve the name of this scene.
         string sceneName = currentScene.name;
 
         if (sceneName == "main") 
         {
            Play("MainTheme");
         }
         else if (sceneName == "Start Screen")
         {
            Play("MenuTheme");
         }
         else if(sceneName == "WinGame"){
            Play("WinTheme");
         }
         else if(sceneName == "EndScene"){
            Play("PlayerDeath");
         }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(String name){
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s != null){
            s.source.Play();
        }
        else{
            Debug.Log("Cant play sound: " + s + " !");
        }
    }

    public void Stop(String name){
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s != null){
            s.source.Stop();
        }
        else{
            Debug.Log("Cant stop sound: " + s + " !");
        }
    }
}
