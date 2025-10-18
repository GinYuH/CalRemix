using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Buffs.DamageOverTime;

namespace CalRemix.Content.NPCs.Bosses.BossChanges.SupremeCalamitas
{
    public class SupremeSickle : ModProjectile
    {
        public ref float IdealPosX => ref Projectile.ai[0];
        public ref float IdealPosY => ref Projectile.ai[1];
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 300;
        }
        public override void AI()
        {
            Projectile.rotation += 0.75f;
            Projectile.velocity += new Vector2(IdealPosX, IdealPosY) * 0.5f;

            // from brimstone hellblast
            Lighting.AddLight(Projectile.Center, 0.9f * Projectile.Opacity, 0f, 0f);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 120);
        }
        public override void OnKill(int timeLeft)
        {
            /*for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f);
                d.velocity = new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
            }*/
        }
        public override bool PreDraw(ref Color lightColor)
        {
            // from brimstone hellblast
            lightColor.R = (byte)(255 * Projectile.Opacity);
            return true;
        }
    }
}
