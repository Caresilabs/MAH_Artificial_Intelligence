#include "RSprite.h"

void RSprite::SetSize( float width, float height, bool center ) {
	setScale( width / getTextureRect().width, height / getTextureRect().height );

	if ( center )
		setOrigin( sf::Vector2f( getTextureRect().width / 2.f, getTextureRect().height / 2.f ) );
}

RSprite::~RSprite() {
}
