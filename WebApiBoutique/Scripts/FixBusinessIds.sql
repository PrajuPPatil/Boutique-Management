-- SQL Script to update existing users with unique BusinessIds
-- Run this script to fix existing users who have the same BusinessId

UPDATE Users 
SET BusinessId = ABS(CHECKSUM(NEWID())), 
    BusinessName = UserName + '''s Boutique',
    UpdatedAt = GETUTCDATE()
WHERE BusinessId = 1 OR BusinessId IS NULL;

-- Verify the update
SELECT Id, UserName, Email, BusinessId, BusinessName 
FROM Users 
ORDER BY Id;