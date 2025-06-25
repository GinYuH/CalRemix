using CalamityMod;
using CalamityMod.Graphics.Primitives;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Sounds;
using CalRemix.Content.Buffs;
using CalRemix.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class AergiaNeuronCore : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Aergia Neuron Core");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
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

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            CheckActive(owner);

            // If the player is channeling the staff
            bool channelingStaff = owner.controlUseTile && owner.HeldItem.type == ModContent.ItemType<AergianTechnistaff>() && !owner.CCed;

            // Tick up ai2
            if (channelingStaff)
            {
                Projectile.ai[2]++;
            }

            // Once ai2 hits 2 seconds and the player is still channeling, switch to charging ai
            if (Projectile.ai[2] > 120 && channelingStaff)
            {
                Projectile.ChargingMinionAI(1600f, 2500f, 2800f, 400f, 1, 30f, 24f, 12f, Vector2.Zero, 30f, 10f, true, true);
            }
            // else reset 
            else if (!channelingStaff)
            {
                Projectile.ai[2] = 0;
            }
            // While not charging, move back to the player's center
            if (Projectile.ai[2] <= 120)
            {
                if (Projectile.Distance(owner.Center) > 22)
                    Projectile.Center = Vector2.Lerp(Projectile.Center, owner.Center - Vector2.UnitY * owner.gfxOffY, 0.5f);
                else
                    Projectile.Center = owner.Center;
                Projectile.velocity = Vector2.Zero;
            }
            // Charge sound once full
            if (Projectile.ai[2] == 120)
            {
                SoundEngine.PlaySound(CommonCalamitySounds.ELRFireSound, Projectile.Center);
            }
            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (p.owner != Projectile.owner)
                    continue;
                if (p.type == ModContent.ProjectileType<AergiaNeuronSummon>())
                NeuronAI(p);
            }
        }

        private void CheckActive(Player owner)
        {
            owner.AddBuff(ModContent.BuffType<AergiaNeuronBuff>(), 3600);
            if (Projectile.type != ModContent.ProjectileType<AergiaNeuronCore>())
                return;
            if (owner.dead)
                owner.GetModPlayer<CalRemixPlayer>().neuron = false;
            if (owner.GetModPlayer<CalRemixPlayer>().neuron)
                Projectile.timeLeft = 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            int neuronType = ModContent.ProjectileType<AergiaNeuronSummon>();
            // Draw lightning between all of the neurons
            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (p.type != neuronType)
                    continue;
                if (p.owner != Projectile.owner)
                    continue;
                if (p.ai[0] != Projectile.whoAmI)
                    continue;

                foreach (Projectile pe in Main.ActiveProjectiles)
                {
                    if (pe.type != neuronType)
                        continue;
                    if (pe.owner != Projectile.owner)
                        continue;
                    if (pe.ai[0] != Projectile.whoAmI)
                        continue;
                    // If a neuron has a higher value OR the final neuron finds the first neuron, draw the lightning
                    if (pe.ai[1] != p.ai[1] + 1 && !(pe.ai[1] == (pe.ai[2] - 1) && p.ai[1] == 0))
                        continue;
                    List<Vector2> points = AresTeslaOrb.DetermineElectricArcPoints(p.Center, pe.Center, (int)(250290787 * pe.ai[1]));
                    PrimitiveRenderer.RenderTrail(points, new(BackgroundWidthFunction, BackgroundColorFunction, smoothen: false), 90);
                    PrimitiveRenderer.RenderTrail(points, new(WidthFunction, ColorFunction, smoothen: false), 90);
                }
            }
            return false;
        }


        internal float WidthFunction(float completionRatio) => 0.9f;

        internal float BackgroundWidthFunction(float completionRatio) => WidthFunction(completionRatio) * 4f;

        public Color BackgroundColorFunction(float completionRatio) => Color.CornflowerBlue * 0.4f;

        internal Color ColorFunction(float completionRatio)
        {
            Color baseColor1 = Color.Cyan;
            Color baseColor2 = Color.Cyan;

            float fadeToWhite = MathHelper.Lerp(0f, 0.65f, (float)Math.Sin(MathHelper.TwoPi * completionRatio + Main.GlobalTimeWrappedHourly * 4f) * 0.5f + 0.5f);
            Color baseColor = Color.Lerp(baseColor1, Color.White, fadeToWhite);
            Color color = Color.Lerp(baseColor, baseColor2, ((float)Math.Sin(MathHelper.Pi * completionRatio + Main.GlobalTimeWrappedHourly * 4f) * 0.5f + 0.5f) * 0.8f) * 0.65f;
            color.A = (byte)MathHelper.Lerp(0, 84, MathHelper.Min(Projectile.ai[2], 120) / 120);
            return color;
        }

        /// <summary>
        /// Simulates the movement for the neurons
        /// This is done here because god said if it's done in the neuron's ai it occasionally gets a bugged offset
        /// </summary>
        /// <param name="p"></param>
        public static void NeuronAI(Projectile p)
        {
            Projectile owner = Main.projectile[(int)p.ai[0]];

            Vector2 destination = Vector2.Zero;

            // Orbit the core
            p.localAI[0] += MathHelper.Lerp(1, 12, MathHelper.Min(owner.ai[2], 120) / 120);
            int distance = 200;
            double deg = p.ai[1] * 360 / p.ai[2] + p.localAI[0];
            double rad = deg * (Math.PI / 180);
            destination.X = owner.Center.X - (int)(Math.Cos(rad) * distance);
            destination.Y = owner.Center.Y - (int)(Math.Sin(rad) * distance);

            p.Center = destination;
            p.velocity = Vector2.Zero;
            p.extraUpdates = owner.extraUpdates;
            p.numUpdates = owner.numUpdates;
            p.netUpdate = owner.netUpdate;
        }
    }
}
