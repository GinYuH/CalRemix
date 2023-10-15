using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalRemix.Tiles.PlaguedJungle
{
	public class PlaguedCactus : ModCactus
	{
        
        public override void SetStaticDefaults()
        {
            // Grows on astral sand
            GrowsOnTileId = new int[1] { ModContent.TileType<PlaguedSand>() };
        }

        //Idk what to make with the glowmask
        public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("CalamityMod/Tiles/PlaguedJungle/PlaguedCactus");

        //What is a FruitTexture
        public override Asset<Texture2D> GetFruitTexture() => null;
    }
}