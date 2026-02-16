using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Items.Potions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent;
using System;
using CalamityMod.Graphics.Primitives;
using CalRemix.UI;
using Terraria.UI;
using static Terraria.Graphics.Effects.Filters;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader.IO;
using CalamityMod.NPCs.Cryogen;
using Terraria.Graphics.Shaders;
using CalRemix.Core.World;
using CalamityMod.Projectiles.Boss;
using Terraria.Audio;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.Sounds;
using CalamityMod.World;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using CalRemix.Content.Items.Materials;
using rail;
using Terraria.Graphics.CameraModifiers;
using CalRemix.Core.Graphics;
using CalamityMod.NPCs.Providence;
using CalRemix.Content.NPCs.Subworlds.GreatSea;
using CalamityMod.Physics;
using System.Linq;
using Steamworks;

// So like, technically she's not in the Sealed Dimension, but Horizon is a mechanical extension of it so...
namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class Crevivence : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];

        public int GemIndex
        {
            get => (int)NPC.ai[2];
            set => NPC.ai[2] = value;
        }

        public NPC Gem => Main.npc[(int)NPC.ai[2]];

        public ref float ExtraVar => ref NPC.ai[3];
        public Vector2 SavePosition
        {
            get => new Vector2(NPC.Calamity().newAI[2], NPC.Calamity().newAI[1]);
            set
            {
                NPC.Calamity().newAI[2] = value.X;
                NPC.Calamity().newAI[1] = value.Y;
            }
        }
        public Vector2 OldPosition
        {
            get => new Vector2(NPC.localAI[2], NPC.localAI[1]);
            set
            {
                NPC.localAI[2] = value.X;
                NPC.localAI[1] = value.Y;
            }
        }

        public enum PhaseType
        {
            SpawnAnimation = 0,
            Idle = 1,
            AttackOne = 2,
            AttackTwo = 3,
            AttackThree = 4,
            AttackFour = 5,
            AttackFive = 6,
            AttackSix = 7,
            AttackSeven = 8,
            AttackEight = 9,
            DeathAnimation = 10
        }

        public PhaseType CurrentPhase
        {
            get => (PhaseType)State;
            set => State = (float)value;
        }

        public static float SunOpacity = 1f;

        public RopeHandle? LeftRibbon;
        public RopeHandle? RightRibbon;

        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 15;
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 80;
            NPC.height = 80;
            NPC.lifeMax = 300000;
            NPC.damage = 310;
            NPC.defense = 50;
            NPC.noGravity = true;
            NPC.HitSound = Cryogen.HitSound with { Pitch = -1 };
            NPC.DeathSound = Cryogen.DeathSound with { Pitch = 1 };
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.alpha = 255;
            NPC.Calamity().canBreakPlayerDefense = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VoidForestBiome>().Type };
            Music = CalRemixMusic.TheCalamity;
        }
        public override void AI()
        {
            Vector2 ribbonL = NPC.Center + new Vector2(-60, 70).RotatedBy(NPC.rotation);
            Vector2 ribbonR = NPC.Center + new Vector2(60, 70).RotatedBy(NPC.rotation);
            if (LeftRibbon == null || RightRibbon == null)
            {
                int ribbonSegmentCount = 40;
                float distancePerSegment = 400 / ribbonSegmentCount;
                RopeSettings ribbonSettings = new RopeSettings()
                {
                    StartIsFixed = true,
                    Mass = 0.1f,
                    RespondToEntityMovement = true,
                    RespondToWind = false
                };
                LeftRibbon = GetInstance<RopeManagerSystem>().RequestNew(ribbonL, NPC.Center + Vector2.UnitY * 260, ribbonSegmentCount, distancePerSegment, Vector2.UnitY * 20, ribbonSettings, 80);
                RightRibbon = GetInstance<RopeManagerSystem>().RequestNew(ribbonR, NPC.Center + Vector2.UnitY * 260, ribbonSegmentCount, distancePerSegment, Vector2.UnitY * 20, ribbonSettings, 80);
            }
            else
            {
                RopeHandle left = LeftRibbon.Value;
                RopeHandle right = RightRibbon.Value;
                left.Start = ribbonL;
                left.Gravity = Vector2.UnitY.RotatedBy(NPC.rotation) * 10;
                right.Start = ribbonR;
                right.Gravity = Vector2.UnitY.RotatedBy(NPC.rotation) * 10;
            }
            NPC.velocity = Main.MouseWorld - NPC.Center;
            if (NPC.velocity.X > 0)
            {
                NPC.rotation = Utils.AngleLerp(NPC.rotation, MathHelper.ToRadians(15), 0.2f);
            }
            else if (NPC.velocity.X < 0)
            {
                NPC.rotation = Utils.AngleLerp(NPC.rotation, -MathHelper.ToRadians(15), 0.2f);
            }
            else
            {
                NPC.rotation = Utils.AngleLerp(NPC.rotation, 0, 0.2f);
            }
            NPC.TargetClosest(false);
            switch (CurrentPhase)
            {
                case PhaseType.SpawnAnimation:
                    {
                        if (true)
                        {
                            NPC.Opacity = 1;
                            return;
                        }
                        float startAction = 60;
                        float absorbSun = startAction + 300;
                        float waitForIt = absorbSun + 90;
                        float stopFade = waitForIt + 5;
                        float linger = stopFade + 120;
                        if (Timer < startAction)
                        {

                        }
                        else if (Timer < absorbSun)
                        {
                            SunOpacity = MathHelper.Lerp(1, 0.6f, Utils.GetLerpValue(startAction, absorbSun, Timer, true));
                            Main.LocalPlayer.Calamity().GeneralScreenShakePower = MathHelper.Lerp(0, 10, Utils.GetLerpValue(startAction + 60, absorbSun, Timer, true));
                        }
                        else if (Timer < waitForIt)
                        {

                        }
                        else if (Timer <= stopFade)
                        {
                            if (Timer % 5 == 0)
                            {
                                SoundEngine.PlaySound(Providence.HolyRaySound with { Pitch = -1, MaxInstances = 0, PitchVariance = 0.25f });
                                SoundEngine.PlaySound(Providence.HolyRaySound with { Pitch = 1, MaxInstances = 0, PitchVariance = 0.25f });
                                //Particle BS
                            }
                            NPC.Opacity = MathHelper.Lerp(0, 1, Utils.GetLerpValue(waitForIt, stopFade, Timer, true));
                        }
                        else if (Timer < linger)
                        {
                            NPC.Opacity = 1;
                            Main.LocalPlayer.Calamity().GeneralScreenShakePower = 3;
                        }
                        else if (Timer == linger)
                        {

                        }

                        if (Timer > absorbSun)
                        {
                            float cameraPanInterpolant = Utils.GetLerpValue(absorbSun + 10, absorbSun + 40, Timer, true);
                            //float cameraZoom = Utils.GetLerpValue(absorbSun + , 60f, Timer, true) * 0.2f;
                            CameraPanSystem.CameraFocusPoint = Main.LocalPlayer.Center;
                            CameraPanSystem.CameraPanInterpolant = cameraPanInterpolant;
                            //CameraPanSystem.Zoom = cameraZoom;
                        }
                    }
                    break;
            }
            Timer++;
        }

        public void ChangePhase(PhaseType newPhase)
        {
            CurrentPhase = newPhase;
            //CurrentPhase = PhaseType.Metagross;
            Timer = 0;
            ExtraVar = 0;
            OldPosition = Vector2.Zero;
            SavePosition = Vector2.Zero;
            NPC.netUpdate = true;
        }


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemType<GildedShard>(), 1, 30, 60);
        }

        public override void BossLoot(ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void OnKill()
        {
            RemixDowned.downedCrevi = true;
            CalRemixWorld.UpdateWorldBool();
        }

        public override bool CheckActive()
        {
            return !NPC.HasValidTarget;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Microsoft.Xna.Framework.Color drawColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Sealed/Crevivience").Value;
            Texture2D sigils = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Sealed/CrevivienceBodySigils").Value;
            Texture2D ring = Request<Texture2D>("CalamityMod/Particles/BloomRing").Value;
            Texture2D bloom = Request<Texture2D>("CalamityMod/Particles/Light").Value;

            float eyeScale = 0.8f;
            Vector2 eyePos = NPC.Center + Vector2.UnitY.RotatedBy(NPC.rotation) * 20 - screenPos;

            float correctedRotation = NPC.rotation;

            Vector2 ribbonOffset = -Vector2.UnitY.RotatedBy(correctedRotation) * -44f;

            float currentSegmentRotation = correctedRotation;
            List<Vector2> ribbonDrawPositions = new List<Vector2>();
            for (int i = 0; i < 12; i++)
            {
                float ribbonCompletionRatio = i / 12f;
                float wrappedAngularOffset = MathHelper.WrapAngle(NPC.oldRot[i + 1] - currentSegmentRotation) * 0.1f;

                Vector2 ribbonSegmentOffset = Vector2.UnitY.RotatedBy(currentSegmentRotation) * ribbonCompletionRatio * 500;
                ribbonDrawPositions.Add(NPC.Center + ribbonSegmentOffset + ribbonOffset);

                currentSegmentRotation += wrappedAngularOffset;
            }

            float startWidth = 40;
            float windWidth = 20;

            Vector2 startPos = NPC.Center + Vector2.UnitY.RotatedBy(NPC.rotation) * 120;
            Vector2 endPos = ribbonDrawPositions[^2];

            for (int i = 0; i < 3; i++)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    List<Vector2> wingPoints = new();
                    for (int k = 0; k < 20; k++)
                    {
                        Vector2 start = Vector2.Lerp(startPos, endPos, i / 2f) + Vector2.UnitX * j * MathHelper.Lerp(36, 4, i / 2f);
                        Vector2 end = Vector2.Lerp(startPos, endPos, i / 2f) + Vector2.UnitX * j * MathHelper.Lerp(400, 100, i / 2f);
                        Vector2 segPos = Vector2.Lerp(start, end, k / 19f) + Vector2.UnitY * MathF.Sin(0.2f * Main.GlobalTimeWrappedHourly * 22 + k * 0.2f) * MathHelper.Lerp(10, 60, k / 19f);
                        segPos = segPos.RotatedBy(NPC.rotation, start);
                        wingPoints.Add(segPos);


                        int wingSize = (int)MathHelper.Lerp(100, 300, k / 19f);

                        Rectangle wingSedRect = new Rectangle(0, 0, wingSize, wingSize);

                        spriteBatch.Draw(TextureAssets.MagicPixel.Value, segPos - screenPos, wingSedRect, Color.PaleGoldenrod * 0.1f * NPC.Opacity, 0, wingSedRect.Size() / 2, 1, 0, 0);
                    }
                    PrimitiveRenderer.RenderTrail(wingPoints, new((float f) => (1 - f) * windWidth + 4, (float f) => Color.DarkGoldenrod * NPC.Opacity));
                    PrimitiveRenderer.RenderTrail(wingPoints, new((float f) => (1 - f) * windWidth, (float f) => Color.PaleGoldenrod * NPC.Opacity));
                }
            }

            PrimitiveRenderer.RenderTrail(ribbonDrawPositions, new((float f) => (1 - f) * startWidth + 4, (float f) => Color.DarkGoldenrod * NPC.Opacity));
            PrimitiveRenderer.RenderTrail(ribbonDrawPositions, new((float f) => (1 - f) * startWidth, (float f) => Color.PaleGoldenrod * NPC.Opacity));

            spriteBatch.Draw(tex, NPC.Center - screenPos, null, Color.White * NPC.Opacity, NPC.rotation, tex.Size() / 2, NPC.scale, 0, 0);
            spriteBatch.Draw(bloom, eyePos, null, new Color(194, 175, 189) * NPC.Opacity, NPC.rotation, bloom.Size() / 2, NPC.scale * 1.6f * eyeScale, 0, 0);
            spriteBatch.EnterShaderRegion(BlendState.Additive);
            spriteBatch.Draw(ring, eyePos, null, Color.HotPink * NPC.Opacity, NPC.rotation, ring.Size() / 2, NPC.scale * 0.4f * eyeScale, 0, 0);
            spriteBatch.ExitShaderRegion();
            spriteBatch.Draw(bloom, eyePos, null, new Color(233, 39, 89) * NPC.Opacity, NPC.rotation, bloom.Size() / 2, NPC.scale * 1f * eyeScale, 0, 0);
            spriteBatch.Draw(bloom, eyePos, null, Color.White * NPC.Opacity, NPC.rotation, bloom.Size() / 2, NPC.scale * 0.55f * eyeScale, 0, 0);

            for (int i = 0; i < 7; i++)
            {
                Rectangle newR = i switch
                {
                    0 => new Rectangle(0, 0, 52, 54),
                    1 => new Rectangle(6, 61, 45, 50),
                    2 => new Rectangle(18, 120, 24, 21),
                    3 => new Rectangle(11, 114, 31, 39),
                    4 => new Rectangle(9, 188, 27, 21),
                    5 => new Rectangle(8, 211, 20, 42),
                    6 => new Rectangle(3, 259, 8, 18),
                    _ => new Rectangle(0, 0, 1, 1)
                };
                Vector2 finalPos = Vector2.Lerp(startPos, endPos, i / 6f);
                Vector2 upPos = Vector2.Lerp(startPos, endPos, i / 6f - 0.2f);
                spriteBatch.Draw(sigils, finalPos - screenPos, newR, Color.White * NPC.Opacity, finalPos.DirectionTo(upPos).ToRotation() + MathHelper.PiOver2, newR.Size() / 2, NPC.scale, 0, 0);
            }
            spriteBatch.ExitShaderRegion();
            if (NPC.Opacity > 0)
            {
                for (int i = -1; i <= 1; i += 2)
                {
                    List<Vector2> poses = new();
                    RopeHandle handle = i == -1 ? LeftRibbon.Value : RightRibbon.Value;
                    for (int j = 0; j < handle.SegmentCount; j++)
                    {
                        Vector2 ribPos = handle.Positions.ToList()[j] + (j == 0 ? Vector2.Zero : (Vector2.UnitX * MathF.Cos(i * Main.GlobalTimeWrappedHourly * 2 + j * 0.2f) * MathHelper.Lerp(10, 40, j / 19f)));
                        poses.Add(ribPos);
                        if (j == handle.SegmentCount - 1)
                        {
                            spriteBatch.Draw(bloom, ribPos - screenPos, null, new Color(254, 152, 232) * NPC.Opacity, 0, bloom.Size() / 2, NPC.scale * 0.8f, 0, 0);
                        }
                    }
                    PrimitiveRenderer.RenderTrail(poses, new((float f) => 3, (float f) => Color.DarkGoldenrod * NPC.Opacity));
                }
            }

            return false;
        }
    }
}
