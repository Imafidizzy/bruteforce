using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip introSpeech;
    public AudioClip agentSpeech;
    public GameObject begin;

    public float timer = 0f;
    private bool introPlayed = false;

    void Start()
    {
        Time.timeScale = 1f;
        audio.PlayOneShot(introSpeech, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer> 44 && !introPlayed){
            audio.PlayOneShot(agentSpeech, 5f);
            introPlayed = true;
        }
        if(timer > 15){
            begin.SetActive(true);
        }
    }

    public void beginChallenge(){
        SceneManager.LoadScene("Selection");
    }
}
