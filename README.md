# TournamentTracker
My first 'huge' project in c# 

The tournament tracker program gives you the ability to create your oun tournaments and track how your favorite teams proceed.
When you get the code, open the solution in visual studio and set TrackerUI as a StartUp project. Run the program.
The program supports two modes SQL mode (With the connection to local SQL server) and TXT mode (Default mode, the text files will be created automatically). 

In order to use the SQL mode you need to have 'Microsoft SQL Server Management Studio':
1)In TrackerUi modify Program.cs file at the end of the line 22 change DatabaseType.TXT to DatabaseType.SQL 
2)Open the 'Microsoft SQL Server Management Studio' and create the database with the name 'Tournaments' (without the quotes)
3)After the database is created, double click to 'TournamentTracker.sql' file in the main folder and execute the query
Hopefully after these steps the SQL mode will work.  

The TXT mode is set by default and ready to launch without any preparations.
All the files are saved in the Project folder (TrackerLibrary\TextFiles). If for any reason you will face the issues with the text files,
most probably, the issue with the relative path that i specified for text files to be saved, in order to fix this bug:
1)Open the App.config file and at the line 4  change the path of the value="..." to whatever you prefer
