/**
 * Assignment: A7: Sprite Editor
 * Authors: Shad Boswell, Zack Freeman, Jacob Hopkins, and Rayyan Hamid
 *
 * This code cannot be used without permission.
 */
#include "spritemodel.h"
/**
 * @brief SpriteModel::SpriteModel
 * @param parent
 *
 * This is the constructor for the Sprite Model.
 *
 * Reviewed by: Shad Boswell
 */
SpriteModel::SpriteModel(QWidget *parent)
    : QMainWindow{parent}
{
    animationPopUpIsOpen = false;
    mirrorHorizontal = false;
    mirrorVertical = false;
    mainWindowEnabled = false;

    fps = 1;
    penSize = 1;
    eraserSize = 1;
    currentToolSize = penSize;
    currentColor = qRgb(0,0,0);
    canvasSize = 1;
    height = 1;
    width = 1;
    editingWindowHeight = 400;
    editingWindowWidth = 400;
    currentFrame = 0;
    numberOfFrames = 1;
    animationSpotCounter = 0;
    selectedTool = Pen;

    QImage image(width,height, QImage::Format_ARGB32); //creates blank image 16x16
    image.fill(QColor(250,250,250,0)); // fill image with white pixels
    frames.push_back(image);
}

/**
 * @brief SpriteModel::addNewFrame
 *
 * Adds a new blank frame to frames
 */
void SpriteModel::addNewFrame()
{
    QImage image(width, height, QImage::Format_ARGB32); //creates blank image 16x16
    numberOfFrames++;

    image.fill(QColor(0,0,0,0)); // fill image with white pixels
    frames.push_back(image);
}

/**
 * @brief SpriteModel::changePixels
 * @param originalX
 * @param originalY
 *
 * This method changes the pixels within the currentToolSize X currentToolSize grid that
 * is located at originalX and originalY. How the pixels are changed and how many
 * depend on the selected tool.
 */
void SpriteModel::changePixels(int originalX, int originalY)
{
    for (int row = 0; row < currentToolSize; row++)
    {
        for (int col = 0; col < currentToolSize; col++)
        {
            if(selectedTool == Pen)
                frames[currentFrame].setPixel(originalX + col, originalY + row, currentColor);
            else
                frames[currentFrame].setPixelColor(originalX + col, originalY + row, QColor(0,0,0,0));
        }
    }
}

/**
 * @brief SpriteModel::changePixelOnMouseClick
 * @param pos
 *
 * This method uses the given position to calculate the corresponding editor window position.
 * Then the pixel at the editor position is changed according to the selected tools.
 */
void SpriteModel::changePixelOnMouseClick(QPoint pos)
{
    if(mainWindowEnabled == false)
        return;

    // get x and y coordinates of mouse
    float curx = pos.x();
    float cury = pos.y();

    // convert coordinates to pixel coordinates
    float x = (curx * (float)width)/(float)editingWindowWidth;
    float y = (cury * (float)height)/(float)editingWindowHeight;

    changePixels(x, y); // draw pixels will erase and draw
    if(selectedTool == Pen)
    {
        // mirrored coordinates
        float mirroredX = width - x;
        float mirroredY = height - y;

        if(mirrorHorizontal)
            changePixels(x, mirroredY);

        if(mirrorVertical)
            changePixels(mirroredX, y);

        if(mirrorVertical && mirrorHorizontal)
            changePixels(mirroredX, mirroredY);
    }

    emit showImage(frames[currentFrame]);
}

/**
 * @brief SpriteModel::addFrameClicked
 *
 * Slot from frameviewer to handle when new frame is clicked
 */
void SpriteModel::addFrameClicked()
{
    addNewFrame();
    QString s = "Current Frame: " + QString::number(currentFrame + 1) + "/" + QString::number(frames.size());
    emit setFrameViewText(s);
}

/**
 * @brief SpriteModel::removeFrame
 *
 * This method removes a frame from the editor and displays either the previous or next frame in the series
 */
void SpriteModel::removeFrame()
{
    if(currentFrame > 0)
    {
        emit showImage(frames[currentFrame - 1]);
        frames.erase(frames.begin() + currentFrame);
        currentFrame--;
    }
    else if(frames.size() > 1)
    {
        emit showImage(frames[currentFrame + 1]);
        frames.erase(frames.begin());
        currentFrame = 0;
    }

    numberOfFrames--;
    QString s = "Current Frame: " + QString::number(currentFrame + 1) + "/" + QString::number(frames.size());
    emit setFrameViewText(s);
}
/**
 * @brief SpriteModel::rightFrameClicked
 *
 * Slot from frameviewer to handle when shift to right frame is clicked
 */
void SpriteModel::rightFrameClicked()
{
    int totalFrame = frames.size();
    if(currentFrame + 1 < totalFrame)
    {
        currentFrame++;
        QString s = "Current Frame: " + QString::number(currentFrame + 1) + "/" + QString::number(frames.size());
        emit setFrameViewText(s);
        emit showImage(frames[currentFrame]);
    }
}

/**
 * @brief SpriteModel::leftFrameClicked
 *
 * Slot from frameviewer to handle when shift to left frame is clicked
 */
void SpriteModel::leftFrameClicked()
{
    if(currentFrame > 0)
    {
        currentFrame--;
        QString s = "Current Frame: " + QString::number(currentFrame + 1) + "/" + QString::number(frames.size());
        emit setFrameViewText(s);
        emit showImage(frames[currentFrame]);
    }
}

/**
 * @brief SpriteModel::animationPopUp
 *
 * Slot from animationpopup, tells main window to display animation pop up
 */
void SpriteModel::animationPopUp()
{
    animationPopUpIsOpen = true;
    emit openAnimationPopup();
    //playAnimation();
}

/**
 * @brief SpriteModel::updateFPS
 * @param sliderFPS desired fps
 *
 * Slot from animationpopup, updatesfps based on slider
 */
void SpriteModel::updateFPS(int sliderFPS)
{
    fps = sliderFPS;
    emit updateFPSForView(fps);
}

/**
 * @brief SpriteModel::setEditingWindowDimensions
 * @param h
 * @param w
 *
 * Slot from editing window, sets editing window dimension variables given by editing window
 */
void SpriteModel::setEditingWindowDimensions(int h, int w)
{
    editingWindowHeight = h;
    editingWindowWidth = w;
}

/**
 * @brief SpriteModel::sendShowImageToPopUp
 *
 * Sends an image in frames at current animationSpot counter
 * then moves the counter to the next frame.
 * Tells animationpopup what image to display
 */
void SpriteModel::sendShowImageToPopUp()
{
    if(animationSpotCounter == frames.size())
           animationSpotCounter = 0;

    QImage &testframe = frames.at(animationSpotCounter);
    emit showImageToPopUp(testframe);
    animationSpotCounter++;
}

/**
 * @brief SpriteModel::animationPopUpOpen
 * @param animationWindowOpen true if the popup is open
 *
 * Slot for animationpopup, sets instance
 * variable in the model to keep track if the pop up is open
 */
void SpriteModel::animationPopUpOpen(bool animationWindowOpen)
{
    animationPopUpIsOpen = animationWindowOpen;
}

/**
 * @brief SpriteModel::setHeight
 * @param y height in pixels
 *
 * Sets the current height of the sprite in pixels
 */
void SpriteModel::setHeight(int y)
{
    height = y;
}

/**
 * @brief SpriteModel::setWidth
 * @param x width in pixels
 *
 * Sets the current width of the sprite in pixels
 */
void SpriteModel::setWidth(int x)
{
    width = x;
}

/**
 * @brief SpriteModel::createProject
 * Creates a new sprite, uses current width and height
 */
void SpriteModel::createProject()
{
    frames.clear();
    QImage image(width,height, QImage::Format_ARGB32); //creates blank image 16x16
    image.fill(QColor(250,250,250,0)); // fill image with white pixels
    frames.push_back(image);
    mainWindowEnabled = true;
    emit sendPictureDimensions(width,height);
    emit showImage(image);
}

/**
 * @brief SpriteModel::changePenColor
 * @param newColor
 *
 * Changes the current color of the pen tool
 */
void SpriteModel::changePenColor(uint newColor)
{
    currentColor = newColor;
}

/**
 * @brief SpriteModel::changeToEraser
 *
 * Changes to the eraser tool
 */
void SpriteModel::changeToEraser()
{
    selectedTool = Eraser;
    currentToolSize = eraserSize;
    emit disablePenButtonHighlight();
}

/**
 * @brief SpriteModel::changeToPen
 *
 * Changes to the pen tool
 */
void SpriteModel::changeToPen()
{
    selectedTool = Pen;
    currentToolSize = penSize;
    emit disableEraserButtonHighlight();
}

/**
 * @brief SpriteModel::setMirrorHorizontal
 * @param mirrorHorizontalSelected
 *
 * Either selects or deselects the mirror horizontal tool
 */
void SpriteModel::setMirrorHorizontal(bool mirrorHorizontalSelected)
{
    mirrorHorizontal = mirrorHorizontalSelected;
}

/**
 * @brief SpriteModel::setMirrorVerticle
 * @param mirrorVerticalSelected
 *
 * Either selects or deselects the mirror vertical tool
 */
void SpriteModel::setMirrorVerticle(bool mirrorVerticalSelected)
{
    mirrorVertical = mirrorVerticalSelected;
}

/**
 * @brief SpriteModel::invertCurrentFrame
 * This method changes currently viewed frame to have inverted colors
 */
void SpriteModel::invertCurrentFrame()
{
    frames[currentFrame].invertPixels();
    emit showImage(frames[currentFrame]);
}

/**
 * @brief SpriteModel::loadEditorFromJson
 * @param fileName
 *
 * This opens a file with the given file name and reads all of the contents.
 * This method also checks to see if the file contains a Json object.
 */
void SpriteModel::loadEditorFromJson(const QString &fileName)
{
    QFile file(fileName);
    QString contents;

    if(!file.open(QIODevice::ReadOnly))
    {
        QMessageBox fileError;
        fileError.critical(0,"Error","Cannot open file");
        fileError.show();
        return;
    }

    QTextStream fileStream(&file);
    contents = fileStream.readAll();
    file.close();
    QJsonObject fileJson;

    QJsonDocument fileDocument = QJsonDocument::fromJson(contents.toUtf8());

    if(!fileDocument.isNull())
    {
        if(fileDocument.isObject())
        {
            fileJson = fileDocument.object();
            parseJson(fileJson);
        }
    }
}

/**
 * @brief SpriteModel::parseJson
 * @param editorJson
 *
 * This method calls the appropriate helper methods to parse the given Json object
 */
void SpriteModel::parseJson(const QJsonObject &editorJson)
{
    getPropertiesFromJson(editorJson);
    getFramesFromJson(editorJson);
    refresh();
}

/**
 * @brief SpriteModel::getFramesFromJson
 * @param editorJson
 *
 * This method parses the frames from the given Json Object and adds
 * those frames to the frames vector
 */
void SpriteModel::getFramesFromJson(const QJsonObject &editorJson)
{
    if(editorJson.contains("frames"))
    {
        if(editorJson["frames"].isObject())
        {
            frames.clear();
            QJsonObject frameN = editorJson["frames"].toObject();
            for (auto const &frame : frameN)
            {
                if(frame.isArray())
                {
                    QJsonArray imageRgba = frame.toArray();
                    QImage image = createQImageFromJsonArray(imageRgba);
                    frames.push_back(image);
                }
                else
                {
                    QMessageBox errorBox;
                    errorBox.critical(0,"Error","File does not contain correct format");
                    errorBox.show();
                }
            }
        }
    }
}

/**
 * @brief SpriteModel::getPropertiesFromJson
 * @param editorJson
 *
 * This method parses and sets the properties of the sprite from the
 * given Json object.
 */
void SpriteModel::getPropertiesFromJson(const QJsonObject &editorJson)
{
    if(editorJson.contains("height") && editorJson["height"].isDouble())
        height = editorJson["height"].toInt();

    if(editorJson.contains("width") && editorJson["width"].isDouble())
        width = editorJson["width"].toInt();

    if(editorJson.contains("numberOfFrames") && editorJson["numberOfFrames"].isDouble())
        numberOfFrames = editorJson["numberOfFrames"].toInt();
}

/**
 * @brief SpriteModel::createQImageFromJsonArray
 * @param frame
 * @return
 *
 * This method parses a Json array of rgba values and turns it into a QImage.
 * Used for loading a sprite to the window
 */
QImage SpriteModel::createQImageFromJsonArray(const QJsonArray &frame)
{
    QImage image(width, height, QImage::Format_ARGB32);
    for(int row = 0; row < height; row++)
    {
        for(int col = 0; col < width; col++)
        {
            int red = frame[row][col][0].toInt();
            int green = frame[row][col][1].toInt();
            int blue = frame[row][col][2].toInt();
            int alpha = frame[row][col][3].toInt();
            image.setPixel(col, row, qRgba(red, green, blue, alpha));
        }
    }

    return image;
}

/**
 * @brief SpriteModel::saveFramesToFile
 * @param fileName
 *
 * This method opens a file with the given file name and calls the appropriate method
 * to convert the current state of the editor to a Json String
 */
void SpriteModel::saveFramesToFile(const QString &fileName)
{
    QFile file(fileName);

    if(!file.open(QIODevice::WriteOnly))
    {
        QMessageBox fileError;
        fileError.critical(0,"Error","Cannot open file");
        fileError.show();
        return;
    }

    QTextStream fileStream(&file);
    fileStream << convertEditorToJsonString() << Qt::endl;
    file.close();
}

/**
 * @brief SpriteModel::convertEditorToJsonString
 * @return
 *
 * This method converts the current state of the editor a Json String.
 * It calls teh appropriate method to convert each method to a Json Array.
 */
QString SpriteModel::convertEditorToJsonString()
{
    QJsonObject editorObject;

    editorObject["height"] = height;
    editorObject["width"] = width;
    editorObject["numberOfFrames"] = numberOfFrames;
    QJsonObject frameArray;

    for(int i = 0; i < numberOfFrames; i++)
    {
        QString s = "frame" + QString::number(i);
        frameArray[s] = (convertFrameToJsonObject(frames[i]));
    }

    editorObject["frames"] = frameArray;
    QJsonDocument editorDocument(editorObject);
    QByteArray editorByteArray = editorDocument.toJson();

    return QString(editorByteArray);
}

/**
 * @brief SpriteModel::convertFrameToJsonObject
 * @param image
 * @return
 *
 * This method converts the given QImage to a Json Object of arrays
 * of rgba values.
 */
QJsonArray SpriteModel::convertFrameToJsonObject(const QImage &image)
{
    QJsonArray array;

    for(int row = 0; row < height; row++)
    {
        QJsonArray rowArray;
        for(int col = 0; col < width; col++)
        {
            QColor color = image.pixelColor(col, row);
            int red = color.red();
            int green = color.green();
            int blue = color.blue();
            int alpha = color.alpha();

            QJsonArray colors;
            colors.append(red);
            colors.append(green);
            colors.append(blue);
            colors.append(alpha);
            rowArray.append(colors);
        }
        array.append(rowArray);
    }

    return array;
}

/**
 * @brief SpriteModel::refresh
 *
 * This method refreshes the editor when a sprite is loaded
 */
void SpriteModel::refresh()
{
    emit showImage(frames[0]);
    QString string = "Current Frame: " + QString::number(1) + "/" + QString::number(numberOfFrames);
    currentFrame = 0;
    emit setFrameViewText(string);
    emit sendPictureDimensions(width, height);
    mainWindowEnabled = true;
}

/**
 * @brief SpriteModel::changePenSize
 * @param newSize
 *
 * This method changes the pen size to the given size
 */
void SpriteModel::changePenSize(int newSize)
{
    penSize = newSize;
    if(selectedTool == Pen)
        currentToolSize = penSize;
}

/**
 * @brief SpriteModel::changeEraserSize
 * @param newSize
 *
 * This method changes the eraser size to the given size
 */
void SpriteModel::changeEraserSize(int newSize)
{
    eraserSize = newSize;
    if(selectedTool == Eraser)
        currentToolSize = eraserSize;
}
