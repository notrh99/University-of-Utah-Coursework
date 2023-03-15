/*
 * Author: Krish Mahtani & Rayyan Hamid
 * Date: 28th October 2022
 * A5: Qt Simon Game
*/
#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QTimer>
#include <QTime>
#include <QMainWindow>
#include "Model.h"

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

private:
    Ui::MainWindow *ui;

public:
    MainWindow(Model& model, QWidget *parent = nullptr);
    ~MainWindow();

public slots:
    void flashRedButton(int);
    void flashBlueButton(int);
    void gameLost();
    void afterSimonsSequence();
    void gameWin();

private slots:
   void on_startButton_clicked();
   void on_redButton_clicked();
   void on_blueButton_clicked();

private:
   void delayTime(int time);
     void setProgessBar(int percentage);


};
#endif // MAINWINDOW_H
