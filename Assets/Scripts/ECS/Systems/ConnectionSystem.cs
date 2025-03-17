using ECS.Components;
using ECS.Systems.Jobs;
using Unity.Entities;

namespace ECS.Systems
{
    [UpdateAfter(typeof(ResourceProducerSystem))]
    [UpdateBefore(typeof(ResourceConsumerSystem))]
    public partial struct ConnectionSystem : ISystem
    {
        public void OnCreated(ref SystemState state)
        {
            state.RequireForUpdate<ConnectionComponent>();
        }

        public void OnUpdated(ref SystemState state)
        {
            state.Dependency = new ConnectionJob().ScheduleParallel(state.Dependency);
        }
    }
}
