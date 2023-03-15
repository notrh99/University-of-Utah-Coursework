#ifndef GAMESCREEN_H
#define GAMESCREEN_H

#include "gamemodel.h"
#include <QWidget>

namespace Ui {
class GameScreen;
}

class GameScreen : public QWidget
{
    Q_OBJECT

public:
    explicit GameScreen(GameModel& model, QWidget *parent = nullptr);
    ~GameScreen();

public slots:
    void updateScoreText(int);

private:
    Ui::GameScreen *ui;
};

#endif // GAMESCREEN_H
