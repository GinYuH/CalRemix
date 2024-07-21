using CalamityMod;
using CalRemix.Buffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
    public class AergiaNeuronSummon : ModProjectile
    {
        public ref float OwnerIndex => ref Projectile.ai[0];

        public ref float NeuronNumber => ref Projectile.ai[1];

        public ref float TotalNeurons => ref Projectile.ai[2];

        public ref float RotationTimer => ref Projectile.localAI[0];

        public ref float RotationSpeed => ref Projectile.localAI[1];

        public override string Texture => "CalRemix/NPCs/Bosses/Hypnos/AergiaNeuron";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aergia Neuron Core");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile owner = Main.projectile[(int)Projectile.ai[0]];
            CheckActive(owner);

            Vector2 destination = Vector2.Zero;

            RotationTimer += MathHelper.Lerp(1, 12, MathHelper.Min(owner.ai[2], 120) / 120);
            int distance = 200;
            double deg = NeuronNumber * 360 / TotalNeurons + RotationTimer;
            double rad = deg * (Math.PI / 180);
            destination.X = owner.Center.X - (int)(Math.Cos(rad) * distance);
            destination.Y = owner.Center.Y - (int)(Math.Sin(rad) * distance);

            Projectile.Center = destination;
            Projectile.velocity = Vector2.Zero;
        }

        private void CheckActive(Projectile owner)
        {
            if (Projectile.type != ModContent.ProjectileType<AergiaNeuronSummon>() || owner.type != ModContent.ProjectileType<AergiaNeuronCore>() || !owner.active)
                return;
            Projectile.timeLeft = 2;
        }
    }
}
