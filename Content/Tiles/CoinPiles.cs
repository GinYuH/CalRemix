using CalRemix.Content.Items.Ammo;
using CalRemix.Content.Projectiles.Weapons;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMod.Cil;

using Terraria;
using Terraria.Audio;
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

            IL_WorldGen.PlaceTile += il =>
            {
                var c = new ILCursor(il);

                while (c.TryGotoNext(x => x.MatchCall(typeof(SoundEngine), nameof(SoundEngine.PlaySound)))) { }

                c.GotoPrev(MoveType.After, x => x.MatchLdcI4(0));

                var pos = c.Index;

                var typeLoc = -1;
                c.GotoPrev(x => x.MatchLdloc(out typeLoc));

                c.Index = pos;
                c.EmitLdloc(typeLoc);
                c.EmitDelegate(
                    (int origId, int type) => TileLoader.GetTile(type) is CoinPile ? 18 : origId
                );
            };

            IL_WorldGen.KillTile_PlaySounds += il =>
            {
                /*var c = new ILCursor(il);

                c.GotoNext(MoveType.Before, x => x.MatchLdcI4(330));

                c.EmitDelegate(
                    (int type) => TileLoader.GetTile(type) is CoinPile ? TileID.PlatinumCoinPile : type
                );*/
                
                var c = new ILCursor(il);

                while (c.TryGotoNext(x => x.MatchCall(typeof(SoundEngine), nameof(SoundEngine.PlaySound)))) { }

                c.GotoPrev(MoveType.After, x => x.MatchLdcI4(0));

                var pos = c.Index;

                var typeLoc = -1;
                c.GotoPrev(x => x.MatchLdloc(out typeLoc));

                c.Index = pos;
                c.EmitLdloc(typeLoc);
                c.EmitDelegate(
                    (int origId, int type) => TileLoader.GetTile(type) is CoinPile ? 18 : origId
                );
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