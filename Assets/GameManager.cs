using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public int iPatternNumbers = 0;
    public int Highscore = 0;
    public string sPatternNumbers;
    public string sPatternAnswer;

    public TMP_Text tScore, tHighscore;

    //Buttons
    public List<GameObject> Buttons = new List<GameObject>();
    //Used to enable/disable user able to press buttons - mainly when showing patterns and when not in-game
    public bool isGameButtonsDisabled = false;


    //Button Animation

    public float time_buttonsAreColour = 0.5f;
    public float time_buttonsTimeBetween = 0.25f;


    void Start()
    {
        GeneratePattern();
    }
    void GeneratePattern()
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
        GeneratePattern();
    }
    void EndGame()
    {
        
        
        
        //Show score
        if (iPatternNumbers > Highscore) //If new highscore
        {
            //New highscore!
            Highscore = iPatternNumbers;
        }
        else
        {
            //bleh
        }
        //Play again?
        sPatternAnswer = "";
        iPatternNumbers = 0;
        NextRound();
    }
    
    //Called when a button is pressed
    public void ButtonPressed(int number)
    {
        if (!isGameButtonsDisabled)
        { 
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

    void EnableButtons(bool state)
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

}
