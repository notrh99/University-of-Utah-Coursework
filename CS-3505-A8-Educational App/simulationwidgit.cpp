#include "simulationwidgit.h"
#include "ui_simulationwidgit.h"
#include <QPainter>
#include <iostream>
#include <QVector>
#include "contactlistener.h"
#include <cmath>
#include <QDebug>
#include <QRandomGenerator>
#include <QAudioOutput>


SimulationWidgit::SimulationWidgit(QImage bulletImage, QImage duckImage, QImage resultantImage, GameModel& model, QWidget *parent) :
    QWidget(parent),
    ui(new Ui::SimulationWidgit),
    world(b2Vec2(0.0f, 10.0f)),
    timer(this),
    timer2(this)
{
    ui->setupUi(this);
    duckSize = 70;
    bulletSize = 20;
    resultantSize = 38;
    duckVelocity = 25;
    currentRoundAmmo = 10;
    roundCollisions = 0;
    bulletSprite = bulletImage.scaled( bulletSize, bulletSize, Qt::KeepAspectRatio);
    duckSprite = duckImage.scaled( duckSize, duckSize, Qt::KeepAspectRatio);
    resultantSprite = resultantImage.scaled( resultantSize, resultantSize, Qt::KeepAspectRatio);
    QPixmap ammoImage = QPixmap::fromImage(bulletSprite);
    ui->AmmoPictureLabel->setPixmap(ammoImage);
    QString s = QString::number(currentRoundAmmo);
    ui->AmmoAmmountLabel->setText(s);
    ui->ScoreAmountLabel->setText("0");

    QImage gamma = QImage(":/TestPrefix/Gamma");
    gammaRaysSprite = gamma.scaled( 45, 45, Qt::KeepAspectRatio);
    QImage proton = QImage(":/TestPrefix/H1");
    ProtonsSprite = proton.scaled( resultantSize, resultantSize, Qt::KeepAspectRatio);
    QImage Neutrino = QImage(":/TestPrefix/Neutrino");
    NeutrinosSprite = Neutrino.scaled( resultantSize, resultantSize, Qt::KeepAspectRatio);
    QImage positron = QImage(":/TestPrefix/Positron");
    PositronsSprite = positron.scaled( resultantSize * 2.5, resultantSize * 2.5, Qt::KeepAspectRatio);

    Positrons = true;
    Neutrinos = true;
    gammaRays = false;
    Protons = false;

    playerXSize = 50;
    playerYSize = 50;
    playerSprite = QImage(":/PlayerSprite/gun_character_two.png").scaled(playerXSize, playerYSize, Qt::KeepAspectRatio);

    setMouseTracking(true);

    cl = new ContactListener();
    world.SetContactListener(cl);

    connect(&model, &GameModel::dropGammaRays, this, &SimulationWidgit::setgammaRays);
    connect(&model, &GameModel::dropPositrons, this, &SimulationWidgit::setPositrons);
    connect(&model, &GameModel::dropProtons, this, &SimulationWidgit::setProtons);
    connect(&model, &GameModel::dropNeutrinos, this, &SimulationWidgit::setNeutrinos);
    connect(&model, &GameModel::changeBulletSize, this, &SimulationWidgit::setBulletSize);
    connect(&model, &GameModel::changeDuckSize, this, &SimulationWidgit::setDuckSize);
    connect(&model, &GameModel::changeResultantSize, this, &SimulationWidgit::setResultantSize);
    connect(&model, &GameModel::setDuckSprite, this, &SimulationWidgit::setDuckSprite);
    connect(&model, &GameModel::setBulletSprite, this, &SimulationWidgit::setBulletSprite);
    connect(&model, &GameModel::setResultantSprite, this, &SimulationWidgit::setResultantSprite);
    connect(&model, &GameModel::setDuckVelocity, this, &SimulationWidgit::setDuckVelocity);
    connect(&model, &GameModel::sendScoreText, this, &SimulationWidgit::setScoreText);
    connect(&model, &GameModel::sendGameAmmoAmount, this, &SimulationWidgit::setCurrentRoundAmmo);

    connect(this, &SimulationWidgit::collisionOccured, &model, &GameModel::updateScoreOnCollision);

    connect(this,
            &SimulationWidgit::endRound,
            &model,
            &GameModel::roundOver);

    connect(&model,
            &GameModel::updateSprites,
            this,
            &SimulationWidgit::setSpriteImages);

    connect(cl, &ContactListener::deleteBodies, this, &SimulationWidgit::deleteBodiesFromListener);

    connect(&timer, &QTimer::timeout, this, &SimulationWidgit::updateWorld);
    timer.start(15);

    connect(&timer2, &QTimer::timeout, this, &SimulationWidgit::throwProjectile);

    timer2.start(2000);

    bulletSound = new QMediaPlayer();
    bulletSound->setSource(QUrl("qrc:/Music/blaster-2-81267.mp3"));
    QAudioOutput* audioOutput = new QAudioOutput();
    bulletSound->setAudioOutput(audioOutput);
    audioOutput->setVolume(25);
}

void SimulationWidgit::deleteBodiesFromListener(b2Body* bodyA, b2Body* bodyB)
{
    if(bodyA->IsBullet() && bodyB->IsBullet()){

    } else if(bodyA->IsFixedRotation() || bodyB->IsFixedRotation()){

    }else {
        if(!deadBodies.contains(bodyA)){
            deadBodies.append(bodyA);
        }
        if(!deadBodies.contains(bodyB)){
            deadBodies.append(bodyB);
        }
          emit collisionOccured();
        lastCollisionPosition = bodyA->GetPosition();
        makeResultants(lastCollisionPosition);
    }

}

void SimulationWidgit::paintEvent(QPaintEvent *)
{
    QVectorIterator<b2Body*> duckList(duckBodies);
    b2Body* temp;
    while (duckList.hasNext()) {
        temp = duckList.next();
        // Create a painter
        QPainter painter(this);
        b2Vec2 position = temp->GetPosition();
        if(position.x > 40 || position.y > 40 || position.x < -1 || position.y < -1){
            duckBodies.removeOne(temp);
            world.DestroyBody(temp);
        } else {
            painter.drawImage((int)(position.x*20), (int)(position.y*20), duckSprite);
            painter.end();
        }
    }

    QVectorIterator<b2Body*> bulletList(bulletBodies);
    b2Body* temp2;
    while (bulletList.hasNext()) {
        temp2 = bulletList.next();
        // Create a painter
        QPainter painter(this);
        b2Vec2 position = temp2->GetPosition();
        if(position.x > 40 || position.y > 40 || position.x < 0 || position.y < 0){
            bulletBodies.removeOne(temp2);
            world.DestroyBody(temp2);
        } else {
            painter.drawImage((int)(position.x*20), (int)(position.y*20), bulletSprite);
            painter.end();
        }
    }


    QMap<b2Body*, int>::iterator i;
    for (i = resultantBodies.begin(); i != resultantBodies.end(); ++i){
        QPainter painter(this);
        b2Vec2 position = i.key()->GetPosition();
        if(position.x < 0 || position.y < 0 || position.x > 30 || position.y > 30){
            resultantBodies.remove(i.key());
            world.DestroyBody(i.key());
        } else {
            if(i.value() == 0){
                painter.drawImage((int)(position.x*20), (int)(position.y*20), resultantSprite);
            } else if (i.value() == 1){
                painter.drawImage((int)(position.x*20), (int)(position.y*20), ProtonsSprite);
            } else if (i.value() == 2){
                painter.drawImage((int)(position.x*20), (int)(position.y*20), PositronsSprite);
            } else if (i.value() == 3){
                painter.drawImage((int)(position.x*20), (int)(position.y*20), gammaRaysSprite);
            } else {
                painter.drawImage((int)(position.x*20), (int)(position.y*20), NeutrinosSprite);
            }
        }
        painter.end();
    }
    drawPlayer();
}

float SimulationWidgit::calculatePlayerAngle(float deltaX, float deltaY)
{
    float thetaRadians = std::atan(deltaY/deltaX);
    float thetaDegrees = thetaRadians * (180/M_PI);
    float rotateAngle;
    if(deltaX < 0)
    {
        rotateAngle = -90 - thetaDegrees;
    }
    else
    {
        rotateAngle = 90 - thetaDegrees;
    }
    return rotateAngle;
}

void SimulationWidgit::drawPlayer()
{
    // create painter
    QPainter playerPainter(this);

    // define the translation coordinates
    int translateX = (this->size().width())/2;
    int translateY = this->size().height();

    // get mouse position
    QPoint mousePos = mapFromGlobal(QCursor::pos());
    float mouseX = mousePos.x();
    float mouseY = mousePos.y();

    // get differences between mouse position and the translation position
    float deltaX = mouseX - translateX;
    float deltaY = translateY - mouseY;

    // get corresponding angle
    float rotationAngle = calculatePlayerAngle(deltaX, deltaY);

    // update mouse vector (used for shooting particles)
    mouseVector.Set(deltaX, -deltaY);
    mouseVector.Normalize();
    float bulletVelocity = 30;
    mouseVector *= bulletVelocity;

    // find position at which player is drawn
    int drawX = -1 * playerXSize/4;
    int drawY = -1 * playerYSize;

    // draw player (gun)
    playerPainter.translate(translateX, translateY);
    playerPainter.rotate(rotationAngle);
    playerPainter.drawImage(drawX, drawY, playerSprite);
}

void SimulationWidgit::decideEndGameScreen()
{
    if(roundCollisions == 0)
        emit endGame(0);
    else
        emit endGame(1);
}

void SimulationWidgit::throwProjectile()
{
    ducksToCreate++;
}

void SimulationWidgit::updateWorld()
{
    for(int i = 0; i < ducksToCreate; i++)
    {
        makeDuck();
    }
    for(int i = 0; i<bulletsToCreate; i++)
    {
        makeBullet();
    }
    if(resultantsToCreate > 0){
        makeSingleResultant(lastCollisionPosition, 0);
        if(Protons){
            makeSingleResultant(lastCollisionPosition, 1);
        }
        if(Positrons){
            makeSingleResultant(lastCollisionPosition, 2);
            makeSingleResultant(lastCollisionPosition, 2);
        }
        if(gammaRays){
            makeSingleResultant(lastCollisionPosition, 3);
        }
        if(Neutrinos){
            makeSingleResultant(lastCollisionPosition, 4);
        }
    }
    QVectorIterator<b2Body*> bodiesToRemove(deadBodies);
    while (bodiesToRemove.hasNext())
    {

        b2Body* temp = bodiesToRemove.next();
        if(temp->IsBullet())
        {
            bulletBodies.removeOne(temp);
        }
        else if(temp->IsFixedRotation())
        {
            resultantBodies.remove(temp);
        }
        else
        {
            duckBodies.removeOne(temp);
            roundCollisions++;
        }
        deadBodies.removeOne(temp);
        world.DestroyBody(temp);
    }

    if(currentRoundAmmo == 0 && bulletBodies.length() == 0)
    {
        timer.stop();
        timer2.stop();

        if(roundCollisions == 0)
        {
            emit endGame(0);
            return;
        }

        emit endRound(roundCollisions);
    }

    ducksToCreate = 0;
    bulletsToCreate = 0;
    resultantsToCreate = 0;
    world.Step(1.0/60.0, 6, 2);
    update();
}

void SimulationWidgit::mousePressEvent(QMouseEvent *ev)
{
    bulletsToCreate++;
    //Play Bullet Sound
    if(bulletSound->playbackState() == QMediaPlayer::PlayingState){
        bulletSound->setPosition(0);
    }else if(bulletSound->playbackState() == QMediaPlayer::StoppedState){
        bulletSound->play();
    }
}

void SimulationWidgit::makeBullet()
{
    b2BodyDef bulletDef;
    bulletDef.type = b2_dynamicBody;
    bulletDef.position.Set(15.0f, 21.0f); //position body will fire from
    if(currentRoundAmmo == 0)
        return;

    b2Body* bullet = world.CreateBody(&bulletDef);

    b2PolygonShape bulletShape;
    bulletShape.SetAsBox(bulletSize/22.0f,bulletSize/22.0f);
    bullet->SetBullet(true);
    bullet->CreateFixture(&bulletShape, 1.0f)->SetUserData(this);

    bullet->SetLinearVelocity( mouseVector ); //speed/direction body will go
    bulletBodies.append(bullet);

    currentRoundAmmo--;
    QString s = QString::number(currentRoundAmmo);
    ui->AmmoAmmountLabel->setText(s);
}

void SimulationWidgit::makeDuck()
{

    b2BodyDef bodyDef;
    bodyDef.type = b2_dynamicBody;
    double leftOrRight = QRandomGenerator::global()->bounded(1, 3);
    double randomHeight = QRandomGenerator::global()->bounded(1, 8);
    double value = QRandomGenerator::global()->bounded(10, 15);
    double value2 = QRandomGenerator::global()->bounded(-8, 2);
    b2PolygonShape shape2;

    if(leftOrRight == 1){
        bodyDef.position.Set(0.0f, randomHeight);//position body will fire from
        b2Body* bodyTest = world.CreateBody(&bodyDef);
        shape2.SetAsBox(duckSize/40,duckSize/40);
        bodyTest->CreateFixture(&shape2, 1.0f)->SetSensor(false);
        bodyTest->SetLinearVelocity( b2Vec2( value, value2 ) ); //speed/direction body will go
        duckBodies.append(bodyTest);
    } else {
        bodyDef.position.Set(25.0f, randomHeight);//position body will fire from
        b2Body* bodyTest = world.CreateBody(&bodyDef);
        shape2.SetAsBox(duckSize/40,duckSize/40);
        bodyTest->CreateFixture(&shape2, 1.0f)->SetSensor(false);
        bodyTest->SetLinearVelocity( b2Vec2( -value, value2 ) ); //speed/direction body will go
        duckBodies.append(bodyTest);
    }
}

void SimulationWidgit::makeResultants(b2Vec2 position){
    resultantsToCreate++;
    if(Protons){
        resultantsToCreate++;
    }
    if(Positrons){
        resultantsToCreate++;
        resultantsToCreate++;
    }
    if(gammaRays){
        resultantsToCreate++;
    }
    if(Neutrinos){
        resultantsToCreate++;
    }
}

void SimulationWidgit::makeSingleResultant(b2Vec2 position, int resultType){
    b2BodyDef bodyDef;
    bodyDef.type = b2_dynamicBody;
    bodyDef.position.Set(position.x, position.y);//position body will fire from

    b2Body* bodyTest = world.CreateBody(&bodyDef);
    bodyTest->SetFixedRotation(true);
    b2PolygonShape shape2;
    shape2.SetAsBox(1.0f,1.0f);

    bodyTest->CreateFixture(&shape2, 1.0f)->SetSensor(true);

    double vecX = QRandomGenerator::global()->bounded(-4, 8);
    double vecY = QRandomGenerator::global()->bounded(-4, 8);

    bodyTest->SetLinearVelocity( b2Vec2( vecX, vecY ) ); //speed/direction body will go
    resultantBodies.insert(bodyTest, resultType);
}

void SimulationWidgit::setPositrons(bool setter){
    Positrons = setter;
}

void SimulationWidgit::setNeutrinos(bool setter){
    Neutrinos = setter;
}

void SimulationWidgit::setgammaRays(bool setter){
    gammaRays = setter;
}

void SimulationWidgit::setProtons(bool setter){
    Protons = setter;
}

void SimulationWidgit::setDuckSize(int newSize){
    duckSize = newSize;
    duckSprite = duckSprite.scaled( duckSize, duckSize, Qt::KeepAspectRatio);
}

void SimulationWidgit::setBulletSize(int newSize){
    bulletSize = newSize;
    bulletSprite = bulletSprite.scaled( duckSize, duckSize, Qt::KeepAspectRatio);
}

void SimulationWidgit::setResultantSize(int newSize){
    resultantSize = newSize;
    resultantSprite = resultantSprite.scaled( duckSize, duckSize, Qt::KeepAspectRatio);
}

void SimulationWidgit::setDuckSprite(QString newImagePath){
    QImage newImage = QImage(newImagePath);
    duckSprite = newImage.scaled( duckSize, duckSize, Qt::KeepAspectRatio);
}

void SimulationWidgit::setBulletSprite(QString newImagePath){
    QImage newImage = QImage(newImagePath);
    bulletSprite = newImage.scaled( bulletSize, bulletSize, Qt::KeepAspectRatio);
    QPixmap ammoImage = QPixmap::fromImage(bulletSprite);
    ui->AmmoPictureLabel->setPixmap(ammoImage);
}

void SimulationWidgit::setResultantSprite(QString newImagePath){
    QImage newImage = QImage(newImagePath);
    resultantSprite = newImage.scaled( resultantSize, resultantSize, Qt::KeepAspectRatio);
}

void SimulationWidgit::setDuckVelocity(int newVelocity)
{
    duckVelocity = newVelocity;
}

void SimulationWidgit::resetSimulation()
{
    currentRoundAmmo = roundCollisions;
    roundCollisions = 0;
    timer.start(15);
    timer2.start(2000);
}

void SimulationWidgit::resetGame()
{
    currentRoundAmmo = 20;
    roundCollisions = 0;
    timer.start(15);
    timer2.start(2000);
}

void SimulationWidgit::setSpriteImages(int roundNumber)
{
    if(roundNumber == 0)
    {
        Positrons = true;
        Neutrinos = true;
        gammaRays = false;
        Protons = false;
        setDuckSprite(":/TestPrefix/H1");
        setResultantSprite(":/TestPrefix/H2");
        setBulletSprite(":/TestPrefix/H1");
    }
    else if(roundNumber == 1)
    {
        Positrons = true;
        Neutrinos = true;
        gammaRays = false;
        Protons = false;
        setDuckSprite(":/TestPrefix/H1");
        setResultantSprite(":/TestPrefix/H2");
        setBulletSprite(":/TestPrefix/H1");
    }
    else if(roundNumber == 2)
    {
        QString collisionsText;
        collisionsText.append(QString::number(roundCollisions));
        ui->AmmoAmmountLabel->setText(collisionsText);
        setDuckSprite(":/TestPrefix/H2");
        setResultantSprite(":/TestPrefix/He3");
        Positrons = false;
        Neutrinos = false;
        gammaRays = true;
        Protons = false;
    }
    else if(roundNumber == 3)
    {
        QString collisionsText;
        collisionsText.append(QString::number(roundCollisions));
        ui->AmmoAmmountLabel->setText(collisionsText);
        setDuckSprite(":/TestPrefix/He3");
        setBulletSprite(":/TestPrefix/He3");
        setResultantSprite(":/TestPrefix/He4");
        Positrons = false;
        Neutrinos = false;
        gammaRays = false;
        Protons = true;
    }
}

void SimulationWidgit::setScoreText(int newText){
    QString s = QString::number(newText);
    ui->ScoreAmountLabel->setText(s);
}

void SimulationWidgit::setCurrentRoundAmmo(int newAmmoAmount){
    currentRoundAmmo = newAmmoAmount;
    QString s = QString::number(currentRoundAmmo);
    ui->AmmoAmmountLabel->setText(s);}

SimulationWidgit::~SimulationWidgit()
{
    delete ui;
}
