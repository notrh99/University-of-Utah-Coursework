#include "endgame.h"
#include "ui_endgame.h"
#include <QMovie>
#include <QLabel>

EndGame::EndGame(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::EndGame)
{
    ui->setupUi(this);

    explosionMovie = new QMovie(":/TestPrefix/starexplosion2.gif");
    if (explosionMovie->isValid())
    {
        ui->explosionLabel->setMovie(explosionMovie);
        ui->explosionLabel->setScaledContents(true);
    }

    winAnimation = new QMovie(":/TestPrefix/winAnimation.gif");
    if (winAnimation->isValid())
    {
        ui->winAnimationLabel->setMovie(winAnimation);
        ui->winAnimationLabel->setScaledContents(true);
    }

    animationFrameCounter = 0;
    winScreenTimer = new QTimer(this);

    ui->youLoseLabel->setPixmap(QPixmap(":/TestPrefix/youLose.png"));
    ui->youLoseLabel->setScaledContents(true);

    ui->trophyLabel->setPixmap(QPixmap(":/TestPrefix/trophy.png"));
    ui->trophyLabel->setScaledContents(true);

    ui->youWinLabel->setPixmap(QPixmap(":/TestPrefix/youWin.png"));
    ui->youWinLabel->setScaledContents(true);

    ui->skullLabel->setPixmap(QPixmap(":/TestPrefix/skull.png"));
    ui->skullLabel->setScaledContents(true);

   // ui->splitter->setEnabled(false);

    connect(explosionMovie,
            &QMovie::frameChanged,
            this,
            &EndGame::stopExplosionIfNecessary);

    connect(winAnimation,
            &QMovie::frameChanged,
            this,
            &EndGame::stopWinAnimationIfNecessary);

    connect(ui->homeButton,
            &QPushButton::clicked,
            [this](){emit goHome();});
}

void EndGame::setUpEndGameScreen(int screenType)
{
    if(screenType == 0)
    {
        setUpScreenForExplosion();
        playExplosion();
    }
    else if(screenType == 1)
    {
        setUpWinScreen();
        playWinScreenAnimation();
    }

    emit showEndGameScreen();
}

void EndGame::setUpScreenForExplosion()
{
    clearScreenLeaveBackground();
    ui->explosionLabel->setVisible(true);
}

void EndGame::playExplosion()
{
    explosionMovie->start();
}

void EndGame::stopExplosionIfNecessary(int frameNumber)
{
    if(frameNumber == (explosionMovie->frameCount()-1))
    {
       explosionMovie->stop();
       displayGameOver();
    }
}

void EndGame::displayGameOver()
{
    ui->explosionLabel->setVisible(false);
    ui->youLoseLabel->setVisible(true);
    ui->homeButton->setVisible(true);
    ui->homeButton->setEnabled(true);
    ui->skullLabel->setVisible(true);
}

void EndGame::setUpWinScreen()
{
    clearScreenLeaveBackground();
    ui->winAnimationLabel->setVisible(true);
}

void EndGame::playWinScreenAnimation()
{
    winAnimation->start();
}

void EndGame::stopWinAnimationIfNecessary(int frameNumber)
{
    animationFrameCounter++;
    if(animationFrameCounter == ((winAnimation->frameCount()*10)-1))
    {
        displayWin();
        int finalTeachingRound = 4;
        winScreenTimer->singleShot(2000, [this, finalTeachingRound](){emit setUpFinalTeachingRound(finalTeachingRound);});
    }
}

void EndGame::displayWin()
{
    ui->trophyLabel->setVisible(true);
    ui->youWinLabel->setVisible(true);
}

void EndGame::showHomeButton()
{
    ui->homeButton->setVisible(true);
    ui->homeButton->setEnabled(true);
}

void EndGame::sendReplayGameSignal()
{
    emit replayGame();
}

void EndGame::sendGoHomeSignal()
{
    emit goHome();
}

void EndGame::clearScreenLeaveBackground()
{
    ui->skullLabel->setVisible(false);
    ui->youLoseLabel->setVisible(false);
    ui->youWinLabel->setVisible(false);
    ui->homeButton->setVisible(false);
    ui->homeButton->setEnabled(false);
    ui->winAnimationLabel->setVisible(false);
    ui->explosionLabel->setVisible(false);
    ui->trophyLabel->setVisible(false);
}

EndGame::~EndGame()
{
    delete ui;
}
