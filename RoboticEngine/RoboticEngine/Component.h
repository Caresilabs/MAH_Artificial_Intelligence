#pragma once
#include "LuaScript.h"
#include "SFML/Graphics.hpp"

class Component {
public:
	Component() = default;

	Component(LuaScript* table);

	virtual void Update( float delta ) = 0;

	virtual void Draw(sf::RenderWindow* window) = 0;

	virtual ~Component();
};

