#pragma once
#include <string>
#include <sstream>
#include <stdexcept>

class Figure 
{
public:
	Figure();
	virtual std::unique_ptr<Figure> clone() const = 0;
	virtual ~Figure() = default;

	virtual const std::string toString() const = 0;
	/* Figures aren't mutable, so we should store the perimeter rather
	than calculate it each time. Question resides whether or not Figure should have a perimeter attribute 
	and if getPerimeter should be implemented. Risking invalid state vs code reusability  */
	double getPerimeter() const;

protected:
	double perimeter;
	virtual double CalculatePerimeter() const = 0;
};