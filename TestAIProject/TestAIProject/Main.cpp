#include <Game.h>

#include "HelloWorldScreen.h"

int main() {

	Game game( "Hello World", 1280, 720 ); //new HelloWorldScreen()

	game.GameLoop();

	return 0;
}