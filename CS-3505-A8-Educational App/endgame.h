#ifndef ENDGAME_H
#define ENDGAME_H

#include <QWidget>
#include <QTimer>

namespace Ui {
class EndGame;
}

class EndGame : public QWidget
{
    Q_OBJECT

public:
    explicit EndGame(QWidget *parent = nullptr);
    ~EndGame();

public slots:
    void stopExplosionIfNecessary(int);
    void stopWinAnimationIfNecessary(int);
    void setUpEndGameScreen(int);
    void sendReplayGameSignal();
    void sendGoHomeSignal();
    void showHomeButton();

signals:
    void signalStopExplosion();
    void replayGame();
    void goHome();
    void setUpExplosion();
    void showEndGameScreen();
    void setUpFinalTeachingRound(int);

private:
    Ui::EndGame *ui;
    int animationFrameCounter;
    QMovie *explosionMovie;
    QMovie *winAnimation;
    void setUpScreenForExplosion();
    void playExplosion();
    void displayGameOver();
    void setUpWinScreen();
    void playWinScreenAnimation();
    void displayWin();
    void clearScreenLeaveBackground();
    QTimer* winScreenTimer;

};

#endif // ENDGAME_H
