#include "Triangle.h"

Triangle::Triangle(double sideA, double sideB, double sideC)
{
	this->sideA = sideA;
	this->sideB = sideB;
	this->sideC = sideC;
	ValidateTriangle();
	perimeter = CalculatePerimeter();
}

std::unique_ptr<Figure> Triangle::clone() const
{
	return std::make_unique<Triangle>(*this);
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

void Triangle::ValidateTriangle() const
{
	static const std::string NEGATIVE_SIDES_MESSAGE = "Side values must be all greater than 0";
	if (sideA < 0 || sideB < 0 || sideC < 0)
		throw std::invalid_argument(NEGATIVE_SIDES_MESSAGE);
	
	static const std::string SUM_INVALID_MESSAGE = "Sum of lengths of any 2 sides must be greater that the length of the third side!";
	if (sideA >= sideB + sideC || sideB >= sideA + sideC || sideC >= sideA + sideB)
			throw std::invalid_argument(SUM_INVALID_MESSAGE);
}