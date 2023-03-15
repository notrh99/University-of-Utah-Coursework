/*
 * Author: Krish Mahtani & Rayyan Hamid
 * Date: 28th October 2022
 * A5: Qt Simon Game
*/
#include "MainWindow.h"
#include "Model.h"
#include <QApplication>

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    Model model;
    MainWindow w(model);
    w.show();
    return a.exec();
}
