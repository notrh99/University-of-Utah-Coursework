#ifndef STARTMENU_H
#define STARTMENU_H

#include "gamemodel.h"
#include <QWidget>

namespace Ui {
class StartMenu;
}

class StartMenu : public QWidget
{
    Q_OBJECT

public:
    explicit StartMenu(GameModel& model, QWidget *parent = nullptr);
    ~StartMenu();

private slots:
    void on_helpButton_clicked();

    void on_exitButton_clicked();

private:
    Ui::StartMenu *ui;
};

#endif // STARTMENU_H
