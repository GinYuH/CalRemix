using CalRemix.UI;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Core.Retheme.Sneakers
{
    public class UncreasableKicks : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Uncreasable Kicks");
            /* Tooltip.SetDefault("The end of the journey\n" +
                "The highest of peaks\n" +
                "The most blinding light\n" +
                "Uncreasable Kicks, brought to you by Fandom\n\n" +
                "Max hp permanently increased by running\n" +
                "Constantly exhude net worth\n" +
                "Massively boosted run speed"); */

            //Cuz when its initialized its lower than that so we gotta fix it
            if (SneakersRetheme.SneakerList.Length < ItemLoader.ItemCount)
                Array.Resize(ref SneakersRetheme.SneakerList, ItemLoader.ItemCount);

            SneakersRetheme.SneakerList[Type] = true;
            SneakersRetheme.sneakerBrands.Add(Type, SneakersRetheme.ShoeBrand.Fandom);
            NetWorthPlayer.netWorthCapPerSneaker.Add(Type, int.MaxValue - Item.buyPrice(platinum: 500));

        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(platinum:999);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
            Item.rare = ItemRarityID.Master;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<NetWorthPlayer>().netWorthGod = true;
        }
    }
}
