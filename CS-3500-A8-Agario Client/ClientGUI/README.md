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

# Comments to Evaluators:

 We realized that it would be easier to draw the window - and everything else, in relation to the player. We did start by drawing the whole world, but realized that the math - was drawing
 everything from the player's perspective in the game world. It was very confusing at first.


# Assignment Specific Topics

Painting/Invalidating wasn't difficult after we realized what it was doing, but the math to get everything lined up was complicated. 


# Consulted Peers:

Piazza, Lab, and Lecture. Painting the UI was very complicated, because we didn't realize how to do it at first. We consulted the lab 12 to determine the drawing functions.

# References:

How we clipped the Game Window:

1. https://docs.microsoft.com/en-us/dotnet/api/system.drawing.graphics.setclip?view=dotnet-plat-ext-6.0
2. https://stackoverflow.com/questions/28523736/clipping-a-graphic-in-memory-before-drawing-on-form 

Font Sizes, so we can overlay on the player without it being offset.
3. https://stackoverflow.com/questions/22771878/c-sharp-getfontsize-function
4. https://docs.microsoft.com/en-us/dotnet/api/system.drawing.graphics.measurestring?view=windowsdesktop-6.0#system-drawing-graphics-measurestring(system-string-system-drawing-font)

Mouse Controls
5. https://stackoverflow.com/questions/19164933/cursor-position-relative-to-application
6. https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.control.pointtoclient?view=windowsdesktop-6.0
7. https://stackoverflow.com/questions/1913682/control-pointtoclient-vs-pointtoscreen

TimerDocumentation
8. https://docs.microsoft.com/en-us/dotnet/api/system.timers.timer?view=net-6.0 