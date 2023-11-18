#pragma once
#include "FigureFactory.h"
#include "FileContainers/InputFileContainer.h"

class FileFigureFactory : public FigureFactory
{
public:
	FileFigureFactory();
	virtual std::unique_ptr<Figure> createFigure() override final;
	int ReadFiguresCount();
private:
	InputFileContainer fileContainer;
};