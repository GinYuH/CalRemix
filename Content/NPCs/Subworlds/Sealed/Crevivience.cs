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

        public CreviWing[] creviWings = new CreviWing[4];

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
            NPC.velocity = Main.MouseWorld - NPC.Center;
            if (NPC.velocity.X > 0)
            {
                //NPC.rotation = Utils.AngleLerp(NPC.rotation, MathHelper.ToRadians(45), 0.2f);
            }
            else if (NPC.velocity.X < 0)
            {
                //NPC.rotation = Utils.AngleLerp(NPC.rotation, -MathHelper.ToRadians(45), 0.2f);
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
                    int wingType = i switch
                    {
                        2 => (int)CreviWing.WingType.Left,
                        3 => (int)CreviWing.WingType.Right,
                        0 => (int)CreviWing.WingType.LowerLeft,
                        1 => (int)CreviWing.WingType.LowerRight,
                        _ => 0
                    };
                    creviWings[i] = new CreviWing(TextureMesh.CreateRectangularMesh(Vector3.Zero, x, y, finalSpaceX, finalSpaceY, Color.White), wingType, NPC.whoAmI);
                    if (wingType == (int)CreviWing.WingType.Left || wingType == (int)CreviWing.WingType.LowerLeft)
                        creviWings[i].flipped = true;
                    anyNulls = true;
                }
            }
            if (anyNulls)
                return false;

            int readyLength = 30;
            int wait = readyLength + 30;
            int execute = wait + 20;
            int end = execute + 60;
            float init = NPC.localAI[3];
            if (NPC.localAI[2] < wait)
            {
                foreach (CreviWing wing in creviWings)
                {
                    wing.PlayAnimationSingular(CreviWing.PositionType.ReadyAttack);
                }
                //Main.NewText("Pulling back!");
                NPC.localAI[3] = MathHelper.Lerp(0, 1, CalamityUtils.ExpOutEasing(Utils.GetLerpValue(0, readyLength, NPC.localAI[2], true), 1));
            }
            else
            {
                //Main.NewText("ONWARDS!");
                foreach (CreviWing wing in creviWings)
                {
                    wing.PlayAnimationSingular(CreviWing.PositionType.ExecuteAttack);
                }
                NPC.localAI[3] = MathHelper.Lerp(0, 1, CalamityUtils.ExpOutEasing(Utils.GetLerpValue(wait, execute, NPC.localAI[2], true), 1));
            }
            if (NPC.localAI[2] > end)
                NPC.localAI[2] = 0;
            NPC.localAI[2]++;

            for (int i = 0; i < 4; i ++)
            {
                if (!creviWings[i].renderAboveCrevi)
                {
                    DrawWing(spriteBatch, screenPos, creviWings[i]);
                    creviWings[i].flipped = false;
                    creviWings[i].DoWingRotation();
                }
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
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, Color.White * NPC.Opacity, NPC.rotation, tex.Size() / 2, NPC.scale, 0, 0);
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

            for (int i = 0; i < 4; i++)
            {
                if (creviWings[i].renderAboveCrevi)
                {
                    DrawWing(spriteBatch, screenPos, creviWings[i]);
                    creviWings[i].DoWingRotation();
                }
            }
            return false;
        }

        public void DrawWing(SpriteBatch spriteBatch, Vector2 screenPos, CreviWing wing)
        {
            Vector2 realPos = new Vector2(50 * -wing.IsLeftWing.ToDirectionInt(), 30 + (wing.IsLowerWing ? 80 : 0));
            Vector2 drawPos = NPC.Center + realPos.RotatedBy(NPC.rotation) - screenPos;
            wing.mesh.DrawMesh(spriteBatch, drawPos, wing.IsLowerWing ? wingTexLower : wingTexUpper);
            if (Main.LocalPlayer.selectedItem < 5)
                wing.mesh.DrawDebugGrid(drawPos, spriteBatch);

            //spriteBatch.Draw(TextureAssets.MagicPixel.Value, drawPos, new Rectangle(0, 0, 70, 70), wing.IsLeftWing ? Color.Indigo : Color.Red, 0, new Vector2(35, 35), 1, 0, 0);
        }
    }

    public class CreviWing(TextureMesh mesh, int wingType, int creviIndex)
    {
        /// <summary>
        /// Used for identifying individual wings
        /// </summary>
        public enum WingType
        {
            Left = 0,
            Right = 1,
            LowerLeft = 2,
            LowerRight = 3,
            UpperLeft = 0,
            UpperRight = 1,
        }
        /// <summary>
        /// Used to identify which wings should be affected by the given animation
        /// </summary>
        public enum AnimType
        {
            UpperLeft = 0,
            UpperRight = 1,
            LowerLeft = 2,
            LowerRight = 3,
            BothLeft = 4,
            BothRight = 5,
            BothLower = 6,
            BothUpper = 7,
            All = 8
        }
        public enum PositionType
        {
            None = -1,
            IdleBack = 0,
            IdleFront = 1,
            ReadyAttack = 2,
            ExecuteAttack = 3
        }

        public int creviIndex = creviIndex;

        public NPC Crevi => Main.npc[creviIndex];

        public bool IsUpperWing => wingType <= WingType.Right;

        public bool IsLowerWing => wingType >= WingType.LowerLeft;

        public bool IsLeftWing => wingType == WingType.Left || wingType == WingType.LowerLeft;

        public bool IsRightWing => wingType == WingType.Right || wingType == WingType.LowerRight;

        /// <summary>
        /// Should this wing draw above Crevi's body?
        /// </summary>
        public bool renderAboveCrevi = false;

        // TREAD - CAN - TIRE
        public static Dictionary<PositionType, CreviWingAnim> PositionRotations => new()
        {
            { PositionType.IdleBack, new CreviWingAnim(AnimType.All, new Vector3(0, MathHelper.ToRadians(-70), 0)) },
            { PositionType.IdleFront, new CreviWingAnim(AnimType.All, new Vector3(0, MathHelper.ToRadians(30), 0)) },
            { PositionType.ReadyAttack, new CreviWingAnim(AnimType.All, new Vector3(MathHelper.ToRadians(10), 0, MathHelper.ToRadians(-100))) },
            { PositionType.ExecuteAttack, new CreviWingAnim(AnimType.All, new Vector3(MathHelper.ToRadians(-180), 0, MathHelper.ToRadians(30))) }
        };

        public TextureMesh mesh = mesh;
        public WingType wingType = (WingType)wingType;
        public Vector3 newPos = PositionRotations[PositionType.IdleFront].desiredPosition;
        public Vector3 oldPos = PositionRotations[PositionType.IdleBack].desiredPosition;
        public PositionType currentAnimation = PositionType.None;

        public float wingCompletion = 0;

        /// <summary>
        /// Is this wing currently drawing flipped?
        /// </summary>
        public bool flipped = false;

        public bool IsAnimating()
        {
            if (currentAnimation == PositionType.None)
                return false;
            CreviWingAnim currentAnimType = PositionRotations[currentAnimation];
            AnimType animType = currentAnimType.whichWings2Anim;
            if (animType == AnimType.All)
                return true;
            if (IsLeftWing && (animType == AnimType.BothLeft || animType == AnimType.LowerLeft || animType == AnimType.UpperLeft))
                return true;
            if (IsRightWing && (animType == AnimType.BothRight || animType == AnimType.LowerRight || animType == AnimType.UpperRight))
                return true;
            if (IsUpperWing && (animType == AnimType.BothUpper || animType == AnimType.UpperRight || animType == AnimType.UpperLeft))
                return true;
            if (IsLowerWing && (animType == AnimType.BothLower || animType == AnimType.LowerLeft || animType == AnimType.LowerRight))
                return true;
            return false;
        }

        public void PlayAnimationSingular(PositionType animationPosition)
        {
            if (newPos != PositionRotations[animationPosition].desiredPosition)
            {
                oldPos = newPos;
                newPos = PositionRotations[animationPosition].desiredPosition;
                currentAnimation = animationPosition;
            }
        }

        public void DoWingRotation()
        {
            if (IsAnimating())
            {
                //Vector3 finale = Vector3.Lerp(PositionRotations[PositionType.IdleBack].desiredPosition, PositionRotations[PositionType.IdleFront].desiredPosition, MathF.Sin(Main.GlobalTimeWrappedHourly) * 0.5f + 0.5f);
                //Main.NewText(MathHelper.ToDegrees(finale.X) + " " + MathHelper.ToDegrees(finale.Y) + " " + MathHelper.ToDegrees(finale.Z));
                Vector3 finale = Vector3.Lerp(oldPos, newPos, Crevi.localAI[3]);
                float origin = IsUpperWing ? (mesh.segmentsY - 2) * mesh.segmentHeight : 2 * mesh.segmentHeight;
                TextureMesh.RotateGrid(mesh, finale.X, finale.Y * flipped.ToDirectionInt() + (flipped ? MathHelper.Pi : 0), finale.Z + Crevi.rotation, new Vector2(0, origin));
            }
        }
    }

    public class CreviWingAnim(CreviWing.AnimType wingType, Vector3 desiredPosition)
    {
        public CreviWing.AnimType whichWings2Anim = wingType;

        public Vector3 desiredPosition = desiredPosition;

        public List<CreviWing.AnimType> wingsToFlip = new();

    }
}
