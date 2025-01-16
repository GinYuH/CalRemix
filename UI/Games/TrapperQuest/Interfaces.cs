using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CalRemix.UI.Games.TrapperQuest
{
    /// <summary>
    /// Used for any entity that should be available in the level editor
    /// </summary>
    public interface ICreative
    {
        /// <summary>
        /// The ID of the entity
        /// </summary>
        public int ID => -1;

        /// <summary>
        /// How it should draw in the level editor UI
        /// </summary>
        public void DrawCreative(SpriteBatch sb, Vector2 pos)
        {
        }

        /// <summary>
        /// The name that should appear
        /// </summary>
        public string Name => "";

        /// <summary>
        /// Options specific to this entity
        /// </summary>
        public List<EntityOption> EditorOptions => new List<EntityOption>();
    }
    public interface ITile
    {
    }

    public class TrapperTile : GameEntity, ITile
    {
        public int tileX;
        public int tileY;

        public TrapperTile Clone()
        {
            TrapperTile clone = base.Clone() as TrapperTile;
            clone.tileX = tileX;
            clone.tileY = tileY;
            return clone;
        }
    }
}