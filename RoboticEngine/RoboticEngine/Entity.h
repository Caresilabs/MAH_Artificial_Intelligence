#pragma once

#include <map>
#include <string>
#include <typeindex>

#include "SFML/Graphics.hpp"
#include "LuaScript.h"

class Component;

static int NEXT_ID = 1;

class Entity {
public:
	
	Entity();

	Entity(const char* fileName);

	void Draw( sf::RenderWindow* window ) {

	}

	void Update( float delta ) {

	}
	
	void Add( std::type_index type, Component* c );

	template <typename T>
	void AddComponent( LuaScript* table ) {
		Add( std::type_index( typeid(T) ), new T( table ) );
	}

	template <typename T>
	T* Get() {
		auto it = components.find( std::type_index( typeid(T) ) );
		if ( it != components.end() ) {
			return dynamic_cast<T*>(it->second);
		}
		return nullptr;
	}

	void SetType( const std::string& type ) {
		this->type = type;
	}

	std::string GetType() const {
		return type;
	}

	int GetId() const {
		return id;
	}

	~Entity();
private:
	std::string type;
	std::map<std::type_index, Component*> components;
	LuaScript lua;
	int id;
};

