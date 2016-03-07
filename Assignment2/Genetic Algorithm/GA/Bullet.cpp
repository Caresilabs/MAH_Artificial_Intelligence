#include "Bullet.h"

Bullet::Bullet( Unit* owner, sf::Texture texture, sf::Vector2f direction, float speed, float strength )
	: Owner( owner ), Direction( direction ), Speed( speed ), Strength( strength ) {
	Sprite.setTexture( texture );
	Sprite.setPosition( owner->GetSprite().getPosition() );
	Sprite.SetSize( .1f, 0.1f, true );
}

void Bullet::Update( float delta ) {
	Sprite.setPosition( Sprite.getPosition() + (Direction*Speed*delta) );
}

void Bullet::SetColor( const sf::Color & color ) {
	Sprite.setColor( color );
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

const sf::Vector2f & Bullet::GetDirection() const {
	return Direction;
}
