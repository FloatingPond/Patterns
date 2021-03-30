using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Android;
using Unity.Notifications.Android;

public class GameManager : MonoBehaviour
{

    public int iPatternNumbers = 0;
    public int Highscore = 0;
    public float fGameTime; //Seconds for now
    public int buttonsPressed;
    public string sPatternNumbers;
    public string sPatternAnswer;

    public TMP_Text tScore, tHighscore, tAfterGame;

    ////Buttons
    //Game buttons
    public List<GameObject> Buttons = new List<GameObject>();
    //Canvas containing endgame buttons (Return, Restart)
    public Canvas endgameCanvas;
    //Used to enable/disable user able to press buttons - mainly when showing patterns and when not in-game
    public bool isGameButtonsDisabled = false;

    //Button Animation
    public float time_buttonsAreColour = 0.5f;
    public float time_buttonsTimeBetween = 0.25f;
    

    void Start()
    {
        LoadGame();
        //Game no longer automatically starts as that is now controlled by MainMenu
        //GenerateRandomPattern();
        //RestartGame();
    }
    void LoadGame()
    {
        PlayerData data = SaveSystem.LoadGame();

        Highscore = data.highscores[0];
        fGameTime = data.secondsPlayed;
        buttonsPressed = data.buttonsPressed;


    }
    void GenerateRandomPattern()
    {
        iPatternNumbers++;
        SetTextScore();
        sPatternNumbers = "";
        for (int i = 0; i < iPatternNumbers; i++)
        {
            int number = Random.Range(1, 9);
            sPatternNumbers += number.ToString();
        }
        //Animate
        StartCoroutine(AnimateButtonColours());
    }
    void AddToExistingPattern()
    {
        iPatternNumbers++;
        SetTextScore();
        //sPatternNumbers = "";
        int number = Random.Range(1, 9);
        sPatternNumbers += number.ToString();
        //Animate
        StartCoroutine(AnimateButtonColours());
    }
    //Call on every button press
    public void CheckAnswer()
    {
        ////Check if whole thing is correct
        if (sPatternAnswer == sPatternNumbers) //Correct!
        {
            NextRound();
        }
        else //Either partially or not correct
        {
            //Check if length is less than the actual answer 

            if (sPatternAnswer.Length < sPatternNumbers.Length) //Answer is not 
            { 
                //Potentiallly partially correct
                for (int i = 0; i < sPatternAnswer.Length; i++)
                {
                    if (sPatternAnswer[i] == sPatternNumbers[i])
                    {
                        continue;
                    }
                    else
                    {
                        EndGame();
                        return;
                    }
                }
            }
            else //Oh no. Answer is either equal to or more, but somehow here. Bad
            {
                EndGame();
            }
        }
    }

    void NextRound()
    {
        //Clear answer
        sPatternAnswer = "";
        EnableButtons(false);
        //GenerateRandomPattern();
        AddToExistingPattern();
    }
    void EndGame()
    {
        EnableButtons(false);
        endgameCanvas.enabled = true;
        //Show "GAME OVER" TEXT
        
        //Maybe also play a sound
        
        if (iPatternNumbers-1 > Highscore) //If new highscore
        {
            //New highscore!
            Highscore = iPatternNumbers-1;
            tAfterGame.text = "NEW HIGHSCORE!";
            
            
        }
        else
        {
            tAfterGame.text = "Game Over";
        }
        //Save highscore regardless
        Save();
        //Show score
        SetTextScore();
        //Play again?
        //Enable Play Again and Return buttons

    }
    
    //Called when a button is pressed
    public void ButtonPressed(int number)
    {
        
        if (!isGameButtonsDisabled)
        {
            AddToButtonPressed();
            sPatternAnswer += number.ToString();
            CheckAnswer();
        }
    }

    //Sets the text object's score
    void SetTextScore()
    {
        tScore.text = "SCORE: " + ((iPatternNumbers - 1).ToString());
        tHighscore.text = "HIGH SCORE: " + Highscore.ToString();
    }

    IEnumerator AnimateButtonColours()
    {
        for (int i = 0; i < sPatternNumbers.Length; i++)
        {
            yield return new WaitForSeconds(time_buttonsTimeBetween);
            //Get number for button
            int number = int.Parse(sPatternNumbers[i].ToString()) - 1;
            //Colour button
            Buttons[number].GetComponent<Button>().image.color = Color.red;
            yield return new WaitForSeconds(time_buttonsAreColour);
            //Uncolour button
            Buttons[number].GetComponent<Button>().image.color = Color.grey;
        }
        
        EnableButtons(true);
        yield return new WaitForSeconds(0.0f);
    }
    public void EnableButtons(bool state)
    {
        if (state == true)
        {
            isGameButtonsDisabled = false;
            foreach (GameObject button in Buttons)
            {
                button.GetComponent<Button>().image.color = Color.white;
            }
        }
        else
        {
            isGameButtonsDisabled = true;
            foreach (GameObject button in Buttons)
            {
                button.GetComponent<Button>().image.color = Color.grey;
            }
        }
    }


    public void RestartGame()
    {
        //Disable Play Again and Return buttons
        endgameCanvas.enabled = false;
        sPatternAnswer = "";
        sPatternNumbers = "";
        iPatternNumbers = 0;
        tAfterGame.text = "";
        AddToButtonPressed();
        NextRound();
    }

    public void AddToGameTime(float time)
    {
        fGameTime += time;
    }
    public float GetGameTime()
    {
        return fGameTime;
    }

    public void Save()
    {
        SaveSystem.SaveGame(this);
    }
    
    public void AddToButtonPressed()
    {
        buttonsPressed += 1;
        Save();
    }

}
