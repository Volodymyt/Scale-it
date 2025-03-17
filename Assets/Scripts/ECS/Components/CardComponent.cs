using Common.Collection;
using DeckManager;
using ECS.Enums;
using Unity.Collections;
using Unity.Entities;

namespace ECS.Components
{
    public struct CardComponent : IComponentData
    {
        public int CardID;
        public int RecipeID;

        public float ProductionState;

        [NativeDisableParallelForRestriction]
        public NativeHashMap<int, int> ResourcesStored;
        
        [NativeDisableParallelForRestriction]
        public NativeHashMap<int, int> ResourcesLock;

        public Card Card => DeckLoader.Instance.Deck.cards[CardID];
        public TagDictionary Tags => DeckLoader.Instance.Deck.cards[CardID].tags;

        public Recipe ActiveRecipe => DeckLoader.Instance.Deck.cards[CardID].recipes[RecipeID];
    }
}
