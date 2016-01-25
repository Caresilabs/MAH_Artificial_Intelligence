#include "Engine.h"

Engine::Engine() {
}

void Engine::Add( Entity* entity ) {
	this->entities[entity->GetId()] = entity;
}

void Engine::Add( const char * fileName ) {
	auto entity = new Entity(fileName);
}

void Engine::Update( float delta ) {
	for ( auto& entity : entities ) {
		entity.second->Update( delta );
	}
}

void Engine::Draw( sf::RenderWindow * window ) {
	for ( auto& entity : entities ) {
		entity.second->Draw( window );
	}
}


Engine::~Engine() {
	for ( auto& entity : entities ) {
		delete entity.second;
	}
}
