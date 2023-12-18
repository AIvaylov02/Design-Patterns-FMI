#pragma once
#include "Figure.h"

class Circle : public Figure
{
public:
	Circle(double radius);
	virtual std::unique_ptr<Figure> clone() const override final;
	virtual const std::string toString() const override final;

protected:
	virtual double CalculatePerimeter() const override final;

private:
	double radius;
};