#include "GAScreen.h"

#include <RSprite.h>
#include <vector>
#include <iostream>
#include "Helpers.h"

GAScreen::GAScreen() : State( GAState::PRE ), Population( POPULATION_SIZE ), Paused( false ), Generation(1) {
}

void GAScreen::OnCreate() {
	game->UpdatesPerFrame = UPDATES_PER_FRAME;

	// Load assets
	UnitTexture.loadFromFile( "Assets/player.png" );
	Font.loadFromFile( "Assets/Roboto.ttf" );

	// Seed
	GetRandomNumber( 0, 100, true );

	// Populate
	InitRandomPopulation();
}

void GAScreen::InitRandomPopulation() {
	for ( int i = 0; i < POPULATION_SIZE; i++ ) {
		Population[i] = new Unit( this, i, UnitTexture );
		Population[i]->Set( GetRandomNumber( HEALTH_MIN, HEALTH_MAX ), GetRandomNumber( SPEED_MIN, SPEED_MAX ), GetRandomNumber( FIRERATE_MIN, FIRERATE_MAX ) );
	}
}

void GAScreen::OnUpdate( float delta ) {

	switch ( State ) {
	case GAState::PRE:
	{
		UnitIndex1 = 0;
		UnitIndex2 = 1;

		Unit* unit1 = Population[UnitIndex1];
		Unit* unit2 = Population[UnitIndex2];

		// Set Color
		unit1->SetColor( sf::Color::Green );
		unit2->SetColor( sf::Color::Red );

		// Reset Fitness data of all units
		for ( int i = 0; i < POPULATION_SIZE; i++ ) {
			Population[i]->ResetFitnessData();
		}

		unit1->Reset();
		unit2->Reset();

		State = GAState::SIMULATING;
		break;
	}
	case GAState::SIMULATING:
	{
		if ( Paused )
			break;

		Unit* unit1 = Population[UnitIndex1];
		Unit* unit2 = Population[UnitIndex2];

		// Update units
		unit1->Update( unit2, delta );
		unit2->Update( unit1, delta );

		// Update Bullets
		for ( int i = 0; i < Bullets.size();) {
			Bullet* bullet = Bullets[i];

			if ( bullet == nullptr ) {
				++i;
				continue;
			}

			bullet->Update( delta );

			if ( bullet->GetOwner() != unit1 && unit1->GetSprite().getGlobalBounds().intersects( bullet->GetSprite().getGlobalBounds() ) ) {
				unit1->Damage( bullet );
				Bullets.erase( Bullets.begin() + i );
				delete bullet;
			} else if ( bullet->GetOwner() != unit2 && unit2->GetSprite().getGlobalBounds().intersects( bullet->GetSprite().getGlobalBounds() ) ) {
				unit2->Damage( bullet );
				Bullets.erase( Bullets.begin() + i );
				delete bullet;
			} else {
				++i;
			}
		}

		// Fight is over
		if ( unit1->IsDead() || unit2->IsDead() ) {
			// Who won? 
			if ( unit1->IsDead() )
				unit2->Won();

			if ( unit2->IsDead() )
				unit1->Won();

			//  New battle
			++UnitIndex2;

			// Don't fight yourself
			if ( UnitIndex1 == UnitIndex2 )
				++UnitIndex2;

			// If we fought all other units, then I'm done
			if ( UnitIndex2 >= POPULATION_SIZE ) {
				++UnitIndex1;
				UnitIndex2 = 0;

				// Don't fight yourself
				if ( UnitIndex1 == UnitIndex2 ) 
					++UnitIndex2;

				// Generation is done
				if ( UnitIndex1 >= POPULATION_SIZE ) {
					State = GAState::POST;
					break;
				}
			}

			// Reset Sim
			Population[UnitIndex1]->Reset();
			Population[UnitIndex2]->Reset();
			ClearBullets();

			// Change colors
			Population[UnitIndex1]->SetColor( sf::Color::Green );
			Population[UnitIndex2]->SetColor( sf::Color::Red );
		}
	}
	break;
	case GAState::POST:

		// Sort
		std::sort( Population.begin(), Population.end(),
			[]( const Unit* a, const Unit* b ) -> bool {
			return a->FitnessFunction() > b->FitnessFunction();
		} );

		Logger.Log( Population );

		Breed();
		Mutate( 0.1f );

		++Generation;

		State = GAState::PRE;
		break;
	default:
		break;
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


		sf::Text text;

		// select the font
		text.setFont( Font );

		std::string txt =
			"(Green) Id:" + std::to_string( UnitIndex1 )
			+ "\nHealth: " + std::to_string( unit1->GetHealth() )
			+ "\nMaxHealth: " + std::to_string( unit1->GetMaxHealth() )
			+ "\nSize: " + std::to_string( unit1->GetSize() )
			+ "\nSpeed: " + std::to_string( unit1->GetSpeed() )
			+ "\nStrength: " + std::to_string( unit1->GetStrength() )
			+ "\nFirerate: " + std::to_string( unit1->GetFirerate() )
			+ "\nFireError: " + std::to_string( unit1->GetFireError() )
			;

		txt.append( "\n\n(Red) Id:" + std::to_string( UnitIndex2 )
			+ "\nHealth: " + std::to_string( unit2->GetHealth() )
			+ "\nMaxHealth: " + std::to_string( unit2->GetMaxHealth() )
			+ "\nSize: " + std::to_string( unit2->GetSize() )
			+ "\nSpeed: " + std::to_string( unit2->GetSpeed() )
			+ "\nStrength: " + std::to_string( unit2->GetStrength() )
			+ "\nFirerate: " + std::to_string( unit2->GetFirerate() )
			+ "\nFireError: " + std::to_string( unit2->GetFireError() ) );

		txt.append("\n\nGeneration: "  + std::to_string( Generation ) );

		text.setString( txt );
		text.setCharacterSize( 24 );
		text.setScale( 0.01f, 0.01f );

		game->GetWindow().draw( text );
	}
}

void GAScreen::OnEvent( const sf::Event & event ) {

	if ( event.type == event.KeyReleased ) {
		if ( event.key.code == sf::Keyboard::Space )
			Paused = !Paused;

		if ( event.key.code == sf::Keyboard::Num1 )
			game->UpdatesPerFrame = 1;
		if ( event.key.code == sf::Keyboard::Num2 )
			game->UpdatesPerFrame = 2;
		if ( event.key.code == sf::Keyboard::Num3 )
			game->UpdatesPerFrame = 4;
		if ( event.key.code == sf::Keyboard::Num4 )
			game->UpdatesPerFrame = 6;
		if ( event.key.code == sf::Keyboard::Num5 )
			game->UpdatesPerFrame = 8;
		if ( event.key.code == sf::Keyboard::Num6 )
			game->UpdatesPerFrame = 10;
		if ( event.key.code == sf::Keyboard::Num7 )
			game->UpdatesPerFrame = 12;
		if ( event.key.code == sf::Keyboard::Num8 )
			game->UpdatesPerFrame = 14;
		if ( event.key.code == sf::Keyboard::Num9 )
			game->UpdatesPerFrame = 16;
		if ( event.key.code == sf::Keyboard::Num0 )
			game->UpdatesPerFrame = 32;
	}

}

void GAScreen::SpawnBullet( Unit& owner, sf::Vector2f direction, float speed, float error, float strength ) {
	float angle = atan2( direction.y, direction.x );

	angle += GetRandomNumber( -error, error, false );

	Bullet* bullet = new Bullet( &owner, UnitTexture, sf::Vector2f( cos( angle ), sin( angle ) ), speed, strength );
	bullet->SetColor( owner.GetSprite().getColor() );

	Bullets.push_back( bullet );
}

const  std::vector<Bullet*>& GAScreen::GetBullets() const {
	return Bullets;
}

void GAScreen::ClearBullets() {
	for each (auto bullet in Bullets) {
		delete bullet;
	}
	Bullets.clear();
}

GAScreen::~GAScreen() {
	for each (auto unit in Population) {
		delete unit;
	}

	ClearBullets();
}

void GAScreen::Breed() {
	int ToBreed = Population.size() * 0.5f;

	for ( int i = 0; i < ToBreed; i += 2 ) {

		float Health = 0;
		float Speed = 0;
		float Firerate = 0;

		for ( int x = 0; x < 2; x++ ) {
			int HealthIndex = GetRandomNumber( i, i + 1 );
			int SpeedIndex = GetRandomNumber( i, i + 1 );
			int FirerateIndex = GetRandomNumber( i, i + 1 );

			Health = Population[HealthIndex]->GetMaxHealth();
			Speed = Population[SpeedIndex]->GetSpeed();
			Firerate = Population[FirerateIndex]->GetFirerate();

 			int Id = Population.size() - i - 1 - x;
			Unit* Breed = new Unit( this, Id, UnitTexture );
			Breed->Set( Health, Speed, Firerate );

			delete Population[Id];

			Population[Id] = Breed;
		}
	}
}

void GAScreen::Mutate( float Chance ) {
	for each (auto Unit in Population) {
		if ( GetRandomNumber( 0.f, 1.f ) < Chance ) {

			float Health = Clamp( Unit->GetMaxHealth() + GetRandomNumber( -20, 20 ), HEALTH_MIN, HEALTH_MAX );
			float Speed = Clamp( Unit->GetSpeed() + GetRandomNumber( -0.2f, 0.2f ), SPEED_MIN, SPEED_MAX );
			float Firerate = Clamp( Unit->GetFirerate() + GetRandomNumber( -0.7f, 0.7f ), FIRERATE_MIN, FIRERATE_MAX );

			Unit->Set( Health, Speed, Firerate );
		}
	}

}
