using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Sounds;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Buffs.StatDebuffs;
using CalRemix.Projectiles;
using CalRemix.Projectiles.Weapons;
using CalRemix.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Linq;
using System.Collections.Generic;
using CalamityMod;
using System;
using Mono.Cecil;

namespace CalRemix.Items.Weapons
{
    public class TheVerbotenGun : ModItem
	{
        public int coinCount = 0;
        public bool hasThrownCoins = false;
        public override void SetStaticDefaults() 
		{
            SacrificeTotal = 1;
            DisplayName.SetDefault("The Verboten Gun");
            Tooltip.SetDefault("Ultrakill\n" +
                "Left click to destroy everything\n" +
                "This attack has four different types that can be toggled between with the Verboten Gun hotkey\n" +
                "Right click to fire a lifeform disintegrating deathray that loops around the screen multiple times on contact\n" + // Will get to this part later I just wanna finish this gun
                "The dev team is not liable for any game crashes this weapon causes");
        }

        public override void SetDefaults() 
		{
			Item.width = 716;
			Item.height = 212;
			Item.rare = ModContent.RarityType<Violet>();
			Item.value = CalamityGlobalItem.Rarity15BuyPrice;
			Item.useAnimation = 1;
            Item.useTime = 2;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			//Item.UseSound = CommonCalamitySounds.LargeWeaponFireSound;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 6666;
			Item.knockBack = 8f; 
			Item.noMelee = true;
			Item.crit = 1;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
            Item.consumeAmmoOnLastShotOnly = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Item.shootSpeed = 12f;
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, -1250))
            {
                position += muzzleOffset;

            }

            position.Y -= 90;

            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().VerbotenMode == 4)
            {
                velocity = velocity.RotatedByRandom(MathHelper.ToRadians(90));
            }
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(0)); // turn random spread off when mode is not 4

            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().VerbotenMode == 3) // coins!!!
            {
                float shootAngle = (player.Calamity().mouseWorld - player.MountedCenter).ToRotation() * -1;
                if (!hasThrownCoins)
                {
                    if (shootAngle > -MathHelper.Pi + MathHelper.Pi / 4 && shootAngle < -MathHelper.PiOver2)
                        shootAngle = -MathHelper.Pi + MathHelper.Pi / 4;

                    else if (shootAngle < -MathHelper.Pi / 4 && shootAngle >= -MathHelper.PiOver2)
                        shootAngle = -MathHelper.Pi / 4;

                    velocity = (shootAngle * -1).ToRotationVector2() * 1.3f - Vector2.UnitY * 1.12f + player.velocity / 4f;
                }
                shootAngle = (player.Calamity().mouseWorld - player.MountedCenter).ToRotation();
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float rotation = MathHelper.ToRadians(45);
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().VerbotenMode == 1) // random projectile spam
            {
                Item.damage = 888;
                Item.useTime = 3;
                Item.useAnimation = 3;
                Item.UseSound = null;
                int projInd = Projectile.NewProjectile(source, position, velocity, Main.rand.Next(ProjectileLoader.ProjectileCount), damage, knockback, player.whoAmI);
                if (!Main.projectile[projInd].friendly)
                { 
                    Main.projectile[projInd].friendly = true;
                    Main.projectile[projInd].hostile = false;
                }
                if (player.GetModPlayer<CalRemixPlayer>().tvgNoFireList.Contains(Main.projectile[projInd].type))
                {
                    Main.projectile[projInd].active = false;
                }
            }
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().VerbotenMode == 2) // supreme tyranny's end
            {
                Item.damage = 266; // still hits 40k+ dps because this is being multiplied by 32, each shot does 8512 damage before the special effects of each bullet
                Item.useTime = 50;
                Item.useAnimation = 50;
                Item.shootSpeed = 16f;
                Item.UseSound = CommonCalamitySounds.LargeWeaponFireSound; //literally vomit out every kind of bullet projectile I can think of
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AccelerationRoundProj>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AcidRoundProj>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AMRShot>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AstralRound>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<DragonsBreathRound>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<EnhancedNanoRoundProj>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<FlashRoundProj>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<HolyFireBulletProj>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<HyperiusBulletProj>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<IcyBulletProj>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<IlluminatedBullet>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<RealmRavagerBullet>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ShockblastRound>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SicknessRound>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SlagRound>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SuperballBulletProj>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<VeriumBulletProj>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CalamityMod.Projectiles.Ranged.Voidragon>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AxisExoBullet>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ProjectileID.Bullet, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ProjectileID.CrystalBullet, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ProjectileID.MeteorShot, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ProjectileID.CursedBullet, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ProjectileID.ChlorophyteBullet, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ProjectileID.BulletHighVelocity, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ProjectileID.IchorBullet, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ProjectileID.VenomBullet, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ProjectileID.PartyBullet, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ProjectileID.NanoBullet, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ProjectileID.ExplosiveBullet, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ProjectileID.GoldenBullet, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ProjectileID.MoonlordBullet, damage, knockback, player.whoAmI);
            }
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().VerbotenMode == 3) // it never ends it never ends it never en
            {
                Item.useTime = 20;
                Item.damage = 666;
                Item.useAnimation = 20;
                Item.UseSound = CommonCalamitySounds.LargeWeaponFireSound;
                Item.autoReuse = false;
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ContinuumBullet>(), damage, knockback, player.whoAmI);
            }
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().VerbotenMode == 4) // Arngrener
            {
                Item.damage = 666;
                Item.useTime = 1; // can we get much higher
                Item.useAnimation = 1;
                Item.UseSound = CommonCalamitySounds.LargeWeaponFireSound;
                return true;
            }
            Item.autoReuse = true;
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int modeLine = tooltips.FindIndex(line => line.Text == "Left click to destroy everything");

            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().VerbotenMode == 1)
            {
                tooltips[modeLine].Text = "Left click to fire an unholy amount of projectiles from everything imaginable";
                tooltips.Insert(modeLine + 1, new TooltipLine(Mod, Item.Name, "These projectiles are not guarenteed to be friendly and will likely instantly kill you if they aren't"));
            }
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().VerbotenMode == 2)
            {
                tooltips[modeLine].Text = "Left click to fire a .22000 caliber sniper round";
                tooltips.Insert(modeLine + 1, new TooltipLine(Mod, Item.Name, "Ignores every form of defense and inflicts way too many debuffs"));
            }
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().VerbotenMode == 3)
            {
                tooltips[modeLine].Text = "Left click to fire an infinitely-piercing round";
                tooltips.Insert(modeLine + 1, new TooltipLine(Mod, Item.Name, "These bullets warp spacetime and wrap around to the opposite edge of the screen"));
            }
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().VerbotenMode == 4)
            {
                int choice = Main.rand.Next(10); // flickers between these two like, a lot
                switch (choice)
                {
                    case 9:
                        tooltips[modeLine].Text = "Left click to waste bullets really fast";
                        break;
                    default:
                        tooltips[modeLine].Text = "Left click to unleash bullet hell";
                        break;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe(). // you literally need a chest to craft this
                AddIngredient<AcesHigh>(1).
                AddIngredient<AngelicShotgun>(1).
                AddIngredient<Animosity>(1).
                AddIngredient<AntiMaterielRifle>(1).
                AddIngredient<AquashardShotgun>(1).
                AddIngredient<Archerfish>(1).
                AddIngredient<Arngren>(1).
                AddIngredient<AstralBlaster>(1).
                AddIngredient<Auralis>(1).
                AddIngredient<Axisdriver>(2). // you actually need two of these because i put two in the sprite by accident
                AddIngredient<BulletFilledShotgun>(1).
                AddIngredient<Butcher>(1).
                AddIngredient<ClamorRifle>(1).
                AddIngredient<ClaretCannon>(1).
                AddIngredient<ClockGatlignum>(1).
                AddIngredient<ConferenceCall>(1).
                AddIngredient<CursedCapper>(1).
                AddIngredient<Disseminator>(1).
                AddIngredient<DodusHandcannon>(1).
                AddIngredient<DragonsBreath>(1).
                AddIngredient<Eviscerator>(1).
                AddIngredient<FetidEmesis>(1).
                AddIngredient<FrostbiteBlaster>(1).
                AddIngredient<Fungicide>(1).
                AddIngredient<GoldenEagle>(1).
                AddIngredient<GunkShot>(1).
                AddIngredient<HalibutCannon>(1).
                AddIngredient<Hellborn>(1).
                AddIngredient<Helstorm>(1).
                AddIngredient<Infinity>(1).
                AddIngredient<Karasawa>(1).
                AddIngredient<Leviatitan>(1).
                AddIngredient<MagnaStriker>(1).
                //AddIngredient<MarniteBayonet>(1). //removed
                AddIngredient<Megalodon>(1).
                AddIngredient<MidasPrime>(1).
                AddIngredient<Minigun>(1).
                AddIngredient<Needler>(1).
                AddIngredient<OnyxChainBlaster>(1).
                //AddIngredient<OpaiStriker>(1).
                AddIngredient<P90>(1).
                AddIngredient<PearlGod>(1).
                AddIngredient<PestilentDefiler>(1).
                AddIngredient<PridefulHuntersPlanarRipper>(1).
                //AddIngredient<ProporsePistol>(1). //removed
                AddIngredient<RealmRavager>(1).
                AddIngredient<RubicoPrime>(1).
                AddIngredient<SDFMG>(1).
                //AddIngredient<SDOMG>(1).
                AddIngredient<Seadragon>(1).
                AddIngredient<Shredder>(1).
                AddIngredient<Shroomer>(1).
                AddIngredient<SlagMagnum>(1).
                AddIngredient<SomaPrime>(1).
                //AddIngredient<SpectreRifle>(1). //removed
                AddIngredient<Spyker>(1).
                AddIngredient<StormDragoon>(1).
                AddIngredient<SurgeDriver>(1).
                //AddIngredient<Svantechnical>(1). //making it pre-scal cuz fuck her in particular
                AddIngredient<TheJailor>(1).
                AddIngredient<TitaniumRailgun>(1).
                AddIngredient<TyrannysEnd>(1).
                AddIngredient<UniversalGenesis>(1).
                //AddIngredient<Voidragon>(1). //no shadowspec bar weapons
                AddIngredient<Vortexpopper>(1).
                AddTile<DraedonsForge>().
                Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-145f, -40f);
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().VerbotenMode == 2) // apply a shitload of debuffs
            {
                target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 600, false);
                target.AddBuff(ModContent.BuffType<ProfanedWeakness>(), 600, false);
                target.AddBuff(ModContent.BuffType<WitherDebuff>(), 600, false);
                target.AddBuff(ModContent.BuffType<WhisperingDeath>(), 600, false);
                target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 600, false);
                target.AddBuff(ModContent.BuffType<Irradiated>(), 600, false);
                target.AddBuff(ModContent.BuffType<TemporalSadness>(), 600, false);
                target.AddBuff(ModContent.BuffType<Eutrophication>(), 600, false);
                target.AddBuff(ModContent.BuffType<GlacialState>(), 600, false);
                target.AddBuff(BuffID.Ichor, 600, false);
                target.AddBuff(BuffID.CursedInferno, 600, false);
                target.AddBuff(BuffID.BetsysCurse, 600, false);

                //Defies infinite defense enemies that would otherwise survive the shot
                if (target.defense > 999 || target.Calamity().DR >= 0.95f || target.Calamity().unbreakableDR || damage == 0 || damage == 1)
                    target.life = (int)- 0.25f;

                //DR applies after defense, so undo it first
                damage = (int)(damage * (1 / (1 - target.Calamity().DR)));

                //Then proceed to ignore all defense
                int penetratableDefense = (int)Math.Max(target.defense - Main.player[player.whoAmI].GetArmorPenetration<GenericDamageClass>(), 0);
                int penetratedDefense = Math.Min(penetratableDefense, target.defense);
                damage += (int)(0.5f * penetratedDefense);
            }
        }

    }
}