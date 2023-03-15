#include "teachingmenu.h"
#include "ui_teachingmenu.h"
#include <QPixmap>

TeachingMenu::TeachingMenu(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::TeachingMenu)
{
    ui->setupUi(this);
    //ui->textBubbleLabel->setPixmap(QPixmap(":/TeachingMenuImages/textBubble.png"));
    ui->textBubbleLabel->setScaledContents( true );
    ui->exitButton->setIcon(QIcon(":/TestPrefix/exitButton.png"));
    ui->exitButton->hide();

    // set up dictionary for tracking teaching images
    teachingDictionary[0] = teachGameplay;
    teachingDictionary[1] = teachRoundOne;
    teachingDictionary[2] = teachRoundTwo;
    teachingDictionary[3] = teachRoundThree;
    teachingDictionary[4] = teachEndGame;

    ui->nextButton->setStyleSheet("QPushButton{ background-color: grey }");

    einsteinMovie = new QMovie(":/TestPrefix/einstein.gif");
    if (einsteinMovie->isValid())
    {
        ui->einsteinLabel->setMovie(einsteinMovie);
        ui->einsteinLabel->setScaledContents(true);
    }


    einsteinMovie->setSpeed(25);
    einsteinMovie->start();

    currentTeachingPanel = 0;
    currentRound = 0;

    connect(ui->exitButton,
            &QPushButton::clicked,
            this,
            &TeachingMenu::exitButtonClicked);

    connect(ui->nextButton,
            &QPushButton::clicked,
            this,
            &TeachingMenu::nextSlide);
}

void TeachingMenu::prepareTeachingMenu(int roundNumber)
{
    currentRound = roundNumber;
    ui->nextButton->show();
    ui->exitButton->hide();

    nextSlide();

    emit showTeachingMenu();
}

void TeachingMenu::nextSlide()
{
    std::vector<QPixmap> currentTeachingPanels = teachingDictionary[currentRound];
    ui->textBubbleLabel->setPixmap(currentTeachingPanels[currentTeachingPanel]);
    currentTeachingPanel++;

    if (currentTeachingPanel == currentTeachingPanels.size())
    {
        ui->nextButton->hide();
        ui->exitButton->show();
    }
}

void TeachingMenu::exitButtonClicked()
{
    currentTeachingPanel = 0;
    if (currentRound != 4)
    {
        emit exitTeachingMenu();
    }
    else
    {
        emit showHomeButton();
    }
}

TeachingMenu::~TeachingMenu()
{
    delete ui;
}
