using CalamityMod;
using CalRemix.Content.Items.Armor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.RajahItems.Supreme
{
    public class RajahCache : ModItem
    {
        public override void SetStaticDefaults()
        {
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

            itemLoot.Add(ModContent.ItemType<ChampionPlate>(), 1, 15, 31);

            itemLoot.Add(ModContent.ItemType<RajahCape>(), 1);

            List<int> weapons =
            [
                ModContent.ItemType<Excalihare>(),
                ModContent.ItemType<FluffyFury>(),
                ModContent.ItemType<RabbitsWrath>(),
                ModContent.ItemType<BaneOfTheBunnyEX>(),
                ModContent.ItemType<BunzookaEX>(),
                ModContent.ItemType<RoyalScepterEX>(),
                ModContent.ItemType<PunisherEX>(),
                ModContent.ItemType<CottonCaneEX>(),
            ];

            itemLoot.Add(DropHelper.CalamityStyle(DropHelper.NormalWeaponDropRateFraction, weapons.ToArray()));
        }
    }
}