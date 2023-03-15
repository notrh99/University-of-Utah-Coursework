#include "startmenu.h"
#include "ui_startmenu.h"

StartMenu::StartMenu(GameModel& model, QWidget *parent) :
    QWidget(parent),
    ui(new Ui::StartMenu)
{
    ui->setupUi(this);
    ui->titleLabel->setPixmap(QPixmap(":/TestPrefix/theProtonProtonChain.png").scaled(700,500));
    ui->titleLabel->setScaledContents( true );

    ui->titleLabel->setSizePolicy( QSizePolicy::Ignored, QSizePolicy::Ignored );

    ui->startButton->setStyleSheet("QPushButton{ background-color: grey }");
    ui->helpButton->setStyleSheet("QPushButton{ background-color: grey }");

    ui->helpMenuLabel->setPixmap(QPixmap(":/TestPrefix/helpMenu.png"));
    ui->helpMenuLabel->setScaledContents( true );
    ui->helpMenuLabel->hide();
    ui->exitButton->hide();

    ui->exitButton->setIcon(QIcon(":/TestPrefix/exitButton.png"));

    connect(ui->startButton,
            &QPushButton::clicked,
            &model,
            &GameModel::startGame);
    ui->helpButton->hide();
}

StartMenu::~StartMenu()
{
    delete ui;
}

void StartMenu::on_helpButton_clicked()
{
    ui->helpMenuLabel->show();
    ui->exitButton->show();
    ui->startButton->hide();
    ui->helpButton->hide();
    ui->splitter->hide();
}

void StartMenu::on_exitButton_clicked()
{
    ui->helpMenuLabel->hide();
    ui->exitButton->hide();
    ui->splitter->show();
    ui->startButton->show();
    ui->helpButton->show();
}
