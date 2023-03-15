#ifndef ANIMATIONPOPUP_H
#define ANIMATIONPOPUP_H

#include <QDialog>
#include <QTimer>
#include "spritemodel.h"


namespace Ui {
class AnimationPopup;
}

class AnimationPopup : public QDialog
{
    Q_OBJECT

public:
    explicit AnimationPopup(SpriteModel& model, QWidget *parent = nullptr);
    ~AnimationPopup();
    bool actualSize;
    int currentFPS;
signals:
    void fpsSliderAction(int);
    void fpsWindowOpen(bool);


private slots:
    void loadImageToView(QImage image);
    void closeEvent(QCloseEvent *bar);
    void actualSizeBool(bool actualSize);
    void setDisplayFPS(int);

private:
    Ui::AnimationPopup *ui;
    SpriteModel *model;
};

#endif // ANIMATIONPOPUP_H
