using CalRemix.Content.Items.Ammo;
using CalRemix.Content.Projectiles.Weapons;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles;

public abstract class CoinPile : ModTile
{
    private sealed class OnlyCallMeOnce : ILoadable
    {
        public void Load(Mod mod)
        {
            On_TileDrawing.GetTileDrawData += (
                On_TileDrawing.orig_GetTileDrawData orig,
                TileDrawing self,
                int i,
                int i1,
                Tile cache,
                ushort typeCache,
                ref short x,
                ref short y,
                out int width,
                out int height,
                out int top,
                out int brickHeight,
                out int frX,
                out int frY,
                out SpriteEffects effect,
                out Texture2D texture,
                out Rectangle rect,
                out Color color
            ) =>
            {
                orig(self, i, i1, cache, typeCache, ref x, ref y, out width, out height, out top, out brickHeight, out frX, out frY, out effect, out texture, out rect, out color);

                if (TileLoader.GetTile(typeCache) is CoinPile)
                {
                    top += 2;
                }
            };
        }

        public void Unload() { }
    }

    protected abstract int CoinItem { get; }

    protected abstract int PilesProjectile { get; }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        TileID.Sets.Falling[Type] = true;
        TileID.Sets.FallingBlockProjectile[Type] = new TileID.Sets.FallingBlockProjectileInfo(PilesProjectile);
        TileID.Sets.CanPlaceNextToNonSolidTile[Type] = true;

        Main.tileNoFail[Type] = true;
        Main.tilePile[Type] = true;
    }
}

internal sealed class CosmiliteCoinPlaced : CoinPile
{
    protected override int CoinItem => ModContent.ItemType<CosmiliteCoin>();

    protected override int PilesProjectile => ModContent.ProjectileType<CosmiliteCoinPileProjectile>();
}

internal sealed class KlepticoinPlaced : CoinPile
{
    protected override int CoinItem => ModContent.ItemType<CosmiliteCoin>();

    protected override int PilesProjectile => ModContent.ProjectileType<KlepticoinPileProjectile>();
}