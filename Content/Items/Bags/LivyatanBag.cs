﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Items.Accessories;
using CalamityMod.Items.Materials;
using CalRemix.Content.Items.Armor;
using CalamityMod.Sounds;
using CalRemix.Content.Projectiles.Hostile;
using Terraria.Audio;
using Terraria.DataStructures;

namespace CalRemix.Content.Items.Bags
{
    public class LivyatanBag : ModItem
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
            itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, ModContent.ItemType<FrilledShark>(), ModContent.ItemType<RemoraDart>(), ModContent.ItemType<Laevateinn>(), ModContent.ItemType<XiphactinusGun>(), ModContent.ItemType<LivyatanadoStaff>()));
            itemLoot.AddRevBagAccessories();
        }
    }
}
