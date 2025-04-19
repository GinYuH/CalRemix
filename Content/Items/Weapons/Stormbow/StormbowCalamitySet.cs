using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Tiles.Furniture.CraftingStations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    #region Pre-HM
    // sea prism
    public class SeaPrismStormbow : StormbowAbstract
    {
        public override int damage => 14;
        public override int useTime => 22;
        public override SoundStyle useSound => SoundID.NPCHit10;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<Aquashard>() };
        public override int arrowAmount => 2;
        public override OverallRarity overallRarity => OverallRarity.White;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PalmWoodStormbow>().
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient<PearlShard>(30).
                AddIngredient<SeaPrism>(12).
                AddIngredient<Navystone>(50).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    // acid rain t1
    public class TheFrog : StormbowAbstract
    {
        public override int damage => 9;
        public override SoundStyle useSound => SoundID.Item5;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<EXPLODINGFROG>() };
        public override int arrowAmount => 4;
        public override OverallRarity overallRarity => OverallRarity.Green;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Acidwood>(30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient<SulphuricScale>(67).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    // purified gel
    public class Sludgemonger : StormbowAbstract
    {
        public override int damage => 47;
        public override int useTime => 22;
        public override SoundStyle useSound => SoundID.NPCHit10;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<SlimeBolt>() };
        public override int arrowAmount => 2;
        public override OverallRarity overallRarity => OverallRarity.LightRed;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PurifiedGel>(18).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient<BlightedGel>(18).
                AddTile<StaticRefiner>().
                Register();
        }
    }
    #endregion
    #region HM
    // elemental
    public class ElementalStormsurge : StormbowAbstract
    {
        public override int damage => 96;
        public override int crit => 28;
        public override int useTime => 2;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<RainbowBlast>() };
        public override int arrowAmount => 12;
        public override OverallRarity overallRarity => OverallRarity.Purple;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DemeterStormbow>(1).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.DaedalusStormbow, 1).
                AddIngredient(ItemID.LunarBar, 5).
                AddIngredient<LifeAlloy>(5).
                AddIngredient<GalacticaSingularity>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
    #endregion
    #region Post-ML
    #region Stratus
    public class TwinkleTwinkle : StormbowAbstract
    {
        public override int damage => 54;
        public override int crit => 12;
        public override int useTime => 520;
        public override SoundStyle useSound => SoundID.Item5;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<TwinkleStar>() };
        public override int arrowAmount => 1;
        public override OverallRarity overallRarity => OverallRarity.PureGreen;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Lumenyl>(30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient<RuinousSoul>(30).
                AddIngredient<ExodiumCluster>(30).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
    public class TwinkleStar : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Twinkle Star");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 10;
            Projectile.extraUpdates = 10;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            if (Projectile.scale <= 1.5f)
            {
                Projectile.scale *= 1.01f;
            }

            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.35f / 255f, (255 - Projectile.alpha) * 0f / 255f, (255 - Projectile.alpha) * 0.45f / 255f);

            if (Projectile.ai[0]++ > 5f)
            {
                float dustScaleSize = 1f;
                if (Projectile.ai[0] == 6f)
                {
                    dustScaleSize = 0.25f;
                }
                else if (Projectile.ai[0] == 7f)
                {
                    dustScaleSize = 0.5f;
                }
                else if (Projectile.ai[0] == 8f)
                {
                    dustScaleSize = 0.75f;
                }
                Projectile.ai[0] += 1f;
                int dustType = 176;
                for (int i = 0; i < 3; i++)
                {
                    int fire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 1, default, 1f);
                    Dust dust = Main.dust[fire];
                    if (Main.rand.NextBool(3))
                    {
                        dust.noGravity = true;
                        dust.scale *= 1.75f;
                        dust.velocity.X *= 2f;
                        dust.velocity.Y *= 2f;
                    }
                    else
                    {
                        dust.scale *= 0.5f;
                    }
                    dust.velocity.X *= 1.2f;
                    dust.velocity.Y *= 1.2f;
                    dust.scale *= dustScaleSize;
                    dust.velocity += Projectile.velocity;
                    if (!dust.noGravity)
                    {
                        dust.velocity *= 0.5f;
                    }
                }
            }

            Projectile.rotation += 0.3f * (float)Projectile.direction;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Nightwither>(), 240);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Nightwither>(), 240);
        }
    }
    #endregion
    #endregion
}
