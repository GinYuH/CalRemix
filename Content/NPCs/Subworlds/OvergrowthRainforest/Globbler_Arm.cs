using CalamityMod;
using CalamityMod.DataStructures;
using CalamityMod.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Subworlds.OvergrowthRainforest
{
    public class Globbler_Arm : ModNPC
    {
        public NPC dad => Main.npc[(int)NPC.ai[0]];
        public Vector2 dest
        {
            get => new Vector2(NPC.ai[1], NPC.ai[2]);
            set
            { 
                NPC.ai[1] = value.X;
                NPC.ai[2] = value.Y;
            }
        }

        public bool Swinging = false;
        public bool Latched => NPC.ai[3] == 2;

        public List<VerletSimulatedSegment> segments;
        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
        }
        public override void SetDefaults()
        {
            NPC.dontTakeDamage = true;
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 10;
            NPC.height = 10;
            NPC.defense = 0;
            NPC.knockBackResist = 0;
            NPC.lifeMax = 5;
            NPC.value = 0;
            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = true;
        }

        public override void AI()
        {
            if (!dad.active || dad.type != ModContent.NPCType<Globbler>())
            {
                NPC.active = false;
                return;
            }
            CalRemixHelper.CreateVerletChain(ref segments, 20, dad.Center, [0]);

            if (NPC.ai[3] == 1)
            {
                if (NPC.Center.Y < (dad.Center.Y - 16) && Collision.SolidTiles(NPC.position, NPC.width, NPC.height))
                {
                    NPC.velocity = Vector2.Zero;
                    NPC.ai[3] = 2;
                }
                if (NPC.localAI[2] <= 0)
                {
                    NPC.ai[3] = 0;
                    Swinging = false;
                }
            }
            if (Latched)
            {
                NPC.velocity = Vector2.Zero;
            }

            if (segments.Count > 0)
            {
                if (Swinging)
                {
                    segments[0].locked = false;
                    segments[^1].locked = true;
                    segments[^1].oldPosition = segments[^1].position;
                    segments[^1].position = NPC.Center;
                }
                else
                {
                    segments[0].locked = true;
                    segments[0].oldPosition = segments[0].position;
                    segments[0].position = dad.Center;
                    segments[^1].locked = false;
                    if (Latched)
                    NPC.Center = segments[^1].position;
                }

                VerletSimulatedSegment.SimpleSimulation(segments, 10, 30, 1);
            }
            NPC.localAI[2]--;
        }

        public void Launch()
        {
            int dir = dad.velocity.X.DirectionalSign();
            if (Main.rand.NextBool(5))
                dir *= -1;

            NPC.velocity = -Vector2.UnitY.RotatedByRandom(MathHelper.PiOver4) * 10;
            NPC.ai[3] = 1;
            NPC.localAI[2] = 90;
            Swinging = true;
        }

        public void Release()
        {
            NPC.ai[3] = 0;
            Swinging = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Swinging);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Swinging = reader.ReadBoolean();
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.ExitShaderRegion();
            if (segments.Count > 0)
            {
                List<Vector2> pts = new();
                for (int i = 0; i < segments.Count; i++)
                {
                    pts.Add(segments[i].position);
                }
                PrimitiveRenderer.RenderTrail(pts, new PrimitiveSettings(new PrimitiveSettings.VertexWidthFunction((float f, Vector2 v) => 4), new PrimitiveSettings.VertexColorFunction((float f, Vector2 v) => Color.SeaGreen)));

                Texture2D arm = ModContent.Request<Texture2D>(Texture).Value;
                spriteBatch.Draw(arm, segments[^1].position - Main.screenPosition, null, Lighting.GetColor(segments[^1].position.ToTileCoordinates()), segments[^1].position.DirectionTo(dad.Center).ToRotation(), arm.Size() / 2, NPC.scale, 0, 0);
            }
            return false;
        }
    }
}