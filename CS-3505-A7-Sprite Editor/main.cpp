#include "mainwindow.h"

#include <QApplication>

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    //MainWindow w;
    SpriteModel model;
    MainWindow w(model);
    w.show();
    return a.exec();
}
