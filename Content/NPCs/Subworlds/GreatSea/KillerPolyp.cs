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
using CalamityMod.Projectiles.Boss;
using Terraria.Audio;
using System;
using CalamityMod.Graphics.Primitives;
using CalRemix.Core.Biomes;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class KillerPolyp : ModNPC
    {
        public ref float Timer => ref NPC.ai[1];

        List<(Vector2, float)> tentacles = new List<(Vector2, float)>();

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 114;
            NPC.height = 72;
            NPC.defense = 50;
            NPC.lifeMax = 20000;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0;
            NPC.HitSound = SoundID.NPCHit9 with { Pitch = 0.4f };
            NPC.DeathSound = SoundID.NPCDeath12 with { Pitch = -0.8f };
            NPC.GravityIgnoresLiquid = true;
            NPC.npcSlots = 0;
            NPC.dontCountMe = true;
            NPC.waterMovementSpeed = 1f;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<GrandSeaBiome>().Type };
        }

        public override void AI()
        {
            NPC.velocity.Y = 12;
            if (NPC.ai[0] == 0)
            {
                NPC.ai[0] = 1;
                int totent = Main.rand.Next(8, 16);
                int lengthMin = 100;
                int lengthMax = 180;
                for (int i = 0; i < totent; i++)
                {
                    int length = Main.rand.Next(lengthMin, lengthMax);
                    tentacles.Add((-Vector2.UnitX * (MathHelper.Lerp(-30, 30, i / (float)totent) + Main.rand.Next(-10, 10)), length));
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.EnterShaderRegion(BlendState.NonPremultiplied);
            Texture2D tex = TextureAssets.Npc[Type].Value;
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 scale = new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * 4) * 0.022f, MathF.Cos(Main.GlobalTimeWrappedHourly * 4) * 0.022f);
            float widthOffset = 0;
            for (int i = 0; i < tentacles.Count; i++)
            {
                var v = tentacles[i];
                int segCount = (int)(v.Item2 * 2f);
                Vector2 start = NPC.Center + v.Item1 + Vector2.UnitY * widthOffset;
                List<Vector2> points = new();
                for (int j = 0; j < segCount; j++)
                {
                    points.Add(start + new Vector2(MathF.Sin((float)j * 0.05f - Main.GlobalTimeWrappedHourly * (i % 5 * 0.5f + 1) * 2 + i * 2f) * 5f, MathHelper.Lerp(0, -v.Item2, (float)j / (float)(segCount - 1))));
                }
                float dt = Color.DarkTurquoise.G;
                float lt = Color.Turquoise.G;
                int colRange = 22;
                int colorVariety = 22;
                PrimitiveRenderer.RenderTrail(points, new PrimitiveSettings((float c) => 4, (float c) => Color.Lerp(Color.DarkTurquoise with { G = (byte)MathHelper.Lerp(dt - colRange, dt + colRange, i % colorVariety / (float)(colorVariety - 1)) }, Color.Turquoise with { G = (byte)MathHelper.Lerp(lt - colRange, lt + colRange, i % colorVariety / (float)(colorVariety - 1)) }, c))); ;
            }
            spriteBatch.ExitShaderRegion();
            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / Main.npcFrameCount[Type] / 2), NPC.scale * Vector2.One + scale, fx, 0);
            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
    }
}
