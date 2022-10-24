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
