using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System.Collections.Generic;
using System;
using CalamityMod.Graphics.Primitives;

namespace CalRemix.Content.NPCs
{
    public class CarrierHead : ModNPC
    {
        public ref float Timer => ref NPC.ai[1];

        List<(Vector2, float)> topTentacles = new List<(Vector2, float)>();
        List<(Vector2, float)> bottomTentacles = new List<(Vector2, float)>();


        public Vector2 spawnPos
        {
            get
            {
                return new Vector2(NPC.ai[2], NPC.ai[3]);
            }
            set
            {
                NPC.ai[2] = value.X;
                NPC.ai[3] = value.Y;
            }
        }

        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 40;
            NPC.height = 40;
            NPC.defense = 50054744;
            NPC.lifeMax = 5;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0;
            NPC.HitSound = SoundID.NPCHit9 with { Pitch = 0.4f };
            NPC.DeathSound = SoundID.NPCDeath12 with { Pitch = -0.8f };
            NPC.GravityIgnoresLiquid = true;
            NPC.npcSlots = 0;
            NPC.dontCountMe = true;
            NPC.waterMovementSpeed = 1f;
            NPC.dontTakeDamage = true;
            NPC.noGravity = true;
        }

        public override void AI()
        {
            if (spawnPos == default)
            {
                spawnPos = NPC.Center;
                int totent = Main.rand.Next(8, 16);
                int lengthMin = 180;
                int lengthMax = 260;
                for (int i = 0; i < totent; i++)
                {
                    int length = Main.rand.Next(lengthMin, lengthMax);
                    topTentacles.Add((-Vector2.UnitX * (MathHelper.Lerp(-40, 40, i / (float)totent) + Main.rand.Next(-10, 10)), length));
                }
                totent = Main.rand.Next(15, 28);
                lengthMin = 400;
                lengthMax = 560;
                for (int i = 0; i < totent; i++)
                {
                    int length = Main.rand.Next(lengthMin, lengthMax);
                    bottomTentacles.Add((-Vector2.UnitX * (MathHelper.Lerp(-40, 40, i / (float)totent) + Main.rand.Next(-10, 10)), length));
                }
            }
            bool despawn = true;
            Player p = Main.player[(int)NPC.ai[0]];
            if (despawn)
            {
                if (!p.ItemAnimationActive)
                {
                    NPC.active = false;
                    return;
                }
                if (p.Distance(NPC.Center) > 3300)
                {
                    NPC.active = false;
                    return;
                }
            }
            float maxComp = 0.9f;
            Vector2 pos = Vector2.Lerp(spawnPos, p.Center, MathHelper.Clamp(1 - ((p.itemAnimation / (float)p.itemAnimationMax) * maxComp), 0, maxComp) + 0.4f);
            NPC.velocity = pos - NPC.position;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            float angleRange = MathHelper.PiOver4 * 0.3f;
            spriteBatch.EnterShaderRegion(BlendState.AlphaBlend);
            Texture2D tex = TextureAssets.Npc[Type].Value;
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 scale = new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * 4) * 0.022f, MathF.Cos(Main.GlobalTimeWrappedHourly * 4) * 0.022f);
            Texture2D bod = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/CarrierBody").Value;
            spriteBatch.Draw(bod, NPC.Center - screenPos + Vector2.UnitY * 30, null, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(bod.Width / 2, 0), NPC.scale * Vector2.One + scale, fx, 0);
            float widthOffset = 0;
            for (int i = 0; i < topTentacles.Count; i++)
            {
                var v = topTentacles[i];
                int segCount = (int)(v.Item2 * 2f);
                Vector2 start = NPC.Center + v.Item1 + Vector2.UnitY * widthOffset;
                List<Vector2> points = new();
                for (int j = 0; j < segCount; j++)
                {
                    points.Add(start + new Vector2(MathF.Sin((float)j * 0.05f - Main.GlobalTimeWrappedHourly * (i % 5 * 0.5f + 1) * 2 + i * 2f) * 15f, MathHelper.Lerp(0, -v.Item2, (float)j / (float)(segCount - 1))).RotatedBy(MathHelper.Lerp(-angleRange, angleRange, i % 3 / 3f)));
                }
                PrimitiveRenderer.RenderTrail(points, new PrimitiveSettings((float c, Vector2 v) => i % 3 + 1, (float c, Vector2 v) => Color.Lerp(Color.Black, Color.Black * 0, CalamityUtils.CircInEasing(c, 1))));
            }
            for (int i = 0; i < bottomTentacles.Count; i++)
            {
                var v = bottomTentacles[i];
                int segCount = (int)(v.Item2 * 2f);
                Vector2 start = NPC.Center + v.Item1 + Vector2.UnitY * widthOffset;
                List<Vector2> points = new();
                for (int j = 0; j < segCount; j += 10)
                {
                    points.Add(start + new Vector2(MathF.Sin((float)j * 0.05f - Main.GlobalTimeWrappedHourly * (i % 5 * 0.5f + 1) * 2 + i * 2f) * 5f, MathHelper.Lerp(0, v.Item2, (float)j / (float)(segCount - 1))).RotatedBy(MathHelper.Lerp(-angleRange, angleRange, i % 10 / 10f)));
                }
                PrimitiveRenderer.RenderTrail(points, new PrimitiveSettings((float c, Vector2 v) => i % 3 + 1, (float c, Vector2 v) => Color.Lerp(Color.Black, Color.Black * 0, CalamityUtils.CircInEasing(c, 1))));
            }
            spriteBatch.ExitShaderRegion();
            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / Main.npcFrameCount[Type] / 2), NPC.scale * Vector2.One + scale, fx, 0);
            spriteBatch.Draw(ModContent.Request<Texture2D>("CalRemix/Content/NPCs/CarrierGlow").Value, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, new Vector2(tex.Width / 2, tex.Height / Main.npcFrameCount[Type] / 2), NPC.scale * Vector2.One + scale, fx, 0);
            return false;
        }
    }
}
