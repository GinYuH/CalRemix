using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalRemix.Content.Items.Placeables;
using CalRemix.Core.Biomes;
using CalamityMod.BiomeManagers;
using CalRemix.Content.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalRemix.Content.NPCs.Bosses.Origen;
using Newtonsoft.Json.Linq;
using Terraria.Audio;
using Terraria.GameContent;
using System;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class Crustiment : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 40;
            NPC.width = 60;
            NPC.height = 60;
            NPC.defense = 7;
            NPC.lifeMax = 1000;
            NPC.value = 2000;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath41;
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.velocity.ToRotation() - MathHelper.PiOver2, 0.1f);

            Timer++;
            NPC.ai[1]--;
            if ((Timer % 150 == 0 && NPC.ai[1] <= 00) || Math.Abs(NPC.velocity.X) < 1 || Math.Abs(NPC.velocity.Y) < 1)
            {
                NPC.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(3, 5);
            }

            if (NPC.justHit)
            {
                NPC.velocity = Main.player[NPC.target].DirectionTo(NPC.Center) * 10;
                NPC.ai[1] = 60;
                SoundEngine.PlaySound(BetterSoundID.ItemPoopSquish with { Pitch = 0.8f }, NPC.Center);
            }

            if (NPC.ai[1] > 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    Dust d = Dust.NewDustPerfect(NPC.Center, DustID.Blood, -NPC.velocity.SafeNormalize(Vector2.UnitY) * new Vector2(Main.rand.Next(-4, 4), Main.rand.Next(10, 18)), Scale: Main.rand.NextFloat(1.5f, 3f));
                    d.noGravity = true;
                }
                if (NPC.ai[1] % 5 == 0)
                {
                    SoundEngine.PlaySound(BetterSoundID.ItemPoopSquish with { MaxInstances = 0, Pitch = MathHelper.Lerp(0.6f, 0, Utils.GetLerpValue(60, 0, NPC.ai[1], true)) }, NPC.Center);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            float x = 1;
            float y = 1;
            float xSquash = 1.2f;
            float xStretch = 0.8f;
            if (NPC.ai[1] > 40)
            {
                x = MathHelper.Lerp(1, xSquash, Utils.GetLerpValue(60, 0, NPC.ai[1], true));
                y = MathHelper.Lerp(1f, 0.4f, Utils.GetLerpValue(60, 0, NPC.ai[1], true));
            }
            else if (NPC.ai[1] > 0)
            {
                x = MathHelper.Lerp(xSquash, xStretch, Utils.GetLerpValue(60, 0, NPC.ai[1], true));
                y = MathHelper.Lerp(0.4f, 1f, Utils.GetLerpValue(60, 0, NPC.ai[1], true));
            }
            else
            {
                x = MathHelper.Lerp(xStretch, 1f, Utils.GetLerpValue(0, -20, NPC.ai[1], true));
                y = 1;
            }
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, NPC.GetAlpha(drawColor), NPC.rotation, tex.Size() / 2, new Vector2(x, y) * NPC.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.Fries, 1);
        }
    }
}
