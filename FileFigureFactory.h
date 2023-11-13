#pragma once
#include "FigureFactory.h"

class FileFigureFactory : public FigureFactory
{
public:
	static std::unique_ptr<Figure> createFigure(std::stringstream& input);
};