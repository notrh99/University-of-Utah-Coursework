#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include "gamemodel.h"
#include "qmediaplayer.h"
#include "simulationwidgit.h"
#include "startmenu.h"
#include <QMainWindow>
#include "endgame.h"
#include "startmenu.h"
#include "teachingmenu.h"
#include "gamescreen.h"
#include "intermission.h"

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(GameModel& model, QWidget *parent = nullptr);
    ~MainWindow();
    void startGame();
    SimulationWidgit* sim;
    void showTeachingMenu();
    void showHomeScreen();
    void transitionToGameScreen();
    void showIntermission();
    void showGameScreen();
    void hideEndGame();


signals:
    void failGame();
    void winGame();

private:
    EndGame *endGame;
    StartMenu *startMenu;
    TeachingMenu *teachingMenu;
    GameScreen *gameScreen;
    Intermission *intermission;

    QMediaPlayer* player;

    Ui::MainWindow *ui;
};
#endif // MAINWINDOW_H
