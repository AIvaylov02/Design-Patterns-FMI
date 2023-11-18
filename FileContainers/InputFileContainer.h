#pragma once
#include <string>
#include <fstream>
#include <exception>
#include <iostream>

class InputFileContainer
{
public:
	InputFileContainer();
	InputFileContainer(const std::string& fileName);
	// WARNING - you cannot open the same file twice, this should lead to an error. This is why copy constructor and op= are forbidden
	InputFileContainer(const InputFileContainer& other) = delete;
	InputFileContainer& operator=(const InputFileContainer& other) = delete;
	~InputFileContainer();

	void LoadFile(const std::string& fileName);
	void GetToFileStart();
	void GetToFileEnd();
	std::string ReadLine();
	std::string ReadWord();
private:
	std::string fileName;
	std::ifstream file;
};