using CalamityMod;
using CalRemix.Buffs;
using CalRemix.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
    public class AergiaNeuronCore : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aergia Neuron Core");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 0;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
        public override bool MinionContactDamage()
        {
            return true;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            CheckActive(owner);

            bool channelingStaff = owner.controlUseTile && owner.HeldItem.type == ModContent.ItemType<AergianTechnistaff>() && !owner.CCed;

            if (owner.controlUseTile)
            {
                Projectile.ai[2]++;
            }

            if (Projectile.ai[2] > 120 && channelingStaff)
            {
                Projectile.ChargingMinionAI(1600f, 1800f, 2500f, 400f, 1, 30f, 24f, 12f, Vector2.Zero, 30f, 10f, true, true);
            }
            else if (!channelingStaff)
            {
                Projectile.ai[2] = 0;
            }
            if (Projectile.ai[2] <= 120)
            {
                if (Projectile.Distance(owner.Center) > 22)
                    Projectile.Center = Vector2.Lerp(Projectile.Center, owner.Center - Vector2.UnitY * owner.gfxOffY, 0.05f);
                else
                    Projectile.Center = owner.Center;
                Projectile.velocity = Vector2.Zero;
            }
        }

        private void CheckActive(Player owner)
        {
            owner.AddBuff(ModContent.BuffType<MackerelBuff>(), 3600);
            if (Projectile.type != ModContent.ProjectileType<AergiaNeuronCore>())
                return;
            if (owner.dead)
                owner.GetModPlayer<CalRemixPlayer>().mackerel = false;
            if (owner.GetModPlayer<CalRemixPlayer>().mackerel)
                Projectile.timeLeft = 2;
        }
    }
}
