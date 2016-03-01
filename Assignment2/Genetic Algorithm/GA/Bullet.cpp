#include "Bullet.h"

Bullet::Bullet( Unit* owner, sf::Texture texture, sf::Vector2f direction, float speed, float strength )
	: Owner( owner ), Direction( direction ), Speed( speed ), Strength( strength ) {
	Sprite.setTexture( texture );
	Sprite.setPosition( owner->GetSprite().getPosition() );
	Sprite.SetSize( .2f, 0.2f, true );
}

void Bullet::Update( float delta ) {
	Sprite.setPosition( Sprite.getPosition() + (Direction*Speed*delta) );
}

float Bullet::GetStrength() const {
	return Strength;
}

Unit* Bullet::GetOwner() const {
	return Owner;
}

const RSprite & Bullet::GetSprite() const {
	return Sprite;
}
