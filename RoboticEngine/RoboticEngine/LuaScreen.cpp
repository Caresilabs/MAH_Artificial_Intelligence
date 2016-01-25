#include "LuaScreen.h"

LuaScreen::LuaScreen(const char* name) : lua(name) {
	lua.push( "THIS" , this );
	std::cout << "wp0";
}

int LuaScreen::Add( ) {
	std::cout << "wp";
	//engine.Add(  lua_tostring(state, -1 ) ) ;
	return 0;
}


void LuaScreen::OnCreate() {
	lua.call( "OnCreate", 0 );
 }

void LuaScreen::OnUpdate( float delta ) {
	lua.push( delta );
	lua.call( "OnUpdate", 1 );

	engine.Update( delta );
}

void LuaScreen::OnDraw() {
}

void LuaScreen::OnEvent( const sf::Event& event ) {
	//lua.call( "OnEvent", 0 );
}

LuaScreen::~LuaScreen() {
}