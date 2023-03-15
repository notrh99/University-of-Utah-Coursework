```
Author:     Tyler DeBruin
Partner:    Rayyan Hamid
Date:       4-27-2022
Course:     CS 3500-001, University of Utah, School of Computing
GitHub ID:  TylerDeBruin & notrh99
Repo:       https://github.com/Utah-School-of-Computing-de-St-Germain/assignment-nine---web-server-sql-up-in-the-clouds.git
Commit #:   7f3acd337ce1349f6fa6f79140b30b183f7e6d89
Solution:   WebServer
Copyright:  CS 3500 and Tyler DeBruin and Rayyan Hamid - This work may not be copied for use in Academic Coursework.
```

# Database Table Summary:

The Database consists of 3 tables. One for keeping track of the player, one for the Game, and one for the updating player states. 
This Webserver only utilizes the first two, because it can track all of the data neccessary to populate the highscores based on the game. 
The database tables are all relational:
-> Players tracks the PlayerName, and ID.
-> Game tracks the PlayerId, BornTime (When the Player Starts), DeathTime (When the player dies), MaxSize, and MaxSizeTime(When they hit maxsize).
-> PlayerSize tracks the players size at any instance of the game. GameId is referenced, because it ties to a specific game.

The Game Table tracks when a player starts, and ends a game. The client is responsible for persisting the GameId, and Updating MaxSize, and DeathTime.

# Partnership
Rayyan Hamid and Tyler DeBruin
40% / 50%

We paired on this - Tyler spent time alone working on the html pages.


# Extent of work 

The WebServer has the following features:
    - The ability to serve as a web server and return a basic welcome (index.html) page. (This runs on Port 11001).
    - The ability to return a web page with a chart of highscores. (Navigatable from the Home Page.)
    - The ability to take in a request to store a highscore.
    - The ability to return a web page showing a chart/graph/etc. of some other type of information that you save in your database.

All of these are viewable when launching the application. The index.html will display Hyperlinks to navigate to each page.
We also included an endpoint to setup database tables, and seed them with random data.

The solution uitilizes the Networking.dll from the previous assignment.

We were able to utilize Google's API for displaying a front end Bar Chart. The specifics are in the readme for the webserver project. Otherwise - Everything is pretty much accessible from the Index.html.


# Client Instrumentation 

We broke the connection to the database out into a Repository class. This is utilized by the client, and this webserver to read/write data.

You can see the two updated files here at this commit:  fb3b5ea7db8f9210f8a09dabe80b9bb8409f1536
https://github.com/Utah-School-of-Computing-de-St-Germain/assignment-eight---agario-agario-knockoff/commit/fb3b5ea7db8f9210f8a09dabe80b9bb8409f1536


# Branching:
Branched for initial setup of the Readmes/Project Files. All other features are on master.

# Testing:
Repository Class has Unit Tests in the WebServer.Tests project for Database Read/Writes to ensure it works correctly.
The Webserver was tested manually with Chrome and Edge browsers.


# Time Expenditures:

    Assignment Eight - Predicted Hours: 8         
    
    Actual Hours:  11 Hours
    
    1 Hour spent creating the solution, Readmes, default projects. (Split between both of us.)
    5 Hours On the U
    5 Hours Imp[lementing the Repository to setup the database, read/write, and to test it.

We tried to pad the expected time, thinking it would be moire straightforward than the previous assignments. There was a bit more introduced at once that made it more difficult to estimate.