 # Author: Krish Mahtani & Rayyan Hamid
 # Date: 3rd October 2022
 # A4: Refactoring and Testing
 

CC = g++
CFLAGS = -Wall -std=c++14

TrieTests.o: TrieTests.cpp
	$(CC) -I./googletest/googletest/include $(CFLAGS) -c TrieTests.cpp

TrieTests: Trie.o TrieTests.o Node.o
	$(CC) -I./googletest/googletest/include $(CFLAGS) -o TrieTests Node.o Trie.o TrieTests.o -L./googletest/lib -lgtest -lgtest_main -lpthread

Node.o: Node.h Node.cpp
	$(CC) $(CFLAGS) -c -I. Node.cpp

Trie.o: Trie.h Trie.cpp
	$(CC) $(CFLAGS) -c -I. Trie.cpp

Test: TrieTests
	./TrieTests

clean:
	rm *.o
	rm TrieTests