#include "Factories/STDINFigureFactory.h"

std::unique_ptr<Figure> STDINFigureFactory::createFigure()
{
	// read a line, then create a Figure
	std::string line;
	std::cout << "Please enter the figure's credentials. That means follow the conventions for the listed figures below: \n";
	std::cout << "To create a Circle, type \"circle <radiusOfCircle>\" \n";
	std::cout << "To create a Rectangle, type \"rectangle <sideA> <sideB>\" \n";
	std::cout << "To create a Triangle, type \"triangle <sideA> <sideB> <sideC>\" \n";
	std::getline(std::cin, line);
	std::stringstream splitInput;
	splitInput << line;
	std::vector<std::string> command;
	FillStreamIntoStrings(splitInput, command);
	//	
	//	/* static std::unordered_map<std::string> My Idea which would have been great was to create a hashmap of pairs<stringFigureType, function Corresponding constructor>
	//	 But the plan failed as I have no idea how to make the binding of the second argument to the hashmap. This way by making the code functional(as in JS), by
	//	 giving the code a figureType we can get it's exact constructor. Dynamically adding new figures would be easy as there are no more type restrictions */
	std::string typeToCreate = command[0];
	//ValidateFigureType(typeToCreate);
	// note: c++ doesn't support switch with string as comparable object 

	std::vector<double> values = ExtractValues(command);
	if (typeToCreate == "circle")
		return std::make_unique<Circle>(values[0]);
	else if (typeToCreate == "rectangle")
		return std::make_unique<Rectangle>(values[0], values[1]);
	else // triangle
		return std::make_unique<Triangle>(values[0], values[1], values[2]);
}