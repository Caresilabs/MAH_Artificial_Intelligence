#include "HelloWorldScreen.h"


HelloWorldScreen::HelloWorldScreen() {
}


float speed = 200.f;
sf::CircleShape shape( 100.f );
sf::Vector2f velocity( speed, speed );

void HelloWorldScreen::OnCreate() {
	shape.setFillColor( sf::Color::Green );
}

void HelloWorldScreen::OnUpdate( float delta ) {
	shape.setPosition( shape.getPosition() + velocity * delta );

	if ( shape.getPosition().x < 0 )
		velocity.x = speed;

	if ( shape.getPosition().y < 0 )
		velocity.y = speed;

	if ( shape.getPosition().x > game->GetWindow().getSize().x - shape.getRadius()*2.f )
		velocity.x = -speed;


	if ( shape.getPosition().y > game->GetWindow().getSize().y - shape.getRadius() * 2.f )
		velocity.y = -speed;

}

void HelloWorldScreen::OnDraw() {
	game->GetWindow().draw( shape );
}


void HelloWorldScreen::HandleEvent( const sf::Event & event ) {
	if ( event.type == sf::Event::MouseButtonReleased )
		game->GetWindow().close();

	if ( event.type == sf::Event::KeyPressed )
		if ( event.key.code == sf::Keyboard::W )
			speed *= 1.2f;
}

