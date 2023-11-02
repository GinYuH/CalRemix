using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod;

namespace CalRemix.Projectiles.Weapons
{
	public class TotalityEnergy : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Summon/CosmicEnergySpiral";
        public ref float Timer => ref Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exo Rammer");
        }

        public override void SetDefaults()
        {
            Projectile.width = 78;
            Projectile.height = 78;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.aiStyle = ProjAIStyleID.MiniTwins;
        }
        public override void AI()
        {
            Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, Main.DiscoR / 255f, Main.DiscoG / 255f, Main.DiscoB / 255f);
            Projectile.ChargingMinionAI(1200f, 1500f, 2400f, 150f, 0, 6f, 20f, 10f, new Vector2(0f, 0f), 30f, 12f, tileVision: true, ignoreTilesWhenCharging: true);
        }
        public override void PostAI()
        {
            Projectile.rotation += Projectile.velocity.X * 0.25f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 255);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}


