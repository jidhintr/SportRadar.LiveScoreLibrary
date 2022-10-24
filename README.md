# SportRadar.LiveScoreLibrary

The document describes the implementation logic of Live Football match reporting in a library project.
Detailed requirements document is provided next to this wiki page. However this wiki describes the technical `assumptions` made, `logics` & `rules` applied while implementing it.

Also, it includes the technical guide to consume in the client-apps.

___
# Requirement 
Implement a simple 'Live WorldCup Football Score Board', as a Library project for the client-apps to consume it and control the whole workflow of the match.

### Environment & Tooling
  - Windows eco-system 
      - Visual studio 2022
      - NUnit for testing framework 
      - NCrunch for better Test Experience 
      - 
  - Architecture 
    - X64 based DLL, Library project
    - .NET 6 framework for DLL  
    - C# 10 

### Prerequisite & Usage
- Consume the DLL in any .NET eco systems a version ***not lower*** than `6.0` and uses some `C# 10` 
- X64 is the default configuration used to build the package


## Functional Requirement
Basically the DLL exposes 4 broad category of functions.
- `Start Game` -> Initialize the game with 2 team with score (0,0)
- `Update Score` -> Keep updating the score of both teams same time
- `Summary` -> Provide the live score of **games under progress** [ + Sorting Logic]
- `Finished` -> Once the game ends, remove from the summary [No history saved]


## Assumptions made
- All above 4 exposed functionalities requires external interventions.
- Independent of the client / API that which consumes this DLL, its not capable to run the whole workflow.
- Avoid primitives types to Consider extensibility of the app
- One or more games are happening at the same time; so scores has to be per game & team


# Usage 
How client uses the library and call the four functionalities.

> code goes here
>
> Interface exposes 