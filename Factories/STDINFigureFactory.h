#pragma once
#include "FigureFactory.h"

class STDINFigureFactory : public FigureFactory
{
public:
	virtual std::unique_ptr<Figure> createFigure() override final;
};