using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Buffs;
using CalamityMod.Projectiles.Rogue;
using CalamityMod;
using Terraria.Audio;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class PineappleSentry : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Pets/PineapplePetProj";
        public ref float Timer => ref Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Animated Statue");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 64;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.sentry = true;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
                Projectile.frameCounter = 0;
            }
            NPC npc = CalamityUtils.MinionHoming(Projectile.Center, 6400, Main.player[Projectile.owner]);
            if (npc != null)
            {
                if (npc.active && npc.life > 0)
                {
                    Timer++;
                    if (Timer > 20)
                    {
                        SoundEngine.PlaySound(BetterSoundID.ItemMissileFireSqueak, Projectile.Center);
                        for (int i = 1; i < 3; i++)
                        {
                            Projectile proj = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.DirectionTo(npc.Center) * 10 + Main.rand.NextVector2Circular(4, 4), ProjectileID.SeedlerNut, (int)(Projectile.damage * 0.25f), Projectile.knockBack, Projectile.owner)];
                            proj.DamageType = DamageClass.Summon;
                        }
                        Timer = 0;
                    }
                }
            }
        }

        public override bool? CanDamage() => false;
    }
}
