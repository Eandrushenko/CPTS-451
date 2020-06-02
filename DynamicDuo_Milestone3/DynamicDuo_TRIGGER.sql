-- TRIGGER 1

CREATE OR REPLACE FUNCTION incrementReviews() RETURNS trigger AS '
BEGIN 
   UPDATE  business
   SET  reviewRating = ((reviewRating * review_count) + NEW.review_stars) / (review_count + 1), review_count = review_count+1
   WHERE  business_id = NEW.business_id;
   RETURN NEW;
END
' LANGUAGE plpgsql;  
    
CREATE TRIGGER addReview
AFTER INSERT ON Review
FOR EACH ROW
EXECUTE PROCEDURE incrementReviews();

-- TRIGGER 1

-- TRIGGER 2

CREATE OR REPLACE FUNCTION incrementbusiness() RETURNS trigger AS '
BEGIN 
   UPDATE  business
   SET  num_checkins = num_checkins+1
   WHERE  business_id = NEW.business_id;
   RETURN NEW;
END
' LANGUAGE plpgsql;  
    
CREATE TRIGGER addcheckins
AFTER INSERT ON Checkins
FOR EACH ROW
EXECUTE PROCEDURE incrementbusiness();
    
CREATE TRIGGER upcheckins
AFTER update ON Checkins
FOR EACH ROW
EXECUTE PROCEDURE incrementbusiness();


-- TRIGGER 2

-- TESTING

insert into business VALUES('WSU', 'Something', 'Bham', 'WA', '98229', 14, 13, '12', 8, 5, 3.4, true);

insert into users VALUES('samcarroll', 4, 1, 1, 1, 'Sam Carroll', 7, 10, '12/13/1997');

insert into Review VALUES('hooblah', 'samcarroll', 'WSU', 4, '12/13/1997', 1, 1, 1, 'SOMETHIGN');

insert into Checkins VALUES('WSU', 'Monday', 1);

update Checkins SET total = total + 1 where business_id = 'WSU' AND day = 'Monday';

