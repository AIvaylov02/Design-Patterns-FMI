#pragma once
#include "Figure.h"

class Circle : public Figure
{
public:
	Circle(double radius);
	virtual std::unique_ptr<Figure> clone() const override;
	virtual const std::string toString() const override;

protected:
	virtual double CalculatePerimeter() const override;

private:
	double radius;
};