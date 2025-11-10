CREATE DATABASE IF NOT EXISTS `CodeBreaker` COLLATE 'utf8mb4_0900_ai_ci';

-- users 테이블
CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(255) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    score INT DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB COLLATE='utf8mb4_0900_ai_ci';

-- rooms 테이블
CREATE TABLE rooms (
    id INT AUTO_INCREMENT PRIMARY KEY,
    room_code VARCHAR(20) NOT NULL UNIQUE,
    status ENUM('waiting', 'playing', 'finished') NOT NULL DEFAULT 'waiting',
    host_id INT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_rooms_host
        FOREIGN KEY (host_id) REFERENCES users(id)
        ON DELETE CASCADE -- 호스트가 탈퇴하면 방도 삭제 (정책에 따라 RESTRICT 또는 SET NULL로 변경 가능)
        ON UPDATE CASCADE
)

-- room_players 테이블
CREATE TABLE room_players (
    id INT AUTO_INCREMENT PRIMARY KEY,
    room_id INT NOT NULL,
    user_id INT NOT NULL,
    seat_number TINYINT, -- 1 or 2
    joined_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    
    CONSTRAINT fk_room_players_room
        FOREIGN KEY (room_id) REFERENCES rooms(id)
        ON DELETE CASCADE -- 방이 삭제되면 플레이어 정보도 삭제
        ON UPDATE CASCADE,
        
    CONSTRAINT fk_room_players_user
        FOREIGN KEY (user_id) REFERENCES users(id)
        ON DELETE CASCADE -- 유저가 탈퇴하면 플레이어 정보도 삭제
        ON UPDATE CASCADE,
        
    -- 한 방에 같은 유저가 중복 입장하거나 같은 좌석 번호를 갖는 것을 방지
    UNIQUE KEY uk_room_user (room_id, user_id),
    UNIQUE KEY uk_room_seat (room_id, seat_number)
)

-- game_results 테이블
CREATE TABLE game_results (
    id INT AUTO_INCREMENT PRIMARY KEY,
    room_id INT NOT NULL,
    winner_id INT, -- 무승부 등 승자가 없는 경우 NULL 허용
    score_user1 INT DEFAULT 0,
    score_user2 INT DEFAULT 0,
    ended_at DATETIME DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_game_results_room
        FOREIGN KEY (room_id) REFERENCES rooms(id)
        ON DELETE CASCADE -- 방이 삭제되면 게임 결과도 삭제 (정책에 따라 변경)
        ON UPDATE CASCADE,

    CONSTRAINT fk_game_results_winner
        FOREIGN KEY (winner_id) REFERENCES users(id)
        ON DELETE SET NULL -- 승자가 탈퇴해도 기록은 남김 (NULL로 변경)
        ON UPDATE CASCADE
)

-- game_words 테이블
CREATE TABLE game_words (
    id INT AUTO_INCREMENT PRIMARY KEY,
    room_id INT NOT NULL,
    round_number INT,
    scrambled_word VARCHAR(50),
    correct_word VARCHAR(50),
    started_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    ended_at DATETIME,

    CONSTRAINT fk_game_words_room
        FOREIGN KEY (room_id) REFERENCES rooms(id)
        ON DELETE CASCADE -- 방이 삭제되면 관련 단어 정보도 삭제
        ON UPDATE CASCADE
)

-- round_answers 테이블
CREATE TABLE round_answers (
    id INT AUTO_INCREMENT PRIMARY KEY,
    round_id INT NOT NULL,
    user_id INT NOT NULL,
    answer VARCHAR(50),
    is_correct BOOLEAN DEFAULT FALSE,
    answered_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_round_answers_word
        FOREIGN KEY (round_id) REFERENCES game_words(id)
        ON DELETE CASCADE -- 해당 라운드(단어)가 삭제되면 답변도 삭제
        ON UPDATE CASCADE,
    CONSTRAINT fk_round_answers_user
        FOREIGN KEY (user_id) REFERENCES users(id)
        ON DELETE CASCADE -- 유저가 탈퇴하면 답변도 삭제
        ON UPDATE CASCADE
)