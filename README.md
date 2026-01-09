# dotnet-utilities

A collection of .NET utilities and helper functions.  
Feel free to copy and use any code from this repository in your own projects!

## Features

### A bunch of C# Extensions

Includes extensions for:
- Collections
- Strings
- DateTime?, DateOnly? and TimeOnly?
- Numbers
- Enums
- Task? and ValueTask?
- Safe fire and forget for async methods
- Control Flow

### ROC (Republic of China) Calendar Types

Custom date/time types for Taiwan's ROC calendar system:

- **RocDateTime** - A `DateTime` equivalent using ROC year (e.g., 114 instead of 2025)
- **RocDateOnly** - A `DateOnly` equivalent using ROC year
- Includes System.Text.Json, EntityFrameworkCore and Dapper converters for serialization and database mapping

### Statistics

Basic statistical calculation extensions:

- **Average**
- **Stdev**
- **Deviation**
- **LinearRegression**
- **RSquared** - Coefficient of determination
