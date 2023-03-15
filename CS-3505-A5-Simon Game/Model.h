/*
 * Author: Krish Mahtani & Rayyan Hamid
 * Date: 28th October 2022
 * A5: Qt Simon Game
*/
#ifndef MODEL_H
#define MODEL_H

#include <QObject>
#include <list>

class Model : public QObject
{
    Q_OBJECT
public:
    explicit Model(QObject *parent = nullptr);
    std::list<char> generateSequence();
    void checkButton(char);
    int sequenceSize();
    void percentageOfProgressBar();
    void reset();


private:
    std::list<char> simonSequence;
    int currStep;
    void displaySimonsSequence();

signals:
    void disableButton();
    void changeRedButtonColour(int);
    void changeBlueButtonColour(int);
    void gameLost();
    void youLoseMessage();
    void yourTurnMessage();
    void progressBarUpdate(int);
    void winMessage();

public slots:
    void startButtonClicked();
    void redButtonClicked();
    void blueButtonClicked();

};

#endif
