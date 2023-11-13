#include "InputFileContainer.h"
InputFileContainer::InputFileContainer(const std::string& fileName) : fileName(fileName)
{
	file.open(fileName);
	if (!file.is_open())
		throw std::exception("Error while opening the file. Operation nullified!");
}

InputFileContainer::~InputFileContainer()
{
	file.close();
}

void InputFileContainer::GetToFileStart()
{
	// no reason to check for state validaty as for our object to exits it needs to be valid
	file.seekg(0, std::ios_base::beg);
}
void InputFileContainer::GetToFileEnd()
{
	file.seekg(0, std::ios_base::end);
}

const std::string& InputFileContainer::ReadLine()
{
	if (file.eof())
		throw std::exception("The file has already been read");
	std::string line;
	std::getline(file, line);
	return line;
}
const std::string& InputFileContainer::ReadWord()
{
	if (file.eof())
		throw std::exception("The file has already been read");
	std::string line;
	file >> line;
	return line;
}