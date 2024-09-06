# Trading Solution
Trading Solution is a .net web API project that manages sport players and depth chart.

This solution is for a coding challenge.

## Getting Started

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet) installed on your machine
- [Visual Studio Code](https://code.visualstudio.com/) or any C# IDE of your choice
- [VS Code C# Extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
- [Git](https://git-scm.com/)

## Installation

Clone the repository to your current location:

```bash
  git clone git@github.com:jasonz711/TradingSolution.git TradingSolution
```
    
## Usage

1. Open this project from Visual Studio Code.
2. Locate Solution tab on the left side of VS Code.
3. (Optional) Configure to use your prefered http port number from TradingSolution -> Properties -> launchSettings.json
4. In the Solution Explorer, right click TradingSolution and select Run
5. Once it's up and running, open this link from browser http://localhost:[port-number]/swagger/index.html" where port-number is configured in launchSettings.json

Alternatively, within TradingSolution main project, run
```bash
run --project TradingSolution.csproj
```
APIs are exposed and can be tested through Swagger UI interface.


## Running Tests

To run tests, in Solution Explorer, right click TradingSolution.Test project, select Test

Alternatively, run the following command within TradingSolution.Test project

```bash
  dotnet test TradingSolution.Test.csproj
```


## Appendix

### Assumptions
- It stimulates database with in-memory lists to store players and depth chart details. 
- It only has one sport type NFL.
- All players are on the same team.
- Player number is a unique identifier. Attempting to add a different player with an existing player number is not allowed.
- The top of player's depth is 1. Any number below it as an input is treated as null.