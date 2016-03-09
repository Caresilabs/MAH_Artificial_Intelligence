#pragma once
#include <vector>
#include "Unit.h"

class GenerationLogger {
public:
	
					GenerationLogger() = default;

	void			Log( const std::vector<Unit*>& Generation );

};

