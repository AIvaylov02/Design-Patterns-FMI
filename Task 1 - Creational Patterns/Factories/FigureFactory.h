#pragma once
#include "Figures/Triangle.h"
#include "Figures/Circle.h"
#include "Figures/Rectangle.h"
#include "HelperFunctions.h"
#include <unordered_set>

class FigureFactory
{
public:
	virtual std::unique_ptr<Figure> createFigure() = 0;
protected:
	void ValidateFigureType(const std::string& str);
};