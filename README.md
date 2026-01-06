# dotnet-utilities

A collection of .NET utilities and helper functions.  
Feel free to copy and use any code from this repository in your own projects!

## Features

### ROC (Republic of China) Calendar Types

Custom date/time types for Taiwan's ROC calendar system:

- **RocDateTime** - A `DateTime` equivalent using ROC year (e.g., 114 instead of 2025)
- **RocDateOnly** - A `DateOnly` equivalent using ROC year
- Includes JSON and EF Core converters for serialization and database mapping

### Statistics

Basic statistical calculation extensions:

- **Average**
- **Stdev**
- **Deviation**
- **LinearRegression**
- **RSquared** - Coefficient of determination
