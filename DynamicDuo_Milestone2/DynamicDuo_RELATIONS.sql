drop table checkins;
drop table review;
drop table friend;
drop table users;
drop table categories;
drop table hours;
drop table business;

CREATE TABLE Users
(
	user_id VARCHAR PRIMARY KEY,
	average_stars FLOAT,
	cool INTEGER,
	funny INTEGER,
	useful INTEGER,
	name VARCHAR,
	fans INTEGER,
	review_count INTEGER,
	yelping_since DATE
);

CREATE TABLE Business
(
	business_id VARCHAR PRIMARY KEY,
	name VARCHAR,
	city VARCHAR,
	state VARCHAR(2),
	zipcode VARCHAR(5),
	latitude FLOAT,
	longitude FLOAT,
	address VARCHAR,
	review_count INTEGER,
	num_checkins INTEGER,
	reviewRating FLOAT,
	is_open BOOLEAN
);

CREATE TABLE friend
(
	user_id VARCHAR,
	friend_id VARCHAR,
	PRIMARY KEY(user_id, friend_id),
	FOREIGN KEY (user_id) REFERENCES Users(user_id),
	FOREIGN KEY (friend_id) REFERENCES Users(user_id)
);

CREATE TABLE Review
(
	review_id VARCHAR PRIMARY KEY,
	user_id VARCHAR,
	business_id VARCHAR,
	review_stars INTEGER,
	date DATE,
	cool_vote INTEGER,
	funny_vote INTEGER,
	useful_vote INTEGER,
	text VARCHAR,
	FOREIGN KEY (user_id) REFERENCES Users(user_id),
	FOREIGN KEY (business_id) REFERENCES Business(business_id)
);

CREATE TABLE Checkins
(
	business_id VARCHAR,
	day VARCHAR,
	zero INTEGER,
	one INTEGER,
	two INTEGER,
	three INTEGER,
	four INTEGER,
	five INTEGER,
	six INTEGER,
	seven INTEGER,
	eight INTEGER,
	nine INTEGER,
	ten INTEGER,
	eleven INTEGER,
	twelve INTEGER,
	thirteen INTEGER,
	fourteen INTEGER,
	fifteen INTEGER,
	sixteen INTEGER,
	seventeen INTEGER,
	eighteen INTEGER,
	nineteen INTEGER,
	twenty INTEGER,
	twentyone INTEGER,
	twentytwo INTEGER,
	twentythree INTEGER,
	PRIMARY KEY(business_id, day),
	FOREIGN KEY (business_id) REFERENCES Business(business_id)
);

CREATE TABLE Hours
(
	business_id VARCHAR,
	day VARCHAR,
	open VARCHAR,
    close VARCHAR,
	PRIMARY KEY(business_id, day),
	FOREIGN KEY (business_id) REFERENCES Business(business_id)
);

CREATE TABLE Categories
(
	business_id VARCHAR,
	category VARCHAR,
	PRIMARY KEY (business_id, category),
	FOREIGN KEY (business_id) REFERENCES Business(business_id)
);


