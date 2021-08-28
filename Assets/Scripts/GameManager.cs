using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Android;
using Sirenix.OdinInspector;
using System;

public class GameManager : MonoBehaviour
{
    //Gamemode names - NEEDS TO BE CHANGED IN START METHOD IF CHANGED (because it sometimes doesn't change

    [Title("Variables")]

    public string[] gamemodeNames = { "classic", "random", "match", "timedround" };
    public int iPatternNumbers = 0;
    public int[] Highscore = new int[4];
    bool newHighscoreThisGame = false;

    private IEnumerator coroutine;
    public float fGameTime; //Seconds for now
    public float fTimedRoundTimer;
    public float fTimedRoundLength = 60;

    public int buttonsPressed;
    public string sPatternNumbers;
    public string sPatternAnswer;
    public string currentGamemode;

    //Button Animation
    [ProgressBar(0.1f, 2f)]
    public float time_buttonsAreColour = 0.5f;
    [ProgressBar(0.1f, 5f)]
    public float time_buttonsTimeBetween = 0.25f;

    private int matchA, matchB, matchC, matchD, matchBomb;
    private bool bMatchA, bMatchB, bMatchC, bMatchD, bMatchBomb;
    public string matchComparison;
    public int matchComparisonNumber;
    public int newMCnumber;
    public int newNumber, matchCounter = 0;

    [Title("Objects")]
    public List<GameObject> Buttons = new List<GameObject>();
    public TMP_Text tScore, tHighscore, tAfterGame, tTimedRoundsTimer;
    ////Buttons
    //Game buttons
    
    //Panel containing endgame buttons (Return, Restart)
    public GameObject endgamePanel;
    //Used to enable/disable user able to press buttons - mainly when showing patterns and when not in-game
    public bool isGameButtonsDisabled = false;
    
    

    //Game Streak
    [SerializeField]
    private int gameStreak; //Loaded from file
    [SerializeField]
    private int gameStreakLast; //Streak had before current
    [SerializeField]
    private int gameStreakHighscore; //Highest achieved streak

    public DateTime dateLastAcquiredStreak;

    public DateTime dateLastAcquiredStreakLast;
    
    [Title("Managers")]
    //Ad Manager
    public AdManager am;

    //Sound manager
    public SoundManager sm;
    
    public MessageManager mm;

    [Title("Buttons and Debug")]

    //DateTime stuff

    public DateTime dt;
    public DateTime dtnew, dtnew2;
    [PropertyOrder(1)]
    public string dtString2 = "2005-10-05 22:12 PM";
    [PropertyOrder(1)]
    public string dtString3 = "2005-10-05 22:12 PM";
    [PropertyOrder(2)]
    public string dtString4 = "2021-10-05 22:12 PM";

    [Button(ButtonSizes.Small)]
    [PropertyOrder(1)]
    private void ParseStringToDateTime()
    {
        StringToDateTime();
    }

    [Button(ButtonSizes.Small)]
    private void ChangeDateLastAcquiredStreak()
    {
        dateLastAcquiredStreak = DateTime.ParseExact(dtString4, "yyyy-MM-dd HH:mm tt", null);
    }

    [Button(ButtonSizes.Small)]
    [PropertyOrder(3)]
    private void ChangeDlasAndDisplay()
    {
        dateLastAcquiredStreak = DateTime.ParseExact(dtString4, "yyyy-MM-dd HH:mm tt", null);
        //ChangeGameStreak();
        CheckGameStreak(true);
    }

    [Button(ButtonSizes.Small)]
    [PropertyOrder(2)]
    private void GetDateLastAcquiredStreak()
    {
        dtString4 = dateLastAcquiredStreak.ToString("yyyy-MM-dd HH:mm tt");
    }


    void Awake() //Called before MainMenu's Start method
    {
        gamemodeNames = new string[] { "classic", "random", "match", "timedround" };
        Highscore = new int[4];
        Highscore[0] = 0;
        Highscore[1] = 0;
        Highscore[2] = 0;
        Highscore[3] = 0;
        bool playerFirstTime = LoadGame(); //After variables are declared with numbers in case Load doesn't work, LoadGame is 
        am.SetupOnStart(playerFirstTime);


    }

    private void Update()
    {
        if (currentGamemode == gamemodeNames[3])
        {
            if (fTimedRoundTimer > 0)
            { 
                fTimedRoundTimer -= Time.deltaTime;
                tTimedRoundsTimer.text = fTimedRoundTimer.ToString("F0");
            }
            else
            {
                tTimedRoundsTimer.text = "";
                EndGame();
            }
        }
    }

    public bool LoadGame() //Called in Awake - loads data from load system and saves into memory
    {
        PlayerData data = SaveSystem.LoadGame();
        if (data != null) //Date exists
        { 
            Highscore[0] = data.highscores[0];
            Highscore[1] = data.highscores[1];
            Highscore[2] = data.highscores[2];
            Highscore[3] = data.highscores[3];


            fGameTime = data.secondsPlayed;
            buttonsPressed = data.buttonsPressed;
            SetGameStreak(data.gameStreak, data.gameStreakHighscore);
            dateLastAcquiredStreak = DateTime.ParseExact(data.dateLastAcquiredStreak, "yyyy-MM-dd HH:mm tt", null);
            
            am.SetadsRewardsWatched(data.adsRewardsWatched);

            sm.masterFloat = data.master;
            sm.musicFloat = data.music;
            sm.SFX_Float = data.sfx;
            sm.voiceFloat = data.voice;

            am.dtLastTimeRewardAdWatched = DateTime.ParseExact(data.dateLastRewardAdWatched, "yyyy-MM-dd HH:mm tt", null);
            CheckGameStreak(true); //Check Streak
            sm.LoadSliders();
            return false;
        }
        else //No file exists, so populate variables
        {
            Highscore[0] = 0;
            Highscore[1] = 0;
            Highscore[2] = 0;
            Highscore[3] = 0;

            fGameTime = 0;
            buttonsPressed = 0;

            gameStreak = 0;
            gameStreakHighscore = 0;
            am.dtLastTimeRewardAdWatched = DateTime.ParseExact("2001-01-01 12:00 PM", "yyyy-MM-dd HH:mm tt", null); //Set far into the past
            mm.DisplayWelcomeMessage();
            sm.masterFloat = 1;
            sm.musicFloat = 1;
            sm.SFX_Float = 1;
            sm.voiceFloat = 1;
            sm.LoadSliders();
            return true;
        }
        
    }
    public void Save() //Saves data into system
    {
        SaveSystem.SaveGame(this);
    }
    void RandomGenerateRandomPattern() //Game 1
    {
        EnableButtons(false);
        iPatternNumbers++;
        SetTextScore();
        sPatternNumbers = "";
        for (int i = 0; i < iPatternNumbers; i++)
        {
            int number = UnityEngine.Random.Range(1, 9);
            sPatternNumbers += number.ToString();
        }
        //Check and set highscore text
        CheckHighscore();
        SetTextScore();
        //Animate
        coroutine = AnimateButtonColours();
        StartCoroutine(coroutine);
    }
    void ClassicAddToExistingPattern() //Game 2
    {
        EnableButtons(false);
        iPatternNumbers++;
        CheckHighscore();
        SetTextScore();
        //sPatternNumbers = "";
        int number = UnityEngine.Random.Range(1, 9);
        sPatternNumbers += number.ToString();
        //Animate
        coroutine = AnimateButtonColours();
        StartCoroutine(coroutine);
    }
    void MatchPattern() //Game 3
    {
        foreach (GameObject button in Buttons)
        {
            button.GetComponent<Button>().interactable = true;
        }
        ResetMatchTileCounters();
        EnableButtons(false);
        iPatternNumbers++;
        CheckHighscore();
        SetTextScore();
        SetMatchButtons();
        StartCoroutine(HideMatchButtons());
    }
    void TimedRoundCallNextNumber() //Game 4
    {
        iPatternNumbers++;
        CheckHighscore();
        SetTextScore();

        string oldnumber = sPatternNumbers;
        while (oldnumber == sPatternNumbers)
        {
            int number = UnityEngine.Random.Range(1, 9);
            sPatternNumbers = number.ToString();
            if (oldnumber == sPatternNumbers)
            {
                Debug.Log("STILL THE SAME - " + sPatternNumbers);
            }
            else
            {
                Debug.Log("we out - " + sPatternNumbers);
            }
        }

        coroutine = AnimateButtonColours();
        StartCoroutine(coroutine);

    }

    void SetMatchButtons()  //Called from Game 3
    {
        bool[] buttonIsSet = new bool[9];
        for (int i = 0; i < 9; i++)
        {
            while (buttonIsSet[i] == false)
            {
                switch (UnityEngine.Random.Range(1, 6))
                {
                    case 1:
                        if (matchA < 2)
                        {
                            Buttons[i].GetComponent<Image>().color = Color.green;
                            matchA++;
                            Buttons[i].tag = "MatchA";
                            buttonIsSet[i] = true;
                        }
                        break;
                    case 2:
                        if (matchB < 2)
                        {
                            Buttons[i].GetComponent<Image>().color = Color.red;
                            matchB++;
                            Buttons[i].tag = "MatchB";
                            buttonIsSet[i] = true;
                        }
                        break;
                    case 3:
                        if (matchC < 2)
                        {
                            Buttons[i].GetComponent<Image>().color = Color.yellow;
                            matchC++;
                            Buttons[i].tag = "MatchC";
                            buttonIsSet[i] = true;
                        }
                        break;
                    case 4:
                        if (matchD < 2)
                        {
                            Buttons[i].GetComponent<Image>().color = Color.cyan;
                            matchD++;
                            Buttons[i].tag = "MatchD";
                            buttonIsSet[i] = true;
                        }
                        break;
                    case 5:
                        if (matchBomb < 1)
                        {
                            Buttons[i].GetComponent<Image>().color = Color.black;
                            matchBomb++;
                            Buttons[i].tag = "MatchBomb";
                            buttonIsSet[i] = true;
                        }
                        break;
                }
            }
        }
    }
    IEnumerator HideMatchButtons() //Called from Game 3
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 9; i++)
        {
            Buttons[i].GetComponent<Image>().color = Color.white;
        }
        EnableButtons(true);
    }
    
    void StringToDateTime() //Used to calculate the time between two datetimes
    {
        dtnew = DateTime.ParseExact(dtString2, "yyyy-MM-dd HH:mm tt", null);
        dtnew2 = DateTime.ParseExact(dtString3, "yyyy-MM-dd HH:mm tt", null);
        TimeSpan TimeBetween = dtnew2.Subtract(dtnew);
        Debug.Log(TimeBetween);
        Debug.Log(TimeBetween.Hours);

        Debug.Log(dtnew);
    
}
    
    void ResetMatchTileCounters() 
    {
        matchA = 0;
        matchB = 0;
        matchC = 0;
        matchD = 0;
        matchBomb = 0;
    }
    
    public void CheckAnswer() //Call on every button press when game is playing
    {
        if ((currentGamemode == gamemodeNames[0]) || (currentGamemode == gamemodeNames[1]))
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
        else if (currentGamemode == gamemodeNames[2])
        {

        }
        else if (currentGamemode == gamemodeNames[3])
        {
            if (sPatternAnswer == sPatternNumbers) //Correct!
            {
                StopCoroutine(coroutine);
                NextRound();
            }
            else
            {
                sPatternAnswer = "";
                //Try again
            }
        }
        else
        {
            //Error
        }
    }
    
    //Called when the criteria has been met for a gamemode
    void NextRound()
    {
        //Clear answer
        sPatternAnswer = "";
        
        //GenerateRandomPattern();
        if (currentGamemode == gamemodeNames[0])
            ClassicAddToExistingPattern();
        if (currentGamemode == gamemodeNames[1])
            RandomGenerateRandomPattern();
        if (currentGamemode == gamemodeNames[2])
            MatchPattern();
        if (currentGamemode == gamemodeNames[3])
            TimedRoundCallNextNumber();
    }

    //Called when the gamemode criteria has been failed
    public void EndGame()
    {
        //FOR NOW
        ChangeGameStreak();
        //Anyway
        StopAllCoroutines();
        EnableButtons(false);
        endgamePanel.SetActive(true);
        //Show "GAME OVER" TEXT

        //Maybe also play a sound
        CheckHighscoreAtEndOfGame();
        
        //Save highscore regardless
        Save();
        //Show score
        SetTextScore();
        //Play again?
        //Enable Play Again and Return buttons
        CheckGameStreak(false);

    }

    public void MakeButtonsInteractable()
    {
        foreach (GameObject button in Buttons)
        {
            button.GetComponent<Button>().interactable = true;
            button.GetComponent<Image>().color = Color.white;
        }
    }
    //Called when a button is pressed
    public void ButtonPressed(int number)
    {
        if (!isGameButtonsDisabled)
        {
            AddToButtonPressed();
            sPatternAnswer += number.ToString();
            if (currentGamemode != gamemodeNames[2])
            {
                CheckAnswer();
            }
            else if (currentGamemode == gamemodeNames[2])
            {
                CheckMatchAnswer(number);
            }
        }
    }
    //USED FOR MATCH GAME MODE
    void CheckMatchAnswer(int number)
    {
        //This corrects the number comparison due to there being a difference in the keypad shown to the player and the array
        //Ie array starts at 0 and keypad starts at 1

        if (number > 0)
        {
            newNumber = number - 1;
        }
        else
        {
            newNumber = 0;
        }
        //If player clicks bomb, lose game
        if (Buttons[newNumber].tag == "MatchBomb")
        {
            matchComparisonNumber = 0;
            matchComparison = "";
            matchCounter = 0;
            EndGame();
        }
        else
        {
            //IF THE PLAYER HAS NOT CLICKED A BUTTON YET/DOES NOT HAVE A TILE STORED TO COMPARE
            if (matchComparison == "")
            {
                matchComparison = Buttons[newNumber].tag;
                matchComparisonNumber = number;
                //DISABLES BUTTON SO PLAYER CANNOT MATCH THE SAME BUTTON THEY JUST CLICKED
                Buttons[newNumber].GetComponent<Image>().color = Color.grey;
                Buttons[newNumber].GetComponent<Button>().interactable = false;
            }
            else
            {
                //IF IS A MATCH
                if (matchComparison == Buttons[newNumber].tag)
                {
                    if (matchComparisonNumber > 0)
                    {
                        newMCnumber = matchComparisonNumber - 1;
                    }
                    //DISABLES BUTTONS THAT HAVE JUST BEEN MATCHED
                    Buttons[newMCnumber].GetComponent<Image>().color = Color.grey;
                    Buttons[newMCnumber].GetComponent<Button>().interactable = false;
                    Buttons[newNumber].GetComponent<Image>().color = Color.grey;
                    Buttons[newNumber].GetComponent<Button>().interactable = false;
                    matchComparisonNumber = 0;
                    matchComparison = ""; //TAKES OUT CURRENTLY STORED TILE FOR COMPARISON
                    matchCounter++; //INCREMENTS TRACKER OF HOW MANY MATCHES THERE HAVE BEEN
                    sm.LoadAudioClip_SFX("Cute GUI Sound Set/Correct");
                    sm.PlaySFX();
                    //ROUND WIN CONDITION: If there have been 4 matches, move to the next round
                    if (matchCounter == 4)
                    {
                        matchCounter = 0;
                        NextRound();
                    }
                }
                else
                {
                    sm.LoadAudioClip_SFX("Cute GUI Sound Set/Wrong");
                    sm.PlaySFX();
                    matchComparisonNumber = 0;
                    matchComparison = "";
                    matchCounter = 0;
                    EndGame();
                }
            }
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
            if (currentGamemode == gamemodeNames[3])
            {
                EnableButtons(true);
                Buttons[number].GetComponent<Button>().image.color = Color.red;
            }

            yield return new WaitForSeconds(time_buttonsAreColour);
            //Uncolour button
            if (currentGamemode != gamemodeNames[3])
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
        if (gamemode != "") //Used when game has been chosen for the first time
        {
            currentGamemode = gamemode;
        }
        Debug.Log(currentGamemode);
        //Disable Play Again and Return buttons
        endgamePanel.SetActive(false);
        sPatternAnswer = "";
        sPatternNumbers = "";
        iPatternNumbers = 0;
        tAfterGame.text = "";
        newHighscoreThisGame = false;
        AddToButtonPressed();
        Debug.Log(currentGamemode + "," + gamemodeNames[3]);

        if (currentGamemode == gamemodeNames[3])
        {
            fTimedRoundTimer = fTimedRoundLength;
        }
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
    
    public void AddToButtonPressed()
    {
        buttonsPressed += 1;
        //Saves to increase buttonPressed each time
        Save();
    }

    
    void CheckHighscore() //Used midgame to check whether a highscore has been exceeded
    {
        if (currentGamemode == gamemodeNames[0]) //Classic
        {
            if (iPatternNumbers - 1 > Highscore[0]) //If new highscore
            {
                //New highscore!
                Highscore[0] = iPatternNumbers - 1;
                newHighscoreThisGame = true;
            }
            
        }
        else if (currentGamemode == gamemodeNames[1]) //Random
        {
            if (iPatternNumbers - 1 > Highscore[1]) //If new highscore
            {
                //New highscore!
                Highscore[1] = iPatternNumbers - 1;
                newHighscoreThisGame = true;
            }
            
        }
        else if (currentGamemode == gamemodeNames[2]) //Game 3
        {
            if (iPatternNumbers - 1 > Highscore[2]) //If new highscore
            {
                //New highscore!
                Highscore[2] = iPatternNumbers - 1;
                newHighscoreThisGame = true;
            }
            
        }
        else if (currentGamemode == gamemodeNames[3]) //Timed Round
        {
            if (iPatternNumbers - 1 > Highscore[3]) //If new highscore
            {
                //New highscore!
                Highscore[3] = iPatternNumbers - 1;
                newHighscoreThisGame = true;
            }
            
        }
        else
        {
            Debug.Log("Error - invalid gamemode");
            newHighscoreThisGame = false;
        }
    }
    
    void CheckHighscoreAtEndOfGame() //Used to add the AfterGame text depending on if the user has achieved a new highscore
    {
        if (newHighscoreThisGame == true)
        {
           //New highscore!
            tAfterGame.text = "NEW HIGHSCORE!";
        }
        else
        {
            tAfterGame.text = "Game Over";
        }
    }

    /////Game Streak Stuff
    public int GetGameStreak()
    {
        return gameStreak;
    }

    public int GetGameStreakHighscore()
    {
        return gameStreakHighscore;
    }
    
    void SetGameStreak(int gs, int gshs) //Used when loading in from Save data
    {
        gameStreak = gs;
        gameStreakHighscore = gshs;
    }

    public void AddToGameStreak() //Increments the streak
    {
        gameStreak++;
        ChangeGameStreakHighscore();
    }

    public void ResetGameStreak() 
    {
        gameStreak = 1;
    }

    public void ChangeGameStreakHighscore() //Called after gameStreak changes
    {
        if (gameStreak > gameStreakHighscore)
        {
            gameStreakHighscore = gameStreak;
        }
    }

    public void CheckGameStreak(bool gameOpened) //Called when game is opened to check streak
    {
        //For hours
        DateTime dLastAcquired = dateLastAcquiredStreak;
        //For days
        DateTime dLastAcquiredDateOnly = dLastAcquired.Date;
        int minutes = (DateTime.Now - dLastAcquired).Minutes;
        int hours = (int)(DateTime.Now - dLastAcquired).TotalHours;
        int days = (int)(DateTime.Now.Date - dLastAcquiredDateOnly).TotalDays;
        //Debug.Log("Total hours:" + hours + ", Total Days:" + days);
        //If X is less than 6 hours AND day is 1 apart
        //  Come back in X
        if (days < 2) //If Days are less than 2
        {
            if (hours < 6) //Player has returned to play
            {
                //Debug.Log("No new streak, too few hours. Come back later");
                if (gameOpened) //Game just opened
                { 
                    mm.DisplayDailyMessageAbleToGetStreak("Welcome back", "Play to beat your scores!", "Wicked");
                }
                //come back later
            }
            else if (hours > 6 && days > 0) //Player can play and acquire a streak
            {
                if(gameOpened) //Game just opened
                { 
                    mm.DisplayDailyMessageAbleToGetStreak("Streak: " + gameStreak.ToString(), "GO GET STREAK", "Yes dad");
                }
            }
        }
        else if (days > 2) //Too late
        {
            //  Too late
            //  Save streak as last streak
            gameStreakLast = gameStreak;
            dateLastAcquiredStreakLast = dateLastAcquiredStreak;

            //  No chance to reclaim
            //  Streak reset to zero
            gameStreak = 0;
            if (gameOpened) //Game just opened
            { 
                mm.DisplayDailyMessageAbleToGetStreak("Streak: " + gameStreak.ToString(), "You lost streak " + days.ToString() + " days ago.", "I am shit");
            }
        }
        else if (days > 1) //Can still reclaim streak
        {
            //  SAVE STREAK AS LAST STREAK
            gameStreakLast = gameStreak;
            dateLastAcquiredStreakLast = dateLastAcquiredStreak;
            //  Streak reset to zero
            gameStreak = 0;
            //  OFFER CHANCE TO RECLAIM STREAK
        }
        Save();
    }

    public void ChangeGameStreak() //Called when a game has been played
    {
        if(gameStreak == 0) //Commonly when game is played for the first time
        {
            gameStreak = 1;
            dateLastAcquiredStreak = DateTime.Now;
            Debug.Log(dateLastAcquiredStreak);
            Save();
        }
        else
        {
            //For hours
            DateTime dLastAcquired = dateLastAcquiredStreak;
            //For days
            DateTime dLastAcquiredDateOnly = dLastAcquired.Date;
            int minutes = (DateTime.Now - dLastAcquired).Minutes;
            int hours = (int)(DateTime.Now - dLastAcquired).TotalHours;
            int days = (int)(DateTime.Now.Date - dLastAcquiredDateOnly).TotalDays;
            Debug.Log("Total hours:" + hours + ", Total Days:" + days);
            //Debug.Log(hours + "," + days + "," + minutes);

            if (days < 2) //If Days are less than 2
            {
                if (hours < 6)
                {
                    Debug.Log("No new streak, too few hours. Come back later");
                    //come back later
                }
                else if (hours > 6 && days > 0)
                {
                    Debug.Log("NEW STREAK!");
                    gameStreak++;
                    dateLastAcquiredStreak = DateTime.Now;
                }
                else
                {
                    //NEED TO CHECK IF THE LOCAL DATE IS DIFFERENT
                    //Right now it does not check 15th vs 14th, only if it has been one
                    //day since last recorded
                    //Needs to check date, not time between
                    //Currently is just a stopgap
                }
            }
            //These two methods shouldn't necessarily be called because they will be called when game is opened.
            else if (days > 2)
            {
                Debug.Log("sTREAK LOST");
                gameStreakLast = gameStreak;
                dateLastAcquiredStreakLast = dateLastAcquiredStreak;
                gameStreak = 0;
            }
            else if (days > 1)
            {
                Debug.Log("Streak lost but reclaim-able");
                gameStreakLast = gameStreak;
                dateLastAcquiredStreakLast = dateLastAcquiredStreak;
                gameStreak = 0;
            }
            
        }
        Save();
        //If time between now and last acquired streak is less than 6 hours
        //Can ignore
        //Else If time between
    }



}
