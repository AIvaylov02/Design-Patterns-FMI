#include "OutputFileContainer.h"
OutputFileContainer::OutputFileContainer(const std::string& fileName, char mode) : fileName(fileName)
{
	this->mode = mode;
	if (mode == 't') // t from trunc
		file.open(fileName);
	else
		file.open(fileName, std::ios::app);
	
	if (!file.is_open())
		throw std::exception("Error while opening the file. Operation nullified!");
}

OutputFileContainer::~OutputFileContainer()
{
	file.close();
}

void OutputFileContainer::GetToFileStart()
{
	// no reason to check for state validaty as for our object to exits it needs to be valid
	file.seekp(0, std::ios_base::beg);
}
void OutputFileContainer::GetToFileEnd()
{
	file.seekp(0, std::ios_base::end);
}

void OutputFileContainer::WriteLine(const std::string& line)
{
	file << line;
}

void OutputFileContainer::WriteLine(int num)
{
	file << num;
}

void OutputFileContainer::WriteWord(const std::string& word)
{
	file << word;
}