using CalamityMod;
using CalamityMod.DataStructures;
using CalamityMod.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.PandemicPanic
{
    public class DendtritiatorArm : ModNPC
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public List<VerletSimulatedSegment> Segments;
        int segmentCount = 30;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dendritiator");
            NPCID.Sets.MustAlwaysDraw[Type] = true;
            this.HideFromBestiary();
        }
        public override void SetDefaults()
        {
            NPC.damage = 60;
            NPC.width = 48;
            NPC.height = 54;
            NPC.defense = 20;
            NPC.lifeMax = 10000;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.dontTakeDamage = true;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath8;
            NPC.aiStyle = -1;
            NPC.netAlways = true;
            AIType = -1;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            foreach (var v in Segments)
            {
                writer.Write(v.position.X);
                writer.Write(v.position.Y);
                writer.Write(v.locked);
                writer.Write(v.oldPosition.X);
                writer.Write(v.oldPosition.Y);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            List<VerletSimulatedSegment> segs = new List<VerletSimulatedSegment>();
            for (int i = 0; i < 30; i++)
            {
                VerletSimulatedSegment v = new VerletSimulatedSegment(new Vector2(reader.ReadSingle(), reader.ReadSingle()), reader.ReadBoolean());
                v.oldPosition = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                segs.Add(v);
            }
            Segments = segs;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Segments == null || Segments.Count < segmentCount)
            {
                Segments = new List<VerletSimulatedSegment>(segmentCount);
                for (int i = 0; i < segmentCount; i++)
                {
                    VerletSimulatedSegment segment = new VerletSimulatedSegment(NPC.Center + Vector2.UnitY * 5 * i);
                    Segments.Add(segment);
                }

                Segments[0].locked = true;
                Segments[^1].locked = true;
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (Segments != null)
            for (int i = segmentCount - 1; i > 5; i--)
            {
                if (target.getRect().Intersects(new Rectangle((int)Segments[i].position.X, (int)Segments[i].position.Y, 10, 10)))
                {
                    target.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), NPC.damage, 1);
                }
            }
            return false;
        }

        public override void AI()
        {
            NPC phyto = Main.npc[(int)NPC.ai[0]];
            if (phyto == null || !phyto.active || phyto.type != ModContent.NPCType<Dendritiator>())
            {
                NPC.active = false;
                return;
            }
            Entity targ = phyto.ModNPC<Dendritiator>().targeto;
            if (Segments is null)
            {
                Segments = new List<VerletSimulatedSegment>(segmentCount);
                for (int i = 0; i < segmentCount; ++i)
                    Segments[i] = new VerletSimulatedSegment(NPC.Center, false);
            }

            Segments[0].oldPosition = Segments[0].position;
            Segments[0].position = NPC.Center;

            Segments[Segments.Count - 1].oldPosition = Segments[Segments.Count - 1].position;
            Segments[Segments.Count - 1].position = phyto.Center;

            Segments = VerletSimulatedSegment.SimpleSimulation(Segments, 2, loops: segmentCount, gravity: NPC.whoAmI % 2 == 0 ? 22f : -22f);

            NPC.netUpdate = true;
            NPC.netSpam = 0;

            NPC.rotation = NPC.DirectionTo(phyto.Center).ToRotation() - MathHelper.PiOver2;

            int maxDist = 900;
            Vector2 gotoe = (targ != null && targ.active && !(targ is NPC n && n.life <= 0)) && Main.rand.NextBool(5) ? targ.Center : phyto.Center;
            NPC.ai[2]++;
            if (NPC.ai[2] >= NPC.ai[3] && NPC.velocity == Vector2.Zero)
            {
                NPC.velocity = NPC.DirectionTo(gotoe) * 16;
                NPC.ai[2] = 0;
                NPC.ai[3] = Main.rand.Next(120, 240);
                NPC.netUpdate = true;
            }

            if (Collision.IsWorldPointSolid(NPC.Center) || Main.tile[(int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16)].WallType > 0 || NPC.Distance(phyto.Center) > maxDist)
            {
                if (NPC.ai[2] > 60)
                {
                    NPC.velocity = Vector2.Zero;
                    NPC.netUpdate = true;
                }
                else if (Collision.IsWorldPointSolid(NPC.Center))
                {
                    NPC.velocity = NPC.DirectionTo(gotoe) * 16;
                    NPC.netUpdate = true;
                }
            }
            if (NPC.Distance(NPC.Center) > maxDist)
            {
                NPC.velocity = NPC.DirectionTo(NPC.Center) * 16;
                NPC.netUpdate = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC phyto = Main.npc[(int)NPC.ai[0]];
            if (phyto == null || !phyto.active || phyto.type != ModContent.NPCType<Dendritiator>())
            {
                return false;
            }
            if (Segments == null || Segments.Count <= 0)
            {
                Segments = new List<VerletSimulatedSegment>(segmentCount);
                for (int i = 0; i < segmentCount; i++)
                {
                    VerletSimulatedSegment segment = new VerletSimulatedSegment(NPC.Center + Vector2.UnitY * 5 * i);
                    Segments.Add(segment);
                }

                Segments[0].locked = true;
                Segments[Segments.Count - 1].locked = true;
            }
            List<Vector2> vecList = new List<Vector2>();
            for (int i = Segments.Count - 1; i >= 0; i--)
            {
                vecList.Add(Segments[i].position);
            }
            Main.spriteBatch.EnterShaderRegion();

            // Prepare the flame trail shader with its map texture.
            GameShaders.Misc["CalamityMod:ImpFlameTrail"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/ScarletDevilStreak"));
            Vector2 trailOffset = NPC.Size * 0.5f;
            trailOffset += (NPC.rotation + MathHelper.PiOver2).ToRotationVector2();
            PrimitiveRenderer.RenderTrail(vecList, new(FlameTrailWidthFunction, FlameTrailColorFunction, (_, _) => Vector2.Zero, shader: GameShaders.Misc["CalamityMod:ImpFlameTrail"]), 17);

            Main.spriteBatch.ExitShaderRegion();
            
            return false;
        }

        public override bool CheckActive()
        {
            return !NPC.AnyNPCs(ModContent.NPCType<Dendritiator>());
        }


        public float FlameTrailWidthFunction(float completionRatio, Vector2 v) => MathHelper.Lerp(60, 1, completionRatio);

        public Color FlameTrailColorFunction(float completionRatio, Vector2 v)
        {
            Color color = Color.Violet;
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>() != null)
            {
                if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().phd)
                {
                    color = Color.Lime;
                }
            }
            return Color.Lerp(color, default, completionRatio);
        }
    }
}