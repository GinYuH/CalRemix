using CalamityMod;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.NPCs.Bosses.RajahBoss;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.RajahItems
{
    public class RajahBag : ModItem
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Treasure Bag");
            //Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 32;
            Item.height = 32;
            Item.expert = true; Item.expertOnly = true;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossBags;
        }

        public override bool CanRightClick() => true;

        public override Color? GetAlpha(Color lightColor) => Color.Lerp(lightColor, Color.White, 0.4f);

        public override void PostUpdate() => Item.TreasureBagLightAndDust();

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return CalamityUtils.DrawTreasureBagInWorld(Item, spriteBatch, ref rotation, ref scale, whoAmI);
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ModContent.ItemType<RajahMask>(), 7);

            itemLoot.Add(ModContent.ItemType<RajahPelt>(), 1, 15, 31);

            itemLoot.Add(ModContent.ItemType<RajahSash>(), 1);

            List<int> weapons =
            [
                ModContent.ItemType<BaneOfTheBunny>(),
                ModContent.ItemType<Bunzooka>(),
                ModContent.ItemType<RoyalScepter>(),
                ModContent.ItemType<Punisher>(),
                ModContent.ItemType<RabbitcopterEars>(),
            ];
            if (ModLoader.HasMod("ThoriumMod"))
                weapons.Add(ModContent.ItemType<CarrotFarmer>());
            itemLoot.Add(DropHelper.CalamityStyle(DropHelper.NormalWeaponDropRateFraction, weapons.ToArray()));
        }
    }
}