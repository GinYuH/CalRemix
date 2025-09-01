using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using System;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Boss;
using Terraria.Audio;
using CalamityMod.Items.Potions.Alcohol;
using Terraria.ModLoader.Utilities;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System.Collections.Generic;
using CalamityMod.Graphics.Primitives;
using CalamityMod.DataStructures;
using CalRemix.Content.Items.Weapons;

namespace CalRemix.Content.NPCs
{
    public class Graknight : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 15;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 40;
            NPC.width = 20;
            NPC.height = 46;
            NPC.defense = 10;
            NPC.lifeMax = 500;
            NPC.knockBackResist = 0f;
            NPC.value = 3000;
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit41 with { Pitch = -0.3f };
            NPC.DeathSound = SoundID.NPCDeath43 with { Pitch = -0.3f };
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToWater = true;
        }

        public override void AI()
        {
            bool flag45 = false;
            bool flag47 = false;
            NPC.TargetClosest();
            if (Main.player[NPC.target].dead)
            {
                flag47 = true;
                flag45 = true;
            }
            else if (Main.netMode != 1 && NPC.target >= 0 && NPC.target < 255)
            {
                int num1355 = 4800;
                if (NPC.timeLeft < NPC.activeTime && Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) < (float)num1355)
                {
                    NPC.timeLeft = NPC.activeTime;
                }
            }
            if (NPC.localAI[0] == 0f && Main.netMode != 1)
            {
                NPC.localAI[0] = 1f;
            }
            int[] array2 = new int[3];
            float num1357 = 0f;
            float num1358 = 0f;
            int num1359 = 0;
            for (int num1360 = 0; num1360 < 200; num1360++)
            {
                if (Main.npc[num1360].active && Main.npc[num1360].aiStyle == 52)
                {
                    num1357 += Main.npc[num1360].Center.X;
                    num1358 += Main.npc[num1360].Center.Y;
                    array2[num1359] = num1360;
                    num1359++;
                    if (num1359 > 2)
                    {
                        break;
                    }
                }
            }
            num1357 /= (float)num1359;
            num1358 /= (float)num1359;
            float num1361 = 2.5f;
            float num1363 = 0.025f;
            if (NPC.life < NPC.lifeMax / 2)
            {
                num1361 = 5f;
                num1363 = 0.05f;
            }
            if (NPC.life < NPC.lifeMax / 4)
            {
                num1361 = 7f;
            }
            if (Main.expertMode)
            {
                num1361 += 1f;
                num1361 *= 1.1f;
                num1363 += 0.01f;
                num1363 *= 1.1f;
            }
            if (Main.getGoodWorld)
            {
                num1361 *= 1.15f;
                num1363 *= 1.15f;
            }
            Vector2 vector315 = new Vector2(num1357, num1358);
            float num1364 = Main.player[NPC.target].Center.X - vector315.X;
            float num1365 = Main.player[NPC.target].Center.Y - vector315.Y;
            if (flag47)
            {
                num1365 *= -1f;
                num1364 *= -1f;
                num1361 += 8f;
            }
            float num1366 = (float)Math.Sqrt(num1364 * num1364 + num1365 * num1365);
            int num1367 = 500;
            if (flag45)
            {
                num1367 += 350;
            }
            if (Main.expertMode)
            {
                num1367 += 150;
            }
            if (num1366 >= (float)num1367)
            {
                num1366 = (float)num1367 / num1366;
                num1364 *= num1366;
                num1365 *= num1366;
            }
            num1357 += num1364;
            num1358 += num1365;
            vector315 = new Vector2(NPC.Center.X, NPC.Center.Y);
            num1364 = num1357 - vector315.X;
            num1365 = num1358 - vector315.Y;
            num1366 = (float)Math.Sqrt(num1364 * num1364 + num1365 * num1365);
            if (num1366 < num1361)
            {
                num1364 = NPC.velocity.X;
                num1365 = NPC.velocity.Y;
            }
            else
            {
                num1366 = num1361 / num1366;
                num1364 *= num1366;
                num1365 *= num1366;
            }
            NPC.velocity = NPC.DirectionTo(Main.player[NPC.target].Center);
            Vector2 vector316 = new Vector2(NPC.Center.X, NPC.Center.Y);
            float num1368 = Main.player[NPC.target].Center.X - vector316.X;
            float num1369 = Main.player[NPC.target].Center.Y - vector316.Y;
            NPC.rotation = (float)Math.Atan2(num1369, num1368) + 1.57f;
            
            int num1370 = 50;
            if (flag45)
            {
                num1370 *= 2;
            }
            if (Main.netMode == 1)
            {
                return;
            }
            NPC.localAI[1] += 1f;
            if ((double)NPC.life < (double)NPC.lifeMax * 0.9)
            {
                NPC.localAI[1] += 1f;
            }
            if ((double)NPC.life < (double)NPC.lifeMax * 0.8)
            {
                NPC.localAI[1] += 1f;
            }
            if ((double)NPC.life < (double)NPC.lifeMax * 0.7)
            {
                NPC.localAI[1] += 1f;
            }
            if ((double)NPC.life < (double)NPC.lifeMax * 0.6)
            {
                NPC.localAI[1] += 1f;
            }
            if (flag45)
            {
                NPC.localAI[1] += 3f;
            }
            if (Main.expertMode)
            {
                NPC.localAI[1] += 1f;
            }
            if (Main.expertMode && NPC.justHit && Main.rand.Next(2) == 0)
            {
                NPC.localAI[3] = 1f;
            }
            if (Main.getGoodWorld)
            {
                NPC.localAI[1] += 1f;
            }
            if (!(NPC.localAI[1] > 180f))
            {
                return;
            }
            NPC.localAI[1] = 0f;
            bool flag48 = Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height);
            if (NPC.localAI[3] > 0f)
            {
                flag48 = true;
                NPC.localAI[3] = 0f;
            }
            Vector2 vector317 = new Vector2(NPC.Center.X, NPC.Center.Y);
            float num1371 = 15f;
            if (Main.expertMode)
            {
                num1371 = 17f;
            }
            float num1372 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector317.X;
            float num1374 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector317.Y;
            float num1375 = (float)Math.Sqrt(num1372 * num1372 + num1374 * num1374);
            num1375 = num1371 / num1375;
            num1372 *= num1375;
            num1374 *= num1375;
            int num1377 = 275;
            int maxValue2 = 4;
            int maxValue3 = 8;
            if (Main.expertMode)
            {
                maxValue2 = 2;
                maxValue3 = 6;
            }
            if ((double)NPC.life < (double)NPC.lifeMax * 0.8 && Main.rand.Next(maxValue2) == 0)
            {
                NPC.localAI[1] = -30f;
                num1377 = 276;
            }
            else if ((double)NPC.life < (double)NPC.lifeMax * 0.8 && Main.rand.Next(maxValue3) == 0)
            {
                NPC.localAI[1] = -120f;
                num1377 = 277;
            }
            int num1376 = CalRemixHelper.ProjectileDamage(20, 40);
            vector317.X += num1372 * 3f;
            vector317.Y += num1374 * 3f;
            SoundEngine.PlaySound(BetterSoundID.ItemSpaceLaser, NPC.Center);
            int num1378 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector317.X, vector317.Y, num1372 * 0.4f, num1374 * 0.4f, ProjectileID.MartianWalkerLaser, num1376, 0f, Main.myPlayer);
            if (num1377 != 277)
            {
                Main.projectile[num1378].timeLeft = 300;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Granite,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea || !spawnInfo.Player.ZoneGranite || !Main.hardMode)
            {
                return 0f;
            }
            return SpawnCondition.Cavern.Chance * 0.05f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Granite, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Granite, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.Granite, 1, 23, 34);
            npcLoot.Add(ModContent.ItemType<Grakit>(), 10);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;
            spriteBatch.ExitShaderRegion();
            // Mostly exo twins ribbon code
            for (int direction = -1; direction <= 1; direction += 2)
            {
                if (NPC.IsABestiaryIconDummy)
                    break;

                float correctedRotation = NPC.rotation;

                Vector2 ribbonOffset = -Vector2.UnitY.RotatedBy(correctedRotation) * -24f;
                ribbonOffset += Vector2.UnitX.RotatedBy(correctedRotation) * direction * 25f;

                float currentSegmentRotation = correctedRotation;
                List<Vector2> ribbonDrawPositions = new List<Vector2>();
                for (int i = 0; i < 12; i++)
                {
                    float ribbonCompletionRatio = i / 12f;
                    float wrappedAngularOffset = MathHelper.WrapAngle(NPC.oldRot[i + 1] - currentSegmentRotation) * 0.3f;

                    Vector2 ribbonSegmentOffset = Vector2.UnitY.RotatedBy(currentSegmentRotation) * ribbonCompletionRatio * 200;
                    float other = direction == -1 ? -1 : 1;
                    ribbonDrawPositions.Add(NPC.Center + (ribbonSegmentOffset + ribbonOffset).RotatedBy(direction * 0.2f * MathF.Sin(i + 2 * Main.GlobalTimeWrappedHourly)));

                    currentSegmentRotation += wrappedAngularOffset;
                }
                PrimitiveRenderer.RenderTrail(ribbonDrawPositions, new(RibbonTrailWidthFunctionSmall, RibbonTrailColorFunction));

                Vector2 tuskOffset = -Vector2.UnitY.RotatedBy(correctedRotation) * 24f;
                tuskOffset += Vector2.UnitX.RotatedBy(correctedRotation) * direction * 25f;

                List<Vector2> tuskDrawPositions = new List<Vector2>()
                {
                    NPC.Center + tuskOffset,
                    NPC.Center + tuskOffset - Vector2.UnitY.RotatedBy(MathHelper.PiOver4 * direction).RotatedBy(NPC.rotation) * 64,
                    NPC.Center + tuskOffset - Vector2.UnitY.RotatedBy(NPC.rotation) * 140
                };
                BezierCurve curve = new BezierCurve(tuskDrawPositions.ToArray());
                tuskDrawPositions = curve.GetPoints(12);
                for (int l = 0; l < tuskDrawPositions.Count; l++)
                {
                    tuskDrawPositions[l] = (tuskDrawPositions[l] - NPC.Center).RotatedBy(0.2f * direction * MathF.Sin(5 * Main.GlobalTimeWrappedHourly)) + NPC.Center;
                }
                PrimitiveRenderer.RenderTrail(tuskDrawPositions, new(RibbonTrailWidthFunctionSmall, RibbonTrailColorFunction));


                Vector2 biterOffset = -Vector2.UnitY.RotatedBy(correctedRotation) * 46f;
                biterOffset += Vector2.UnitX.RotatedBy(correctedRotation) * direction * 10f;

                List<Vector2> biterDrawPositions = new List<Vector2>()
                {
                    NPC.Center + biterOffset,
                    NPC.Center + biterOffset - Vector2.UnitY.RotatedBy(MathHelper.PiOver4 * direction).RotatedBy(NPC.rotation) * 10,
                    NPC.Center + biterOffset - Vector2.UnitY.RotatedBy(NPC.rotation) * 20
                };
                BezierCurve curve2 = new BezierCurve(biterDrawPositions.ToArray());
                biterDrawPositions = curve2.GetPoints(12);
                for (int l = 0; l < biterDrawPositions.Count; l++)
                {
                    biterDrawPositions[l] = (biterDrawPositions[l] - NPC.Center).RotatedBy(0.05f * direction * MathF.Sin(5 * Main.GlobalTimeWrappedHourly)) + NPC.Center;
                }
                PrimitiveRenderer.RenderTrail(biterDrawPositions, new(RibbonTrailWidthFunctionSmall, RibbonTrailColorFunction));
            }




            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D tex2 = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/GraknightGlow").Value;
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, NPC.GetAlpha(drawColor), NPC.rotation, tex.Size() / 2, NPC.scale, 0, 0);
            spriteBatch.Draw(tex2, NPC.Center - screenPos, null, Color.White, NPC.rotation, tex.Size() / 2, NPC.scale, 0, 0);
            return false;
        }


        public float RibbonTrailWidthFunctionSmall(float completion)
        {
            return MathHelper.Lerp(10, 1, completion);
        }

        public Color RibbonTrailColorFunction(float color)
        {
            return Color.Lerp(Color.Navy, Color.MediumAquamarine, color) * 0.4f;
        }
    }
}
