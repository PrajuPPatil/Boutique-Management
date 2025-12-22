-- Apply measurement validation to existing database
USE DB_New_Boutique;

-- Create CustomerMeasurements table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='CustomerMeasurements' AND xtype='U')
BEGIN
    CREATE TABLE CustomerMeasurements (
        MeasurementId INT IDENTITY(1,1) PRIMARY KEY,
        CustomerId INT NOT NULL,
        Gender CHAR(1) NOT NULL,
        MeasurementType NVARCHAR(50) NOT NULL,
        MeasurementValue DECIMAL(5,2) NOT NULL,
        Unit NVARCHAR(10) DEFAULT 'inches',
        CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
        
        CONSTRAINT FK_CustomerMeasurements_Customer 
            FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
        
        CONSTRAINT CK_Gender CHECK (Gender IN ('M', 'F')),
        CONSTRAINT CK_MenChest CHECK (NOT (Gender = 'M' AND MeasurementType = 'Chest' AND (MeasurementValue < 34 OR MeasurementValue > 52))),
        CONSTRAINT CK_MenWaist CHECK (NOT (Gender = 'M' AND MeasurementType = 'Waist' AND (MeasurementValue < 28 OR MeasurementValue > 44))),
        CONSTRAINT CK_MenHips CHECK (NOT (Gender = 'M' AND MeasurementType = 'Hips' AND (MeasurementValue < 34 OR MeasurementValue > 48))),
        CONSTRAINT CK_MenShoulder CHECK (NOT (Gender = 'M' AND MeasurementType = 'Shoulder' AND (MeasurementValue < 15 OR MeasurementValue > 22))),
        CONSTRAINT CK_MenSleeveLength CHECK (NOT (Gender = 'M' AND MeasurementType = 'Sleeve Length' AND (MeasurementValue < 22 OR MeasurementValue > 27))),
        CONSTRAINT CK_MenNeck CHECK (NOT (Gender = 'M' AND MeasurementType = 'Neck' AND (MeasurementValue < 14 OR MeasurementValue > 20))),
        CONSTRAINT CK_WomenBust CHECK (NOT (Gender = 'F' AND MeasurementType = 'Bust' AND (MeasurementValue < 30 OR MeasurementValue > 46))),
        CONSTRAINT CK_WomenWaist CHECK (NOT (Gender = 'F' AND MeasurementType = 'Waist' AND (MeasurementValue < 24 OR MeasurementValue > 40))),
        CONSTRAINT CK_WomenHips CHECK (NOT (Gender = 'F' AND MeasurementType = 'Hips' AND (MeasurementValue < 32 OR MeasurementValue > 48))),
        CONSTRAINT CK_WomenShoulder CHECK (NOT (Gender = 'F' AND MeasurementType = 'Shoulder' AND (MeasurementValue < 13 OR MeasurementValue > 19))),
        CONSTRAINT CK_WomenUpperArm CHECK (NOT (Gender = 'F' AND MeasurementType = 'Upper Arm' AND (MeasurementValue < 10 OR MeasurementValue > 16)))
    );
    
    CREATE INDEX IX_CustomerMeasurements_Customer_Gender ON CustomerMeasurements(CustomerId, Gender);
    PRINT 'CustomerMeasurements table created with validation constraints';
END
ELSE
BEGIN
    PRINT 'CustomerMeasurements table already exists';
END

-- Test the validation
PRINT 'Testing validation...';

-- This should work
INSERT INTO CustomerMeasurements (CustomerId, Gender, MeasurementType, MeasurementValue) 
VALUES (1, 'M', 'Chest', 38.5);
PRINT 'Valid measurement inserted successfully';

-- This should fail
BEGIN TRY
    INSERT INTO CustomerMeasurements (CustomerId, Gender, MeasurementType, MeasurementValue) 
    VALUES (1, 'M', 'Chest', 30.0);
END TRY
BEGIN CATCH
    PRINT 'Validation working: ' + ERROR_MESSAGE();
END CATCH