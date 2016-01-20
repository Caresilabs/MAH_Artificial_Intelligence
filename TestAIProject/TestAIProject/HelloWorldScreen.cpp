#include "HelloWorldScreen.h"

#include <RSprite.h>
#include <vector>

HelloWorldScreen::HelloWorldScreen() {
}


float speed = 2.f;
std::vector<sf::RectangleShape*> shapes;

sf::Vector2f velocity( speed, speed );

sf::Texture tex;
RSprite sprite;


void HelloWorldScreen::OnCreate() {
	tex.loadFromFile( "wheeljoy.png" );

	sprite.setTexture( tex );
	sprite.SetSize( game->GetViewSize().x , game->GetViewSize().y, false );

	sf::RectangleShape* shape = new sf::RectangleShape(sf::Vector2f( 1.f, 1.9f ));
	shape->setFillColor( sf::Color::Green );
	shape->setOrigin( sf::Vector2f( shape->getSize().x / 2.f, shape->getSize().y / 2.f ) );

	shapes.push_back(shape);
}

sf::Clock colorClock;

sf::Clock spawnClock;

void HelloWorldScreen::OnUpdate( float delta ) {
	for ( sf::RectangleShape* const shape : shapes ) {
		shape->setPosition( shape->getPosition() + velocity * delta );

		shape->rotate( delta * 180 );
	}
	

	/*if ( shape.getPosition().x < 0 )
		velocity.x = speed;

	if ( shape.getPosition().y < 0 )
		velocity.y = speed;

	if ( shape.getPosition().x > game->GetViewSize().x - shape.getSize().x )
		velocity.x = -speed;


	if ( shape.getPosition().y > game->GetViewSize().y - shape.getSize().y )
		velocity.y = -speed;*/


	if ( spawnClock.getElapsedTime().asSeconds() > 1.5f ) {
		spawnClock.restart();

		auto shape = new sf::RectangleShape( sf::Vector2f( 1.f, 1.f ) );
		shape->setOrigin( sf::Vector2f( shape->getSize().x / 2.f, shape->getSize().y / 2.f ) );

		shapes.push_back( shape );

	}

	
	/*if ( colorClock.getElapsedTime().asMilliseconds() > 1.f ) {
		colorClock.restart();

		shape.setFillColor( sf::Color( shape.getFillColor() ) + sf::Color(delta * 255.f, delta * 100.f, delta * 80.f) );

		if ( shape.getFillColor() == sf::Color::White ) {
			shape.setFillColor( sf::Color::Black );
		}
	}*/


}

void HelloWorldScreen::OnDraw() {

	game->GetWindow().draw( sprite );


	for ( sf::RectangleShape* const shape : shapes ) {
		game->GetWindow().draw( *shape );
	}


}

void HelloWorldScreen::OnEvent( const sf::Event & event ) {
	if ( event.type == sf::Event::MouseButtonReleased ) {
		sf::Vector2f input;
		input = game->GetMousePosition();

		auto i = std::begin( shapes );
		while ( i != std::end( shapes ) ) {
			if ( *i != nullptr && (*i)->getGlobalBounds().contains( input ) ) {
				delete *i;
				i = shapes.erase( i );
			}
			else
				++i;
		}

	}

	if ( event.type == sf::Event::KeyPressed )
		if ( event.key.code == sf::Keyboard::W )
			speed *= 1.2f;
}