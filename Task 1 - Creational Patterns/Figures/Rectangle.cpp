#include "Rectangle.h"

Rectangle::Rectangle(double sideA, double sideB)
{
	static const std::string ERROR_MESSAGE = "Side values must be all greater than 0";
	if (sideA < 0 || sideB < 0)
		throw std::invalid_argument(ERROR_MESSAGE);

	this->sideA = sideA;
	this->sideB = sideB;
	perimeter = CalculatePerimeter();
}

std::unique_ptr<Figure> Rectangle::clone() const
{
	return std::make_unique<Rectangle>(*this);
}

const std::string Rectangle::toString() const
{
	std::stringstream builder;
	builder << "rectangle " << sideA << ' ' << sideB;
	return builder.str();
}

double Rectangle::CalculatePerimeter() const
{
	return 2 * (sideA + sideB);
}