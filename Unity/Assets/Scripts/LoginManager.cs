using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections;
[System.Serializable]
public class LoginData
{
    public string username;
    public string password;
}

public class LoginManager : MonoBehaviour
{
    public InputField idInput;
    public InputField pwInput;
    public Text messageText;

    private string URL = "http://localhost:3000";

    public void OnLogin()
    {
        StartCoroutine(LoginRequest());
    }

    IEnumerator LoginRequest()
    {
        LoginData data = new LoginData()
        {
            username = idInput.text,
            password = pwInput.text
        };

        string json = JsonConvert.SerializeObject(data);

        using (UnityWebRequest www =
            new UnityWebRequest($"{URL}/login", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                messageText.text = "로그인 성공";

                // 필요하면 여기서 GameDataManager에 유저 저장
                // GameDataManager.Instance.CurrentPlayer = ...

                SceneManager.LoadScene("LobbyScene");
            }
            else
            {
                messageText.text = "로그인 실패";
            }
        }
    }
}
