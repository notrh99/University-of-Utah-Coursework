#include "contactlistener.h"
#include "simulationwidgit.h"
#include <iostream>

ContactListener::ContactListener()
{

}

void ContactListener::BeginContact(b2Contact* contact)
{
    b2Body* bodyA = contact->GetFixtureA()->GetBody();
       b2Body* bodyB = contact->GetFixtureB()->GetBody();
       if(contact->GetFixtureA()->IsSensor() || contact->GetFixtureB()->IsSensor()){

       } else {
           emit deleteBodies(bodyA, bodyB);
       }
}

void ContactListener::EndContact(b2Contact* contact)
{

}

