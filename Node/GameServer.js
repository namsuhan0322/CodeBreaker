const express = require("express");
const mysql = require("mysql2/promise");
const app = express();

require("dotenv").config();
const host = process.env.DB_HOST;
const user = process.env.DB_USER;
const password = process.env.DB_PASSWARD;
const database = process.env.DB_DATABASE;
const port = process.env.PORT;

app.use(express.json());

const pool = mysql.createPool({
  host,
  user,
  password,
  database,
});

// 테스트 게임 단어 조회
app.get("/gameword/:wordId", async (req, res) => {
  try {
    const [gameword] = await pool.query(
      "SELECT id, room_id, round_number, scrambled_word, correct_word FROM game_words WHERE id = ?",
      [req.params.wordId]
    );
    console.log(gameword);
    res.json(gameword);
  } catch (error) {
    res.status(500).json({ success: false, message: error.message });
  }
});

app.listen(port || 4000, () => {
  console.log(`서버 실행 중 : ${port}`);
});
