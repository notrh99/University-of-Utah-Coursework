#include "intermission.h"
#include "ui_intermission.h"

Intermission::Intermission(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::Intermission)
{
    ui->setupUi(this);

    ui->startLabel->setPixmap(QPixmap(":/TestPrefix/start.png"));
    ui->startLabel->setScaledContents(true);
    ui->ammoLabel->setPixmap(QPixmap(":/TestPrefix/ammo.png"));
    ui->ammoLabel->setScaledContents(true);
    ui->getReadyLabel->setPixmap(QPixmap(":/TestPrefix/getReady.png"));
    ui->getReadyLabel->setScaledContents(true);
    ui->collisionsLabel->setPixmap(QPixmap(":/TestPrefix/collisions.png"));
    ui->collisionsLabel->setScaledContents(true);
    ui->collisionsLabel->setVisible(false);

    ui->startLabel->setVisible(false);
    ui->warningLabel->setVisible(false);
    ui->nextRound->setVisible(false);
    ui->nextRound->setEnabled(false);

    roundStart = new QTimer();
    countdown = new QMovie(":/TestPrefix/countdown.gif");
    if (countdown->isValid())
    {
        ui->countdownLabel->setMovie(countdown);
        ui->countdownLabel->setScaledContents(true);
    }

    connect(countdown,
            &QMovie::frameChanged,
            this,
            &Intermission::stopCountdownIfNecessary);

    connect(ui->nextRound,
            &QPushButton::clicked,
            [this](){emit goToNextRound();});
}

void Intermission::updateIntermission(int roundNumber)
{

}

void Intermission::setCollisions(int collisions)
{
    QString collisionsText;
    collisionsText.append("X");
    collisionsText.append(QString::number(collisions));
    ui->ammoQuantityLabel->setText(collisionsText);
}

void Intermission::setUpIntermission(int roundNumber)
{
    ui->startLabel->setVisible(false);
    ui->nextRound->setVisible(false);
    ui->nextRound->setEnabled(false);
    ui->collisionsLabel->setVisible(false);
    ui->countdownLabel->setVisible(true);
    ui->ammoLabel->setVisible(true);
    ui->ammoQuantityLabel->setVisible(true);
    ui->ammoPicture->setVisible(true);
    ui->getReadyLabel->setVisible(true);

    if(roundNumber == 0)
    {

    }
    else if(roundNumber == 1)
    {
        ui->roundTitle->setPixmap(QPixmap(":/TestPrefix/roundOne.png"));
        ui->roundTitle->setScaledContents(true);
        ui->ammoQuantityLabel->setText("X20");

        ui->ammoPicture->setPixmap(QPixmap(":/TestPrefix/H1"));
        ui->ammoPicture->setScaledContents(true);
    }
    else if(roundNumber == 2)
    {
        ui->roundTitle->setPixmap(QPixmap(":/TestPrefix/roundTwo.png"));
        ui->roundTitle->setScaledContents(true);
        ui->ammoPicture->setPixmap(QPixmap(":/TestPrefix/H1"));
        ui->ammoPicture->setScaledContents(true);
    }
    else if(roundNumber == 3)
    {
        ui->roundTitle->setPixmap(QPixmap(":/TestPrefix/roundThree.png"));
        ui->roundTitle->setScaledContents(true);
        ui->ammoPicture->setPixmap(QPixmap(":/TestPrefix/He3"));
        ui->ammoPicture->setScaledContents(true);
    }
    else
    {
        return;
    }

    emit showIntermission();
    roundStart->singleShot(2000, this, &Intermission::startCountdown);
}

void Intermission::endRound(int roundNumber)
{
    if(roundNumber == 1)
    {

    }
    else if(roundNumber == 2)
    {

    }
    else if(roundNumber == 3)
    {
        emit endGame();
        return;
    }

    ui->nextRound->setVisible(true);
    ui->nextRound->setEnabled(true);
    ui->collisionsLabel->setVisible(true);
    ui->ammoLabel->setVisible(false);
    ui->ammoPicture->setVisible(false);
    ui->roundTitle->setPixmap(QPixmap(":/TestPrefix/roundOver.png"));
    ui->getReadyLabel->setVisible(false);
    ui->startLabel->setVisible(false);
    emit showIntermission();
}

void Intermission::startCountdown()
{
    ui->getReadyLabel->setVisible(false);
    countdown->start();
}

void Intermission::askForTeachingScreen()
{
    //emit goToTeachingScreen();
}

void Intermission::stopCountdownIfNecessary(int frameNumber)
{
    if(frameNumber == (countdown->frameCount()-1))
    {
        countdown->stop();
        ui->countdownLabel->setVisible(false);
        ui->startLabel->setVisible(true);
        roundStart->singleShot(1000, this, &Intermission::sendGoToGameScreen);
    }
}

void Intermission::sendGoToGameScreen()
{
    emit goToSim();
}

Intermission::~Intermission()
{
    delete ui;
}
