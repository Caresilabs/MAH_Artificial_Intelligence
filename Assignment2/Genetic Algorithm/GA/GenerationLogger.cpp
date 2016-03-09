#include "GenerationLogger.h"
#include <iostream>
#include <fstream>

void GenerationLogger::Log( const std::vector<Unit*>& Generation ) {
	std::ofstream File;

	File.open( "Logs/Median.txt", std::ios_base::app );
	File << std::to_string( Generation[Generation.size() / 2]->FitnessFunction() ) << std::endl;
	File.close();

	File.open( "Logs/Average.txt", std::ios_base::app );
	float Average = 0;
	{
		for each (auto Unit in Generation) {
			Average += Unit->FitnessFunction();
		}
		Average /= Generation.size();
	}
	File << std::to_string( Average ) << std::endl;
	File.close();

	File.open( "Logs/Best.txt", std::ios_base::app );
	File << std::to_string( Generation[0]->FitnessFunction() ) << std::endl;
	File.close();

	File.open( "Logs/Worst.txt", std::ios_base::app );
	File << std::to_string( Generation[Generation.size() - 1]->FitnessFunction() ) << std::endl;
	File.close();

	File.open( "Logs/StatsBest.txt", std::ios_base::app );
	File << "Health: " << std::to_string( Generation[0]->GetMaxHealth() ) << ", Speed: " << std::to_string( Generation[0]->GetSpeed() ) << ", Firerate: " << std::to_string( Generation[0]->GetFirerate() ) << std::endl;
	File.close();
}
