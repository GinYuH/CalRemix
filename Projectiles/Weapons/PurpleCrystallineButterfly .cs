using CalamityMod;
using CalamityMod.Projectiles.Summon;
using CalRemix.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
    public class PurpleCrystallineButterfly : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Pink Crystalline Butterfly");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 68;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 2f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft *= 5;
            Projectile.tileCollide = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.aiStyle = ProjAIStyleID.MiniTwins;
        }
        public override void AI()
        {
            Owner.AddBuff(ModContent.BuffType<TheDreamingGhostBuff>(), 3600);
            if (Projectile.type != ModContent.ProjectileType<PurpleCrystallineButterfly>())
                return;

            if (Owner.dead)
                Owner.GetModPlayer<CalRemixPlayer>().dreamingGhost = false;
            if (Owner.GetModPlayer<CalRemixPlayer>().dreamingGhost)
                Projectile.timeLeft = 2;
            Projectile.ChargingMinionAI(1200f, 1500f, 2400f, 150f, 0, 30f, 18f, 9f, new Vector2(0f, -60f), 30f, 12f, tileVision: true, ignoreTilesWhenCharging: true);
            if (Projectile.ai[1] >= 30f)
            {
                NPC npc = Projectile.Center.MinionHoming(550f, Main.player[Projectile.owner]);
                if (npc != null)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.DirectionTo(npc.Center) * 20f, ModContent.ProjectileType<SakuraBullet>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
                }
            }
            Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 3)
            {
                Projectile.frame = 0;
            }
        }
    }
}
