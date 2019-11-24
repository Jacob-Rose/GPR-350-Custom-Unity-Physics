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

float* PhysicsWorld::GetParticlePos(const char* id)
{
	return &m_ParticleRegistry[id]->m_Pos[0];
}

void PhysicsWorld::SetParticlePos(const char* id, float* pos)
{
	m_ParticleRegistry[id]->m_Pos[0] = pos[0];
	m_ParticleRegistry[id]->m_Pos[1] = pos[1];
	m_ParticleRegistry[id]->m_Pos[2] = pos[2];
}

void Particle3D::reset()
{
	//set everything to zero
	m_Pos[0] = 0;
	m_Pos[1] = 0;
	m_Pos[2] = 0;
	m_Velocity[0] = 0;
	m_Velocity[1] = 0;
	m_Velocity[2] = 0;
	m_Acceleration[0] = 0;
	m_Acceleration[1] = 0;
	m_Acceleration[2] = 0;
	m_Force[0] = 0;
	m_Force[1] = 0;
	m_Force[2] = 0;
	m_InvMass = 0;
}
