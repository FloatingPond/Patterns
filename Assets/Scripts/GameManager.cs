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
    public float fTimedRoundTimer; //Timer for Timed Rounds game (Game 3)
    public float fGameStopwatch; //Stopwatch for all games
    public float fTimedRoundLength = 60;
    public int iTimedRoundWrongButtonsPressed = 0;

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
    public GameObject gamePanel;
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

    public DateTime dateStreakHighscore;
    
    [Title("Managers")]
    //Ad Manager
    public AdManager am;

    //Sound manager
    public SoundManager sm;
    
    public MessageManager mm;

    public AchievementManager achm;


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
        CheckGameStreak(false);
    }

    [Button(ButtonSizes.Small)]
    [PropertyOrder(2)]
    private void GetDateLastAcquiredStreak()
    {
        dtString4 = dateLastAcquiredStreak.ToString("yyyy-MM-dd HH:mm tt");
    }

    //Newest debugging for streak stuff
    [Title("Buttons and Debug")]
    [PropertyOrder(4)]
    public string sTestOne;
    [PropertyOrder(4)]
    public string sTestTwo;

    public DateTime dtTestOne;
    public DateTime dtTestTwo;

    [PropertyOrder(4)]

    [Button(ButtonSizes.Small)]
    private void SetdtTestOneToNow()
    {
        sTestOne = DateTime.Now.ToString("yyyy-MM-dd HH:mm tt");
    }
    [PropertyOrder(4)]

    [Button(ButtonSizes.Small)]
    private void SetdtTestTwoToNow()
    {
        sTestTwo = DateTime.Now.ToString("yyyy-MM-dd HH:mm tt");
    }
    [PropertyOrder(4)]

    [Button(ButtonSizes.Small)]
    private void TestCheckStreakButton()
    {
        dtTestOne = StringToDateTime(sTestOne);
        dtTestTwo = StringToDateTime(sTestTwo);

        TestCheckStreak(dtTestOne, dtTestTwo);
    }
    [PropertyOrder(4)]


    //End of variables and buttons

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
        if (currentGamemode == gamemodeNames[3]) //Countdown timer
        {
            if (endgamePanel.activeSelf == false)
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
        if (endgamePanel.activeSelf == false && gamePanel.activeSelf == true)
        {
            fGameStopwatch += Time.deltaTime;
        }
    }

    public bool LoadGame() //Called in Awake - loads data from load system and saves into memory
    {
        PlayerData data = SaveSystem.LoadGame();
        if (data != null) //Date exists
        { 
            //Highscore
            Highscore[0] = data.highscores[0];
            Highscore[1] = data.highscores[1];
            Highscore[2] = data.highscores[2];
            Highscore[3] = data.highscores[3];

            //Streak
            gameStreak = data.gameStreak;
            dateLastAcquiredStreak = DateTime.ParseExact(data.dateLastAcquiredStreak, "yyyy-MM-dd HH:mm tt", null);

            gameStreakLast = data.gameStreakLast;
            dateLastAcquiredStreakLast = DateTime.ParseExact(data.dateLastAcquiredStreakLast, "yyyy-MM-dd HH:mm tt", null);
            
            gameStreakHighscore = data.gameStreakHighscore;
            dateStreakHighscore = DateTime.ParseExact(data.dateLastAcquiredStreak, "yyyy-MM-dd HH:mm tt", null);

            fGameTime = data.secondsPlayed;
            buttonsPressed = data.buttonsPressed;
            
            am.SetadsRewardsWatched(data.adsRewardsWatched); 

            sm.masterFloat = data.master;
            sm.musicFloat = data.music;
            sm.SFX_Float = data.sfx;
            sm.voiceFloat = data.voice;

            am.dtLastTimeRewardAdWatched = DateTime.ParseExact(data.dateLastRewardAdWatched, "yyyy-MM-dd HH:mm tt", null);
            CheckGameStreak(true); //Check Streak and display message depending on said streak
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
            gameStreakLast = 0;
            gameStreakHighscore = 0;
            
            am.dtLastTimeRewardAdWatched = DateTime.ParseExact("2001-01-01 12:00 PM", "yyyy-MM-dd HH:mm tt", null); //Set far into the past to ensure that it does not break later
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
            int number = UnityEngine.Random.Range(1, 9); //Returns random number between 1 and 9
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
        if ((currentGamemode == gamemodeNames[0]) || (currentGamemode == gamemodeNames[1])) //If Game 1 or 2
        { 
            ////Check if whole thing is correct
            if (sPatternAnswer == sPatternNumbers) //Correct!
            {
                AddToButtonPressed();
                NextRound();
            }
            else //Either partially or not correct
            {
                AddToButtonPressed();
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
        else if (currentGamemode == gamemodeNames[2]) //Counts button
        {
            AddToButtonPressed();
        }
        else if (currentGamemode == gamemodeNames[3]) //Timed round
        {
            if (sPatternAnswer == sPatternNumbers) //Correct!
            {
                StopCoroutine(coroutine);
                AddToButtonPressed();
                NextRound();
            }
            else //Nothing, game does not stop. Player has unlimited tries in this game mode
            {
                sPatternAnswer = "";
                //Does not count buttons pressed for wrong ones
                iTimedRoundWrongButtonsPressed += 1; //Counts player pressing wrong button - used for achievement
                //Try again
            }
        }
        else
        {
            //Error - some reason the currentGamemode is not any of them or is an invalid one
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
        //Call Achievement Checker for achievements that have things such as timers
        //Called here because after next round set up, numbers are most active
        achm.UnlockGameplayAchievementDuringActiveGame(currentGamemode, iPatternNumbers - 1, fGameStopwatch, this);
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

        //Check achievements here
        achm.UnlockGameplayAchievement(currentGamemode, iPatternNumbers - 1, fGameStopwatch, this);

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
    
    public void ButtonPressed(int number) //Called when a button is pressed
    {
        if (!isGameButtonsDisabled)
        {
            sPatternAnswer += number.ToString();
            if (currentGamemode != gamemodeNames[2]) //If not matching game
            {
                CheckAnswer();
            }
            else if (currentGamemode == gamemodeNames[2]) //If is the matching game
            {
                CheckMatchAnswer(number);
            }
        }
    }
    
    void CheckMatchAnswer(int number) //USED FOR MATCH GAME MODE
    {
        AddToButtonPressed();
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
        else //No currentGamemode - possibly due to EndGame is also called when Returning to main menu
        {
            Debug.Log("Error in high score text setting");
        }

    }

    IEnumerator AnimateButtonColours()
    {
        for (int i = 0; i < sPatternNumbers.Length; i++)
        {
            if (i == sPatternNumbers.Length - 1 && currentGamemode == "classic") //Last number in the length - shorter uncolouring time for quicker gameplay
            {
                yield return new WaitForSeconds(time_buttonsTimeBetween); //Time between uncoloured button and coloured button
            }
            else if (currentGamemode == "timedround")
            {
                //Don't wait to uncolour
            }
            else
            {
                float timeAddedToRetract = sPatternNumbers.Length / 200f;
                Debug.Log(timeAddedToRetract);
                yield return new WaitForSeconds(time_buttonsTimeBetween - timeAddedToRetract); //Time between uncoloured button and coloured button
            }
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
        //Disable Play Again and Return buttons
        endgamePanel.SetActive(false);
        sPatternAnswer = "";
        sPatternNumbers = "";
        iPatternNumbers = 0;
        iTimedRoundWrongButtonsPressed = 0;
        tAfterGame.text = "";
        newHighscoreThisGame = false;
        //Reset Stopwatch
        fGameStopwatch = 0;
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
    
    public void AddToButtonPressed() //A correct button press being counted then saved
    {
        buttonsPressed += 1; //Increments
        //If enough button presses, achievements!
        if (buttonsPressed == 1000) //Checks from 1000
        {
            achm.UnlockButtonAchivement(buttonsPressed);
        }
        if (buttonsPressed == 9000) //Checks from 1000
        {
            achm.UnlockButtonAchivement(buttonsPressed);
        }
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

    public int GetGameStreakLast()
    {
        return gameStreakLast;
    }

    public int GetGameStreakHighscore()
    {
        return gameStreakHighscore;
    }

    public void AddToGameStreak() //Increments the streak
    {
        gameStreak++;
        dateLastAcquiredStreak = DateTime.Now;
        ChangeGameStreakHighscore();
    }
    
    public void ChangeGameStreakHighscore() //Called after gameStreak changes
    {
        if (gameStreak > gameStreakHighscore)
        {
            gameStreakHighscore = gameStreak;
            dateStreakHighscore = dateLastAcquiredStreak;
        }
    }

    public void CheckGameStreak(bool showMessages) //Called in LoadGame when game is opened to check streak
    {
        //For hours
        DateTime dLastAcquired = dateLastAcquiredStreak;
        //For days
        DateTime dLastAcquiredDateOnly = dLastAcquired.Date;
        int minutes = (DateTime.Now - dLastAcquired).Minutes;
        int hours = (int)(DateTime.Now - dLastAcquired).TotalHours;
        int days = (int)(DateTime.Now.Date - dLastAcquiredDateOnly).TotalDays;

        String dtString = dLastAcquired.ToString("dd-MM-yyyy");
        if (dtString.Contains("01-01-0001")) //First time playing - usually when player opens and closes game without getting streak
        {
            mm.DisplayWelcomeMessage();
        }

        else if (days > 2) //Too late
        {
            //  Too late
            //  Save streak as last streak
            Debug.Log("Too late, even to save the streak");
            SaveFormerStreak();
            //  No chance to reclaim
            //  Streak reset to zero
            gameStreak = 0;

            if (showMessages) //Game just opened
            {
                mm.DisplayDailyMessageAbleToGetStreak("Streak: " + gameStreak.ToString(), "You lost your streak " + days.ToString() + " days ago.", "A shame");
            }
        }
        else if (days > 1) //Can still reclaim streak
        {
            Debug.Log("Streak lost but time to reclaim");
            //  SAVE STREAK AS LAST STREAK
            SaveFormerStreak();
            //  Streak reset to zero
            gameStreak = 0;
            //  OFFER CHANCE TO RECLAIM STREAK
            if (showMessages) //Game just opened
            {
                //IF HAS PREMIUM AD FREE
                //Streak Saved
                //Else
                //Show this window
                mm.DisplayAbleToWatchRewardAd("Want a free streak?", "Watch a reward ad to claim your streak back!", "Yes Please!", "No thanks!");
            }

        }
        else if (days == 1 && hours >= 6) //Can get streak
        {
            Debug.Log("Play to acquire streak");

            if (showMessages) //Game just opened
            {
                mm.DisplayDailyMessageAbleToGetStreak("Streak: " + gameStreak.ToString(), "Play to increase your streak!", "On it");
            }

        }
        else if (days < 1) //Day early
        {
            Debug.Log("Come back tomorrow for streak");

            if (showMessages) //Game just opened
            {
                mm.DisplayDailyMessageAbleToGetStreak("Welcome back", "Play to beat your scores!", "Okay");
            }
        }
        else if (hours < 6) //Needs to wait longer
        {
            if (showMessages) //Game just opened
            {
                mm.DisplayDailyMessageAbleToGetStreak("Welcome back", "Play to beat your scores!", "Okay");
            }
        }
        //Time traveller
        else if (hours < 0 || minutes < 0)
        {
            Debug.Log("begone time traveller");
            return;
        }
        CheckStreakHighscore(gameStreak, dateLastAcquiredStreak);
        CheckStreakHighscore(gameStreakLast, dateLastAcquiredStreakLast);
        //Ultimately save
        Save();
    }

    private void SaveFormerStreak() //Saves the current streak as the last streak aquired
    {
        gameStreakLast = gameStreak; //Sets the player's last streak as the current streak before it is reset
        dateLastAcquiredStreakLast = dateLastAcquiredStreak; //Sets the date as the date the player acquired their last streak
    }

    private void CheckStreakHighscore(int streak, DateTime dt)
    {
        if (streak > gameStreakHighscore)
        {
            gameStreakHighscore = streak;
            dateStreakHighscore = dt;
        }
    }

    public void ChangeGameStreak() //Called when a game has been played
    {
        if(gameStreak == 0) //Commonly when game is played for the first time or player has lost streak
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

            if (hours > 6 && days == 1)
            {
                Debug.Log("NEW STREAK!");
                AddToGameStreak();
            }
        }
        Save();
    }

    //TESTING FOR STREAK
    public void TestCheckStreak(DateTime dtTestNow, DateTime dtTestLastAcquired)
    {
        //For hours
        DateTime dLastAcquired = dtTestLastAcquired;
        //For days
        DateTime dLastAcquiredDateOnly = dtTestLastAcquired.Date;

        int minutes = (dtTestNow - dLastAcquired).Minutes;
        int hours = (int)(dtTestNow - dLastAcquired).TotalHours;
        int days = (int)(dtTestNow - dLastAcquiredDateOnly).TotalDays;
        Debug.Log("Days: " + days + ", hours: " + hours + ", minutes: " + minutes);


        if (days > 2) //Too late
        {
            //  Too late
            //  Save streak as last streak
            Debug.Log("Too late, even to save the streak");
            gameStreakLast = gameStreak; //Sets the player's last streak as the current streak before it is reset
            dateLastAcquiredStreakLast = dateLastAcquiredStreak; //Sets the date as the date the player acquired their last streak
            //  No chance to reclaim
            //  Streak reset to zero
            gameStreak = 0;

            
                mm.DisplayDailyMessageAbleToGetStreak("Streak: " + gameStreak.ToString(), "You lost your streak " + days.ToString() + " days ago.", "A shame");
            
        }
        else if (days > 1) //Can still reclaim streak
        {
            Debug.Log("Streak lost but time to reclaim");
            //  SAVE STREAK AS LAST STREAK
            gameStreakLast = gameStreak; //Sets the player's last streak as the current streak before it is reset
            dateLastAcquiredStreakLast = dateLastAcquiredStreak; //Sets the date as the date the player acquired their last streak
            //  Streak reset to zero
            gameStreak = 0;
            //  OFFER CHANCE TO RECLAIM STREAK
            
                mm.DisplayAbleToWatchRewardAd("Want a free streak?", "Watch a reward ad to claim your streak back!", "Yes Please!", "No thanks!");
            

        }
        else if (days == 1 && hours >= 6) //Can get streak
        {
            Debug.Log("Play to acquire streak");

            
                mm.DisplayDailyMessageAbleToGetStreak("Streak: " + gameStreak.ToString(), "Play to increase your streak!", "On it");
            

        }
        else if (days < 1) //Day early
        {
            Debug.Log("Come back tomorrow for streak");

            mm.DisplayDailyMessageAbleToGetStreak("Welcome back", "Play to beat your scores!", "Okay");
            
        }
        else if (hours < 6) //Needs to wait longer
        {
            Debug.Log("come back later for streak.");
        }
        //Time traveller
        else if (hours < 0 || minutes < 0)
        {
            Debug.Log("begone time traveller");
            return;
        }


    }

    public void TestCheckStreak2(DateTime dtTestNow, DateTime dtTestLastAcquired)
    {
        //For hours
        DateTime dLastAcquired = dtTestLastAcquired;
        //For days
        DateTime dLastAcquiredDateOnly = dtTestLastAcquired.Date;

        int minutes = (dtTestNow - dLastAcquired).Minutes;
        int hours = (int)(dtTestNow - dLastAcquired).TotalHours;
        int days = (int)(dtTestNow - dLastAcquiredDateOnly).TotalDays;
        Debug.Log("Days: " + days + ", hours: " + hours + ", minutes: " + minutes);

        //Time traveller
        if (hours < 0 || minutes < 0)
        {
            Debug.Log("begone time traveller");
            return;
        }

        if (days < 2) //If Days are less than 2
        {
            if (hours < 6 || days == 0) //Player has returned to play
            {
                //come back later
                Debug.Log("come back later - " + hours + " hours, "+days+" days");
            }
            else if (hours >= 6 && days > 0) //Player can play and acquire a streak
            {
                //Player can play and acquire a streak
                Debug.Log("Play to acquire streak");
            }
        }
        else if (days > 2) //Too late
        {
            //  Too late
            //  Save streak as last streak
            Debug.Log("Too late, even to save the streak");
            //  No chance to reclaim
            //  Streak reset to zero

        }
        else if (days > 1) //Can still reclaim streak
        {
            //  SAVE STREAK AS LAST STREAK
            //  Streak reset to zero
            //  OFFER CHANCE TO RECLAIM STREAK
            Debug.Log("Streak lost but time to reclaim");
        }
    }

    public DateTime StringToDateTime(String sDateTime)
    {
        DateTime dt = DateTime.ParseExact(sDateTime, "yyyy-MM-dd HH:mm tt", null); //Set far into the past
        return dt;
    }




}
