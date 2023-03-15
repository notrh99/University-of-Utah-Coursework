#Rayyan Hamid - u1298801
#CS-3505 Fall 2022
#A3 - A Trie and Rule-of-Three

CC = g++
CFLAGS = -Wall -std=c++11 -fsanitize=address -fsanitize=undefined


TrieTest: TrieTest.cpp Trie.o Node.o
	$(CC) $(CFLAGS) -o TrieTest Trie.o Node.o TrieTest.cpp

Node.o: Node.cpp Node.h 
	$(CC) $(CFLAGS) -c  Node.cpp

Trie.o: Trie.h Trie.cpp
	$(CC) $(CFLAGS) -c Trie.cpp

clean:
	rm *.o
	rm TrieTest