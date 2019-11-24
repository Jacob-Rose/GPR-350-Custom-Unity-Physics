#pragma once

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

	PHYSICS_NATIVE_PLUGIN_API void AddParticle(const char* id, float invMass);
	PHYSICS_NATIVE_PLUGIN_API void RemoveParticle(const char* id);

	PHYSICS_NATIVE_PLUGIN_API void SetParticlePos(const char* id, float* pos);
	PHYSICS_NATIVE_PLUGIN_API float* GetParticlePos(const char* id);
}