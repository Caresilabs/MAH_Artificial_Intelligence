#pragma once

#include <RSprite.h>

class GAScreen;
class Bullet;

class Unit {
public:
					Unit( GAScreen* world, int id, sf::Texture& texture );

	void			Set( float health, float speed, float firerate );

	void			Update( const Unit* enemy, float delta );

	void			UpdateAI( float delta );

	void			UpdateMovement( float delta );

	void			Damage( Bullet* bullet );

	void			AddDamageDealt( float Damage );

	void			Reset();

	void			Won();

	void			SetColor( const sf::Color& color );

	void			ResetFitnessData();

	float			FitnessFunction() const;

	int				GetId() const;

	bool			IsDead() const;

	float			GetHealth() const;

	float			GetVelocity() const;

	float			GetAngle() const;

	const RSprite&	GetSprite() const;

	// Attributes
	float			GetMaxHealth() const;
	float			GetSpeed() const;
	float			GetFirerate() const;
	float			GetFireError() const;
	float			GetStrength() const;
	float			GetSize() const;

private:
	static const int BULLET_SPEED = 12;

	RSprite			Sprite;
	GAScreen*		World;

	// Angular Velocity
	float			Velocity;
	float			Angle;

	// info
	int				Id;
	bool			Dead;
	float			TotalTimeAlive;
	float			FirerateTime;
	int				Wins;
	float			DamageDealt;

	

	// Attributes
	float			Speed;
	float			Strength;

	float			Health;
	float			MaxHealth;
	float			Size;

	float			Firerate;
	float			FireError;

};

