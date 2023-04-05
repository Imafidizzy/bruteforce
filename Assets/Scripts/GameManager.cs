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
    public GameObject inventory;
    public float wait = 0f;
    private string answer;
    private bool hasWon = false;
    public bool hasPlayed = false;
    public GameObject vigenereMeter;
    public GameObject caesarMeter;
    public GameObject caesarTool;
    public GameObject vigenereTool;
    public int missionNumber;

    private Utility.PlayerData PlayerData = new Utility.PlayerData();
    private Utility.Tool _caesarTool;
    private Utility.Tool _vigenereTool;

    void Start()
    {
        PlayerData.Load();
        _caesarTool = PlayerData.GetTool("Caeser");
        _vigenereTool = PlayerData.GetTool("Vigenere");
        StartCoroutine(setInterval(10f, () => {
            rechargeTool(ref _caesarTool, updateCaesarCharge);
        }));
        StartCoroutine(setInterval(10f, () => {
            rechargeTool(ref _vigenereTool, updateVigenereCharge);
        }));

        Time.timeScale = 1f;
        string currentScene = SceneManager.GetActiveScene().name;
        if(currentScene=="Challenge1"){
            answer = "WELCOME";
        }
        else if(currentScene=="Challenge2"){
            answer = "YOUHAVESEENTOOMUCH";
        }
        else{
            answer = "DONTTRUSTTHEM";
        }
        updateCaesarCharge();
        updateVigenereCharge();

        if(_vigenereTool.Unlocked){
            inventory.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().SetText("V");
        }
    }

    void Update()
    {   
        if(Input.GetKeyDown("1")){
            caesarTool.SetActive(true);
            vigenereTool.SetActive(false);
        }
        if(Input.GetKeyDown("2") && _vigenereTool.Unlocked){
            vigenereTool.SetActive(true);
            caesarTool.SetActive(false);
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
                _vigenereTool.Unlocked = true;
                PlayerData.SetTool("Vigenere", _vigenereTool);
                PlayerData.CompleteLevel(missionNumber);
                audio.PlayOneShot(winAudio, 10f);  
            }
            else {
                audio.PlayOneShot(loseAudio, 10f);
            }
        }
        if(wait > 11){
            SceneManager.LoadScene("Selection");
            }
    }

    public void caesarDecrypt(){
        int key;
        bool succeed = int.TryParse(keyInputField.text, out key);
        if(succeed && _caesarTool.ChargeCount>=3){
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
            if(result.Length > 30){
                result= result.Substring(0,29);
            }
            resultUI.SetText(result);
            _caesarTool.ChargeCount-=3;
            PlayerData.SetTool("Caeser", _caesarTool);
            updateCaesarCharge();
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
            if(oldtext.Length > 30){
                oldtext= oldtext.Substring(0,29);
            }
            vigresultUI.SetText(oldtext);
            _vigenereTool.ChargeCount-=3;
            PlayerData.SetTool("Vigenere", _vigenereTool);
            updateVigenereCharge();
        }
        
    }

    private void updateCaesarCharge(){
        foreach (Transform child in caesarMeter.transform) {
             GameObject.Destroy(child.gameObject);
        }
        for(int i=0; i<_caesarTool.ChargeCount; i++) {
            GameObject battery = Instantiate(Resources.Load("Battery")) as GameObject;
            battery.transform.SetParent(caesarMeter.transform);
            battery.transform.localScale = new Vector3(1,1,1);
        }
    }

    private void updateVigenereCharge(){
        foreach (Transform child in vigenereMeter.transform) {
             GameObject.Destroy(child.gameObject);
        }
        for(int i=0; i<_vigenereTool.ChargeCount; i++) {
            GameObject battery = Instantiate(Resources.Load("Battery")) as GameObject;
            battery.transform.SetParent(vigenereMeter.transform);
            battery.transform.localScale = new Vector3(1,1,1);
        }
    }

    private void rechargeTool(ref Utility.Tool tool, System.Action callback)
    {
        if(tool.ChargeCount < 10) {
            tool.ChargeCount++;
            PlayerData.SetTool(tool.Name, tool);
            callback();
        }
    }

    private IEnumerator setInterval(float interval, System.Action callback)
    {
        while(true)
        {
            yield return new WaitForSeconds(interval);
            callback();
        }
    }
}
