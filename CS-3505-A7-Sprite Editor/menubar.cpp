#include "menubar.h"
#include "ui_menubar.h"
#include "spritemodel.h"
#include <QMenuBar>
#include <QMenu>

/**
 * @brief MenuBar::MenuBar
 * @param model
 * @param parent
 *
 * This is the constu
 */
MenuBar::MenuBar(SpriteModel& model, QWidget *parent) :
    QWidget(parent),
    ui(new Ui::MenuBar)
{
    this->model = &model;

    ui->setupUi(this);
}

MenuBar::~MenuBar()
{
    delete ui;
}
