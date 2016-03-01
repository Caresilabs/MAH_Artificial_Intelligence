#pragma once

#include <RSprite.h>

class GAScreen;

class Unit {
public:
	Unit(GAScreen* world, int id, sf::Texture& texture);

	void	Set( float health, float speed, float firerate );

	void	Update( const Unit* enemy, float delta );

	void	UpdateAI( float delta );

	void	UpdateMovement( float delta );

	void	Reset();

	float	FitnessFunction() const;

	int		GetId() const;

	bool	IsDead() const;

	const RSprite& GetSprite() const;

private:
	RSprite		Sprite;

	GAScreen*	World;

	float		SessionTime;

	int		Id;

	bool	Dead;

	float	Speed;
	float	Strength;

	float	Health;
	float	MaxHealth;
	float	Size;

	float	Firerate;
	float	FireError;

};

