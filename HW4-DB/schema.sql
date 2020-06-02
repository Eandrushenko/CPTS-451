CREATE TABLE Teams (
        teamCode  char(3) PRIMARY KEY,
		teamName varchar(50), 
        city varchar(50)
);

CREATE TABLE Players (
        playerID integer PRIMARY KEY,
        name varchar(50),
		teamCode  char(3),
        height integer,
        weight integer,
		FOREIGN KEY (teamCode) REFERENCES Teams(teamCode)
);

CREATE TABLE Games (
        gameID integer PRIMARY KEY,
        hometeam char(3) NOT NULL,
        awayteam char(3) NOT NULL,
        homescore integer,
        awayscore integer,
		FOREIGN KEY (hometeam) REFERENCES Teams(teamCode),      
		FOREIGN KEY (awayteam) REFERENCES Teams(teamCode)
);

CREATE TABLE GameStats (
        playerID integer NOT NULL,
        gameID integer NOT NULL,
        points integer,
        PRIMARY KEY (playerID, gameID),
		FOREIGN KEY (playerID) REFERENCES Players(playerID),
		FOREIGN KEY (gameID) REFERENCES Games(gameID)
);
