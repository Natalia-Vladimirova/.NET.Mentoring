## Task 9 - Logging and Monitoring

### General

The goal of the task is to add performance counters and logging into existing project. By default, it is suggested to use the MvcMusicStore application, updated on ASP.Net MVC 5.

### Subtask 1. Performance Counters

Add several performance counters into the project:
+ a number of successful LogIn;
+ a number of successful LogOff;
+ one on your choice.

To register and remove counters it may need a separate application (which may need to be run with elevated privileges).  
To perform the task it is possible to use either System.Diagnostics class or third-party libraries like [Performance Counter Helper](https://perfmoncounterhelper.codeplex.com/).

### Subtask 2. Logging

Add logging to the project. It needs to follow the next rules:
+ support several logging levels (at least Error, Info, Debug);
+ logging should be switched on/off via configuration file.

### Subtask 3. Collection and analysis of logs

Create log report generator using [Log Parser](https://www.microsoft.com/en-us/download/details.aspx?id=24659) command line or API. The report should contain:
+ total number of records of each type;
+ a list of errors (records with Error code).

### Note
To check work of performance counters:
+ run cmd as administrator and go to bin folder of MvcMusicStore app;
+ execute a command: PerformanceCounterHelper.Installer.exe "<path_to_MvcMusicStore>\bin\MvcMusicStore.dll";
+ open Performance Monitor (press Windows+R and type perfmon);
+ choose added performance counters
