#include <SFML/System.hpp>

#include "Screen.h"
#include "Game.h"
#include "LuaScreen.h"

Game::Game( char* title, int width, int height, Screen* startScreen ) : m_window( new sf::RenderWindow() ), UpdatesPerFrame(1) {
	this->m_window->create( sf::VideoMode( width, height ), title );
	this->m_window->setFramerateLimit( 60 );

	m_window->setView( sf::View( sf::FloatRect( 0, 0, 16, 16 * (height / (float)width) ) ) );

	if ( startScreen != nullptr ) {
		SetScreen( startScreen );
	} else {
		SetScreen( new LuaScreen() );
	}
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
				break;
			}
			case sf::Event::Resized:
			{
				//m_window->setView( sf::View( sf::FloatRect( 0, 0, event.size.width, event.size.height ) ) );
				m_window->setView( sf::View( sf::FloatRect( 0, 0, 16, 16 * (event.size.height / (float)event.size.width) ) ) );
				break;
			}
			default:
				break;
			}
			m_screen->OnEvent( event );
		}

		for ( int i = 0; i < UpdatesPerFrame; i++ ) {
			m_screen->OnUpdate( dt );
			this->m_window->clear( sf::Color::Black );
			m_screen->OnDraw();
			this->m_window->display();
		}
	
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

const sf::Vector2f & Game::GetViewSize() const {
	return m_window->getView().getSize();
}

const sf::Vector2f Game::GetMousePosition() const {
	auto mousePos = m_window->mapPixelToCoords( sf::Mouse::getPosition( *m_window ) );
	return mousePos;
}


Game::~Game() {
	if ( m_screen != nullptr )
		delete m_screen;

	delete m_window;
}
