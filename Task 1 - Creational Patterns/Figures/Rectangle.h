#pragma once
#include "Figure.h"

class Rectangle : public Figure
{
public:
	Rectangle(double sideA, double sideB);
	virtual std::unique_ptr<Figure> clone() const override final;
	virtual const std::string toString() const override final;

protected:
	virtual double CalculatePerimeter() const override final;

private:
	double sideA, sideB;
};