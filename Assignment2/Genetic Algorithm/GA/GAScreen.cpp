#include "GAScreen.h"

#include <RSprite.h>
#include <vector>
#include <iostream>

GAScreen::GAScreen() : UpdatesPerFrame( 1 ), State( GAState::PRE ), Population( POPULATION_SIZE ) {
}


void GAScreen::OnCreate() {
	// Load assets
	Tex.loadFromFile( "wheeljoy.png" );
	UnitTexture.loadFromFile( "wheeljoy.png" );

	// Seed
	GetRandomNumber( 0, 100, true );

	// Populate
	InitRandomPopulation();
}

void GAScreen::InitRandomPopulation() {
	for ( size_t i = 0; i < POPULATION_SIZE; i++ ) {
		Population[i] = new Unit( i, UnitTexture );
		Population[i]->Set( 1, 1, 1 );
	}
	UnitIndex1 = 0;
	UnitIndex2 = 1;
}


void GAScreen::OnUpdate( float delta ) {
	for ( size_t i = 0; i < UpdatesPerFrame; i++ ) {

		switch ( State ) {
		case GAState::PRE:
			Unit* unit1 = Population[UnitIndex1];
			Unit* unit2 = Population[UnitIndex2];

			unit1->Reset();
			unit2->Reset();

			State = GAState::SIMULATING;
			break;
		case GAState::SIMULATING:
		{
			Unit* unit1 = Population[UnitIndex1];
			Unit* unit2 = Population[UnitIndex2];

			unit1->Update( unit2, delta );
			unit2->Update( unit1, delta );

			if ( unit1->IsDead() || unit2->IsDead() ) {
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
					}
				}
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
	game->GetWindow().clear( sf::Color::Color( 100,100,100 ));
	if ( State == GAState::SIMULATING ) {
		Unit* unit1 = Population[UnitIndex1];
		Unit* unit2 = Population[UnitIndex2];

		game->GetWindow().draw( unit1->GetSprite() );
		game->GetWindow().draw( unit2->GetSprite() );
	}
}

void GAScreen::OnEvent( const sf::Event & event ) {

}

int GAScreen::GetRandomNumber( int min, int max, bool seed ) {
	int	number;

	if ( seed )
		srand( (unsigned)time( NULL ) );

	number = (((abs( rand() ) % (max - min + 1)) + min));

	if ( number>max )
		number = max;

	if ( number<min )
		number = min;

	return number;
}

GAScreen::~GAScreen() {
	for each (auto unit in Population) {
		delete unit;
	}
}
