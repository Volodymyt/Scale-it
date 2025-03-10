# Terms

## Entities

- `Map` - game world, structured layout of connected cards
- `Card` - game card, individual production or processing unit
- `Deck` - card collection. Allowed use `Collection` instead
- `Connection` - link between cards for resource transfer
- ...

## Mechanics

- `Ticks`
  - `Update` (`Game Tick`) - runtime/realtime updates, default in unity. In our case we use it for UI and to display the state of game objects. NOT FOR RECALCULATION OF PRODUCTION STATE
    - `UPS` (`Update per Second`) - update unity objects for the render
    - `FPS` (`Frame per Second`) - not for us)
  - `ProductionChainSystem` (`Production Chain System`) - parallel process (C# jobs/DOTS) for recalculate game state
    - `ProductionTick` - single tick for recalculate production
    - `TPS` (`Tick per Second`) - game speed
    - `Pause` - game state when TPS = 0, not blocked unity updates and user interactions

IMPORTANT! In current project UPS must be equal FPS to ensure that the response to user actions occurs without delay

---

### Cards Types

- `Card` / `AbsractCard` - default game card
  - `ResourceCard` - card that generates or stores resources
  - `ToolCard` - card representing a production tool or machine  
  - `ModifierCard` - card that enhances or alters other cards' functionality 

### Card inner

- `ResourceInput: <ResourceCard>`
- `ModifierInput: <ModifierCard>`
- `ResourceOutput: <ResourceCard>`
- `ModifierOutput: <ModifierCard>`