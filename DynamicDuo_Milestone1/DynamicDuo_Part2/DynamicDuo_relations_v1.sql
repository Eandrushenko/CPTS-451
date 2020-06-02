CREATE TABLE Yelper
(
Yelper_ID VARCHAR() NOT NULL;
Yelper_Name VARCHAR() NOT NULL;
NumVotes INTEGER:
Latitude INTEGER;
Longitude INTEGER;
PRIMARY KEY(Yelper_ID)
);

CREATE TABLE Buisness
(
Buisness_ID VARCHAR() NOT NULL;
Buisness_Name VARCHAR() NOT NULL;
Address VARCHAR() NOT NULL;
City VARCHAR() NOT NULL;
US_State VARCHAR() NOT NULL;
Avg_Rating FLOAT;
Categories VARCHAR();
NumReviews INTEGER;
Yelper_Rating INTEGER;
Distance FLOAT();
PRIMARY KEY(Buisness_ID);
);

CREATE TABLE Review
(
Review_ID VARCHAR() NOT NULL;
Stars INTEGER;
Review_Time DATETIME;
Text VARCHAR();
Yelper_ID VARCHAR();
Buisness_ID VARCHAR();
PRIMARY KEY (Review_ID, Yelper_ID, Buisness_ID);
FOREIGN KEY (Yelper_ID) REFERENCES Yelper(Yelper_ID);
FOREIGN KEY (Buisness_ID) REFERENCES Buisness(Buisness_ID);
);

CREATE TABLE Check_In
(
Check_In_Time DATETIME;
Yelper_ID VARCHAR();
Buisness_ID VARCHAR();
PRIMARY KEY (Yelper_ID, Buisness_ID);
FOREIGN KEY (Yelper_ID) REFERENCES Yelper(Yelper_ID);
FOREIGN KEY (Buisness_ID) REFERENCES Buisness(Buisness_ID);
);

