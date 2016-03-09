#pragma once

#include <SFML/System/Vector2.hpp>

sf::Vector2f		Normalize( const sf::Vector2f& source );

float				Length( const sf::Vector2f& source );

float				Dot( const sf::Vector2f& a, const sf::Vector2f& b );

float				Det( const sf::Vector2f& a, const sf::Vector2f& b );

float				Clamp( float n, float lower, float upper );

float				NormalizeAngle( float x );

int					GetRandomNumber( int min, int max, bool seed = false );

float				GetRandomNumber( float min, float max, bool seed = false );

int					Sign( float x );