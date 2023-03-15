/*
Rayyan Hamid - u1298801
CS 3505 - Fall 2022
A2 - Classes, Facades, and Makefiles
*/

#ifndef A2_HARUPDF_H
#define A2_HARUPDF_H
#include "hpdf.h"

//Declaring all the methods stubs and variables used in .cpp file
class HaruPDF
{
public:
    HaruPDF();
    void plotCharacters(char character, float angle, float x, float y);
    void saveDocument(const char *fname);

private:
    HPDF_Doc pdf;
    HPDF_Page page;
    HPDF_Font font;
};

#endif