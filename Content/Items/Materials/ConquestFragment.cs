using CalamityMod;
using CalamityMod.Items.TreasureBags;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class ConquestFragment : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Conquest Fragment");
			Item.ResearchUnlockCount = 100;
    	}
		public override void SetDefaults()
		{
            Item.rare = ItemRarityID.Red;
            Item.expert = true;
            Item.value = 0;
			Item.maxStack = 9999;
        }
    }
    public class ConquestLoot : GlobalItem
    {
        private static readonly List<int> PMBags = new()
        {
            ItemID.KingSlimeBossBag,
            ModContent.ItemType<DesertScourgeBag>(),
            ItemID.EyeOfCthulhuBossBag,
            ModContent.ItemType<CrabulonBag>(),
            ItemID.EaterOfWorldsBossBag,
            ItemID.BrainOfCthulhuBossBag,
        };
        private static readonly List<int> PMBags2 = new()
        {
            ModContent.ItemType<HiveMindBag>(),
            ModContent.ItemType<PerforatorBag>(),
            ItemID.QueenBeeBossBag,
            ItemID.DeerclopsBossBag,
            ItemID.SkeletronBossBag,
            ModContent.ItemType<SlimeGodBag>(),

        };
        private static readonly List<int> HMBags = new()
        {
            ItemID.WallOfFleshBossBag,
            ItemID.QueenSlimeBossBag,
            ModContent.ItemType<CryogenBag>(),
            ItemID.TwinsBossBag,
            ModContent.ItemType<AquaticScourgeBag>(),
            ItemID.DestroyerBossBag,
            ModContent.ItemType<BrimstoneWaifuBag>(),
            ItemID.SkeletronPrimeBossBag,

        };
        private static readonly List<int> HMBags2 = new()
        {
            ModContent.ItemType<DragonfollyBag>(),
            ModContent.ItemType<CalamitasCloneBag>(),
            ItemID.PlanteraBossBag,
            ModContent.ItemType<LeviathanBag>(),
            ModContent.ItemType<AstrumAureusBag>(),
            ItemID.GolemBossBag,
            ItemID.FishronBossBag,
            ModContent.ItemType<PlaguebringerGoliathBag>(),
            ItemID.FairyQueenBossBag,
            ModContent.ItemType<RavagerBag>(),
            ItemID.CultistBossBag,
            ModContent.ItemType<AstrumDeusBag>(),

        };
        private static readonly List<int> GMBags = new()
        {
            ItemID.MoonLordBossBag,
            ModContent.ItemType<ProvidenceBag>(),
            ModContent.ItemType<CeaselessVoidBag>(),
            ModContent.ItemType<StormWeaverBag>(),
            ModContent.ItemType<SignusBag>(),
            ModContent.ItemType<PolterghastBag>(),
            ModContent.ItemType<OldDukeBag>(),

        };
        private static readonly List<int> GMBags2 = new()
        {
            ModContent.ItemType<DevourerofGodsBag>(),
            ModContent.ItemType<YharonBag>(),
            ModContent.ItemType<CalamitasCoffer>(),
            ModContent.ItemType<DraedonBag>(),
        };
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (PMBags.Contains(item.type))
                itemLoot.Add(ModContent.ItemType<ConquestFragment>(), 1, 1, 2);
            if (PMBags2.Contains(item.type))
                itemLoot.Add(ModContent.ItemType<ConquestFragment>(), 1, 2, 3);
            if (HMBags.Contains(item.type))
                itemLoot.Add(ModContent.ItemType<ConquestFragment>(), 1, 3, 5);
            if (HMBags2.Contains(item.type))
                itemLoot.Add(ModContent.ItemType<ConquestFragment>(), 1, 5, 8);
            if (GMBags.Contains(item.type))
                itemLoot.Add(ModContent.ItemType<ConquestFragment>(), 1, 10, 20);
            if (GMBags2.Contains(item.type))
                itemLoot.Add(ModContent.ItemType<ConquestFragment>(), 1, 20, 40);
        }
    }
}
