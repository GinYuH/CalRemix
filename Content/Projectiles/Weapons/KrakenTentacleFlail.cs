using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class KrakenTentacleFlail : BaseFlailProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 180);

            if (target.Distance(Main.player[Projectile.owner].Center) > 400)
            if (Projectile.localAI[2] == 0)
            {
                Main.player[Projectile.owner].velocity = Main.player[Projectile.owner].DirectionTo(target.Center) * 30;
                Main.player[Projectile.owner].Remix().krakenInvince = 60;
                    SoundStyle bling = new SoundStyle("CalamityMod/Sounds/Custom/Ultrabling") with { Pitch = 1f, Volume = 2 };
                    SoundEngine.PlaySound(bling, Main.player[Projectile.owner].Center);
                Projectile.localAI[2] = 1;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), damageDone * 20, 0, Projectile.owner, target.whoAmI);
            }
        }

        public override Color SpecialDrawColor => default;
        public override void GenerateDust()
        {
            
        }
    }
}
