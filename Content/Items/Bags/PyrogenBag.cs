using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.Armor;
using CalamityMod.Items.Materials;

namespace CalRemix.Content.Items.Bags
{
    public class PyrogenBag : ModItem
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
            Item.rare = ItemRarityID.Cyan;
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
            itemLoot.Add(ModContent.ItemType<EssenceofHavoc>(), 1, 8, 10);
            itemLoot.Add(ModContent.ItemType<PyrogenMask>(), 7);
            itemLoot.Add(ModContent.ItemType<SoulofPyrogen>());
            itemLoot.AddRevBagAccessories();
            itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, ModContent.ItemType<PyroclasticFlow>(), ModContent.ItemType<PlumeflameBow>(), ModContent.ItemType<TheFirestorm>(), ModContent.ItemType<Magmasher>(), ModContent.ItemType<PhreaticChanneler>()));
        }
        public override void RightClick(Player player)
        {
            Point p = player.Bottom.ToTileCoordinates();
            Tile t = CalamityUtils.ParanoidTileRetrieval(p.X, p.Y);
            if (!t.HasTile && t.LiquidAmount <= 0)
            {
                t.LiquidType = LiquidID.Lava;
                t.LiquidAmount = 255;
            }
        }
    }
}
