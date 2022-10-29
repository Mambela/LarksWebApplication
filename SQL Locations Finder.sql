
SELECT * FROM Addresses

SELECT * FROM Location

SELECT * FROM Addresses a 
WHERE (
          acos(sin(a.lat * 0.0175) * sin(-8.2335 * 0.0175) 
               + cos(a.lat * 0.0175) * cos(-8.2335 * 0.0175) *    
                 cos((13.88946 * 0.0175) - (a.lng * 0.0175))
              ) * 6371 <= 2
      )
--miles 3959 
--KM  6371

--Update Addresses SET lat=-8.2514,lng=13.87756 where id=5
-- -8.2335	13.88946