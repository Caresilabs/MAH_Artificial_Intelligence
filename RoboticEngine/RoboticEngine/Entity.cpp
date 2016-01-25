#include "Entity.h"

#include "Component.h"
#include "SpriteComponent.h"

Entity::Entity() : id(NEXT_ID++), lua("") {
}

Entity::Entity( const char * fileName ) : id (NEXT_ID++), lua (fileName){
	auto table = lua.getTableKeys( "player" );

	for ( auto& key : table ) {
		if ( key == "SpriteComponent" ) {
			AddComponent<SpriteComponent>( &lua );
		}
	}
}

void Entity::Add( std::type_index type, Component* c ) {
	components[type] = c;
}

Entity::~Entity() {
	for ( auto& c : components ) {
		delete c.second;
	}
}
