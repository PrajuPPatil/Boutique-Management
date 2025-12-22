-- Fix ApplicationUsers table to match Identity requirements
USE [DB_New_Boutique]
GO

-- Add missing Identity columns
ALTER TABLE [dbo].[ApplicationUsers] 
ADD [UserName] NVARCHAR(256) NULL,
    [NormalizedUserName] NVARCHAR(256) NULL,
    [NormalizedEmail] NVARCHAR(256) NULL,
    [EmailConfirmed] BIT NOT NULL DEFAULT 0,
    [PhoneNumber] NVARCHAR(MAX) NULL,
    [PhoneNumberConfirmed] BIT NOT NULL DEFAULT 0,
    [TwoFactorEnabled] BIT NOT NULL DEFAULT 0,
    [LockoutEnd] DATETIMEOFFSET(7) NULL,
    [LockoutEnabled] BIT NOT NULL DEFAULT 0,
    [AccessFailedCount] INT NOT NULL DEFAULT 0,
    [SecurityStamp] NVARCHAR(MAX) NULL,
    [ConcurrencyStamp] NVARCHAR(MAX) NULL;
GO

-- Copy Username to UserName for existing records
UPDATE [dbo].[ApplicationUsers] 
SET [UserName] = [Username],
    [NormalizedUserName] = UPPER([Username]),
    [NormalizedEmail] = UPPER([Email]),
    [EmailConfirmed] = [IsEmailConfirmed],
    [SecurityStamp] = NEWID(),
    [ConcurrencyStamp] = NEWID()
WHERE [UserName] IS NULL;
GO

-- Create required indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UserNameIndex')
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[ApplicationUsers]
(
    [NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'EmailIndex')
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[ApplicationUsers]
(
    [NormalizedEmail] ASC
);
GO

PRINT 'ApplicationUsers table updated successfully!'