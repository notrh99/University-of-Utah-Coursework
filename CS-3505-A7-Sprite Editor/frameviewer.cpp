/**
 * Assignment: A7: Sprite Editor
 * Authors: Shad Boswell, Zack Freeman, Jacob Hopkins, and Rayyan Hamid
 *
 * This code cannot be used without permission.
 *
 * Reviewed by: Jacob Hopkins
 */
#include "frameviewer.h"
#include "ui_frameviewer.h"

/**
 * @brief FrameViewer::FrameViewer
 * @param model
 * @param parent
 *
 * This is the constructor for the frameViewer class.
 * This contains the connections necessary for the frame viewer to function.
 */
FrameViewer::FrameViewer(SpriteModel& model, QWidget *parent) :
    QWidget(parent),
    ui(new Ui::FrameViewer)
{
    this->model = &model;

    ui->setupUi(this);

    connect(ui->addFrame,
            &QPushButton::clicked,
            &model,
            &SpriteModel::addFrameClicked);
    connect(ui->removeFrame,
            &QPushButton::clicked,
            &model,
            &SpriteModel::removeFrame);
    connect(ui->arrowRight,
            &QPushButton::clicked,
            &model,
            &SpriteModel::rightFrameClicked);
    connect(ui->arrowLeft,
            &QPushButton::clicked,
            &model,
            &SpriteModel::leftFrameClicked);
    connect(&model,
            &SpriteModel::setFrameViewText,
            this,
            &FrameViewer::setText);
}

/**
 * @brief FrameViewer::setText
 * @param s
 *
 * This method sets the label in the frame viewer to the given string
 */
void FrameViewer::setText(QString s)
{
    ui->label->setText(s);
}

/**
 * @brief FrameViewer::~
 *
 * The destructor for the frame viewer
 */
FrameViewer::~FrameViewer()
{
    delete ui;
}
