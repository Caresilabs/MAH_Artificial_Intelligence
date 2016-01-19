#pragma once

#include <SFML/Graphics.hpp>

#include "Game.h"

class Screen {
public:
	Screen() = default;

	virtual void OnCreate() = 0;

	virtual void OnUpdate( float delta ) = 0;

	virtual void OnDraw() = 0;

	virtual void HandleEvent( const sf::Event& event ) = 0;

	void SetGame( Game* game );

	virtual ~Screen();

protected:
	Game* game;
};

