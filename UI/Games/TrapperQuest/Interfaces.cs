using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.GameContent;
using CalamityMod.Systems;

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