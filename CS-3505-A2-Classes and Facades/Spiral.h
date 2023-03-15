/*
Rayyan Hamid - u1298801
CS 3505 - Fall 2022
A2 - Classes, Facades, and Makefiles
*/

// Including Guards
#ifndef A2_SPIRAL_H
#define A2_SPIRAL_H
#include <iostream>

// Declaring all the methods stubs and variables used in .cpp file
class Spiral
{
    double _centerX;
    double _centerY;
    double latestX;
    double latestY;
    double latestAngle;
    double latestRadius;
    double latestLetterAngle;
    double latestRadianAngle;
    int counter; 
    double newRadius;

public:
    Spiral(double centerX, double centerY, double startingAngle, double startingRadius);
    Spiral operator++(int counter);
    Spiral &operator++();
    void changeCharacterPosition();
    double getTextX();
    double getTextY();
    double getLetterAngle();
    double getLetterAngleRadian();
};

#endif