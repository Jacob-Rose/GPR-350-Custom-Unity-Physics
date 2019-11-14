#include "PhysicsPlugin.h"

PHYSICS_NATIVE_PLUGIN_API void CreatePhysicsWorld()
{
	PhysicsWorld::initInstance();
}

PHYSICS_NATIVE_PLUGIN_API void DestroyPhysicsWorld()
{
	PhysicsWorld::cleanupInstance();
}

PHYSICS_NATIVE_PLUGIN_API void UpdatePhysicsWorld(float deltaTime)
{
	PhysicsWorld::m_Instance->Update(deltaTime);
}

PHYSICS_NATIVE_PLUGIN_API void AddParticle(std::string id, float invMass, float* pos)
{
	return PhysicsWorld::m_Instance->AddParticle(id, invMass, pos);
}

PHYSICS_NATIVE_PLUGIN_API float* GetPhysicsObjectPos(std::string id)
{
	return PhysicsWorld::m_Instance->getParticlePos(id);
}
