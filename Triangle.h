#pragma once
#include "Figure.h"

class Triangle : public Figure
{
public:
	Triangle(double sideA, double sideB, double sideC);
	virtual std::unique_ptr<Figure> clone() const override;
	virtual const std::string toString() const override;

protected:
	virtual double CalculatePerimeter() const override;

private:
	double sideA, sideB, sideC;
};