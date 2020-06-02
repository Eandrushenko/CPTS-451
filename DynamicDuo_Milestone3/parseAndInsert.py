import json
import psycopg2


def cleanStr4SQL(s):
    return s.replace("'", "`").replace("\n", " ")


def int2BoolStr (value):
    if value == 0:
        return 'False'
    else:
        return 'True'


def insert2BusinessTable():
    with open('./JSONfiles/yelp_business.json', 'r') as f:
        line = f.readline()
        count_line = 0
        try:
            conn = psycopg2.connect("dbname='milestone1db' user='postgres' host='localhost' password='zxcvbnm'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()
        categories = []

        while line:
            data = json.loads(line)
            sql_str = "INSERT INTO Business(business_id, name, address, state, " \
                      "city, zipcode, latitude, longitude, reviewRating, review_count, " \
                      "is_open, num_checkins) " \
                      "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + \
                      cleanStr4SQL(data["name"]) + "','" + cleanStr4SQL(data["address"]) \
                      + "','" + cleanStr4SQL(data["state"]) + "','" + \
                      cleanStr4SQL(data["city"]) + "','" + cleanStr4SQL(data["postal_code"]) \
                      + "'," + str(data["latitude"]) + "," + str(data["longitude"]) + "," + \
                      str(data["stars"]) + "," + str(data["review_count"]) + "," + \
                      int2BoolStr(data["is_open"]) + ",0);"
            try:
                cur.execute(sql_str)
            except:
                print("Insert to businessTABLE failed!")
            conn.commit()
            
            hours = data["hours"]
            for day in hours:
                somehold = hours[day].split("-")
                sql_str = "INSERT INTO Hours(business_id, day, open, close) VALUES ('" + \
                                cleanStr4SQL(data['business_id']) + "','" + day + "','" + \
                                somehold[0] + "','" + somehold[1] + "');"
                try:
                    cur.execute(sql_str)
                except:
                    print("Insert to hours failed!")
                conn.commit()

            categories = data["categories"]
            for cat in categories:
                sql_str = "INSERT INTO Categories(business_id, category) VALUES ('" \
                                     + cleanStr4SQL(data['business_id']) + "','" + cleanStr4SQL(cat) + "');"
                try:
                    cur.execute(sql_str)
                except:
                    print("Insert to categories failed!")
                conn.commit()

            line = f.readline()
            count_line += 1

        cur.close()
        conn.close()
    f.close()


def insert2UsersTable():
    with open('./JSONfiles/yelp_user.json', 'r') as f:
        line = f.readline()
        count_line = 0

        try:
            conn = psycopg2.connect("dbname='milestone1db' user='postgres' host='localhost' password='zxcvbnm'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            sql_str = "INSERT INTO Users(user_id, name, yelping_since, average_stars, " \
                                 "review_count, cool, funny, useful, fans) " \
                      "VALUES ('" + cleanStr4SQL(data['user_id']) + "','" + \
                      cleanStr4SQL(data["name"]) + "','" + cleanStr4SQL(data["yelping_since"]) \
                      + "'," + str(data["average_stars"]) + "," + \
                      str(data["review_count"]) + "," + str(data["cool"]) \
                      + "," + str(data["funny"]) + "," + str(data["useful"]) + "," + str(data["fans"])+ ");"
            try:
                cur.execute(sql_str)
            except:
                print("Insert to user table failed!")
            conn.commit()

            line = f.readline()
            count_line += 1

        cur.close()
        conn.close()

    print(count_line)
    f.close()


def insert2FriendsTable():
    with open('./JSONfiles/yelp_user.json', 'r') as f:
        line = f.readline()
        count_line = 0

        try:
            conn = psycopg2.connect("dbname='milestone1db' user='postgres' host='localhost' password='zxcvbnm'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)

            friends = data["friends"]
            for user in friends:
                sql_str = "INSERT INTO friend(user_id, friend_id) VALUES ('" + \
                                cleanStr4SQL(data['user_id']) + "','" + user + "');"

                try:
                    cur.execute(sql_str)
                except:
                    print("Insert to friends failed!")
                conn.commit()

            line = f.readline()
            count_line += 1

        cur.close()
        conn.close()

    print(count_line)
    f.close()


def insert2ReviewsTable():
    with open('./JSONfiles/yelp_review.json', 'r') as f:
        line = f.readline()

        try:
            conn = psycopg2.connect("dbname='milestone1db' user='postgres' host='localhost' password='zxcvbnm'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            sql_str = "INSERT INTO Review(review_id, user_id, business_id, review_stars, date, " \
                            "cool_vote, funny_vote, useful_vote, text) " \
                            "VALUES ('" + cleanStr4SQL(data['review_id']) + "','" + \
                            cleanStr4SQL(data["user_id"]) + "','" + cleanStr4SQL(data["business_id"]) \
                            + "'," + str(data["stars"]) + ",'" + cleanStr4SQL(data["date"]) + "'," + \
                            str(data["cool"]) + "," + str(data["funny"]) + "," + \
                            str(data["useful"]) + ",'" + cleanStr4SQL(data["text"]) + "');"
            try:
                cur.execute(sql_str)
            except:
                print("Insert to review table failed!")
            conn.commit()

            line = f.readline()

        cur.close()
        conn.close()

    f.close()


def insert2CheckinsTable():
    with open('./JSONfiles/yelp_checkin.json', 'r') as f:
        
        line = f.readline()
        count_line = 0
        total = 0
        try:
            conn = psycopg2.connect("dbname='milestone1db' user='postgres' host='localhost' password='zxcvbnm'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            business_id = str(cleanStr4SQL(data['business_id']))
            for day in data['time']:
                for time in data['time'][day]:
                        total += data['time'][day][time]

                sql_str = "INSERT INTO Checkins(business_id, day, total) VALUES ('" + str(business_id) + "','" + str(day) + "','" + str(total) + "');"
                total = 0
                try:
                    cur.execute(sql_str)
                except:
                    print("Insert to checkin table failed!" + business_id)
                conn.commit()

            line = f.readline()
            count_line += 1

        cur.close()
        conn.close()
    f.close()


insert2BusinessTable()
insert2UsersTable()
insert2FriendsTable()
insert2ReviewsTable()
insert2CheckinsTable()
