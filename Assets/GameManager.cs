using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int iPatternNumbers = 0;
    public string sPatternNumbers;
    public string sPatternAnswer;

    void Start()
    {
        GeneratePattern();
    }

    void Update()
    {
        
    }
    void GeneratePattern()
    {
        iPatternNumbers++;
        sPatternNumbers = "";
        for (int i = 0; i < iPatternNumbers; i++)
        {
            int number = Random.Range(1, 9);
            sPatternNumbers += number.ToString();
        }
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
        GeneratePattern();
    }
    void EndGame()
    {
        //If new highscore
        //New highscore!
        //Else
        //Show score

        //Play again?
        sPatternAnswer = "";
        iPatternNumbers = 0;
        NextRound();
    }
    public void ButtonPressed(int number)
    {
        sPatternAnswer += number.ToString();
        CheckAnswer();
    }
}
