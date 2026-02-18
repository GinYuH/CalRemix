using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.GameContent.ItemDropRules;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes
{
    public class FabergeEgg : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.Quest;
            Item.useAnimation = 30;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Cryonix.Cryonix>()) || NPC.AnyNPCs(ModContent.NPCType<Vernix.Vernix>()) || NPC.AnyNPCs(ModContent.NPCType<Chaotrix.Chaotrix>()))
                return false;

            if (WhatFreakingBiomeAmIInAnyways() != 0)
                return true;

            return false;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                int biome = WhatFreakingBiomeAmIInAnyways();
                if (biome != 0)
                {
                    int type = 0;
                    switch (biome)
                    {
                        case 1:
                            type = ModContent.NPCType<Cryonix.Cryonix>();
                            break;
                        case 2:
                            type = ModContent.NPCType<Vernix.Vernix>();
                            break;
                        case 3:
                            type = ModContent.NPCType<Chaotrix.Chaotrix>();
                            break;
                    }
                    SoundEngine.PlaySound(SoundID.Roar, player.position);
                    CalRemixHelper.SpawnClientBossRandomPos(type, player.Center);
                }
            }
            return true;
        }

        private float currentGlowFade = 0;
        private float GlowFadeMax = 100;
        private Color biomeColor = new Color();
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D tex = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/RebornModPhoenixes/FabergeEgg_Glow").Value;
            int biome = WhatFreakingBiomeAmIInAnyways();
            float glowAmt = 1f + (currentGlowFade / GlowFadeMax) - 1f;

            if (glowAmt == 0)
            {
                if (biome == 1)
                    biomeColor = Color.DarkBlue;
                else if (biome == 2)
                    biomeColor = Color.DarkGreen;
                else if (biome == 3)
                    biomeColor = Color.DarkRed;
                else
                    biomeColor = new Color();
            }

            if (biome != 0)
                currentGlowFade++;
            else
                currentGlowFade--;

            // ugly
            if (biome != 0 && ((biome != 1 && biomeColor == Color.DarkBlue) || (biome != 2 && biomeColor == Color.DarkGreen) || (biome != 3 && biomeColor == Color.DarkRed)) || biomeColor == new Color())
            {
                currentGlowFade--;
                currentGlowFade--;
            }

            if (currentGlowFade > GlowFadeMax)
                currentGlowFade = GlowFadeMax;
            else if (currentGlowFade < 0)
                currentGlowFade = 0;

            Color newColor = biomeColor * glowAmt;
            newColor.A = (byte)(255 * glowAmt);
            spriteBatch.Draw(tex, position, frame, newColor * glowAmt, 0, origin, scale, SpriteEffects.None, 0);
        }

        private int WhatFreakingBiomeAmIInAnyways()
        {
            // too lazy to make an enum
            // 1 is cryonix, 2 is vernix, 3 is chaotrix
            if (Main.LocalPlayer.ZoneSnow)
                return 1;
            else if (Main.LocalPlayer.ZoneJungle)
                return 2;
            else if (Main.LocalPlayer.Calamity().ZoneAbyss)
                return 3;

            return 0;
        }
    }
}
