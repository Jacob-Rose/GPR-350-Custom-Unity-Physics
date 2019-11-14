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
		m_Particles.push_back(Particle3D());
	}
}

void PhysicsWorld::cleanup()
{

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

void PhysicsWorld::AddForceToParticle(std::string id, float* force)
{
}

void PhysicsWorld::AddParticle(std::string id, float invMass, float* pos)
{
	m_ParticleRegistry.insert(std::pair<std::string, int>(id, m_ParticleRegistry.size()));
	m_Particles.at(m_ParticleRegistry[id]).reset();
	m_Particles.at(m_ParticleRegistry[id]).m_Pos.x = pos[0];
	m_Particles.at(m_ParticleRegistry[id]).m_Pos.y = pos[1];
	m_Particles.at(m_ParticleRegistry[id]).m_Pos.z = pos[2];
	m_Particles.at(m_ParticleRegistry[id]).m_InvMass = invMass;
}

float* PhysicsWorld::getParticlePos(std::string id)
{
	return &m_Particles[m_ParticleRegistry[id]].m_Pos.x;
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
