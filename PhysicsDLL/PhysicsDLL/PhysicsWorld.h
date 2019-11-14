#pragma once
#include <memory>
#include <vector>
#include <string>
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
	void AddParticle(std::string id, float invMass, float* pos);

	float* getParticlePos(std::string id);

	void AddForceToParticle(std::string id, float* force);

private: 
	std::vector<Particle3D> m_Particles;
	std::map<std::string, int> m_ParticleRegistry;
};