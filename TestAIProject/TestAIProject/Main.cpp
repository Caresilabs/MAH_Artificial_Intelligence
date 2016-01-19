#include <Game.h>

#include "HelloWorldScreen.h"


int main() {

	Game game( new HelloWorldScreen() );

	game.GameLoop();

	return 0;
}