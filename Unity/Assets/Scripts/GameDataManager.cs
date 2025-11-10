using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class GameDataManager : MonoBehaviour
{
    private string URL = "http://localhost:3000";

    public User CurrentPlayer { get; private set; }
    public Room CurrentRoom { get; private set; }
    public GameWord CurrentWord { get; private set; } // 테스트 용
    public GameResult CurrentGameResult { get; private set; }

    [Header("데이터 리스트")]
    public List<Room> rooms = new List<Room>();
    public List<RoomPlayer> roomPlayers = new List<RoomPlayer>();
    public List<GameWord> gameWords = new List<GameWord>();
    public List<RoundAnswer> roundAnswers = new List<RoundAnswer>();
    public List<GameResult> matchHistory = new List<GameResult>();

    [Tooltip("이벤트 (Event Delegates)")]
    // 로비의 전체 방 목록이 업데이트될 때
    public delegate void OnRoomListUpdateHandler(List<Room> rooms);
    public event OnRoomListUpdateHandler OnRoomListUpdate;
    // 현재 입장한 방의 플레이어 목록이 변경될 때
    public delegate void OnRoomPlayersUpdateHandler(List<RoomPlayer> players);
    public event OnRoomPlayersUpdateHandler OnRoomPlayersUpdate;
    // 게임이 시작되고 단어 목록(문제)을 받았을 때
    public delegate void OnGameStartHandler(List<GameWord> words);
    public event OnGameStartHandler OnGameStart;
    // 누군가 정답을 제출하는 등 답변 목록이 갱신될 때
    public delegate void OnRoundAnswersUpdateHandler(List<RoundAnswer> answers);
    public event OnRoundAnswersUpdateHandler OnRoundAnswersUpdate;
    // 게임이 종료되고 최종 결과를 받았을 때
    public delegate void OnGameResultHandler(GameResult result);
    public event OnGameResultHandler OnGameResult;
    // 현재 플레이어의 정보가 갱신될 때 (예: 점수 갱신)
    public delegate void OnPlayerUpdateHandler(User player);
    public event OnPlayerUpdateHandler OnPlayerUpdate;
    // 매치 히스토리(전적) 목록을 불러왔을 때
    public delegate void OnMatchHistoryUpdateHandler(List<GameResult> history);
    public event OnMatchHistoryUpdateHandler OnMatchHistoryUpdate;

    private void Start()
    {
        CurrentWord = new GameWord();
        CurrentWord.Id = 1;

        StartCoroutine(GetGameWords());
    }

    // 테스트 게임 단어 조회
    private IEnumerator GetGameWords()
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{URL}/gameword/{CurrentWord.Id}"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                gameWords = JsonConvert.DeserializeObject<List<GameWord>>(www.downloadHandler.text);
                Debug.Log("게임 단어들 : ");
                foreach (GameWord word in gameWords)
                {
                    Debug.Log($" - {word.Id}, {word.RoomId}, {word.RoundNumber}, {word.ScrambledWord}, {word.CorrectWord}");
                }

                OnGameStart?.Invoke(gameWords);
            }
            else
            {
                Debug.LogError($"게임 단어 조회 실패 : {www.error}");
            }
        }
    }

    public GameWord GetGameWords(int wordId)
    {
        return gameWords.Find(gameword => gameword.Id == wordId);
    }
}
