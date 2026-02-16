using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod;

namespace CalRemix.Content.Items.Weapons
{
    public class Doubler : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.Cyan;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.UseSound = BetterSoundID.ItemSlapHandSmack;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Item.damage = 168;
            Item.knockBack = 12f;
        }

        public override void HoldItem(Player player)
        {
            //player.moveSpeed *= 3;
        }
    }
}
