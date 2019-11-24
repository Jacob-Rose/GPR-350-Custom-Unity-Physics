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
	float m_Pos[3];
	float m_Velocity[3];
	float m_Acceleration[3];
	float m_Force[3];
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
	
	float* GetParticlePos(const char* id);
	void SetParticlePos(const char* id, float* pos);

	//void AddForceToParticle(std::string id, float* force);

private: 
	std::vector<Particle3D*> m_ParticlePool;
	std::map<const char*, Particle3D*> m_ParticleRegistry;
};