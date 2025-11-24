using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Core.Biomes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod;
using Terraria.Audio;

namespace CalRemix.Content.NPCs.Subworlds.Nowhere
{
    public class Nothing : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 40;
            NPC.width = 60;
            NPC.height = 60;
            NPC.defense = 27;
            NPC.lifeMax = 10000;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0f;
            NPC.HitSound = ScornEater.HitSound with { Pitch = -2 };
            NPC.DeathSound = ScornEater.DeathSound with { Pitch = 2 };
            SpawnModBiomes = new int[1] { ModContent.GetInstance<NowhereBiome>().Type };
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D normal = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Nowhere/Noone").Value;

            Rectangle rectSize = new Rectangle(0, 0, (int)normal.Width, (int)normal.Height);

            Rectangle randomRect = new Rectangle(Main.rand.Next(0, normal.Width * 2), Main.rand.Next(0, normal.Height * 2), rectSize.Width, rectSize.Height);

            float shakeMult = MathHelper.Lerp(5, 50, Utils.GetLerpValue(NPC.lifeMax, 0, NPC.life, true));
            spriteBatch.Draw(tex, NPC.Center - screenPos + Main.rand.NextVector2Circular(shakeMult, shakeMult), randomRect, Color.White, NPC.rotation, normal.Size() / 2, new Vector2(randomRect.Height / (float)randomRect.Width, 1), SpriteEffects.None, 0);
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
