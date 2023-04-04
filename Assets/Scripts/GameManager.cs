using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    public float timer;
    public TMP_Text timerUI;
    public AudioSource audio;
    public AudioClip challengeMusic;
    public AudioClip loseAudio;
    public AudioClip winAudio;
    public TMP_InputField guessInputField;
    public TMP_InputField keyInputField;
    public TMP_InputField codeInputField;
    public TMP_Text resultUI;
    public float wait = 0f;
    private string answer;
    private bool hasWon = false;
    public bool hasPlayed = false;
    private bool vigenereUnlocked = false;
    public GameObject caesarTool;
    public GameObject vigenereTool;


    void Start()
    {
        Time.timeScale = 1f;
        answer = "WELCOME";
    }

    // Update is called once per frame
    void Update()
    {   
        if(Input.GetKeyDown("1")){
            caesarTool.SetActive(true);
        }

        if(Input.GetKeyDown("2") && vigenereUnlocked){
            vigenereTool.SetActive(true);
        }

        string scoreText;
        timer -= Time.deltaTime;
        if(timer > 0){
            scoreText = Mathf.Floor(timer/60).ToString("00") + ":" +  Mathf.Floor(timer%60).ToString("00");
        }
        else{
            scoreText = "00:00";
        }
    	timerUI.SetText(scoreText);

        if(timer <=0 || hasWon){
            EndLevel(hasWon);
        }
    }

    public void AttemptHack(){
        if(guessInputField.text.ToUpper() == answer){
            hasWon = true;
        }
    }

    private void EndLevel(bool win){
        wait += Time.deltaTime;
        if(!hasPlayed){
            hasPlayed = true;
            if(win){
                audio.PlayOneShot(winAudio, 5f);  
            }
            else {
                audio.PlayOneShot(loseAudio, 10f);
            }
        }
        if(wait > 11){
            SceneManager.LoadScene("Titlescreen");
            }
    }

    public void caesarDecrypt(){
        int key;
        bool succeed = int.TryParse(keyInputField.text, out key);
        if(succeed){
            key = key % 26;
            char [] buffer = codeInputField.text.ToUpper().ToCharArray();
            for(int i =0; i < buffer.Length; i++){
                char letter = buffer[i];
                letter = (char) (letter - key);
                if (letter > 'Z')
                {
                    letter = (char)(letter - 26);
                }
                else if (letter < 'A')
                {
                    letter = (char)(letter + 26);
                }
                buffer[i] = letter;
            }
            string result = new string(buffer);
            resultUI.SetText(result);
        }

    }

    public void vigenereDecrypt(){
        
    }
}
