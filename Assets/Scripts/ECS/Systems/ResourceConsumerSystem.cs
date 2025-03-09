using Unity.Entities;

namespace ECS.Systems
{
    [UpdateAfter(typeof(ConnectionSystem))]
    public partial struct ResourceConsumerSystem : ISystem
    {
        // TODO
    }
}
