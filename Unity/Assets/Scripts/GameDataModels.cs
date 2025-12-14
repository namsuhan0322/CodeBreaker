using System;
using Newtonsoft.Json;

public enum RoomStatus { waiting, playing, finished }

[Serializable]
public class User
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("score")]
    public int Score { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }
}

[Serializable]
public class Room
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("room_code")]
    public string RoomCode { get; set; }

    [JsonProperty("status")]
    public RoomStatus Status { get; set; }

    [JsonProperty("host_id")]
    public int HostId { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }
}

[Serializable]
public class RoomPlayer
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("room_id")]
    public int RoomId { get; set; }

    [JsonProperty("user_id")]
    public int UserId { get; set; }

    [JsonProperty("seat_number")]
    public int SeatNumber { get; set; }

    [JsonProperty("joined_at")]
    public string JoinedAt { get; set; }
}

[Serializable]
public class GameResult
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("room_id")]
    public int RoomId { get; set; }

    [JsonProperty("winner_id")]
    public int WinnerId { get; set; }

    [JsonProperty("score_user1")]
    public int ScoreUser1 { get; set; }

    [JsonProperty("score_user2")]
    public int ScoreUser2 { get; set; }

    [JsonProperty("ended_at")]
    public string EndedAt { get; set; }
    public int Score;
}

[Serializable]
public class GameWord
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("room_id")]
    public int RoomId { get; set; }

    [JsonProperty("round_number")]
    public int RoundNumber { get; set; }

    [JsonProperty("scrambled_word")]
    public string ScrambledWord { get; set; }

    [JsonProperty("correct_word")]
    public string CorrectWord { get; set; }

    [JsonProperty("started_at")]
    public string StartedAt { get; set; }

    [JsonProperty("ended_at")]
    public string EndedAt { get; set; }
}

[Serializable]
public class RoundAnswer
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("round_id")]
    public int RoundId { get; set; }

    [JsonProperty("user_id")]
    public int UserId { get; set; }

    [JsonProperty("answer")]
    public string Answer { get; set; }

    [JsonProperty("is_correct")]
    public bool IsCorrect { get; set; }

    [JsonProperty("answered_at")]
    public string AnsweredAt { get; set; }
}