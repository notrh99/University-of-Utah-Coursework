/**
 * Assignment: A7: Sprite Editor
 * Authors: Shad Boswell, Zack Freeman, Jacob Hopkins, and Rayyan Hamid
 *
 * This code cannot be used without permission.
 *
 * Reviewed by: Rayyan Hamid
 */
#include "editingwindow.h"
#include "ui_editingwindow.h"

/**
 * @brief EditingWindow::EditingWindow
 * @param model
 * @param parent
 *
 * This is the constructor for the EditingWindow class.
 * This contains all of the connetions necessary for the EditingWindow to function.
 */
EditingWindow::EditingWindow(SpriteModel& model, QWidget *parent) :
    QWidget(parent),
    ui(new Ui::EditingWindow)
{
    this->model = &model;
    ui->setupUi(this);

    ui->backGroundLabel->setText("");
    ui->backGroundLabel->setGeometry(0,0,400,400);
    ui->mainLabel->setText("");
    ui->mainLabel->setGeometry(0,0,400,400);
    windowHeight = 400;
    windowWidth = 400;

    connect(this,
            &EditingWindow::mousePressed,
            &model,
            &SpriteModel::changePixelOnMouseClick);
    connect(&model,
            &SpriteModel::showImage,
            this,
            &EditingWindow::loadImageToView);
    connect(&model,
            &SpriteModel::sendPictureDimensions,
            this,
            &EditingWindow::setHeightWidth);
    connect(this,
            &EditingWindow::sendWindowDimensions,
            &model,
            &SpriteModel::setEditingWindowDimensions);
}

/**
 * @brief EditingWindow::setHeightWidth
 * @param picWidth the width of an image in pixels
 * @param picHeight the height of an image in pixels
 *
 * Slot for model, sets main label and background label with given size of frames
 */
void EditingWindow::setHeightWidth(int picWidth, int picHeight)
{
    double x;
    double y;

    double picH = picHeight;
    double picW = picWidth;
    if(picH > picW)
    {
        y = 450;
        x = 450*(picW / picH);
    }
    else
    {
        x = 450;
        y = 450*(picH / picW);
    }
    windowWidth = x;
    windowHeight = y;
    emit sendWindowDimensions(windowHeight, windowWidth);
    ui->backGroundLabel->setGeometry(0,0,x,y);
    ui->mainLabel->setGeometry(0,0,x,y);
    placeBackground(picWidth, picHeight);

}

/**
 * @brief EditingWindow::placeBackground
 * @param width
 * @param height
 *
 * This method places the grid background on the editing window
 */
void EditingWindow::placeBackground(int width, int height)
{
    background = createBackgroundImage(width,height);
    QPixmap back = QPixmap::fromImage(background);
    ui->backGroundLabel->setPixmap(back.scaled(QSize( windowWidth,windowHeight),Qt::KeepAspectRatio));//set to back.scaled to get background working
}

/**
 * @brief EditingWindow::mousePressEvent
 * @param ev
 *
 * This method handles the case when any button on the mouse is clicked.
 */
void EditingWindow::mousePressEvent(QMouseEvent *ev)
{
    if(ev->button() == Qt::LeftButton)
    {
        pos = ev->pos(); //save mouse click position
        emit mousePressed(pos);
    }
}

/**
 * @brief EditingWindow::mouseMoveEvent
 * @param ev
 *
 * This method tracks any button while it is pressed down.
 * Specifically, this method has been set up to track the left button.
 * The left button can be used to continuously draw/erase.
 */
void EditingWindow::mouseMoveEvent(QMouseEvent *ev)
{
    if(ev->buttons() & Qt::LeftButton)
    {
        pos = ev->pos();
        emit mousePressed(pos);
    }
}

/**
 * @brief EditingWindow::createBackgroundImage
 * @param x width in pixels
 * @param y height in pixels
 * @return QImage
 *
 * Creates a QImage of given size with a grid pattern
 */
QImage EditingWindow::createBackgroundImage(int x, int y)
{
    QImage image = QPixmap(x, y).toImage(); //creates blank image 16x16
    for (int i = 0; i < x; i++) {
        for (int j = 0; j < y; j++)
        {
            if(((j % 2) == 0 && (i % 2) == 0) || ((j%2) == 1 && (i%2) == 1))
            {
                image.setPixelColor(i, j, QColor(211,211,211));
            }
            else
            {
                image.setPixelColor(i, j, QColor(250,250,250));
            }
        }
    }
    return image;
}

/**
 * @brief EditingWindow::loadImageToView
 * @param image image to be displayed in main window
 *
 * Slot for model that displays given image
 */
void EditingWindow::loadImageToView(QImage image)
{
    QPixmap pix = QPixmap::fromImage(image);
    ui->mainLabel->raise();
    ui->mainLabel->show();
    ui->mainLabel->setPixmap(pix.scaled(QSize(windowWidth,windowHeight),Qt::KeepAspectRatio));//set to back.scaled to get background working
    ui->mainLabel->setScaledContents( true );
    ui->mainLabel->setSizePolicy(QSizePolicy::Ignored, QSizePolicy::Ignored);
}

/**
 * @brief EditingWindow::~EditingWindow
 *
 * The destructor for the EditingWindow
 */
EditingWindow::~EditingWindow()
{
    delete ui;
}
