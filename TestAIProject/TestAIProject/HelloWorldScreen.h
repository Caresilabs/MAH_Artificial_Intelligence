#pragma once

#include <Screen.h>

class HelloWorldScreen : public Screen {
public:
	HelloWorldScreen();

	// Inherited via Screen
	virtual void OnCreate() override;
	virtual void OnUpdate( float delta ) override;
	virtual void OnDraw() override;
	virtual void HandleEvent( const sf::Event & event ) override;
};

