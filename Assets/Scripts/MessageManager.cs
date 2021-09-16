using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageManager : MonoBehaviour
{
    //This script is for managing the message box(es)

    public GameObject popUpBase, popUpTutorial, popUpReclaim;

    public PopUpMessage MainMessageBox, ReclaimStreakMessageBox, TutorialMessageBox;

    //Message
    public void OpenMessage()
    {
        popUpBase.GetComponent<Animator>().SetTrigger("Open");
    }
    public void CloseMessage()
    {
        popUpBase.GetComponent<Animator>().SetTrigger("Close");
    }
    public void DisplayWelcomeMessage()
    {
        PopulateButton("WELCOME", "Welcome to Patterns. Choose a game mode.", "Alright!");
    }
    public void DisplayDailyMessageAbleToGetStreak(string title, string text, string button)
    {
        PopulateButton(title, text, button);
    }
    void PopulateButton(string title, string body, string button) //For the smaller intro message
    {
        //Set title to 'Welcome'
        MainMessageBox.tTitle.text = title;

        //Set body to 'Something'
        MainMessageBox.tBody.text = body;

        //Set button to 'Something'
        MainMessageBox.tButton.text = button;

        OpenMessage();
    }
    
    //Reclaim
    public void OpenReclaimMessage()
    {
       popUpReclaim.GetComponent<Animator>().SetTrigger("Open");
    }
    public void CloseReclaimMessage()
    {
        popUpReclaim.GetComponent<Animator>().SetTrigger("Close");
    }
    public void DisplayAbleToWatchRewardAd(string title, string text, string buttonPos, string buttonNeg)
    {
        PopulateButtonReclaim(title, text, buttonPos, buttonNeg);
    }
    void PopulateButtonReclaim(string title, string body, string buttonPos, string buttonNeg) //This is for the reward pop up message
    {
        //Set title to 'Welcome'
        ReclaimStreakMessageBox.tTitle.text = title;

        //Set body to 'Something'
        ReclaimStreakMessageBox.tBody.text = body;

        //Set button to 'Something'
        ReclaimStreakMessageBox.tButton.text = buttonPos;

        //Set button to 'Something'
        ReclaimStreakMessageBox.tButton2.text = buttonNeg;

        OpenReclaimMessage();
    }
    
    //Tutorial
    public void OpenMessageTutorial()
    {
        popUpTutorial.GetComponent<Animator>().SetTrigger("Open");
    }
    public void CloseTutorialMessage()
    {
        popUpTutorial.GetComponent<Animator>().SetTrigger("Close");
    }
    public void DisplayTutorialMessage(GameManager gm)
    {
        if (gm.currentGamemode == gm.gamemodeNames[0]) //Classic
        {
            PopulateTutorial("Tutorial: Classic", "Remember the pattern. Press buttons in pattern shown.", "Start");
        }
        if (gm.currentGamemode == gm.gamemodeNames[1]) //Random
        {
            PopulateTutorial("Tutorial: Random", "Pattern will be random each time. Press buttons in pattern shown.", "Start");
        }
        if (gm.currentGamemode == gm.gamemodeNames[2]) //Match
        {
            PopulateTutorial("Tutorial: Match", "Match each colour to its partner. Don't touch the bomb!", "Start");
        }
        if (gm.currentGamemode == gm.gamemodeNames[3]) //Timed
        {
            PopulateTutorial("Tutorial: Timed Round", "HIT the shown tiles as quickly as possible!", "HIT");
        }
    }
    void PopulateTutorial(string title, string body, string button) //For the tutorial message
    {
        //Set title to 'Welcome'
        TutorialMessageBox.tTitle.text = title;

        //Set body to 'Something'
        TutorialMessageBox.tBody.text = body;

        //Set button to 'Something'
        TutorialMessageBox.tButton.text = button;

        OpenMessageTutorial();
    }










    




}
