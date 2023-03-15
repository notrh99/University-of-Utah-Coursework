#include "gamescreen.h"
#include "ui_gamescreen.h"

GameScreen::GameScreen(GameModel& model, QWidget *parent) :
    QWidget(parent),
    ui(new Ui::GameScreen)
{
    ui->setupUi(this);

    ui->backgroundLabel->setPixmap(QPixmap(":/TestPrefix/starBackground.png"));
    ui->backgroundLabel->setScaledContents( true );

    ui->consoleLabel->setPixmap((QPixmap(":/TestPrefix/gameboyScreen.png")));
    ui->consoleLabel->setScaledContents(true);
    //connect(&model, &GameModel::sendScoreText, this, &GameScreen::updateScoreText);

}

void GameScreen::updateScoreText(int newScore){
    QString s = QString::number(newScore);
    //ui->ScoreTotalLabel->setText(s);
}


GameScreen::~GameScreen()
{
    delete ui;
}
