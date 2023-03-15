#ifndef TEACHINGMENU_H
#define TEACHINGMENU_H

#include <QWidget>
#include <QMovie>

namespace Ui {
class TeachingMenu;
}

class TeachingMenu : public QWidget
{
    Q_OBJECT

public:
    explicit TeachingMenu(QWidget *parent = nullptr);
    ~TeachingMenu();

    int currentRound;
    int currentTeachingPanel;
    void exitButtonClicked();

    std::map<int, std::vector<QPixmap>> teachingDictionary;

    std::vector<QPixmap> teachGameplay{QPixmap(":/TeachingMenuImages/teachingROneOne.png"),
                                       QPixmap(":/TeachingMenuImages/teachingROneTwo.png")};

    std::vector<QPixmap> teachRoundOne{QPixmap(":/TeachingMenuImages/teachingROneThree.png"),
                                       QPixmap(":/TeachingMenuImages/teachingROneFour.png"),
                                       QPixmap(":/TeachingMenuImages/teachingROneFive.png"),
                                       QPixmap(":/TeachingMenuImages/teachingROneSix.png")};

    std::vector<QPixmap> teachRoundTwo{QPixmap(":/TeachingMenuImages/teachingRTwoOne.png"),
                                       QPixmap(":/TeachingMenuImages/teachingRTwoTwo.png"),
                                       QPixmap(":/TeachingMenuImages/teachingRTwoThree.png")};

    std::vector<QPixmap> teachRoundThree{QPixmap(":/TeachingMenuImages/teachingRThreeOne.png"),
                                         QPixmap(":/TeachingMenuImages/teachingRThreeTwo.png"),
                                         QPixmap(":/TeachingMenuImages/teachingRThreeThree.png")};

    std::vector<QPixmap> teachEndGame{QPixmap(":/TeachingMenuImages/teachingGOOne.png"),
                                      QPixmap(":/TeachingMenuImages/teachingGOTwo.png"),
                                      QPixmap(":/TeachingMenuImages/teachingGOThree.png")};

public slots:
    void prepareTeachingMenu(int);
    void nextSlide();

signals:
    void exitTeachingMenu();
    void showTeachingMenu();
    void showHomeButton();

private:
    Ui::TeachingMenu *ui;
    QMovie *einsteinMovie;

};

#endif // TEACHINGMENU_H
