#ifndef GAMEMODEL_H
#define GAMEMODEL_H

#include <QMainWindow>


class GameModel : public QMainWindow
{
    Q_OBJECT
public:
    explicit GameModel(QWidget *parent = nullptr);
    int score;

public slots:
    void startGame();
    void exitTeachingMenu();
    void goToNextRound();
    void resetGame();
    void roundOver(int);
    void updateScoreOnCollision();

signals:
    void hideStartScreen();
    void updateSprites(int);
    void changeDuckSize(int);
    void changeBulletSize(int);
    void changeResultantSize(int);
    void dropProtons(bool);
    void dropPositrons(bool);
    void dropNeutrinos(bool);
    void dropGammaRays(bool);
    void setBulletSprite(QString);
    void setResultantSprite(QString);
    void setDuckSprite(QString);
    void setDuckVelocity(int);
    void goToIntermission(int);
    void endRound(int);
    void goToTeachingScreen(int);
    void updateCollisions(int);
    void sendScoreText(int);
    void sendGameAmmoAmount(int);

private:
    int currentRound;
    int currentRoundAmmo;
};

#endif // GAMEMODEL_H
