/**
 * Assignment: A7: Sprite Editor
 * Authors: Shad Boswell, Zack Freeman, Jacob Hopkins, and Rayyan Hamid
 *
 * This code cannot be used without permission.
 *
 * Citations for this entire project:
 *
 * Convert json string to object:
 *  https://stackoverflow.com/questions/26804660/how-to-initialize-qjsonobject-from-qstring
 * Layout:
 *  https://doc.qt.io/qt-6/qtwidgets-layouts-borderlayout-example.html
 * JSON in qt:
 *  https://doc.qt.io/qt-6/json.html
 *
 *  Reviewed by: Zack Freeman
 */
#include "mainwindow.h"
#include "ui_mainwindow.h"

/**
 * @brief MainWindow::MainWindow
 * @param model
 * @param parent
 *
 * This is the constructor for the MainWindow class.
 * The MainWindow contains all of the widgets necessary for the sprite editor to function
 * This class also sets the layout of the editor and contains connections
 * that pertain to the MainWindow class
 */
MainWindow::MainWindow(SpriteModel& model, QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    setWindowTitle(tr("Sprite Editor"));
    framesPerSecond = 1;

    animationPopUp = new AnimationPopup(model, this);
    tool = new ToolBar(model, this);
    edit = new EditingWindow(model, this);
    frameViewer = new FrameViewer(model, this);
    timer = new QTimer(this);

    newProjectPop = new newProjectPopup(model, this);

    QMenu *file = menuBar()->addMenu("&File");
    auto *save = new QAction("&Save", this);
    auto *load = new QAction("&Load", this);
    auto *newProject = new QAction("&New Project", this);

    file->addAction(load);
    file->addAction(save);
    file->addAction(newProject);

    BorderLayout *layout = new BorderLayout;
    centralWidget()->setLayout(layout);

    layout->addWidget(edit, BorderLayout::Center);
    layout->addWidget(frameViewer, BorderLayout::South);
    layout->addWidget(file, BorderLayout::North);
    layout->addWidget(tool, BorderLayout::West);

    setLayout(layout);

    connect(&model,
            &SpriteModel::updateFPSForView,
            this,
            &MainWindow::updateFPS);
    connect(timer,
            SIGNAL(timeout()),
            &model,
            SLOT(sendShowImageToPopUp()));
    connect(load,
            &QAction::triggered,
            this,
            &MainWindow::openLoadFileDialog);
    connect(this,
            &MainWindow::loadFile,
            &model,
            &SpriteModel::loadEditorFromJson);
    connect(save,
            &QAction::triggered,
            this,
            &MainWindow::openSaveFileDialog);
    connect(this,
            &MainWindow::saveFile,
            &model,
            &SpriteModel::saveFramesToFile);
    connect(&model,
            &SpriteModel::openAnimationPopup,
            this,
            &MainWindow::openAnimationPopup);
    connect(newProject,
            &QAction::triggered,
            this,
            &MainWindow::openNewFilePopup);

    setWindowTitle(tr("Sprite Editor"));
}

/**
 * @brief MainWindow::updateFPS
 * @param framePerSecond
 *
 * This method updates the frames per second to the given value
 * and restarts the timer
 */
void MainWindow::updateFPS(int framePerSecond)
{
    framesPerSecond = framePerSecond;
    timer->start(1000/framesPerSecond);
}

/**
 * @brief MainWindow::openAnimationPopup
 *
 * This method opens the animation popup and starts
 * the fps timer to play the animation
 */
void MainWindow::openAnimationPopup()
{
    animationPopUp->show();
    timer->start(1000/framesPerSecond);
}
/**
 * @brief MainWindow::openNewFilePopup
 *
 * This method opens the newProjectPopup
 */
void MainWindow::openNewFilePopup()
{
    newProjectPop->show();
}

/**
 * @brief MainWindow::~MainWindow
 *
 * The destructor for the MainWindow
 */
MainWindow::~MainWindow()
{
    delete ui;
}

/**
 * @brief MainWindow::openLoadFileDialog
 *
 * This method gets the name of the file to be loaded and sends it to the
 * appropriate slot in the model
 */
void MainWindow::openLoadFileDialog()
{
    QString fileName = QFileDialog::getOpenFileName(
                this,
                tr("Open Document"),
                QDir::currentPath(),
                ("Images (*.ssp)"));

    emit loadFile(fileName);
}

/**
 * @brief MainWindow::openSaveFileDialog
 *
 * This method gets the name of the file to be saved and sends it to the
 * appropriate slot in the model
 */
void MainWindow::openSaveFileDialog()
{
    QString fileName = QFileDialog::getSaveFileName(
                this,
                tr("Save File"),
                QDir::currentPath(),
                ("Images (*.ssp)"));

    emit saveFile(fileName);
}

