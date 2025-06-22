using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.Misc;
using Terraria.GameContent.ItemDropRules;

namespace CalRemix.Content.Items.Bags
{
    public class CreeperBag : ModItem
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
            Item.width = 32;
            Item.height = 32;
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
            itemLoot.AddIf(() => !Main.masterMode, ItemID.TissueSample, new Fraction(2, 3), 1, 3);
            itemLoot.AddIf(() => !Main.masterMode, ItemID.CrimtaneOre, new Fraction(2, 3), 5, 7);
            itemLoot.AddIf(() => !Main.expertMode && Main.masterMode, ItemID.TissueSample, new Fraction(1, 2), 1, 2);
            itemLoot.AddIf(() => !Main.expertMode && Main.masterMode, ItemID.CrimtaneOre, new Fraction(2, 3), 2, 4);
            itemLoot.Add(ItemID.Heart, 2);
        }
    }
    public class EaterOfWorldsBodyBag : ModItem
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
            Item.width = 32;
            Item.height = 32;
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
            itemLoot.AddIf(() => !Main.masterMode, ItemID.DemoniteOre, new Fraction(1, 2), 1, 3);
            itemLoot.AddIf(() => !Main.masterMode, ItemID.ShadowScale, new Fraction(1, 5), 1, 2);
            itemLoot.AddIf(() => !Main.expertMode && Main.masterMode, ItemID.DemoniteOre, new Fraction(1, 3), 1, 2);
            itemLoot.AddIf(() => !Main.expertMode && Main.masterMode, ItemID.ShadowScale, new Fraction(1, 10), 1, 2);
        }
    }
    public class EaterOfWorldsTailBag : ModItem
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
            Item.width = 32;
            Item.height = 32;
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
            itemLoot.AddIf(() => !Main.masterMode, ItemID.DemoniteOre, new Fraction(1, 2), 1, 3);
            itemLoot.AddIf(() => !Main.masterMode, ItemID.ShadowScale, new Fraction(1, 5), 1, 2);
            itemLoot.AddIf(() => !Main.expertMode && Main.masterMode, ItemID.DemoniteOre, new Fraction(1, 3), 1, 2);
            itemLoot.AddIf(() => !Main.expertMode && Main.masterMode, ItemID.ShadowScale, new Fraction(1, 10), 1, 2);
        }
    }

    public class WorldEvilBagsNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.Creeper)
            {
                npcLoot.AddConditionalPerPlayer(() => Main.expertMode, ModContent.ItemType<CreeperBag>());
            }
            else if (npc.type == NPCID.EaterofWorldsBody)
            {
                npcLoot.AddConditionalPerPlayer(() => Main.expertMode, ModContent.ItemType<EaterOfWorldsBodyBag>());
            }
            else if (npc.type == NPCID.EaterofWorldsTail)
            {
                npcLoot.AddConditionalPerPlayer(() => Main.expertMode, ModContent.ItemType<EaterOfWorldsTailBag>());
            }
        }
    }
}
