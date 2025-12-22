-- Comprehensive Measurement Validation System for Boutique Database
-- Based on Indian Anthropometric Data with Standard Sizing (XS to XXL)

-- Create CustomerMeasurements table with validation constraints
CREATE TABLE CustomerMeasurements (
    MeasurementId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    Gender CHAR(1) NOT NULL CHECK (Gender IN ('M', 'F')),
    MeasurementType NVARCHAR(50) NOT NULL,
    MeasurementValue DECIMAL(5,2) NOT NULL,
    Unit NVARCHAR(10) DEFAULT 'inches',
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    
    -- Foreign key constraint
    CONSTRAINT FK_CustomerMeasurements_Customer 
        FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
    
    -- Gender-specific measurement validation constraints
    CONSTRAINT CK_MenChest CHECK (
        NOT (Gender = 'M' AND MeasurementType = 'Chest' AND (MeasurementValue < 34 OR MeasurementValue > 52))
    ),
    CONSTRAINT CK_MenWaist CHECK (
        NOT (Gender = 'M' AND MeasurementType = 'Waist' AND (MeasurementValue < 28 OR MeasurementValue > 44))
    ),
    CONSTRAINT CK_MenHips CHECK (
        NOT (Gender = 'M' AND MeasurementType = 'Hips' AND (MeasurementValue < 34 OR MeasurementValue > 48))
    ),
    CONSTRAINT CK_MenShoulder CHECK (
        NOT (Gender = 'M' AND MeasurementType = 'Shoulder' AND (MeasurementValue < 15 OR MeasurementValue > 22))
    ),
    CONSTRAINT CK_MenSleeveLength CHECK (
        NOT (Gender = 'M' AND MeasurementType = 'Sleeve Length' AND (MeasurementValue < 22 OR MeasurementValue > 27))
    ),
    CONSTRAINT CK_MenNeck CHECK (
        NOT (Gender = 'M' AND MeasurementType = 'Neck' AND (MeasurementValue < 14 OR MeasurementValue > 20))
    ),
    
    -- Women's measurement validation constraints
    CONSTRAINT CK_WomenBust CHECK (
        NOT (Gender = 'F' AND MeasurementType = 'Bust' AND (MeasurementValue < 30 OR MeasurementValue > 46))
    ),
    CONSTRAINT CK_WomenWaist CHECK (
        NOT (Gender = 'F' AND MeasurementType = 'Waist' AND (MeasurementValue < 24 OR MeasurementValue > 40))
    ),
    CONSTRAINT CK_WomenHips CHECK (
        NOT (Gender = 'F' AND MeasurementType = 'Hips' AND (MeasurementValue < 32 OR MeasurementValue > 48))
    ),
    CONSTRAINT CK_WomenShoulder CHECK (
        NOT (Gender = 'F' AND MeasurementType = 'Shoulder' AND (MeasurementValue < 13 OR MeasurementValue > 19))
    ),
    CONSTRAINT CK_WomenUpperArm CHECK (
        NOT (Gender = 'F' AND MeasurementType = 'Upper Arm' AND (MeasurementValue < 10 OR MeasurementValue > 16))
    )
);

-- Create index for better performance
CREATE INDEX IX_CustomerMeasurements_Customer_Gender 
ON CustomerMeasurements(CustomerId, Gender);

-- Validation trigger with custom error messages
GO
CREATE OR ALTER TRIGGER TR_ValidateMeasurements
ON CustomerMeasurements
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ErrorMessage NVARCHAR(500);
    
    -- Men's validation with specific error messages
    IF EXISTS (SELECT 1 FROM inserted WHERE Gender = 'M' AND MeasurementType = 'Chest' AND (MeasurementValue < 34 OR MeasurementValue > 52))
    BEGIN
        SET @ErrorMessage = 'Invalid chest measurement for men - must be 34-52 inches';
        THROW 50001, @ErrorMessage, 1;
    END
    
    IF EXISTS (SELECT 1 FROM inserted WHERE Gender = 'M' AND MeasurementType = 'Waist' AND (MeasurementValue < 28 OR MeasurementValue > 44))
    BEGIN
        SET @ErrorMessage = 'Invalid waist measurement for men - must be 28-44 inches';
        THROW 50002, @ErrorMessage, 1;
    END
    
    IF EXISTS (SELECT 1 FROM inserted WHERE Gender = 'M' AND MeasurementType = 'Hips' AND (MeasurementValue < 34 OR MeasurementValue > 48))
    BEGIN
        SET @ErrorMessage = 'Invalid hips measurement for men - must be 34-48 inches';
        THROW 50003, @ErrorMessage, 1;
    END
    
    IF EXISTS (SELECT 1 FROM inserted WHERE Gender = 'M' AND MeasurementType = 'Shoulder' AND (MeasurementValue < 15 OR MeasurementValue > 22))
    BEGIN
        SET @ErrorMessage = 'Invalid shoulder measurement for men - must be 15-22 inches';
        THROW 50004, @ErrorMessage, 1;
    END
    
    IF EXISTS (SELECT 1 FROM inserted WHERE Gender = 'M' AND MeasurementType = 'Sleeve Length' AND (MeasurementValue < 22 OR MeasurementValue > 27))
    BEGIN
        SET @ErrorMessage = 'Invalid sleeve length measurement for men - must be 22-27 inches';
        THROW 50005, @ErrorMessage, 1;
    END
    
    IF EXISTS (SELECT 1 FROM inserted WHERE Gender = 'M' AND MeasurementType = 'Neck' AND (MeasurementValue < 14 OR MeasurementValue > 20))
    BEGIN
        SET @ErrorMessage = 'Invalid neck measurement for men - must be 14-20 inches';
        THROW 50006, @ErrorMessage, 1;
    END
    
    -- Women's validation with specific error messages
    IF EXISTS (SELECT 1 FROM inserted WHERE Gender = 'F' AND MeasurementType = 'Bust' AND (MeasurementValue < 30 OR MeasurementValue > 46))
    BEGIN
        SET @ErrorMessage = 'Invalid bust measurement for women - must be 30-46 inches';
        THROW 50007, @ErrorMessage, 1;
    END
    
    IF EXISTS (SELECT 1 FROM inserted WHERE Gender = 'F' AND MeasurementType = 'Waist' AND (MeasurementValue < 24 OR MeasurementValue > 40))
    BEGIN
        SET @ErrorMessage = 'Invalid waist measurement for women - must be 24-40 inches';
        THROW 50008, @ErrorMessage, 1;
    END
    
    IF EXISTS (SELECT 1 FROM inserted WHERE Gender = 'F' AND MeasurementType = 'Hips' AND (MeasurementValue < 32 OR MeasurementValue > 48))
    BEGIN
        SET @ErrorMessage = 'Invalid hips measurement for women - must be 32-48 inches';
        THROW 50009, @ErrorMessage, 1;
    END
    
    IF EXISTS (SELECT 1 FROM inserted WHERE Gender = 'F' AND MeasurementType = 'Shoulder' AND (MeasurementValue < 13 OR MeasurementValue > 19))
    BEGIN
        SET @ErrorMessage = 'Invalid shoulder measurement for women - must be 13-19 inches';
        THROW 50010, @ErrorMessage, 1;
    END
    
    IF EXISTS (SELECT 1 FROM inserted WHERE Gender = 'F' AND MeasurementType = 'Upper Arm' AND (MeasurementValue < 10 OR MeasurementValue > 16))
    BEGIN
        SET @ErrorMessage = 'Invalid upper arm measurement for women - must be 10-16 inches';
        THROW 50011, @ErrorMessage, 1;
    END
END;

-- Sample INSERT statements for testing
GO

-- Valid measurements (these should succeed)
INSERT INTO CustomerMeasurements (CustomerId, Gender, MeasurementType, MeasurementValue) VALUES
-- Men's measurements
(1, 'M', 'Chest', 38.5),
(1, 'M', 'Waist', 32.0),
(1, 'M', 'Hips', 36.0),
(1, 'M', 'Shoulder', 18.0),
(1, 'M', 'Sleeve Length', 24.5),
(1, 'M', 'Neck', 16.0),

-- Women's measurements
(2, 'F', 'Bust', 34.0),
(2, 'F', 'Waist', 28.0),
(2, 'F', 'Hips', 36.0),
(2, 'F', 'Shoulder', 15.5),
(2, 'F', 'Upper Arm', 12.0);

-- Invalid measurements (these should fail with specific error messages)
-- Uncomment to test validation

/*
-- This will fail: Invalid chest measurement for men
INSERT INTO CustomerMeasurements (CustomerId, Gender, MeasurementType, MeasurementValue) 
VALUES (1, 'M', 'Chest', 30.0);

-- This will fail: Invalid waist measurement for women
INSERT INTO CustomerMeasurements (CustomerId, Gender, MeasurementType, MeasurementValue) 
VALUES (2, 'F', 'Waist', 20.0);

-- This will fail: Invalid shoulder measurement for men
INSERT INTO CustomerMeasurements (CustomerId, Gender, MeasurementType, MeasurementValue) 
VALUES (1, 'M', 'Shoulder', 25.0);
*/

-- View to display measurement ranges by gender
GO
CREATE OR ALTER VIEW MeasurementRanges AS
SELECT 
    'M' as Gender,
    'Chest' as MeasurementType,
    34.00 as MinValue,
    52.00 as MaxValue,
    'inches' as Unit
UNION ALL
SELECT 'M', 'Waist', 28.00, 44.00, 'inches'
UNION ALL
SELECT 'M', 'Hips', 34.00, 48.00, 'inches'
UNION ALL
SELECT 'M', 'Shoulder', 15.00, 22.00, 'inches'
UNION ALL
SELECT 'M', 'Sleeve Length', 22.00, 27.00, 'inches'
UNION ALL
SELECT 'M', 'Neck', 14.00, 20.00, 'inches'
UNION ALL
SELECT 'F', 'Bust', 30.00, 46.00, 'inches'
UNION ALL
SELECT 'F', 'Waist', 24.00, 40.00, 'inches'
UNION ALL
SELECT 'F', 'Hips', 32.00, 48.00, 'inches'
UNION ALL
SELECT 'F', 'Shoulder', 13.00, 19.00, 'inches'
UNION ALL
SELECT 'F', 'Upper Arm', 10.00, 16.00, 'inches';

-- Function to validate measurement before insert
GO
CREATE OR ALTER FUNCTION ValidateMeasurement(
    @Gender CHAR(1),
    @MeasurementType NVARCHAR(50),
    @MeasurementValue DECIMAL(5,2)
)
RETURNS BIT
AS
BEGIN
    DECLARE @IsValid BIT = 0;
    
    SELECT @IsValid = CASE 
        WHEN @MeasurementValue BETWEEN MinValue AND MaxValue THEN 1
        ELSE 0
    END
    FROM MeasurementRanges
    WHERE Gender = @Gender AND MeasurementType = @MeasurementType;
    
    RETURN ISNULL(@IsValid, 0);
END;

-- Stored procedure for safe measurement insertion
GO
CREATE OR ALTER PROCEDURE InsertMeasurement
    @CustomerId INT,
    @Gender CHAR(1),
    @MeasurementType NVARCHAR(50),
    @MeasurementValue DECIMAL(5,2),
    @Unit NVARCHAR(10) = 'inches'
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Validate the measurement
    IF dbo.ValidateMeasurement(@Gender, @MeasurementType, @MeasurementValue) = 1
    BEGIN
        INSERT INTO CustomerMeasurements (CustomerId, Gender, MeasurementType, MeasurementValue, Unit)
        VALUES (@CustomerId, @Gender, @MeasurementType, @MeasurementValue, @Unit);
        
        SELECT 'Measurement inserted successfully' as Result;
    END
    ELSE
    BEGIN
        DECLARE @ErrorMsg NVARCHAR(200);
        DECLARE @MinVal DECIMAL(5,2), @MaxVal DECIMAL(5,2);
        
        SELECT @MinVal = MinValue, @MaxVal = MaxValue
        FROM MeasurementRanges
        WHERE Gender = @Gender AND MeasurementType = @MeasurementType;
        
        SET @ErrorMsg = CONCAT('Invalid ', @MeasurementType, ' measurement for ', 
                              CASE @Gender WHEN 'M' THEN 'men' ELSE 'women' END,
                              ' - must be ', @MinVal, '-', @MaxVal, ' inches');
        
        THROW 50000, @ErrorMsg, 1;
    END
END;