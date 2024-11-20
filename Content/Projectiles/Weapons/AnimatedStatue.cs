using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Buffs;
using CalamityMod.Projectiles.Rogue;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class AnimatedStatue : ModProjectile
    {
        public ref float Timer => ref Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Animated Statue");
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 43;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            CheckActive();
            if (Projectile.velocity.Y < 9.8)
                Projectile.velocity.Y += 0.25f;
            Timer++;
            if (Timer > 180)
            {
                for (int i = 1; i < 5; i++)
                {
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(1,0).RotatedBy(MathHelper.ToRadians(180/5 * -i)).RotatedByRandom(MathHelper.ToRadians(5)) * 8f, (Main.rand.NextBool(2)) ? ModContent.ProjectileType<EquanimityDarkShard>() : ModContent.ProjectileType<EquanimityLightShard>(), Projectile.damage / 2, 0);
                    proj.DamageType = DamageClass.Summon;
                }
                Timer = 0;
            }
            if (Projectile.Center.Distance(Owner.Center) > 800)
                Projectile.Center = Owner.Center;
        }
        private void CheckActive()
        {
            Owner.AddBuff(ModContent.BuffType<StatueBuff>(), 3600);
            if (Projectile.type != ModContent.ProjectileType<AnimatedStatue>())
                return;
            if (Owner.dead)
                Owner.GetModPlayer<CalRemixPlayer>().statue = false;
            if (Owner.GetModPlayer<CalRemixPlayer>().statue)
                Projectile.timeLeft = 2;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }
    }
}
