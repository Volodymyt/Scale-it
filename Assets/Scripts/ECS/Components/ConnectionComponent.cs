using ECS.Enums;
using Unity.Entities;

namespace ECS.Components
{
    public struct ConnectionComponent : IComponentData
    {
        // public Entity OutputEntity;
        public ConnectionLinkState OutputLinkState;
        public int OutputResourceID;
        public int OutputAmount;

        // public Entity InputEntity;
        public ConnectionLinkState InputLinkState;
        public int InputCapacity;
        public int InputReceived;


        public bool IsReadyToTransfer()
        {
            return OutputLinkState == ConnectionLinkState.ReadyToSend
                   && InputLinkState == ConnectionLinkState.ReadyToReceive;
        }
    }

    [InternalBufferCapacity(4)]
    public struct OutputConnectionElement : IBufferElementData
    {
        public Entity ConnectionEntity;
    }

    [InternalBufferCapacity(4)]
    public struct InputConnectionElement : IBufferElementData
    {
        public Entity ConnectionEntity;
    }

    [InternalBufferCapacity(2)]
    public struct ModifierConnectionElement : IBufferElementData
    {
        public Entity ConnectionEntity;
    }
}
