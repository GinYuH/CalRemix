using CalRemix.Content.Items.Ammo;
using CalRemix.Content.Tiles;

using MonoMod.Cil;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons;

public abstract class CoinPileProjectile : ModProjectile
{
    private sealed class DontLikeThis : ILoadable
    {
        public void Load(Mod mod)
        {
            IL_Projectile.HandleMovement += il =>
            {
                var c = new ILCursor(il);

                c.GotoNext(MoveType.Before, x => x.MatchLdcI4(ProjectileID.PlatinumCoinsFalling));
                c.EmitDelegate(
                    (int type) => ProjectileLoader.GetProjectile(type) is CoinPileProjectile ? ProjectileID.PlatinumCoinsFalling : type
                );
            };
        }

        public void Unload() { }
    }

    protected abstract int CoinItem { get; }

    protected abstract int CoinPile { get; }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        ProjectileID.Sets.FallingBlockTileItem[Type] = new ProjectileID.Sets.FallingBlockTileItemInfo(CoinPile, CoinItem);
        ProjectileID.Sets.FallingBlockDoesNotFallThroughPlatforms[Type] = true;
        ProjectileID.Sets.ForcePlateDetection[Type] = true;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Projectile.CloneDefaults(ProjectileID.PlatinumCoinsFalling);
    }

    public override void AI()
    {
        base.AI();

        // TODO: Dust
        // if (rand(3))
        // Dust.NewDust(new Vector2(position.X, position.Y), width, height, num3, 0f, velocity.Y / 2f);
        // noGravity = true;
        // velocity -= velocity * 0.5f;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        // TODO: also dust
    }
}

internal sealed class CosmiliteCoinPileProjectile : CoinPileProjectile
{
    protected override int CoinItem => ModContent.ItemType<CosmiliteCoin>();

    protected override int CoinPile => ModContent.TileType<CosmiliteCoinPlaced>();
}

internal sealed class KlepticoinPileProjectile : CoinPileProjectile
{
    protected override int CoinItem => ModContent.ItemType<Klepticoin>();

    protected override int CoinPile => ModContent.TileType<KlepticoinPlaced>();
}

public abstract class CoinProjectile : ModProjectile
{
    public override void SetDefaults()
    {
        base.SetDefaults();

        Projectile.CloneDefaults(ProjectileID.PlatinumCoin);
        AIType = ProjectileID.PlatinumCoin;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        /*for (int num558 = 0; num558 < 10; num558++) {
            int num559 = Dust.NewDust(new Vector2(position.X, position.Y), width, height, 11);
            Main.dust[num559].noGravity = true;
            Dust dust2 = Main.dust[num559];
            dust2.velocity -= velocity * 0.5f;
        }*/
    }
}

internal sealed class CosmiliteCoinProjectile : CoinProjectile { }

internal sealed class KlepticoinProjectile : CoinProjectile { }