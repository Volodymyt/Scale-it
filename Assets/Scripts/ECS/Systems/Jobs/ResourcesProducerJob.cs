using System;
using System.Collections.Generic;
using System.Linq;
using ECS.Components;
using ECS.Enums;
using Unity.Entities;

namespace ECS.Systems.Jobs
{
    public partial struct ResourcesProducerJob : IJobEntity
    {
        public ComponentLookup<ConnectionComponent> ConnectionComponents;
        public BufferLookup<OutputConnectionElement> OutputConnections;

        delegate void SplitStrategy(ref CardComponent card, ref DynamicBuffer<OutputConnectionElement> outputBuffer);

        public void Execute(Entity entity, ref CardComponent card)
        {
            if (!OutputConnections.HasBuffer(entity)) return;

            var outputBuffer = OutputConnections[entity];

            var strategyMap = new Dictionary<string, SplitStrategy>
            {
                { "exclusive", ExclusiveStrategy },
            };

            var strategyName = card.Tags["split_strategy"]?.First() ?? "exclusive";
            if (strategyMap.TryGetValue(strategyName, out SplitStrategy strategy))
            {
                strategy(ref card, ref outputBuffer);
            }
            else
            {
                UnityEngine.Debug.Log($"Split strategy {strategyName} not implemented! Card id: {card.CardID}");
                return;
            }
        }

        private void ExclusiveStrategy(ref CardComponent card, ref DynamicBuffer<OutputConnectionElement> outputBuffer)
        {
            foreach (var outputElement in outputBuffer)
            {
                if (!ConnectionComponents.HasComponent(outputElement.ConnectionEntity))
                {
                    UnityEngine.Debug.Log("Entity has incorrect connection in buffer");
                    continue;
                }

                var output = ConnectionComponents[outputElement.ConnectionEntity];

                WriteOffLocked(ref card, ref output);
                RecalculateOutputAmount(ref card, ref output);

                ConnectionComponents[outputElement.ConnectionEntity] = output;
            }
        }

        private static void WriteOffLocked(ref CardComponent card, ref ConnectionComponent output)
        {
            if (output.OutputLinkState != ConnectionLinkState.Sent) return;

            card.ResourcesLock[output.OutputResourceID] -= output.InputReceived;
            output.InputReceived = 0;
            output.OutputLinkState = ConnectionLinkState.NotReadyToSend;
        }

        private static void RecalculateOutputAmount(ref CardComponent card, ref ConnectionComponent output)
        {
            if (output.OutputLinkState != ConnectionLinkState.NotReadyToSend &&
                output.OutputLinkState != ConnectionLinkState.ReadyToSend)
                return;

            var need = Math.Min(
                card.ResourcesStored[output.OutputResourceID],
                Math.Max(0, output.InputCapacity) - output.OutputAmount
            );

            card.ResourcesStored[output.OutputResourceID] =
                Math.Max(0, card.ResourcesStored[output.OutputResourceID] - need);
            card.ResourcesLock[output.OutputResourceID] =
                Math.Max(0, card.ResourcesLock[output.OutputResourceID] + need);
            output.OutputAmount = Math.Max(0, output.OutputAmount + need);

            output.OutputLinkState = (output.OutputAmount > 0)
                ? ConnectionLinkState.ReadyToSend
                : ConnectionLinkState.NotReadyToSend;
        }
    }
}