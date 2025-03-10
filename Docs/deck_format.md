## Tags

### Card Types

- `resource`
- `modifier`
- `splitter`
  - `split_modifier` - split modifier to multiple connection
  - `split_strategy:*` - strategy to split resources
    - `split_strategy:one_by_one` - share with all the output in turn, only one output per tick
    - `split_strategy:equally` - split equally between outputs
- `tool` - 

### 

- `self_disposing` - not required to utilization
- `storable` - allow to store in containers
  - `solid`
  - `bulk`

- `playable` - allowed move to player inventory as card  


# Calculation

## Consumption



## Production ratio
```
Output[Link] = (Modifier.Affects[Link] ? Modifier.Affects[Link] * Modifier) : 1) * Output[Link].Ratio;
```

### Example

```
card.worker
  -> Output[0] = ResourceCardID:20100 (modifier.handwork) with Ratio: 100
    | connection to modifier
card.woodcutter_axe
  -> Output[0] = ResourceCardID:20210 (modifier.chopping) with Ratio: 1 -> (modifier 100 * link ratio 0.5) * 1 = 50 per tick
    | connection to modifier
card.forestry
  -> Output[0] = ResourceID:10101 (resource.wood_logs) with Ratio: 1 -> (modifier 50 * link ratio 0.04) * 1 = 2 per tick
  -> Output[1] = ResourceID:10101 (resource.wood_waste) with Ratio 3 -> (modifier 50 * link ratio 0.01) * 3 = 1.5 per tick
```

Change axe to chainsaw

```
card.worker
  -> Output[0] = ResourceCardID:20100 (modifier.handwork) with Ratio: 100
    | connection to modifier
card.chainsaw
  -> Output[0] =  ResourceCardID:20210 (modifier.chopping) with Ratio: 1 -> (modifier 100 * link ratio 3) * 1 = 300 per tick
    | connection to modifier
card.forestry
  -> Output[0] = ResourceID:10101 (resource.wood_logs) with Ratio: 1 -> (modifier 300 * link ratio 0.04) * 1 = 6 per tick
  -> Output[0] = ResourceID:10101 (resource.wood_waste) with Ratio 3 -> (modifier 300 * link ratio 0.01) * 3 = 9 per tick
```

Important: we see a non-linear dependence of the output size on the modifier.
In this case, more work means even more waste per unit of output.