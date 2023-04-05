using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MissionSelect : MonoBehaviour
{
    private int mission_no = 0;
    public TMP_Text title;
    public TMP_Text requirement;
    private string[] titles = {"1 : Tutorial", "2 : The French Connection", "3 : The Handler"};
    private string[] requirements = {"Tools Needed: Caesar Tool", "Tools Needed: Vigenere Tool","Tools Needed: Caesar Tool, Vigenere Tool"};
    public GameObject backArrow;
    public GameObject frontArrow;
    public TMP_Text displayText;
    public Image displayed;
    string caesarDesc = "Caesar";
    string vigenereDesc = "Vigenere";
    public Sprite caesar;
    public Sprite vigenere;
    private int maxUnlocked;

    private Utility.PlayerData PlayerData = new Utility.PlayerData();

    void Start()
    {
        PlayerData.Load();
        maxUnlocked = PlayerData.GetCompletedLevelCount();
        displayMission();
    }

    public void nextMission(){
        mission_no+=1;
        displayMission();
    }
    
    public void previousMission(){
        mission_no-=1;
        displayMission();
    }

    private void displayMission(){
        title.SetText(titles[mission_no]);
        requirement.SetText(requirements[mission_no]);
        arrow();
    }

    private void arrow(){
        if(mission_no <= 0){
            backArrow.SetActive(false);
        }
        else {
            backArrow.SetActive(true);
        }

        if(mission_no >= (maxUnlocked)){
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
