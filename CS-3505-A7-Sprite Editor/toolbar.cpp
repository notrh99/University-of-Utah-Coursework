/**
 * Assignment: A7: Sprite Editor
 * Authors: Shad Boswell, Zack Freeman, Jacob Hopkins, and Rayyan Hamid
 *
 * This code cannot be used without permission.
 *
 * Reviewed by: Rayyan Hamid
 */
#include "toolbar.h"
#include "ui_toolbar.h"

/**
 * @brief ToolBar::ToolBar
 * @param model
 * @param parent
 *
 * This is the constructor for the ToolBar class
 * This contains all the connections necessary for the tool bar to function
 * This also sets all the images of the buttons in the tool bar
 */
ToolBar::ToolBar(SpriteModel& model, QWidget *parent) :
    QWidget(parent),
    ui(new Ui::ToolBar)
{
    this->model = &model;
    ui->setupUi(this);
    colorSelector = new QColorDialog();

    connect(ui->colorSelection,
            &QPushButton::clicked,
            this,
            &ToolBar::colorSelectionButtonClicked);

    connect(this,
            &ToolBar::colorChanged,
            &model,
            &SpriteModel::changePenColor);

    connect(ui->eraserButton,
            &QPushButton::clicked,
            &model,
            &SpriteModel::changeToEraser);

    connect(ui->penButton,
            &QPushButton::clicked,
            &model,
            &SpriteModel::changeToPen);

    connect(ui->mirrorHorizontal,
            &QPushButton::toggled,
            &model,
            &SpriteModel::setMirrorHorizontal);

    connect(ui->mirrorVerticle,
            &QPushButton::toggled,
            &model,
            &SpriteModel::setMirrorVerticle);

    connect(ui->colorInvertButton,
            &QPushButton::pressed,
            &model,
            &SpriteModel::invertCurrentFrame);

    connect(ui->animationPlay,
            &QPushButton::clicked,
            &model,
            &SpriteModel::animationPopUp);

    connect(ui->penSizeSpinBox,
            &QSpinBox::valueChanged,
            &model,
            &SpriteModel::changePenSize);
    connect(ui->eraserSizeSpinBox,
            &QSpinBox::valueChanged,
            &model,
            &SpriteModel::changeEraserSize);

    connect(&model,
            &SpriteModel::disableEraserButtonHighlight,
            this,
            &ToolBar::disableEraserHighlight);

    connect(&model,
            &SpriteModel::disablePenButtonHighlight,
            this,
            &ToolBar::disablePenHighlight);

    colorSelector = new QColorDialog();

    QPalette pal = ui->colorSelection->palette();
    uint initialColor = qRgb(0,0,0);

    pal.setColor(QPalette::Button, initialColor);
    ui->colorSelection->setAutoFillBackground(true);
    ui->colorSelection->setFlat(true);
    ui->colorSelection->setPalette(pal);
    ui->colorSelection->update();


    ui->colorSelection->setToolTip("Color Selection");
    ui->animationPlay->setToolTip("Play Animation");
    ui->eraserButton->setToolTip("Eraser Tool");
    ui->penButton->setToolTip("Brush Tool");
    ui->colorInvertButton->setToolTip("Invert Color");
    ui->mirrorHorizontal->setToolTip("Horizontal Mirror");
    ui->mirrorVerticle->setToolTip("Vertical Mirror");

    ui->penButton->setIcon(QIcon(":/IconImages/icons8-paint-96.png"));
    ui->eraserButton->setIcon(QIcon(":/IconImages/icons8-eraser-100.png"));
    ui->mirrorHorizontal->setIcon(QIcon(":/IconImages/mirror-vertical.png"));
    ui->mirrorVerticle->setIcon(QIcon(":/IconImages/mirror-horizontally.png"));
    ui->animationPlay->setIcon(QIcon(":/IconImages/play-button.png"));
    ui->colorInvertButton->setIcon(QIcon(":/IconImages/InvertColor.png"));
    ui->colorSelection->setIcon(QIcon(":/IconImages/RGB.png"));
}

/**
 * @brief ToolBar::colorSelectionButtonClicked
 *
 * This method gets the rgb values of the selected color
 * and send the selected color to the model.
 * This also updates the background of the color selector button
 * to match the selected color.
 */
void ToolBar::colorSelectionButtonClicked()
{
    QColor color = colorSelector->getColor(Qt::yellow, this);
    int red = color.red();
    int green = color.green();
    int blue = color.blue();
    int alpha = color.alpha();
    uint newColor = qRgba(red, green, blue, alpha);

    emit colorChanged(newColor);

    QPalette pal = ui->colorSelection->palette();
    pal.setColor(QPalette::Button, color);
    ui->colorSelection->setAutoFillBackground(true);
    ui->colorSelection->setFlat(true);
    ui->colorSelection->setPalette(pal);
    ui->colorSelection->update();
}

/**
 * @brief ToolBar::disableEraserHighlight
 *
 * This method visually deselects the eraser tool
 */
void ToolBar::disableEraserHighlight()
{
    ui->eraserButton->setChecked(false);
}

/**
 * @brief ToolBar::disablePenHighlight
 *
 * This method visually deselects the pen tool
 */
void ToolBar::disablePenHighlight()
{
    ui->penButton->setChecked(false);
}

/**
 * @brief ToolBar::~ToolBar
 *
 * The destructor for the ToolBar
 */
ToolBar::~ToolBar()
{
    delete ui;
}


