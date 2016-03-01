#include "Unit.h"

#include "Helpers.h"
#include "GAScreen.h"
#include "Bullet.h"

Unit::Unit( GAScreen* world, int id, sf::Texture & texture ) : Id(id), Dead( false ), World(world)  {
	Sprite.setTexture( texture );
}

void Unit::Set( float health, float speed, float firerate ) {
	MaxHealth = health;
	Health = MaxHealth;
	Size = 0.1 + Health * 0.005f;
	Sprite.SetSize( Size, Size, true );

	Speed = speed;
	Strength = Speed * 0.5f;

	Firerate = firerate;
	FireError = Firerate * 5;

	Sprite.setPosition( 8 + cos( Id )*5.f, 5 + sin( Id )*5.f );
	Wins = 0;
	TotalTimeAlive = 0;
}

void Unit::Update( const Unit* enemy, float delta ) {
	//Sprite.setPosition(Sprite.getPosition() + sf::Vector2f(1* delta,0));
	
	Sprite.setPosition( Sprite.getPosition() + (Velocity*delta) );

	if ( FirerateClock.getElapsedTime().asSeconds() >= (1 / Firerate) ) {
		FirerateClock.restart();
		sf::Vector2f dir = enemy->GetSprite().getPosition() - Sprite.getPosition();
		
		World->SpawnBullet( *this, Normalize(dir), BULLET_SPEED, 0, Strength );
	}
}

void Unit::UpdateAI( float delta ) {
}

void Unit::UpdateMovement( float delta ) {
}

void Unit::Damage( Bullet * bullet ) {
	Health -= bullet->GetStrength();
	if ( Health <= 0 )
		Dead = true;
}

void Unit::Reset() {
	Health = MaxHealth;
	Dead = false;
}

void Unit::Won() {
	++Wins;
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

