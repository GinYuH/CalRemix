using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod.Items.Weapons.Rogue;

namespace CalRemix.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Head)]
    public class RetromansPowerStar : ModItem
    {
        public override string Texture => "CalRemix/Content/Items/Accessories/RetroMan";
        public override void Load()
        {
            // The code below runs only if we're not loading on a server
            if (Main.netMode == NetmodeID.Server)
                return;

            // Add equip textures
            EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Head}", EquipType.Head, this);
            EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Body}", EquipType.Body, this);
            EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this);
        }

        public void SetSet(Player player)
        {
            player.head = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            player.body = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            player.legs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Retroman's Power Star");
            Tooltip.SetDefault("Transforms you into the Retro Man \n Item dedicated to QuestNinja");

            if (Main.netMode == NetmodeID.Server)
                return;

            int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

            ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
            ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
            ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
            ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Gray;
            Item.vanity = true;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
                player.GetModPlayer<CalRemixPlayer>().retroman = true;
        }

        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<CalRemixPlayer>().retroman = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient<ThrowingBrick>(100)
                .Register();
        }
    }

}
