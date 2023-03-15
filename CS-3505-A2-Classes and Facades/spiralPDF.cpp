/*
Rayyan Hamid - u1298801
CS 3505 - Fall 2022
A2 - Classes, Facades, and Makefiles
*/

#include <iostream>
#include "HaruPDF.h"
#include "Spiral.h"
#include <string.h>
#include <math.h>
#include "hpdf.h"

/*
This main methods calls the Spiral and HaruPDF classes, to generate a spiral using the text
povided in a PDF file.
*/
int main(int argc, char **argv)
{
    // If the text provided in not long enough, we end the program.
    if (argc < 2)
    {
        std::cout << "Input not valid." << std::endl;
        return 0;
    }

    char fname[256] = "spiralPDF.pdf";
    HaruPDF pdf;
    Spiral spiral(210, 300, 180, 75);
    const char *text = argv[1];
    unsigned int i;
    
    for (i = 0; i < strlen(text); i++)
    {
        pdf.plotCharacters(text[i], spiral.getLetterAngleRadian() , spiral.getTextX(), spiral.getTextY());
        spiral++;
    };
    pdf.saveDocument(fname);
}
