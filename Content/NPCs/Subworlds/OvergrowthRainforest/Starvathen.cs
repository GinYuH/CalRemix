using CalamityMod;
using CalamityMod.BiomeManagers;
using CalamityMod.Graphics.Primitives;
using CalRemix.Content.Items.Potions;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Core.Biomes.Subworlds;
using CalRemix.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.UI;
using static Terraria.GameContent.Animations.IL_Actions.Sprites;

namespace CalRemix.Content.NPCs.Subworlds.OvergrowthRainforest
{
    public class Starvathen : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.aiStyle = NPCAIStyleID.Unicorn;
            NPC.damage = 240;
            NPC.width = 32;
            NPC.height = 60;
            NPC.defense = 18;
            NPC.lifeMax = 10000;
            NPC.knockBackResist = 0.5f;
            NPC.value = Item.buyPrice(gold: 1, silver: 2);
            NPC.noGravity = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = true;
            SpawnModBiomes = [ModContent.GetInstance<OvergrowthRainforestBiome>().Type];
        }

        public override void AI()
        {
            NPC.spriteDirection = NPC.velocity.X.DirectionalSign();
            if (NPC.velocity.X != 0 && NPC.velocity.Y == 0)
                NPC.Calamity().newAI[0] += 0.1f * NPC.velocity.X;

            float maxRot = MathHelper.ToRadians(50);
            NPC.rotation = MathHelper.Lerp(-maxRot, maxRot, Utils.GetLerpValue(-6, 6, NPC.velocity.X, true));

            foreach (Player p in Main.ActivePlayers)
            {
                if (p.getRect().Intersects(NPC.getRect()) && !p.dead && !p.HasIFrames())
                {
                    p.KillMe(PlayerDeathReason.ByNPC(NPC.whoAmI), 10000, 1);
                    SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/StarvathenShrill"));
                    NPC.Remix().GreenAI[0] = 0.01f;
                }
            }

            if (NPC.Remix().GreenAI[0] > 0)
            {
                NPC.Remix().GreenAI[0] += 0.05f;
                if (NPC.Remix().GreenAI[0] > 4)
                    NPC.active = false;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D legProximal = ModContent.Request<Texture2D>(Texture + "_Leg").Value;
            Texture2D legDistal = ModContent.Request<Texture2D>(Texture + "_Leg2").Value;


            Vector2 pos = NPC.Center - screenPos;
            Vector2 scale = new Vector2(NPC.scale);

            if (NPC.Remix().GreenAI[0] > 0)
            {
                float maxScale = Main.screenWidth / tex.Width;

                scale = new Vector2(MathHelper.Lerp(1, maxScale, Utils.GetLerpValue(0, 1, NPC.Remix().GreenAI[0], false)));
                pos = Main.ScreenSize.ToVector2() / 2;
                NPC.rotation = 0;
                Main.LocalPlayer.Calamity().GeneralScreenShakePower += 2;
            }


            spriteBatch.Draw(legDistal, pos + new Vector2(NPC.spriteDirection * 8, 8) * scale, null, drawColor, NPC.rotation + MathF.Sin(NPC.Calamity().newAI[0]) * 0.3f, new Vector2(NPC.spriteDirection == 1 ? legDistal.Width - 6 : 6, 2), scale, NPC.FlippedEffects(), 0f);

            spriteBatch.Draw(tex, pos, null, drawColor, NPC.rotation, new Vector2(15, 51), scale, NPC.FlippedEffects(), 0f);

            spriteBatch.Draw(legProximal, pos + new Vector2(NPC.spriteDirection * -8, 0) * scale, null, drawColor, NPC.rotation - MathF.Sin(NPC.Calamity().newAI[0]) * 0.3f, new Vector2(NPC.spriteDirection == 1 ? legProximal.Width - 10 : 10, 4), scale, NPC.FlippedEffects(), 0f);

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