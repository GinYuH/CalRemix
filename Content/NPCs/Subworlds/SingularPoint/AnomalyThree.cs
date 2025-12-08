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
using System;

namespace CalRemix.Content.NPCs.Subworlds.SingularPoint
{
    // [AutoloadBossHead]
    public class AnomalyThree : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref float Timer => ref NPC.ai[1];

        public ref float ExtraOne => ref NPC.ai[2];

        public ref float ExtraTwo => ref NPC.ai[3];

        public Vector2 SavePosition
        {
            get => new Vector2(NPC.Calamity().newAI[0], NPC.Calamity().newAI[1]);
            set
            {
                NPC.Calamity().newAI[0] = value.X;
                NPC.Calamity().newAI[1] = value.Y;
            }
        }

        public ref float EyeRotation => ref NPC.Calamity().newAI[2];

        public NPC MainHead
        {
            get => Main.npc[(int)NPC.localAI[0]];
            set => NPC.localAI[0] = value.whoAmI;
        }

        public NPC DragonHead
        {
            get => Main.npc[(int)NPC.localAI[1]];
            set => NPC.localAI[1] = value.whoAmI;
        }

        public bool PhaseTwo
        {
            get => NPC.localAI[2] == 1;
            set => NPC.localAI[2] = value.ToInt();
        }

        public enum PhaseType
        {
            SpawnAnim = 0,
            BouncyBalls = 1,
            Orbitals = 2,
            Knockout = 3
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
            NPC.lifeMax = 300000;
            NPC.damage = 340;
            NPC.defense = 100;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.HitSound = null;
            NPC.DeathSound = null;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 0);
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
            if (!MainHead.active || MainHead.type != ModContent.NPCType<AnomalyTwo>())
            {
                int dHead = NPC.FindFirstNPC(ModContent.NPCType<AnomalyTwo>());
                if (dHead != -1)
                {
                    MainHead = Main.npc[dHead];
                    NPC.netUpdate = true;
                }
            }
            if (!DragonHead.active || DragonHead.type != ModContent.NPCType<AnomalyOne>())
            {
                int oHead = NPC.FindFirstNPC(ModContent.NPCType<AnomalyOne>());
                if (oHead != -1)
                {
                    DragonHead = Main.npc[oHead];
                    NPC.netUpdate = true;
                }
            }
            float phaseGate = 0.7f;
            if (!PhaseTwo && NPC.life < (NPC.lifeMax * phaseGate))
            {
                ChangePhase(PhaseType.Knockout);
            }
            switch (CurrentPhase)
            {
                case PhaseType.SpawnAnim:
                    {
                    }
                    break;
                case PhaseType.BouncyBalls:
                    {
                    }
                    break;
                case PhaseType.Orbitals:
                    {
                    }
                    break;
                case PhaseType.Knockout:
                    {
                        NPC.dontTakeDamage = true;
                        NPC.velocity = Vector2.Zero;
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
            writer.Write(NPC.Calamity().newAI[2]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
            NPC.localAI[1] = reader.ReadSingle();
            NPC.localAI[2] = reader.ReadSingle();
            NPC.Calamity().newAI[0] = reader.ReadSingle();
            NPC.Calamity().newAI[1] = reader.ReadSingle();
            NPC.Calamity().newAI[2] = reader.ReadSingle();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D chain = ModContent.Request<Texture2D>(Texture + "_Segment").Value;
            if (MainHead.active)
            {
                if (MainHead.type == ModContent.NPCType<AnomalyTwo>())
                {
                    int chainAmt = 18;

                    for (int i = 0; i < chainAmt; i++)
                    {
                        Vector2 pos = Vector2.Lerp(NPC.Center, MainHead.Bottom, i / (float)chainAmt);
                        Main.EntitySpriteDraw(chain, pos - screenPos, null, Color.White, 0, chain.Size() / 2, NPC.scale, 0);
                    }
                }
            }
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
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D eye = ModContent.Request<Texture2D>(Texture + "_Eye").Value;
            Color norm = overrideColor == default ? NPC.GetAlpha(drawColor) : overrideColor;
            spriteBatch.Draw(tex, NPC.Center - screenPos + offset, null, norm, NPC.rotation, tex.Size() / 2, NPC.scale, 0, 0);
            for (int i = 0; i < 3; i++)
            {
                Vector2 pos = NPC.Center - Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / 3f)).RotatedBy(EyeRotation) * 50;
                spriteBatch.Draw(eye, pos - screenPos, null, Color.White, NPC.rotation, eye.Size() / 2, NPC.scale, 0, 0);
            }
        }
    }
}