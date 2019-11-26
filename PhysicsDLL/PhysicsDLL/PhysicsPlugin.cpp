#include "PhysicsPlugin.h"

PHYSICS_NATIVE_PLUGIN_API void CreatePhysicsWorld()
{
	PhysicsWorld::initInstance();
	return;
}

PHYSICS_NATIVE_PLUGIN_API void DestroyPhysicsWorld()
{
	PhysicsWorld::cleanupInstance();
	return;
}

PHYSICS_NATIVE_PLUGIN_API void UpdatePhysicsWorld(float deltaTime)
{
	PhysicsWorld::m_Instance->Update(deltaTime);
	return;
}

PHYSICS_NATIVE_PLUGIN_API void AddParticle(const char* id, float invMass)
{
	PhysicsWorld::m_Instance->AddParticle(id, invMass);
	return;
}

PHYSICS_NATIVE_PLUGIN_API void RemoveParticle(const char* id)
{
	PhysicsWorld::m_Instance->RemoveParticle(id);
	return;
}

PHYSICS_NATIVE_PLUGIN_API void SetParticlePosX(const char* id, float pos)
{
	PhysicsWorld::m_Instance->SetParticlePosX(id, pos);
	return;
}
PHYSICS_NATIVE_PLUGIN_API void SetParticlePosY(const char* id, float pos)
{
	PhysicsWorld::m_Instance->SetParticlePosY(id, pos);
	return;
}
PHYSICS_NATIVE_PLUGIN_API void SetParticlePosZ(const char* id, float pos)
{
	PhysicsWorld::m_Instance->SetParticlePosZ(id, pos);
	return;
}

PHYSICS_NATIVE_PLUGIN_API float GetParticlePosX(const char* id)
{
	return PhysicsWorld::m_Instance->GetParticlePosX(id);
}
PHYSICS_NATIVE_PLUGIN_API float GetParticlePosY(const char* id)
{
	return PhysicsWorld::m_Instance->GetParticlePosY(id);
}
PHYSICS_NATIVE_PLUGIN_API float GetParticlePosZ(const char* id)
{
	return PhysicsWorld::m_Instance->GetParticlePosZ(id);
}

PHYSICS_NATIVE_PLUGIN_API void AddForceXToParticle(const char* id, float force)
{
	PhysicsWorld::m_Instance->AddForceXToParticle(id, force);
	return;
}
PHYSICS_NATIVE_PLUGIN_API void AddForceYToParticle(const char* id, float force)
{
	PhysicsWorld::m_Instance->AddForceYToParticle(id, force);
	return;
}
PHYSICS_NATIVE_PLUGIN_API void AddForceZToParticle(const char* id, float force)
{
	PhysicsWorld::m_Instance->AddForceZToParticle(id, force);
	return;
}
