#include <QCoreApplication>
#include <QtDebug>

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

    char* name = "John Doe <Default Name>";
    if(argc == 1)
        qInfo() << ("Hello World %s",name);
    return a.exec();
}
