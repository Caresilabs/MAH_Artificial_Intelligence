#include "Unit.h"

#include "GAScreen.h"

Unit::Unit( GAScreen* world, int id, sf::Texture & texture ) : Id(id), Dead( false ), World(world)  {
	Sprite.setTexture( texture );
}

void Unit::Set( float health, float speed, float firerate ) {
	MaxHealth = health;
	Health = MaxHealth;
	Size = Health;
	Sprite.SetSize( Size, Size, true );

	Sprite.setPosition(cos(Id)*8.f,sin(Id)*8.f);

	Speed = speed;
	Strength = Speed * 0.5f;

	Firerate = firerate;
	FireError = Firerate * 5;

	AliveClock.restart();
}

void Unit::Update( const Unit* enemy, float delta ) {
	//Sprite.setPosition(Sprite.getPosition() + sf::Vector2f(1* delta,0));
	


}

void Unit::UpdateAI( float delta ) {
}

void Unit::UpdateMovement( float delta ) {
}

void Unit::Reset() {
	Health = MaxHealth;
	Dead = false;
	SessionTime = 0;
}

float Unit::FitnessFunction() const {
	return 0.0f;
}

int Unit::GetId() const {
	return Id;
}

bool Unit::IsDead() const {
	return Dead;
}

const RSprite & Unit::GetSprite() const {
	return Sprite;
}

