#pragma once

#include <SFML/Graphics.hpp>

class Screen;

class Game {
public:
	Game( char* title, int width, int height, Screen* startScreen );

	void GameLoop();

	void SetScreen( Screen* newScreen );

	sf::RenderWindow& GetWindow() const;

	const sf::Vector2f& GetViewSize() const;

	const sf::Vector2f GetMousePosition() const;

	virtual ~Game();

private:
	sf::RenderWindow* m_window;

	Screen* m_screen;

};

