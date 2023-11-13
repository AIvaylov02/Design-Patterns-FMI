#pragma once
#include "FigureFactory.h"

class STDINFigureFactory : public FigureFactory
{
public:
	static std::unique_ptr<Figure> createFigure(std::stringstream& input);
};