using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Text roomNameText;
    public Button joinButton;

    private int roomId;

    public void Setup(Room room)
    {
        roomId = room.Id;
        roomNameText.text = $"¹æ {room.Id}";

        joinButton.onClick.RemoveAllListeners();
        joinButton.onClick.AddListener(() =>
        {
            StartCoroutine(GameDataManager.Instance.JoinRoom(roomId));
        });
    }
}
