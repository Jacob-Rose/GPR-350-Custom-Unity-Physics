#pragma once
#include <memory>
#include <vector>
#include <map>

struct Vector3
{
	float x;
	float y;
	float z;
};

struct Particle3D
{
public:
	Vector3 m_Pos;
	Vector3 m_Velocity;
	Vector3 m_Acceleration;
	Vector3 m_Force;
	float m_InvMass;

	void reset();
};

class PhysicsWorld
{
public:
	static std::unique_ptr<PhysicsWorld> m_Instance;
	static void initInstance();
	static void cleanupInstance();

	explicit PhysicsWorld();
	~PhysicsWorld();

	void init();
	void cleanup();

	void Update(float deltaTime);

	/*Pos is a float array size 3*/
	void AddParticle(const char* id, float invMass);
	void RemoveParticle(const char* id);
	
	float GetParticlePosX(const char* id);
	float GetParticlePosY(const char* id);
	float GetParticlePosZ(const char* id);

	void SetParticlePosX(const char* id, float pos);
	void SetParticlePosY(const char* id, float pos);
	void SetParticlePosZ(const char* id, float pos);

	void AddForceXToParticle(const char* id, float force);
	void AddForceYToParticle(const char* id, float force);
	void AddForceZToParticle(const char* id, float force);

private: 
	std::vector<Particle3D*> m_ParticlePool;
	std::map<const char*, Particle3D*> m_ParticleRegistry;
};