#pragma once
#include "FigureFactory.h"

class RandomFigureFactory : public FigureFactory
{
public:
	static std::unique_ptr<Figure> createFigure();
};