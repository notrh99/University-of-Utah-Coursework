/*
Rayyan Hamid - u1298801
CS-3505 Fall 2022
A3 - A Trie and Rule-of-Three
*/
// Include Guards
#ifndef TRIE_H
#define TRIE_H

#include <iostream>
#include <vector>
#include "Node.h"

using namespace std;

// Declare all the variables and methods that the Trie class implements.
class Trie
{
public:
    Node *rootNode;
    Trie();
    ~Trie();
    Trie(const Trie &);
    void addAWord(std::string);
    bool isAWord(std::string);
    Trie &operator=(Trie);
    vector<string> allWordsBeginningWithPrefix(string wordString);
    void TrieTraversal(Node *currentNOde, string &prefix, vector<std::string> &list);
};

#endif