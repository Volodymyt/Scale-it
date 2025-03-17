using ECS.Components;
using ECS.Systems.Jobs;
using Unity.Entities;

namespace ECS.Systems
{
    [UpdateBefore(typeof(ConnectionSystem))]
    [UpdateBefore(typeof(ResourceConsumerSystem))]
    public partial struct ResourceProducerSystem : ISystem
    {
        public void OnCreated(ref SystemState state)
        {
            state.RequireForUpdate<CardComponent>();
        }

        public void OnUpdated(ref SystemState state)
        {
            state.Dependency = new ResourcesProducerJob
            {
                ConnectionComponents = SystemAPI.GetComponentLookup<ConnectionComponent>(),
                OutputConnections = SystemAPI.GetBufferLookup<OutputConnectionElement>()
            }.ScheduleParallel(state.Dependency);
        }
    }
}
