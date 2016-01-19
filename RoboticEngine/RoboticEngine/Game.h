#pragma once

#include <SFML/Graphics.hpp>

class Screen;

class Game {
public:
	Game( Screen* startScreen );

	void GameLoop();

	void SetScreen( Screen* newScreen );

	sf::RenderWindow& GetWindow() const;

	virtual ~Game();

private:
	sf::RenderWindow* m_window;

	Screen* m_screen;

};

