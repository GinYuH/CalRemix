using System;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using CalamityMod.Sounds;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Content.DamageClasses;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.UI.ElementalSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public class JetstreamSam : ModItem, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;

            Item.width = 20;
            Item.height = 342;
            Item.damage = 6;
            Item.crit = 20;
            Item.useTime = 3;
            Item.useAnimation = 3;

            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(2, 13));
            ItemID.Sets.AnimatesAsSoul[Type] = true;

            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.shoot = ModContent.ProjectileType<NanomachinesSon>();
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // big govt secret: this is actually just a really edited undines retribution. but dont tell anyone that
            // u can edit the i < whatever for extra arrows lool. lol. haha lol
            for (int i = 0; i < 3; i++)
            {
                Vector2 cursorPos = Main.MouseWorld;
                cursorPos.X = player.Center.X + (Main.MouseWorld.X - player.Center.X);
                cursorPos.Y = player.Center.Y - 800 - (100 * (i * 0.75f));

                // arrow position noise pass
                cursorPos.X += Main.rand.Next(-120, 121);
                cursorPos.Y += Main.rand.Next(-60, 61);

                // tile array stuff
                // we gyatta get the highest bit of collision from the spawned point so it looks nice and chill
                for (int ii = 0; ii < 222; ii++)
                {
                    Tile proposedTile = CalamityUtils.ParanoidTileRetrieval((int)(cursorPos.X / 16), (int)((cursorPos.Y / 16) + ii));
                    if (proposedTile != null)
                        if (proposedTile.IsTileSolid())
                        {
                            cursorPos.Y = (cursorPos.Y + (ii * 16));
                            break;
                        }
                }

                int projectile = Projectile.NewProjectile(source, cursorPos.X, cursorPos.Y, 0, 0, type, damage, knockback, player.whoAmI, 0.0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Murasama>().
                AddIngredient(ItemID.Cobweb, 15).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
    public class NanomachinesSon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nanomachine's Son");
            Main.projFrames[Type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 86;
            Projectile.height = 1080;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 1800;
            Projectile.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {

            if (Projectile.frameCounter > Main.projFrames[Type])
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Type])
            {
                Projectile.Kill();
            }
            Projectile.frameCounter++;

            if (Projectile.frame == 1)
            {
                SoundEngine.PlaySound(CommonCalamitySounds.ExoDeathSound, Projectile.Center);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.position - Main.screenPosition, tex.Frame(1, Main.projFrames[Type], 0, Projectile.frame), Color.White, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2 / Main.projFrames[Type]), Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

        //public override Color? GetAlpha(Color lightColor) => new Color(100, 0, 0, 0);
    }
}