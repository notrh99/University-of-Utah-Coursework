/*
Rayyan Hamid - u1298801
CS-3505 Fall 2022
A3 - A Trie and Rule-of-Three
*/
#include "Trie.h"
#include "Node.h"
#include <iostream>
#include <vector>
#include <string>
#include <fstream>

using namespace std;

// This main method takes in two filenames as input.
int main(int argc, char *argv[])
{
    if (argc != 3)
    {
        cout << "Please give two filenames." << endl;
        return 0;
    }

    // string to read the values in the files
    string inputWord = "";
    // strings to hold the words and queries files
    string words = argv[1];
    string queries = argv[2];
    // to read the contents of the file
    ifstream inputStream;
    // Making a Trie object
    Trie *newTrie = new Trie();

    // open the word file
    inputStream.open(words);
    // read the file if its open
    if (inputStream.is_open())
    {
        // read each line from the file
        while (getline(inputStream, inputWord))
        {
            // Adding the word to the Trie
            newTrie->addAWord(inputWord);
        }
    }
    else
    {
        // Error Message
        cout << "The words file had an error, try again." << endl;
        return 0;
    }
    // Close words file
    inputStream.close();
    inputWord = "";

    // open the queries file
    inputStream.open(queries);
    // read the file if its open
    if (inputStream.is_open())
    {
        // read each line from the file
        while (getline(inputStream, inputWord))
        {
            // If the word is found, print message
            if (newTrie->isAWord(inputWord))
            {
                cout << inputWord << " is found." << endl;
            }
            else
            {
                // Find the suggested words with the prefix
                vector<string> wordsFromQueries = newTrie->allWordsBeginningWithPrefix(inputWord);
                // If no suggestions found, print message
                if (wordsFromQueries.size() == 0)
                {
                    cout << "no alternatives found" << endl;
                }
                // If suggested words are found, print list with the words.
                else
                {
                    cout << inputWord << " is not found, did you mean:" << endl;
                    for (string &word : wordsFromQueries)
                    {
                        cout << "   " << word << endl;
                    }
                }
            }
        }
    }
    else
    {
        // Error Message
        cout << "The queries file had an error, try again." << endl;
        return 0;
    }
    // Delete Trie object
    delete newTrie;
    return 0;
}