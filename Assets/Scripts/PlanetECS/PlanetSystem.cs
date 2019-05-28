using Unity.Collections;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


namespace PlanetECS
{
    public class PlanetSystem : JobComponentSystem
    {
        private const float gravityConst = 1.0f;
        private const float centerMass = 1000.0f;
        
        
        [BurstCompile]
        struct PlanetJob : IJobProcessComponentData<Translation, PlanetData>
        {
            [ReadOnly] public float dt;
            public void Execute(ref Translation pos, ref PlanetData planetData)
            {
                float3 p = pos.Value;

                var newForce = CalculateNewForce(p, planetData.mass);

                planetData.velocity = planetData.velocity + newForce / planetData.mass * dt;
                pos.Value = pos.Value + planetData.velocity * dt;

            }
        }
        
        public static float3 CalculateNewForce(float3 position, float planetMass) 
        {
            var deltaToCenter = -position;
            var r = math.length(deltaToCenter);
            var force = gravityConst * centerMass * planetMass / (r*r);
            return deltaToCenter / r * force;
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new PlanetJob()
            {
                dt = Time.deltaTime
            };

            return job.Schedule(this, inputDeps);
        }
        
    }
}