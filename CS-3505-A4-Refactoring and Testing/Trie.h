/*
 * Author: Krish Mahtani & Rayyan Hamid
 * Date: 3rd October 2022
 * A4: Refactoring and Testing
 */

#ifndef TRIE_H
#define TRIE_H

#include <stdlib.h>
#include <stdio.h>
#include <iostream>
#include <vector>
#include "Node.h"

class Trie {
    //variables
    Node root;

    //functions
    public:
        Trie();
        void addAWord(std::string);
        bool isAWord (std::string);
        std::vector<std::string> allWordsBeginningWithPrefix(std::string);
        void traverse(Node*, std::vector<std::string>&, std::string&);
};

#endif