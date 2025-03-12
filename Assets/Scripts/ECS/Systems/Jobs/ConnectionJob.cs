using System;
using ECS.Components;
using ECS.Enums;
using Unity.Entities;

namespace ECS.Systems.Jobs
{
    public partial struct ConnectionJob : IJobEntity
    {
        public static void Execute(ref ConnectionComponent connection)
        {
            if (!connection.IsReadyToTransfer()) return;
            
            connection.InputReceived = Math.Min(connection.InputCapacity, connection.OutputAmount);
            connection.InputLinkState = ConnectionLinkState.Received;

            connection.OutputAmount -= connection.InputReceived;
            connection.OutputLinkState = ConnectionLinkState.Sent;
        }
    }
}
