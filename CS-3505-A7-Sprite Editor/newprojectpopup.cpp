/**
 * Assignment: A7: Sprite Editor
 * Authors: Shad Boswell, Zack Freeman, Jacob Hopkins, and Rayyan Hamid
 *
 * This code cannot be used without permission.
 *
 * Reviewed By: Jacob Hopkins
 */
#include "newprojectpopup.h"
#include "ui_newprojectpopup.h"

/**
 * @brief newProjectPopup::newProjectPopup
 * @param model
 * @param parent
 *
 * This is the constructor for the popup that appears when you creat a new project.
 * Contains the connections for each element in the window
 */
newProjectPopup::newProjectPopup(SpriteModel& model, QWidget *parent) :
    QDialog(parent),
    ui(new Ui::newProjectPopup)
{
    ui->setupUi(this);
    this->model = &model;

    connect(ui->horizontalSlider,
            &QSlider::valueChanged,
            &model,
            &SpriteModel::setWidth);
    connect(ui->horizontalSlider,
            &QSlider::valueChanged,
            this,
            &newProjectPopup::setWidthValueLabel);
    connect(ui->verticleSlider,
            &QSlider::valueChanged,
            &model,
            &SpriteModel::setHeight);
    connect(ui->verticleSlider,
            &QSlider::valueChanged,
            this,
            &newProjectPopup::setHeightValueLabel);
    connect(ui->createButton,
            &QPushButton::clicked,
            this,
            &newProjectPopup::createClicked);
    connect(ui->createButton,
            &QPushButton::clicked,
            &model,
            &SpriteModel::createProject);
}

/**
 * @brief newProjectPopup::setHeightValueLabel
 * @param value
 *
 * This method sets the height label to the given value
 */
void newProjectPopup::setHeightValueLabel(int height)
{
    QString s = QString::number(height);
    ui->currentHeight->setText(s);
}

/**
 * @brief newProjectPopup::setWidthValueLabel
 * @param value
 *
 * This method sets the width lable to the given value
 */
void newProjectPopup::setWidthValueLabel(int height)
{
    QString s = QString::number(height);
    ui->currentWidth->setText(s);
}

/**
 * @brief newProjectPopup::createClicked
 *
 * This method closes the popup when create is clicked
 */
void newProjectPopup::createClicked()
{
    this->close();
}

/**
 * @brief newProjectPopup::~newProjectPopup
 *
 * The destructor for the popup window
 */
newProjectPopup::~newProjectPopup()
{
    delete ui;
}
