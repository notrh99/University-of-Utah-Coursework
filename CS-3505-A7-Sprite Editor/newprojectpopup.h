#ifndef NEWPROJECTPOPUP_H
#define NEWPROJECTPOPUP_H

#include <QDialog>
#include "spritemodel.h";

namespace Ui { class newProjectPopup; }

class newProjectPopup : public QDialog
{
    Q_OBJECT

public:
    explicit newProjectPopup(SpriteModel& model, QWidget *parent = nullptr);
    ~newProjectPopup();

private slots:
    void createClicked();
    void setWidthValueLabel(int);
    void setHeightValueLabel(int);

private:
    Ui::newProjectPopup *ui;
    SpriteModel *model;

};

#endif // NEWPROJECTPOPUP_H
