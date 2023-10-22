using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Accessories;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Rarities;
using CalamityMod.Items;
using CalamityMod;
using Terraria.DataStructures;
using CalRemix.Projectiles.Accessories;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using CalamityMod.Items.Weapons.Summon;

namespace CalRemix.Items.Accessories
{
    public class ZenithArcanum : ModItem
    {
        public override string Texture => "CalamityMod/Items/Accessories/TheEvolution";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Zenith Arcanum");
            Tooltip.SetDefault("'Top of the food chain'\n"+
            "Summons various spirits of the world to protect you\n" +
            "20 % increase to summon damage and defense\n" +
            "+ 4 life regeneration, 15 % increased pick speed, and + 8 max minions\n" +
            "Increased minion knockback\n" +
            "Minions inflict a variety of debuffs and spawn skeletal limbs on enemy hits"); 
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 10));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 36;
            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.rare = RarityType<Violet>();
            Item.accessory = true;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            if (player.GetModPlayer<CalRemixPlayer>().arcanumHands)
                return false;

            return true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<SummonDamageClass>() += 0.2f;
            player.statDefense *= 1.2f;
            player.pickSpeed = (int)(1.15 * player.pickSpeed);
            player.maxMinions += 8;
            player.lifeRegen += 4;

            CalamityPlayer caPlayer = player.Calamity();
            caPlayer.shadowMinions = true; 
            caPlayer.holyMinions = true; 
            caPlayer.voltaicJelly = true;
            caPlayer.starTaintedGenerator = true;
            caPlayer.nucleogenesis = true;

            caPlayer.howlTrio = true;
            caPlayer.howlsHeart = true;
            caPlayer.brimstoneWaifu = true;
            caPlayer.sirenWaifu = true;
            caPlayer.sandWaifu = true;
            caPlayer.sandBoobWaifu = true;
            caPlayer.cloudWaifu = true;
            caPlayer.MutatedTruffleBool = true;
            caPlayer.miniOldDuke = true;
            caPlayer.allWaifus = true;
            caPlayer.elementalHeart = true;
            caPlayer.virili = true;

            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            modPlayer.arcanumHands = true; 
            int brimmy = ProjectileType<BrimstoneElementalMinion>();
            int siren = ProjectileType<WaterElementalMinion>();
            int healer = ProjectileType<SandElementalHealer>();
            int sandy = ProjectileType<SandElementalMinion>();
            int cloudy = ProjectileType<CloudElementalMinion>();
            int thomas = ProjectileType<PlaguePrincess>();
            int yd = ProjectileType<MutatedTruffleMinion>();
            caPlayer.gladiatorSword = true;

            var source = player.GetSource_Accessory(Item);
            Vector2 velocity = new Vector2(0f, -1f);
            int elementalDmg = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(290);
            float kBack = 2f + player.GetKnockback<SummonDamageClass>().Additive;

            if (player.whoAmI == Main.myPlayer)
            {
                int baseDamage = 290;
                int swordDmg = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(baseDamage);
                if (player.ownedProjectileCounts[brimmy] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, brimmy, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 20;
                }
                if (player.ownedProjectileCounts[siren] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, siren, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 290;
                }
                if (player.ownedProjectileCounts[healer] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, healer, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 290;
                }
                if (player.ownedProjectileCounts[sandy] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, sandy, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 290;
                }
                if (player.ownedProjectileCounts[cloudy] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, cloudy, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 290;
                }
                if (player.ownedProjectileCounts[thomas] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, thomas, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 290;
                }
                if (player.ownedProjectileCounts[ProjectileType<HowlsHeartHowl>()] < 1)
                {
                    int damage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(290);
                    Projectile howl = Projectile.NewProjectileDirect(source, player.Center, -Vector2.UnitY, ProjectileType<HowlsHeartHowl>(), damage, 1f, player.whoAmI, 0f, 1f);
                    howl.originalDamage = damage;
                }
                if (player.ownedProjectileCounts[ProjectileType<HowlsHeartCalcifer>()] < 1)
                {
                    Projectile.NewProjectile(source, player.Center, -Vector2.UnitY, ProjectileType<HowlsHeartCalcifer>(), 0, 0f, player.whoAmI, 0f, 0f);
                }
                if (player.ownedProjectileCounts[ProjectileType<HowlsHeartTurnipHead>()] < 1)
                {
                    Projectile.NewProjectile(source, player.Center, -Vector2.UnitY, ProjectileType<HowlsHeartTurnipHead>(), 0, 0f, player.whoAmI, 0f, 0f);
                }
                if (player.ownedProjectileCounts[ProjectileType<MutatedTruffleMinion>()] < 1)
                {
                    int damage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(290);
                    Projectile dud = Projectile.NewProjectileDirect(source, player.Center, -Vector2.UnitY, ProjectileType<MutatedTruffleMinion>(), damage, 1f, player.whoAmI, 0f, 1f);
                    dud.originalDamage = damage;
                }
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<HeartoftheElements>(1).
                AddIngredient<HowlsHeart>(1).
                AddIngredient<MutatedTruffle>(1).
                AddIngredient<GladiatorsLocket>(1).
                AddIngredient<Nucleogenesis>(1).
                AddIngredient<DarkSunRing>(1).
                AddIngredient<AshesofAnnihilation>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
