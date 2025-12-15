require('dotenv').config();

const express = require('express');
const mysql = require('mysql2/promise');
const jwt = require('jsonwebtoken');

const app = express();
app.use(express.json());

const pool = mysql.createPool({
  host: process.env.DB_HOST,
  user: process.env.DB_USER,
  password: process.env.DB_PASSWORD,
  database: process.env.DB_DATABASE,
});

const JWT_SECRET = process.env.JWT_SECRET;
const REFRESH_TOKEN_SECRET = process.env.REFRESH_TOKEN_SECRET;
const PORT = Number(process.env.PORT) || 3000;

const refreshTokenStore = {}; 

function signAccessToken(payload) {
  return jwt.sign(payload, JWT_SECRET, { expiresIn: '15m' });
}

function signRefreshToken(payload) {
  return jwt.sign(payload, REFRESH_TOKEN_SECRET, { expiresIn: '7d' });
}

function generateRoomCode() {
  const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
  let code = '';
  for (let i = 0; i < 5; i++) {
    code += chars.charAt(Math.floor(Math.random() * chars.length));
  }
  return code;
}

app.post('/login', async (req, res) => {
  const { username, password } = req.body;

  try {
    const [rows] = await pool.query(
      'SELECT user_id, username, password FROM users WHERE username = ?',
      [username]
    );

    if (rows.length === 0) {
      return res.status(401).json({ success: false, message: '아이디 또는 비밀번호가 틀렸습니다.' });
    }

    if (password !== rows[0].password) {
      return res.status(401).json({ success: false, message: '아이디 또는 비밀번호가 틀렸습니다.' });
    }

    const payload = {
      userId: rows[0].user_id,
      username: rows[0].username
    };

    const accessToken = signAccessToken(payload);
    const refreshToken = signRefreshToken(payload);

    refreshTokenStore[refreshToken] = rows[0].user_id;

    res.json({
      success: true,
      userId: rows[0].user_id,
      username: rows[0].username,
      accessToken,
      refreshToken
    });

  } catch (error) {
    res.status(500).json({ success: false, message: error.message });
  }
});

app.post('/token', (req, res) => {
  const { refreshToken } = req.body;
  if (!refreshToken) return res.sendStatus(401);

  const userId = refreshTokenStore[refreshToken];
  if (!userId) return res.sendStatus(403);

  jwt.verify(refreshToken, REFRESH_TOKEN_SECRET, (err, decoded) => {
    if (err) return res.sendStatus(403);

    const newAccessToken = signAccessToken({
      userId: decoded.userId,
      username: decoded.username
    });

    res.json({ accessToken: newAccessToken });
  });
});

app.post('/logout', (req, res) => {
  const { refreshToken } = req.body;
  if (refreshToken) delete refreshTokenStore[refreshToken];
  res.json({ success: true });
});

function authenticateToken(req, res, next) {
  const authHeader = req.headers['authorization'];
  const token = authHeader && authHeader.split(' ')[1];
  if (!token) return res.sendStatus(401);

  jwt.verify(token, JWT_SECRET, (err, user) => {
    if (err) return res.sendStatus(403);
    req.user = user;
    next();
  });
}

app.post('/rooms', authenticateToken, async (req, res) => {
  const userId = req.user.userId;
  const roomCode = generateRoomCode();

  try {
    const [result] = await pool.query(
      'INSERT INTO rooms (room_code, status, host_id) VALUES (?, ?, ?)',
      [roomCode, 'waiting', userId]
    );

    const roomId = result.insertId;

    await pool.query(
      'INSERT INTO room_players (room_id, user_id, seat_number) VALUES (?, ?, ?)',
      [roomId, userId, 1]
    );

    res.status(201).json({
      success: true,
      roomId,
      roomCode
    });

  } catch (error) {
    res.status(500).json({ success: false, message: error.message });
  }
});

app.listen(PORT, () => {
  console.log(`서버 실행 중 : ${PORT}`);
});