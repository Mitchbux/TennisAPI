-- Tennis Players Database Schema
-- PostgreSQL Creation Script

-- Countries Table
CREATE TABLE countries (
id INTEGER PRIMARY KEY,
code VARCHAR(3) UNIQUE NOT NULL,
picture TEXT,
created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Players Table
CREATE TABLE players (
id INTEGER PRIMARY KEY,
firstname VARCHAR(100) NOT NULL,
lastname VARCHAR(100) NOT NULL,
shortname VARCHAR(10),
sex VARCHAR(1) CHECK (sex IN ('M', 'F')),
country_id INTEGER REFERENCES countries(id),
picture TEXT,
);

-- Player Data Table
CREATE TABLE player_data (
id INTEGER PRIMARY KEY,
player_id INTEGER REFERENCES players(id) ON DELETE CASCADE,
rank INTEGER,
points INTEGER,
weight INTEGER,
height INTEGER,
age INTEGER,
);

-- Last Results Table (for the "last" array)
CREATE TABLE last_results (
id INTEGER PRIMARY KEY,
player_id INTEGER REFERENCES players(id) ON DELETE CASCADE,
result INTEGER CHECK (result IN (0, 1))
);

-- Indexes for better performance
CREATE INDEX idx_players_country ON players(country_id);
CREATE INDEX idx_player_data_player ON player_data(player_id);
CREATE INDEX idx_last_results_player ON last_results(player_id);
CREATE INDEX idx_players_rank ON player_data(rank);