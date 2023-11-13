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

std::unique_ptr<Figure> Triangle::clone() const
{
	//return std::make_unique<Triangle>(*this);
	std::unique_ptr<Triangle> ptr = std::make_unique<Triangle>(*this);
	return std::move(ptr); // it broke, maybe this correction of the above code will fix it
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