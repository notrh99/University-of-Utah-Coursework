LIBHARU = ./libharu
ENDS = -lhpdfs 
CC = g++
CFLAGS = -Wall -std=c++11

spiralPDF: Spiral.o HaruPDF.o spiralPDF.o spiralPDF.cpp
	$(CC) $(CFLAGS) Spiral.o HaruPDF.o spiralPDF.cpp -o spiralPDF -L $(LIBHARU)/src -I $(LIBHARU)/include $(ENDS)

Spiral.o: Spiral.cpp
	$(CC) $(CFLAGS) -c Spiral.cpp

HaruPDF.o: HaruPDF.cpp
	$(CC) $(CFLAGS) -c HaruPDF.cpp -L $(LIBHARU)/src -I $(LIBHARU)/include $(ENDS)

spiralPDF.o: spiralPDF.cpp
	$(CC) $(CFLAGS) -c spiralPDF.cpp -L $(LIBHARU)/src -I $(LIBHARU)/include $(ENDS)
clean:
	rm *.o
	rm spiralPDF

test:
	./spiralPDF "This is the text that will be turned into a spiral. Now we know that the spiral will work always and forever!"