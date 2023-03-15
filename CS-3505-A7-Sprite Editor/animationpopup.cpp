/**
 * Assignment: A7: Sprite Editor
 * Authors: Shad Boswell, Zack Freeman, Jacob Hopkins, and Rayyan Hamid
 *
 * This code cannot be used without permission.
 *
 * Reviewed By: Rayyan Hamid
 */
#include "animationpopup.h"
#include "ui_animationpopup.h"

/**
 * @brief AnimationPopup::AnimationPopup
 * @param model
 * @param parent
 *
 * This is the constructor for the AnimationPopup
 * This contains all the connections to make the popup function
 */
AnimationPopup::AnimationPopup(SpriteModel& model, QWidget *parent) :
    QDialog(parent),
    ui(new Ui::AnimationPopup)
{
    this->model = &model;
    ui->setupUi(this);
    currentFPS = 1;
    actualSize = false;

    connect(ui->fpsSlider,
            &QSlider::valueChanged,
            &model,
            &SpriteModel::updateFPS);
    connect(ui->fpsSlider,
            &QSlider::valueChanged,
            this,
            &AnimationPopup::setDisplayFPS);
    connect(this,
            &AnimationPopup::fpsWindowOpen,
            &model,
            &SpriteModel::animationPopUpOpen);
    connect(&model,
            &SpriteModel::showImageToPopUp,
            this,
            &AnimationPopup::loadImageToView);
    connect(ui->actualSizeSelector,
            &QRadioButton::clicked,
            this,
            &AnimationPopup::actualSizeBool);

}

/**
 * @brief AnimationPopup::closeEvent
 * @param bar
 *
 * This method lets the model now that the animation window is closed
 */
void AnimationPopup::closeEvent(QCloseEvent *bar)
{
    emit fpsWindowOpen(false);
}

/**
 * @brief AnimationPopup::setDisplayFPS
 * @param input
 *
 * This method sets the fps of the animation window
 */
void AnimationPopup::setDisplayFPS(int input)
{
    currentFPS = input;
    QString s = QString::number(currentFPS);
    ui->currentFPSLabel->setText(s);
}

/**
 * @brief AnimationPopup::actualSizeBool
 * @param setActualSize
 *
 * This method selects or deselects the actualSize bool according to the
 * given value
 */
void AnimationPopup::actualSizeBool(bool setActualSize)
{
    actualSize = setActualSize;
}

/**
 * @brief AnimationPopup::loadImageToView
 * @param image
 *
 * This method loads given image to popup
 */
void AnimationPopup::loadImageToView(QImage image)
{
    QPixmap pix = QPixmap::fromImage(image);
    if(actualSize)
    {
        ui->ALabel->setPixmap(pix.scaled(QSize(image.width(),image.height()),Qt::KeepAspectRatio));
        ui->ALabel->setScaledContents( false );
        ui->ALabel->setSizePolicy( QSizePolicy::Ignored, QSizePolicy::Ignored );

    }
    else
    {
        double x;
        double y;

        double pixHeight = pix.height();
        double pixWidth = pix.width();

        if(pixHeight > pixWidth)
        {
            y = 200;
            x = 200*(pixWidth / pixHeight);
        }
        else
        {
            x = 200;
            y = 200*(pixHeight / pixWidth);
        }

        ui->ALabel->setGeometry(10,10, x, y);
        ui->ALabel->setPixmap(pix.scaled(QSize(400, 400),Qt::KeepAspectRatio));
        ui->ALabel->setScaledContents( true );
        ui->ALabel->setSizePolicy(QSizePolicy::Ignored, QSizePolicy::Ignored);
    }
}

/**
 * @brief AnimationPopup::~AnimationPopup
 *
 * The destructor for the AnimationPopUp
 */
AnimationPopup::~AnimationPopup()
{
    delete ui;
}


