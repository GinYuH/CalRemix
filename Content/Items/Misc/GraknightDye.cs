using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Dyes;
using CalamityMod.Items.Weapons.Melee;
using CalRemix.Content.Items.Weapons;
using CalamityMod.Items;

namespace CalRemix.Content.Items.Misc
{
    public class GraknightDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(Mod.Assets.Request<Effect>("Assets/Effects/GrakitShader"), "DyePass");
        public override void SafeSetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
        }

        public override void SafeSetDefaults()
        {
            Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.BottledWater).
                AddIngredient(ItemID.Granite).
                AddTile(TileID.DyeVat).
                Register();
        }
    }
}
