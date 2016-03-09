#pragma once

#include <RSprite.h>
#include "Unit.h"

class Bullet {
public:
							Bullet(Unit* owner, sf::Texture texture, sf::Vector2f direction, float speed, float strength);

	void					Update( float delta );
	
	void					SetColor( const sf::Color& color );

	float					GetStrength() const;

	Unit*					GetOwner() const;

	const RSprite&			GetSprite() const;

	const sf::Vector2f&		GetDirection() const;

private:

	Unit*					Owner;

	RSprite					Sprite;

	sf::Vector2f			Direction;

	float					Speed;

	float					Strength;
};

