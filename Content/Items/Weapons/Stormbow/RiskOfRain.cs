using System;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.FurnitureStratus;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Content.DamageClasses;
using CalRemix.Content.Items.Misc;
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
    public class RiskOfRain : ModItem, ILocalizedModType
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
            Item.height = 46;
            Item.damage = 164;
            Item.crit = 4;
            Item.useTime = 190;
            Item.useAnimation = 190;

            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.Calamity().donorItem = true;
            Item.shoot = ModContent.ProjectileType<RiskOfRainArrow>();
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int projectile = Projectile.NewProjectile(source, Main.MouseWorld.X, Main.MouseWorld.Y, 0, 0, type, damage, knockback, player.whoAmI, 1);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.AquaScepter, 1).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient<AquasScepter>().
                AddIngredient(ItemID.WaterBucket, 5).
                AddIngredient<Watercooler>().
                AddIngredient<JellyChargedBattery>(2).
                AddIngredient<CosmiliteBar>(400).
                AddIngredient<AscendantSpiritEssence>(77).
                AddIngredient<Necroplasm>(668).
                AddIngredient(ItemID.TungstenOre, 37).
                AddIngredient(ItemID.BookStaff, 1).
                AddIngredient<StratusBricks>(5000).
                AddIngredient(ItemID.WaterBucket, 10).
                AddIngredient<ReaperTooth>(40).
                AddIngredient<NightmareFuel>(50).
                AddIngredient<EndothermicEnergy>(50).
                AddIngredient<DarksunFragment>(25).
                AddIngredient(ItemID.Toilet, 1).
                AddIngredient<ElectrolyteGelPack>(5).
                AddIngredient<Water>(22).
                AddIngredient<Those>().
                AddIngredient<Who>().
                AddIngredient<Know>().
                AddIngredient(ItemID.Skull, 1).
                AddIngredient(ItemID.Skull, 1).
                AddIngredient(ItemID.Skull, 1).
                AddIngredient(ItemID.Skull, 1).
                AddIngredient(ItemID.Skull, 1).
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient(ItemID.AquaScepter, 1).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient<AquasScepter>().
                AddIngredient(ItemID.WaterBucket, 5).
                AddIngredient<Watercooler>().
                AddIngredient<JellyChargedBattery>(2).
                AddIngredient<CosmiliteBar>(400).
                AddIngredient<AscendantSpiritEssence>(77).
                AddIngredient<Necroplasm>(668).
                AddIngredient(ItemID.TungstenOre, 37).
                AddIngredient(ItemID.BookStaff, 1).
                AddIngredient<StratusBricks>(5000).
                AddIngredient(ItemID.WaterBucket, 10).
                AddIngredient<ReaperTooth>(40).
                AddIngredient<NightmareFuel>(50).
                AddIngredient<EndothermicEnergy>(50).
                AddIngredient<DarksunFragment>(25).
                AddIngredient(ItemID.Toilet, 1).
                AddIngredient<ElectrolyteGelPack>(5).
                AddIngredient<NebulousCore>().
                AddTile<CosmicAnvil>().
                Register();
        }
    }
    public class RiskOfRainArrow : ModProjectile
    {
        public ref float Mode => ref Projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BoneArrow);

            Projectile.width = 18;
            Projectile.height = 52;

            Projectile.arrow = true;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Projectile.timeLeft = 220;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.penetrate = 1;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Mode == 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 cursorPos = Main.MouseWorld;
                    cursorPos.X = Projectile.Center.X;
                    cursorPos.Y = Projectile.Center.Y - 800 - (100 * (i * 0.75f));
                    float speedX = Main.rand.Next(-60, 91) * 0.02f;
                    float speedY = Main.rand.Next(-60, 91) * 0.02f;
                    speedY += 15;

                    // arrow position noise pass
                    cursorPos.X += Main.rand.Next(-60, 61);
                    cursorPos.Y += Main.rand.Next(-60, 61);

                    int projectile = Projectile.NewProjectile(Projectile.GetSource_FromThis(), cursorPos, new Vector2(speedX, speedY), ModContent.ProjectileType<RiskOfRainArrow>(), Projectile.damage, 0);

                    for (int ii = 0; ii < 5; ii++)
                    {
                        Dust dust = Dust.NewDustDirect(cursorPos, Projectile.width, Projectile.height, DustID.Electric);
                        dust.noGravity = true;
                        dust.velocity *= 1.5f;
                        dust.scale *= 0.9f;
                    }
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            if (Mode != 1)
            {
                SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
                for (int i = 0; i < 3; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric);
                    dust.noGravity = true;
                    dust.velocity *= 1.5f;
                    dust.scale *= 0.9f;
                }
            }
        }
        public override bool? CanDamage()
        {
            if (Mode == 1)
                return false;

            return null;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Mode != 1)
            {
                Texture2D arrow = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
                Main.EntitySpriteDraw(arrow, Projectile.position - Main.screenPosition, null, Color.AliceBlue, Projectile.rotation, Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Size() * 0.5f, Projectile.scale, SpriteEffects.None);
            }

            return false;
        }
    }
}