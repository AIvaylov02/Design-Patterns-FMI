#pragma once
#include "Figure.h"

class Rectangle : public Figure
{
public:
	Rectangle(double sideA, double sideB);
	virtual std::unique_ptr<Figure> clone() const override;
	virtual const std::string toString() const override;

protected:
	virtual double CalculatePerimeter() const override;

private:
	double sideA, sideB;
};