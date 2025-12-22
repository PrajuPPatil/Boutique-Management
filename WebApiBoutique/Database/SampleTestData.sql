-- Sample Test Data for Measurement Validation System

-- Test valid measurements
EXEC InsertMeasurement @CustomerId = 1, @Gender = 'M', @MeasurementType = 'Chest', @MeasurementValue = 38.5;
EXEC InsertMeasurement @CustomerId = 1, @Gender = 'M', @MeasurementType = 'Waist', @MeasurementValue = 32.0;
EXEC InsertMeasurement @CustomerId = 1, @Gender = 'M', @MeasurementType = 'Hips', @MeasurementValue = 36.0;

EXEC InsertMeasurement @CustomerId = 2, @Gender = 'F', @MeasurementType = 'Bust', @MeasurementValue = 34.0;
EXEC InsertMeasurement @CustomerId = 2, @Gender = 'F', @MeasurementType = 'Waist', @MeasurementValue = 28.0;
EXEC InsertMeasurement @CustomerId = 2, @Gender = 'F', @MeasurementType = 'Hips', @MeasurementValue = 36.0;

-- Test invalid measurements (these will throw errors)
/*
EXEC InsertMeasurement @CustomerId = 1, @Gender = 'M', @MeasurementType = 'Chest', @MeasurementValue = 30.0;
EXEC InsertMeasurement @CustomerId = 2, @Gender = 'F', @MeasurementType = 'Waist', @MeasurementValue = 20.0;
*/

-- Query to view all measurement ranges
SELECT * FROM MeasurementRanges ORDER BY Gender, MeasurementType;

-- Query to view all customer measurements
SELECT 
    cm.MeasurementId,
    c.CustomerName,
    cm.Gender,
    cm.MeasurementType,
    cm.MeasurementValue,
    cm.Unit,
    cm.CreatedDate
FROM CustomerMeasurements cm
JOIN Customers c ON cm.CustomerId = c.CustomerId
ORDER BY c.CustomerName, cm.Gender, cm.MeasurementType;