#include <Game.h>

#include "GAScreen.h"

int main() {

	Game game( "Arena - Genetic Algorithm", 1280, 720, new GAScreen() );

	game.GameLoop();

	return 0;
}