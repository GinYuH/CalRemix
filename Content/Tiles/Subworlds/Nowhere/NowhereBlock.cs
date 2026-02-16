using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using System.ComponentModel.DataAnnotations;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Subworlds.Nowhere
{
    public class NowhereBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(125, 125, 125));
            HitSound = null;
            DustType = -1;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            float one = 224 / 255f;
            float two = 194 / 255f;
            Vector2 screenPos = new Vector2(i, j) * 16 - Main.screenPosition + CalamityUtils.TileDrawOffset;
            float comp = Utils.GetLerpValue((int)(Main.maxTilesY * 0.35f), (int)(Main.maxTilesY * 0.4f), j, true);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, screenPos, new Rectangle(0, 0, 16, 16), Color.Lerp(new Color(one, one, one), new Color(two, two, two), comp));
            return true;
        }
    }
}