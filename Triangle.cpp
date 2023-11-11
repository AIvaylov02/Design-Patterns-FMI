#include "Triangle.h"

Triangle::Triangle(double sideA, double sideB, double sideC)
{
	static const std::string ERROR_MESSAGE = "Side values must be all greater than 0";
	if (sideA < 0 || sideB < 0 || sideC < 0)
		throw std::invalid_argument(ERROR_MESSAGE);
	
	this->sideA = sideA;
	this->sideB = sideB;
	this->sideC = sideC;
	perimeter = CalculatePerimeter();
}

Triangle* Triangle::clone() const
{
	return new Triangle(*this);
}

const std::string Triangle::toString() const
{
	std::stringstream builder;
	builder << "triangle " << sideA << ' ' << sideB << ' ' << sideC;
	return builder.str();
}

double Triangle::CalculatePerimeter() const
{
	return sideA + sideB + sideC;
}