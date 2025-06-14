using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System.Collections.Generic;
using CalamityMod.DataStructures;
using Terraria.GameContent.Animations;
using CalRemix.Core.World;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class XiphactinusHead : ModNPC
    {
        public int BodyIndex => (int)NPC.ai[0] - 1;

        public ref float Timer => ref NPC.ai[1];

        public NPC Body => Main.npc[BodyIndex];

        List<VerletSimulatedSegment> Segments = new List<VerletSimulatedSegment>();

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 70;
            NPC.width = 70;
            NPC.height = 40;
            NPC.defense = 0;
            NPC.lifeMax = 100;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.dontTakeDamage = true;
            NPC.knockBackResist = 0;
            NPC.HitSound = SoundID.NPCHit50 with { Pitch = -0.4f };
            NPC.DeathSound = SoundID.NPCDeath40;
            NPC.GravityIgnoresLiquid = true;
            NPC.npcSlots = 0;
            NPC.dontCountMe = true;
            NPC.waterMovementSpeed = 1f;
        }

        public override void AI()
        {
            NPC.timeLeft = 1000000;
            if (BodyIndex != -1)
            {
                if (!Body.active || Body.life <= 0 || Body.type != ModContent.NPCType<Xiphactinus>())
                {
                    NPC.active = false;
                    return;
                }
            }
            else
            {
                NPC.active = false;
                return;
            }

            int segCount = 20;
            Segments = CalRemixHelper.CreateVerletChain(ref Segments, segCount, NPC.Center, new List<int>() { 0, segCount - 1 } );

            Segments[0].oldPosition = Segments[0].position;
            Segments[0].position = NPC.Center;

            Segments[Segments.Count - 1].oldPosition = Segments[Segments.Count - 1].position;
            Segments[Segments.Count - 1].position = Body.Center;

            Segments = VerletSimulatedSegment.SimpleSimulation(Segments, 2, loops: 10, gravity: 22f);
        

            NPC.netUpdate = true;
            NPC.netSpam = 0;

            Timer++;
            if (Timer >= 20)
            {
                NPC.velocity *= 0.95f;
                Body.ai[1] = 3;
                NPC.spriteDirection = Body.Center.X < NPC.Center.X ? -1 : 1;
                NPC.rotation = (NPC.DirectionTo(Body.Center).ToRotation() + (NPC.spriteDirection == -1 ? 0 : MathHelper.Pi));
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 6)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 2)
            {
                NPC.frame.Y = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (BodyIndex > -1)
            {
                Texture2D tex = TextureAssets.Npc[Type].Value;
                Texture2D chain = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/XiphactinusChain").Value;
                // Grab the position at which Hydrogen's chain should connect to
                Vector2 bottom = Body.Center;
                Vector2 distToProj = NPC.Center;
                float projRotation = NPC.AngleTo(bottom) - 1.57f;
                bool doIDraw = true;

                while (doIDraw)
                {
                    float distance = (bottom - distToProj).Length();
                    if (distance < (chain.Height + 1))
                    {
                        doIDraw = false;
                    }
                    else if (!float.IsNaN(distance))
                    {
                        Color drawColore = Lighting.GetColor((int)distToProj.X / 16, (int)(distToProj.Y / 16f));
                        distToProj += NPC.DirectionTo(bottom) * chain.Height;
                        Main.EntitySpriteDraw(chain, distToProj - Main.screenPosition,
                            new Rectangle(0, 0, chain.Width, chain.Height), drawColore, projRotation,
                            Utils.Size(chain) / 2f, 1f, SpriteEffects.None, 0);
                    }
                }
                SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / Main.npcFrameCount[Type] / 2), NPC.scale, fx, 0);
            }
            return false;
        }
    }
}
