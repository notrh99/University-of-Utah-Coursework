#ifndef FRAMEVIEWER_H
#define FRAMEVIEWER_H

#include <QWidget>
#include "spritemodel.h"

namespace Ui { class FrameViewer; }

class FrameViewer : public QWidget
{
    Q_OBJECT

public:
    explicit FrameViewer(SpriteModel& model, QWidget *parent = nullptr);
    ~FrameViewer();
    void setText(QString);

private:
    Ui::FrameViewer *ui;
    SpriteModel *model;

};

#endif // FRAMEVIEWER_H
