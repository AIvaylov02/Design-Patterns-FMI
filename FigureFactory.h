#pragma once
#include "Triangle.h"
#include "Circle.h"
#include "Rectangle.h"
// #include <unordered_map>
#include <unordered_set>

class FigureFactory
{
protected:
	static std::unique_ptr<Figure> createFigure(std::stringstream& input);
private:
	// bool isValidFigureType(const std::string& str, const std::unordered_map<std::string, constructor_func>& collection) const;
	static void ValidateFigureType(const std::string& str);
};