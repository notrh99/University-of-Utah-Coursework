#ifndef SPRITEMODEL_H
#define SPRITEMODEL_H
#include <QImage>
#include <QMainWindow>
#include <QTimer>
#include <QJsonObject>
#include <QJsonDocument>
#include <QJsonArray>
#include <QFile>
#include <iostream>
#include <QDebug>
#include <QMessageBox>

class SpriteModel : public QMainWindow
{
    Q_OBJECT

private:
    enum Tool{Pen,Eraser};
    Tool selectedTool;
    int penSize;
    int eraserSize;
    int currentToolSize;
    void parseJson(const QJsonObject&);
    QImage createQImageFromJsonArray(const QJsonArray&);
    QString convertEditorToJsonString();
    QJsonArray convertFrameToJsonObject(const QImage&);
    void refresh();
    void getPropertiesFromJson(const QJsonObject&);
    void getFramesFromJson(const QJsonObject&);
    void changePixels(int, int);

public:
    explicit SpriteModel(QWidget *parent = nullptr);
    std::vector<QImage> frames;
    uint currentColor;
    QColor qtcurrentColor;
    int height; //height in pixels of sprite
    int width;//width in pixels of sprite
    int currentFrame; //int represents which frame is active in frames
    int numberOfFrames;
    int fps;
    void addNewFrame();
    bool animationPopUpIsOpen; //true if pop up is open
    bool mainWindowEnabled;
    int animationSpotCounter;
    QTimer *timer;
    bool mirrorHorizontal; //true if horizontal mirror button is selected
    bool mirrorVertical; //true if vertical mirror button is selected
    int canvasSize;
    int editingWindowHeight; //holds the height of the label in editing window
    int editingWindowWidth;

signals:
    void setFrameViewText(QString);
    //animation popup signals
    void openAnimationPopup();
    void showImageToPopUp(QImage);
    void updateFPSForView(int);
    //editing window signals
    void showImage(QImage);
    void sendPictureDimensions(int,int);
    // pen and eraser signals
    void disablePenButtonHighlight();
    void disableEraserButtonHighlight();

public slots:
    //frameviewer slots
    void rightFrameClicked();
    void leftFrameClicked();
    void addFrameClicked();
    void removeFrame();
    //tool bar slots
    void changeToEraser();
    void changeToPen();
    void setMirrorHorizontal(bool);
    void setMirrorVerticle(bool);
    void invertCurrentFrame();
    void changePenColor(uint);
    //animationpopup slots
    void updateFPS(int);
    void animationPopUpOpen(bool);
    void sendShowImageToPopUp();
    void animationPopUp();
    //editingwindow slots
    void setHeight(int);
    void setWidth(int);
    void createProject();
    void changePixelOnMouseClick(QPoint);
    //saving/loading slots
    void setEditingWindowDimensions(int, int);

    void loadEditorFromJson(const QString&);
    void saveFramesToFile(const QString&);
    void changePenSize(int);
    void changeEraserSize(int);
};

#endif // SPRITEMODEL_H
