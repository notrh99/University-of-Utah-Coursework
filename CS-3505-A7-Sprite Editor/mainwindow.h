#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QMenuBar>
#include <QMenu>
#include <QGridLayout>
#include <QLabel>
#include <QPixmap>
#include <QMouseEvent>
#include <QTimer>
#include <QFileDialog>
#include <iostream>
#include "spritemodel.h"
#include "toolbar.h"
#include "editingwindow.h"
#include "frameviewer.h"
#include "animationpopup.h"
#include "newprojectpopup.h"
#include "borderlayout.h"

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(SpriteModel& model, QWidget *parent = nullptr);
    ~MainWindow();
    ToolBar *tool;
    EditingWindow *edit;
    FrameViewer *frameViewer;
    AnimationPopup *animationPopUp;
    newProjectPopup *newProjectPop;
    QTimer *timer;
    int framesPerSecond;
    //MenuBar *menuBar;

public slots:
    void openAnimationPopup();
    void updateFPS(int);
    void openLoadFileDialog();
    void openSaveFileDialog();
    void openNewFilePopup();

private:
    Ui::MainWindow *ui;

signals:
    void loadFile(QString);
    void saveFile(QString);
};
#endif // MAINWINDOW_H
