using CalamityMod;
using CalamityMod.Sounds;
using CalRemix.Content.Buffs;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class AergiaNeuronSummon : ModProjectile
    {
        public ref float OwnerIndex => ref Projectile.ai[0]; // The core projectile

        public ref float NeuronNumber => ref Projectile.ai[1]; // Which neuron this is, not to be confused with its index

        public ref float TotalNeurons => ref Projectile.ai[2]; // The amount of neurons the player has

        public ref float RotationTimer => ref Projectile.localAI[0]; // Determines how rotated the neuron is

        public ref float RotationSpeed => ref Projectile.localAI[1]; // How fast the neuron rotates

        public ref float ShotTimer => ref Projectile.localAI[2]; // Shoot timer

        public override string Texture => "CalRemix/Content/NPCs/Bosses/Hypnos/AergiaNeuron";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aergia Neuron Core");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
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

        public override void AI()
        {
            Projectile owner = Main.projectile[(int)Projectile.ai[0]];
            CheckActive(owner);

            // Shoot lasers if it finds something
            NPC targ = CalamityUtils.MinionHoming(Projectile.Center, 2100, Main.player[Projectile.owner]);
            if (targ != null)
            {
                if (targ.active && targ.life > 0)
                {
                    ShotTimer++;
                    bool desyncfire = (ShotTimer + NeuronNumber) % (TotalNeurons * 2) == 0; // Desync when launched
                    if ((owner.ai[2] > 120 && desyncfire) || (owner.ai[2] <= 120 && ShotTimer % 45 == 0))
                    {
                        SoundEngine.PlaySound(CommonCalamitySounds.ExoLaserShootSound with { Pitch = 0.6f, Volume = 0.3f }, Projectile.Center);
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.DirectionTo(targ.Center) * 22, ModContent.ProjectileType<BlueExoPulseLaserFriendly>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                }
            }
        }

        private void CheckActive(Projectile owner)
        {
            if (Projectile.type != ModContent.ProjectileType<AergiaNeuronSummon>() || owner.type != ModContent.ProjectileType<AergiaNeuronCore>() || !owner.active)
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 2;
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
            Projectile.localAI[2] = reader.ReadSingle();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
            writer.Write(Projectile.localAI[2]);
        }
    }
}
