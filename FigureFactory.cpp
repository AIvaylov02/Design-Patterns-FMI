#include "FigureFactory.h"
#include "HelperFunctions.h"
#include <iostream>


void FigureFactory::ValidateFigureType(const std::string& str)
{
	static std::unordered_set<std::string> allowedFigures (
		{"rectangle", "circle", "triangle"}
	);
	static std::string MESSAGE = "No such figure is supported by the programme. Check your spelling. \n Available types are: 'rectangle', 'circle', 'triangle' ";
	if (allowedFigures.find(str) == allowedFigures.end())
		throw std::invalid_argument(MESSAGE);
}

Figure* FigureFactory::createFigure(std::stringstream& stream)
{
	std::vector<std::string> command;
	FillStreamIntoStrings(stream, command);
	
	/* static std::unordered_map<std::string> My Idea which would have been great was to create a hashmap of pairs<stringFigureType, function Corresponding constructor>
	 But the plan failed as I have no idea how to make the binding of the second argument to the hashmap. This way by making the code functional(as in JS), by
	 giving the code a figureType we can get it's exact constructor. Dynamically adding new figures would be easy as there are no more type restrictions */
	std::string typeToCreate = command[0];
	ValidateFigureType(typeToCreate);

	// note: c++ doesn't support switch with string as comparable object 
	std::vector<double> values = ExtractValues(command);
	if (typeToCreate == "circle")
		return new Circle(values[0]);
	else if (typeToCreate == "rectangle")
		return new Rectangle(values[0], values[1]);
	else // triangle
		return new Triangle(values[0], values[1], values[2]);
}
