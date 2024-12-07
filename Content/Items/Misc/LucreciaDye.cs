using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Dyes;
using CalamityMod.Items.Weapons.Melee;

namespace CalRemix.Content.Items.Misc
{
    public class LucreciaDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(Mod.Assets.Request<Effect>("Assets/Effects/LucreciaDyeShader"), "DyePass").
            UseColor(new Color(186, 75, 213)).UseSecondaryColor(new Color(255, 127, 255));
        public override void SafeSetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
        }

        public override void SafeSetDefaults()
        {
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe(3).
                AddIngredient(ItemID.BottledWater, 3).
                AddIngredient<Lucrecia>().
                AddTile(TileID.DyeVat).
                Register();
        }
    }
}
