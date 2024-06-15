using CalamityMod.Dusts;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalamityMod.BiomeManagers;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System;
using CalRemix.Projectiles.Hostile;
using CalamityMod.Projectiles.Boss;
using System.Collections.Generic;
using CalamityMod.DataStructures;
using Terraria.DataStructures;
using Terraria.GameContent.Animations;
using CalamityMod.Particles;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;

namespace CalRemix.NPCs.BioWar
{
    public class Ecolium : ModNPC
    {
        Entity target = null;
        public List<List<VerletSimulatedSegment>> Chains;
        public List<Vector2> anchors = new List<Vector2>();
        int segmentCount = 30;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ecolium");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 60;
            NPC.width = 44;
            NPC.height = 44;
            NPC.defense = 4;
            NPC.lifeMax = 1000;
            NPC.knockBackResist = 0f;
            NPC.value = 0;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.scale = 2f;
            NPC.HitSound = CalamityMod.NPCs.Perforator.PerforatorHeadMedium.HitSound;
            NPC.DeathSound = CalamityMod.NPCs.Perforator.PerforatorHeadMedium.DeathSound;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Chains = new List<List<VerletSimulatedSegment>>();
            int chainCount = Main.rand.Next(4, 8);
            for (int i = 0; i < chainCount; i++)
            {
                List<VerletSimulatedSegment> Segmentos = new List<VerletSimulatedSegment>();
                if (Segmentos.Count < segmentCount)
                {
                    for (int k = 0; k < segmentCount; k++)
                    {
                        VerletSimulatedSegment segment = new VerletSimulatedSegment(NPC.Center + Vector2.UnitY * 5 * k);
                        Segmentos.Add(segment);
                    }

                    Segmentos[0].locked = true;
                }
                Chains.Add(Segmentos);
            }
            for (int i = 0; i < chainCount; i++)
            {
                anchors.Add(new Vector2(NPC.Center.X + Main.rand.Next(-10, 20), NPC.Center.Y + Main.rand.Next(-10, 20)));
            }
        }

        public override void AI()
        {
            if (target == null || !target.active)
            {
                target = BioWar.BioGetTarget(false, NPC);
            }
            if (NPC.ai[0] <= 0)
            {
                if (Main.rand.NextBool() && target != null && target.active && !(target is NPC nee && nee.life <= 0))
                {
                    NPC.velocity = NPC.DirectionTo(target.Center) * Main.rand.Next(10, 18);
                }
                else
                {
                    NPC.velocity = Main.rand.NextVector2Circular(22, 22).SafeNormalize(Vector2.UnitY) * Main.rand.Next(10, 18);
                }
                NPC.ai[0] = 120;
            }
            NPC.ai[0]--;
            NPC.velocity *= 0.98f;
            if (target != null && target.active && !(target is NPC ne && ne.life <= 0))
            {
                NPC.ai[1]++;
                if (NPC.ai[1] % 30 == 0)
                {
                    int shotSpeed = 16;
                    Vector2 dir = Main.rand.NextBool(5) ? NPC.DirectionTo(target.Center) * shotSpeed : Main.rand.NextVector2CircularEdge(shotSpeed, shotSpeed);
                    int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, dir, ProjectileID.BloodShot, (int)(NPC.damage * 0.33f), 0f);
                    Main.projectile[p].friendly = false;
                    Main.projectile[p].hostile = true;
                    Main.projectile[p].DamageType = DamageClass.Generic;
                }
            }

            for (int i = 0; i < Chains.Count; i++)
            {
                List<VerletSimulatedSegment> Segments = Chains[i];
                if (Segments is null)
                {
                    Segments = new List<VerletSimulatedSegment>(segmentCount);
                    for (int j = 0; j < segmentCount; ++j)
                        Segments[j] = new VerletSimulatedSegment(NPC.Center, false);
                }

                Segments[0].oldPosition = Segments[0].position;
                Segments[0].position = anchors[i];

                Chains[i] = VerletSimulatedSegment.SimpleSimulation(Segments, 3, loops: segmentCount, gravity: 0.2f * (i + 1));

                NPC.netUpdate = true;
                NPC.netSpam = 0;
            }
            Vector2 bloodSpawnPosition = NPC.Center + Main.rand.NextVector2Circular(NPC.width, NPC.height) * 0.33f;
            Vector2 splatterDirection = Main.rand.NextVector2Circular(8, 8);

            if (Main.rand.NextBool(5))
            {
                for (int i = 0; i < 2; i++)
                {
                    int bloodLifetime = Main.rand.Next(22, 36);
                    float bloodScale = Main.rand.NextFloat(0.6f, 0.8f);
                    Color bloodColor = Color.Lerp(Color.Red, Color.DarkRed, Main.rand.NextFloat());
                    bloodColor = Color.Lerp(bloodColor, new Color(51, 22, 94), Main.rand.NextFloat(0.65f));

                    if (Main.rand.NextBool(20))
                        bloodScale *= 2f;

                    Vector2 bloodVelocity = splatterDirection.RotatedByRandom(0.81f) * Main.rand.NextFloat(1f, 2f);
                    bloodVelocity.Y -= 12f;
                    BloodParticle blood = new BloodParticle(bloodSpawnPosition, bloodVelocity, bloodLifetime, bloodScale, bloodColor);
                    GeneralParticleHandler.SpawnParticle(blood);
                }
            }
            for (int i = 0; i < anchors.Count; i++)
            {
                anchors[i] += NPC.velocity;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("A violent strain of bacteria whose goal is to cause as many ruptures as possible.")
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 position = NPC.Center - Main.screenPosition;
            Vector2 origin = texture.Size() * 0.5f;
            Color color = Color.Red * 0.1f;
            Vector2 scale = Vector2.One;
            for (int i = 0; i < 10; i++)
            {
                Vector2 vector2 = (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() + (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() * 2 * Math.Abs((float)Math.Sin(Main.GlobalTimeWrappedHourly));
                Main.spriteBatch.Draw(texture, position + vector2, null, color, NPC.rotation, origin, scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, position, null, NPC.GetAlpha(drawColor), NPC.rotation, origin, scale, SpriteEffects.None, 0f);
            for (int g = 0; g < Chains.Count; g++)
            {
                List<VerletSimulatedSegment> Segments = Chains[g];
                if (Segments == null || Segments.Count <= 0)
                {
                    Segments = new List<VerletSimulatedSegment>(segmentCount);
                    for (int i = 0; i < segmentCount; i++)
                    {
                        VerletSimulatedSegment segment = new VerletSimulatedSegment(NPC.Center + Vector2.UnitY * 5 * i);
                        Segments.Add(segment);
                    }

                    Segments[0].locked = true;
                }
                Chains[g] = Segments;
                for (int i = 0; i < Segments.Count; i++)
                {
                    if (i == 0)
                        continue;
                    VerletSimulatedSegment seg = Segments[i];
                    float dist = i > 0 ? Vector2.Distance(seg.position, Segments[i - 1].position) : 0;
                    if (dist <= 2)
                        dist = 2;
                    dist += 2;
                    float rot = 0f;
                    if (i > 0)
                        rot = seg.position.DirectionTo(Segments[i - 1].position).ToRotation();
                    else
                        rot = NPC.rotation;
                    float scalee = (1 - (i / Segments.Count)) * 0.8f;
                    Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, seg.position - Main.screenPosition, new Rectangle(0, 0, (int)dist, 2), Color.SaddleBrown, rot, TextureAssets.BlackTile.Size() / 2, scalee, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                }
            }
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
