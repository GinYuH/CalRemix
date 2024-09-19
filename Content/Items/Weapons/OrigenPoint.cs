using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System.Collections.Generic;
using Terraria.Utilities;
using CalRemix.Content.NPCs.Bosses.Origen;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons
{
    public class OrigenPoint : ModItem
    {
        public static List<Color> origenPalette = new()
        {
            new Color(17, 80, 81),
            new Color(16, 81, 82),
            new Color(13, 90, 93),
            new Color(15, 86, 88),
            new Color(15, 100, 98),
            new Color(14, 97, 96),
            new Color(13, 93, 95),
            new Color(15, 100, 97),
            new Color(15, 101, 101),
            new Color(14, 103, 106),
            new Color(13, 105, 114),
            new Color(14, 105, 110),
            new Color(14, 107, 114),
            new Color(225, 254, 254),
            new Color(15, 111, 115),
            new Color(17, 117, 118),
            new Color(21, 136, 135),
            new Color(10, 134, 143),
            new Color(22, 121, 120),
            new Color(36, 125, 127),
            new Color(29, 131, 131),
            new Color(28, 139, 139),
            new Color(28, 156, 153),
            new Color(26, 172, 167),
            new Color(21, 137, 135),
            new Color(45, 148, 142),
            new Color(52, 152, 144),
            new Color(34, 153, 151),
            new Color(29, 157, 155),
            new Color(68, 174, 162),
            new Color(70, 177, 167),
            new Color(26, 172, 167),
            new Color(80, 194, 177),
            new Color(85, 185, 182),
            new Color(52, 174, 176),
            new Color(25, 182, 180),
            new Color(13, 187, 183),
            new Color(57, 194, 193),
            new Color(82, 183, 180),
            new Color(38, 224, 215),
            new Color(63, 223, 216),
            new Color(78, 221, 222),
        };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Origen Point");
            Tooltip.SetDefault("Creates large, slow moving pixels");
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = RarityHelper.Origen;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.useTime = 47;
            Item.useAnimation = 47;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.autoReuse = true;
            Item.UseSound = CalamityMod.CalPlayer.CalamityPlayer.DefenseDamageSound with { PitchRange = (-1, 1)};
            Item.DamageType = DamageClass.Generic;
            Item.damage = 12;
            Item.knockBack = 22f;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<OrigenPointProjectile>();
            Item.shootSpeed = 0.01f;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(TextureAssets.Item[Type].Value, position, null, origenPalette[(int)Main.rand.NextFloat(0, origenPalette.Count)], 0f, origin, scale, SpriteEffects.None, 0);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            spriteBatch.Draw(TextureAssets.Item[Type].Value, Item.position - Main.screenPosition, null, origenPalette[(int)Main.rand.NextFloat(0, origenPalette.Count)], rotation, TextureAssets.Item[Type].Size() / 2, scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
