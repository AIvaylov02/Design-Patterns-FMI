#pragma once
#include <string>
#include <fstream>
#include <exception>

class OutputFileContainer
{
public:
	OutputFileContainer(const std::string& fileName, char mode='t');
	// WARNING - you cannot open the same file twice, this should lead to an error. This is why copy constructor and op= are forbidden
	OutputFileContainer(const OutputFileContainer& other) = delete;
	OutputFileContainer& operator=(const OutputFileContainer& other) = delete;
	~OutputFileContainer();

	void GetToFileStart();
	void GetToFileEnd();
	void WriteLine(const std::string& line);
	void WriteLine(int num);
	void WriteWord(const std::string& word);
private:
	char mode;
	std::string fileName;
	std::ofstream file;
};