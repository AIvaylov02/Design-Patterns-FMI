#include <iostream>
#include "Factories/STDINFigureFactory.h"
#include "FileContainers/OutputFileContainer.h"
#include "FileContainers/InputFileContainer.h"
#include "Factories/RandomFigureFactory.h"
#include "Factories/FileFigureFactory.h"

void ValidateFactoryChoice(int& choice, int lowerBound, int upperBound);
std::vector<std::unique_ptr<Figure>> InitialPrompt();
void PrintCollectionOperationHints();
void PrintFiguresOnConsole(const std::vector<std::unique_ptr<Figure>>& figures);
bool ValidateIndexForCollectionManipulation(int lowerLimit, int upperLimit, const std::string entryMessage);
void DeleteFigure(std::vector<std::unique_ptr<Figure>>& figures);
void DisplayFigureAdditionMessage();
// Inserts the figure at the desired location. Note that it adds an extra element and does not replace an existing one if such exists at that spot
void AddFigureToList(std::vector<std::unique_ptr<Figure>>& figures, std::unique_ptr<Figure> figure, int index);
void DuplicateFigure(std::vector<std::unique_ptr<Figure>>& figures);
void SaveCollectionToFile(const std::vector<std::unique_ptr<Figure>>& figures);
void PerformCollectionOperations(std::vector<std::unique_ptr<Figure>>& figures);


int main() {	
	std::vector<std::unique_ptr<Figure>> figures = InitialPrompt();
	PerformCollectionOperations(figures);
}



void ValidateFactoryChoice(int& choice, int lowerBound, int upperBound)
{
	static const int DEFAULT_INVALID_CHOICE = -1;
	while (choice == DEFAULT_INVALID_CHOICE)
	{
		std::cin >> choice;
		if (choice < lowerBound || choice > upperBound)
		{
			// nullify the input and try for a reprompt
			choice = DEFAULT_INVALID_CHOICE;
			std::cout << "Please enter a valid choice value between [" << lowerBound << ", " << upperBound << "]\n";
		}
	}
}

std::vector<std::unique_ptr<Figure>> InitialPrompt()
{
	std::cout << "Greetings. How would you like to create a list of figures. Here are the valid options\n";
	std::cout << "Press 1) to choose random figures generation\n";
	std::cout << "Press 2) to choose to manually input figures from the console\n";
	std::cout << "Press 3) to choose figure creation by specifying an input file\n";

	enum creationChoice
	{
		random = 1,
		consoleInput,
		fileInput
	};

	
	int choice = -1;
	ValidateFactoryChoice(choice, creationChoice::random, creationChoice::fileInput);

	std::unique_ptr<FigureFactory> factory;
	std::vector<std::unique_ptr<Figure>> figures;
	int figuresToGenerate = 0;
	if (choice == creationChoice::fileInput)
	{
		factory = std::make_unique<FileFigureFactory>();
		// we need to read the lines count. We cannot break the createFigure signature and use it to return a whole collection. We need to use dynamic cast
		// which will limit us to using rawPtr but this way we have access to the new method of readFiguresCount;
		FileFigureFactory* rawPtr = dynamic_cast<FileFigureFactory*>(factory.release());
		int figuresToGenerate = rawPtr->ReadFiguresCount();
		for (int i = 0; i < figuresToGenerate; i++)
		{
			try 
			{
				figures.push_back(rawPtr->createFigure());
			}
			catch (std::invalid_argument)
			{
				std::cout << "Current figure was not created properly. Bear in mind that the collection will have a figure less!";
			}
		}
			
		delete rawPtr;
	}
	else // we have to receive figures' count from console
	{
		std::cout << "Please enter your desired count of figures to generate: ";
		std::cin >> figuresToGenerate;
		while (figuresToGenerate <= 0)
		{
			std::cout << "Please enter a positive integer!\n";
			std::cout << "Figures to generate: ";
			std::cin >> figuresToGenerate;
		}

		if (choice == creationChoice::consoleInput)
			factory = std::make_unique<STDINFigureFactory>();
		else
			factory = std::make_unique<RandomFigureFactory>();
			
		// clear the cin buffer before entering figure creation to have getline be waited by the function
		std::cin.ignore();
		for (int i = 0; i < figuresToGenerate; i++)
		{
			// if a figure fails to create, the user can manipulate the input or the programme can generate another one
			try
			{
				figures.push_back(factory->createFigure());
			}
			catch (std::invalid_argument)
			{
				std::cout << "Figure was not created successfully. Don't worry you will be prompted/the programme will generate a new one!\n";
				i--;
			}
		}

	}
	
	return figures;
}

void PrintCollectionOperationHints()
{
	std::cout << "What would you like to do with the accumulated figures? Here are the valid options:\n";
	std::cout << "Press 0) to exit the programme.\n";
	std::cout << "Press 1) to print this help dialog again.\n";
	std::cout << "Press 2) to print all figures in the collection on the console.\n";
	std::cout << "Press 3) to delete a figure from the collection.\n";
	std::cout << "Press 4) to duplicate a figure in the collection.\n";
	std::cout << "Press 5) to save the collection to an external file.\n";
}

void PrintFiguresOnConsole(const std::vector<std::unique_ptr<Figure>>& figures)
{
	for (auto iterToElement = figures.begin(); iterToElement != figures.end(); iterToElement++)
		std::cout << (*iterToElement)->toString() << '\n';
}

bool ValidateIndexForCollectionManipulation(int lowerLimit, int upperLimit, const std::string entryMessage)
{
	int index = -1;
	do
	{
		std::cout << entryMessage << " You can choose a number between :";
		std::cout << "[" << lowerLimit << ", " << upperLimit << "] \n";
		std::cin >> index;
	} while (index < 0 || index > upperLimit);
	return index;
}

void DeleteFigure(std::vector<std::unique_ptr<Figure>>& figures)
{
	if (figures.empty())
	{
		std::cout << "The collection is empty. You cannot delete from it. Please enter a new collection to use this operation.\n";
		return;
	}

	int index = ValidateIndexForCollectionManipulation(0, figures.end() - figures.begin() - 1, "Please enter which figure to delete by specifying its index.");
	figures.erase(figures.begin() + index); // remove the smart ptr(who will delete his own data) from the vector
}

void DisplayFigureAdditionMessage()
{
	std::cout << "Where do you want to put the newly cloned figure? Choose one of the following keys\n";
	std::cout << "e) To put it at the end of the collection\n";
	std::cout << "a) To put it right after the cloned object\n";
	std::cout << "n) To enter a number that will be used as 'resting' place for this newly created figure\n";
}

// Inserts the figure at the desired location. Note that it adds an extra element and does not replace an existing one if such exists at that spot
void AddFigureToList(std::vector<std::unique_ptr<Figure>>& figures, std::unique_ptr<Figure> figure, int index)
{
	DisplayFigureAdditionMessage();
	enum inputPlace
	{
		end = 'e',
		after = 'a',
		selected_index = 'n'
	};

	char ch = 0;
	while (true)
	{
		std::cin >> ch;
		switch (ch)
		{
		case inputPlace::end:
			figures.push_back(std::move(figure));
			return;

		case inputPlace::selected_index:
			index = ValidateIndexForCollectionManipulation(0, figures.end() - figures.begin() - 1, "Please enter on which index to put the newly cloned figure.");
		case inputPlace::after:
			figures.insert(figures.begin() + index, std::move(figure));
			return;

		default: // invalidInput
			std::cout << "Please constrain yourself to the given options!\n";
			DisplayFigureAdditionMessage();
		}
	}

}

void DuplicateFigure(std::vector<std::unique_ptr<Figure>>& figures)
{
	if (figures.empty())
	{
		std::cout << "The collection is empty. You cannot clone figures from it. Please enter a new collection to use this operation.\n";
		return;
	}
	//Generate figure duplicate
	int index = ValidateIndexForCollectionManipulation(0, figures.end() - figures.begin() - 1, "Please enter which figure to clone by specifying its index.");
	std::unique_ptr<Figure> figure = figures[index]->clone();

	AddFigureToList(figures, std::move(figure), index);
}



void SaveCollectionToFile(const std::vector<std::unique_ptr<Figure>>& figures)
{
	const std::string fileName = ValidateFileNameUntilItsCorrect();
	std::cout << "Enter a mode please for writing to the file. By default the file will be overwritten. If you don't want that press 't'\n";
	char mode = 'a';
	std::cin >> mode;
	OutputFileContainer outputFile(fileName, mode);
	outputFile.WriteLine(figures.size());
	for (auto iterToElement = figures.begin(); iterToElement < figures.end(); iterToElement++)
		outputFile.WriteLine((*iterToElement)->toString());
}

void PerformCollectionOperations(std::vector<std::unique_ptr<Figure>>& figures)
{
	enum availableActions
	{
		exit,
		printHelpInfo,
		printFigures,
		deleteFigure,
		duplicateFigure,
		saveToFile
	};

	int choice;
	PrintCollectionOperationHints();
	do
	{
		std::cin >> choice;
		switch (choice)
		{
		case exit:
			break;
		case printHelpInfo:
			PrintCollectionOperationHints();
			break;
		case printFigures:
			PrintFiguresOnConsole(figures);
			std::cout << "Done. Please select the next operation\n";
			break;
		case deleteFigure:
			DeleteFigure(figures);
			std::cout << "Done. Please select the next operation\n";
			break;
		case duplicateFigure:
			DuplicateFigure(figures);
			std::cout << "Done. Please select the next operation\n";
			break;
		case saveToFile:
			// Add a file wrapper class which will manage the resources of the file. Then use this implementation in the fileFactory and here
			SaveCollectionToFile(figures);
			PrintCollectionOperationHints();
			break;
		default: // invalid input entered
			std::cout << "Please enter a valid choice value between [" << availableActions::exit << ", " << availableActions::saveToFile << "]\n";
			std::cout << "If you need help, then press 1) to receive additional help information or 0) to exit the programme if you are done\n";
		}
	} while (choice != exit);
}