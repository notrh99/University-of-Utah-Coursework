/*
Rayyan Hamid - u1298801
CS-3505 Fall 2022
A3 - A Trie and Rule-of-Three
*/

//Include Guards
#ifndef NODE_H
#define NODE_H

//Declare all the variables and methods that the Node class implements. 
class Node
{
    Node *letterArray[26];
    bool validWordFlag;

public:
    Node();
    void makeNodeCopy(Node*);
    void deleteNode();
    Node* hasCharacter(char);
    Node* addCharacter(char);
    void setValidWordFlag();
    bool isWordValid();

};

#endif
