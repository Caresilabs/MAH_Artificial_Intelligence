#pragma once

#include <RSprite.h>

class GAScreen;
class Bullet;

class Unit {
public:
	Unit( GAScreen* world, int id, sf::Texture& texture );

	void	Set( float health, float speed, float firerate );

	void	Update( const Unit* enemy, float delta );

	void	UpdateAI( float delta );

	void	UpdateMovement( float delta );

	void	Damage( Bullet* bullet );

	void	Reset();

	void	Won();

	float	FitnessFunction() const;

	int		GetId() const;

	bool	IsDead() const;

	const RSprite& GetSprite() const;

private:
	static const int BULLET_SPEED = 50;

	RSprite		Sprite;

	GAScreen*	World;

	sf::Vector2f Velocity;

	// info
	int			Id;
	bool		Dead;
	float		SessionTime;
	sf::Clock	FirerateClock;
	int			Wins;

	// Attributes

	float	Speed;
	float	Strength;

	float	Health;
	float	MaxHealth;
	float	Size;

	float	Firerate;
	float	FireError;

};

