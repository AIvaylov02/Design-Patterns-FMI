#include "Circle.h"

Circle::Circle(double radius)
{
	static const std::string ERROR_MESSAGE = "Radius value must be greater than 0";
	if (radius < 0)
		throw std::invalid_argument(ERROR_MESSAGE);

	this->radius = radius;
	perimeter = CalculatePerimeter();
}

std::unique_ptr<Figure> Circle::clone() const
{
	//return std::make_unique<Circle>(*this);
	std::unique_ptr<Circle> ptr = std::make_unique<Circle>(*this);
	return std::move(ptr); // it broke, maybe this correction of the above code will fix it
}

const std::string Circle::toString() const
{
	std::stringstream builder;
	builder << "circle " << radius;
	return builder.str();
}

double Circle::CalculatePerimeter() const
{
	static const double PI = 3.1416;
	return 2 * radius * PI;
}