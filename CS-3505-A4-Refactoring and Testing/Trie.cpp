/*
 * Author: Krish Mahtani & Rayyan Hamid
 * Date: 3rd October 2022
 * A4: Refactoring and Testing
 */

#include <stdlib.h>
#include <stdio.h>
#include <iostream>
#include <vector>
#include "Trie.h"
#include "Node.h"

/*
 * default constructor
 */
Trie::Trie() {
    root = Node();
}

/*
 * addding a word to the trie
 */
void Trie::addAWord(std::string word) {
    Node* current = &root;
    for(unsigned int i = 0; i < word.length(); i++) {
        if(!current->hasChar(word[i])) {
            current->addChar(word[i]);
        }
        current = &(current->branches[word[i]]);
    }
    current->setIsWord(true);
}

/*
 * checking if word is in the trie
 */
bool Trie::isAWord (std::string word) {
    Node* current = &root;
    if(word == "")
        return false;
    for(unsigned int i = 0; i < word.length(); i++) {
        if(!current->hasChar(word[i]))
            return false;
        current = &(current->branches[word[i]]);
    }
    return current->getIsWord();
}

/*
 * helper method for allWordsBeginningWithPrefix method to traverse the trie
 */
void Trie::traverse(Node* node, std::vector<std::string>& words, std::string& prefix) {
    if(node == nullptr || node->branches.size() == 0)
        return;

    if(node->getIsWord())
        words.push_back(prefix);
   
    for(unsigned int i = 0; i < 26; i++) {
        char curr = 97+i;
        Node* temp = nullptr;
        if(node->hasChar(prefix[i])) {
            node = &(node->branches[prefix[i]]);
            temp = &(node->branches[prefix[i]]);
        }
        if(temp != nullptr) {
            prefix.push_back(curr);
            traverse(temp, words, prefix);
            prefix.pop_back();
        }
    }
}

/*
 * checking all words starting with the prefix
 */
std::vector<std::string> Trie::allWordsBeginningWithPrefix(std::string prefix) {
    std::vector<std::string> words;
    Node* current = &root;

    for(unsigned int i = 0; i < prefix.length(); i++) {
        Node* temp = nullptr;
        if(current->hasChar(prefix[i])) {
            current = &(current->branches[prefix[i]]);
            temp = &(current->branches[prefix[i]]);
        }
        if(temp == nullptr) {
            words.push_back("");
            return words;
        }
        current = temp;
    }
        traverse(current, words, prefix);
        return words;
}