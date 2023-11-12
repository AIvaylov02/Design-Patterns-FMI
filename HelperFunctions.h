#pragma once
#include <vector>
#include <sstream>
#include <string>

void FillStreamIntoStrings(std::stringstream& stream, std::vector<std::string>& arr)
{
	std::string current;
	while (stream >> current)
		arr.push_back(current);
}
std::vector<double> ExtractValues(const std::vector<std::string>& arr)
{
	std::vector<double> values;
	for (auto i = arr.begin(); i < arr.end(); i++)
		values.push_back(std::stod(*i));
	return values;
}