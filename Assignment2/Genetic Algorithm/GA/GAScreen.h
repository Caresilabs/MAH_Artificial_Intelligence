#pragma once

#include <Screen.h>
#include "Unit.h"
#include "Bullet.h"
#include "GenerationLogger.h"

#define HEALTH_MIN 50
#define HEALTH_MAX 250

#define SPEED_MIN 0.25f 
#define SPEED_MAX 1.25f

#define FIRERATE_MIN 1.2f
#define FIRERATE_MAX 4.0f

class GAScreen : public Screen {
public:
									GAScreen();

	virtual void					OnCreate() override;

	void							InitRandomPopulation();

	virtual void					OnUpdate( float delta ) override;
	virtual void					OnDraw() override;
	virtual void					OnEvent( const sf::Event & event ) override;

	void							SpawnBullet( Unit& owner, sf::Vector2f direction, float speed, float error, float strength );
	const  std::vector<Bullet*>&	GetBullets() const;

	void							ClearBullets();

									~GAScreen();

	enum class GAState {
		PRE, SIMULATING, POST
	};

private:
	void							Breed();
	void							Mutate( float Chance );

	static const unsigned int		POPULATION_SIZE = 100;
	static const unsigned int		GENERATION_COUNT = 1000;
	static const unsigned int		UPDATES_PER_FRAME = 1;

	GAState							State;
	GenerationLogger				Logger;

	std::vector<Unit*>				Population;
	std::vector<Bullet*>			Bullets;

	int								UnitIndex1;
	int								UnitIndex2;

	int								Generation;

	bool							Paused;

	// Assets
	sf::Texture						UnitTexture;
	sf::Font						Font;
};

