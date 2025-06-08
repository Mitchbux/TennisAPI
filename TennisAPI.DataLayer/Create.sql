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

-- Functions for statistics
CREATE FUNCTION CountryFirst()
RETURNS TEXT
LANGUAGE SQL
AS $$;
WITH points AS (
SELECT CODE, SUM(result) as points, COUNT(result) as total FROM Players
INNER JOIN PLAYER_DATA ON PLAYER_DATA.PLAYER_ID = Players.id
INNER JOIN COUNTRIES ON COUNTRIES.ID = Players.COUNTRY_ID
INNER JOIN LAST_RESULTS ON LAST_RESULTS.PLAYER_ID = Players.id
GROUP BY CODE
)
SELECT CODE FROM points
ORDER BY (points/total)
LIMIT 1
$$;

CREATE FUNCTION MeanWeightRatio()
RETURNS FLOAT
LANGUAGE SQL
AS $$;
WITH imc as ( select weight/1000.0 as w, height/100.0 as h from player_data )
SELECT AVG(w/(h*h)) FROM imc
$$;

CREATE FUNCTION MedianeHeight()
RETURNS int
LANGUAGE SQL
AS $$;

WITH heights as ( select height as h, ROW_NUMBER() OVER (ORDER BY height) AS id from player_data )
SELECT h FROM heights
GROUP BY id, h
HAVING id = (count(h)/2)+1

$$;