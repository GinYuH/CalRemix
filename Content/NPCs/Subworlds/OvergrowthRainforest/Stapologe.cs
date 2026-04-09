using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalamityMod.BiomeManagers;
using CalRemix.Content.Items.Placeables.Banners;
using Microsoft.Xna.Framework;
using System;
using Terraria.Enums;
using CalamityMod.NPCs.NormalNPCs;
using Microsoft.Xna.Framework.Graphics;
using CalRemix.Content.Items.Placeables.Subworlds.Wolf;
using Terraria.GameContent;
using CalamityMod.Items.Materials;
using CalRemix.Core.Biomes.Subworlds;
using Terraria.Audio;
using CalamityMod.Sounds;
using CalamityMod.NPCs.Abyss;
using System.Collections.Generic;
using CalamityMod.Graphics.Primitives;

namespace CalRemix.Content.NPCs.Subworlds.OvergrowthRainforest
{
    public class Stapologe : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.ImmuneToAllBuffs[Type] = true;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 40;
            NPC.height = 100;
            NPC.defense = 999999;
            NPC.knockBackResist = 0;
            NPC.Calamity().unbreakableDR = true;
            NPC.lifeMax = 5000;
            NPC.value = Item.buyPrice(silver: 5);
            NPC.HitSound = null;
            NPC.DeathSound = SoundID.NPCDeath1 with { Pitch = 0.4f };
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.rarity = 2;
            SpawnModBiomes = [ModContent.GetInstance<OvergrowthRainforestBiome>().Type, ModContent.GetInstance<BigOlBranchesBiome>().Type, ModContent.GetInstance<TitanicTrunksBiome>().Type];
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void AI()
        {
            NPC.velocity.X = 0;
            NPC.TargetClosest();
            NPC.ai[2]++;
            switch (NPC.ai[0])
            {
                case 0:
                    NPC.dontTakeDamage = true;
                    NPC.Calamity().unbreakableDR = true;
                    NPC.chaseable = false;
                    NPC.defense = 999999;
                    if (NPC.ai[2] > CalamityUtils.SecondsToFrames(30))
                    {
                        SoundEngine.PlaySound(SoundID.NPCHit31, NPC.Center);
                        NPC.ai[2] = 0;
                        NPC.ai[0] = 1;
                    }
                    if (Main.rand.NextBool(60))
                    {
                        SoundEngine.PlaySound(CommonCalamitySounds.ELRFireSound with { Pitch = 0.6f, PitchVariance = 0.2f, MaxInstances = 0 }, NPC.Center);
                    }
                    break;
                case 1:
                    {
                        NPC.dontTakeDamage = false;
                        NPC.Calamity().unbreakableDR = false;
                        NPC.chaseable = true;
                        NPC.defense = 20;
                        int phaseTime = CalamityUtils.SecondsToFrames(10);
                        int openTime = 15;
                        if (NPC.ai[2] <= openTime)
                        {
                            NPC.ai[1] = MathHelper.Lerp(0, MathHelper.ToRadians(80), CalamityUtils.ExpOutEasing(Utils.GetLerpValue(0, openTime, NPC.ai[2], true), 1));
                        }
                        else if (NPC.ai[2] > openTime && NPC.ai[2] < phaseTime - openTime)
                        {
                            NPC.ai[1] = Utils.AngleLerp(NPC.ai[1], MathHelper.ToRadians(80 + MathF.Sin(NPC.ai[2] * 0.2f) * 10), 0.3f);
                            if (NPC.ai[2] % Main.rand.Next(15, 30) == 0)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Vector2 pos = NPC.Bottom - Vector2.UnitY * 90;
                                    SoundEngine.PlaySound(BetterSoundID.ItemGrenadeChuck with { Pitch = 0.4f }, NPC.Center);
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), pos, pos.DirectionTo(Main.player[NPC.target].Center) * 8, ProjectileID.PoisonSeedPlantera, CalRemixHelper.ProjectileDamage(90, 160), 1);
                                }
                            }
                        }
                        else if (NPC.ai[2] >= phaseTime - openTime && NPC.ai[2] <= phaseTime)
                        {
                            NPC.ai[1] = MathHelper.Lerp(MathHelper.ToRadians(80), 0, CalamityUtils.ExpInEasing(Utils.GetLerpValue(phaseTime - openTime, phaseTime, NPC.ai[2], true), 1));
                        }
                        else if (NPC.ai[2] > phaseTime)
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[0] = 0;
                            NPC.ai[2] = 0;
                        }
                    }
                    break;
            }
            NPC.velocity.Y += 5;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D blade = ModContent.Request<Texture2D>(Texture + "_Blade").Value;
            Texture2D eye = ModContent.Request<Texture2D>(Texture + "_Eye").Value;

            Vector2 drawPos = NPC.Bottom - screenPos;

            float eyeHeight = MathHelper.Lerp(-10, -90, Utils.GetLerpValue(0, MathHelper.ToRadians(80), NPC.ai[1], true));

            List<Vector2> points = new();

            for (int i = 0; i < 20; i++)
            {
                points.Add(Vector2.Lerp(drawPos + screenPos, drawPos + screenPos + Vector2.UnitY * eyeHeight, i / 19f) + Vector2.UnitX * MathF.Sin(i + Main.GlobalTimeWrappedHourly) * 5);
            }

            PrimitiveRenderer.RenderTrail(points, new(new PrimitiveSettings.VertexWidthFunction((float f, Vector2 v) => 4), new PrimitiveSettings.VertexColorFunction((float f, Vector2 v) => Lighting.GetColor((v + screenPos).ToTileCoordinates()).MultiplyRGB(Color.Lime))));

            spriteBatch.Draw(eye, drawPos + Vector2.UnitY * eyeHeight, null, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(eye.Width / 2, eye.Height / 2), NPC.scale, 0, 0);
            spriteBatch.Draw(tex, drawPos +Vector2.UnitY * 6, null, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height), NPC.scale, 0, 0);
            spriteBatch.Draw(blade, drawPos + new Vector2(-6, -16), null, NPC.GetAlpha(drawColor), NPC.rotation - NPC.ai[1], new Vector2(18, 78), NPC.scale, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(blade, drawPos + new Vector2(6, -16), null, NPC.GetAlpha(drawColor), NPC.rotation + NPC.ai[1], new Vector2(18, 78), NPC.scale, 0, 0);
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Grass, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Grass, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.AcidDye, 1, 8, 13);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit)
        {
            if (NPC.ai[0] == 0)
            {
                SoundEngine.PlaySound(ScornEater.HitSound with { Pitch = -0.3f }, NPC.Center);
            }
            else
            {
                SoundEngine.PlaySound(DevilFish.MaskBreakSound with { Volume = 1.3f }, NPC.Center);
            }
        }
    }
}
