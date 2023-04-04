using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionSelect : MonoBehaviour
{
    private int mission_no = 0;
    public TMP_Text title;
    private string[] titles = {"1 : Tour De France", "2 : The Handler", "3 : Double Trouble"};
    public GameObject backArrow;
    public GameObject frontArrow;
    void Start()
    {
        displayMission();
    }

    // Update is called once per frame
    void Update()
    {

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
        arrow();
    }

    private void arrow(){
        if(mission_no <= 0){
            backArrow.SetActive(false);
        }
        else {
            backArrow.SetActive(true);
        }

        if(mission_no == (titles.Length -1)){
            frontArrow.SetActive(false);
        }
        else {
            frontArrow.SetActive(true);
        }
    }

    
}
