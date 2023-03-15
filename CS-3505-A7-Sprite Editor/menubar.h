#ifndef MENUBAR_H
#define MENUBAR_H

#include <QWidget>
#include "spritemodel.h"
#include "QMenuBar"

namespace Ui { class MenuBar; }

class MenuBar : public QWidget
{
    Q_OBJECT

public:
    explicit MenuBar(SpriteModel& model, QWidget *parent = nullptr);
    ~MenuBar();

private:
    Ui::MenuBar *ui;
    SpriteModel *model;


};

#endif // MENUBAR_H
