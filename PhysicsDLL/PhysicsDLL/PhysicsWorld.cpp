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
	std::map<const char*, Particle3D*>::iterator it = m_ParticleRegistry.begin();
	while (it != m_ParticleRegistry.end())
	{
		it->second->Update(deltaTime);
		++it;
	}
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
	return m_ParticleRegistry[id]->m_Pos.y;
}
float PhysicsWorld::GetParticlePosZ(const char* id)
{
	return m_ParticleRegistry[id]->m_Pos.z;
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
	m_ParticleRegistry[id]->m_Force.y += force;
}
void PhysicsWorld::AddForceZToParticle(const char* id, float force)
{
	m_ParticleRegistry[id]->m_Force.z += force;
}

void Particle3D::reset()
{
	//set everything to zero
	m_Pos.reset();
	m_Velocity.reset();
	m_Acceleration.reset();
	m_Force.reset();
	m_InvMass = 0;
}

void Particle3D::Update(float DeltaTime)
{
	UpdateAcceleration();
	UpdatePositionKinematic(DeltaTime);
}

void Particle3D::UpdateAcceleration()
{
	m_Acceleration.x = m_Force.x * m_InvMass;
	m_Acceleration.y = m_Force.y * m_InvMass;
	m_Acceleration.z = m_Force.z * m_InvMass;
	m_Force.reset();
}

void Particle3D::UpdatePositionEuler(float DeltaTime)
{
	m_Pos.x += m_Velocity.x * DeltaTime;
	m_Pos.y += m_Velocity.y * DeltaTime;
	m_Pos.z += m_Velocity.z * DeltaTime;
	m_Velocity.x += m_Acceleration.x * DeltaTime;
	m_Velocity.y += m_Acceleration.y * DeltaTime;
	m_Velocity.z += m_Acceleration.z * DeltaTime;
}

void Particle3D::UpdatePositionKinematic(float DeltaTime)
{
	m_Pos.x += (m_Velocity.x * DeltaTime) + (0.5f * m_Acceleration.x * DeltaTime * DeltaTime);
	m_Pos.y += (m_Velocity.y * DeltaTime) + (0.5f * m_Acceleration.y * DeltaTime * DeltaTime);
	m_Pos.z += (m_Velocity.z * DeltaTime) + (0.5f * m_Acceleration.z * DeltaTime * DeltaTime);
	m_Velocity.x += m_Acceleration.x * DeltaTime;
	m_Velocity.y += m_Acceleration.y * DeltaTime;
	m_Velocity.z += m_Acceleration.z * DeltaTime;
}

void Vector3::reset()
{
	x = 0;
	y = 0;
	z = 0;
}
