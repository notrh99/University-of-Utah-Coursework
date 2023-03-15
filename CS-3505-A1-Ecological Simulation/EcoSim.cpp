/*
Rayyan Hamid
A1 - ECOSIM
EcoSim - We are plotting the change in rabbits and foxs populations in an ascii chart.
We take the users input parameter and use them in the Lotka-Vollterra equations.
*/
#include <iostream>
#include <math.h>
using namespace std;
/*
 This function takes in the parameter for the Lotka-Volterra equations
 and updates the raddits and foxes populations.
*/
void updatePopulations(double g, double p, double c, double m, double K,
                       double &numRabbits, double &numFoxes)
{

    double deltaRabbit;
    double deltaFoxes;

    // Lotka-Volterra Equations
    deltaRabbit = g * numRabbits * (1 - (numRabbits / K)) - p * numRabbits * numFoxes;
    deltaFoxes = (c * p * numRabbits * numFoxes) - (m * numFoxes);

    // Updating Population
    numRabbits += deltaRabbit;
    numFoxes += deltaFoxes;
}
/*
 This function takes in an int number and char a, and sends the number of spaces
 to cout. If number is less than one, char a is outputted.
*/
void plotCharacter(int number, char character)
{
    char space = ' ';
    // If there only one character we print it, else we print the number of spaces and the char.
    if (number == 0)
    {
        cout << character;
    }
    else
    {
        // Running a loop for the number of spaces.
        for (int i = 0; i < number; i++)
        {
            cout << space;
        }
        cout << character;
    }
}
/*
This function takes in the number of rabbits, foxes and a fractionscale, it
prints the ascii chart with "r", "F" and "*" respectively of the population
changes.
*/
void plotPopulations(double noOfRabits, double noOfFoxes, double fractionScale)
{

    char foxes = 'F';
    char rabbits = 'r';
    char overlap = '*';

    // Equation Provided
    int foxPosition = floor(noOfFoxes * fractionScale);
    int rabbitPosition = floor(noOfRabits * fractionScale);

    // We determine which letter needs to outputted first and modify
    // the next letters position accordingly.
    if (foxPosition > rabbitPosition)
    {
        foxPosition -= rabbitPosition + 1;
        plotCharacter(rabbitPosition, rabbits);
        plotCharacter(foxPosition, foxes);
    }
    else if (rabbitPosition > foxPosition)
    {
        rabbitPosition -= foxPosition + 1;
        plotCharacter(foxPosition, foxes);
        plotCharacter(rabbitPosition, rabbits);
    }
    // If there a tie between the rabbits and foxes populations
    //'*' is printed.
    else
    {
        plotCharacter(foxPosition, overlap);
    }
}

/*
The fucntion is used as a helper method, it incremnets the counter
and has a pointer to its integer parameter.
*/
void incrementCounter(int *counter)
{
    (*counter)++;
}

/*
This function takes in the population and number of iterations and runs
until either of them reache zero.
*/
void runSimulation(int iterations, double rabbits, double foxes)
{

    static const double rabbitGrowth = 0.2;
    static const double predationRate = 0.0022;
    static const double foxPreyConversion = 0.6;
    static const double foxMortalityRate = 0.2;
    static const double carryCapacity = 1000.0;
    static const double fractionScale = 0.1;
    // Plothing the initial values.
    plotPopulations(rabbits, foxes, fractionScale);
    // integer i is used to run the loop and gets incremented using the helper method.
    int i = 0;
    // Looping for each iteration and printing the populations accordingly.
    while (i < iterations && rabbits > 0 && foxes > 0)
    {
        // Calling updatePopulations with all the parameter.
        updatePopulations(rabbitGrowth, predationRate, foxPreyConversion, foxMortalityRate, carryCapacity, rabbits, foxes);
        // Calling plotPopulations to plot the updated population.
        plotPopulations(rabbits, foxes, fractionScale);
        cout << endl;
        incrementCounter(&i);
    }
}

/*
The main function asks the user for the initial populations, takes the input and
checks weither the input is valid.
*/
int main()
{
    double rabbitPopulation, foxPopulation;
    cout << "Enter initial population of rabbits ." << endl;
    // Taking input for the initial rabbit population.
    cin >> rabbitPopulation;
    cout << "Enter initial population of foxes." << endl;
    // Taking input for the initial rabbit population.
    cin >> foxPopulation;
    bool fail = cin.fail();
    // Checking if the input is of the correct type.
    if (cin.fail())
    {
        cerr << "Invalid input.";
        cout << endl;
        return 0;
    }
    // A Call to runSimulation with the initail populations.
    runSimulation(500, rabbitPopulation, foxPopulation);
}
