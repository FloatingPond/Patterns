using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Android;
using Unity.Notifications.Android;

public class GameManager : MonoBehaviour
{
    public string[] gamemodeNames = { "classic", "random", "Game3", "Game4" };
    public int iPatternNumbers = 0;
    public int[] Highscore = new int[4];
    
    public float fGameTime; //Seconds for now
    public int buttonsPressed;
    public string sPatternNumbers;
    public string sPatternAnswer;
    string currentGamemode;

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

    //Game Streak
    [SerializeField]
    private int gameStreak;
    [SerializeField]
    private int gameStreakHighscore;


    public int GetGameStreak()
    {
        return gameStreak;
    }
    public int GetGameStreakHighscore()
    {
        return gameStreakHighscore;
    }

    //Used when loading in from Save data
    void SetGameStreak(int gs, int gshs)
    {
        gameStreak = gs;
        gameStreakHighscore = gshs;
    }

    public void AddToGameStreak()
    {
        gameStreak++;
        ChangeGameStreakHighscore();
    }
    public void ResetGameStreak()
    {
        gameStreak = 1;
    }

    //Called after gameStreak changes
    public void ChangeGameStreakHighscore()
    {
        if (gameStreak > gameStreakHighscore)
        {
            gameStreakHighscore = gameStreak;
        }
    }

    void Start()
    {
        Highscore = new int[4];
        Highscore[0] = 0;
        Highscore[1] = 0;
        Highscore[2] = 0;
        Highscore[3] = 0;
        LoadGame();
        //Game no longer automatically starts as that is now controlled by MainMenu
        //GenerateRandomPattern();
        //RestartGame();
    }
    void LoadGame()
    {
        PlayerData data = SaveSystem.LoadGame();
        if (data != null)
        { 
            Highscore[0] = data.highscores[0];
            Highscore[1] = data.highscores[1];
        
            fGameTime = data.secondsPlayed;
            buttonsPressed = data.buttonsPressed;
            SetGameStreak(data.gameStreak, data.gameStreakHighscore);
        }
        

    }
    void RandomGenerateRandomPattern()
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
    void ClassicAddToExistingPattern()
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
        if (currentGamemode == gamemodeNames[0])
        ClassicAddToExistingPattern();
        if (currentGamemode == gamemodeNames[1])
            RandomGenerateRandomPattern();

    }
    void EndGame()
    {
        EnableButtons(false);
        endgameCanvas.enabled = true;
        //Show "GAME OVER" TEXT

        //Maybe also play a sound
        CheckHighscore();
        
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
        if (currentGamemode == gamemodeNames[0])
        {
            tHighscore.text = "HIGH SCORE: " + Highscore[0].ToString();
        }
        else if (currentGamemode == gamemodeNames[1])
        {
            tHighscore.text = "HIGH SCORE: " + Highscore[1].ToString();
        }
        else if (currentGamemode == gamemodeNames[2])
        {
            tHighscore.text = "HIGH SCORE: " + Highscore[2].ToString();
        }
        else if (currentGamemode == gamemodeNames[3])
        {
            tHighscore.text = "HIGH SCORE: " + Highscore[3].ToString();
        }
        else
        {
            Debug.Log("Error in high score text setting");
        }

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
    
    public void RestartGame(string gamemode)
    {
        if (gamemode != "")
        {
            currentGamemode = gamemode;
        }
        else
        {
            //currentGamemode stays the same
        }
        Debug.Log(currentGamemode);
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

    void CheckHighscore()
    {
        if (currentGamemode == gamemodeNames[0])
        {
            if (iPatternNumbers - 1 > Highscore[0]) //If new highscore
            {
                //New highscore!
                Highscore[0] = iPatternNumbers - 1;
                tAfterGame.text = "NEW HIGHSCORE!";
            }
            else
            {
                tAfterGame.text = "Game Over";
            }
        }
        else if (currentGamemode == gamemodeNames[1])
        {
            if (iPatternNumbers - 1 > Highscore[1]) //If new highscore
            {
                //New highscore!
                Highscore[1] = iPatternNumbers - 1;
                tAfterGame.text = "NEW HIGHSCORE!";
            }
            else
            {
                tAfterGame.text = "Game Over";
            }
        }
        else if (currentGamemode == gamemodeNames[2])
        {
            if (iPatternNumbers - 1 > Highscore[2]) //If new highscore
            {
                //New highscore!
                Highscore[2] = iPatternNumbers - 1;
                tAfterGame.text = "NEW HIGHSCORE!";
            }
            else
            {
                tAfterGame.text = "Game Over";
            }
        }
        else if (currentGamemode == gamemodeNames[3])
        {
            if (iPatternNumbers - 1 > Highscore[3]) //If new highscore
            {
                //New highscore!
                Highscore[3] = iPatternNumbers - 1;
                tAfterGame.text = "NEW HIGHSCORE!";
            }
            else
            {
                tAfterGame.text = "Game Over";
            }
        }
        else
        {
            Debug.Log("Error - invalid gamemode");
        }
    }



}
