#ifndef EDITINGWINDOW_H
#define EDITINGWINDOW_H

#include <QWidget>
#include <QMouseEvent>
#include <QPainter>
#include "spritemodel.h"

namespace Ui {
class EditingWindow;
}

class EditingWindow : public QWidget
{
    Q_OBJECT

public:
    explicit EditingWindow(SpriteModel& model, QWidget *parent = nullptr);
    ~EditingWindow();
    QImage myImage;
    void mousePressEvent(QMouseEvent *ev);
    void mouseMoveEvent(QMouseEvent *ev);
    void loadImageToView(QImage image);
    QPoint pos;
    int heigth;
    int width;
    int windowHeight;
    int windowWidth;
    QImage background;
    QImage createBackgroundImage(int x, int y);
    void setHeightWidth(int height, int width);
    void placeBackground(int height, int width);

    //void setText(QString);

signals:
    void mousePressed(QPoint);
    void sendWindowDimensions(int, int);



private:
    Ui::EditingWindow *ui;
    SpriteModel *model;
};

#endif // EDITINGWINDOW_H
