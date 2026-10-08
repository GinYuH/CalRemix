using CalamityMod;
using CalamityMod.Graphics.Primitives;
using CalamityMod.NPCs.Cryogen;
using CalamityMod.NPCs.Providence;
using CalamityMod.Physics;
using CalRemix.Content.Items.Materials;
using CalRemix.Core.Biomes;
using CalRemix.Core.Graphics;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

// So like, technically she's not in the Sealed Dimension, but Horizon is a mechanical extension of it so...
namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class Crevivience : ModNPC
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

        public CreviWingRotation[] creviWings = new CreviWingRotation[4];

        public static Asset<Texture2D> wingTexUpper;
        public static Asset<Texture2D> wingTexLower;
        public static Asset<Texture2D> bodyTexture;

        public override void Load()
        {
            wingTexUpper = Request<Texture2D>(Texture + "WingUpper");
            wingTexLower = Request<Texture2D>(Texture + "WingLower");
            bodyTexture = Request<Texture2D>(Texture + "BodyTest");
        }

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
            //if (mesh != null)
            //mesh.ControlGrid(NPC.Center, 22);
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
            //NPC.velocity = Main.MouseWorld - NPC.Center;
            if (NPC.velocity.X > 0)
            {
                NPC.rotation = Utils.AngleLerp(NPC.rotation, MathHelper.ToRadians(45), 0.2f);
            }
            else if (NPC.velocity.X < 0)
            {
                NPC.rotation = Utils.AngleLerp(NPC.rotation, -MathHelper.ToRadians(45), 0.2f);
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
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D ring = Request<Texture2D>("CalamityMod/Particles/BloomRing").Value;
            Texture2D bloom = Request<Texture2D>("CalamityMod/Particles/Light").Value;

            #region Wings
            bool anyNulls = false;
            int x = 6;
            int y = 6;
            int spaceX = wingTexUpper.Value.Width / x;
            int spaceY = wingTexUpper.Value.Height / y;
            int spaceXL = wingTexLower.Value.Width / x;
            int spaceYL = wingTexLower.Value.Height / y;
            for (int i = 0; i < 4; i++)
            {
                if (creviWings[i] == null)
                {
                    int finalSpaceX = i >= 2 ? spaceX : spaceXL;
                    int finalSpaceY = i >= 2 ? spaceY : spaceYL;
                    creviWings[i] = new CreviWingRotation(TextureMesh.CreateRectangularMesh(Vector3.Zero, x, y, finalSpaceX, finalSpaceY, Color.White));
                    if (i % 2 == 0)
                        creviWings[i].flipped = true;
                    anyNulls = true;
                }
            }
            if (anyNulls)
                return false;
            for (int i = 0; i < 4; i++)
            {
                DrawWing(spriteBatch, screenPos, creviWings[i].mesh, i % 2 == 0, i < 2);
                creviWings[i].DoWingRotation();
            }
            #endregion

            #region Body Drawing
            Vector2 ribbonOffset = -Vector2.UnitY.RotatedBy(NPC.rotation) * -44f;
            float currentSegmentRotation = NPC.rotation;
            List<Vector2> ribbonDrawPositions = new List<Vector2>();
            for (int i = 0; i < 12; i++)
            {
                float ribbonCompletionRatio = i / 12f;
                float wrappedAngularOffset = MathHelper.WrapAngle(NPC.oldRot[i + 1] - currentSegmentRotation) * 0.25f;

                Vector2 ribbonSegmentOffset = Vector2.UnitY.RotatedBy(currentSegmentRotation) * ribbonCompletionRatio * 500;
                ribbonDrawPositions.Add(NPC.Center + ribbonSegmentOffset + ribbonOffset);

                currentSegmentRotation += wrappedAngularOffset;
            }

            Vector2 segmentAreaTopLeft = Vector2.One * 999999f;
            Vector2 segmentAreaTopRight = Vector2.Zero;
            Vector2[] segmentPositions = ribbonDrawPositions.ToArray();

            for (int i = 0; i < segmentPositions.Length; i++)
            {
                if (segmentAreaTopLeft.X > segmentPositions[i].X)
                    segmentAreaTopLeft.X = segmentPositions[i].X;
                if (segmentAreaTopLeft.Y > segmentPositions[i].Y)
                    segmentAreaTopLeft.Y = segmentPositions[i].Y;

                if (segmentAreaTopRight.X < segmentPositions[i].X)
                    segmentAreaTopRight.X = segmentPositions[i].X;
                if (segmentAreaTopRight.Y < segmentPositions[i].Y)
                    segmentAreaTopRight.Y = segmentPositions[i].Y;
            }
            Vector2 primitiveArea = new Vector2(bodyTexture.Value.Width, 0);
            GameShaders.Misc["CalamityMod:PrimitiveTexture"].SetShaderTexture(bodyTexture);
            GameShaders.Misc["CalamityMod:PrimitiveTexture"].Shader.Parameters["uPrimitiveSize"].SetValue(primitiveArea);
            Main.instance.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            PrimitiveRenderer.RenderTrail(ribbonDrawPositions, new((_, _) => 60, (_, _) => Color.White, pixelate: false, shader: GameShaders.Misc["CalamityMod:PrimitiveTexture"]), 80);
            #endregion

            #region Head
            float eyeScale = 0.8f;
            Vector2 eyePos = NPC.Center + Vector2.UnitY.RotatedBy(NPC.rotation) * 20 - screenPos;
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, Color.White * NPC.Opacity, NPC.rotation, tex.Size() / 2, NPC.scale, 0, 0);
            spriteBatch.Draw(bloom, eyePos, null, new Color(194, 175, 189) * NPC.Opacity, NPC.rotation, bloom.Size() / 2, NPC.scale * 1.6f * eyeScale, 0, 0);
            spriteBatch.EnterShaderRegion(BlendState.Additive);
            spriteBatch.Draw(ring, eyePos, null, Color.HotPink * NPC.Opacity, NPC.rotation, ring.Size() / 2, NPC.scale * 0.4f * eyeScale, 0, 0);
            spriteBatch.ExitShaderRegion();
            spriteBatch.Draw(bloom, eyePos, null, new Color(233, 39, 89) * NPC.Opacity, NPC.rotation, bloom.Size() / 2, NPC.scale * 1f * eyeScale, 0, 0);
            spriteBatch.Draw(bloom, eyePos, null, Color.White * NPC.Opacity, NPC.rotation, bloom.Size() / 2, NPC.scale * 0.55f * eyeScale, 0, 0);

            spriteBatch.ExitShaderRegion();
            #endregion

            #region Tendrils
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
                    PrimitiveRenderer.RenderTrail(poses, new((float f, Vector2 v) => 3, (float f, Vector2 v) => Color.DarkGoldenrod * NPC.Opacity));
                }
            }
            #endregion

            return false;
        }

        public static Dictionary<int, List<Vector3>> idSlots3 = new();

        public void DrawWing(SpriteBatch spriteBatch, Vector2 screenPos, TextureMesh which, bool left = false, bool lower = false)
        {
            if (idSlots3.Count == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    idSlots3.Add(i, new List<Vector3>());
                }
            }

            /*if (Main.LocalPlayer.controlUseTile && !Main.LocalPlayer.controlUseItem)
            {
                idSlots3[Main.LocalPlayer.selectedItem].Clear();
                for (int i = 0; i < which.vertices.Length; i++)
                {
                    idSlots3[Main.LocalPlayer.selectedItem].Add(which.vertices[i].Position);
                }
                Main.NewText("Saved slot " + Main.LocalPlayer.selectedItem);
            }
            else if (Main.LocalPlayer.controlUseTile && Main.LocalPlayer.controlUseItem)
            {
                if (idSlots3[Main.LocalPlayer.selectedItem].Count > 0)
                {
                    for (int i = 0; i < which.vertices.Length; i++)
                    {
                        which.vertices[i].Position = Vector3.Lerp(which.vertices[i].Position, idSlots3[Main.LocalPlayer.selectedItem][i], 0.06f);
                    }
                    Main.NewText("Loaded slot " + Main.LocalPlayer.selectedItem);
                }
            }*/
            NPC.rotation = NPC.DirectionTo(Main.MouseWorld).ToRotation() + MathHelper.PiOver2;
            Vector2 realPos = new Vector2(50 * -left.ToDirectionInt(), 30 + (lower ? 150 : 0));
            Vector2 originPos = NPC.Center + realPos.RotatedBy(NPC.rotation) - screenPos;
            Vector2 drawPos = originPos;
            which.DrawMesh(spriteBatch, drawPos, lower ? wingTexLower : wingTexUpper);
            if (Main.LocalPlayer.selectedItem < 5)
                which.DrawDebugGrid(drawPos, spriteBatch);

            //spriteBatch.Draw(TextureAssets.MagicPixel.Value, originPos, new Rectangle(0, 0, 20, 20), Color.Red, 0, new Vector2(10, 10), 1, 0, 0);
        }
    }

    public class CreviWingRotation(TextureMesh mesh)
    {
        public enum PositionType
        {
            IdleBack = 0,
            IdleFront = 1,
            ReadyAttack = 2,
            ExecuteAttack = 3
        }

        public static Dictionary<PositionType, Vector3> PositionRotations => new()
        {
            { PositionType.IdleBack, new Vector3(0, MathHelper.ToRadians(-70), 0) },
            { PositionType.IdleFront, new Vector3(0, MathHelper.ToRadians(30), 0) },
            { PositionType.ReadyAttack, new Vector3(0, MathHelper.ToRadians(-90), 0) },
            { PositionType.ExecuteAttack, new Vector3(0, MathHelper.ToRadians(90), 0) }
        };

        public TextureMesh mesh = mesh;
        public Vector3 newPos = PositionRotations[PositionType.IdleFront];
        public Vector3 oldPos = PositionRotations[PositionType.IdleBack];

        public float wingCompletion = 0;

        public bool flipped = false;

        public void UpdateRotations(PositionType oldPos, PositionType newPos)
        {
            this.oldPos = PositionRotations[oldPos];
            this.newPos = PositionRotations[newPos];
        }

        public void DoWingRotation()
        {
            int idxx = 0;
            foreach (NPC n in Main.ActiveNPCs)
            {
                if (n.type == ModContent.NPCType<Crevivience>())
                {
                    idxx = n.whoAmI;
                }
            }
            Vector3 finale = Vector3.Lerp(oldPos, newPos, MathF.Sin(Main.GlobalTimeWrappedHourly * 12) * 0.5f + 0.5f);
            TextureMesh.RotateGrid(mesh, finale.X, finale.Y * flipped.ToDirectionInt() + (flipped ? MathHelper.Pi : 0), finale.Z + Main.npc[idxx].rotation, new Vector2(0, (mesh.segmentsY - 2) * mesh.segmentHeight));
        }
    }
}
