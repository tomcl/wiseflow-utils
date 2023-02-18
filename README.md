# Wiseflow Pearson discriminant

## Getting Started

To run this analysis on any system (Windows/Macos/Linux):

1. Download and unzip the latest release source code or fork and clone this repo.
2. Download an install [.NET SDK 7](https://dotnet.microsoft.com/en-us/download). (If you have .NET 6 SDK installed that will also work).
3. Replace `WFlow.xlxs` in **this directory** (the one containing this README file) by your Wiseflow mark output file (you must use that name)
4. To run the analysis and output Pearson discriminant data for your questions:
    * Option 1 (Windows only) double-click `Run.bat` in **this directory**
    * Option 2 (any platform)
        1. Start a command line in **this directory**
        2. run `dotnet run --project markAnalysis`

## What this utility does

This utility calculates the per-question **Pearson Discriminant** data that Blackboard MCQ analysis provides. It requires
a Wiseflow mark output file. The data can be used to check whether MCQ questions are suitable, 
and is very good at detecting if any questions have incorrect answers.

Pearson Discriminant (PD) values lie between -1 and 1. Interpret the results for each question as follows:

* PD < 0 => probably a wrong answer or impossible question
* PD < 0.1 => Unusually low - worth reviewing for errors
* PD > 0.1 => Ok
* PD > 0.3 => Fine


