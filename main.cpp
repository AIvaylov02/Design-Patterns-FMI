#include <iostream>
#include "Triangle.h"
#include "Circle.h"
#include "Rectangle.h"

int main() {
	Triangle tr(10, 20, 30);
	Circle cl(5);
	Rectangle rec(2, 5);
	std::cout << tr.toString() << " 's perimeter is: " << tr.getPerimeter()
		<< '\n' << cl.toString() << " 's perimeter is: " << cl.getPerimeter()
		<< '\n' << rec.toString() << " 's perimeter is: " << rec.getPerimeter();
}