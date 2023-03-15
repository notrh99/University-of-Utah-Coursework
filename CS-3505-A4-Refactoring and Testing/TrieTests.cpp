/*
 * Author: Krish Mahtani & Rayyan Hamid
 * Date: 3rd October 2022
 * A4: Refactoring and Testing
 */

#include "Trie.h"
#include <gtest/gtest.h>

using namespace std;


TEST(TrieTests, testIsAWord)
{
    Trie trie;

    ASSERT_FALSE(trie.isAWord("utah"));
    trie.addAWord("utah");
    ASSERT_TRUE(trie.isAWord("utah"));
}

TEST(TrieTests, testShortWord)
{
    Trie trie;

    ASSERT_FALSE(trie.isAWord("x"));
    trie.addAWord("x");
    ASSERT_TRUE(trie.isAWord("x"));
}

TEST(TrieTests, testWeirdWords)
{
    Trie trie;

    ASSERT_FALSE(trie.isAWord("rrrrrrrrrr"));
    trie.addAWord("rrrrrrrrrr");
    ASSERT_TRUE(trie.isAWord("rrrrrrrrrr"));
    ASSERT_FALSE(trie.isAWord("rrrrrrr"));
}

// Edge Cases
TEST(TrieTests, testDifferentWord)
{
    Trie trie;
    trie.addAWord("skydive");
    ASSERT_FALSE(trie.isAWord("skydives"));
}

TEST(TrieTests, testEmptyString)
{
    Trie trie;
    trie.addAWord("");
    ASSERT_FALSE(trie.isAWord(""));
}

TEST(TrieTests, halfWords)
{
    Trie trie;
    trie.addAWord("skydive");
    ASSERT_FALSE(trie.isAWord("sky"));
}

TEST(TrieTests, testALongString)
{
    Trie trie;
    trie.addAWord("qwertyuiopasdfghjklzxcvbnm");
    ASSERT_TRUE(trie.isAWord("qwertyuiopasdfghjklzxcvbnm"));
}

TEST(TrieTests, copyToNewTrie)
{
    Trie trie;
    trie.addAWord("orange");
    Trie secondTrie = trie;
    ASSERT_TRUE(secondTrie.isAWord("orange"));
}

TEST(TrieTests, testPrefixWithSameWord)
{
    Trie trie;
    trie.addAWord("galaxy");
    vector<string> wordsList = trie.allWordsBeginningWithPrefix("galaxy");
    ASSERT_EQ(wordsList[0], "galaxy");
}


TEST(TrieTests, testPrefixeBetweenDifferentTries)
{
    Trie trie;
    trie.addAWord("hello");
    Trie secondTrie = trie;
    ASSERT_EQ("hello", secondTrie.allWordsBeginningWithPrefix("he")[0]);
}

TEST(TrieTests, testPrefixWithCopyTrie)
{
    Trie trie;
    trie.addAWord("earth");
    Trie secondTrie = trie;
    ASSERT_EQ("earth", secondTrie.allWordsBeginningWithPrefix("ear")[0]);
    ASSERT_FALSE(secondTrie.isAWord("red"));
    secondTrie.addAWord("red");
    ASSERT_TRUE(secondTrie.isAWord("red"));
    ASSERT_EQ("red", secondTrie.allWordsBeginningWithPrefix("re")[0]);
    ASSERT_FALSE(trie.isAWord("red"));
}

