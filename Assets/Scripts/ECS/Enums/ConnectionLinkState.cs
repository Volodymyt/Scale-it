namespace ECS.Enums
{
    // For correct save game force set value of enums
    public enum ConnectionLinkState
    {
        // Connection locked by some system or process
        Busy = 0,
        
        // Output link states
        NotReadyToSend = 10,
        ReadyToSend = 11,
        Sent = 12,
        
        // Input link states
        NotReadyToReceive = 20,
        ReadyToReceive = 21,
        Received = 22,
    }
}
