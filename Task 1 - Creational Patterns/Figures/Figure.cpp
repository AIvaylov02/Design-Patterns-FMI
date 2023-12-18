#include "Figure.h"
Figure::Figure() 
{
	/* Maybe better alternative would be to remove the
	cpp file and have the compiler set the value in the class Figure() : perimeter(-1) */ 
	static const double INVALID_DEFAULT_VALUE = -1;
	perimeter = INVALID_DEFAULT_VALUE;
}

double Figure::getPerimeter() const 
{
	return perimeter;
}