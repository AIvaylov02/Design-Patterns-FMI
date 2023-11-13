#include <iostream>
#include "FigureFactory.h"
#include "InputFileContainer.h"
#include "OutputFileContainer.h"

std::vector<std::unique_ptr<Figure>> ExecuteRandomChoice()
{
	std::cout << "Please enter your desired count of figures to generate: ";
	int figuresToGenerate = 0;
	std::cin >> figuresToGenerate;
	while (figuresToGenerate <= 0)
	{
		std::cout << "Please enter a positive integer!\n";
		std::cout << "Figures to generate: ";
		std::cin >> figuresToGenerate;
	}

	std::vector<std::unique_ptr<Figure>> figures(figuresToGenerate);
	for (int i = 0; i < figuresToGenerate; i++)
	{
		//figures[i] = RandomFigureFactory::createFigure
	}
	return figures;
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

	const int DEFAULT_INVALID_CHOICE = -1;
	int choice = DEFAULT_INVALID_CHOICE;
	while (choice == DEFAULT_INVALID_CHOICE)
	{
		std::cin >> choice;
		switch (choice)
		{
			case creationChoice::random:
				std::cout << "Rand";
				// TODO
				return ExecuteRandomChoice();

			case creationChoice::consoleInput:
				// TODO
				std::cout << "Console";
				break;

			case creationChoice::fileInput:
				// TODO
				std::cout << "File";
				break;

			default:
				// nullify the input and try for a reprompt
				choice = DEFAULT_INVALID_CHOICE;
				std::cout << "Please enter a valid choice value between [" << creationChoice::random << ", " << creationChoice::fileInput << "]\n";
				break;
		}
	}
	
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
		std::cin >> index; // begin e 0, za 7 elementa end e 8
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
				figures.push_back(figure);
				return;

			case inputPlace::selected_index:
				index = ValidateIndexForCollectionManipulation(0, figures.end() - figures.begin() - 1, "Please enter on which index to put the newly cloned figure.");
			case inputPlace::after:
				figures.insert(figures.begin() + index, figure);
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
	std::unique_ptr<Figure> figure = std::move(figures[index]->clone());

	AddFigureToList(figures, figure, index);
}

const std::string ValidateFileNameUntilItsCorrect()
{
	std::string fileName;
	while (true)
	{
		std::cout << "Please enter the file's name, together with its extensions. It should follow the format <fileName>.<extension>\n";
		std::cout << "Note that dots in the file's name are considered forbidden.\n";
		std::getline(std::cin, fileName);
		// check if the fileName is in the supported format. Only one dot is allowed
		if (fileName.find('.') != -1 && fileName.find('.') != fileName.rfind('.'))
			break;
	}
	return fileName;
}

void SaveCollectionToFile(const std::vector<std::unique_ptr<Figure>>& figures)
{
	std::cout << "Please enter the file's name, together with its extensions. It should follow the format <fileName>.<extension>\n";
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
				break;
			case deleteFigure:
				DeleteFigure(figures);
				break;
			case duplicateFigure:
				DuplicateFigure(figures);
				break;
			case saveToFile:
				// Add a file wrapper class which will manage the resources of the file. Then use this implementation in the fileFactory and here
				SaveCollectionToFile(figures);
				break;
			default: // invalid input entered
				std::cout << "Please enter a valid choice value between [" << availableActions::exit << ", " << availableActions::saveToFile << "]\n";
				std::cout << "If you need help, then press 1) to receive additional help information or 0) to exit the programme if you are done\n";
		}
	} while (choice != exit);
}

int main() {
	/*Triangle tr(10, 20, 30);
	Circle cl(5);
	Rectangle rec(2, 5);
	std::cout << tr.toString() << " 's perimeter is: " << tr.getPerimeter()
		<< '\n' << cl.toString() << " 's perimeter is: " << cl.getPerimeter()
		<< '\n' << rec.toString() << " 's perimeter is: " << rec.getPerimeter();*/
	
		/*std::string input("rectangle 5 2");
	std::stringstream splitInput;
	splitInput << input;
	FigureFactory::createFigure(splitInput);*/
	std::vector<std::unique_ptr<Figure>> figures = InitialPrompt();
	// we have a valid collection now, lets do some operations on them
	PerformCollectionOperations(figures);
}