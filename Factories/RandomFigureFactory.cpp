#include "Factories/RandomFigureFactory.h"

std::unique_ptr<Figure> RandomFigureFactory::createFigure()
{
	int randomFigureType = std::rand() % 3; // returns a number between [0, 2]
	enum availableFigures
	{
		circle,
		rectangle,
		triangle
	};

	int valueOne = GenerateRandomLength();
	int valueTwo = GenerateRandomLength();
	int valueThree = GenerateRandomLength();

	switch (randomFigureType)
	{
		case availableFigures::circle:
			return std::make_unique<Circle>(valueOne);
		case availableFigures::rectangle:
			return std::make_unique<Rectangle>(valueOne, valueTwo);
		case availableFigures::triangle:
			return std::make_unique<Triangle>(valueOne, valueTwo, valueThree);
	}
}