#include "gamemodel.h"
#include <QTimer>
#include <iostream>

GameModel::GameModel(QWidget *parent)
    : QMainWindow{parent}
{
    currentRound = 0;
    currentRoundAmmo = 20;
    score = 0;
}

void GameModel::updateScoreOnCollision()
{
    score = score + (1000);
    emit sendScoreText(score);
}

void GameModel::startGame()
{
    emit goToTeachingScreen(currentRound);
    emit sendGameAmmoAmount(currentRoundAmmo);
}

void GameModel::goToNextRound()
{
    emit goToTeachingScreen(currentRound);
    emit updateSprites(currentRound);
}

void GameModel::exitTeachingMenu()
{
    if(currentRound == 0)
        emit goToTeachingScreen(++currentRound);
    else
        emit goToIntermission(currentRound);
}

void GameModel::resetGame()
{
    currentRound = 0;
    emit sendScoreText(0);
    emit updateSprites(currentRound);
}

void GameModel::roundOver(int collisions)
{
    emit endRound(currentRound++);
    emit updateCollisions(collisions);
}


