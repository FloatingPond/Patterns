using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageManager : MonoBehaviour
{
    //This script is for managing the message box(es)

    public GameObject popUpBase, popUpTutorial, Variant;

    public PopUpMessage MainMessageBox, VariantMessageBox, TutorialMessageBox;
    
    public void CloseMessage()
    {
        popUpBase.GetComponent<Animator>().SetTrigger("Close");
    }
    public void CloseVariantMessage()
    {
        Variant.GetComponent<Animator>().SetTrigger("Close");
    }
    public void CloseTutorialMessage()
    {
        popUpTutorial.GetComponent<Animator>().SetTrigger("Close");
    }

    public void OpenMessage()
    {
        //Debug.Log("Test open message");
        popUpBase.GetComponent<Animator>().SetTrigger("Open");
    }

    public void OpenMessageTutorial()
    {
        //Debug.Log("Test open message");
        popUpTutorial.GetComponent<Animator>().SetTrigger("Open");
    }

    public void OpenVariantMessage()
    {
        //Debug.Log("Test open message");
        Variant.GetComponent<Animator>().SetTrigger("Open");
    }

    public void DisplayWelcomeMessage()
    {
        PopulateButton("WELCOME", "Welcome to Patterns. Choose a game mode.", "Alright!");
    }

    public void DisplayDailyMessageAbleToGetStreak(string title, string text, string button)
    {
        PopulateButton(title, text, button);
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

    public void DisplayAbleToWatchRewardAd(string title, string text, string buttonPos, string buttonNeg)
    {
        PopulateButtonVariant(title, text, buttonPos, buttonNeg);
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

    void PopulateButtonVariant(string title, string body, string buttonPos, string buttonNeg) //This is for the reward pop up message
    {
        //Set title to 'Welcome'
        VariantMessageBox.tTitle.text = title;

        //Set body to 'Something'
        VariantMessageBox.tBody.text = body;

        //Set button to 'Something'
        VariantMessageBox.tButton.text = buttonPos;

        //Set button to 'Something'
        VariantMessageBox.tButton2.text = buttonNeg;

        OpenVariantMessage();
    }




}
