#ifndef CONTACTLISTENER_H
#define CONTACTLISTENER_H
#include "Box2D/Box2D.h"
#include "qtmetamacros.h"
#include <QMainWindow>


class ContactListener : public QMainWindow, public b2ContactListener
{
    Q_OBJECT;
public:
    ContactListener();
    void BeginContact(b2Contact* contact);
    void EndContact(b2Contact* contact);
signals:
    void deleteBodies(b2Body* a, b2Body* b);

};

#endif // CONTACTLISTENER_H
