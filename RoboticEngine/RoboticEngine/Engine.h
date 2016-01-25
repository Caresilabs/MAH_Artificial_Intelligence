#pragma once

#include <map>

#include "LuaScript.h"
#include "Entity.h"
#include "SFML/Graphics.hpp"

class Engine {
public:
	Engine();

	void Add( Entity* entity );

	void Add( const char* fileName );

	void Update( float delta );

	void Draw( sf::RenderWindow* window );

	virtual ~Engine();

private:
	std::map<int, Entity*> entities;
};

