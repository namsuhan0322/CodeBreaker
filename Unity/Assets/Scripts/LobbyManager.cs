using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public InputField roomNameInput;
    public Transform roomListContent;
    public GameObject roomItemPrefab;

    private void Start()
    {
        GameDataManager.Instance.OnRoomListUpdate += UpdateRoomList;
    }

    public void CreateRoom()
    {
        string roomName = roomNameInput.text;

        if (string.IsNullOrEmpty(roomName))
        {
            Debug.LogWarning("방 이름을 입력하세요.");
            return;
        }

        StartCoroutine(GameDataManager.Instance.CreateRoom(roomName));
    }

    public void FindMatch()
    {
        StartCoroutine(GameDataManager.Instance.FindMatch());
    }

    private void UpdateRoomList(System.Collections.Generic.List<Room> rooms)
    {
        foreach (Transform child in roomListContent)
            Destroy(child.gameObject);

        foreach (Room room in rooms)
        {
            GameObject item = Instantiate(roomItemPrefab, roomListContent);
            item.GetComponent<RoomItem>().Setup(room);
        }
    }
}
