#include "GAScreen.h"

#include <RSprite.h>
#include <vector>
#include <iostream>

GAScreen::GAScreen() : State( GAState::PRE ), Population( POPULATION_SIZE ) {
}


void GAScreen::OnCreate() {
	// Load assets
	BulletTexture.loadFromFile( "wheeljoy.png" );
	UnitTexture.loadFromFile( "player.png" );
	Font.loadFromFile( "Roboto.ttf" );

	// Seed
	GetRandomNumber( 0, 100, true );

	// Populate
	InitRandomPopulation();
}

void GAScreen::InitRandomPopulation() {
	for ( size_t i = 0; i < POPULATION_SIZE; i++ ) {
		Population[i] = new Unit( this, i, UnitTexture );
		Population[i]->Set( GetRandomNumber( 20, 200 ), GetRandomNumber( 50, 150 ), GetRandomNumber( 1, 5 ) );
	}
	UnitIndex1 = 0;
	UnitIndex2 = 1;
}


void GAScreen::OnUpdate( float delta ) {
	for ( size_t i = 0; i < UPDATES_PER_FRAME; i++ ) {

		switch ( State ) {
		case GAState::PRE:
		{
			Unit* unit1 = Population[UnitIndex1];
			Unit* unit2 = Population[UnitIndex2];

			unit1->Reset();
			unit2->Reset();

			State = GAState::SIMULATING;
			break;
		}
		case GAState::SIMULATING:
		{
			Unit* unit1 = Population[UnitIndex1];
			Unit* unit2 = Population[UnitIndex2];

			unit1->Update( unit2, delta );
			unit2->Update( unit1, delta );

			for each (auto bullet in Bullets) {
				bullet->Update( delta );

				if ( bullet->GetOwner() != unit1 && unit1->GetSprite().getGlobalBounds().intersects( bullet->GetSprite().getGlobalBounds() ) ) {
					unit1->Damage( bullet );
				}
				if ( bullet->GetOwner() != unit2 && unit2->GetSprite().getGlobalBounds().intersects( bullet->GetSprite().getGlobalBounds() ) ) {
					unit2->Damage( bullet );
				}
			}

			if ( unit1->IsDead() || unit2->IsDead() ) {
				if ( unit1->IsDead() )
					unit2->Won();

				if ( unit2->IsDead() )
					unit1->Won();

				//  New battle
				++UnitIndex2;
				if ( UnitIndex1 == UnitIndex2 )
					++UnitIndex2;

				if ( UnitIndex2 >= POPULATION_SIZE ) {
					++UnitIndex1;
					UnitIndex2 = 0;

					if ( UnitIndex1 == UnitIndex2 )
						++UnitIndex2;

					if ( UnitIndex1 >= POPULATION_SIZE ) {
						State = GAState::POST;
						break;
					}
				}
				// Reset Sim
				Population[UnitIndex1]->Reset();
				Population[UnitIndex2]->Reset();
				ClearBullets();
			}
		}
		break;
		case GAState::POST:
			State = GAState::POST;
			break;
		default:
			break;
		}

	}

}

void GAScreen::OnDraw() {
	game->GetWindow().clear( sf::Color::Color( 100, 100, 100 ) );
	if ( State == GAState::SIMULATING ) {
		Unit* unit1 = Population[UnitIndex1];
		Unit* unit2 = Population[UnitIndex2];

		game->GetWindow().draw( unit1->GetSprite() );
		game->GetWindow().draw( unit2->GetSprite() );

		// Draw bullets
		for each (auto bullet in Bullets) {
			game->GetWindow().draw( bullet->GetSprite() );
		}
	}

	sf::Text text;

	// select the font
	text.setFont( Font ); // font is a sf::Font

	text.setString( std::to_string( UnitIndex1 ) + " V.S " + std::to_string( UnitIndex2 ) );
	text.setCharacterSize( 24 );
	text.setScale( 0.01f, 0.01f );

	game->GetWindow().draw( text );
}

void GAScreen::OnEvent( const sf::Event & event ) {

}

void GAScreen::SpawnBullet( Unit& owner, sf::Vector2f direction, float speed, float error, float strength ) {
	float angle = atan2( direction.y, direction.x );
	angle += GetRandomNumber( 0, error, false ) - (error*0.5f);
	Bullet* bullet = new Bullet( &owner, BulletTexture, sf::Vector2f( cos( angle ), sin( angle ) ), speed, strength );

	Bullets.push_back( bullet );
}

void GAScreen::ClearBullets() {
	for each (auto bullet in Bullets) {
		delete bullet;
	}
	Bullets.clear();
}

int GAScreen::GetRandomNumber( int min, int max, bool seed ) {
	int	number;

	if ( seed )
		srand( (unsigned)time( NULL ) );

	number = (((abs( rand() ) % (max - min + 1)) + min));

	if ( number > max )
		number = max;

	if ( number < min )
		number = min;

	return number;
}

GAScreen::~GAScreen() {
	for each (auto unit in Population) {
		delete unit;
	}

	ClearBullets();
}
