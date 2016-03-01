#pragma once

#include <RSprite.h>
#include "Unit.h"

class Bullet {
public:
	Bullet(Unit* owner, sf::Texture texture, sf::Vector2f direction, float speed, float strength);

	void Update( float delta );

	float GetStrength() const;

	Unit* GetOwner() const;

	const RSprite& GetSprite() const;

private:

	Unit*		Owner;

	RSprite			Sprite;

	sf::Vector2f	Direction;

	float			Speed;

	float			Strength;
};

