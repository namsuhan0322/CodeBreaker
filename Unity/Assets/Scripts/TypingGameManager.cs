using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TypingGameManager : MonoBehaviour
{
    public InputField inputField;

    private void Update()
    {
        if (!GameManager.Instance.isGameRunning) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckAnswer();
        }
    }

    void CheckAnswer()
    {
        string input = inputField.text;
        GameWord currentWord =
            GameDataManager.Instance.gameWords[GameManager.Instance.currentRound - 1];

        bool isCorrect = input == currentWord.CorrectWord;

        NetworkManager.Instance.SendAnswer(input, isCorrect);

        inputField.text = "";
        GameManager.Instance.NextRound();
    }
}
