#include <SFML/System.hpp>

#include "Screen.h"
#include "Game.h"

Game::Game( Screen * startScreen ) : m_window( new sf::RenderWindow() ) {
	this->m_window->create( sf::VideoMode( 1280, 720 ), "Robotic Engine V0.1" );
	this->m_window->setFramerateLimit( 60 );

	SetScreen( startScreen );
}

void Game::GameLoop() {
	sf::Clock clock;

	while ( this->m_window->isOpen() ) {
		sf::Time elapsed = clock.restart();
		float dt = elapsed.asSeconds();

		if ( m_screen == nullptr )
			continue;

		sf::Event event;

		while ( m_window->pollEvent( event ) ) {
			switch ( event.type ) {
			case sf::Event::Closed:
			{
				m_window->close();
				break;
			}
			case sf::Event::KeyPressed:
			{
				if ( event.key.code == sf::Keyboard::Escape )
					m_window->close();
				else
					m_screen->HandleEvent( event );
				break;
			}
			case sf::Event::Resized:
			{
				m_window->setView( sf::View( sf::FloatRect( 0, 0, event.size.width, event.size.height ) ) );
				m_screen->HandleEvent( event );
				break;
			}
			default:
				m_screen->HandleEvent( event );
				break;
			}
		}

		m_screen->OnUpdate( dt );
		this->m_window->clear( sf::Color::Black );
		m_screen->OnDraw();
		this->m_window->display();
	}
}

void Game::SetScreen( Screen* newScreen ) {
	if ( m_screen != nullptr )
		delete m_screen; 

	this->m_screen = newScreen;
	this->m_screen->SetGame( this );
	this->m_screen->OnCreate();
}

sf::RenderWindow& Game::GetWindow() const {
	return *m_window;
}


Game::~Game() {
	if ( m_screen != nullptr )
		delete m_screen;

	delete m_window;
}
