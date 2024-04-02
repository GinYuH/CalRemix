using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace CalRemix
{
    public class CalRemixBuff : GlobalBuff
    {
        public override bool PreDraw(SpriteBatch spriteBatch, int type, int buffIndex, ref BuffDrawParams drawParams)
        {
            return base.PreDraw(spriteBatch, type, buffIndex, ref drawParams);
        }
    }
}