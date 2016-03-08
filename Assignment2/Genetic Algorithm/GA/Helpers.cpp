#define _USE_MATH_DEFINES

#include "Helpers.h"
#include <math.h>
#include <stdlib.h>
#include <time.h>
#include <algorithm>

sf::Vector2f Normalize( const sf::Vector2f& source ) {
	float length = sqrt( (source.x * source.x) + (source.y * source.y) );
	if ( length != 0 )
		return sf::Vector2f( source.x / length, source.y / length );
	else
		return source;
}

float Length( const sf::Vector2f& source ) {
	float slen = (source.x * source.x) + (source.y * source.y);
	return sqrtf( slen );
}

float Dot( const sf::Vector2f& a, const sf::Vector2f& b ) {
	return (a.x * b.x) + (a.y * b.y);
}

float Det( const sf::Vector2f& a, const sf::Vector2f& b ) {
	return (a.x * b.y) - (a.y * b.x);
}

float Clamp( float n, float lower, float upper ) {
	return std::max( lower, std::min( n, upper ) );
}

float NormalizeAngle( float x ) {
	x = fmod( x + M_PI, M_PI*2.f );
	if ( x < 0 )
		x += M_PI*2.f;
	return x - M_PI;
}

int GetRandomNumber( int min, int max, bool seed ) {
	int	number;

	if ( seed )
		srand( (unsigned)time( NULL ) );

	number = (((abs( rand() ) % (max - min + 1)) + min));

	if ( number > max )
		number = max;

	if ( number < min )
		number = min;

	return number;
}

float GetRandomNumber( float Min, float Max, bool Seed ) {
	if ( Seed )
		srand( (unsigned)time( NULL ) );

	return ((float( rand() ) / float( RAND_MAX )) * (Max - Min)) + Min;
}