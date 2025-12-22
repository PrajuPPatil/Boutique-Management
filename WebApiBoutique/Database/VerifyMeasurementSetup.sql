-- Verify Measurement Management Setup
-- Run this in SQL Server Management Studio or Visual Studio

USE Boutique_Management;
GO

-- 1. Check if CustomerMeasurements table exists
IF OBJECT_ID('CustomerMeasurements', 'U') IS NOT NULL
    PRINT '✓ CustomerMeasurements table exists'
ELSE
    PRINT '✗ CustomerMeasurements table NOT found - Run migrations!'
GO

-- 2. Check table structure
SELECT 
    COLUMN_NAME, 
    DATA_TYPE, 
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'CustomerMeasurements'
ORDER BY ORDINAL_POSITION;
GO

-- 3. Check if any customers exist
SELECT COUNT(*) as CustomerCount FROM Customers;
GO

-- 4. List all customers
SELECT CustomerId, CustomerName, Email, PhoneNo FROM Customers;
GO

-- 5. Check existing measurements
SELECT COUNT(*) as MeasurementCount FROM CustomerMeasurements;
GO

-- 6. View all measurements
SELECT 
    cm.MeasurementId,
    cm.CustomerId,
    c.CustomerName,
    cm.Gender,
    cm.MeasurementType,
    cm.MeasurementValue,
    cm.Unit,
    cm.CreatedDate
FROM CustomerMeasurements cm
INNER JOIN Customers c ON cm.CustomerId = c.CustomerId
ORDER BY cm.CreatedDate DESC;
GO

-- 7. Insert test customer if none exist
IF NOT EXISTS (SELECT 1 FROM Customers)
BEGIN
    INSERT INTO Customers (CustomerName, Email, PhoneNo, Address, Category, CreatedDate, Active)
    VALUES ('Test Customer', 'test@example.com', '1234567890', 'Test Address', 'Men', GETUTCDATE(), 1);
    
    PRINT '✓ Test customer created with ID: ' + CAST(SCOPE_IDENTITY() AS VARCHAR);
END
ELSE
    PRINT '✓ Customers already exist'
GO

-- 8. Test insert a measurement (change CustomerId to match your customer)
DECLARE @TestCustomerId INT = (SELECT TOP 1 CustomerId FROM Customers);

IF @TestCustomerId IS NOT NULL
BEGIN
    INSERT INTO CustomerMeasurements (CustomerId, Gender, MeasurementType, MeasurementValue, Unit, CreatedDate)
    VALUES (@TestCustomerId, 'M', 'Chest', 40.0, 'inches', GETUTCDATE());
    
    PRINT '✓ Test measurement inserted successfully';
    
    -- Show the inserted measurement
    SELECT TOP 1 * FROM CustomerMeasurements ORDER BY MeasurementId DESC;
END
ELSE
    PRINT '✗ No customers found to test with'
GO

-- 9. Check constraints
SELECT 
    CONSTRAINT_NAME,
    CONSTRAINT_TYPE
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE TABLE_NAME = 'CustomerMeasurements';
GO

-- 10. Summary
PRINT '========================================';
PRINT 'SUMMARY';
PRINT '========================================';
SELECT 
    (SELECT COUNT(*) FROM Customers) as TotalCustomers,
    (SELECT COUNT(*) FROM CustomerMeasurements) as TotalMeasurements,
    (SELECT COUNT(DISTINCT CustomerId) FROM CustomerMeasurements) as CustomersWithMeasurements;
GO
