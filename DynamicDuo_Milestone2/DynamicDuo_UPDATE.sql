UPDATE business
SET review_count = R.rs
FROM (SELECT business_id, count(review_stars) as rs 
	  FROM review 
	  group by business_id) R 
	  where business.business_id = R.business_id;
	  
UPDATE business
SET reviewrating = R.aver
FROM (SELECT business_id, AVG(review_stars) as aver 
	  FROM review group by(business_id)) R 
	  where business.business_id = R.business_id;
	  
	  
UPDATE business
SET num_checkins = R.adder
FROM (SELECT business_id, SUM(total) as adder 
	  FROM checkins group by(business_id)) R 
	  where business.business_id = R.business_id;