using CalamityMod;
using CalamityMod.Items.Potions;
using CalamityMod.NPCs.Providence;
using CalamityMod.Particles;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.Utils;
using CalRemix.Content.Items.Pets;
using CalRemix.Content.Particles;
using CalRemix.Core.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using static System.MathF;
using static Microsoft.Xna.Framework.MathHelper;
using static CalRemix.CalRemixHelper;
using CalRemix.Content.Items.Tools;
using CalRemix.Content.Items.Placeables.Relics;
using CalRemix.Content.Items.Placeables.Trophies;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Items.Accessories;
using CalamityMod.DataStructures;

namespace CalRemix.Content.NPCs.Bosses.Noxus.Purity
{
    // My pride and joy of Terraria.

    // I at one point sought to replicate the magic of others. To create something "special" that stands above almost all others in enjoyment and graphical fidelity.
    // I at one point sought to create a Seth of my own. A MEAC Empress. Something so incredible that people would pay attention to it long after it has finished.
    // Something that would elevate me to the point of being a "somebody". A "master".
    // I no longer hold that desire. I have no need to prove myself to this community any longer. I need only to prove myself to myself.
    //
    // I have done exactly that here.
    //
    // And amusingly, with that paradigm shift, the object of the abandoned desire will be realized.
    [AutoloadBossHead]
    public class PurityofNoxus : ModNPC
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override string Texture => "CalRemix/Content/NPCs/Bosses/Noxus/EntropicGod";
        #region Custom Types and Enumerations
        public enum EntropicGodAttackType
        {
            // Phase 1 attacks.
            DarkExplosionCharges,
            DarkEnergyBoltHandWave,
            FireballBarrage,
            HoveringHandGasBursts,
            RapidExplosiveTeleports,
            TeleportAndShootNoxusGas,

            // Phase 2 attacks.
            Phase2Transition,
            GeometricSpikesTeleportAndFireballs,
            ThreeDimensionalNightmareDeathRay,
            PortalChainCharges,
            RealityWarpSpinCharge,
            OrganizedPortalGasBursts,

            // Phase 3 attacks.
            Phase3Transition,
            BrainFogAndThreeDimensionalCharges,
            PortalChainCharges2,

            // Lol lmao get FUCKED Noxus!!!!!!!!! (Cooldown ""attack"")
            MigraineAttack,

            // Self-explanatory.
            DeathAnimation
        }

        public class EntropicGodHand
        {
            public int Frame;

            public int FrameTimer;

            public bool ShouldOpen;

            public float? RotationOverride;

            public Vector2 Center;

            public Vector2 Velocity;

            public Vector2 DefaultOffset;

            public void WriteTo(BinaryWriter writer)
            {
                writer.WriteVector2(Center);
                writer.WriteVector2(Velocity);
                writer.WriteVector2(DefaultOffset);
            }

            public void ReadFrom(BinaryReader reader)
            {
                Center = reader.ReadVector2();
                Velocity = reader.ReadVector2();
                DefaultOffset = reader.ReadVector2();
            }
        }
        #endregion Custom Types and Enumerations

        #region Fields and Properties
        private static NPC myself;

        public EntropicGodHand[] Hands = new EntropicGodHand[2];

        public int CurrentPhase
        {
            get;
            set;
        }

        public int PhaseCycleIndex
        {
            get;
            set;
        }

        public int PortalChainDashCounter
        {
            get;
            set;
        }

        public int BrainFogChargeCounter
        {
            get;
            set;
        }

        public int FightLength
        {
            get;
            set;
        }

        public float LaserSpinDirection
        {
            get;
            set;
        }

        public float LaserTelegraphOpacity
        {
            get;
            set;
        }

        public float LaserSquishFactor
        {
            get;
            set;
        }

        public float LaserLengthFactor
        {
            get;
            set;
        }

        public float FogIntensity
        {
            get;
            set;
        }

        public float FogSpreadDistance
        {
            get;
            set;
        }

        public float HeadSquishiness
        {
            get;
            set;
        }

        public float EyeGleamInterpolant
        {
            get;
            set;
        }

        public float BigEyeOpacity
        {
            get;
            set;
        }

        public Vector2 TeleportPosition
        {
            get;
            set;
        }

        public Vector2 TeleportDirection
        {
            get;
            set;
        }

        public Vector2 PortalArcSpawnCenter
        {
            get;
            set;
        }

        public Vector3 LaserRotation
        {
            get;
            set;
        }

        public float LifeRatio => NPC.life / (float)NPC.lifeMax;

        public Player Target => Main.player[NPC.target];

        public Color GeneralColor => Color.Lerp(Color.White, Color.Black, Clamp(Abs(ZPosition) * 0.35f, 0f, 1f));

        public EntropicGodAttackType CurrentAttack
        {
            get => (EntropicGodAttackType)NPC.ai[0];
            set => NPC.ai[0] = (int)value;
        }

        public ref float AttackTimer => ref NPC.ai[1];

        public ref float SpinAngularOffset => ref NPC.ai[2];

        public ref float ZPosition => ref NPC.ai[3];

        public ref float TeleportVisualsInterpolant => ref NPC.localAI[0];

        public ref float ChargeAfterimageInterpolant => ref NPC.localAI[1];

        public ref float HeadRotation => ref NPC.localAI[2];

        public Vector2 TeleportVisualsAdjustedScale
        {
            get
            {
                float maxStretchFactor = 1.3f;
                Vector2 scale = Vector2.One * NPC.scale;
                if (TeleportVisualsInterpolant > 0f)
                {
                    // 1. Horizontal stretch.
                    if (TeleportVisualsInterpolant <= 0.166f)
                    {
                        float localInterpolant = GetLerpValue(0f, 0.166f, TeleportVisualsInterpolant);
                        scale.X *= Lerp(1f, maxStretchFactor, Sin(Pi * localInterpolant));
                        scale.Y *= Lerp(1f, 0.2f, Sin(Pi * localInterpolant));
                    }
                    
                    // 2. Vertical stretch.
                    else if (TeleportVisualsInterpolant <= 0.333f)
                    {
                        float localInterpolant = GetLerpValue(0.166f, 0.333f, TeleportVisualsInterpolant);
                        scale.X *= Lerp(1f, 0.2f, Sin(Pi * localInterpolant));
                        scale.Y *= Lerp(1f, maxStretchFactor, Sin(Pi * localInterpolant));
                    }

                    // 3. Shrink into nothing on both axes.
                    else if (TeleportVisualsInterpolant <= 0.5f)
                    {
                        float localInterpolant = GetLerpValue(0.333f, 0.5f, TeleportVisualsInterpolant);
                        scale *= Pow(1f - localInterpolant, 4f);
                    }

                    // 4. Return to normal scale, use vertical overshoot at the end.
                    else
                    {
                        float localInterpolant = GetLerpValue(0.5f, 0.73f, TeleportVisualsInterpolant, true);

                        // 1.17234093 = 1 / sin(1.8)^6, acting as a correction factor to ensure that the final scale in the sinusoidal overshoot is one.
                        float verticalScaleOvershot = Pow(Sin(localInterpolant * 1.8f), 6f) * 1.17234093f;
                        scale.X = localInterpolant;
                        scale.Y = verticalScaleOvershot;
                    }
                }
                return scale;
            }
        }

        public bool ShouldDrawBehindTiles => ZPosition >= 0.2f;

        public Vector2 HeadOffset => -Vector2.UnitY.RotatedBy(NPC.rotation) * TeleportVisualsAdjustedScale * 60f;

        public static List<VerletSimulatedSegment> rArm = new List<VerletSimulatedSegment>();

        public static List<VerletSimulatedSegment> lArm = new List<VerletSimulatedSegment>();

        public static NPC Myself
        {
            get
            {
                if (myself is not null && !myself.active)
                    return null;

                return myself;
            }
            private set => myself = value;
        }

        public static int CometDamage => Main.expertMode ? 425 : 275;

        public static int FireballDamage => Main.expertMode ? 400 : 250;

        public static int NoxusGasDamage => Main.expertMode ? 425 : 275;

        public static int SpikeDamage => Main.expertMode ? 400 : 250;

        public static int ExplosionDamage => Main.expertMode ? 450 : 300;

        public static int NightmareDeathrayDamage => Main.expertMode ? 750 : 480;

        public static int DebuffDuration_RegularAttack => CalamityUtils.SecondsToFrames(5f);

        public static int DebuffDuration_PowerfulAttack => CalamityUtils.SecondsToFrames(10f);

        public static int IdealFightDuration => CalamityUtils.SecondsToFrames(180f);

        public static float MaxTimedDRDamageReduction => 0.45f;

        public static readonly Vector2 DefaultHandOffset = new(226f, 108f);

        // Used during the migraine stun behavior.
        public static readonly SoundStyle BrainRotSound = new("CalRemix/Assets/Sounds/Noxus/NoxusBrainRot");

        public static readonly SoundStyle ClapSound = new SoundStyle("CalRemix/Assets/Sounds/Noxus/NoxusClap") with { Volume = 1.5f };

        public static readonly SoundStyle ExplosionSound = new("CalRemix/Assets/Sounds/Noxus/NoxusExplosion");

        public static readonly SoundStyle ExplosionTeleportSound = new("CalRemix/Assets/Sounds/Noxus/NoxusExplosionTeleport");

        public static readonly SoundStyle FireballShootSound = new SoundStyle("CalRemix/Assets/Sounds/Noxus/NoxusFireballShoot") with { Volume = 0.65f, MaxInstances = 20 };

        public static readonly SoundStyle HitSound = new SoundStyle("CalRemix/Assets/Sounds/Noxus/NoxusHurt") with { PitchVariance = 0.4f, Volume = 0.5f };

        public static readonly SoundStyle JumpscareSound = new("CalRemix/Assets/Sounds/Noxus/NoxusJumpscare");

        public static readonly SoundStyle NightmareDeathrayShootSound = new SoundStyle("CalRemix/Assets/Sounds/Noxus/NoxusNightmareDeathray") with { Volume = 1.56f };

        public static readonly SoundStyle ScreamSound = new SoundStyle("CalRemix/Assets/Sounds/Noxus/NoxusScream") with { Volume = 0.45f, MaxInstances = 20 };

        public static readonly SoundStyle TwinkleSound = new SoundStyle("CalRemix/Assets/Sounds/Noxus/NoxusTwinkle") with { MaxInstances = 5, PitchVariance = 0.16f };

        public const int DefaultTeleportDelay = 22;

        public const float Phase2LifeRatio = 0.65f;

        public const float Phase3LifeRatio = 0.25f;

        public const float DefaultDR = 0.23f;

        public const int segmentCount = 22;
        #endregion Fields and Properties

        #region Attack Cycles
        public static EntropicGodAttackType[] Phase1AttackCycle => new EntropicGodAttackType[]
        {
            EntropicGodAttackType.DarkExplosionCharges,
            EntropicGodAttackType.DarkEnergyBoltHandWave,
            EntropicGodAttackType.FireballBarrage,
            EntropicGodAttackType.HoveringHandGasBursts,
            EntropicGodAttackType.MigraineAttack,
            EntropicGodAttackType.RapidExplosiveTeleports,
            EntropicGodAttackType.TeleportAndShootNoxusGas,
            EntropicGodAttackType.DarkExplosionCharges,
            EntropicGodAttackType.FireballBarrage,
            EntropicGodAttackType.MigraineAttack,
            EntropicGodAttackType.RapidExplosiveTeleports,
            EntropicGodAttackType.TeleportAndShootNoxusGas,
            EntropicGodAttackType.DarkEnergyBoltHandWave,
            EntropicGodAttackType.HoveringHandGasBursts,
            EntropicGodAttackType.MigraineAttack
        };

        public static EntropicGodAttackType[] Phase2AttackCycle => new EntropicGodAttackType[]
        {
            EntropicGodAttackType.GeometricSpikesTeleportAndFireballs,
            EntropicGodAttackType.PortalChainCharges,
            EntropicGodAttackType.ThreeDimensionalNightmareDeathRay,
            EntropicGodAttackType.OrganizedPortalGasBursts,
            EntropicGodAttackType.MigraineAttack,
            EntropicGodAttackType.FireballBarrage,
            EntropicGodAttackType.RealityWarpSpinCharge,
            EntropicGodAttackType.TeleportAndShootNoxusGas,
            EntropicGodAttackType.MigraineAttack,
            EntropicGodAttackType.GeometricSpikesTeleportAndFireballs,
            EntropicGodAttackType.ThreeDimensionalNightmareDeathRay,
            EntropicGodAttackType.PortalChainCharges,
            EntropicGodAttackType.OrganizedPortalGasBursts,
            EntropicGodAttackType.MigraineAttack,
            EntropicGodAttackType.RealityWarpSpinCharge,
            EntropicGodAttackType.FireballBarrage,
            EntropicGodAttackType.TeleportAndShootNoxusGas,
            EntropicGodAttackType.MigraineAttack,
        };

        public static EntropicGodAttackType[] Phase3AttackCycle => new EntropicGodAttackType[]
        {
            EntropicGodAttackType.PortalChainCharges,
            EntropicGodAttackType.PortalChainCharges2,
            EntropicGodAttackType.MigraineAttack,
            EntropicGodAttackType.RealityWarpSpinCharge,
            EntropicGodAttackType.BrainFogAndThreeDimensionalCharges,
            EntropicGodAttackType.ThreeDimensionalNightmareDeathRay,
            EntropicGodAttackType.MigraineAttack,
        };
        #endregion Attack Cycles

        #region Initialization
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 1;
            NPCID.Sets.MustAlwaysDraw[Type] = true;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 90;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new()
            {
                Scale = 0.3f,
                PortraitScale = 0.5f
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 50f;
            NPC.damage = 335;
            NPC.width = 122;
            NPC.height = 290;
            NPC.defense = 130;
            NPC.LifeMaxNERB(15088800, 16950000);

            // That is all. Goodbye.
            // No, I will not entertain Master Mode or the difficulty seeds.
            if (CalamityWorld.death)
                NPC.lifeMax = 24955200;

            if (Main.expertMode)
            {
                NPC.damage = 666;

                // Undo vanilla's automatic Expert boosts.
                NPC.lifeMax /= 2;
                NPC.damage /= 2;
            }

            double HPBoost = CalamityServerConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.canGhostHeal = false;
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = null;
            NPC.DeathSound = null;
            NPC.value = Item.buyPrice(50, 0, 0, 0) / 5;
            NPC.netAlways = true;
            NPC.hide = true;
            NPC.Calamity().ShouldCloseHPBar = true;
            InitializeHandsIfNecessary();

            Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/RealityFailureTemp");
        }

        public void InitializeHandsIfNecessary()
        {
            if (Hands is not null && Hands[0] is not null)
                return;

            Hands[0] = new()
            {
                DefaultOffset = DefaultHandOffset * new Vector2(-1f, 1f),
                Center = NPC.Center + DefaultHandOffset * new Vector2(-1f, 1f),
                Velocity = Vector2.Zero
            };
            Hands[1] = new()
            {
                DefaultOffset = DefaultHandOffset,
                Center = NPC.Center + DefaultHandOffset,
                Velocity = Vector2.Zero
            };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement($"Mods.{Mod.Name}.Bestiary.{Name}"),
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement()
            });
        }
        #endregion Initialization

        #region Multiplayer Syncs
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(FightLength);
            writer.Write(PhaseCycleIndex);
            writer.Write(PortalChainDashCounter);
            writer.Write(CurrentPhase);
            writer.Write(BrainFogChargeCounter);
            writer.Write(NPC.Opacity);
            writer.Write(LaserSpinDirection);
            writer.WriteVector2(TeleportPosition);
            writer.WriteVector2(TeleportDirection);
            writer.WriteVector2(PortalArcSpawnCenter);
            writer.Write(LaserRotation.X);
            writer.Write(LaserRotation.Y);
            writer.Write(LaserRotation.Z);

            InitializeHandsIfNecessary();
            Hands[0].WriteTo(writer);
            Hands[1].WriteTo(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            FightLength = reader.ReadInt32();
            PhaseCycleIndex = reader.ReadInt32();
            PortalChainDashCounter = reader.ReadInt32();
            CurrentPhase = reader.ReadInt32();
            BrainFogChargeCounter = reader.ReadInt32();
            NPC.Opacity = reader.ReadSingle();
            LaserSpinDirection = reader.ReadSingle();
            TeleportPosition = reader.ReadVector2();
            TeleportDirection = reader.ReadVector2();
            PortalArcSpawnCenter = reader.ReadVector2();
            LaserRotation = new(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

            InitializeHandsIfNecessary();
            Hands[0].ReadFrom(reader);
            Hands[1].ReadFrom(reader);
        }
        #endregion Multiplayer Syncs

        #region AI
        public override void AI()
        {
            // Pick a target if the current one is invalid.
            bool invalidTargetIndex = NPC.target is < 0 or >= 255;
            if (invalidTargetIndex)
                NPC.TargetClosest();

            bool invalidTarget = Target.dead || !Target.active;
            if (invalidTarget)
                NPC.TargetClosest();

            if (!NPC.WithinRange(Target.Center, 4600f - Target.aggro))
                NPC.TargetClosest();

            // Hey bozo the player's gone. Leave.
            if (Target.dead || !Target.active)
                NPC.active = false;

            // Grant the target infinite flight.
            Target.wingTime = Target.wingTimeMax;

            // Disable rain.
            CalamityMod.CalamityMod.StopRain();

            // Set the global NPC instance.
            Myself = NPC;

            // Reset things every frame.
            NPC.damage = NPC.defDamage;
            NPC.defense = NPC.defDefense;
            NPC.dontTakeDamage = false;
            NPC.ShowNameOnHover = true;
            NPC.Calamity().DR = DefaultDR;

            // Make hands by default close and not use a rotation override.
            for (int i = 0; i < Hands.Length; i++)
            {
                Hands[i].ShouldOpen = false;
                Hands[i].RotationOverride = null;
            }

            // Make the head spin back into place.
            HeadRotation = HeadRotation.AngleTowards(0f, 0.02f);
            HeadSquishiness = Clamp(HeadSquishiness - 0.02f, 0f, 0.5f);

            // Ensure that the player receives the boss effects buff.
            NPC.Calamity().KillTime = IdealFightDuration;

            // Do not despawn.
            NPC.timeLeft = 7200;

            // Make the charge afterimage interpolant dissipate.
            ChargeAfterimageInterpolant = Clamp(ChargeAfterimageInterpolant * 0.98f - 0.02f, 0f, 1f);

            // Make the laser telegraph opacity dissipate. This is useful for cases where Noxus changes phases in the middle of the telegraph being prepared.
            LaserTelegraphOpacity = Clamp(LaserTelegraphOpacity - 0.01f, 0f, 1f);

            // Initialize arm verlet

            if (rArm == null || rArm.Count < segmentCount)
            {
                rArm = new List<VerletSimulatedSegment>(segmentCount);
                for (int i = 0; i < segmentCount; i++)
                {
                    VerletSimulatedSegment segment = new VerletSimulatedSegment(NPC.Center);
                    rArm.Add(segment);
                }

                rArm[0].locked = true;
                rArm[^1].locked = true;
            }
            if (lArm == null || lArm.Count < segmentCount)
            {
                lArm = new List<VerletSimulatedSegment>(segmentCount);
                for (int i = 0; i < segmentCount; i++)
                {
                    VerletSimulatedSegment segment = new VerletSimulatedSegment(NPC.Center);
                    lArm.Add(segment);
                }

                lArm[0].locked = true;
                lArm[^1].locked = true;
            }

            switch (CurrentAttack)
            {
                case EntropicGodAttackType.DarkExplosionCharges:
                    DoBehavior_DarkExplosionCharges();
                    break;
                case EntropicGodAttackType.DarkEnergyBoltHandWave:
                    DoBehavior_DarkEnergyBoltHandWave();
                    break;
                case EntropicGodAttackType.FireballBarrage:
                    DoBehavior_FireballBarrage();
                    break;
                case EntropicGodAttackType.RealityWarpSpinCharge:
                    DoBehavior_RealityWarpSpinCharge();
                    break;
                case EntropicGodAttackType.OrganizedPortalGasBursts:
                    DoBehavior_OrganizedPortalGasBursts();
                    break;
                case EntropicGodAttackType.HoveringHandGasBursts:
                    DoBehavior_HoveringHandGasBursts();
                    break;
                case EntropicGodAttackType.RapidExplosiveTeleports:
                    DoBehavior_RapidExplosiveTeleports();
                    break;
                case EntropicGodAttackType.Phase2Transition:
                    DoBehavior_Phase2Transition();
                    break;
                case EntropicGodAttackType.GeometricSpikesTeleportAndFireballs:
                    DoBehavior_GeometricSpikesTeleportAndFireballs();
                    break;
                case EntropicGodAttackType.TeleportAndShootNoxusGas:
                    DoBehavior_TeleportAndShootNoxusGas();
                    break;
                case EntropicGodAttackType.ThreeDimensionalNightmareDeathRay:
                    DoBehavior_ThreeDimensionalNightmareDeathRay();
                    break;
                case EntropicGodAttackType.Phase3Transition:
                    DoBehavior_Phase3Transition();
                    break;
                case EntropicGodAttackType.BrainFogAndThreeDimensionalCharges:
                    DoBehavior_BrainFogAndThreeDimensionalCharges();
                    break;
                case EntropicGodAttackType.PortalChainCharges:
                    DoBehavior_PortalChainCharges();
                    break;
                case EntropicGodAttackType.PortalChainCharges2:
                    DoBehavior_PortalChainCharges2();
                    break;
                case EntropicGodAttackType.MigraineAttack:
                    DoBehavior_MigraineAttack();
                    break;
                case EntropicGodAttackType.DeathAnimation:
                    DoBehavior_DeathAnimation();
                    break;
            }

            // Handle phase transition triggers.
            PreparePhaseTransitionsIfNecessary();

            // Update all hands.
            UpdateHands();

            // Perform Z position visual effects.
            PerformZPositionEffects();

            // Disable damage when invisible.
            if (NPC.Opacity <= 0.35f)
            {
                NPC.dontTakeDamage = true;
                NPC.damage = 0;
            }

            // Rotate slightly based on horizontal movement.
            bool teleported = NPC.position.Distance(NPC.oldPosition) >= 80f;
            NPC.rotation = Clamp((NPC.position.X - NPC.oldPosition.X) * 0.0024f, -0.16f, 0.16f);
            if (teleported)
                NPC.rotation = 0f;

            // Emit pitch black metaballs around based on movement.
            else if (NPC.Opacity >= 0.5f)
            {
                int metaballSpawnLoopCount = (int)Remap(NPC.Opacity, 1f, 0f, 9f, 1f) - (int)Remap(ZPosition, 0.1f, 1.2f, 0f, 5f);

                for (int i = 0; i < metaballSpawnLoopCount; i++)
                {
                    Vector2 gasSpawnPosition = NPC.Center + Main.rand.NextVector2Circular(82f, 82f) * TeleportVisualsAdjustedScale + (NPC.position - NPC.oldPosition).SafeNormalize(Vector2.UnitY) * 3f;
                    float gasSize = NPC.width * TeleportVisualsAdjustedScale.X * NPC.Opacity * 0.45f;
                    float angularOffset = Sin(Main.GlobalTimeWrappedHourly * 1.1f) * 0.77f;
                    PitchBlackMetaball.CreateParticle(gasSpawnPosition, Main.rand.NextVector2Circular(2f, 2f) + NPC.velocity.RotatedBy(angularOffset).RotatedByRandom(0.6f) * 0.26f, gasSize);
                }
            }

            // Gain permanent afterimages if in phase 2 and onward.
            if (CurrentPhase >= 1)
                ChargeAfterimageInterpolant = 1f;

            AttackTimer++;
            FightLength++;

            float armDistX = 80 * NPC.scale;
            float armDistY = 50 * NPC.scale;

            rArm[0].oldPosition = rArm[0].position;
            rArm[0].position = NPC.Center + new Vector2(-armDistX, armDistY);

            rArm[rArm.Count - 1].oldPosition = rArm[^1].position;
            rArm[rArm.Count - 1].position = (Hands[0].Center + (Hands[0].RotationOverride == null ? (new Vector2(0, -30 * NPC.scale)) : ((float)Hands[0].RotationOverride * Vector2.One * 22 * NPC.scale)));

            rArm = VerletSimulatedSegment.SimpleSimulation(rArm, 2 * NPC.scale, loops: segmentCount, gravity: 22f);


            lArm[0].oldPosition = rArm[0].position;
            lArm[0].position = NPC.Center + new Vector2(armDistX , armDistY);

            lArm[lArm.Count - 1].oldPosition = lArm[^1].position;
            lArm[lArm.Count - 1].position = (Hands[1].Center + (Hands[1].RotationOverride == null ? (new Vector2(0, -30 * NPC.scale)) : ((float)Hands[1].RotationOverride * Vector2.One * 22 * NPC.scale)));

            lArm = VerletSimulatedSegment.SimpleSimulation(lArm, 2 * NPC.scale, loops: segmentCount, gravity: 22f);
        }

        public void DoBehavior_DarkExplosionCharges()
        {
            int chargeDelay = 40;
            int chargeTime = 41;
            int explosionCreationRate = 10;
            int chargeTeleportCount = 3;
            int wrappedAttackTimer = (int)AttackTimer % (DefaultTeleportDelay + chargeDelay + chargeTime);
            float initialChargeSpeed = 6f;
            float chargeAcceleration = 1.1f;
            float maxChargeSpeed = 62f;
            Vector2 leftHandDestination = NPC.Center + Hands[0].DefaultOffset * NPC.scale;
            Vector2 rightHandDestination = NPC.Center + Hands[1].DefaultOffset * NPC.scale;
            ref Vector2 closestHandToTargetDestination = ref leftHandDestination;
            if (Target.Center.X >= NPC.Center.X)
                closestHandToTargetDestination = ref rightHandDestination;

            // Go to the next attack state once all charges have been performed.
            if (AttackTimer >= (DefaultTeleportDelay + chargeDelay + chargeTime) * chargeTeleportCount)
            {
                SelectNextAttack();
                return;
            }

            // Slow down in anticipation of the teleport. Teleport visual effects are executed later on.
            if (wrappedAttackTimer <= DefaultTeleportDelay)
            {
                NPC.velocity *= 0.8f;

                // Teleport near the target when ready.
                if (wrappedAttackTimer == DefaultTeleportDelay)
                {
                    Vector2 hoverDestination = Target.Center + new Vector2((Target.Center.X < NPC.Center.X).ToDirectionInt() * 480f, -420f);
                    TeleportTo(hoverDestination);
                }
            }

            // Raise the hand closest to the player up in anticipation of the charge.
            else if (wrappedAttackTimer <= DefaultTeleportDelay + chargeDelay)
            {
                float anticipationInterpolant = GetLerpValue(0f, chargeDelay, wrappedAttackTimer - DefaultTeleportDelay, true);

                closestHandToTargetDestination.X += Sign(NPC.Center.X - closestHandToTargetDestination.X) * Pow(anticipationInterpolant, 3.5f) * 180f;
                closestHandToTargetDestination.Y -= Pow(anticipationInterpolant, 2.6f) * 360f;

                // Cease all movement.
                NPC.velocity = Vector2.Zero;
            }

            // Do charge effects.
            else if (wrappedAttackTimer <= DefaultTeleportDelay + chargeDelay + chargeTime)
            {
                // Perform the charge.
                if (wrappedAttackTimer == DefaultTeleportDelay + chargeDelay + 1f)
                {
                    NPC.velocity = NPC.SafeDirectionTo(Target.Center) * initialChargeSpeed;
                    NPC.netUpdate = true;
                }

                // Accelerate.
                if (NPC.velocity.Length() < maxChargeSpeed)
                    NPC.velocity *= chargeAcceleration;

                // Arc a little bit at first.
                if (wrappedAttackTimer <= DefaultTeleportDelay + chargeDelay + 16f)
                {
                    float idealDirection = NPC.AngleTo(Target.Center);
                    float currentDirection = NPC.velocity.ToRotation();
                    NPC.velocity = NPC.velocity.RotatedBy(WrapAngle(idealDirection - currentDirection) * 0.12f);
                }

                // Make hands move towards the target, as if they're attempting to grab them.
                leftHandDestination = NPC.Center + (Target.Center - Hands[0].Center).SafeNormalize(Vector2.UnitY) * 300f;
                rightHandDestination = NPC.Center + (Target.Center - Hands[1].Center).SafeNormalize(Vector2.UnitY) * 300f;
                MakeHandsOpen();

                // Periodically create explosions.
                if (wrappedAttackTimer % explosionCreationRate == explosionCreationRate - 1f)
                {
                    SoundEngine.PlaySound(SoundID.Item72, NPC.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        NewProjectileBetter(NPC.Center, Vector2.Zero, ModContent.ProjectileType<NoxusExplosion>(), ExplosionDamage, 0f);
                }

                ChargeAfterimageInterpolant = GetLerpValue(0f, 8f, AttackTimer - DefaultTeleportDelay - chargeDelay, true);
            }

            // Prepare teleport scaling visual effects.
            TeleportVisualsInterpolant = GetLerpValue(0f, DefaultTeleportDelay * 2f, wrappedAttackTimer, true);

            // Become invisible in accordance with how far along the teleport visuals are.
            NPC.Opacity = GetLerpValue(0.34f, 0.03f, TeleportVisualsInterpolant, true) + GetLerpValue(0.56f, 0.84f, TeleportVisualsInterpolant, true);

            // Move hands.
            DefaultHandDrift(Hands[0], leftHandDestination);
            DefaultHandDrift(Hands[1], rightHandDestination);
        }

        public void DoBehavior_DarkEnergyBoltHandWave()
        {
            int handArcTime = 50;
            int spikeReleaseRate = 2;
            int telegraphTime = NoxSpike.TelegraphTime;
            int attackTransitionDelay = 90;
            float handSpeedFactor = 1.8f;
            float maxArcAngle = ToRadians(160f);

            if (Main.expertMode)
            {
                spikeReleaseRate--;
                handArcTime -= 10;
                handSpeedFactor += 1.1f;
            }

            Vector2 leftHandDestination = NPC.Center + Hands[0].DefaultOffset * NPC.scale;
            Vector2 rightHandDestination = NPC.Center + Hands[1].DefaultOffset * NPC.scale;

            // Disable contact damage. It is not relevant for this attack.
            NPC.damage = 0;

            // Slow down and begin to teleport above the player. This also involves waiting after the teleport before attacking.
            if (AttackTimer <= DefaultTeleportDelay * 2f)
            {
                NPC.velocity *= 0.8f;

                // Raise hands as the teleport happens.
                float teleportWaitInterpolant = GetLerpValue(0f, DefaultTeleportDelay - 9f, AttackTimer, true);
                if (AttackTimer >= DefaultTeleportDelay + 1f)
                    teleportWaitInterpolant = 0f;
                else
                    handSpeedFactor += 0.75f;

                float verticalOffset = Pow(teleportWaitInterpolant, 1.7f) * 400f;
                leftHandDestination.Y -= verticalOffset;
                rightHandDestination.Y -= verticalOffset;

                // Teleport near the target when ready.
                if (AttackTimer == DefaultTeleportDelay)
                {
                    Vector2 hoverDestination = Target.Center - Vector2.UnitY * 540f;
                    TeleportTo(hoverDestination);
                }
            }

            // Have the hand stay below Noxus before moving outward to create a slightly greater than 90 degree arc in both directions.
            // Spikes are created when this happens, but they don't initially move.
            else if (AttackTimer <= DefaultTeleportDelay * 2f + handArcTime)
            {
                float arcInterpolant = GetLerpValue(11f, handArcTime - 8f, AttackTimer - DefaultTeleportDelay * 2f, true);
                leftHandDestination = CalculateHandOffsetForClapAnticipation(arcInterpolant, maxArcAngle, true);
                rightHandDestination = CalculateHandOffsetForClapAnticipation(arcInterpolant, maxArcAngle, false);
                MakeHandsOpen();

                // Release the spikes.
                // (((((OK yeah technically they move but it's only for rotation and so slow that it's negligible)))))
                if (AttackTimer % spikeReleaseRate == spikeReleaseRate - 1f && AttackTimer >= DefaultTeleportDelay * 2f + 16f)
                {
                    int telegraphDelay = (int)AttackTimer - (DefaultTeleportDelay * 2 + handArcTime);
                    NewProjectileBetter(Hands[0].Center, (Hands[0].Center - NPC.Center).SafeNormalize(Vector2.UnitY) * 0.0001f, ModContent.ProjectileType<NoxSpike>(), SpikeDamage, 0f, -1, 0f, telegraphDelay);
                    NewProjectileBetter(Hands[1].Center, (Hands[1].Center - NPC.Center).SafeNormalize(Vector2.UnitY) * 0.0001f, ModContent.ProjectileType<NoxSpike>(), SpikeDamage, 0f, -1, 0f, telegraphDelay);
                }
            }

            // Hold the hands in place after the spikes have been cast.
            else if (AttackTimer <= DefaultTeleportDelay * 2f + handArcTime + telegraphTime)
            {
                leftHandDestination = NPC.Center + Vector2.UnitY.RotatedBy(maxArcAngle) * 250f + Vector2.UnitX * -8f;
                rightHandDestination = NPC.Center + Vector2.UnitY.RotatedBy(-maxArcAngle) * 250f + Vector2.UnitX * 8f;
            }

            // Prepare teleport scaling visual effects.
            TeleportVisualsInterpolant = GetLerpValue(0f, DefaultTeleportDelay * 2f, AttackTimer, true);

            // Clap hands and make all spikes fire in their projected paths.
            if (AttackTimer >= DefaultTeleportDelay * 2f + handArcTime + telegraphTime - 5f)
            {
                if (AttackTimer == DefaultTeleportDelay * 2f + handArcTime + telegraphTime)
                {
                    SoundEngine.PlaySound(ClapSound, Target.Center);
                    ScreenEffectSystem.SetBlurEffect((Hands[0].Center + Hands[1].Center) * 0.5f, 0.7f, 27);

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        NewProjectileBetter((Hands[0].Center + Hands[1].Center) * 0.5f, Vector2.Zero, ModContent.ProjectileType<DarkWave>(), 0, 0f);
                }

                // Fly around above the player after the spikes have been fired.
                if (AttackTimer >= DefaultTeleportDelay * 2f + handArcTime + telegraphTime + 30f)
                    BasicFlyMovement(Target.Center - Vector2.UnitY * 333f);
                else
                {
                    handSpeedFactor *= 3f;
                    leftHandDestination = NPC.Center + new Vector2(8f, 150f);
                    rightHandDestination = NPC.Center + new Vector2(-8f, 150f);
                }
            }

            if (AttackTimer >= DefaultTeleportDelay * 2f + handArcTime + telegraphTime + attackTransitionDelay)
                SelectNextAttack();

            // Move hands.
            DefaultHandDrift(Hands[0], leftHandDestination, handSpeedFactor);
            DefaultHandDrift(Hands[1], rightHandDestination, handSpeedFactor);
        }

        public void DoBehavior_FireballBarrage()
        {
            int teleportDelay = 15;
            int chargeDelay = 27;
            int chargeTime = 54;
            int chargeTeleportCount = 2;
            int wrappedAttackTimer = (int)AttackTimer % (teleportDelay + chargeDelay + chargeTime);
            int chargeCounter = (int)AttackTimer / (teleportDelay + chargeDelay + chargeTime);
            int fireballShootRate = 7;
            float initialChargeSpeed = 6.7f;
            float chargeAcceleration = 1.08f;
            float maxChargeSpeed = 54f;
            float handSpeedFactor = 1f;
            float fireballShootSpeed = 9f;

            if (Main.expertMode)
            {
                initialChargeSpeed += 1.2f;
                fireballShootSpeed += 2f;
            }
            if (CalamityWorld.revenge)
            {
                fireballShootRate--;
                chargeAcceleration += 0.032f;
            }

            bool teleportBelowTarget = chargeCounter % 2f == 0f;
            Vector2 leftHandDestination = NPC.Center + Hands[0].DefaultOffset * NPC.scale;
            Vector2 rightHandDestination = NPC.Center + Hands[1].DefaultOffset * NPC.scale;

            // Go to the next attack state once all charges have been performed.
            if (AttackTimer >= (teleportDelay + chargeDelay + chargeTime) * chargeTeleportCount)
            {
                NPC.velocity *= 0.92f;
                if (AttackTimer >= (teleportDelay + chargeDelay + chargeTime) * chargeTeleportCount + 32f)
                    SelectNextAttack();
                return;
            }

            // Slow down in anticipation of the teleport. Teleport visual effects are executed later on.
            if (wrappedAttackTimer <= teleportDelay)
                NPC.velocity *= 0.8f;

            // Teleport to the side of the player and wait before charging.
            else if (wrappedAttackTimer <= teleportDelay + chargeDelay)
            {
                if (wrappedAttackTimer == teleportDelay + 1f)
                {
                    Vector2 teleportPosition = Target.Center + new Vector2(Target.direction * -840f, teleportBelowTarget.ToDirectionInt() * 424f);
                    TeleportTo(teleportPosition);

                    SoundEngine.PlaySound(ExplosionTeleportSound with
                    {
                        Volume = 0.7f
                    }, Target.Center);
                }

                NPC.velocity = Vector2.Zero;
            }

            // Charge and release fireballs.
            else if (wrappedAttackTimer <= teleportDelay + chargeDelay + chargeTime)
            {
                // Charge horizontally.
                if (wrappedAttackTimer == teleportDelay + chargeDelay + 1f)
                {
                    NPC.velocity = Vector2.UnitX * Sign(NPC.SafeDirectionTo(Target.Center).X) * initialChargeSpeed;
                    NPC.netUpdate = true;
                }

                // Use afterimages.
                ChargeAfterimageInterpolant = GetLerpValue(teleportDelay + chargeDelay, teleportDelay + chargeDelay + 7f, wrappedAttackTimer, true);

                // Accelerate.
                if (NPC.velocity.Length() < maxChargeSpeed)
                    NPC.velocity *= chargeAcceleration;

                // Release fireballs.
                if ((wrappedAttackTimer % fireballShootRate == 0f || Distance(NPC.Center.X, Target.Center.X) <= 120f))
                {
                    SoundEngine.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy with
                    {
                        Pitch = -Main.rand.NextFloat(0.2f, 0.5f),
                        Volume = 0.8f,
                        MaxInstances = 50
                    }, Target.Center);

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 fireballShootVelocity = -Vector2.UnitY.RotatedByRandom(0.2f) * teleportBelowTarget.ToDirectionInt() * fireballShootSpeed;
                        NewProjectileBetter(NPC.Center + fireballShootVelocity * 10f, fireballShootVelocity, ModContent.ProjectileType<DarkFireball>(), FireballDamage, 0f, -1, 0f, 60f);
                    }
                }
            }

            // Prepare teleport scaling visual effects.
            TeleportVisualsInterpolant = GetLerpValue(0f, teleportDelay * 2f, wrappedAttackTimer, true);

            // Become invisible in accordance with how far along the teleport visuals are.
            NPC.Opacity = GetLerpValue(0.34f, 0.03f, TeleportVisualsInterpolant, true) + GetLerpValue(0.56f, 0.84f, TeleportVisualsInterpolant, true);

            // Make hands raise in the direction of the player as time goes on.
            float handRaiseInterpolant = GetLerpValue(teleportDelay + 4f, teleportDelay + chargeDelay + 9f, wrappedAttackTimer, true);
            float verticalHandOffset = handRaiseInterpolant * teleportBelowTarget.ToDirectionInt() * 100f;
            leftHandDestination.Y -= verticalHandOffset;
            rightHandDestination.Y -= verticalHandOffset;
            handSpeedFactor += handRaiseInterpolant * 1.6f;

            // Move hands.
            DefaultHandDrift(Hands[0], leftHandDestination, handSpeedFactor);
            DefaultHandDrift(Hands[1], rightHandDestination, handSpeedFactor);
        }

        public void DoBehavior_RealityWarpSpinCharge()
        {
            int teleportDelay = 15;
            int spinDelay = 16;
            int spinTime = 76;
            int chargeTime = 48;
            int slowDarkCometCount = 5;
            int fastDarkCometCount = 7;
            float darkCometSpread = ToRadians(72f);
            float fastDarkCometShootSpeed = 5f;
            float spinRadius = 600f;
            float maxSpinSpeed = ToRadians(10f);
            float handSpeedFactor = 4f;
            float startingChargeSpeed = 3f;

            if (Main.expertMode)
            {
                spinTime -= 10;
                maxSpinSpeed *= 1.333f;
            }

            if (CalamityWorld.revenge)
            {
                spinTime -= 5;
                startingChargeSpeed += 0.95f;
                fastDarkCometShootSpeed += 2.2f;
            }

            if (CurrentPhase >= 2)
            {
                spinTime -= 22;
                maxSpinSpeed *= 1.5f;
                startingChargeSpeed += 1.2f;
            }

            float slowDarkCometShootSpeed = fastDarkCometShootSpeed * 0.56f;
            Vector2 leftHandDestination = NPC.Center + Hands[0].DefaultOffset * NPC.scale;
            Vector2 rightHandDestination = NPC.Center + Hands[1].DefaultOffset * NPC.scale;

            // Go to the next attack state once all charges have been performed.
            if (AttackTimer >= (teleportDelay + spinDelay + spinTime + chargeTime))
            {
                NPC.Opacity = 1f;
                SelectNextAttack();
                return;
            }

            // Slow down in anticipation of the teleport. Teleport visual effects are executed later on.
            // Also delete leftover projectiles, since they're not gonna be easy to see after the screen shatter.
            if (AttackTimer <= teleportDelay)
            {
                NPC.velocity *= 0.8f;
                ClearAllProjectiles();
            }

            // Teleport to the side of the player and wait before charging.
            else if (AttackTimer <= teleportDelay + spinDelay)
            {
                if (AttackTimer == teleportDelay + 1f)
                {
                    SpinAngularOffset = Main.rand.NextFloat(TwoPi);
                    TeleportTo(Target.Center + SpinAngularOffset.ToRotationVector2() * spinRadius);
                    ScreenShatterSystem.CreateShatterEffect(NPC.Center - Main.screenPosition);

                    SoundEngine.PlaySound(ExplosionTeleportSound with
                    {
                        Volume = 0.7f
                    }, Target.Center);
                }

                NPC.velocity = Vector2.Zero;
            }

            // Spin around the player.
            if (AttackTimer >= teleportDelay && AttackTimer <= teleportDelay + spinDelay + spinTime)
            {
                float spinInterpolant = GetLerpValue(teleportDelay + spinDelay, teleportDelay + spinDelay + spinTime, AttackTimer, true);
                float spinSpeed = MathHelper.SmoothStep(0f, maxSpinSpeed, GetLerpValue(0f, 0.35f, spinInterpolant, true) * GetLerpValue(0.98f, 0.6f, spinInterpolant, true));
                SpinAngularOffset += spinSpeed;
                NPC.Center = Target.Center + SpinAngularOffset.ToRotationVector2() * spinRadius;

                // Make the afterimages appear.
                ChargeAfterimageInterpolant = GetLerpValue(0f, 0.1f, spinInterpolant, true);
            }

            // Charge at the player and release bursts of dark comets.
            if (AttackTimer >= teleportDelay + spinDelay + spinTime)
            {
                ChargeAfterimageInterpolant = 1f;

                // Curve towards the player before accelerating.
                Vector2 idealDirection = NPC.SafeDirectionTo(Target.Center);
                if (AttackTimer >= teleportDelay + spinDelay + spinTime + 7f)
                    idealDirection = NPC.velocity.SafeNormalize(Vector2.UnitY);
                if (AttackTimer == teleportDelay + spinDelay + spinTime + 1f)
                    NPC.velocity = idealDirection * startingChargeSpeed;
                NPC.velocity *= 1.1f;

                // Perform clap effects and release dark comets.
                if (AttackTimer == teleportDelay + spinDelay + spinTime + 3f)
                {
                    SoundEngine.PlaySound(ClapSound, Target.Center);
                    ScreenEffectSystem.SetBlurEffect((Hands[0].Center + Hands[1].Center) * 0.5f, 0.7f, 27);

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        NewProjectileBetter((Hands[0].Center + Hands[1].Center) * 0.5f, Vector2.Zero, ModContent.ProjectileType<DarkWave>(), 0, 0f);

                    // Shoot the comets.
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 cometSpawnPosition = (Hands[0].Center + Hands[1].Center) * 0.5f;
                        for (int i = 0; i < slowDarkCometCount; i++)
                        {
                            float localDarkCometSpread = Lerp(-darkCometSpread, darkCometSpread, i / (float)(slowDarkCometCount - 1f));
                            Vector2 darkCometShootVelocity = NPC.SafeDirectionTo(Target.Center).RotatedBy(localDarkCometSpread) * slowDarkCometShootSpeed;
                            NewProjectileBetter(cometSpawnPosition, darkCometShootVelocity, ModContent.ProjectileType<DarkComet>(), NPC.damage, 0f);
                        }
                        for (int i = 0; i < fastDarkCometCount; i++)
                        {
                            float localDarkCometSpread = Lerp(-darkCometSpread, darkCometSpread, i / (float)(fastDarkCometCount - 1f));
                            Vector2 darkCometShootVelocity = NPC.SafeDirectionTo(Target.Center).RotatedBy(localDarkCometSpread) * fastDarkCometShootSpeed;
                            NewProjectileBetter(cometSpawnPosition, darkCometShootVelocity, ModContent.ProjectileType<DarkComet>(), NPC.damage, 0f);
                        }
                    }
                }

                handSpeedFactor *= 2f;
                leftHandDestination = NPC.Center + new Vector2(8f, 150f);
                rightHandDestination = NPC.Center + new Vector2(-8f, 150f);

                // Fade away.
                if (NPC.velocity.Length() >= 12f)
                    NPC.Opacity = Clamp(NPC.Opacity - 0.03f, 0f, 1f);
            }

            // Move hands.
            DefaultHandDrift(Hands[0], leftHandDestination, handSpeedFactor);
            DefaultHandDrift(Hands[1], rightHandDestination, handSpeedFactor);
        }

        public void DoBehavior_OrganizedPortalGasBursts()
        {
            int handRaiseTime = 26;
            int portalCastTime = 26;
            int portalSummonRate = 3;
            int portalLingerTime = 70;
            int attackTransitionDelay = 150;
            float handSpeedFactor = 1.96f;
            float portalScale = 0.8f;
            Vector2 leftHandDestination = NPC.Center + Hands[0].DefaultOffset * NPC.scale;
            Vector2 rightHandDestination = NPC.Center + Hands[1].DefaultOffset * NPC.scale;

            // Disable contact damage. It is not relevant for this attack.
            NPC.damage = 0;

            // Slow down and begin to teleport above the player. This also involves waiting after the teleport before attacking.
            if (AttackTimer <= DefaultTeleportDelay * 2f)
            {
                NPC.velocity *= 0.8f;

                // Raise hands as the teleport happens.
                float teleportWaitInterpolant = GetLerpValue(0f, DefaultTeleportDelay - 9f, AttackTimer, true);
                if (AttackTimer >= DefaultTeleportDelay + 1f)
                    teleportWaitInterpolant = 0f;
                else
                    handSpeedFactor += 0.75f;

                float verticalOffset = Pow(teleportWaitInterpolant, 1.74f) * 330f;
                leftHandDestination.Y -= verticalOffset;
                rightHandDestination.Y -= verticalOffset;

                // Teleport near the target when ready.
                if (AttackTimer == DefaultTeleportDelay)
                {
                    Vector2 hoverDestination = Target.Center - Vector2.UnitY * 350f;
                    TeleportTo(hoverDestination);
                }
            }

            // Raise hands before summoning the portals.
            else if (AttackTimer <= DefaultTeleportDelay * 2f + handRaiseTime)
            {
                float handRaiseInterpolant = GetLerpValue(DefaultTeleportDelay * 2f, DefaultTeleportDelay * 2f + handRaiseTime - 6f, AttackTimer, true);
                leftHandDestination = Vector2.Lerp(leftHandDestination, NPC.Center + new Vector2(-80f, -200f) * NPC.scale, handRaiseInterpolant);
                rightHandDestination = Vector2.Lerp(rightHandDestination, NPC.Center + new Vector2(80f, -200f) * NPC.scale, handRaiseInterpolant);

                PortalArcSpawnCenter = Target.Center;
            }

            // Move hands in an arc and summon portals to the sides of the target.
            else if (AttackTimer < DefaultTeleportDelay * 2f + handRaiseTime + portalCastTime)
            {
                float arcInterpolant = GetLerpValue(DefaultTeleportDelay * 2f + handRaiseTime, DefaultTeleportDelay * 2f + handRaiseTime + portalCastTime - 15f, AttackTimer, true);
                float horizontalArcOffset = CalamityUtils.Convert01To010(arcInterpolant);
                leftHandDestination = NPC.Center + new Vector2(-horizontalArcOffset * 110f - 80f, arcInterpolant * 400f - 200f) * NPC.scale;
                rightHandDestination = NPC.Center + new Vector2(horizontalArcOffset * 110f + 80f, arcInterpolant * 400f - 200f) * NPC.scale;

                // Summon two portals above and below the target.
                if (AttackTimer == DefaultTeleportDelay * 2f + handRaiseTime + 4f)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 portalSpawnPosition = PortalArcSpawnCenter - Vector2.UnitY * 400f;
                        NewProjectileBetter(portalSpawnPosition, (PortalArcSpawnCenter - portalSpawnPosition).SafeNormalize(Vector2.UnitY), ModContent.ProjectileType<DarkPortal>(), 0, 0f, -1, portalScale, portalLingerTime);

                        portalSpawnPosition = PortalArcSpawnCenter + Vector2.UnitY * 400f;
                        NewProjectileBetter(portalSpawnPosition, (PortalArcSpawnCenter - portalSpawnPosition).SafeNormalize(Vector2.UnitY), ModContent.ProjectileType<DarkPortal>(), 0, 0f, -1, portalScale, portalLingerTime);
                    }
                }

                // Summon the portals.
                if (AttackTimer % portalSummonRate == 0f)
                {
                    SoundEngine.PlaySound(FireballShootSound with { MaxInstances = 10 }, Target.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 portalSpawnPosition = PortalArcSpawnCenter + new Vector2(-720f - horizontalArcOffset * 240f, -560f + arcInterpolant * 1120f);
                        NewProjectileBetter(portalSpawnPosition, (PortalArcSpawnCenter - portalSpawnPosition).SafeNormalize(Vector2.UnitY), ModContent.ProjectileType<DarkPortal>(), 0, 0f, -1, portalScale, portalLingerTime);

                        portalSpawnPosition = PortalArcSpawnCenter + new Vector2(720f + horizontalArcOffset * 240f, -560f + arcInterpolant * 1120f);
                        NewProjectileBetter(portalSpawnPosition, (PortalArcSpawnCenter - portalSpawnPosition).SafeNormalize(Vector2.UnitY), ModContent.ProjectileType<DarkPortal>(), 0, 0f, -1, portalScale, portalLingerTime);
                    }
                }

                // Keep all portals in stasis so that they fire at the same time.
                if (AttackTimer < DefaultTeleportDelay * 2f + handRaiseTime + portalCastTime - 10f)
                {
                    foreach (Projectile portal in AllProjectilesByID(ModContent.ProjectileType<DarkPortal>()))
                    {
                        if (portal.ModProjectile<DarkPortal>().Time >= portalLingerTime * 0.5f - 9f)
                            portal.ModProjectile<DarkPortal>().Time = (int)(portalLingerTime * 0.5f - 9f);
                    }
                }
            }

            // Teleport away and let all portals naturally fire.
            if (AttackTimer == DefaultTeleportDelay * 2f + handRaiseTime + portalCastTime)
            {
                SoundEngine.PlaySound(ExplosionTeleportSound);
                TeleportTo(Target.Center - Vector2.UnitX * Target.direction * 300f);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NewProjectileBetter(NPC.Center, Vector2.Zero, ModContent.ProjectileType<DarkWave>(), 0, 0f);
            }

            if (AttackTimer >= DefaultTeleportDelay * 2f + handRaiseTime + portalCastTime + attackTransitionDelay)
                SelectNextAttack();

            // Move hands.
            DefaultHandDrift(Hands[0], leftHandDestination, handSpeedFactor);
            DefaultHandDrift(Hands[1], rightHandDestination, handSpeedFactor);
        }

        public void DoBehavior_HoveringHandGasBursts()
        {
            int shootDelay = 60;
            int projectileShootRate = 28;
            int shootTime = 240;
            float handSpeedFactor = 1.33f;
            float hoverAcceleration = 0.75f;
            float hoverFlySpeed = 24f;
            Vector2 hoverOffset = new(-90f, -350f);
            Vector2 leftHandDestination = NPC.Center + Hands[0].DefaultOffset * NPC.scale;
            Vector2 rightHandDestination = NPC.Center + Hands[1].DefaultOffset * NPC.scale;

            // Disable contact damage. It is not relevant for this attack.
            NPC.damage = 0;

            NPC.Center = Vector2.Lerp(NPC.Center, Target.Center + hoverOffset, 0.027f);
            if (!NPC.WithinRange(Target.Center + hoverOffset, 100f))
                NPC.SimpleFlyMovement(NPC.SafeDirectionTo(Target.Center + hoverOffset) * hoverFlySpeed, hoverAcceleration);
            else
                NPC.velocity = (NPC.velocity * 1.04f).SafeNormalize(Vector2.UnitY) * Clamp(NPC.velocity.Length(), 7f, hoverFlySpeed);

            // Bring hands to the sides of the target.
            float handVerticalOffset = Sin(TwoPi * AttackTimer / 180f) * 400f + Target.velocity.Y * 44f;
            float handHoverInterpolant = GetLerpValue(0f, shootDelay, AttackTimer, true);
            leftHandDestination = Vector2.Lerp(leftHandDestination, Target.Center - new Vector2(780f, handVerticalOffset), handHoverInterpolant);
            rightHandDestination = Vector2.Lerp(rightHandDestination, Target.Center + new Vector2(780f, handVerticalOffset), handHoverInterpolant);
            MakeHandsOpen();

            // Make hands shoot gas bursts and comets.
            if (handHoverInterpolant >= 1f && AttackTimer % projectileShootRate == projectileShootRate - 1f && AttackTimer <= shootDelay + shootTime)
            {
                Vector2 leftCometShootVelocity = (Target.Center - Hands[0].Center).SafeNormalize(Vector2.UnitY) * 4f;
                Vector2 rightCometShootVelocity = (Target.Center - Hands[1].Center).SafeNormalize(Vector2.UnitY) * 4f;

                // Create gas particles.
                SoundEngine.PlaySound(SoundID.Item104, NPC.Center);
                for (int i = 0; i < 40; i++)
                    NoxusGasMetaball.CreateParticle(Hands[0].Center + leftCometShootVelocity.RotatedByRandom(0.98f) * Main.rand.NextFloat(4f), leftCometShootVelocity.RotatedByRandom(0.68f) * Main.rand.NextFloat(3f), Main.rand.NextFloat(13f, 56f));

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (!Target.WithinRange(Hands[0].Center, 330f))
                        NewProjectileBetter(Hands[0].Center, leftCometShootVelocity, ModContent.ProjectileType<DarkComet>(), CometDamage, 0f);
                    if (!Target.WithinRange(Hands[1].Center, 330f))
                        NewProjectileBetter(Hands[1].Center, rightCometShootVelocity, ModContent.ProjectileType<DarkComet>(), CometDamage, 0f);
                }
            }

            if (AttackTimer >= shootDelay + shootTime + 60)
                SelectNextAttack();

            // Move hands.
            DefaultHandDrift(Hands[0], leftHandDestination, handSpeedFactor);
            DefaultHandDrift(Hands[1], rightHandDestination, handSpeedFactor);
        }

        public void DoBehavior_RapidExplosiveTeleports()
        {
            int teleportCount = 7;
            int teleportRate = 23;
            int teleportCounter = (int)AttackTimer / teleportRate;
            int handRaiseTime = 67;
            int attackTransitionDelay = 180; // This is intentionally a bit long to give the player an opportunity to do melee hits.
            int fireballCount = 17;
            float handSpeedFactor = 2.1f;
            float maxArcAngle = ToRadians(165f);
            Vector2 leftHandDestination = NPC.Center + Hands[0].DefaultOffset * NPC.scale;
            Vector2 rightHandDestination = NPC.Center + Hands[1].DefaultOffset * NPC.scale;

            // Disable contact damage. It is not relevant for this attack.
            NPC.damage = 0;

            // Fade in quickly after teleports.
            if (teleportCounter < teleportCount)
                NPC.Opacity = GetLerpValue(0f, 5f, AttackTimer % teleportRate, true);
            else
                NPC.Opacity = Clamp(NPC.Opacity + 0.1f, 0f, 1f);

            // Teleport around.
            if (AttackTimer % teleportRate == teleportRate - 1f && teleportCounter < teleportCount)
            {
                Vector2 teleportDestination;
                do
                    teleportDestination = Target.Center + Main.rand.NextVector2Unit() * Main.rand.NextFloat(356f, 400f);
                while (NPC.WithinRange(teleportDestination, 200f));

                // Teleport in front of the target if this is the final teleport.
                if (teleportCounter == teleportCount - 1)
                    teleportDestination = Target.Center + Target.velocity * new Vector2(40f, 30f) + Main.rand.NextVector2CircularEdge(200f, 200f);

                // Release an explosion at the old position prior to the teleport.
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NewProjectileBetter(NPC.Center, Vector2.Zero, ModContent.ProjectileType<NoxusExplosion>(), ExplosionDamage, 0f);

                // Make things blurrier for a short time.
                if (NPC.WithinRange(Target.Center, 400f))
                    ScreenEffectSystem.SetBlurEffect(NPC.Center, 0.28f, 12);

                TeleportToWithDecal(teleportDestination);
            }

            // Raise hands after the final teleport.
            float handRaiseInterpolant = GetLerpValue(teleportCount * teleportRate, teleportCount * teleportRate + handRaiseTime, AttackTimer, true);

            // Once the hands are done raising, make them clap.
            if (handRaiseInterpolant >= 1f)
            {
                maxArcAngle = ToRadians(3f);
                handSpeedFactor += 3f;
            }

            if (handRaiseInterpolant > 0f)
            {
                leftHandDestination = CalculateHandOffsetForClapAnticipation(handRaiseInterpolant, maxArcAngle, true);
                rightHandDestination = CalculateHandOffsetForClapAnticipation(handRaiseInterpolant, maxArcAngle, false);
            }

            // Release the barrage of explosive fireballs once the clap is done.
            if (AttackTimer == teleportCount * teleportRate + handRaiseTime + 6f)
            {
                SoundEngine.PlaySound(ClapSound, Target.Center);
                ScreenEffectSystem.SetBlurEffect((Hands[0].Center + Hands[1].Center) * 0.5f, 0.7f, 27);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NewProjectileBetter((Hands[0].Center + Hands[1].Center) * 0.5f, Vector2.Zero, ModContent.ProjectileType<DarkWave>(), 0, 0f);

                    Vector2 fireballSpawnPosition = (Hands[0].Center + Hands[1].Center) * 0.5f;
                    for (int i = 0; i < fireballCount; i++)
                    {
                        Vector2 fireballVelocity = (Target.Center - fireballSpawnPosition).SafeNormalize(Vector2.UnitY).RotatedByRandom(1.34f) * Main.rand.NextFloat(9.5f, 24f);
                        NewProjectileBetter(fireballSpawnPosition, fireballVelocity, ModContent.ProjectileType<DarkFireball>(), FireballDamage, 0f, -1, 0f, Main.rand.NextFloat(18f, 36f));
                    }
                }
            }

            if (AttackTimer >= teleportCount * teleportRate + handRaiseTime + attackTransitionDelay)
                SelectNextAttack();

            // Move hands.
            DefaultHandDrift(Hands[0], leftHandDestination, handSpeedFactor);
            DefaultHandDrift(Hands[1], rightHandDestination, handSpeedFactor);
        }

        public void DoBehavior_Phase2Transition()
        {
            int anticipationSoundTime = 289;
            int screamTime = 180;
            float handSpeedFactor = 1.3f;
            Vector2 headCenter = NPC.Center + HeadOffset;
            Vector2 headTangentDirection = (NPC.rotation + HeadRotation).ToRotationVector2();
            Vector2 leftHandDestination = NPC.Center + Hands[0].DefaultOffset * NPC.scale;
            Vector2 rightHandDestination = NPC.Center + Hands[1].DefaultOffset * NPC.scale;

            // Disable contact damage. It is not relevant for this behavior.
            NPC.damage = 0;

            // Teleport above the player on the first frame.
            if (AttackTimer == 1f)
            {
                TeleportTo(Target.Center - Vector2.UnitY * 350f);
                SoundEngine.PlaySound(BrainRotSound);
            }

            // Have the head rotate to the side.
            HeadRotation = Pi / 13f;

            // Periodically create chromatic aberration effects in accordance with the heartbeat of the sound.
            if (AttackTimer < anticipationSoundTime && AttackTimer % 30f == 29f)
            {
                float attackCompletion = AttackTimer / anticipationSoundTime;
                ScreenEffectSystem.SetChromaticAberrationEffect(NPC.Center, attackCompletion * 2f + 0.3f, 12);
                Main.LocalPlayer.Calamity().GeneralScreenShakePower = Lerp(3f, 9f, attackCompletion) * GetLerpValue(NPC.Distance(Main.LocalPlayer.Center), 3200f, 2400f, true);
            }

            // Make the eye gleam before teleporting.
            EyeGleamInterpolant = GetLerpValue(anticipationSoundTime - 90f, anticipationSoundTime - 5f, AttackTimer, true);
            if (AttackTimer >= anticipationSoundTime)
                EyeGleamInterpolant = 0f;

            // Move up and down and jitter a bit.
            float jitterIntensity = GetLerpValue(0f, anticipationSoundTime, AttackTimer, true);
            NPC.velocity = Vector2.UnitY * Sin(TwoPi * AttackTimer / 56f) * 2f;
            if (jitterIntensity < 1f)
                NPC.Center += Main.rand.NextVector2Circular(15f, 15f) * Pow(jitterIntensity, 1.56f);

            // Scream after the anticipation is over.
            if (AttackTimer >= anticipationSoundTime)
            {
                if (AttackTimer == anticipationSoundTime)
                {
                    TeleportTo(Target.Center - Vector2.UnitY * 350f);

                    SoundEngine.PlaySound(ExplosionTeleportSound);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        NewProjectileBetter(NPC.Center + HeadOffset, Vector2.Zero, ModContent.ProjectileType<DarkWave>(), 0, 0f);
                }

                HeadRotation = 0f;
                NPC.velocity = Vector2.Zero;

                if (AttackTimer % 11f == 1f && AttackTimer <= anticipationSoundTime + screamTime - 45f)
                {
                    SoundEngine.PlaySound(ScreamSound with { Volume = 1.4f });
                    Color burstColor = Main.rand.NextBool() ? Color.SlateBlue : Color.Lerp(Color.White, Color.MediumPurple, 0.7f);

                    // Create blur and burst particle effects.
                    ExpandingChromaticBurstParticle burst = new(NPC.Center + HeadOffset, Vector2.Zero, burstColor, 16, 0.1f);
                    GeneralParticleHandler.SpawnParticle(burst);
                    ScreenEffectSystem.SetBlurEffect(NPC.Center, 1f, 30);
                    Target.Calamity().GeneralScreenShakePower = 12f;
                }

                NPC.Center += Main.rand.NextVector2Circular(12.5f, 12.5f);

                leftHandDestination.Y += TeleportVisualsAdjustedScale.Y * 75f;
                rightHandDestination.Y += TeleportVisualsAdjustedScale.Y * 75f;
            }
            else
            {
                leftHandDestination = headCenter + headTangentDirection * NPC.scale * -80f + Main.rand.NextVector2Circular(7f, 7f);
                rightHandDestination = headCenter + headTangentDirection * NPC.scale * 80f + Main.rand.NextVector2Circular(7f, 7f);
            }

            if (AttackTimer >= anticipationSoundTime + screamTime)
                SelectNextAttack();

            // Move hands.
            DefaultHandDrift(Hands[0], leftHandDestination, handSpeedFactor);
            DefaultHandDrift(Hands[1], rightHandDestination, handSpeedFactor);
        }

        public void DoBehavior_GeometricSpikesTeleportAndFireballs()
        {
            int hoverTime = 45;
            int handRaiseTime = 30;
            int fireballShootRate = 10;
            int spikeShootCount = 17;
            int attackTransitionDelay = 105;
            float fireballShootSpeed = 17f;
            float maxHandRaiseOffset = 332f;
            float handSpeedFactor = 4f;

            if (Main.expertMode)
            {
                hoverTime -= 8;
                fireballShootSpeed += 3f;
            }
            if (CalamityWorld.revenge)
                fireballShootSpeed += 3.5f;

            Vector2 leftHandDestination = NPC.Center + Hands[0].DefaultOffset * NPC.scale;
            Vector2 rightHandDestination = NPC.Center + Hands[1].DefaultOffset * NPC.scale;
            Vector2 flyHoverOffset = new(Sin(TwoPi * AttackTimer / 90f) * 300f, Sin(TwoPi * AttackTimer / 70f + PiOver4) * 50f);

            // Disable contact damage. It is not relevant for this attack.
            NPC.damage = 0;

            // Simply hover above the target at first.
            if (AttackTimer <= hoverTime)
                BasicFlyMovement(Target.Center - Vector2.UnitY * 420f + flyHoverOffset);

            // Slow down and raise hands. As the hands are raised they shoot fireballs at the target.
            else if (AttackTimer <= hoverTime + handRaiseTime)
            {
                // Rapidly decelerate.
                NPC.velocity *= 0.93f;

                float handRaiseInterpolant = GetLerpValue(hoverTime, hoverTime + handRaiseTime, AttackTimer, true);
                float handRaiseVerticalOffset = Pow(handRaiseInterpolant, 2.3f) * maxHandRaiseOffset;
                leftHandDestination.X = Lerp(leftHandDestination.X, NPC.Center.X, handRaiseInterpolant * 0.7f);
                rightHandDestination.X = Lerp(rightHandDestination.X, NPC.Center.X, handRaiseInterpolant * 0.7f);
                leftHandDestination.Y -= handRaiseVerticalOffset;
                rightHandDestination.Y -= handRaiseVerticalOffset;

                // Open hands.
                MakeHandsOpen();

                // Shoot fireballs at the target.
                if (AttackTimer % fireballShootRate == fireballShootRate - 1f)
                {
                    SoundEngine.PlaySound(FireballShootSound, Hands[0].Center);
                    SoundEngine.PlaySound(FireballShootSound, Hands[1].Center);

                    // Make the fireballs slower at first.
                    fireballShootSpeed *= Remap(AttackTimer - hoverTime, 0f, 24f, 0.4f, 1f);

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 fireballShootVelocity = (Target.Center - Hands[0].Center).SafeNormalize(Vector2.UnitY) * fireballShootSpeed;
                        NewProjectileBetter(Hands[0].Center, fireballShootVelocity, ModContent.ProjectileType<DarkFireball>(), FireballDamage, 0f);

                        fireballShootVelocity = (Target.Center - Hands[1].Center).SafeNormalize(Vector2.UnitY) * fireballShootSpeed;
                        NewProjectileBetter(Hands[1].Center, fireballShootVelocity, ModContent.ProjectileType<DarkFireball>(), FireballDamage, 0f);
                    }
                }
            }

            // Teleport and leave behind a trail of spikes.
            if (AttackTimer >= hoverTime + handRaiseTime && AttackTimer <= hoverTime + handRaiseTime + DefaultTeleportDelay)
            {
                leftHandDestination.Y -= maxHandRaiseOffset;
                rightHandDestination.Y -= maxHandRaiseOffset;
                leftHandDestination.X = Lerp(leftHandDestination.X, NPC.Center.X, 0.7f);
                rightHandDestination.X = Lerp(rightHandDestination.X, NPC.Center.X, 0.7f);

                // Teleport far away from the target and release the spikes when ready.
                if (AttackTimer == hoverTime + handRaiseTime + DefaultTeleportDelay)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < spikeShootCount; i++)
                        {
                            float horizonalSpikeOffsetDirection = Lerp(-1f, 1f, i / (float)(spikeShootCount - 1f));
                            Vector2 spikeSpawnPosition = Target.Center + new Vector2(horizonalSpikeOffsetDirection * 1500f, -900f);
                            spikeSpawnPosition.Y = NPC.Center.Y;

                            Vector2 spikeShootDirection = Vector2.Lerp(Vector2.UnitY, (Target.Center - spikeSpawnPosition).SafeNormalize(Vector2.UnitY), 0.2f).SafeNormalize(Vector2.UnitY);
                            NewProjectileBetter(spikeSpawnPosition, spikeShootDirection * 0.0001f, ModContent.ProjectileType<NoxSpike>(), SpikeDamage, 0f);
                            NewProjectileBetter(spikeSpawnPosition, spikeShootDirection * new Vector2(1f, -1f) * 0.0001f, ModContent.ProjectileType<NoxSpike>(), SpikeDamage, 0f);
                        }
                        NewProjectileBetter(NPC.Center, Vector2.Zero, ModContent.ProjectileType<NoxusExplosion>(), ExplosionDamage, 0f);
                        NewProjectileBetter(NPC.Center, Vector2.Zero, ModContent.ProjectileType<DarkWave>(), 0, 0f);
                    }

                    TeleportTo(Target.Center + Vector2.UnitY * 2500f);
                    SoundEngine.PlaySound(ExplosionTeleportSound with
                    {
                        Volume = 0.7f
                    }, Target.Center);
                }
            }

            if (AttackTimer >= hoverTime + handRaiseTime + DefaultTeleportDelay + attackTransitionDelay)
                SelectNextAttack();

            // Handle teleport visual effects.
            TeleportVisualsInterpolant = GetLerpValue(hoverTime + handRaiseTime, hoverTime + handRaiseTime + DefaultTeleportDelay * 2f, AttackTimer, true);
            NPC.Opacity = GetLerpValue(0.4f, 0.1f, TeleportVisualsInterpolant, true) + GetLerpValue(0.6f, 0.85f, TeleportVisualsInterpolant, true);

            // Move hands.
            DefaultHandDrift(Hands[0], leftHandDestination, handSpeedFactor);
            DefaultHandDrift(Hands[1], rightHandDestination, handSpeedFactor);
        }

        public void DoBehavior_TeleportAndShootNoxusGas()
        {
            int wrappedAttackTimer = (int)AttackTimer % (DefaultTeleportDelay * 2);
            int teleportCounter = (int)AttackTimer / (DefaultTeleportDelay * 2);
            int gasShootCount = 3;
            float handSpeedFactor = 2.52f;
            float gasShootMaxAngle = ToRadians(37f);
            float gasShootSpeed = 10.4f;
            Vector2 leftHandDestination = NPC.Center + Hands[0].DefaultOffset * NPC.scale;
            Vector2 rightHandDestination = NPC.Center + Hands[1].DefaultOffset * NPC.scale;
            Vector2 closestHandPosition = Hands[0].Center;
            ref Vector2 closestHandToTargetDestination = ref leftHandDestination;
            if (Target.Center.X >= NPC.Center.X)
            {
                closestHandToTargetDestination = ref rightHandDestination;
                closestHandPosition = Hands[1].Center;
            }

            // Disable contact damage. It is not relevant for this attack.
            NPC.damage = 0;

            // Handle teleport visual effects.
            TeleportVisualsInterpolant = GetLerpValue(0f, DefaultTeleportDelay * 2f, wrappedAttackTimer, true);
            NPC.Opacity = GetLerpValue(0.4f, 0.1f, TeleportVisualsInterpolant, true) + GetLerpValue(0.6f, 0.85f, TeleportVisualsInterpolant, true);

            // Perform the teleport.
            if (wrappedAttackTimer == DefaultTeleportDelay)
            {
                float teleportOffsetAngle = TwoPi * (teleportCounter % 4f) / 4f + PiOver4;
                Vector2 teleportOffset = teleportOffsetAngle.ToRotationVector2() * new Vector2(1050f, 360f);
                Vector2 teleportDestination = Target.Center + teleportOffset;

                // Move a bit further away if the target is moving in the direction of the teleport offset.
                if (Vector2.Dot(Target.velocity, teleportOffset) > 0f)
                    teleportDestination += Target.velocity * 28f;

                if (teleportCounter >= 4)
                {
                    teleportDestination = Target.Center - Vector2.UnitY * 340f;
                    SelectNextAttack();
                }

                TeleportTo(teleportDestination);
            }

            // Make the hand that's closest to the target face towards them before firing the gas.
            float aimHandAtTargetInterpolant = GetLerpValue(DefaultTeleportDelay, DefaultTeleportDelay + 10f, AttackTimer, true);
            closestHandToTargetDestination = Vector2.Lerp(closestHandToTargetDestination, NPC.Center + NPC.SafeDirectionTo(Target.Center) * 157f, aimHandAtTargetInterpolant);

            // Release Noxus gas in a spread.
            if (wrappedAttackTimer == DefaultTeleportDelay * 2f - 12f)
            {
                SoundEngine.PlaySound(FireballShootSound, Target.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < gasShootCount; i++)
                    {
                        float gasShootArc = Lerp(-gasShootMaxAngle, gasShootMaxAngle, i / (float)(gasShootCount - 1f)) + Main.rand.NextFloatDirection() * 0.048f;
                        Vector2 gasShootVelocity = (Target.Center - closestHandPosition).SafeNormalize(Vector2.UnitY).RotatedBy(gasShootArc) * gasShootSpeed;
                        NewProjectileBetter(closestHandPosition, gasShootVelocity, ModContent.ProjectileType<NoxusGas>(), NoxusGasDamage, 0f);
                    }
                }
            }

            // Move hands.
            DefaultHandDrift(Hands[0], leftHandDestination, handSpeedFactor);
            DefaultHandDrift(Hands[1], rightHandDestination, handSpeedFactor);
        }

        public void DoBehavior_ThreeDimensionalNightmareDeathRay()
        {
            int moveIntoBackgroundTime = 90;
            int spinTime = 169;
            int aimUpwardsTime = 60;
            int aimDownwardsTime = 15;
            int laserShootCount = 1;
            int secondarySlamTime = 0;
            float handSpeedFactor = 2f;

            if (CurrentPhase >= 2)
            {
                moveIntoBackgroundTime -= 20;
                secondarySlamTime = 109;
            }

            int deathrayShootTime = spinTime + aimUpwardsTime + aimDownwardsTime + secondarySlamTime + 24;
            float wrappedAttackTimer = AttackTimer % (moveIntoBackgroundTime + deathrayShootTime);
            Vector2 headCenter = NPC.Center + HeadOffset;
            Vector2 headTangentDirection = (NPC.rotation + HeadRotation).ToRotationVector2();
            Vector2 leftHandDestination = NPC.Center + Hands[0].DefaultOffset * NPC.scale;
            Vector2 rightHandDestination = NPC.Center + Hands[1].DefaultOffset * NPC.scale;

            // Disable contact damage. It is not relevant for this attack.
            NPC.damage = 0;

            // Teleport above the target and decide the laser spin direction on the first frame.
            if (wrappedAttackTimer == 1f)
            {
                TeleportTo(Target.Center - Vector2.UnitY * 300f);
                LaserSpinDirection = Main.rand.NextFromList(-1f, 1f);
            }

            if (AttackTimer >= (moveIntoBackgroundTime + deathrayShootTime) * laserShootCount)
            {
                if (AttackTimer >= (moveIntoBackgroundTime + deathrayShootTime) * laserShootCount + 3f)
                    SelectNextAttack();
                return;
            }

            // Move into the background and hover above the player.
            if (wrappedAttackTimer <= moveIntoBackgroundTime)
            {
                ZPosition = Pow(wrappedAttackTimer / moveIntoBackgroundTime, 0.45f) * 1.83f;
                NPC.Center = Vector2.Lerp(NPC.Center, Target.Center + new Vector2(Target.velocity.X * 12f, ZPosition * -200f), 0.14f);
                NPC.velocity.X *= 0.9f;

                // Have Noxus spread his hands outward in a T-pose right before the lasers fire.
                float tPoseInterpolant = GetLerpValue(moveIntoBackgroundTime - 30f, moveIntoBackgroundTime - 12f, wrappedAttackTimer, true);
                leftHandDestination = Vector2.Lerp(leftHandDestination, NPC.Center - Vector2.UnitX * NPC.scale * 400f, tPoseInterpolant);
                rightHandDestination = Vector2.Lerp(rightHandDestination, NPC.Center + Vector2.UnitX * NPC.scale * 400f, tPoseInterpolant);
            }

            // Have Noxus hold his hands up to his head during the spin attack, as though they're casting energy into the orb.
            // This has a small amount of jitter for detail.
            else
            {
                leftHandDestination = headCenter + headTangentDirection * NPC.scale * -80f + Main.rand.NextVector2Circular(7f, 7f);
                rightHandDestination = headCenter + headTangentDirection * NPC.scale * 80f + Main.rand.NextVector2Circular(7f, 7f);
                MakeHandsOpen();
            }

            // Make the laser telegraph opacity go up before the deathray fires.
            LaserTelegraphOpacity = 0f;
            if (wrappedAttackTimer >= moveIntoBackgroundTime - 54f && wrappedAttackTimer < moveIntoBackgroundTime)
                LaserTelegraphOpacity = GetLerpValue(moveIntoBackgroundTime - 54f, moveIntoBackgroundTime - 12f, wrappedAttackTimer, true) * GetLerpValue(moveIntoBackgroundTime - 1f, moveIntoBackgroundTime - 6f, wrappedAttackTimer, true);

            // Fire the deathray and create some chromatic aberration effects.
            if (wrappedAttackTimer == moveIntoBackgroundTime)
            {
                SoundEngine.PlaySound(ClapSound);
                SoundEngine.PlaySound(ExplosionSound);
                SoundEngine.PlaySound(NightmareDeathrayShootSound);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NewProjectileBetter(NPC.Center, Vector2.UnitX, ModContent.ProjectileType<NightmareDeathRay>(), NightmareDeathrayDamage, 0f, -1, 0f, deathrayShootTime);
                    NewProjectileBetter(NPC.Center, Vector2.Zero, ModContent.ProjectileType<DarkWave>(), 0, 0f);
                }

                ScreenEffectSystem.SetFlashEffect(NPC.Center, 4f, 45);
                ScreenEffectSystem.SetChromaticAberrationEffect(NPC.Center, 3f, 45);
            }

            // Try to stay near the target on the Y axis when the laser is spinning.
            float verticalFollowPlayerInterpolant = GetLerpValue(moveIntoBackgroundTime, moveIntoBackgroundTime + 30f, wrappedAttackTimer, true) * GetLerpValue(moveIntoBackgroundTime + spinTime, moveIntoBackgroundTime + spinTime - 30f, wrappedAttackTimer, true);
            NPC.Center = Vector2.Lerp(NPC.Center, new Vector2(NPC.Center.X, Target.Center.Y), verticalFollowPlayerInterpolant * 0.02f);

            // Orient the laser direction in 3D space.
            // It begins by spinning around, before orienting itself upward and slicing downward, releasing countless spikes.
            float generalSpin = TwoPi * (wrappedAttackTimer - moveIntoBackgroundTime) * GetLerpValue(moveIntoBackgroundTime, moveIntoBackgroundTime + 30f, wrappedAttackTimer, true) / 105f;
            if (LaserSpinDirection == -1f)
                generalSpin = -generalSpin - Pi;

            float aimUpwardsInterpolant = Pow(GetLerpValue(spinTime, spinTime + aimUpwardsTime - 20f, wrappedAttackTimer, true), 0.589f);
            float aimDownwardsInterpolant = Pow(GetLerpValue(spinTime + aimUpwardsTime, spinTime + aimUpwardsTime + aimDownwardsTime, wrappedAttackTimer, true), 0.71f);

            LaserRotation = new(0f, 0f, WrapAngle(generalSpin));
            LaserRotation = new(0f, 0f, LaserRotation.Z.AngleLerp(PiOver2, aimUpwardsInterpolant));

            // Make the laser shift based on the upwards/downwards interpolants.
            LaserSquishFactor = Lerp(1f, 0.4f, aimUpwardsInterpolant) + aimDownwardsInterpolant * 0.5f;
            LaserLengthFactor = Lerp(1f, -1f, aimDownwardsInterpolant);

            // Make the laser slam down again if necessary.
            if (secondarySlamTime > 0)
            {
                float secondarySlamInterpolant = GetLerpValue(0f, secondarySlamTime, wrappedAttackTimer - spinTime - aimUpwardsTime - aimDownwardsTime, true);
                if (secondarySlamInterpolant > 0f)
                {
                    LaserLengthFactor = Lerp(-1f, 1f, GetLerpValue(0.33f, 0.5f, secondarySlamInterpolant, true) * GetLerpValue(0.9f, 0.78f, secondarySlamInterpolant, true));
                    LaserSquishFactor = Lerp(LaserSquishFactor, 0.1f, GetLerpValue(-1f, -0.6f, LaserSquishFactor, true) * GetLerpValue(1f, 0.6f, LaserSquishFactor, true));
                }

                // Make the screen shatter and release a bunch of perpendicular spikes when the laser is done slamming.
                if ((int)(secondarySlamInterpolant * secondarySlamTime) == (int)(secondarySlamTime * 0.9f))
                {
                    // Make the screen shatter.
                    SoundEngine.PlaySound(ExplosionTeleportSound);
                    ScreenShatterSystem.CreateShatterEffect(new Vector2(NPC.Center.X - Main.screenPosition.X, Main.screenHeight));

                    // Release the spikes.
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        float spikeVerticalOffset = Main.rand.NextFloat(32f, 84f);
                        for (float dy = spikeVerticalOffset; dy < 2000f; dy += 118f)
                        {
                            NewProjectileBetter(NPC.Center + Vector2.UnitY * dy, Vector2.UnitX * 0.0001f, ModContent.ProjectileType<NoxSpike>(), SpikeDamage, 0f);
                            NewProjectileBetter(NPC.Center + Vector2.UnitY * dy, -Vector2.UnitX * 0.0001f, ModContent.ProjectileType<NoxSpike>(), SpikeDamage, 0f);
                        }
                    }
                }
            }

            // Make Noxus raise his right hand when the laser is aiming upward and vice versa.
            rightHandDestination.Y -= aimUpwardsInterpolant * NPC.scale * LaserLengthFactor * 280f;

            // Make the right hand open.
            if (aimUpwardsInterpolant > 0f)
                Hands[1].ShouldOpen = true;

            // Rise upward as the laser is moved upward.
            if (aimUpwardsInterpolant > 0f && LaserLengthFactor != -1f && NPC.Center.Y < Target.Center.Y)
                NPC.Center = Vector2.Lerp(NPC.Center, new Vector2(NPC.Center.X, Target.Center.Y - aimUpwardsInterpolant * 370f), aimUpwardsInterpolant * 0.2f);

            // Make the screen shatter.
            if (wrappedAttackTimer == spinTime + aimUpwardsTime + 10f)
            {
                // Release perpendicular spikes.
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float spikeVerticalOffset = Main.rand.NextFloat(32f, 84f);
                    for (float dy = spikeVerticalOffset; dy < 2000f; dy += 118f)
                    {
                        NewProjectileBetter(NPC.Center + Vector2.UnitY * dy, Vector2.UnitX * 0.0001f, ModContent.ProjectileType<NoxSpike>(), SpikeDamage, 0f);
                        NewProjectileBetter(NPC.Center + Vector2.UnitY * dy, -Vector2.UnitX * 0.0001f, ModContent.ProjectileType<NoxSpike>(), SpikeDamage, 0f);
                    }
                }

                SoundEngine.PlaySound(ExplosionTeleportSound);
                ScreenShatterSystem.CreateShatterEffect(new Vector2(NPC.Center.X - Main.screenPosition.X, Main.screenHeight));
            }

            // Move hands.
            DefaultHandDrift(Hands[0], leftHandDestination, handSpeedFactor);
            DefaultHandDrift(Hands[1], rightHandDestination, handSpeedFactor);
        }

        public void DoBehavior_Phase3Transition()
        {
            int stunTime = 240;
            int screamTime = 210;
            float handSpeedFactor = 2f;
            Vector2 leftHandDestination = NPC.Center + Hands[0].DefaultOffset * NPC.scale;
            Vector2 rightHandDestination = NPC.Center + Hands[1].DefaultOffset * NPC.scale;

            // Disable contact damage. It is not relevant for this behavior.
            NPC.damage = 0;

            // Do stunned effects and make the music go away.
            if (AttackTimer <= stunTime)
            {
                // Do a loud explosion effect on the first frame to mask the fact that the music is gone. Having it just abrupt go away without a seam would be weird.
                if (AttackTimer == 1f)
                {
                    TeleportTo(Target.Center - Vector2.UnitY * 205f);

                    SoundEngine.PlaySound(ExplosionTeleportSound with { Volume = 1.5f });
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        NewProjectileBetter(NPC.Center, Vector2.Zero, ModContent.ProjectileType<DarkWave>(), 0, 0f);
                }

                // Temporarily disable the music.
                Music = 0;

                // Fade away over time.
                NPC.Opacity = GetLerpValue(stunTime - 6f, stunTime - 105f, AttackTimer, true);

                // Have the head rotate to the side.
                HeadRotation = Pi / 13f;

                // Make the camera zoom in on Noxus.
                float cameraPanInterpolant = GetLerpValue(5f, 11f, AttackTimer, true);
                float cameraZoom = GetLerpValue(11f, 60f, AttackTimer, true) * 0.2f;
                CameraPanSystem.CameraFocusPoint = NPC.Center;
                CameraPanSystem.CameraPanInterpolant = cameraPanInterpolant;
                CameraPanSystem.Zoom = cameraZoom;

                // Make the boss bar close.
                NPC.Calamity().ShouldCloseHPBar = true;
                NPC.dontTakeDamage = true;
            }

            else
            {
                // Bring the music back.
                if (Music == 0)
                    Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/GodIsDead");

                // Teleport above the player again.
                if (AttackTimer == stunTime + 1f)
                {
                    NPC.Opacity = 1f;
                    TeleportTo(Target.Center - Vector2.UnitY * 350f);

                    SoundEngine.PlaySound(ExplosionTeleportSound);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        NewProjectileBetter(NPC.Center + HeadOffset, Vector2.Zero, ModContent.ProjectileType<DarkWave>(), 0, 0f);
                }

                // Reset the head being rotated.
                HeadRotation = 0f;
                NPC.velocity = Vector2.Zero;

                // Make the big eye appear.
                BigEyeOpacity = Clamp(BigEyeOpacity + 0.075f, 0f, 1f);

                // Create scream shockwaves.
                if (AttackTimer % 9f == 1f && AttackTimer <= stunTime + screamTime - 45f)
                {
                    SoundEngine.PlaySound(ScreamSound with { Volume = 1.3f });
                    Color burstColor = Main.rand.NextBool() ? Color.SlateBlue : Color.Lerp(Color.White, Color.MediumPurple, 0.7f);

                    // Create blur and burst particle effects.
                    ExpandingChromaticBurstParticle burst = new(NPC.Center + HeadOffset, Vector2.Zero, burstColor, 16, 0.1f);
                    GeneralParticleHandler.SpawnParticle(burst);
                    ScreenEffectSystem.SetBlurEffect(NPC.Center, 1f, 30);
                    Target.Calamity().GeneralScreenShakePower = 15f;
                }

                // Jitter in place violently.
                NPC.Center += Main.rand.NextVector2Circular(12.5f, 12.5f);

                // Create powerful, lingering screen shake effects.
                Target.Calamity().GeneralScreenShakePower = 13f;

                // Create explosions everywhere near the player.
                if (Main.netMode != NetmodeID.MultiplayerClient && AttackTimer % 5f == 4f)
                {
                    Vector2 explosionSpawnPosition = Target.Center + Main.rand.NextVector2Unit() * Main.rand.NextFloat(250f, 720f);
                    NewProjectileBetter(explosionSpawnPosition, Vector2.Zero, ModContent.ProjectileType<NoxusExplosion>(), 0, 0f);
                }

                leftHandDestination.Y += TeleportVisualsAdjustedScale.Y * 75f;
                rightHandDestination.Y += TeleportVisualsAdjustedScale.Y * 75f;
            }

            // Move hands.
            DefaultHandDrift(Hands[0], leftHandDestination, handSpeedFactor);
            DefaultHandDrift(Hands[1], rightHandDestination, handSpeedFactor);

            if (AttackTimer >= stunTime + screamTime)
                SelectNextAttack();
        }

        public void DoBehavior_BrainFogAndThreeDimensionalCharges()
        {
            int fogCoverTime = 145;
            int handPressTime = 36;
            int disappearIntoFogTime = 70;
            int chargeCount = 1;

            if (BrainFogChargeCounter >= 1)
            {
                fogCoverTime -= 55;
                disappearIntoFogTime -= 25;
            }

            int delayBeforeInvisible = DefaultTeleportDelay * 2 + fogCoverTime + disappearIntoFogTime;
            int delayBeforeTwinklesAppear = 60;
            int twinkleCount = 5;
            int delayPerTwinkle = 16;
            int chargeDelay = 48;
            int chargeTime = 20;

            if (Main.expertMode)
            {
                twinkleCount--;
                delayPerTwinkle -= 2;
            }
            if (CalamityWorld.revenge)
            {
                twinkleCount--;
                delayPerTwinkle--;
            }

            int twinkleHoverTime = delayBeforeTwinklesAppear + delayPerTwinkle * twinkleCount + chargeDelay;
            float handSpeedFactor = 1.8f;
            float fogSpreadInterpolant = GetLerpValue(DefaultTeleportDelay * 2f, DefaultTeleportDelay * 2f + fogCoverTime, AttackTimer, true);
            float handPressInterpolant = GetLerpValue(DefaultTeleportDelay * 2f, DefaultTeleportDelay * 2f + handPressTime, AttackTimer, true);
            Vector2 leftHandDestination = NPC.Center + Hands[0].DefaultOffset * NPC.scale;
            Vector2 rightHandDestination = NPC.Center + Hands[1].DefaultOffset * NPC.scale;

            // Disable contact damage by default.
            // This will get enabled again later in this method, during the charges.
            NPC.damage = 0;

            // Create some fog effects on the head as the fog gets stronger.
            if (fogSpreadInterpolant > 0f && ZPosition == 0f)
            {
                float fogOpacity = (fogSpreadInterpolant * 0.3f + 0.2f) * GetLerpValue(0.9f, 0.67f, fogSpreadInterpolant, true);
                for (int i = 0; i < 3; i++)
                {
                    Vector2 fogVelocity = Main.rand.NextVector2Circular(36f, 36f) * fogSpreadInterpolant;
                    HeavySmokeParticle fog = new(NPC.Center + HeadOffset, fogVelocity, NoxusSky.FogColor, 50, 3f, fogOpacity, 0f, true);
                    GeneralParticleHandler.SpawnParticle(fog);
                }
            }

            // Slow down and begin to teleport above the player. This also involves waiting after the teleport before attacking.
            if (AttackTimer <= DefaultTeleportDelay * 2f)
            {
                NPC.velocity *= 0.8f;

                // Raise hands as the teleport happens.
                float teleportWaitInterpolant = GetLerpValue(0f, DefaultTeleportDelay - 9f, AttackTimer, true);
                if (AttackTimer >= DefaultTeleportDelay + 1f)
                    teleportWaitInterpolant = 0f;
                else
                    handSpeedFactor += 0.75f;

                float verticalOffset = Pow(teleportWaitInterpolant, 1.7f) * 400f;
                leftHandDestination.Y -= verticalOffset;
                rightHandDestination.Y -= verticalOffset;

                // Teleport near the target when ready.
                if (AttackTimer == DefaultTeleportDelay)
                {
                    Vector2 hoverDestination = Target.Center - Vector2.UnitY * 400f;
                    TeleportTo(hoverDestination);
                }
            }

            // The fog is coming the fog is coming the fog is coming the fog is coming the fog is coming the fog is coming the fog is coming
            else if (AttackTimer <= DefaultTeleportDelay * 2f + fogCoverTime)
            {
                FogSpreadDistance = Pow(fogSpreadInterpolant, 3.8f) * 2f;
                FogIntensity = Pow(fogSpreadInterpolant, 0.85f);
            }

            // Fly away into the fog.
            else if (AttackTimer <= delayBeforeInvisible)
            {
                float fadeIntoFogInterpolant = GetLerpValue(DefaultTeleportDelay * 2f + fogCoverTime, delayBeforeInvisible, AttackTimer, true);
                ZPosition = fadeIntoFogInterpolant * 2.2f;
                NPC.Opacity = GetLerpValue(1f, 0.5f, fadeIntoFogInterpolant, true);

                // Move above the target.
                NPC.Center = Vector2.Lerp(NPC.Center, Target.Center - Vector2.UnitY * fadeIntoFogInterpolant * 360f, fadeIntoFogInterpolant * 0.08f);

                if (fadeIntoFogInterpolant >= 1f)
                    CreateTwinkle(NPC.Center, Vector2.One * 2f);
            }

            // Silently hover near the player while invisible. Twinkles are periodically released as indicates when this happens.
            else if (AttackTimer <= delayBeforeInvisible + twinkleHoverTime)
            {
                float predictivenessFadeOffInterpolant = GetLerpValue(delayBeforeInvisible + twinkleHoverTime - 6f, delayBeforeInvisible + twinkleHoverTime - 20f, AttackTimer, true);
                Vector2 hoverDestination = Target.Center + Target.velocity * GetLerpValue(Target.velocity.Length(), 5f, 10f, true) * predictivenessFadeOffInterpolant * 50f;
                if (NPC.WithinRange(hoverDestination, 150f))
                {
                    NPC.velocity *= 0.89f;
                    NPC.Center = Vector2.Lerp(NPC.Center, hoverDestination, 0.11f);
                }
                else
                    NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.SafeDirectionTo(Target.Center) * 18f, 0.11f);

                if (AttackTimer >= delayBeforeInvisible + delayBeforeTwinklesAppear && AttackTimer % delayPerTwinkle == delayPerTwinkle - 1f)
                    CreateTwinkle(NPC.Center + Main.rand.NextVector2Circular(30f, 30f), Vector2.One * 1.35f);

                // Make the screen shake as the wait continues.
                Target.Calamity().GeneralScreenShakePower = GetLerpValue(delayBeforeInvisible, delayBeforeInvisible + twinkleHoverTime, AttackTimer, true) * 8f + 1f;
            }

            // Charge very, very, VERY quickly at the target.
            else if (AttackTimer <= delayBeforeInvisible + twinkleHoverTime + chargeTime)
            {
                if (AttackTimer == delayBeforeInvisible + twinkleHoverTime + 1f)
                {
                    if (NPC.WithinRange(Target.Center, 105f))
                        NPC.Center = Target.Center + Main.rand.NextVector2CircularEdge(0.04f, 0.04f);

                    NPC.netUpdate = true;

                    SoundEngine.PlaySound(JumpscareSound);
                    ScreenEffectSystem.SetBlurEffect(NPC.Center, 1f, 20);
                    ScreenEffectSystem.SetChromaticAberrationEffect(NPC.Center, 1.3f, 60);
                    Target.Calamity().GeneralScreenShakePower = 15f;
                }

                // Get close and make the fog dissipate.
                FogIntensity = GetLerpValue(delayBeforeInvisible + twinkleHoverTime + chargeTime - 4f, delayBeforeInvisible + twinkleHoverTime, AttackTimer, true);
                ZPosition = Clamp(ZPosition - 0.17f, -0.98f, 4f);
                NPC.Opacity = ZPosition <= -0.98f ? 0f : 1f;

                if (Distance(NPC.Center.X, Target.Center.X) >= 100f)
                    NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.UnitX * NPC.SafeDirectionTo(Target.Center) * 16f, 0.067f);

                // Do damage again if zoomed in enough.
                if (ZPosition <= 0.3f && ZPosition >= -0.8f)
                    NPC.damage = NPC.defDamage;

                handPressInterpolant = 0f;
                handSpeedFactor += 6f;
            }

            // Shatter the screen after the charge has completed.
            // Also release comets at the player if they're relatively far away.
            if (AttackTimer == delayBeforeInvisible + twinkleHoverTime + chargeTime)
            {
                ScreenShatterSystem.CreateShatterEffect(NPC.Center - Main.screenPosition);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NewProjectileBetter(NPC.Center, Vector2.Zero, ModContent.ProjectileType<DarkWave>(), 0, 0f);

                    if (!NPC.WithinRange(Target.Center, 350f))
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            Vector2 cometVelocity = (Target.Center - NPC.Center) * 0.0098f + Main.rand.NextVector2Circular(4f, 4f);
                            NewProjectileBetter(NPC.Center, cometVelocity, ModContent.ProjectileType<DarkComet>(), CometDamage, 0f);
                        }
                    }
                }
            }

            if (AttackTimer >= delayBeforeInvisible + twinkleHoverTime + chargeTime + 20f)
            {
                NPC.Opacity = 1f;
                ZPosition = 0f;
                FogSpreadDistance = 0f;
                FogIntensity = 0f;
                AttackTimer = 0f;
                BrainFogChargeCounter++;
                TeleportTo(Target.Center - Vector2.UnitY * 1200f);
                NPC.netUpdate = true;

                if (BrainFogChargeCounter >= chargeCount)
                    SelectNextAttack();
            }

            // Press hands together after the teleport.
            leftHandDestination = Vector2.Lerp(leftHandDestination, NPC.Center + TeleportVisualsAdjustedScale * new Vector2(-30f, 160f - Sqrt(fogSpreadInterpolant) * 66f), handPressInterpolant);
            rightHandDestination = Vector2.Lerp(rightHandDestination, NPC.Center + TeleportVisualsAdjustedScale * new Vector2(30f, 160f - Sqrt(fogSpreadInterpolant) * 66f), handPressInterpolant);

            // Move hands.
            DefaultHandDrift(Hands[0], leftHandDestination, handSpeedFactor);
            DefaultHandDrift(Hands[1], rightHandDestination, handSpeedFactor);
        }

        public void DoBehavior_PortalChainCharges()
        {
            int portalExistTime = 76;
            int teleportDelay = 44;
            int fireballCount = 4;
            int chargeCount = 3;
            float portalScale = 1f;
            float startingChargeSpeed = 11f;
            float chargeAcceleration = 1.12f;
            float maxFireballShootAngle = ToRadians(75f);
            float fireballShootSpeed = 12f;

            if (Main.expertMode)
            {
                portalExistTime -= 5;
                fireballShootSpeed += 3f;
            }
            if (CalamityWorld.revenge)
            {
                chargeCount++;
                fireballShootSpeed += 3.5f;
            }

            if (CurrentPhase >= 2)
            {
                portalExistTime -= 3;
                fireballCount += 2;
                chargeCount++;
                maxFireballShootAngle *= 1.28f;
                fireballShootSpeed += 4f;
                portalScale += 0.1f;
            }
            float wrappedAttackTimer = AttackTimer % (portalExistTime + 6f);

            // Create portals.
            if (wrappedAttackTimer == 1f)
            {
                SoundEngine.PlaySound(FireballShootSound with { Volume = 2f }, Target.Center);

                // Teleport away from the player at first if this is the first time Noxus is charging, to ensure he doesn't weirdly disappear for the portal teleport.
                if (AttackTimer <= 5f)
                    TeleportToWithDecal(Target.Center + Vector2.UnitY * 2300f);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 portalSpawnPosition = Target.Center + Target.velocity.SafeNormalize(Main.rand.NextVector2Unit()) * 480f;
                    Vector2 portalDirection = (Target.Center - portalSpawnPosition).SafeNormalize(Vector2.UnitY);
                    NewProjectileBetter(portalSpawnPosition + Target.velocity * 30f, portalDirection, ModContent.ProjectileType<DarkPortal>(), 0, 0f, -1, portalScale, portalExistTime);

                    TeleportPosition = portalSpawnPosition + Target.velocity * 30f;
                    TeleportDirection = portalDirection;

                    portalSpawnPosition += portalDirection * Target.Distance(portalSpawnPosition) * 2f;
                    portalDirection = (Target.Center - portalSpawnPosition).SafeNormalize(Vector2.UnitY);
                    NewProjectileBetter(portalSpawnPosition + Target.velocity * 30f, portalDirection, ModContent.ProjectileType<DarkPortal>(), 0, 0f, -1, portalScale, portalExistTime);

                    NPC.netUpdate = true;
                }
            }

            // Do the teleport and release fireballs.
            if (wrappedAttackTimer == teleportDelay)
            {
                NPC.Center = TeleportPosition;
                NPC.velocity = TeleportDirection * startingChargeSpeed;
                NPC.netUpdate = true;

                for (int i = 0; i < NPC.oldPos.Length; i++)
                    NPC.oldPos[i] = Vector2.Zero;

                SoundEngine.PlaySound(JumpscareSound with { Volume = 0.5f }, NPC.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NewProjectileBetter(NPC.Center, Vector2.Zero, ModContent.ProjectileType<DarkWave>(), 0, 0f);
                    for (int i = 0; i < fireballCount; i++)
                    {
                        float fireballShootAngle = Lerp(-maxFireballShootAngle, maxFireballShootAngle, i / (float)(fireballCount - 1f));
                        NewProjectileBetter(NPC.Center, NPC.SafeDirectionTo(Target.Center).RotatedBy(fireballShootAngle) * fireballShootSpeed, ModContent.ProjectileType<DarkFireball>(), CometDamage, 0f);
                    }
                    NewProjectileBetter(NPC.Center, NPC.SafeDirectionTo(Target.Center) * fireballShootSpeed * 0.45f, ModContent.ProjectileType<DarkFireball>(), CometDamage, 0f);
                }
            }

            // Do post-teleport behaviors.
            if (wrappedAttackTimer >= teleportDelay)
            {
                NPC.Opacity = GetLerpValue(teleportDelay - 3f, teleportDelay + 6f, wrappedAttackTimer, true) * GetLerpValue(teleportDelay + 19f, teleportDelay + 15f, wrappedAttackTimer, true);
                NPC.velocity *= chargeAcceleration;
                ChargeAfterimageInterpolant = 1f;

                // Stop moving if very far away from the target. This is done to prevent the music from getting screwed up.
                if (!NPC.WithinRange(Target.Center, 1400f))
                    NPC.velocity = Vector2.Zero;
            }

            if (AttackTimer >= (portalExistTime + 6f) * chargeCount)
            {
                ClearAllProjectiles();
                SelectNextAttack();
            }
        }

        public void DoBehavior_PortalChainCharges2()
        {
            int initialPortalExistTime = 132;
            int aimedPortalExistTime = 75;
            int teleportDelay = 44;
            int cometCount = 7;
            int aimedDashTime = 33;
            int aimedDashCount = 3;
            float portalScale = 1.5f;
            float horizontalPortalOffset = 600f;
            float startingChargeSpeed = 10f;
            float maxChargeSpeed = 93.63f;
            float chargeAcceleration = 1.09f;

            if (PortalChainDashCounter >= 1)
            {
                teleportDelay -= 4;
                aimedPortalExistTime -= 4;
            }

            if (Main.expertMode)
            {
                cometCount += 2;
                initialPortalExistTime -= 10;
            }
            if (CalamityWorld.revenge)
            {
                teleportDelay -= 5;
                aimedDashCount += 2;
            }

            // Create two portals above (Or below if they're flying upward) the target on the first frame.
            if (AttackTimer <= 1f)
            {
                SoundEngine.PlaySound(FireballShootSound with { Volume = 2f }, Target.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 portalSpawnPosition = Target.Center - Vector2.UnitY * (Target.velocity.Y > 0f).ToDirectionInt() * 350f;
                    NewProjectileBetter(portalSpawnPosition - Vector2.UnitX * horizontalPortalOffset, Vector2.UnitX, ModContent.ProjectileType<DarkPortal>(), 0, 0f, -1, portalScale, initialPortalExistTime);
                    NewProjectileBetter(portalSpawnPosition + Vector2.UnitX * horizontalPortalOffset, -Vector2.UnitX, ModContent.ProjectileType<DarkPortal>(), 0, 0f, -1, portalScale, initialPortalExistTime);

                    TeleportDirection = Vector2.UnitX * Main.rand.NextFromList(-1f, 1f);
                    TeleportPosition = portalSpawnPosition - Vector2.UnitX * TeleportDirection * horizontalPortalOffset;
                    NPC.netUpdate = true;
                }

                NPC.Opacity = 0f;
                NPC.netUpdate = true;
            }

            // Perform accelerating teleports.
            if (AttackTimer == teleportDelay || (AttackTimer >= teleportDelay + 1f && NPC.Opacity <= 0f && AttackTimer < initialPortalExistTime - 25f))
            {
                NPC.Opacity = 1f;
                NPC.Center = TeleportPosition;
                NPC.velocity = TeleportDirection * MathF.Max(startingChargeSpeed, NPC.velocity.Length());
                NPC.netSpam = 0;
                NPC.netUpdate = true;

                for (int i = 0; i < NPC.oldPos.Length; i++)
                    NPC.oldPos[i] = Vector2.Zero;

                SoundEngine.PlaySound(JumpscareSound with { Volume = 0.45f, MaxInstances = 9 }, NPC.Center);
            }

            // Accelerate.
            if (AttackTimer >= teleportDelay + 1f && AttackTimer < initialPortalExistTime)
            {
                NPC.velocity = (NPC.velocity * chargeAcceleration).ClampMagnitude(startingChargeSpeed, maxChargeSpeed);

                // Fade away based on how close Noxus is to the next portal.
                NPC.Opacity = GetLerpValue(NPC.velocity.Length() + 4f, NPC.velocity.Length() * 4f, NPC.Distance(TeleportPosition + TeleportDirection * horizontalPortalOffset * 2f), true);
                NPC.Opacity *= GetLerpValue(initialPortalExistTime - 20f, initialPortalExistTime - 23f, AttackTimer, true);
            }

            // Secretly stay below the target as the aimed-ahead portal appears and when it goes away.
            if ((AttackTimer >= initialPortalExistTime && AttackTimer < initialPortalExistTime + teleportDelay) || AttackTimer >= initialPortalExistTime + teleportDelay + 42f)
            {
                NPC.Center = Target.Center + Vector2.UnitY * 1600f;
                NPC.Opacity = 0f;
            }

            // Be invisible right before the initial portal disappears.
            if (AttackTimer > initialPortalExistTime - 15f && AttackTimer < initialPortalExistTime)
                NPC.Opacity = 0f;

            // Create the aimed-ahead portal.
            if (AttackTimer == initialPortalExistTime)
            {
                SoundEngine.PlaySound(FireballShootSound with { Volume = 2f }, Target.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    TeleportPosition = Target.Center + Target.velocity.SafeNormalize(Vector2.UnitX * Target.direction) * new Vector2(800f, 500f);
                    if (Target.velocity.Length() <= 3f)
                        TeleportPosition = Target.Center + Main.rand.NextVector2CircularEdge(450f, 450f);

                    TeleportDirection = (Target.Center - TeleportPosition).SafeNormalize(Vector2.UnitY);

                    NewProjectileBetter(TeleportPosition, TeleportDirection, ModContent.ProjectileType<DarkPortal>(), 0, 0f, -1, portalScale + 0.3f, aimedPortalExistTime);
                    NPC.netUpdate = true;
                }
            }

            // Do the hilarious super charge.
            if (AttackTimer == initialPortalExistTime + teleportDelay)
            {
                NPC.Center = TeleportPosition;
                SoundEngine.PlaySound(JumpscareSound with { Volume = 0.6f }, NPC.Center);
                ScreenEffectSystem.SetChromaticAberrationEffect(NPC.Center, 1.9f, 20);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NewProjectileBetter(NPC.Center, Vector2.Zero, ModContent.ProjectileType<DarkWave>(), 0, 0f);
                    for (int i = 0; i < cometCount; i++)
                        NewProjectileBetter(NPC.Center, (TwoPi * i / cometCount).ToRotationVector2() * 4.4f, ModContent.ProjectileType<DarkComet>(), CometDamage, 0f);
                }

                for (int i = 0; i < NPC.oldPos.Length; i++)
                    NPC.oldPos[i] = Vector2.Zero;

                Target.Calamity().GeneralScreenShakePower = 15f;
                NPC.velocity = TeleportDirection * maxChargeSpeed * 0.75f;

                // Charge in the opposite direction if the player has gone behind the portal.
                if (Vector2.Dot(Target.Center - TeleportPosition, TeleportDirection) < 0f)
                    NPC.velocity *= -1f;

                NPC.Opacity = 1f;
                NPC.netUpdate = true;
            }
            if (AttackTimer >= initialPortalExistTime + teleportDelay)
                NPC.Opacity = 1f;

            // Prepare the next dash.
            if (AttackTimer >= initialPortalExistTime + teleportDelay + aimedDashTime && PortalChainDashCounter < aimedDashCount - 1f)
            {
                AttackTimer = initialPortalExistTime - 1f;
                PortalChainDashCounter++;
                NPC.netUpdate = true;
            }

            if (AttackTimer >= initialPortalExistTime + aimedPortalExistTime)
                SelectNextAttack();
        }

        public void DoBehavior_MigraineAttack()
        {
            // In sync with the sound.
            int attackTransitionDelay = 289;

            // Handle teleport visual effects.
            float fadeInInterpolant = GetLerpValue(0f, DefaultTeleportDelay, AttackTimer, true);
            TeleportVisualsInterpolant = fadeInInterpolant * 0.5f + 0.5f;
            NPC.Opacity = Pow(fadeInInterpolant, 1.34f);

            // Teleport above the player at first.
            if (AttackTimer == 1f)
            {
                TeleportTo(Target.Center - Vector2.UnitY * 200f);
                NPC.velocity = NPC.SafeDirectionTo(Target.Center).RotatedByRandom(0.66f) * -8f;
                SoundEngine.PlaySound(BrainRotSound with { Volume = 0.55f });
            }

            // Disable contact damage and remove defensive things.
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.Calamity().DR = 0f;

            // Slow down.
            NPC.velocity *= 0.98f;

            // Make the hand whirl around.
            HeadRotation = Sin(TwoPi * AttackTimer / 31f) * 0.25f;
            HeadSquishiness = HeadRotation * 0.3f;

            // Have hands grip the edges of the head, as though Noxus is having a serious headache.
            Vector2 headCenter = NPC.Center + HeadOffset;
            Vector2 headTangentDirection = (NPC.rotation + HeadRotation).ToRotationVector2();
            Vector2 leftHandDestination = headCenter + headTangentDirection * NPC.scale * -80f;
            Vector2 rightHandDestination = headCenter + headTangentDirection * NPC.scale * 80f;

            // Periodically create chromatic aberration effects in accordance with the heartbeat of the sound.
            if (AttackTimer % 30f == 29f)
            {
                float attackCompletion = AttackTimer / attackTransitionDelay;
                ScreenEffectSystem.SetChromaticAberrationEffect(NPC.Center, attackCompletion * 2f + 0.3f, 12);
                Main.LocalPlayer.Calamity().GeneralScreenShakePower = Lerp(3f, 9f, attackCompletion) * GetLerpValue(NPC.Distance(Main.LocalPlayer.Center), 3200f, 2400f, true);
            }

            // Jitter in place.
            NPC.Center += Main.rand.NextVector2CircularEdge(1f, 1f) * Lerp(1.5f, 8f, Pow(AttackTimer / attackTransitionDelay, 1.9f));

            // Fade away right before the attack ends. Also have the hands move away from the head.
            float attackEndInterpolant = GetLerpValue(attackTransitionDelay - 96f, attackTransitionDelay - 30f, AttackTimer, true);
            if (attackEndInterpolant > 0f)
            {
                NPC.Opacity *= GetLerpValue(attackTransitionDelay, attackTransitionDelay - 11f, AttackTimer, true);
                leftHandDestination -= (headCenter - leftHandDestination).SafeNormalize(Vector2.UnitY) * new Vector2(90f, -135f) * attackEndInterpolant;
                rightHandDestination -= (headCenter - rightHandDestination).SafeNormalize(Vector2.UnitY) * new Vector2(90f, -135f) * attackEndInterpolant;

                // Make the head stop spinning.
                HeadRotation *= 1f - attackEndInterpolant;
            }

            if (AttackTimer >= attackTransitionDelay)
            {
                SoundEngine.PlaySound(ExplosionTeleportSound, Target.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NewProjectileBetter((Hands[0].Center + Hands[1].Center) * 0.5f, Vector2.Zero, ModContent.ProjectileType<DarkWave>(), 0, 0f);
                ScreenEffectSystem.SetChromaticAberrationEffect(NPC.Center, 2.8f, 60);
                SelectNextAttack();
            }

            // Move hands.
            DefaultHandDrift(Hands[0], leftHandDestination, 1.1f);
            DefaultHandDrift(Hands[1], rightHandDestination, 1.1f);
        }

        public void DoBehavior_DeathAnimation()
        {
            int portalSummonDelay = 90;
            int portalExistTime = 96;
            float portalVerticalOffset = 600f;
            float portalScale = 1.8f;
            Vector2 leftHandDestination = NPC.Center + Hands[0].DefaultOffset * NPC.scale;
            Vector2 rightHandDestination = NPC.Center + Hands[1].DefaultOffset * NPC.scale;

            // Make fog disappear.
            FogIntensity = Clamp(FogIntensity - 0.06f, 0f, 1f);
            FogSpreadDistance = Clamp(FogSpreadDistance - 0.02f, 0f, 1f);

            // Teleport above the player on the first frame.
            if (AttackTimer == 1f)
                TeleportToWithDecal(Target.Center - Vector2.UnitY * 300f);

            // Disable damage. It is not relevant for this behavior.
            NPC.damage = 0;
            NPC.dontTakeDamage = true;

            // Close the HP bar.
            NPC.Calamity().ShouldCloseHPBar = true;

            // Move hands.
            DefaultHandDrift(Hands[0], leftHandDestination, 1.1f);
            DefaultHandDrift(Hands[1], rightHandDestination, 1.1f);

            // Create the portal.
            if (AttackTimer == portalSummonDelay)
            {
                SoundEngine.PlaySound(FireballShootSound);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NewProjectileBetter(NPC.Center + Vector2.UnitY * portalVerticalOffset, -Vector2.UnitY, ModContent.ProjectileType<DarkPortal>(), 0, 0f, -1, portalScale, portalExistTime);
            }

            // Move into the portal and leave.
            if (AttackTimer >= portalSummonDelay + 30f)
            {
                NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.UnitY * portalVerticalOffset / 18f, 0.09f);
                NPC.Opacity = Clamp(NPC.Opacity - 0.07f, 0f, 1f);
            }

            // Disappear.
            if (AttackTimer >= portalSummonDelay + portalExistTime)
            {
                NPC.Center = Target.Center - Vector2.UnitY * 900f;
                if (NPC.position.Y < 50f)
                    NPC.position.Y = 50f;

                NPC.life = 0;
                NPC.HitEffect(0, 9999);
                NPC.NPCLoot();
                NPC.checkDead();
                NPC.active = false;
            }
        }

        public void SelectNextAttack()
        {
            AttackTimer = 0f;
            NPC.Opacity = 1f;
            TeleportVisualsInterpolant = 0f;
            ZPosition = 0f;
            BrainFogChargeCounter = 0;
            PortalChainDashCounter = 0;
            PhaseCycleIndex++;

            // Cycle through attacks based on phase.
            if (CurrentPhase == 2)
                CurrentAttack = Phase3AttackCycle[PhaseCycleIndex % Phase3AttackCycle.Length];
            else if (CurrentPhase == 1)
                CurrentAttack = Phase2AttackCycle[PhaseCycleIndex % Phase2AttackCycle.Length];
            else
                CurrentAttack = Phase1AttackCycle[PhaseCycleIndex % Phase1AttackCycle.Length];

            NPC.netUpdate = true;
        }

        public void BasicFlyMovement(Vector2 hoverDestination)
        {
            if (Distance(hoverDestination.X, NPC.Center.X) >= 100f || Distance(hoverDestination.Y, NPC.Center.Y) >= 180f)
            {
                Vector2 idealVelocity = (hoverDestination - NPC.Center) * 0.075f;
                NPC.velocity = Vector2.Lerp(NPC.velocity, idealVelocity, 0.11f);
            }
        }

        public void DefaultHandDrift(EntropicGodHand hand, Vector2 hoverDestination, float speedFactor = 1f)
        {
            float maxFlySpeed = NPC.velocity.Length() + 13f;
            Vector2 idealVelocity = (hoverDestination - hand.Center) * 0.2f;

            // Propel the hands away from the center of Noxus, to prevent them from being unseeable.
            if (CurrentAttack != EntropicGodAttackType.MigraineAttack && CurrentAttack != EntropicGodAttackType.RapidExplosiveTeleports && CurrentAttack != EntropicGodAttackType.BrainFogAndThreeDimensionalCharges && CurrentAttack != EntropicGodAttackType.Phase2Transition)
                idealVelocity += NPC.SafeDirectionTo(hand.Center) * Remap(NPC.Distance(hand.Center) / NPC.scale, 200f, 50f, 0f, 27f);

            if (idealVelocity.Length() >= maxFlySpeed)
                idealVelocity = idealVelocity.SafeNormalize(Vector2.UnitY) * maxFlySpeed;
            if (hand.Velocity.Length() <= maxFlySpeed * 0.7f)
                hand.Velocity *= 1.056f;

            hand.Velocity = Vector2.Lerp(hand.Velocity, idealVelocity * speedFactor, 0.39f);
        }

        public Vector2 CalculateHandOffsetForClapAnticipation(float arcInterpolant, float maxArcAngle, bool left)
        {
            // This blends between a sharp x^3 polynomial and the more gradual smoothstep function, to take advantage of both the very sudden increases
            // with exponents greater than one and the calm, non-linear slowdown right before reaching one that smoothsteps are notorious for.
            // The connective interpolant of x^2 exists to give more weight to the sharp polynomial at first.
            float smoothenedInterpolant = Lerp(Pow(arcInterpolant, 3f), MathHelper.SmoothStep(0f, 1f, arcInterpolant), Pow(arcInterpolant, 2f));
            float arcAngle = maxArcAngle * smoothenedInterpolant;
            return NPC.Center + Vector2.UnitY.RotatedBy(arcAngle * left.ToDirectionInt()) * 250f + Vector2.UnitX * left.ToDirectionInt() * -4f;
        }

        public void PerformZPositionEffects()
        {
            // Give the illusion of being in 3D space by shrinking. This is also followed by darkening effects in the draw code, to make it look like he's fading into the dark clouds.
            // The DrawBehind section of code causes Noxus to layer being things like trees to better sell the illusion.
            NPC.scale = 1f / (ZPosition + 1f);
            if (Math.Abs(ZPosition) >= 2.03f)
            {
                NPC.dontTakeDamage = true;
                NPC.ShowNameOnHover = false;
            }

            if (ZPosition <= -0.96f)
                NPC.scale = 0f;

            // Resize the hitbox based on scale.
            int oldWidth = NPC.width;
            int idealWidth = (int)(NPC.scale * 122f);
            int idealHeight = (int)(NPC.scale * 290f);
            if (idealWidth != oldWidth)
            {
                NPC.position.X += NPC.width / 2;
                NPC.position.Y += NPC.height / 2;
                NPC.width = idealWidth;
                NPC.height = idealHeight;
                NPC.position.X -= NPC.width / 2;
                NPC.position.Y -= NPC.height / 2;
            }
        }

        public void MakeHandsOpen()
        {
            for (int i = 0; i < Hands.Length; i++)
                Hands[i].ShouldOpen = true;
        }

        public void PreparePhaseTransitionsIfNecessary()
        {
            if (CurrentPhase == 0 && LifeRatio < Phase2LifeRatio)
            {
                SelectNextAttack();
                ClearAllProjectiles();
                CurrentAttack = EntropicGodAttackType.Phase2Transition;
                PhaseCycleIndex = -1;
                CurrentPhase++;
                NPC.netUpdate = true;
            }

            if (CurrentPhase == 1 && LifeRatio < Phase3LifeRatio)
            {
                SelectNextAttack();
                ClearAllProjectiles();
                CurrentAttack = EntropicGodAttackType.Phase3Transition;
                PhaseCycleIndex = -1;
                CurrentPhase++;
                NPC.netUpdate = true;
            }
        }

        public void UpdateHands()
        {
            foreach (EntropicGodHand hand in Hands)
            {
                hand.FrameTimer++;
                if (hand.FrameTimer % 5 == 4)
                    hand.Frame = Clamp(hand.Frame - hand.ShouldOpen.ToDirectionInt(), 0, 2);

                hand.Center += hand.Velocity;
                hand.Velocity *= 0.99f;
            }
        }

        public void TeleportTo(Vector2 teleportPosition)
        {
            NPC.Center = teleportPosition;
            NPC.velocity = Vector2.Zero;
            NPC.netUpdate = true;

            // Reorient hands to account for the sudden change in position.
            foreach (EntropicGodHand hand in Hands)
                hand.Center = NPC.Center + (hand.Center - NPC.Center).SafeNormalize(Vector2.UnitY) * 100f;

            // Reset the oldPos array, so that afterimages don't suddenly "jump" due to the positional change.
            for (int i = 0; i < NPC.oldPos.Length; i++)
                NPC.oldPos[i] = NPC.position;

            SoundEngine.PlaySound(Providence.NearBurnSound with
            {
                Pitch = 0.5f,
                Volume = 2f,
                MaxInstances = 8
            }, NPC.Center);

            // Create teleport particle effects.
            ExpandingGreyscaleCircleParticle circle = new(NPC.Center, Vector2.Zero, new(219, 194, 229), 10, 0.28f);
            VerticalLightStreakParticle bigLightStreak = new(NPC.Center, Vector2.Zero, new(228, 215, 239), 10, new(2.4f, 3f));
            MagicBurstParticle magicBurst = new(NPC.Center, Vector2.Zero, new(150, 109, 219), 12, 0.1f);
            for (int i = 0; i < 30; i++)
            {
                Vector2 smallLightStreakSpawnPosition = NPC.Center + Main.rand.NextVector2Square(-NPC.width, NPC.width) * new Vector2(0.4f, 0.2f);
                Vector2 smallLightStreakVelocity = Vector2.UnitY * Main.rand.NextFloat(-3f, 3f);
                VerticalLightStreakParticle smallLightStreak = new(smallLightStreakSpawnPosition, smallLightStreakVelocity, Color.White, 10, new(0.1f, 0.3f));
                GeneralParticleHandler.SpawnParticle(smallLightStreak);
            }

            GeneralParticleHandler.SpawnParticle(circle);
            GeneralParticleHandler.SpawnParticle(bigLightStreak);
            GeneralParticleHandler.SpawnParticle(magicBurst);
        }

        public void TeleportToWithDecal(Vector2 teleportPosition)
        {
            // Create the decal particle at the old position before teleporting.
            NoxusDecalParticle decal = new(NPC.Center, NPC.rotation, Color.Lerp(Color.Cyan, Color.HotPink, 0.7f), 27, NPC.scale);
            GeneralParticleHandler.SpawnParticle(decal);

            NPC.Center = teleportPosition;
            NPC.velocity = Vector2.Zero;
            NPC.netUpdate = true;

            // Reorient hands to account for the sudden change in position.
            foreach (EntropicGodHand hand in Hands)
                hand.Center = NPC.Center + (hand.Center - NPC.Center).SafeNormalize(Vector2.UnitY) * 100f;

            // Reset the oldPos array, so that afterimages don't suddenly "jump" due to the positional change.
            for (int i = 0; i < NPC.oldPos.Length; i++)
                NPC.oldPos[i] = NPC.position;

            SoundEngine.PlaySound(Providence.NearBurnSound with
            {
                Pitch = 0.5f,
                Volume = 2f,
                MaxInstances = 8
            }, NPC.Center);

            ExpandingGreyscaleCircleParticle circle = new(NPC.Center, Vector2.Zero, new Color(219, 194, 229) * 0.5f, 10, 0.28f);
            for (int i = 0; i < 30; i++)
            {
                Vector2 smallLightStreakSpawnPosition = NPC.Center + Main.rand.NextVector2Square(-NPC.width, NPC.width) * new Vector2(0.4f, 0.2f);
                Vector2 smallLightStreakVelocity = Vector2.UnitY * Main.rand.NextFloat(-3f, 3f);
                VerticalLightStreakParticle smallLightStreak = new(smallLightStreakSpawnPosition, smallLightStreakVelocity, Color.White, 10, new(0.1f, 0.3f));
                GeneralParticleHandler.SpawnParticle(smallLightStreak);
            }

            GeneralParticleHandler.SpawnParticle(circle);
        }

        public static void ClearAllProjectiles()
        {
            List<int> projectileTypesToDelete = new()
            {
                ModContent.ProjectileType<DarkComet>(),
                ModContent.ProjectileType<DarkFireball>(),
                ModContent.ProjectileType<DarkPortal>(),
                ModContent.ProjectileType<NightmareDeathRay>(),
                ModContent.ProjectileType<NoxSpike>(),
                ModContent.ProjectileType<NoxusExplosion>(),
                ModContent.ProjectileType<NoxusGas>(),
            };

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < Main.maxProjectiles; j++)
                {
                    Projectile p = Main.projectile[j];
                    if (p.active && projectileTypesToDelete.Contains(p.type))
                        p.Kill();
                }
            }
        }

        public static TwinkleParticle CreateTwinkle(Vector2 spawnPosition, Vector2 scaleFactor)
        {
            Color twinkleColor = Color.Lerp(Color.HotPink, Color.Cyan, Main.rand.NextFloat(0.36f, 0.64f));
            TwinkleParticle twinkle = new(spawnPosition, Vector2.Zero, twinkleColor, 30, 6, scaleFactor);
            GeneralParticleHandler.SpawnParticle(twinkle);

            SoundEngine.PlaySound(TwinkleSound);
            return twinkle;
        }
        #endregion AI

        #region Drawing

        public override void DrawBehind(int index)
        {
            if (NPC.hide && NPC.Opacity >= 0.02f)
            {
                if (ZPosition < -0.1f)
                    ScreenOverlaysSystem.DrawCacheAfterNoxusFog.Add(index);
                else if (ShouldDrawBehindTiles)
                    ScreenOverlaysSystem.DrawCacheBeforeBlack.Add(index);
                else
                    Main.instance.DrawCacheNPCProjectiles.Add(index);
            }
        }

        public override void BossHeadSlot(ref int index)
        {
            // Make the head icon disappear if Noxus is invisible.
            if (TeleportVisualsAdjustedScale.Length() <= 0.1f || NPC.Opacity <= 0.45f)
                index = -1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // Initialize hands if necessary.
            Texture2D handTexture = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Noxus/EntropicGodHand").Value;
            InitializeHandsIfNecessary();

            // Draw the back and use preset hand offests if in the bestiary.
            if (NPC.IsABestiaryIconDummy)
            {
                DrawBack(NPC.Center - screenPos, Color.Lerp(Color.Purple, Color.Black, 0.885f), NPC.rotation);
                Hands[0].Center = NPC.Center + new Vector2(136f, 54f) * NPC.scale;
                Hands[1].Center = NPC.Center + new Vector2(-136f, 54f) * NPC.scale;
                Hands[0].Frame = 2;
                Hands[1].Frame = 2;
            }

            // Draw the main texture and bright afterimages.
            float horizontalRibOffsetTime = Main.GlobalTimeWrappedHourly * 2f;
            if (ChargeAfterimageInterpolant > 0f)
            {
                float universalOpacity = 0.27f;
                float minClosenessInterpolant = 0.86f;

                if (CurrentPhase >= 1)
                {
                    universalOpacity = 0.43f;
                    minClosenessInterpolant = 0.76f;
                }
                if (CurrentAttack == EntropicGodAttackType.PortalChainCharges2)
                {
                    universalOpacity = 0.95f;
                    minClosenessInterpolant = 0.64f;
                }

                // Make afterimages less tight during spin charges, so that the circular motion can be better appreciated.
                if (CurrentAttack == EntropicGodAttackType.RealityWarpSpinCharge)
                {
                    universalOpacity = 0.34f;
                    minClosenessInterpolant = 0.26f;
                }
                float afterimageClosenessInterpolant = Lerp(1f, minClosenessInterpolant, ChargeAfterimageInterpolant);
                for (int i = 60; i >= 0; i--)
                {
                    // Make afterimages sharply taper off in opacity as they get longer.
                    float afterimageOpacity = ChargeAfterimageInterpolant * Pow(1f - i / 61f, 5.9f) * universalOpacity;

                    Vector2 afterimageDrawPosition = Vector2.Lerp(NPC.oldPos[i] + NPC.Size * 0.5f, NPC.Center, afterimageClosenessInterpolant) - screenPos;
                    Color afterimageColor = Color.Lerp(new(209, 155, 218, 0), Color.Black, Abs(ZPosition) * 0.35f) * afterimageOpacity;

                    // The subtraction of i / 60 acts as a correction offset, since ribs from previous frames had different time values.
                    float ribAnimationSpeed = horizontalRibOffsetTime / Main.GlobalTimeWrappedHourly;
                    DrawBody(afterimageDrawPosition, NPC.GetAlpha(afterimageColor), NPC.rotation, horizontalRibOffsetTime - i * ribAnimationSpeed / 60f);
                }
            }
            DrawBody(NPC.Center - screenPos, NPC.GetAlpha(GeneralColor), NPC.rotation, horizontalRibOffsetTime);
            DrawHead(NPC.Center - screenPos, NPC.GetAlpha(GeneralColor), NPC.rotation + HeadRotation);

            // Draw the hands.
            foreach (EntropicGodHand hand in Hands)
            {
                float handRotation = NPC.AngleTo(hand.Center);

                // Make the hands aim towards the hand if close.
                Vector2 headPosition = NPC.Center + HeadOffset;
                float angleToHead = (headPosition - hand.Center).ToRotation() - Sign(hand.DefaultOffset.X) * PiOver2;
                handRotation = handRotation.AngleLerp(angleToHead, GetLerpValue(148f, 60f, hand.Center.Distance(headPosition), true));

                // Use the hand rotation override instead if it's defined.
                handRotation = hand.RotationOverride ?? handRotation;

                Vector2 handDrawPosition = hand.Center - screenPos;
                Rectangle frame = handTexture.Frame(1, 3, 0, hand.Frame);
                SpriteEffects handDirection = hand.Center.X >= NPC.Center.X ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                if (handDirection == SpriteEffects.FlipHorizontally)
                    handRotation += Pi;

                Main.EntitySpriteDraw(handTexture, handDrawPosition, frame, NPC.GetAlpha(GeneralColor), handRotation, frame.Size() * 0.5f, NPC.scale * 0.75f, handDirection, 0);
            }

            // Draw the laser telegraph area once ready.
            if (LaserTelegraphOpacity > 0f)
                DrawLaserTelegraphZone();

            return false;
        }

        public void DrawBody(Vector2 drawPosition, Color color, float rotation, float horizontalRibOffsetTime)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;

            // Draw the tendril parts.
            Main.EntitySpriteDraw(texture, drawPosition - Vector2.UnitY.RotatedBy(rotation) * NPC.scale * 48f, NPC.frame, color, rotation, NPC.frame.Size() * 0.5f, TeleportVisualsAdjustedScale, 0, 0);

            // Draw ribs.
            for (int i = 3; i >= 1; i--)
            {
                float horizontalRibOffset = Pow(Sin(horizontalRibOffsetTime + i), 3f) * 6f;

                Vector2 ribDrawOffset = new Vector2(horizontalRibOffset + 36f, i * 38f + 55f).RotatedBy(rotation) * NPC.scale;
                Texture2D ribsTexture = ModContent.Request<Texture2D>($"CalRemix/Content/NPCs/Bosses/Noxus/EntropicGodRibs{i}").Value;
                Main.EntitySpriteDraw(ribsTexture, drawPosition + ribDrawOffset, null, color, rotation, ribsTexture.Size() * 0.5f, TeleportVisualsAdjustedScale, SpriteEffects.FlipHorizontally, 0);

                ribDrawOffset = new Vector2(-horizontalRibOffset - 36f, i * 38f + 55f).RotatedBy(rotation) * NPC.scale;
                Main.EntitySpriteDraw(ribsTexture, drawPosition + ribDrawOffset, null, color, rotation, ribsTexture.Size() * 0.5f, TeleportVisualsAdjustedScale, 0, 0);
            }
        }

        public void DrawBack(Vector2 drawPosition, Color color, float rotation)
        {
            drawPosition += Vector2.UnitY.RotatedBy(rotation) * NPC.scale * 6f;
            Texture2D backTexture = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Noxus/EntropicGodBack").Value;
            Main.EntitySpriteDraw(backTexture, drawPosition, null, color, rotation, backTexture.Size() * 0.5f, TeleportVisualsAdjustedScale, 0, 0);
        }

        public void DrawHead(Vector2 drawPosition, Color color, float rotation)
        {
            // Calculate the head scale factor. This includes a bit of squishiness if desired.
            Vector2 headScaleFactor = Vector2.One;
            headScaleFactor.Y += Cos(Main.GlobalTimeWrappedHourly * -12f) * HeadSquishiness - HeadRotation * 0.25f;

            // Draw the head.
            Texture2D headTexture = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Noxus/EntropicGodHead").Value;
            Main.EntitySpriteDraw(headTexture, drawPosition + HeadOffset, null, color, rotation, headTexture.Size() * 0.5f, headScaleFactor * TeleportVisualsAdjustedScale, 0, 0);

            // Draw an eye gleam over the head, if said gleam is in effect.
            if (EyeGleamInterpolant > 0f)
            {
                CalRemixHelper.SetBlendState(Main.spriteBatch, BlendState.Additive);

                Texture2D eyeTexture = ModContent.Request<Texture2D>("Terraria/Images/Extra_89").Value;
                float eyePulse = Main.GlobalTimeWrappedHourly * 2.1f % 1f;
                Vector2 eyePosition = drawPosition + HeadOffset + new Vector2(-20f, 12f).RotatedBy(HeadRotation) * headScaleFactor * TeleportVisualsAdjustedScale;
                Vector2 baseEyeScale = headScaleFactor * TeleportVisualsAdjustedScale * EyeGleamInterpolant * new Vector2(0.67f, 0.59f) * 2f;
                Main.EntitySpriteDraw(eyeTexture, eyePosition, null, Color.Fuchsia * EyeGleamInterpolant, rotation, eyeTexture.Size() * 0.5f, baseEyeScale, 0, 0);
                Main.EntitySpriteDraw(eyeTexture, eyePosition, null, Color.Cyan * EyeGleamInterpolant, rotation, eyeTexture.Size() * 0.5f, baseEyeScale * new Vector2(0.7f, 1f), 0, 0);
                Main.EntitySpriteDraw(eyeTexture, eyePosition, null, Color.Violet * EyeGleamInterpolant * (1f - eyePulse), rotation, eyeTexture.Size() * 0.5f, baseEyeScale * new Vector2(eyePulse * 2f + 1f, eyePulse + 1f), 0, 0);

                Main.spriteBatch.ExitShaderRegion();
            }

            // Draw the big eye over the head if necessary.
            if (BigEyeOpacity > 0f)
            {
                CalRemixHelper.SetBlendState(Main.spriteBatch, BlendState.Additive);

                Texture2D eyeTexture = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Noxus/NoxusEye").Value;
                float eyePulse = Main.GlobalTimeWrappedHourly * 1.3f % 1f;
                Vector2 baseEyeScale = headScaleFactor * TeleportVisualsAdjustedScale * BigEyeOpacity * 0.15f;
                Vector2 eyePosition = drawPosition + HeadOffset + new Vector2(19f, -12f).RotatedBy(HeadRotation) * baseEyeScale;
                Main.EntitySpriteDraw(eyeTexture, eyePosition, null, Color.BlueViolet.MultiplyRGBA(color) * BigEyeOpacity, rotation, eyeTexture.Size() * 0.5f, baseEyeScale, 0, 0);
                Main.EntitySpriteDraw(eyeTexture, eyePosition, null, Color.MidnightBlue.MultiplyRGBA(color) * BigEyeOpacity * (1f - eyePulse), rotation, eyeTexture.Size() * 0.5f, baseEyeScale * (eyePulse * 0.39f + 1f), 0, 0);

                Main.spriteBatch.ExitShaderRegion();
            }
        }

        public void DrawLaserTelegraphZone()
        {
            CalRemixHelper.SetBlendState(Main.spriteBatch, BlendState.Additive);

            Texture2D telegraphBorderTexture = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Noxus/LaserTelegraphBorder").Value;
            Vector2 telegraphBorderDrawPosition = new(NPC.Center.X - Main.screenPosition.X, Main.screenHeight * 0.5f);
            Vector2 scale = new Vector2(Main.screenWidth, Main.screenHeight) / telegraphBorderTexture.Size();
            Vector2 origin = new(0f, 0.5f);
            SpriteEffects direction = SpriteEffects.FlipHorizontally;
            if (LaserSpinDirection < 0f)
            {
                origin.X = 1f - origin.X;
                direction = SpriteEffects.None;
            }
            Main.spriteBatch.Draw(telegraphBorderTexture, telegraphBorderDrawPosition, null, Color.MediumPurple * LaserTelegraphOpacity * 0.45f, 0f, telegraphBorderTexture.Size() * origin, scale, direction, 0f);
            Main.spriteBatch.Draw(telegraphBorderTexture, telegraphBorderDrawPosition, null, Color.Fuchsia * LaserTelegraphOpacity * 0.2f, 0f, telegraphBorderTexture.Size() * origin, scale, direction, 0f);
            Main.spriteBatch.DrawBloomLine(NPC.Center - Vector2.UnitY * 4000f, NPC.Center + Vector2.UnitY * 4000f, Color.Purple * LaserTelegraphOpacity, LaserTelegraphOpacity * 30f);

            Main.spriteBatch.ExitShaderRegion();
        }

        public void DrawDecal(Vector2 drawPosition, Color decalColor, float rotation)
        {
            Color baseColor = decalColor;
            DrawBack(drawPosition, baseColor * 0.3f, rotation);
            DrawBody(drawPosition, baseColor, rotation, 0f);
            DrawHead(drawPosition, baseColor, rotation);
        }
        #endregion Drawing

        #region Hit Effects and Loot

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay >= 1 || CurrentAttack == EntropicGodAttackType.DeathAnimation)
                return;

            NPC.soundDelay = 9;
            SoundEngine.PlaySound(HitSound, NPC.Center);
        }

        public override bool CheckDead()
        {
            AttackTimer = 0f;

            // Disallow natural death. The time check here is as a way of catching cases where multiple hits happen on the same frame and trigger a death.
            // If it just checked the attack state, then hit one would trigger the state change, set the HP to one, and the second hit would then deplete the
            // single HP and prematurely kill Noxus.
            if (CurrentAttack == EntropicGodAttackType.DeathAnimation && AttackTimer >= 10f)
                return true;

            SelectNextAttack();
            ClearAllProjectiles();
            NPC.life = 1;
            NPC.dontTakeDamage = true;
            CurrentAttack = EntropicGodAttackType.DeathAnimation;
            NPC.netUpdate = true;
            return false;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // General drops.
            npcLoot.Add(ModContent.ItemType<NoxiousEvocator>());
            npcLoot.Add(ModContent.ItemType<NoxusSprayer>());

            // Vanity and decorations.
            npcLoot.Add(ModContent.ItemType<NoxusMask>(), 7);
            npcLoot.Add(ModContent.ItemType<NoxusTrophy>(), 10);
            npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ModContent.ItemType<NoxusRelic>());
            npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ModContent.ItemType<OblivionRattle>());
        }

        public override void BossLoot(ref int potionType) => potionType = ModContent.ItemType<OmegaHealingPotion>();

        // Ensure that Noxus' contact damage adhere to the special boss-specific cooldown slot, to prevent things like lava cheese.
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<NoxusFumes>(), DebuffDuration_RegularAttack);
        }

        // Timed DR but a bit different. I'm typically very, very reluctant towards this mechanic, but given that this boss exists in shadowspec tier, I am willing to make
        // an exception. This will not cause the dumb "lol do 0 damage for 30 seconds" problems that Calamity had in the past.
        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            // Calculate how far ahead Noxus' HP is relative to how long he's existed so far.
            // This would be one if you somehow got him to death on the first frame of the fight.
            // This naturally tapers off as the fight goes on.
            float fightLengthInterpolant = GetLerpValue(0f, IdealFightDuration, FightLength, true);
            float aheadOfFightLengthInterpolant = MathF.Max(0f, 1f - fightLengthInterpolant - LifeRatio);

            float damageReductionInterpolant = Pow(aheadOfFightLengthInterpolant, 0.64f);
            float damageReductionFactor = Lerp(1f, MaxTimedDRDamageReduction, damageReductionInterpolant);
            modifiers.FinalDamage *= damageReductionFactor;
        }
        #endregion Hit Effects and Loot

        #region Gotta Manually Disable Despawning Lmao
        // Disable natural despawning for Noxus.
        public override bool CheckActive() => false;

        #endregion Gotta Manually Disable Despawning Lmao
    }
}
