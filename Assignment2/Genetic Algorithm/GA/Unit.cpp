#include "Unit.h"
#include "GAScreen.h"
#include "Bullet.h"
#include "Helpers.h"

Unit::Unit( GAScreen* world, int id, sf::Texture & texture ) : Id( id ), Dead( false ), World( world ) {
	Sprite.setTexture( texture );
}

void Unit::Set( float health, float speed, float firerate ) {
	MaxHealth = health;
	Health = MaxHealth;
	Size = 0.05 + Health * 0.006f;
	Sprite.SetSize( Size, Size, true );

	Speed = speed;
	Strength = 1.f + 3 * (15.f / ((Speed*3.f) + 1));

	Firerate = firerate;
	FireError = (Firerate * Firerate) * 0.0174533 * 1.6f; // Error in Radians 

	Angle = Id;
	Wins = 0;
	TotalTimeAlive = 0;
	DamageDealt = 0;
}

void Unit::Update( const Unit* enemy, float delta ) {
	TotalTimeAlive += delta;

	Angle += Velocity * delta;

	Sprite.setPosition( 8 + cos( Angle )*5.f, 4.5f + sin( Angle )*3.5f );

	// Shoot
	if ( (FirerateTime += delta) >= (1 / Firerate) ) {
		// Reset clock
		FirerateTime = 0;

		float len = Length( (enemy->GetSprite().getPosition() - Sprite.getPosition()) );

		float EnemyAngle = enemy->Angle + enemy->Velocity * (len / BULLET_SPEED);

		sf::Vector2f Dir( 8 + cos( EnemyAngle )*5.f, 4.5f + sin( EnemyAngle )*3.5f );

		Dir -= Sprite.getPosition();

		World->SpawnBullet( *this, Normalize( Dir ), BULLET_SPEED, FireError, Strength );
	}

	// Dodge bullets
	float NewVelocity = Velocity;
	for each (auto bullet in World->GetBullets()) {
		if ( bullet->GetOwner() == this )
			continue;

		float len = Length( (bullet->GetSprite().getPosition() - Sprite.getPosition()) );

		float dot = Dot( bullet->GetDirection(), (Sprite.getPosition() - bullet->GetSprite().getPosition()) );
		float det = Det( bullet->GetDirection(), (Sprite.getPosition() - bullet->GetSprite().getPosition()) );

		// Angle between bullet direction and bulletposition-mypostion vector.
		float myangle = NormalizeAngle( atan2( det, dot ) );

		NewVelocity += (myangle * 20) / len;

	}
	//  Keep enemies apart from each other
	float len = Length( (enemy->GetSprite().getPosition() - Sprite.getPosition()) );
	//if ( len < 4 && abs( enemy->Velocity ) > 0.1f )
		NewVelocity -= 0.3f * ((Speed * 100 * Sign( NormalizeAngle( enemy->Angle - Angle ) )) / len);

	Velocity = NewVelocity;

	// Clamp Velocity
	Velocity = Clamp( Velocity, -Speed, Speed );

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

void Unit::AddDamageDealt( float Damage ) {
	DamageDealt += Damage;
}

void Unit::Reset() {
	Health = MaxHealth;
	Dead = false;
}

void Unit::Won() {
	++Wins;
}

void Unit::SetColor( const sf::Color & color ) {
	Sprite.setColor( color );
}

void Unit::ResetFitnessData() {
	TotalTimeAlive = 0;
	DamageDealt = 0;
	Wins = 0;
}

float Unit::FitnessFunction() const {
	return (TotalTimeAlive) + (DamageDealt * 0.01f) + (20 * Wins);
}

int Unit::GetId() const {
	return Id;
}

bool Unit::IsDead() const {
	return Dead;
}

float Unit::GetHealth() const {
	return Health;
}

float Unit::GetVelocity() const {
	return Velocity;
}

float Unit::GetAngle() const {
	return Angle;
}

const RSprite & Unit::GetSprite() const {
	return Sprite;
}

float Unit::GetMaxHealth() const {
	return MaxHealth;
}

float Unit::GetSpeed() const {
	return Speed;
}

float Unit::GetFirerate() const {
	return Firerate;
}

float Unit::GetFireError() const {
	return FireError;
}

float Unit::GetStrength() const {
	return Strength;
}

float Unit::GetSize() const {
	return Size;
}

