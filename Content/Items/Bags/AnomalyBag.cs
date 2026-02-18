using CalamityMod;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Rarities;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables.Subworlds.SingularPoint;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Items.Weapons.Stormbow;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Bags
{
    public class AnomalyBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
            ItemID.Sets.BossBag[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.expert = true;
        }
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossBags;
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(lightColor, Color.White, 0.4f);
        }
        public override void PostUpdate()
        {
            Item.TreasureBagLightAndDust();
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return CalamityUtils.DrawTreasureBagInWorld(Item, spriteBatch, ref rotation, ref scale, whoAmI);
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ModContent.ItemType<XenoxApparatus>());
            itemLoot.Add(ModContent.ItemType<VirisiteTear>(), 1, 25, 40);
            itemLoot.Add(ModContent.ItemType<Virisite>(), 1, 100, 200);
            //itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, ModContent.ItemType<Pigeon>(), ModContent.ItemType<FrilledShark>(), ModContent.ItemType<RemoraDart>(), ModContent.ItemType<Laevateinn>(), ModContent.ItemType<XiphactinusGun>(), ModContent.ItemType<LivyatanadoStaff>()));
            itemLoot.AddRevBagAccessories();
        }
    }
}
