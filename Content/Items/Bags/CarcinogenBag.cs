using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.Placeables;
using CalRemix.Content.Items.Armor;
using Terraria.Audio;
using CalRemix.Content.NPCs.Bosses.Carcinogen;

namespace CalRemix.Content.Items.Bags
{
    public class CarcinogenBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
            ItemID.Sets.BossBag[Type] = true;
            ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;
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
            itemLoot.Add(ModContent.ItemType<Asbestos>(), 1, 216, 224);
            itemLoot.Add(ModContent.ItemType<SoulofCarcinogen>());
            itemLoot.Add(ModContent.ItemType<FiberBaby>());
            itemLoot.Add(ModContent.ItemType<Chainsmoker>());
            itemLoot.Add(ModContent.ItemType<CarcinogenMask>(), 7);
            itemLoot.AddRevBagAccessories();
        }
        public override void RightClick(Player player)
        {
            SoundEngine.PlaySound(Carcinogen.DeathSound, player.Center);
            for (int i = 0; i < Main.maxDust; i++)
            {
                Dust.NewDust(player.Center - Vector2.One * 80, 160, 160, DustID.Smoke, Scale: Main.rand.NextFloat(2, 6));
            }
        }
    }
}
