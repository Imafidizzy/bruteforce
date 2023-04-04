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
    public TMP_InputField vigkeyInputField;
    public TMP_InputField vigcodeInputField;
    public TMP_Text vigresultUI;
    public float wait = 0f;
    private string answer;
    private bool hasWon = false;
    public bool hasPlayed = false;
    private bool vigenereUnlocked = false;// Siwtch to true when you beat first level
    public GameObject vigenereMeter;
    public GameObject caesarMeter;
    private int vigenereCharge = 10;
    private int caesarCharge = 10;
    public GameObject caesarTool;
    public GameObject vigenereTool;


    void Start()
    {
        Time.timeScale = 1f;
        string currentScene = SceneManager.GetActiveScene().name;
        if(currentScene=="Challenge3")
        {
            answer = "YOUHAVESEENTOOMUCH";
        }

        //Loads battery symbols when game start depending on charge available
        updateCharge();
    }

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
        if(timer > 0 && !hasWon){
            scoreText = Mathf.Floor(timer/60).ToString("00") + ":" +  Mathf.Floor(timer%60).ToString("00");
        }
        else if (hasWon){
            scoreText = "";
        }
        else{
            scoreText = "00:00";
        }
    	timerUI.SetText(scoreText);

        if(timer <=0 || hasWon){
            EndLevel(hasWon);
        }

        //charge logic

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
        if(succeed && caesarCharge>=3){
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
            caesarCharge-=3;
            updateCharge();
        }
    }

    public void vigenereDecrypt()
    {
        string key = vigkeyInputField.text;
        bool succeed = true;

        foreach(char c in key)
        {
            if(!char.IsLetter(c))
            {
                succeed = false;
                break;
            }
        }

        //Means key only contains letters
        if(succeed)
        {
            key = key.ToUpper();
            string text = vigcodeInputField.text.ToUpper();

            string oldtext = "";
            int keyIndex = 0;
            foreach(char c in text)
            {
                char keyLetter = key[keyIndex];
                int alpha = (int)(keyLetter - 'A'); // number in the alphabet of letter of key letter

                int alphaText = (int)(c - 'A'); // number in the alphabet of letter of input letter

                int dif = alphaText - alpha;

                int alphaNew = (dif + 26) % 26;

                char oldLetter = (char) (alphaNew + 'A');

                oldtext += oldLetter;

                // Incrementing the key index 
                keyIndex = (keyIndex + 1) % key.Length;
            }
            vigresultUI.SetText(oldtext);
        }
        
    }

    private void updateCharge(){
        foreach (Transform child in caesarMeter.transform) {
             GameObject.Destroy(child.gameObject);
        }
        for(int i=0; i<caesarCharge; i++) {
            GameObject battery = Instantiate(Resources.Load("Battery")) as GameObject;
            battery.transform.SetParent(caesarMeter.transform);
            battery.transform.localScale = new Vector3(1,1,1);
        }
    }
}
