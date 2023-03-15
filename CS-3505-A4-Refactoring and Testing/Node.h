/*
 * Author: Krish Mahtani & Rayyan Hamid
 * Date: 3rd October 2022
 * A4: Refactoring and Testing
 */

#ifndef NODE_H
#define NODE_H

#include <stdlib.h>
#include <stdio.h>
#include <iostream>
#include <map>

class Node {
    //functions
    public:
        std::map<char, Node> branches;
        bool isWord;

    //variables
    public:
        Node();
        // Node* addChar(char);
        // Node* hasChar(char);
        void addChar(char);
        bool hasChar(char);
        void setIsWord(bool);
        bool getIsWord();
};

#endif