#pragma once
#include <string>
#include <fstream>
#include <exception>

class InputFileContainer
{
public:
	InputFileContainer(const std::string& fileName);
	// WARNING - you cannot open the same file twice, this should lead to an error. This is why copy constructor and op= are forbidden
	InputFileContainer(const InputFileContainer& other) = delete;
	InputFileContainer& operator=(const InputFileContainer& other) = delete;
	~InputFileContainer();

	void GetToFileStart();
	void GetToFileEnd();
	const std::string& ReadLine();
	const std::string& ReadWord();
private:
	std::string fileName;
	std::ifstream file;
};