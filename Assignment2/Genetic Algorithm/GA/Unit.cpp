#include "Unit.h"

#include "GAScreen.h"
#include "Bullet.h"
#include "Helpers.h"

Unit::Unit( GAScreen* world, int id, sf::Texture & texture ) : Id(id), Dead( false ), World(world)  {
	Sprite.setTexture( texture );
}

void Unit::Set( float health, float speed, float firerate ) {
	MaxHealth = health;
	Health = MaxHealth;
	Size = 0.1 + Health * 0.005f;
	Sprite.SetSize( Size, Size, true );

	Speed = speed;
	Strength = Speed * 5.f;

	Firerate = firerate;
	FireError = Firerate * 0.002f;

	//Sprite.setPosition( 8 + cos( Id )*5.f, 5 + sin( Id )*3.f );
	Angle = Id;
	Wins = 0;
	TotalTimeAlive = 0;
}

void Unit::Update( const Unit* enemy, float delta ) {
	//Sprite.setPosition(Sprite.getPosition() + sf::Vector2f(1* delta,0));

	// Clamp Velocity
	Velocity = Clamp( Velocity, -Speed, Speed );

	Angle += Velocity * delta;

	//Sprite.setPosition( Sprite.getPosition() + (Velocity*delta) );
	Sprite.setPosition( 8 + cos( Angle )*5.f, 4.5f + sin( Angle )*3.5f );

	if ( FirerateClock.getElapsedTime().asSeconds() >= (1 / Firerate) ) {
		FirerateClock.restart();

		//sf::Vector2f Dir = enemy->GetSprite().getPosition() - Sprite.getPosition();
		float len = Length( (enemy->GetSprite().getPosition() - Sprite.getPosition()) );

		// TODO approx shoot
		// s/v = t
		float EnemyAngle = enemy->Angle;// +(enemy->Velocity * (len / BULLET_SPEED));
		
		sf::Vector2f Dir( 8 + cos( EnemyAngle )*5.f, 4.5f + sin( EnemyAngle )*3.5f );

		Dir -= Sprite.getPosition();

		//float ShootAngle = NormalizeAngle( atan2( Dir.y, Dir.x ) + GetRandomNumber(-FireError, FireError));
		
		World->SpawnBullet( *this, Normalize(Dir), BULLET_SPEED, FireError, Strength );
	}

	// Dodge bullets
	float NewVelocity = Velocity;
	for each (auto bullet in World->GetBullets()) {
		if ( bullet->GetOwner() == this )
			continue;

		float len = Length((bullet->GetSprite().getPosition() - Sprite.getPosition()));

		float dot = Dot(bullet->GetDirection(), ( Sprite.getPosition() - bullet->GetSprite().getPosition() ) );
		float det = Det( bullet->GetDirection(), ( Sprite.getPosition() - bullet->GetSprite().getPosition()) );
		
		float myangle = NormalizeAngle(atan2( det, dot ));

		if ( myangle < 0 )
			int i =1;

		NewVelocity += (myangle * 50) / len;

	}
	// TODO keep enemies apart from each other
	//float len = Length( (enemy->GetSprite().getPosition() - Sprite.getPosition()) );
	//NewVelocity -= len / 5.f;

	Velocity = NewVelocity;

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

void Unit::SetColor( const sf::Color & color ) {
	Sprite.setColor(color);
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

