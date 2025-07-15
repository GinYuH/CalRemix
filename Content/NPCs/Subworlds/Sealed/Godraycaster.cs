using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.Tiles.Ores;
using CalamityMod.CalPlayer;
using CalRemix.Core.Subworlds;
using Ionic.Zip;
using CalamityMod.Particles;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.World;
using CalamityMod.Graphics.Primitives;
using Terraria.Graphics.Shaders;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class Godraycaster : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];

        public ref float ExtraVar => ref NPC.ai[2];

        public ref float ExtraVar2 => ref NPC.ai[3];
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
            SpawnAnim = 0,
            ProjectileShotsOne = 1,
            ProjectileShotsTwo = 2,
            Dashes = 3,
            Stunned = 4
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 54;
            NPC.height = 86;
            NPC.LifeMaxNERB(25000, 50000, 150000);
            NPC.damage = 250;
            NPC.defense = 24;
            NPC.noGravity = true;
            NPC.HitSound = AuricOre.MineSound;
            NPC.DeathSound = BetterSoundID.ItemElectricFizzleExplosion;
            NPC.noTileCollide = true;
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.DR_NERD(0.1f);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 10, 0, 0);
            NPC.boss = true;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.Calamity().VulnerableToCold = true;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            if (!NPC.HasPlayerTarget || Target.dead)
            {
                NPC.active = false;
                foreach (NPC n in Main.ActiveNPCs)
                {
                    if (n.type == ModContent.NPCType<DreadonFriendly>())
                    {
                        n.active = false;
                    }
                }
            }
            switch ((PhaseType)State)
            {
                case PhaseType.SpawnAnim:
                    {
                        if (Timer == 0)
                        {
                            NPC.velocity = NPC.DirectionTo(SealedSubworldData.TentCenter) * 40;
                        }
                        else
                        {
                            NPC.velocity *= 0.98f;
                        }

                        if (NPC.Center.X > SealedSubworldData.TentCenter.X)
                        {
                            NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.DirectionTo(Target.Center).ToRotation(), 0.05f);
                            ExtraVar++;
                            if (ExtraVar > 90)
                            {
                                ChangePhase(PhaseType.ProjectileShotsOne);
                            }
                        }
                        else
                        {
                            NPC.rotation = NPC.velocity.ToRotation();
                        }
                        Timer++;
                        break;
                    }
                case PhaseType.ProjectileShotsOne:
                    {
                        if (Timer <= 1)
                        {
                            OldPosition = NPC.Center;
                        }
                        else
                        {
                            Vector2 target = SealedSubworldData.TentCenter + SealedSubworldData.TentCenter.DirectionTo(OldPosition).RotatedBy(Timer * 0.025f) * 800;
                            NPC.velocity = NPC.DirectionTo(target) * MathHelper.Lerp(20, 1, Utils.GetLerpValue(1000, 0, NPC.Distance(target), true));
                            NPC.rotation = NPC.DirectionTo(SealedSubworldData.TentCenter).ToRotation();

                            if (Timer > 30 && Timer % 20 == 0)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    int projCount = 3;
                                    for (int i = 0; i < projCount; i++)
                                    {
                                        int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(SealedSubworldData.TentCenter).RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, i / ((float)projCount - 1))) * 30, ProjectileID.MartianTurretBolt, CalRemixHelper.ProjectileDamage(240, 350), 1);
                                        Main.projectile[p].tileCollide = false;
                                    }
                                }
                            }
                        }
                        if (Timer > 300)
                        {
                            ChangePhase(PhaseType.Dashes);
                        }
                        Timer++;
                        break;
                    }
                case PhaseType.Dashes:
                    {
                        float coolTime = 60;
                        float dashLength = 30;
                        float localTimer = Timer % (coolTime + dashLength);
                        if (localTimer < coolTime)
                        {
                            NPC.velocity *= 0.95f;
                            NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.DirectionTo(Target.Center).ToRotation(), 0.4f);
                        }
                        else if (localTimer == coolTime)
                        {
                            NPC.velocity = NPC.DirectionTo(Target.Center) * 50;
                            for (int i = 0; i < NPC.oldRot.Length; i++)
                            {
                                NPC.oldRot[i] = NPC.rotation;
                            }
                        }
                        else
                        {
                            NPC.velocity *= 0.97f;
                        }
                        Timer++;
                        break;
                    }
                case PhaseType.ProjectileShotsTwo:
                    {
                        break;
                    }
                case PhaseType.Stunned:
                    {
                        break;
                    }
            }

            foreach (Player p in Main.ActivePlayers)
            {
                if (p.Center.X < SealedSubworldData.TentLeft || p.Center.X > SealedSubworldData.TentRight || p.Center.Y > SealedSubworldData.TentBottom || p.Center.Y < SealedSubworldData.TentTop)
                {
                    p.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), 2222, 1);
                }
            }
        }

        public void ChangePhase(PhaseType newPhase)
        {
            ExtraVar = 0;
            ExtraVar2 = 0;
            Timer = 0;
            SavePosition = Vector2.Zero;
            OldPosition = Vector2.Zero;
            State = (int)newPhase;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return State == (int)PhaseType.Stunned || State == (int)PhaseType.Dashes;
        }

        public override bool CheckActive()
        {
            bool alive = false;
            foreach (Player p in Main.ActivePlayers)
            {
                if (!p.dead)
                {
                    alive = true;
                }
            }
            return !alive;
        }

        public override void BossLoot(ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 60;
            GameShaders.Misc["CalamityMod:ImpFlameTrail"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/ScarletDevilStreak"));

            float correctedRotation = NPC.rotation + MathHelper.PiOver2;

            float currentSegmentRotation = correctedRotation;
            List<Vector2> ribbonDrawPositions = new List<Vector2>();
            int ct = 50;
            for (int i = 0; i < ct; i++)
            {
                float ribbonCompletionRatio = i / (float)ct;
                float wrappedAngularOffset = MathHelper.WrapAngle(NPC.oldRot[i + 1] - currentSegmentRotation + MathHelper.PiOver2) * 0.1f;

                Vector2 ribbonSegmentOffset = Vector2.UnitY.RotatedBy(currentSegmentRotation) * ribbonCompletionRatio * 290f;
                ribbonDrawPositions.Add(NPC.Center + ribbonSegmentOffset);

                currentSegmentRotation += wrappedAngularOffset;
            }

            spriteBatch.EnterShaderRegion();
            PrimitiveRenderer.RenderTrail(ribbonDrawPositions, new(new((float f) => 80 * (1 - f)), new PrimitiveSettings.VertexColorFunction((float f) => (Color.Lerp(Color.CornflowerBlue, default, f))), shader: GameShaders.Misc["CalamityMod:ImpFlameTrail"]));
            spriteBatch.ExitShaderRegion();

            Texture2D aye = TextureAssets.Npc[Type].Value;
            spriteBatch.Draw(aye, NPC.Center - screenPos, null, drawColor, NPC.rotation + MathHelper.PiOver2, aye.Size() / 2, NPC.scale, 0, 0);
            return false;
        } 
    }
}