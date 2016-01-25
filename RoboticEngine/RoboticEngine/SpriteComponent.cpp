#include "SpriteComponent.h"

SpriteComponent::SpriteComponent( LuaScript * table ) {
	tex.loadFromFile(table->get<std::string>("filename"));
	sprite.setTexture( tex );
}


void SpriteComponent::Update( float delta ) {
	sprite.setPosition( sprite.getPosition() + sf::Vector2f( 1, 0 ) );
}

void SpriteComponent::Draw( sf::RenderWindow * window ) {
	window->draw( sprite );
}

SpriteComponent::~SpriteComponent() {
}