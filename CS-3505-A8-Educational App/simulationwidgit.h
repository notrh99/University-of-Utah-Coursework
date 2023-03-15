#ifndef SIMULATIONWIDGIT_H
#define SIMULATIONWIDGIT_H

#include "Box2D/Box2D.h"
#include "gamemodel.h"
#include <QWidget>
#include <QTimer>
#include <QImage>
#include "contactlistener.h"
#include <QRandomGenerator>
#include <QMediaPlayer>


namespace Ui {
class SimulationWidgit;
}

class SimulationWidgit : public QWidget
{
    Q_OBJECT

public:
    explicit SimulationWidgit(QImage bulletImage, QImage duckImage, QImage resultantImage, GameModel& model, QWidget *parent = nullptr);
    ~SimulationWidgit();
    void paintEvent(QPaintEvent *);
    void mousePressEvent(QMouseEvent *ev);
    void deleteBodiesFromListener(b2Body* bodyA, b2Body* bodyB);
    void makeDuck();
    void makeBullet();
    void makeResultants(b2Vec2 position);
    void makeSingleResultant(b2Vec2 position, int resultType);
    bool Positrons;
    bool Neutrinos;
    bool gammaRays;
    bool Protons;
    float calculatePlayerAngle(float, float);
    void drawPlayer();
    void setPositrons(bool);
    void setNeutrinos(bool);
    void setgammaRays(bool);
    void setProtons(bool);
    void setDuckSprite(QString);
    void setBulletSprite(QString);
    void setResultantSprite(QString);
    void setDuckVelocity(int);
    //void BeginContact(b2Contact* contact);

signals:
    void showTeachingScreen();
    void endRound(int);
    void endGame(int);
    void collisionOccured();

public slots:
    void updateWorld();
    void throwProjectile();
    void decideEndGameScreen();
    void resetSimulation();
    void resetGame();
    void setSpriteImages(int);
    void setScoreText(int);
    void setCurrentRoundAmmo(int);

private:
    Ui::SimulationWidgit *ui;
//    b2World world;
//    b2Body body;
    QRandomGenerator gen1; // Default, seeded with value of 1.
    b2World world;
    b2Body* body;
    QTimer timer;
    QTimer timer2;
    QImage bulletSprite;
    QImage duckSprite;
    QImage playerSprite;
    QImage resultantSprite;
    QImage PositronsSprite;
    QImage NeutrinosSprite;
    QImage gammaRaysSprite;
    QImage ProtonsSprite;
    int duckSize;
    int bulletSize;
    int resultantSize;
    void setDuckSize(int);
    void setBulletSize(int);
    void setResultantSize(int);
    int duckVelocity;
    int currentRoundAmmo;
    int playerXSize;
    int playerYSize;
    int currentRound;
    int roundCollisions;
    ContactListener* cl;
    QVector<b2Body*> bulletBodies;
    QVector<b2Body*> duckBodies;
    QVector<b2Body*> deadBodies;
    QMap<b2Body*, int> resultantBodies;
    int ducksToCreate;
    int bulletsToCreate;
    int resultantsToCreate;
    b2Vec2 lastCollisionPosition;
    b2Vec2 mouseVector;
    QMediaPlayer* bulletSound;
    QMediaPlayer* backGroundSound;

};

#endif // SIMULATIONWIDGIT_H
