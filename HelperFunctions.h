#pragma once
#include <vector>
#include <sstream>
#include <string>
#include <iostream>

void FillStreamIntoStrings(std::stringstream& stream, std::vector<std::string>& arr);
std::vector<double> ExtractValues(const std::vector<std::string>& arr);
const std::string ValidateFileNameUntilItsCorrect();
int GenerateRandomLength();