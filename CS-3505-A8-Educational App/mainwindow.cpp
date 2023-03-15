#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QVBoxLayout>
#include <QMediaPlayer>
#include <QAudioOutput>


MainWindow::MainWindow(GameModel& model, QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    gameScreen = new GameScreen(model,this);
    endGame = new EndGame(this);
    teachingMenu = new TeachingMenu(this);
    startMenu = new StartMenu(model, this);
    startMenu->hide();
    intermission = new Intermission(this);
    gameScreen->show();

    endGame->hide();
    teachingMenu->hide();

    startMenu->setGeometry(100,85,600,450);
    startMenu->show();
    QImage bulletImage = QImage(":/TestPrefix/H1");
    QImage duckImage = QImage(":/TestPrefix/H1");
    QImage resultImage = QImage(":/TestPrefix/H2");
    sim = new SimulationWidgit(bulletImage, duckImage, resultImage, model, this);
    teachingMenu->setGeometry(100,85,600,450);
    intermission->hide();
    sim->hide();


    connect(endGame,
            &EndGame::goHome,
            this,
            &MainWindow::showHomeScreen);

    connect(endGame,
            &EndGame::goHome,
            sim,
            &SimulationWidgit::resetGame);

    connect(endGame,
            &EndGame::goHome,
            &model,
            &GameModel::resetGame);

    connect(&model,
            &GameModel::goToIntermission,
            intermission,
            &Intermission::setUpIntermission);

    connect(intermission,
            &Intermission::goToSim,
            this,
            &MainWindow::showGameScreen);

    connect(intermission,
            &Intermission::endGame,
            sim,
            &SimulationWidgit::decideEndGameScreen);

    connect(sim,
            &SimulationWidgit::endGame,
            endGame,
            &EndGame::setUpEndGameScreen);

    connect(teachingMenu,
            &TeachingMenu::exitTeachingMenu,
            &model,
            &GameModel::exitTeachingMenu);

    connect(&model,
            &GameModel::goToTeachingScreen,
            teachingMenu,
            &TeachingMenu::prepareTeachingMenu);


    connect(endGame,
            &EndGame::setUpFinalTeachingRound,
            teachingMenu,
            &TeachingMenu::prepareTeachingMenu);

    connect(endGame,
            &EndGame::setUpFinalTeachingRound,
            this,
            &MainWindow::hideEndGame);

    connect(teachingMenu,
            &TeachingMenu::showHomeButton,
            endGame,
            &EndGame::showHomeButton);

    connect(teachingMenu,
            &TeachingMenu::showHomeButton,
            [this](){endGame->show(); teachingMenu->hide();});

    connect(&model,
            &GameModel::endRound,
            intermission,
            &Intermission::endRound);

    connect(intermission,
            &Intermission::goToNextRound,
            &model,
            &GameModel::goToNextRound);

    connect(intermission,
            &Intermission::goToNextRound,
            sim,
            &SimulationWidgit::resetSimulation);

    connect(teachingMenu,
            &TeachingMenu::showTeachingMenu,
            [this](){teachingMenu->show(); startMenu->hide(); intermission->hide();});

    connect(endGame,
            &EndGame::showEndGameScreen,
            [this](){intermission->hide(); sim->hide();
            endGame->setGeometry(100,85,600,450); endGame->show();});

    connect(intermission,
            &Intermission::showIntermission,
            this,
            &MainWindow::showIntermission);

    connect(&model,
            &GameModel::updateCollisions,
            intermission,
            &Intermission::setCollisions);

    //    connect(&model,
    //               &GameModel::hideStartScreen,
    //               this,
    //               &MainWindow::hideStartWidget);

    //     connect(ui->centralwidget->s,
    //              &QPushButton::clicked,
    //              &model,
    //              &GameModel::startGame);

    player = new QMediaPlayer();
    QAudioOutput* audioOutput = new QAudioOutput();
    player->setAudioOutput(audioOutput);
    audioOutput->setVolume(25);
    player->setSource(QUrl("qrc:/Music/gameMusic.mp3"));
    player->play();
}

void MainWindow::showIntermission()
{
    teachingMenu->hide();
    startMenu->hide();
    sim->hide();
    intermission->setGeometry(100,85,600,450);
    intermission->show();
}

void MainWindow::hideEndGame()
{
    endGame->hide();
}

void MainWindow::transitionToGameScreen()
{
    startMenu->hide();
    endGame->show();
}

void MainWindow::showGameScreen()
{
    intermission->hide();
    teachingMenu->hide();
    sim->setGeometry(100,85,600,450);
    sim->show();
    gameScreen->show();
}

void MainWindow::showTeachingMenu()
{
    sim->hide();
    gameScreen->hide();
    teachingMenu->show();
}

void MainWindow::showHomeScreen()
{
    endGame->hide();
    startMenu->show();
}

MainWindow::~MainWindow()
{
    delete ui;
}

