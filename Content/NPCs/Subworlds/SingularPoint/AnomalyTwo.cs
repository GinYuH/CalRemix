using CalRemix.Core.Biomes;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;

namespace CalRemix.Content.NPCs.Subworlds.SingularPoint
{
   // [AutoloadBossHead]
    public class AnomalyTwo : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref float Timer => ref NPC.ai[1];

        public ref float ExtraOne => ref NPC.ai[2];

        public ref float ExtraTwo => ref NPC.ai[3];

        public ref float JawRotation => ref NPC.localAI[2];

        public Vector2 SavePosition
        {
            get => new Vector2(NPC.Calamity().newAI[0], NPC.Calamity().newAI[1]);
            set
            {
                NPC.Calamity().newAI[0] = value.X;
                NPC.Calamity().newAI[1] = value.Y;
            }
        }

        public NPC DragonHead
        {
            get => Main.npc[(int)NPC.localAI[0]];
            set => NPC.localAI[0] = value.whoAmI;
        }

        public NPC OrbHead
        {
            get => Main.npc[(int)NPC.localAI[1]];
            set => NPC.localAI[1] = value.whoAmI;
        }

        public enum PhaseType
        {
            PhaseOne = 0,
            PhaseTwo = 1,
            Rise = 2,
            IdleBehaviour = 3,
            Enrage = 4,
            Desperation = 5
        }

        public PhaseType CurrentPhase
        {
            get => (PhaseType)Phase;
            set => Phase = (int)value;
        }

        public override void SetStaticDefaults()
        {
            NPCID.Sets.MustAlwaysDraw[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 150;
            NPC.height = 150;
            NPC.lifeMax = 500000;
            NPC.damage = 340;
            NPC.defense = 100;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.HitSound = null;
            NPC.DeathSound = null;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 60);
            NPC.boss = true;
            NPC.alpha = 255;
            NPC.dontTakeDamage = true;
            NPC.behindTiles = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VoidForestBiome>().Type };
            Music = CalRemixMusic.TheCalamity;
        }

        public override void AI()
        {
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];
            if (target == null || !target.active || target.dead)
            {
                NPC.active = false;
                return;
            }
            if (!DragonHead.active || DragonHead.type != ModContent.NPCType<AnomalyOne>())
            {
                int dHead = NPC.FindFirstNPC(ModContent.NPCType<AnomalyOne>());
                if (dHead != -1)
                {
                    DragonHead = Main.npc[dHead];
                    NPC.netUpdate = true;
                }
            }
            if (!OrbHead.active || OrbHead.type != ModContent.NPCType<AnomalyThree>())
            {
                int oHead = NPC.FindFirstNPC(ModContent.NPCType<AnomalyThree>());
                if (oHead != -1)
                {
                    OrbHead = Main.npc[oHead];
                    NPC.netUpdate = true;
                }
            }
            switch (CurrentPhase)
            {
                case PhaseType.PhaseOne:
                    {
                        if (Timer > 120)
                        {
                            if (!NPC.AnyNPCs(ModContent.NPCType<AnomalyOne>()))
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    DragonHead = NPC.QuickSpawnNPC(ModContent.NPCType<AnomalyOne>());
                                }
                            }
                        }
                        if (DragonHead.active)
                        {
                            if (DragonHead.ai[0] == 3)
                            {
                                ChangePhase(PhaseType.PhaseTwo);
                            }
                        }
                    }
                    break;
                case PhaseType.PhaseTwo:
                    {
                        if (Timer > 120)
                        {
                            if (!NPC.AnyNPCs(ModContent.NPCType<AnomalyThree>()))
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    OrbHead = NPC.QuickSpawnNPC(ModContent.NPCType<AnomalyThree>());
                                }
                            }
                        }
                        if (OrbHead.active)
                        {
                            if (OrbHead.ai[0] == 3)
                            {
                                ChangePhase(PhaseType.Rise);
                            }
                        }
                    }
                    break;
                case PhaseType.Rise:
                    {
                        float startShake = 120;
                        float startRise = startShake + 60;
                        float endRise = startRise + 40;
                        float startFight = endRise + 90;

                        if (Timer >= startShake && Timer < startRise)
                        {
                            Main.LocalPlayer.Calamity().GeneralScreenShakePower = MathHelper.Lerp(1, 10, Utils.GetLerpValue(startShake, startRise, Timer, true));
                        }
                        else if (Timer < endRise)
                        {
                            if (Timer == startRise)
                            {
                                Main.LocalPlayer.Calamity().GeneralScreenShakePower = 30;
                            }
                        }
                        else if (Timer >= startFight)
                        {
                            ChangePhase(PhaseType.IdleBehaviour);
                        }
                    }
                    break;
                case PhaseType.IdleBehaviour:
                    {
                    }
                    break;
                case PhaseType.Enrage:
                    {
                    }
                    break;
                case PhaseType.Desperation:
                    {
                    }
                    break;
            }
            Timer++;
        }

        public void ChangePhase(PhaseType phase)
        {
            CurrentPhase = phase;
            ExtraOne = 0;
            ExtraTwo = 0;
            Timer = 0;
        }


        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[0]);
            writer.Write(NPC.localAI[1]);
            writer.Write(NPC.localAI[2]);
            writer.Write(NPC.Calamity().newAI[0]);
            writer.Write(NPC.Calamity().newAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
            NPC.localAI[1] = reader.ReadSingle();
            NPC.localAI[2] = reader.ReadSingle();
            NPC.Calamity().newAI[0] = reader.ReadSingle();
            NPC.Calamity().newAI[1] = reader.ReadSingle();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Color outlineColor = Color.SeaGreen * NPC.Opacity;
            Vector3 outlineHSL = Main.rgbToHsl(outlineColor);
            float outlineThickness = 2;
            CalamityUtils.EnterShaderRegion(spriteBatch);
            GameShaders.Misc["CalamityMod:BasicTint"].UseOpacity(1f);
            GameShaders.Misc["CalamityMod:BasicTint"].UseColor(Main.hslToRgb(1 - outlineHSL.X, outlineHSL.Y, outlineHSL.Z));
            GameShaders.Misc["CalamityMod:BasicTint"].Apply();
            for (float i = 0; i < 1; i += 0.125f)
            {
                DrawGuy(spriteBatch, screenPos, drawColor, (i * MathHelper.TwoPi + NPC.rotation).ToRotationVector2() * outlineThickness, Color.SeaGreen);
            }
            CalamityUtils.ExitShaderRegion(spriteBatch);
            DrawGuy(spriteBatch, screenPos, drawColor);
            return false;
        }

        public void DrawGuy(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor, Vector2 offset = default, Color overrideColor = default)
        {
            JawRotation = NPC.Center.DirectionTo(Main.MouseWorld).ToRotation();
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D jaw = ModContent.Request<Texture2D>(Texture + "_Jaw").Value;
            Texture2D eye = ModContent.Request<Texture2D>(Texture + "_Eye").Value;
            Texture2D core = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
            Texture2D tongue = ModContent.Request<Texture2D>(Texture + "_Tongue").Value;

            Color norm = overrideColor == default ? NPC.GetAlpha(drawColor) : overrideColor;
            //Color lightColor = overrideColor == default ? Color.White : overrideColor;
            int xOrig = 54;
            int yOrig = 330;
            int xOff = 50;
            int yOff = -60;
            int eyeYOff = -40;
            spriteBatch.Draw(tex, NPC.Center - screenPos + offset, null, norm, 0, new Vector2(tex.Width / 2, tex.Height / 2), NPC.scale, 0, 0);
            spriteBatch.Draw(tongue, NPC.Center - screenPos - Vector2.UnitY * 60, null, Color.White * NPC.Opacity, 0, new Vector2(tongue.Width / 2, tongue.Height), NPC.scale, 0, 0);
            spriteBatch.Draw(jaw, NPC.Center - screenPos + new Vector2(-xOff, yOff) + offset, null, norm, JawRotation, new Vector2(xOrig, yOrig), NPC.scale, 0, 0);
            spriteBatch.Draw(jaw, NPC.Center - screenPos + new Vector2(xOff, yOff) + offset, null, norm, -JawRotation, new Vector2(jaw.Width - xOrig, yOrig), NPC.scale, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(eye, NPC.Center - screenPos + new Vector2(-xOff, yOff) + Vector2.UnitY.RotatedBy(JawRotation) * eyeYOff + offset, eye.Frame(1, 2, 0, 0), Color.White * NPC.Opacity, JawRotation, new Vector2(eye.Width / 2, eye.Height / 4), NPC.scale, 0, 0);
            spriteBatch.Draw(eye, NPC.Center - screenPos + new Vector2(xOff, yOff) + Vector2.UnitY.RotatedBy(-JawRotation) * eyeYOff + offset, eye.Frame(1, 2, 0, 0), Color.White * NPC.Opacity, -JawRotation, new Vector2(eye.Width / 2, eye.Height / 4), NPC.scale, SpriteEffects.FlipHorizontally, 0);

            spriteBatch.Draw(core, NPC.Center - screenPos + offset, null, Color.White * NPC.Opacity, 0, tex.Size() / 2, NPC.scale, 0, 0);
        }
    }
}