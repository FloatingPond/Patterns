using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageManager : MonoBehaviour
{
    //This script is for managing the message box(es)

    public GameObject popUpBase;

    public PopUpMessage MainMessageBox;
    
    //Not yet used
    public PopUpMessage FallenStreakMessageBox;

    
    
    


    public void CloseMessage()
    {
        popUpBase.GetComponent<Animator>().SetTrigger("Close");
    }
    public void OpenMessage()
    {
        Debug.Log("Test open message");
        popUpBase.GetComponent<Animator>().SetTrigger("Open");
    }
    public void DisplayWelcomeMessage()
    {
        PopulateButton("WELCOME", "Welcome first-timer. Time to waste your life.", "Letz g0");
    }

    void PopulateButton(string title, string body, string button)
    {

        //Set title to 'Welcome'
        MainMessageBox.tTitle.text = title;

        //Set body to 'Something'
        MainMessageBox.tBody.text = body;

        //Set body to 'Something'
        MainMessageBox.tButton.text = button;


    }


}
