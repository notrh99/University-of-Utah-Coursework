/*
 * Author: Krish Mahtani & Rayyan Hamid
 * Date: 3rd October 2022
 * A4: Refactoring and Testing
 */

#include <stdlib.h>
#include <stdio.h>
#include <iostream>
#include <map>
#include "Node.h"

/*
 * default constructor
 */
Node::Node() {
    branches = std::map<char, Node>();
    isWord = false;
}

/*
 * adding a character to the node
 */
void Node::addChar(char alphabet) {
    branches[alphabet] = Node();
}

/*
 * checking if node has a character
 */
 bool Node::hasChar(char alphabet) {
    //return &(branches[alphabet]);
    if(branches.count(alphabet) == 1)
        return true;
    return false;
}

/*
 * setting the flag of the node being a word
 */
void Node::setIsWord(bool flag) {
    isWord = flag;
}

/*
 * setting the flag of the node being a word
 */
bool Node::getIsWord() {
    return isWord;
}