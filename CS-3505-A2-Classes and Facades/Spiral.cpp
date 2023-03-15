
/*
Rayyan Hamid - u1298801
CS 3505 - Fall 2022
A2 - Classes, Facades, and Makefiles
*/

#include "Spiral.h"
#include <math.h>
#include <iostream>

/*
This method is a constructor for the Spiral class. It takes in the x,y coordinates and the starting angle and radius.
*/
Spiral::Spiral(double centerX, double centerY, double startingAngle, double startingRadius)
{
    _centerX = centerX;
    _centerY = centerY;
    latestAngle = startingAngle;
    latestRadius = startingRadius;

    if (startingRadius < 25)
    {
        latestAngle = 25;
    }

    // Calculating the spiral and text angle for the letter
    latestLetterAngle = (latestAngle + 180) / 180 * M_PI;
    latestRadianAngle = (latestAngle - 90) / 180 * M_PI;
    // Updating text posstion
    latestX = _centerX + cos(latestRadianAngle) * (latestRadius);
    latestY = _centerY + sin(latestRadianAngle) * (latestRadius);

    latestRadius++;
};

/*
This method performs calculations to find the right spot to place the letter.
It also updates the classes variables each time a letter is placed.
*/
void Spiral::changeCharacterPosition()
{
    // Controlling spacing between letters
    latestAngle = latestAngle - (1000 / latestRadius);
    // Calculating the spiral and text angle for the letter
    latestLetterAngle = (latestAngle + 180) / 180 * M_PI;
    latestRadianAngle = (latestAngle - 90) / 180 * M_PI;
    // Updating text posstion
    latestX = _centerX + cos(latestRadianAngle) * (latestRadius);
    latestY = _centerY + sin(latestRadianAngle) * (latestRadius);

    latestRadius++;
}

// This method overloads the postfix ++ operator.
Spiral Spiral::operator++(int counter)
{
    Spiral output(*this);
    ++(*this);
    return output;
}

// This method overloads the prefix ++ operator.
Spiral &Spiral::operator++()
{
    this->changeCharacterPosition();
    return *this;
}

// This getter method returns the x value of the letter.
double Spiral::getTextX()
{
    return latestX;
}
// This getter method returns the y value of the letter.
double Spiral::getTextY()
{
    return latestY;
}
// This getter method returns the degree angle of the letter.
double Spiral::getLetterAngle()
{
    return latestAngle;
}
// This getter method returns the degree angle of the letter.
double Spiral::getLetterAngleRadian()
{
    return latestLetterAngle;
}