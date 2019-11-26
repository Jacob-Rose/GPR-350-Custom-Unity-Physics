#include "PhysicsWorld.h"

std::unique_ptr<PhysicsWorld> PhysicsWorld::m_Instance = nullptr;

PhysicsWorld::PhysicsWorld()
{
	init();
}

PhysicsWorld::~PhysicsWorld()
{
	cleanup();
}

void PhysicsWorld::init()
{
	for (int i = 0; i < 1024; ++i)
	{
		m_ParticlePool.push_back(new Particle3D()); //they are dynamic because they transfer arrays
	}
}

void PhysicsWorld::cleanup()
{
	for (int i = 0; i < m_ParticlePool.size(); ++i)
	{
		delete m_ParticlePool.at(i);
	}
	std::map<const char*, Particle3D*>::iterator it = m_ParticleRegistry.begin();
	while (it != m_ParticleRegistry.end())
	{
		delete it->second;
		++it;
	}
}

void PhysicsWorld::initInstance()
{
	m_Instance.reset(new PhysicsWorld());
}

void PhysicsWorld::cleanupInstance()
{
	m_Instance.reset(nullptr);
}

void PhysicsWorld::Update(float deltaTime)
{
}

void PhysicsWorld::AddParticle(const char* id, float invMass)
{
	m_ParticleRegistry.insert(std::pair<const char*, Particle3D*>(id, m_ParticlePool[0]));
	m_ParticlePool[0]->reset();
	m_ParticlePool[0]->m_InvMass = invMass;
	m_ParticlePool.erase(m_ParticlePool.begin());
}

void PhysicsWorld::RemoveParticle(const char* id)
{
	std::map<const char*, Particle3D*>::iterator it = m_ParticleRegistry.find(id);
	if (it != m_ParticleRegistry.end())
	{
		m_ParticlePool.push_back(it->second);
		it->second->reset();
		m_ParticleRegistry.erase(it);
	}
}

float PhysicsWorld::GetParticlePosX(const char* id)
{
	return m_ParticleRegistry[id]->m_Pos.x;
}
float PhysicsWorld::GetParticlePosY(const char* id)
{
	return m_ParticleRegistry[id]->m_Pos.x;
}
float PhysicsWorld::GetParticlePosZ(const char* id)
{
	return m_ParticleRegistry[id]->m_Pos.x;
}

void PhysicsWorld::SetParticlePosX(const char* id, float pos)
{
	m_ParticleRegistry[id]->m_Pos.x = pos;
}
void PhysicsWorld::SetParticlePosY(const char* id, float pos)
{
	m_ParticleRegistry[id]->m_Pos.y = pos;
}
void PhysicsWorld::SetParticlePosZ(const char* id, float pos)
{
	m_ParticleRegistry[id]->m_Pos.z = pos;
}

void PhysicsWorld::AddForceXToParticle(const char* id, float force)
{
	m_ParticleRegistry[id]->m_Force.x += force;
}
void PhysicsWorld::AddForceYToParticle(const char* id, float force)
{
	m_ParticleRegistry[id]->m_Force.x += force;
}
void PhysicsWorld::AddForceZToParticle(const char* id, float force)
{
	m_ParticleRegistry[id]->m_Force.z += force;
}



void Particle3D::reset()
{
	//set everything to zero
	m_Pos.x = 0;
	m_Pos.y = 0;
	m_Pos.z = 0;
	m_Velocity.x = 0;
	m_Velocity.y = 0;
	m_Velocity.z = 0;
	m_Acceleration.x = 0;
	m_Acceleration.y = 0;
	m_Acceleration.z = 0;
	m_Force.x = 0;
	m_Force.y = 0;
	m_Force.z = 0;
	m_InvMass = 0;
}
