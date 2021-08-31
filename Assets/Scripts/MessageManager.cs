using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageManager : MonoBehaviour
{
    //This script is for managing the message box(es)

    public GameObject popUpBase, Variant;

    public PopUpMessage MainMessageBox, VariantMessageBox;
    
    public void CloseMessage()
    {
        popUpBase.GetComponent<Animator>().SetTrigger("Close");
    }
    public void CloseVariantMessage()
    {
        Variant.GetComponent<Animator>().SetTrigger("Close");
    }
    public void OpenMessage()
    {
        //Debug.Log("Test open message");
        popUpBase.GetComponent<Animator>().SetTrigger("Open");
    }
    public void OpenVariantMessage()
    {
        //Debug.Log("Test open message");
        Variant.GetComponent<Animator>().SetTrigger("Open");
    }
    public void DisplayWelcomeMessage()
    {
        PopulateButton("WELCOME", "Welcome first-timer. Time to waste your life.", "Letz g0");
    }

    public void DisplayDailyMessageAbleToGetStreak(string title, string text, string button)
    {
        PopulateButton(title, text, button);
    }

    public void DisplayAbleToWatchRewardAd(string title, string text, string buttonPos, string buttonNeg)
    {
        PopulateButtonVariant(title, text, buttonPos, buttonNeg);
    }

    void PopulateButton(string title, string body, string button)
    {
        //Set title to 'Welcome'
        MainMessageBox.tTitle.text = title;

        //Set body to 'Something'
        MainMessageBox.tBody.text = body;

        //Set button to 'Something'
        MainMessageBox.tButton.text = button;

        OpenMessage();
    }
    void PopulateButtonVariant(string title, string body, string buttonPos, string buttonNeg)
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
