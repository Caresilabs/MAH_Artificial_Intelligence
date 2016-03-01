#pragma once

#include <SFML/System/Vector2.hpp>
#include <math.h>

sf::Vector2f Normalize( const sf::Vector2f& source ) {
	float length = sqrt( (source.x * source.x) + (source.y * source.y) );
	if ( length != 0 )
		return sf::Vector2f( source.x / length, source.y / length );
	else
		return source;
}