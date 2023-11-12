#include <iostream>
#include "FigureFactory.h"

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

void DeleteFigure(std::vector<std::unique_ptr<Figure>>& figures)
{
	if (figures.empty())
	{
		std::cout << "The collection is empty. You cannot delete from it. Please enter a new collection to use this operation.\n";
		return;
	}

	int index;
	std::cin >> index;
	DeleteFigureAtIndex(figures, index);
}

void DeleteFigureAtIndex(std::vector<std::unique_ptr<Figure>>& figures, int index)
{
	// Alternative would be to use try catch and disallow the deletion if invalid and exit. I want to stick the deletion for a possible retry of the user.
	do
	{
		std::cout << "Please enter which figure to delete by specifying its index. You can choose a number between:";
		std::cout << "[" << 0 << ", " << figures.end() - figures.begin() - 1 << "] \n";
		std::cin >> index; // begin e 0, za 7 elementa end e 8
	} while (index < 0 || index > figures.size());
	figures.erase(figures.begin() + index); // release the space allocated for the ptr in the vector
}

std::unique_ptr<Figure> GenerateFigureDuplicateFromIndex(const std::vector<std::unique_ptr<Figure>>& figures, int index)
{
	// Alternative would be to use try catch and disallow the cloning if invalid and exit. I want to stick the clone method for a possible retry of the user.
	do
	{
		std::cout << "Please enter which figure to clone by specifying its index. You can choose a number between:";
		std::cout << "[" << 0 << ", " << figures.end() - figures.begin() - 1 << "] \n";
		std::cin >> index; // begin e 0, za 7 elementa end e 8
	} while (index < 0 || index > figures.size());
	return figures[index]->clone();
}

std::unique_ptr<Figure> GenerateFigureDuplicate(const std::vector<std::unique_ptr<Figure>>& figures)
{
	if (figures.empty())
	{
		std::cout << "The collection is empty. You cannot clone from it. Please enter a new collection to use this operation.\n";
		return;
	}

	int index;
	std::cin >> index;
	return GenerateFigureDuplicateFromIndex(figures, index);
}

// Appends the figure to the end
void AddFigureToList(std::vector<std::unique_ptr<Figure>>& figures, std::unique_ptr<Figure> figure)
{
	figures.push_back(figure);
}

// Inserts the figure at the desired location. Note that it adds an extra element and does not replace an existing one
void AddFigureToList(std::vector<std::unique_ptr<Figure>>& figures, std::unique_ptr<Figure> figure, int index)
{
	figures.insert(figures.begin() + index, figure);
}

void DuplicateFigure()
{
	// TODO add UI and combine the above code
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
				// TODO
				DuplicateFigure();
				break;
			case saveToFile:
				// TODO. Add a file wrapper class which will manage the resources of the file. Then use this implementation in the fileFactory and here
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