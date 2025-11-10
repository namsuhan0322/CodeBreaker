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

app.listen(port || 4000, () => {
  console.log(`서버 실행 중 : ${port}`);
});
