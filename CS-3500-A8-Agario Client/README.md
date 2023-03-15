```
Author:     Tyler DeBruin
Partner:    Rayyan Hamid
Date:       4-9-2022
Course:     CS 3500-001, University of Utah, School of Computing
GitHub ID:  TylerDeBruin & notrh99
Repo:       https://github.com/Utah-School-of-Computing-de-St-Germain/assignment-eight---agario-agario-knockoff.git
Commit #:   
Solution:   Agario
Copyright:  CS 3500 and Tyler DeBruin and Rayyan Hamid - This work may not be copied for use in Academic Coursework.
```

# Overview of the Agario Solution functionality
This Agario Solution provides a client that will connect to the Server.exe provided by the assignment specifications. The App draws a 'cell', with a radius provided
by the server, at the center of the screen. As more food is eaten, the cell grows - as the cell grows, the view of the Player expands so he can see more - and the states update on the screen.

WHen the player dies, he is put back to a 'Game Over' screen, which they can then use to change their name and restart the game.


# Partnership:
Rayyan and I pair programmed on the assignment. We did split off and he created the initial GUI while I, Tyler, created the base projects and readmes.

# Branching:
We branches for the initial creation of the GUI, but then paired on the remainder due to the complexity of the assignment.

# Testing:
We manually tested the assignment as we moved along, utilizing the logger and the UI to ensure we were handling stuff correctly.
There is a test project for the Communication.dll that we copied over from the last assignment, but we did not add any additional functionality.


# Time Expenditures:

    Assignment Eight - Predicted Hours: 12         
    
    Actual Hours:  13 Hours
    
    1 Hour spent creating the solution, Readmes, default projects. (Split between both of us.)
    5 Hours On the GUI (Rayyan 2, Tyler 3)
    3 Hours trying to follow a MVC Pattern, so that the client didn't explicitly call the networking code. (Split between both of us.)
    3 Hours setting testing against the server. 1 Hour testing against a remote server.  (Split between both of us.)


Time Estimations are getting better, but like the last assignment - This covered a lot of new stuff. We padded the expected time because we didn't know a lot of this. 