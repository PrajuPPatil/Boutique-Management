# Measurement Validation System

## Overview
Comprehensive validation system for men's and women's clothing measurements based on Indian anthropometric data with standard sizing (XS to XXL).

## Validation Ranges

### Men's Measurements (inches)
- **Chest**: 34-52
- **Waist**: 28-44  
- **Hips**: 34-48
- **Shoulder**: 15-22
- **Sleeve Length**: 22-27
- **Neck**: 14-20

### Women's Measurements (inches)
- **Bust**: 30-46
- **Waist**: 24-40
- **Hips**: 32-48
- **Shoulder**: 13-19
- **Upper Arm**: 10-16

## Implementation

### 1. Database Schema
- **Table**: `CustomerMeasurements`
- **CHECK Constraints**: Gender-specific validation
- **Triggers**: Custom error messages
- **Stored Procedures**: Safe insertion with validation

### 2. API Endpoints

#### POST `/api/customermeasurement`
Create new measurement with validation
```json
{
  "customerId": 1,
  "gender": "M",
  "measurementType": "Chest",
  "measurementValue": 38.5,
  "unit": "inches"
}
```

#### GET `/api/customermeasurement/validate`
Validate measurement before saving
```
GET /api/customermeasurement/validate?gender=M&measurementType=Chest&value=38.5
```

#### GET `/api/customermeasurement/ranges/{gender}`
Get valid ranges for gender
```
GET /api/customermeasurement/ranges/M
```

### 3. Error Messages
- "Invalid chest measurement for men - must be 34-52 inches"
- "Invalid waist measurement for women - must be 24-40 inches"

## Usage

### Database Setup
1. Run `MeasurementValidationSchema.sql`
2. Apply Entity Framework migration
3. Test with `SampleTestData.sql`

### API Testing
```bash
# Valid measurement
curl -X POST "https://localhost:7000/api/customermeasurement" \
  -H "Content-Type: application/json" \
  -d '{"customerId":1,"gender":"M","measurementType":"Chest","measurementValue":38.5}'

# Invalid measurement (will return error)
curl -X POST "https://localhost:7000/api/customermeasurement" \
  -H "Content-Type: application/json" \
  -d '{"customerId":1,"gender":"M","measurementType":"Chest","measurementValue":30.0}'
```

## Files Created
- `Models/CustomerMeasurement.cs` - Entity model
- `Services/MeasurementValidationService.cs` - Business logic
- `Controllers/CustomerMeasurementController.cs` - API endpoints
- `Database/MeasurementValidationSchema.sql` - Database schema
- `Database/SampleTestData.sql` - Test data
- `Migrations/AddCustomerMeasurementValidation.cs` - EF migration