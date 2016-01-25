#pragma once
#include "Component.h"
#include "RSprite.h"

class SpriteComponent : public Component {
public:
	SpriteComponent();
	SpriteComponent( LuaScript* table );
	
	~SpriteComponent();

	// Inherited via Component
	virtual void Update( float delta ) override;
	virtual void Draw( sf::RenderWindow * window ) override;

private:
	RSprite sprite;
	sf::Texture tex;
};

