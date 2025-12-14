using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentRound = 0;
    public int totalRounds = 5;
    public bool isGameRunning = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameDataManager.Instance.OnGameStart += StartGame;
    }

    void StartGame(List<GameWord> words)
    {
        isGameRunning = true;
        currentRound = 0;
        NextRound();
    }

    public void NextRound()
    {
        if (currentRound >= totalRounds)
        {
            EndGame();
            return;
        }

        GameWord word = GameDataManager.Instance.gameWords[currentRound];
        UIManager.Instance.ShowWord(word.ScrambledWord);

        currentRound++;
    }

    void EndGame()
    {
        isGameRunning = false;
        NetworkManager.Instance.SendGameResult();
    }
}

