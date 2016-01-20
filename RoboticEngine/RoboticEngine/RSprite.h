#pragma once
#include <SFML/Graphics.hpp>

class RSprite :
	public sf::Sprite {
public:
	RSprite() = default;

	void SetSize( float width, float height, bool center = true );

	virtual ~RSprite();
};

