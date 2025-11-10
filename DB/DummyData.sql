USE `CodeBreaker`;

-- users 테이블
INSERT INTO `users` (username, email, password, score) VALUES
('PlayerOne', 'player1@test.com', 'hashed_pass_1', 1200),
('PlayerTwo', 'player2@test.com', 'hashed_pass_2', 950),
('TestUser', 'user3@test.com', 'hashed_pass_3', 1000);

-- rooms 테이블
INSERT INTO `rooms` (room_code, status, host_id) VALUES
('ABCD1', 'playing', 1),  -- 1번 방(ID=1): PlayerOne이 호스트이며 '플레이 중'
('EFGH2', 'waiting', 2),  -- 2번 방(ID=2): PlayerTwo가 호스트이며 '대기 중'
('IJKL3', 'finished', 3); -- 3번 방(ID=3): TestUser가 호스트였으며 '종료됨'

-- room_players 테이블 (각 방의 플레이어)
INSERT INTO `room_players` (room_id, user_id, seat_number) VALUES
-- 1번 방 (플레이 중)
(1, 1, 1), -- 1번 방(room_id=1)에 PlayerOne(user_id=1)이 1번 좌석
(1, 2, 2), -- 1번 방(room_id=1)에 PlayerTwo(user_id=2)가 2번 좌석
-- 2번 방 (대기 중)
(2, 2, 1), -- 2번 방(room_id=2)에 PlayerTwo(user_id=2)가 1번 좌석 (혼자)
-- 3번 방 (종료됨)
(3, 3, 1), -- 3번 방(room_id=3)에 TestUser(user_id=3)가 1번 좌석
(3, 1, 2); -- 3번 방(room_id=3)에 PlayerOne(user_id=1)이 2번 좌석

-- game_words 테이블 (게임 라운드별 단어)
INSERT INTO `game_words` (room_id, round_number, scrambled_word, correct_word, ended_at) VALUES
-- 1번 방 (플레이 중)
(1, 1, 'PLEAP', 'APPLE', NULL),      -- 1번 방 1라운드 (word_id=1)
(1, 2, 'NAANAB', 'BANANA', NULL),    -- 1번 방 2라운드 (word_id=2)
-- 3번 방 (종료됨)
(3, 1, 'ITYUN', 'UNITY', NOW()),     -- 3번 방 1라운드 (word_id=3)
(3, 2, 'EDON', 'NODE', NOW());       -- 3번 방 2라운드 (word_id=4)

-- round_answers 테이블 (라운드별 답변)
INSERT INTO `round_answers` (round_id, user_id, answer, is_correct) VALUES
-- 1번 방의 답변들
(1, 1, 'PEAL', 0),       -- 1번 라운드(APPLE), PlayerOne이 'PEAL' 오답
(1, 2, 'APPLE', 1),      -- 1번 라운드(APPLE), PlayerTwo가 'APPLE' 정답
(2, 1, 'BANANA', 1),     -- 2번 라운드(BANANA), PlayerOne이 'BANANA' 정답
(2, 2, 'NABANA', 0),     -- 2번 라운드(BANANA), PlayerTwo가 'NABANA' 오답
-- 3번 방의 답변들
(3, 3, 'UNITY', 1),      -- 3번 라운드(UNITY), TestUser가 'UNITY' 정답
(3, 1, 'UNIT', 0),       -- 3번 라운드(UNITY), PlayerOne이 'UNIT' 오답
(4, 3, 'NODE', 1),       -- 4번 라운드(NODE), TestUser가 'NODE' 정답
(4, 1, 'DONE', 0);       -- 4번 라운드(NODE), PlayerOne이 'DONE' 오답

-- game_results 테이블 (종료된 게임 결과)
INSERT INTO `game_results` (room_id, winner_id, score_user1, score_user2) VALUES
-- 3번 방 (종료됨)
(3, 3, 2, 0); -- 3번 방(room_id=3)의 승자는 TestUser(user_id=3). 1번 좌석(TestUser) 2점, 2번 좌석(PlayerOne) 0점