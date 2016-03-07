#include "GenerationLogger.h"
#include <iostream>
#include <fstream>

void GenerationLogger::Log( const std::vector<Unit*>& generation ) {
	std::ofstream file;
	file.open( "average.txt", std::ios_base::app );
	file << std::to_string(1);
	file.close();

	file.open( "best.txt", std::ios_base::app );
	file << std::to_string( 1 );
	file.close();

	file.open( "worst.txt", std::ios_base::app );
	file << std::to_string( 1 );
	file.close();
}
