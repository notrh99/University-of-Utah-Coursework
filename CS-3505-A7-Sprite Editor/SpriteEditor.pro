QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

CONFIG += c++17

# You can make your code fail to compile if it uses deprecated APIs.
# In order to do so, uncomment the following line.
#DEFINES += QT_DISABLE_DEPRECATED_BEFORE=0x060000    # disables all the APIs deprecated before Qt 6.0.0

SOURCES += \
    animationpopup.cpp \
    borderlayout.cpp \
    editingwindow.cpp \
    frameviewer.cpp \
    main.cpp \
    mainwindow.cpp \
    newprojectpopup.cpp \
    spritemodel.cpp \
    toolbar.cpp

HEADERS += \
    animationpopup.h \
    borderlayout.h \
    editingwindow.h \
    frameviewer.h \
    mainwindow.h \
    newprojectpopup.h \
    spritemodel.h \
    toolbar.h

FORMS += \
    animationpopup.ui \
    editingwindow.ui \
    frameviewer.ui \
    mainwindow.ui \
    newprojectpopup.ui \
    toolbar.ui

# Default rules for deployment.
qnx: target.path = /tmp/$${TARGET}/bin
else: unix:!android: target.path = /opt/$${TARGET}/bin
!isEmpty(target.path): INSTALLS += target

RESOURCES += \
    Resources.qrc
