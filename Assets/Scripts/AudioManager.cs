using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip agentSpeech;
    public GameObject begin;

    private float timer = 0f;
    private bool introPlayed = false;
    private int skipAppear;

    void Start()
    {
        skipAppear = (int) (agentSpeech.length /3);
        Time.timeScale = 1f;
        audio.PlayOneShot(agentSpeech, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > skipAppear){
            begin.SetActive(true);
        }
    }

    public void gotoMainMenu(){
          SceneManager.LoadScene("Selection");
    }
}
