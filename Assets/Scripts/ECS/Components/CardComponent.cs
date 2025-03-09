using ECS.Enums;
using Unity.Collections;
using Unity.Entities;

namespace ECS.Components
{
    public struct CardComponent : IComponentData
    {
        public int CardID;
        public CardType CardType;

        public float ProductionState;

        [NativeDisableParallelForRestriction]
        public NativeHashMap<int, int> ResourcesStored;
        
        [NativeDisableParallelForRestriction]
        public NativeHashMap<int, int> ResourcesLock;
    }
}
