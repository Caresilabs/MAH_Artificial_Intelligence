#pragma once
#include "Screen.h"
#include "LuaScript.h"
#include "Engine.h"

class LuaScreen : public Screen {
public:
	LuaScreen(const char* name = "main.lua");

	int Add(  );

	virtual void OnCreate() override;
	virtual void OnUpdate( float delta ) override;
	virtual void OnDraw() override;
	virtual void OnEvent( const sf::Event & event ) override;

	~LuaScreen();
private:
	LuaScript lua;
	Engine engine;
};

