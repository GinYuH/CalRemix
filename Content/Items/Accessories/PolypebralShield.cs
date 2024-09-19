using Terraria.ModLoader;
using Terraria;
using System.Collections.Generic;
using Terraria.ID;

namespace CalRemix.Content.Items.Accessories
{
    public class PolypebralShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Polypebral Shield");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            base.Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 8;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalRemixPlayer pplayer = player.GetModPlayer<CalRemixPlayer>();
            pplayer.polyShieldChargeEnabled = true;
            player.dashType = 0;

        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            List<string> hkList = CalRemixKeybinds.PolyDashKeybind.GetAssignedKeys();
            string hotkey = "";
            if (hkList.Count == 0 || hkList == null)
            {
                hotkey = "[NONE]";
            } else
            {
                hotkey = "["+hkList[0]+"]";
            }

            /*
            TooltipLine line = list.FirstOrDefault((TooltipLine x) => x.Mod == "Terraria" && x.Name == "Tooltip0");
            if (line != null)
            {
                line.Text = "Press " + hotkey + " to perform a omnidirectional dash that can be used to bonk enemies.\n" +
                    "Has a long cooldown. \n" +
                    "Initiating another dash right before hitting an enemy will allow you to control your ricochet";
            }
            */
        }
    }
}