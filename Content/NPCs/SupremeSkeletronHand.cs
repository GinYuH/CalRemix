using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CalamityMod.NPCs.SupremeCalamitas;
using System;
using CalamityMod.Projectiles.Boss;
using System.Diagnostics;
using Terraria;
using System.Collections.Generic;
using CalamityMod.DataStructures;

namespace CalRemix.Content.NPCs
{
    public class SupremeSkeletronHand : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float oldPosFromHeadX => ref NPC.ai[0];
        public ref float OldPosFromHeadY => ref NPC.ai[1];
        public ref float LeftOrRight => ref NPC.ai[2];
        public ref float Owner => ref NPC.ai[3];
        public ref float Timer => ref NPC.localAI[0];

        private bool isMouthOpen = false;
        public List<VerletSimulatedSegment> Segments;
        int segmentCount = 26;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eucharist Paratope");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 64;
            NPC.height = 92;
            NPC.lifeMax = 500000;
            NPC.damage = 50;
            NPC.defense = 16;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.boss = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SupremeCalamitas.BrotherHit;
            NPC.DeathSound = SupremeCalamitas.BrotherDeath;
            NPC.dontTakeDamage = true;
            NPC.chaseable = false;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
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
            }
        }
        public override void AI()
        {
            NPC skele = Main.npc[(int)NPC.ai[3]];

            if (skele.type != ModContent.NPCType<SupremeSkeletron>())
            {
                NPC.active = false;
            }

            #region Verlet Stuff
            if (Segments is null)
            {
                Segments = new List<VerletSimulatedSegment>(segmentCount);
                for (int i = 0; i < segmentCount; ++i)
                    Segments[i] = new VerletSimulatedSegment(NPC.Center, false);
            }

            Segments[0].oldPosition = Segments[0].position;

            int distanceX = LeftOrRight == 1 ? 30 : -30;
            float accountForPosNotBeingCenter = Segments[0].position.X ;
            Segments[0].position = new Vector2(skele.Center.X + distanceX, skele.Center.Y + 50);
            //Dust.NewDustPerfect(new Vector2(skele.Center.X + distanceX, skele.Center.Y + 50), DustID.CrimsonSpray, Vector2.Zero);

            Segments = VerletSimulatedSegment.SimpleSimulation(Segments, 9f, loops: segmentCount, gravity: 1f);
            #endregion

            // edit the x coord a bit to compensate for the hand sprite being a bit left-heavy
            // it looks a bit jank in some scenarios, most notably when falling downwards, but so does everything else
            NPC.Center = new Vector2(Segments[Segments.Count - 1].position.X + (distanceX / 10), Segments[Segments.Count - 1].position.Y);

            // update old pos if its been changed, and clear afterwards
            if (oldPosFromHeadX != 0 || OldPosFromHeadY != 0)
            {
                Segments[Segments.Count - 1].oldPosition = new Vector2(oldPosFromHeadX, OldPosFromHeadY);
                OldPosFromHeadY = 0;
                oldPosFromHeadX = 0;
            }

            Timer++;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Texture2D limbs = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/SupremeSkeletronLimbs").Value;

            #region Arms
            // tl;dr each segment is calculated, but only like 2 are rendered
            // its kinda hacky but it works, and thats what matters!
            if (Segments == null || Segments.Count <= 0)
            {
                Segments = new List<VerletSimulatedSegment>(segmentCount);
                for (int i = 0; i < segmentCount; i++)
                {
                    VerletSimulatedSegment segment = new VerletSimulatedSegment(NPC.Center + Vector2.UnitY * 5 * i);
                    Segments.Add(segment);
                }

                Segments[0].locked = true;
                //Segments[Segments.Count - 1].locked = true;
            }
            for (int i = 0; i < Segments.Count; i++)
            {
                VerletSimulatedSegment seg = Segments[i];
                float rot = 0f;
                if (i > 0)
                    rot = seg.position.DirectionTo(Segments[i - 1].position).ToRotation() + MathHelper.PiOver2;
                else
                    rot = NPC.rotation;

                bool draw = true;
                int whichLimb = 1;
                // this segment is forearm
                if (i == 3)
                    whichLimb = 1;
                // this segment is lower arm (does it have a name? i did one google search and couldnt find one)
                else if (i == 13)
                    whichLimb = 0;
                else
                    draw = false;

                SpriteEffects flip = LeftOrRight == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                if (draw)
                {
                    Main.EntitySpriteDraw(limbs, seg.position - Main.screenPosition, limbs.Frame(2, 1, whichLimb, 0), NPC.GetAlpha(Lighting.GetColor(new Point((int)seg.position.X / 16, (int)seg.position.Y / 16))), rot, TextureAssets.Npc[Type].Value.Size() / 3, 1f, flip, 0);
                }

                // this sucks and should be changed
                NPC.rotation = rot;
                if (LeftOrRight == 1)
                    NPC.spriteDirection = 1;
            }
            #endregion

            return true;
        }
    }
}
