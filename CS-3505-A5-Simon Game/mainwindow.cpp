/*
 * Author: Krish Mahtani & Rayyan Hamid
 * Date: 28th October 2022
 * A5: Qt Simon Game
*/
#include "MainWindow.h"
#include "ui_MainWindow.h"
#include "Model.h"
#include <QTimer>
#include <QTime>
#include <QPixmap>

MainWindow::MainWindow(Model& model, QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    connect(ui->startButton, &QPushButton::clicked,
            &model, &Model::startButtonClicked);
//Disable the start button when its clicked
    connect(&model, &Model::disableButton,
            this, &MainWindow::on_startButton_clicked);
//Send a signal when the red button is clicked
    connect(ui->redButton, &QPushButton::clicked,
            &model, &Model::redButtonClicked);
//Send a signal when the blue button is clicked
    connect(ui->blueButton, &QPushButton::clicked,
            &model, &Model::blueButtonClicked);
//Flash the red button according to the sequence
    connect(&model, &Model::changeRedButtonColour,
            this, &MainWindow::flashRedButton);
//Flash the blue button according to the sequence
    connect(&model, &Model::changeBlueButtonColour,
            this, &MainWindow::flashBlueButton);
//Display a game lost message when the player does a wrong more
    connect(&model, &Model::youLoseMessage,
            this, &MainWindow::gameLost);
//Display message when its players turn
    connect(&model, &Model::yourTurnMessage,
            this, &MainWindow::afterSimonsSequence);
//Update progress bar as the game progresses
    connect(&model, &Model::progressBarUpdate,
            ui->progressBar,&QProgressBar::setValue);
//Display a game win message when the player gets the round right
    connect(&model, &Model::winMessage,
            this, &MainWindow::gameWin);
}

MainWindow::~MainWindow()
{
    delete ui;
}

///////////
///Slots///
///////////

//When the start button is clicked, it will set it to false
//and set the view for the simons sequence to be displayed
void MainWindow::on_startButton_clicked()
{
   ui->startButton->setEnabled(false);
   ui->label_2->setVisible(false);
   ui->redButton->setStyleSheet("background-color:red");
   ui->blueButton->setStyleSheet("background-color:blue");
   ui->progressBar->setValue(0);
   ui->label->setText("             Simon's Turn.");
   delayTime(200);

}

//When the red button is clicked it will flash to white and back to red
void MainWindow::on_redButton_clicked()
{
    ui->redButton->setStyleSheet( QString("QPushButton {background-color: rgb(255,0,0);} QPushButton:pressed {background-color: rgb(255,255,255);}"));
}

//When the blue button is clicked it will flash to white and back to blue
void MainWindow::on_blueButton_clicked()
{
    ui->blueButton->setStyleSheet( QString("QPushButton {background-color: rgb(0,0,255);} QPushButton:pressed {background-color: rgb(255,255,255);}"));
}

/////////////
///Methods///
/////////////


//This method will flash the red button when it appears on the
//sequence. It will decrease the delay time between flashes
//as the game progresses.
void MainWindow::flashRedButton(int i)
{
     ui->redButton->setStyleSheet("background-color: white");
     if(i*50 > 500)
        delayTime(100);
     else
         delayTime(500-(i*50));
     ui->redButton->setStyleSheet("background-color: red");

     if(i*50 > 500)
        delayTime(100);
     else
         delayTime(500-(i*50));
}

//This method will flash the blue button when it appears on the
//sequence. It will decrease the delay time between flashes
//as the game progresses.
void MainWindow::flashBlueButton(int i)
{
    ui->blueButton->setStyleSheet("background-color: white");
    if(i*50 > 500)
       delayTime(100);
    else
        delayTime(500-(i*50));
    ui->blueButton->setStyleSheet("background-color: blue");

    if(i*50 > 500)
       delayTime(100);
    else
        delayTime(500-(i*50));
}

//This method display a game win message after each round and
//disables the red and blue buttons so the next round can start.
void MainWindow::gameWin()
{
    ui->label->setText("You Win, press Continue for next round.");
    ui->startButton->setText("Continue");
    ui->startButton->setEnabled(true);
    ui->blueButton->setEnabled(false);
    ui->redButton->setEnabled(false);
}

//This method display a game lost message after the player has
//made a wrong sequence move. It also include the special feature.
//Special Feature - It display's an image when you loss.
void MainWindow::gameLost()
{
    ui->label_2->setVisible(true);
    ui->label->setText("             You Loss!");
    //Locate the image in the resource folder
    QPixmap pix(":/resources/image/youlose.jpg");
    //set width and height according to label dimensions.
    int width = ui->label_2->width();
    int heigth= ui->label_2->height();
    //Displays the image
    ui->label_2->setPixmap(pix.scaled(width,heigth,Qt::KeepAspectRatio));
    ui->startButton->setText("New Game");
    ui->startButton->setEnabled(true);
    ui->blueButton->setEnabled(false);
    ui->redButton->setEnabled(false);
}

//This methods sets the screen for the players turn.
void MainWindow::afterSimonsSequence()
{
    ui->progressBar->setVisible(true);
    ui->blueButton->setEnabled(true);
    ui->redButton->setEnabled(true);
    ui->label->setText("             Your Turn");

}

//This methods takes in milliseconds and delays the method where it
//was called from.
//https://stackoverflow.com/questions/3752742/how-do-i-create-a-pause-wait-function-using-qt
void MainWindow::delayTime(int time)
{
    QTime endTime= QTime::currentTime().addMSecs(time);
    while (QTime::currentTime() < endTime)
        QCoreApplication::processEvents(QEventLoop::AllEvents, 100);
}








