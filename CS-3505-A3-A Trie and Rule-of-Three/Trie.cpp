/*
Rayyan Hamid - u1298801
CS-3505 Fall 2022
A3 - A Trie and Rule-of-Three
*/
#include <vector>
#include <string>
#include "Trie.h"
#include "Node.h"

using namespace std;

// Constructor
Trie::Trie()
{
    rootNode = new Node();
}

// Destructor
Trie::~Trie()
{
    // Deleting all the node pointers.
    rootNode->deleteNode();
    // Deleting rootNode pointer.
    delete rootNode;
}

// Copy Constructor
Trie::Trie(const Trie &givenTrie)
{
    //Make a new root node
    rootNode = new Node;
    givenTrie.rootNode->makeNodeCopy(this->rootNode);
}

// This method is our override for the = operator.
Trie &Trie::operator=(Trie givenTrie)
{
    //Make a new root node
    rootNode = new Node;
    givenTrie.rootNode->makeNodeCopy(this->rootNode);
    return *this;
}

// This method adds words to the Trie.
void Trie::addAWord(std::string wordString)
{
    // Making a copy of the root node.
    Node *currentNode = rootNode;
    unsigned int counter;
    for (counter = 0; counter < wordString.length(); counter++)
    {
        // if the current node is the last node
        Node *endNode = currentNode;

        // if the index equals to the last letter of the word.
        if (counter == wordString.length() - 1)
        {
            // Checking if the node is null
            if (currentNode->hasCharacter(wordString[counter]) == nullptr)
            {
                // adding character
                currentNode = endNode->addCharacter(wordString[counter]);
                // set flag
                currentNode->setValidWordFlag();
                break;
            }
            else
            {
                // set flag
                currentNode->setValidWordFlag();
                break;
            }
        }
        // if the node is null
        if ((currentNode = currentNode->hasCharacter(wordString[counter])) == nullptr)
        {
            // Making a new node and adding the character.
            currentNode = endNode->addCharacter(wordString[counter]);
        }
    }
}

// This method checks if a word is already added to the Trur
bool Trie::isAWord(std::string wordString)
{
    // Check to see it the word given is null
    if (wordString.empty())
    {
        return false;
    }

    // Making a copy of the root node.
    Node *currentNode = rootNode;

    // If the node is null, return false.
    if (currentNode == NULL)
    {
        return false;
    }
    else
    {
        unsigned int counter;
        // Looping through the characters.
        for (counter = 0; counter < wordString.length(); counter++)
        {
            // Storing character at counter to the temporary node.
            Node *tempNode = currentNode->hasCharacter(wordString[counter]);

            if (tempNode == NULL)
            {
                return false;
            }
            currentNode = tempNode;
        }
        // Returns the bool flag.
        return (currentNode->isWordValid());
    }
}

// This method returns a list of words, that begin with method parameter input prefix.
vector<string> Trie::allWordsBeginningWithPrefix(string wordString)
{
    // Creating a new vector of strings.
    vector<string> wordList;

    Node *currentNode = rootNode;
    unsigned int counter;
    // Looping through the characters.
    for (counter = 0; counter < wordString.length(); counter++)
    {
        Node *tempNode = currentNode->hasCharacter(wordString[counter]);
        // If node is null return the list.
        if (tempNode == NULL)
        {
            return wordList;
        }
        currentNode = tempNode;
    }

    TrieTraversal(currentNode, wordString, wordList);
    return wordList;
}

// This method serves as the helper method for allWordsBeginningWithPrefix.
void Trie::TrieTraversal(Node *currentNode, string &prefix, vector<std::string> &list)
{
    if (currentNode != NULL)
    {

        if (currentNode->isWordValid())
        {
            // Add the word to the word vector.
            list.push_back(prefix);
        }
        unsigned char counter;
        // Iterate through all pointers in the array.
        for (counter = 0; counter < 26; counter++)
        {
            // Generate next letter
            char character = 97 + counter;
            Node *nodeTemp = currentNode->hasCharacter(character);
            // Add the charater to the word vector.
            prefix.push_back(character);
            // Calling the method again to search the next node.
            TrieTraversal(nodeTemp, prefix, list);
            prefix.pop_back();
        }
    }
    else
    {
        return;
    }
}