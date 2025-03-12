using System.Collections.Generic;
using System.Linq;

namespace DeckManager
{
    [System.Serializable]
    public record Deck
    {
        public DeckMeta meta;
        public List<Card> cards;
    }

    [System.Serializable]
    public record DeckMeta
    {
        public string name;
        public string version;
    }

    [System.Serializable]
    public record Card
    {
        public int cardID;
        public string name;
        public List<string> tags;
        public int stackSize;
        public List<Recipe> recipes;

        public int ModifierCount => !recipes.Any() ? 0 : recipes.First().modifiers.Count();
    }


    [System.Serializable]
    public record Recipe
    {
        public int recipeID;
        public string name;
        public int ticks = 1;
        public List<Modifier> modifiers;
        public List<Input> inputs;
        public List<Output> outputs;
    }

    [System.Serializable]
    public record Input
    {
        public int link;
        public int resourceID;
        public List<string> resourceTags;
        public float ratio = 1f;
        public int storage;
    }

    [System.Serializable]
    public record Output
    {
        public int link;
        public int resourceID;
        public List<string> resourceTags;
        public float ratio = 1f;
        public int storage;
    }

    [System.Serializable]
    public record Modifier
    {
        public int link;
        public int resourceID;
        public List<string> resourceTags;
        public bool required;
        public List<Affect> affects;
    }

    [System.Serializable]
    public record Affect
    {
        public int output;
        public float ratio;
    }
}
