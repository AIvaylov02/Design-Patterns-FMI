#include "HelperFunctions.h"

void FillStreamIntoStrings(std::stringstream& stream, std::vector<std::string>& arr)
{
	std::string current;
	while (stream >> current)
		arr.push_back(current);
}
std::vector<double> ExtractValues(const std::vector<std::string>& arr)
{
	std::vector<double> values;
	for (auto i = arr.begin() + 1; i < arr.end(); i++)
		values.push_back(std::stod(*i));
	return values;
}

const std::string ValidateFileNameUntilItsCorrect()
{
	std::string fileName;
	std::cin.ignore(); // reset cin for the getline intakes incoming
	while (true)
	{
		std::cout << "Please enter the file's name, together with its extensions. It should follow the format <fileName>.<extension>\n";
		std::cout << "Note that dots in the file's name are considered forbidden.\n";
		std::getline(std::cin, fileName);
		// check if the fileName is in the supported format. Only one dot is allowed		
		if (fileName.find('.') != -1 && fileName.find('.') == fileName.rfind('.'))
			break;
	}
	return fileName;
}

int GenerateRandomLength()
{
	static const int MAX_LENGTH_ALLOWED = 99;
	int randResult = std::rand() % MAX_LENGTH_ALLOWED;
	return randResult + 1; // we don't want 0 to be returnable so we will offset it by one
}