#pragma once

#include <Screen.h>
#include "Unit.h"
#include "Bullet.h"

class GAScreen : public Screen {
public:
	GAScreen();

	virtual void OnCreate() override;
	void		 InitRandomPopulation();
	virtual void OnUpdate( float delta ) override;
	virtual void OnDraw() override;
	virtual void OnEvent( const sf::Event & event ) override;

	void SpawnBullet( Unit& owner, sf::Vector2f direction, float speed, float error, float strength );

	void ClearBullets();

	int GetRandomNumber( int min, int max, bool seed = false);

	~GAScreen();

	enum class GAState {
		PRE, SIMULATING ,POST
	};

private:
	static const int	POPULATION_SIZE = 10;
	static const int	GENERATION_COUNT = 1000;
	static const int	UPDATES_PER_FRAME = 1;

	GAState				State;

	std::vector<Unit*>	Population;
	std::vector<Bullet*>Bullets;

	int					UnitIndex1;
	int					UnitIndex2;
	

	// Assets
	sf::Texture			UnitTexture;
	sf::Texture			BulletTexture;
	sf::Font			Font;
};

