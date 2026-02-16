using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalRemix.Core.Biomes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class TheShoalless : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 60;
            NPC.height = 60;
            NPC.defense = 0;
            NPC.lifeMax = 2000;
            NPC.value = 2;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.knockBackResist = 0;
            NPC.DeathSound = SoundID.NPCDeath39 with { Pitch = -0.2f };
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PrimordialCavesBiome>().Type };
        }

        public override void AI()
        {
            if (!NPC.wet)
            {
                NPC.velocity.Y = 12;
                return;
            }
            if (NPC.direction == 0)
            {
                NPC.direction = (int)Main.rand.NextFloatDirection();
            }
            if ((Timer % 120 == 0 && Main.rand.NextBool(5)) || Math.Abs(NPC.velocity.X) < 1)
            {
                NPC.direction *= -1;
                NPC.velocity.X = 3 * NPC.direction;
            }
            Timer++;
            NPC.spriteDirection = NPC.direction = NPC.velocity.X.DirectionalSign();
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 6)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 3)
            {
                NPC.frame.Y = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Vector2 position = NPC.Center - screenPos;
            Vector2 origin = new Vector2(tex.Width / 2, tex.Height / 6);
            Color color = NPC.GetAlpha(Color.Magenta * 0.6f);
            Vector2 scale = Vector2.One;
            Vector2 sin = Vector2.UnitY * MathF.Sin(Main.GlobalTimeWrappedHourly) * 3;
            SpriteEffects fx = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            position += sin;
            for (int i = 0; i < 10; i++)
            {
                Vector2 vector2 = (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() + (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() * 2 * Math.Abs((float)Math.Sin(Main.GlobalTimeWrappedHourly));
                Main.spriteBatch.Draw(tex, position + vector2, NPC.frame, color, NPC.rotation, origin, scale, fx, 0f);
            }
            spriteBatch.Draw(tex, position, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, origin, NPC.scale, fx, 0);
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
