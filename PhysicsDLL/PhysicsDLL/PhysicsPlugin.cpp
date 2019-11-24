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

PHYSICS_NATIVE_PLUGIN_API void SetParticlePos(const char* id, float* pos)
{
	PhysicsWorld::m_Instance->SetParticlePos(id, pos);
	return;
}

PHYSICS_NATIVE_PLUGIN_API float* GetParticlePos(const char* id)
{
	return PhysicsWorld::m_Instance->GetParticlePos(id);
}
