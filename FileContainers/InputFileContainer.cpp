#include "InputFileContainer.h"
InputFileContainer::InputFileContainer()
{
	fileName = "";
}

InputFileContainer::InputFileContainer(const std::string& fileName) : fileName(fileName)
{
	file.open(fileName);
	if (!file.is_open())
		throw std::exception("Error while opening the file. Operation nullified!");
}

InputFileContainer::~InputFileContainer()
{
	if (fileName != "") // a file has been loaded
		file.close();
}

void InputFileContainer::LoadFile(const std::string& fileName)
{
	// we have already loaded a file
	static const std::string FILE_ALREADY_LOADED = "A file has already been loaded in this object! Please create a different wrapper for the new one";
	if (this->fileName != "")
		throw std::logic_error(FILE_ALREADY_LOADED);

	file.open(fileName);
	if (file.is_open())
		this->fileName = fileName;
	else
		std::cout << "Error while opening the file. This container is still empty!";

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

std::string InputFileContainer::ReadLine()
{
	if (file.eof())
		throw std::exception("The file has already been read");
	std::string line;
	std::getline(file, line);
	return line;
}
std::string InputFileContainer::ReadWord()
{
	if (file.eof())
		throw std::exception("The file has already been read");
	std::string line;
	file >> line;
	return line;
}