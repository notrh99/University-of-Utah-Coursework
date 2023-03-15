/*
Rayyan Hamid - u1298801
CS-3505 Fall 2022
A3 - A Trie and Rule-of-Three
*/
#include <iostream>
#include <vector>
#include <string>
#include "Node.h"

using namespace std;
/*
Main constructor for Node class.
*/
Node::Node()
{
    // Filling Array with null pointers
    for (int counter = 0; counter < 26; counter++)
    {
        letterArray[counter] = nullptr;
    }
    validWordFlag = false;
}

/*
This method serves as our copy constructor.
*/
void Node::makeNodeCopy(Node *copyNode)
{
    for (int counter = 0; counter < 26; counter++)
    {
        if (letterArray[counter] != nullptr)
        {

            copyNode->letterArray[counter] = new Node;
            // Copying the bool word flag
            copyNode->letterArray[counter]->validWordFlag = letterArray[counter]->validWordFlag;
            // Calling the method recursively until all pointers are updated.
            letterArray[counter]->makeNodeCopy(copyNode->letterArray[counter]);
        }
    }
}

/*
This method is our destructor. It loops through all the values in the array and deletes
the reference and sets it back to null. Through recursion it ensures everythig is deleted.
*/
void Node::deleteNode()
{
    for (int counter = 0; counter < 26; counter++)
    {
        if (letterArray[counter] != nullptr)
        {
            letterArray[counter]->deleteNode();
            // Calling the method recursively until the bottom node.
            delete letterArray[counter];
            letterArray[counter] = nullptr;
        }
    }
}
/*
Checking if the node has the character.
*/
Node *Node::hasCharacter(char character)
{
    int indexLocation = character - 97;
    return letterArray[indexLocation];
}

/*
This method adds a character to the node.
*/
Node *Node::addCharacter(char character)
{
    int indexLocation = character - 97;
    letterArray[indexLocation] = new Node();
    return letterArray[indexLocation];
}

/*
This methods sets the boolean flag at the words end.
*/
void Node::setValidWordFlag()
{
    validWordFlag = true;
}

/*
This method returns the boolean flag.
*/
bool Node::isWordValid()
{
    return validWordFlag;
}
