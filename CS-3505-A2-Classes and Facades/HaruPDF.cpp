/*
Rayyan Hamid - u1298801
CS 3505 - Fall 2022
A2 - Classes, Facades, and Makefiles
*/

#include "HaruPDF.h"
#include "hpdf.h"
#include <math.h>

/*
This method is a constructor to the class for making and setting up the PDF file.
*/
HaruPDF ::HaruPDF()
{
    pdf = HPDF_New(NULL, NULL);
    // adding a page object
    page = HPDF_AddPage(pdf);
    // Setting page size
    HPDF_Page_SetSize(page, HPDF_PAGE_SIZE_A5, HPDF_PAGE_PORTRAIT);
    HPDF_Page_SetTextLeading(page, 20);
    HPDF_Page_SetGrayStroke(page, 0);
    //Starting to accept text to the PDF.
    HPDF_Page_BeginText(page);
    //Setting the desired font
    font = HPDF_GetFont(pdf, "Courier-Bold", NULL);
    //Setting a desirable font size
    HPDF_Page_SetFontAndSize(page, font, 30);
};

/*
This method adds letters to the PDF, it takes in the letter and the correct spot and angle. 
*/
void HaruPDF::plotCharacters(char character, float angle, float x, float y)
{
    char buffer[2];
    //Generates the letter at the correct spot
    HPDF_Page_SetTextMatrix(page, cos(angle), sin(angle), -sin(angle), cos(angle), x, y);
    //Character Displayed
    buffer[0] = character;
    buffer[1] = 0;
    //Storing the letters on the PDF
    HPDF_Page_ShowText(page, buffer);
}
/*
Saving the document according to the name provided. 
*/
void HaruPDF::saveDocument(const char *fname)
{
    HPDF_Page_EndText(page);
    //Save document to file
    HPDF_SaveToFile(pdf, fname);
    //Clean Up
    HPDF_Free(pdf);
}
