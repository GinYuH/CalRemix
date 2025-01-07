using CalRemix.Content.Cooldowns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class ChloriumConehead : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Banana Clown Mask");
            Tooltip.SetDefault("2 magic damage");
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<MagicDamageClass>().Flat += 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<BananaClownSleeves>() && legs.type == ModContent.ItemType<BananaClownPants>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Consuming fruits grants unlimited mana and increased magic damage for a short period of time\nThis effect has a 1 minute cooldown";
            player.GetModPlayer<CalRemixPlayer>().bananaClown = true;
        }
    }
}
