using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    //This script is for managing the message box(es)

    public PopUpMessage MainMessageBox;
    public GameObject FallenStreakMessageBox, popUpBase;


    public void CloseMessage()
    {
        popUpBase.GetComponent<Animator>().SetTrigger("Close");
    }
    public void OpenMessage()
    {
        popUpBase.GetComponent<Animator>().SetTrigger("Open");
    }
    public void DisplayWelcomeMessage()
    {
        PopulateButton("1", "2", "3");
    }

    void PopulateButton(string title, string body, string button)
    {

        //Set title to 'Welcome'
        //Set body to 'Something'
        //Set body to 'Something'

    }


}
