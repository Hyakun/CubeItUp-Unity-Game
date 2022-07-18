using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static int score = 0;
    public static int intrebariCorecte = 0;
    public static int numarTotalIntrebari = 0;
    public static string emailLogat = "";
    public static bool bossDeath = false;
    public static bool LoggedIn = false;
    public Sound[] sounds;
    public static int sceneCount = 2;

    public static AudioManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    private void Start()
    {
        Play("BackGroundMusic");
    }

    // Update is called once per frame
    public void Play (string name)
    {
      Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
            
        s.source.Play();
    }
    void Update(){
        if(bossDeath){
            OpenScoreSave();
        }
        
    }
    public void OpenScoreSave()
    {
        bossDeath = false;
        StartCoroutine(scoreSave());
    }
    
    IEnumerator scoreSave()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("ScoareSave");
    }
}
