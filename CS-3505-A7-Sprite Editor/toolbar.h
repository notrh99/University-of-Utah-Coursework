#ifndef TOOLBAR_H
#define TOOLBAR_H

#include <QWidget>
#include <QColorDialog>
#include <QPalette>
#include "spritemodel.h"

namespace Ui { class ToolBar; }

class ToolBar : public QWidget
{
    Q_OBJECT

public:
    explicit ToolBar(SpriteModel& model, QWidget *parent = nullptr);
    ~ToolBar();
    void colorSelectionButtonClicked();
    QColorDialog *colorSelector;

signals:
    void colorChanged(uint);
    void animationPopUp();
    void penSizeIncreased();
    void penSizeDecreased();

public slots:
    void disableEraserHighlight();
    void disablePenHighlight();

private:
    Ui::ToolBar *ui;
    SpriteModel *model;

};

#endif // TOOLBAR_H
