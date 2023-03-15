#ifndef INTERMISSION_H
#define INTERMISSION_H

#include <QWidget>
#include "qmovie.h"
#include "qtimer.h"

namespace Ui {
class Intermission;
}

class Intermission : public QWidget
{
    Q_OBJECT

public:
    explicit Intermission(QWidget *parent = nullptr);
    ~Intermission();

public slots:
    void startCountdown();
    void sendGoToGameScreen();
    void askForTeachingScreen();
    void setUpIntermission(int);
    void updateIntermission(int);
    void endRound(int);
    void stopCountdownIfNecessary(int);
    void setCollisions(int);

signals:
    void goToSim();
    void goToTeachingScreen(int);
    void goToNextRound();
    void endGame();
    void showIntermission();

private:
    Ui::Intermission *ui;
    QMovie *countdown;
    QTimer *roundStart;
    int nextRoundAmmo;
    int currentRoundScore;
};

#endif // INTERMISSION_H
