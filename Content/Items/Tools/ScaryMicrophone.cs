using CalamityMod.Items.Materials;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.UI;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Tools
{
    public class ScaryMicrophone : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.useAnimation = 30;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)
        {
            if (player.Remix().jumpscareTimer <= 0)
            {
                Jumpscare[] jumpscareArray = new Jumpscare[EclipseJumpscares.jumpscareTypes.Count];
                EclipseJumpscares.jumpscareTypes.Values.CopyTo(jumpscareArray, 0);
                player.Remix().jumpscare = Utils.SelectRandom(Main.rand, jumpscareArray);
                SoundEngine.PlaySound(player.Remix().jumpscare.sound);
                player.Remix().jumpscareTimer = player.Remix().jumpscare.duration;
                return true;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Megaphone).
                AddIngredient<DarksunFragment>().
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();
        }
    }
}
