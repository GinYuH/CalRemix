using CalamityMod;
using CalamityMod.Projectiles.Summon;
using CalRemix.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
    public class PinkCrystallineButterfly : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pink Crystalline Butterfly");
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
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            Owner.AddBuff(ModContent.BuffType<TheDreamingGhostBuff>(), 3600);
            if (Projectile.type != ModContent.ProjectileType<PinkCrystallineButterfly>())
                return;

            int frameGate = 6;

            Projectile.frameCounter++;
            if (Projectile.frameCounter > frameGate)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 3)
            {
                Projectile.frame = 0;
            }

            if (Owner.dead)
                Owner.GetModPlayer<CalRemixPlayer>().dreamingGhost = false;
            if (Owner.GetModPlayer<CalRemixPlayer>().dreamingGhost)
                Projectile.timeLeft = 2;
            if (Owner.Center.Distance(Projectile.Center) > 450)
            {
                Projectile.aiStyle = ProjAIStyleID.MiniTwins;
            }
            else
            {
                Projectile.aiStyle = -1;
                Projectile.velocity = Vector2.Zero;
            }
            if (Projectile.ai[0] != 0f)
            {
                Projectile.ai[0] -= 1f;
                return;
            }
            NPC npc = Projectile.Center.MinionHoming(550f, Main.player[Projectile.owner]);
            if (npc != null)
            {
                Projectile.spriteDirection = npc.position.X - Projectile.position.X >= 0 ? -1 : 1;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.DirectionTo(npc.Center) * 20f, ModContent.ProjectileType<SakuraBullet>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
                Projectile.ai[0] = 8f;
            }
            else
            {
                Projectile.spriteDirection = Owner.position.X - Projectile.position.X >= 0 ? -1 : 1;
            }
        }
        public override bool? CanDamage()
        {
            return false;
        }
    }
}
