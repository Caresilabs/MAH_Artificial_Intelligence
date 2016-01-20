#pragma once

#include <Screen.h>

class HelloWorldScreen : public Screen {
public:
	HelloWorldScreen();

	virtual void OnCreate() override;
	virtual void OnUpdate( float delta ) override;
	virtual void OnDraw() override;
	virtual void OnEvent( const sf::Event & event ) override;
};

