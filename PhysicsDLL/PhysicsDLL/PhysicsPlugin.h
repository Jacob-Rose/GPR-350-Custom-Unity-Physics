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

	PHYSICS_NATIVE_PLUGIN_API void SetParticlePosX(const char* id, float pos);
	PHYSICS_NATIVE_PLUGIN_API void SetParticlePosY(const char* id, float pos);
	PHYSICS_NATIVE_PLUGIN_API void SetParticlePosZ(const char* id, float pos);

	PHYSICS_NATIVE_PLUGIN_API float GetParticlePosX(const char* id);
	PHYSICS_NATIVE_PLUGIN_API float GetParticlePosY(const char* id);
	PHYSICS_NATIVE_PLUGIN_API float GetParticlePosZ(const char* id);
	PHYSICS_NATIVE_PLUGIN_API void AddForceXToParticle(const char* id, float force);
	PHYSICS_NATIVE_PLUGIN_API void AddForceYToParticle(const char* id, float force);
	PHYSICS_NATIVE_PLUGIN_API void AddForceZToParticle(const char* id, float force);
}