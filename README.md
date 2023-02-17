# Wiseflow Pearson discriminant

To run this analysis on any system (Windows/Macos/Linux)

1. Download and unzip the latest release source code or fork and clone this repo.
2. Install .NET SDK 7. (If you have .NET 6 SDK installed that will also work).
3. Put replace `WFlow.xlxs` in this directory (the one containing this README file) by your Wiseflow mark output file (you must use that name)
4. Start a command line terminal running in this directory.
5. Run command `dotnet run` in the terminal. The Pearson Disciminants for each Wiseflow question will be printed.

Pearson Discriminant (PD) values lie between -1 and 1. Interpret the results for each question as follows:

* < 0 => probably a wrong answer or impossible question
* < 0.1 => Unusually low - worth reviewing for errors
* > 0.1 => Ok
* > 0.3 => Fine


