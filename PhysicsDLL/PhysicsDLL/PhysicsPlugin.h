#pragma once

#include <string>
#include "PhysicsWorld.h"

#ifdef SUPPORT_DLL
#ifdef BUILDING_PHYSICS_NATIVE_PLUGIN
#define PHYSICS_NATIVE_PLUGIN_API __declspec(dllexport)
#else
#define PHYSICS_NATIVE_PLUGIN_API __declspec(dllimport)
#endif
#else
#define PHYSICS_NATIVE_PLUGIN_API
#endif

extern "C"
{
	PHYSICS_NATIVE_PLUGIN_API void CreatePhysicsWorld();
	PHYSICS_NATIVE_PLUGIN_API void DestroyPhysicsWorld();

	PHYSICS_NATIVE_PLUGIN_API void UpdatePhysicsWorld(float deltaTime);

	PHYSICS_NATIVE_PLUGIN_API void AddParticle(std::string id, float invMass, float* pos);

	PHYSICS_NATIVE_PLUGIN_API float* GetPhysicsObjectPos(std::string id);
}