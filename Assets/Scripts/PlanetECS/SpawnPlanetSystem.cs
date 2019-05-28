using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace PlanetECS
{
    public class SpawnPlanetSystem : ComponentSystem
    {
       
        
        EntityQuery m_Spawners;

        protected override void OnCreateManager()
        {
            m_Spawners = GetEntityQuery(typeof(SpawnPlanetData));
        }
        static void GenerateRandomPlanet(float maxRadius, 
        ref NativeArray<float3> positions, ref NativeArray<float3> velocities, 
        ref NativeArray<float> planetMasses)
        {
            var count = positions.Length;
            var radiusSquared = maxRadius * maxRadius;
            for (int i = 0; i < count; i++)
            {
                var p = new float3
                {
                    x = UnityEngine.Random.Range(-maxRadius, maxRadius),
                    y = 0.0f,
                    z = UnityEngine.Random.Range(-maxRadius, maxRadius)
                };

                planetMasses[i] = UnityEngine.Random.Range(0.5f, 1.5f);
                if (math.lengthsq(p) < radiusSquared)
                {
                    positions[i] = p;
                }
                else
                {
                    positions[i] = p / math.length(p) * maxRadius;
                }

                velocities[i] = CalculateInitSpeed(positions[i], planetMasses[i]);
            }
        }
        static float3 CalculateInitSpeed(float3 position, float planetMass) 
        {
            var deltaToCenter = - position;
            var velDir = new float3(-deltaToCenter.z, 0.0f, deltaToCenter.x);
            velDir /= math.length(velDir);
            var newForce = PlanetSystem.CalculateNewForce(position, planetMass);
            var speed = math.sqrt(math.length(newForce) / planetMass * 
                                  math.length(deltaToCenter));
            speed *= UnityEngine.Random.Range(0.8f, 1.2f);
            return new float3(velDir.x*speed, 0.0f,velDir.z*speed);
        }
        
        
        protected override void OnUpdate()
        {
            using (var spawners = m_Spawners.ToEntityArray(Allocator.TempJob))
            {
                foreach (var spawner in spawners)
                {
                    SpawnPlanetData spawnData = EntityManager.GetSharedComponentData<SpawnPlanetData>
                        (spawner);
                    // Create an entity from the prefab set on the spawner component.
                    var prefab = spawnData.prefab;
                    NativeArray<float3> positions = new NativeArray<float3>(spawnData.count, Allocator.Temp);
                    NativeArray<float3> velocities = new NativeArray<float3>(spawnData.count, Allocator.Temp);
                    NativeArray<float> masses = new NativeArray<float>(spawnData.count, Allocator.Temp);
                    
                    GenerateRandomPlanet(spawnData.maxRadius, ref positions, ref velocities, ref masses);
                    
                    for (int i = 0; i < spawnData.count; i++)
                    {
                        var entity = EntityManager.Instantiate(prefab);
                        var position = new Translation()
                        {
                            Value =  positions[i]
                        };
                        EntityManager.SetComponentData(entity, position);
                        
                        var planetData = new PlanetData()
                        {
                            velocity = velocities[i],
                            mass = masses[i]
                        };
                        EntityManager.SetComponentData(entity, planetData);
                        
                    }
                    positions.Dispose();
                    velocities.Dispose();
                    masses.Dispose();

                    // Destroy the spawner so this system only runs once.
                    EntityManager.DestroyEntity(spawner);
                }
            }
            
        }
    }
}