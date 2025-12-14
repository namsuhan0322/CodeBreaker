using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Text wordText;
    public Text roundText;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowWord(string word)
    {
        wordText.text = word;
        roundText.text = $"Round {GameManager.Instance.currentRound}";
    }
}

