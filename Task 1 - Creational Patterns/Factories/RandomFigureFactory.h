#pragma once
#include "FigureFactory.h"

class RandomFigureFactory : public FigureFactory
{
public:
	virtual std::unique_ptr<Figure> createFigure() override final;
};