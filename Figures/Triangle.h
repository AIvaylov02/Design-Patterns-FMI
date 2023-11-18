#pragma once
#include "Figure.h"

class Triangle : public Figure
{
public:
	Triangle(double sideA, double sideB, double sideC);
	virtual std::unique_ptr<Figure> clone() const override final;
	virtual const std::string toString() const override final;

protected:
	virtual double CalculatePerimeter() const override final;

private:
	double sideA, sideB, sideC;
	void ValidateTriangle() const;
};