using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Graphics.Primitives;
using CalamityMod.NPCs.CalamityAIs.CalamityRegularEnemyAIs;
using CalamityMod.NPCs.VanillaNPCAIOverrides.RegularEnemies;
using Terraria.Audio;
using Terraria.GameContent;
//using CalamityMod.CalPlayer;

namespace CalRemix.Content.NPCs.Minibosses
{
    public class Fleshmullet : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fleshmullet");
            Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 15;
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 100;
            NPC.height = 418;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit8;
            NPC.DeathSound = SoundID.NPCDeath10;
            NPC.lifeMax = (int)(ContentSamples.NpcsByNetId[NPCID.WallofFlesh].lifeMax * 0.5f);
            NPC.defense = 14;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.damage = 50;
            NPC.boss = true;
            NPC.aiStyle = -1;
            Music = MusicID.Boss2;
        }

        public override void AI()
        {
            NPC.ai[3] = NPC.whoAmI;
            NPC.realLife = NPC.whoAmI;
            NPC.TargetClosest();
            CalRemixNPC.WormAI(NPC, 12, 0.25f, Main.player[NPC.target], Main.player[NPC.target].Center);
            if (Main.rand.NextBool(300))
            {
                SoundEngine.PlaySound(SoundID.NPCDeath10 with { Pitch = -0.8f }, NPC.Center);
            }
            if (NPC.Calamity().newAI[0] == 0)
            {
                NPC.Calamity().newAI[0] = 1;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y - 200, NPCType<FleshmulletEye>());
                    NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y + 200, NPCType<FleshmulletEye>());
                }
            }
            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (p.type == ProjectileID.EyeLaser)
                {
                    p.damage = (int)(NPC.damage * 0.25f);
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.075f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void SetBestiary(Terraria.GameContent.Bestiary.BestiaryDatabase database, Terraria.GameContent.Bestiary.BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new Terraria.GameContent.Bestiary.IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new Terraria.GameContent.Bestiary.FlavorTextBestiaryInfoElement("The Underworld is full of some tremendously devilish creatures. Fleshmullets are arguably one of the most vile, consuming any and all lifeforms that draw near the searing lava."),
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Player.ZoneUnderworldHeight || !Main.hardMode)
                return 0f;
            if (NPC.CountNPCS(Type) > 1)
                return 0f;

            return SpawnCondition.Underworld.Chance * 0.22f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.Hungerfish);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (!Main.hardMode)
                {
                    NPC.boss = false;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int w = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, NPCID.WallofFlesh);
                        Main.npc[w].StrikeInstantKill();
                    }
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        float correctedRotation = NPC.rotation - MathHelper.PiOver2;

                        Vector2 goreOffset = -Vector2.UnitY.RotatedBy(correctedRotation) * -44f;
                        goreOffset += Vector2.UnitX.RotatedBy(correctedRotation) * Main.rand.Next(-180, 180);
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center + goreOffset, Vector2.UnitY, Main.rand.Next(140, 143), NPC.scale);                        
                    }
                }
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            float collisionPoint = 0f;
            float wallLength = NPC.height * NPC.scale;
            float wellWidth = NPC.width * NPC.scale;
            return (Collision.CheckAABBvLineCollision(target.Hitbox.TopLeft(), target.Hitbox.Size(), NPC.Center - ((NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * wallLength), NPC.Center + ((NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * wallLength), wellWidth, ref collisionPoint));
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // Mostly exo twins ribbon code
            for (int direction = -2; direction <= 2; direction += 1)
            {
                if (NPC.IsABestiaryIconDummy)
                    break;

                bool mainRibbon = direction == -2 || direction == 2;
                float correctedRotation = NPC.rotation - MathHelper.PiOver2;

                Vector2 ribbonOffset = -Vector2.UnitY.RotatedBy(correctedRotation) * -44f;
                ribbonOffset += Vector2.UnitX.RotatedBy(correctedRotation) * direction * 90f;

                float currentSegmentRotation = correctedRotation;
                List<Vector2> ribbonDrawPositions = new List<Vector2>();
                for (int i = 0; i < 12; i++)
                {
                    float ribbonCompletionRatio = i / 12f;
                    float wrappedAngularOffset = MathHelper.WrapAngle(NPC.oldRot[i + 1] - currentSegmentRotation - MathHelper.PiOver2) * 0.3f;

                    Vector2 ribbonSegmentOffset = Vector2.UnitY.RotatedBy(currentSegmentRotation) * ribbonCompletionRatio * (mainRibbon ? 470f : 310f);
                    ribbonDrawPositions.Add(NPC.Center + ribbonSegmentOffset + ribbonOffset);

                    currentSegmentRotation += wrappedAngularOffset;
                }
                if (mainRibbon)
                    PrimitiveRenderer.RenderTrail(ribbonDrawPositions, new(RibbonTrailWidthFunction, RibbonTrailColorFunction), 66);
                else
                    PrimitiveRenderer.RenderTrail(ribbonDrawPositions, new(RibbonTrailWidthFunctionSmall, RibbonTrailColorFunction), 66);
            }
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos + NPC.gfxOffY * Vector2.UnitY, NPC.frame, drawColor, NPC.rotation, new Vector2(TextureAssets.Npc[NPC.type].Width() / 2, TextureAssets.Npc[NPC.type].Height() / 4), NPC.scale, SpriteEffects.None, 0);
            return false;
        }

        public float RibbonTrailWidthFunction(float completion)
        {
            if (completion < 0.9f)
                return Math.Abs(MathF.Sin(completion * 0.5f) + 0.5f) * 10;
            else if (completion < 0.95f)
                return MathHelper.Lerp(10, 14, Utils.GetLerpValue(0.9f, 0.95f, completion));
            else
                return MathHelper.Lerp(14, 10, Utils.GetLerpValue(0.95f, 1f, completion));
        }

        public float RibbonTrailWidthFunctionSmall(float completion)
        {
            float baseg = Math.Abs(MathF.Sin(completion * 0.5f) + 0.5f) * 6;
            if (completion < 0.9f)
                return baseg;
            else
                return baseg + MathF.Sin(completion) * 4;
        }

        public Color RibbonTrailColorFunction(float color)
        {
            return Color.Black;
        }
    }
}