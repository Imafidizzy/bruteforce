using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MissionSelect : MonoBehaviour
{
    private int mission_no;
    public TMP_Text title;
    public TMP_Text requirement;
    private string[] titles = {"1 : Tutorial", "2 : The French Connection", "3 : The Handler"};
    private string[] requirements = {"Tools Needed: Caesar Tool", "Tools Needed: Vigenere Tool","Tools Needed: Caesar Tool, Vigenere Tool"};
    public GameObject backArrow;
    public GameObject frontArrow;
    public TMP_Text displayText;
    public Image displayed;
    string caesarDesc = "The caesar tool requires you to enter the encypted text in the 'code' section and a number in the 'key' section. Caesar encryoption moves all alphabets forward/backwards by the key when encrypting/decrypting.";
    string vigenereDesc = "The vigenere tool takes a word or combination of letters as its key. It works like the caersar cipher but each letter of the text being encrypted is shifted by a different amount. These amounts follow the pattern of the letter in the key. It will loop around the letters in the key until all letters in the code have been encrypted";
    public Sprite caesar;
    public Sprite vigenere;
    private int maxUnlocked;

    private Utility.PlayerData PlayerData = new Utility.PlayerData();

    void Start()
    {
        mission_no = 0;
        PlayerData.Load();
        maxUnlocked = PlayerData.GetCompletedLevelCount();
        displayMission(mission_no);
    }

    public void nextMission(){
        if(mission_no <2){
            mission_no++;
            displayMission(mission_no);
        }
    }
    
    public void previousMission(){
        mission_no--;
        displayMission(mission_no);
    }

    private void displayMission(int number){
        title.SetText(titles[number]);
        requirement.SetText(requirements[number]);
        if(number <= 0){
            backArrow.SetActive(false);
        }
        else {
            backArrow.SetActive(true);
        }

        if(number >= 2) {
            frontArrow.SetActive(false);
        }
        else if(mission_no >= maxUnlocked) {
            frontArrow.SetActive(false);
        }
        else {
            frontArrow.SetActive(true);
        }
    }

    public void startChallenge(){
        if(mission_no == 0){
            SceneManager.LoadScene("Challenge1");
        }
        else if (mission_no == 1){
            SceneManager.LoadScene("Challenge2");
        }
        else {
            SceneManager.LoadScene("Challenge3");
        }
    }

    public void showCaesar(){
        displayed.sprite = caesar;
        displayText.SetText(caesarDesc);

    }
    public void showVigenere(){
        displayed.sprite = vigenere;
        displayText.SetText(vigenereDesc);
    }



    
}
