﻿using CalRemix.Content.Projectiles.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalRemix.Content.DamageClasses;
using CalamityMod.Rarities;
using CalamityMod.Items;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using CalamityMod.Projectiles.Magic;
using Terraria.DataStructures;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public class TheSimpstring : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 4;
            Item.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<RisingFire>().
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.YellowDye, 5).
                AddIngredient<NightmareFuel>(2).
                AddTile<CosmicAnvil>().
                Register();
        }
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<TheSimpstringHoldout>()] < 1;
    }
    public class TheSimpstringHoldout : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];
        public int ogDamage = 0;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 68;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<StormbowDamageClass>();
        }

        public override void OnSpawn(IEntitySource source)
        {
            ogDamage = Projectile.damage;
        }

        public override void AI()
        {
            int totalTime = 60;
            if (!Owner.active || Owner.dead || Owner.HeldItem.type != ModContent.ItemType<TheSimpstring>())
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 2;
            if (Main.myPlayer == Owner.whoAmI)
            {
                Projectile.Center = Owner.Center + Owner.DirectionTo(Main.MouseWorld) * 32f;
                Projectile.spriteDirection = (Main.MouseWorld.X < Owner.Center.X) ? -1 : 1;
                Owner.ChangeDir(Projectile.spriteDirection);
                Projectile.netUpdate = true;
                Owner.heldProj = Projectile.whoAmI;
                Projectile.rotation = Projectile.DirectionTo(Main.MouseWorld).ToRotation();
                Projectile.rotation += Projectile.spriteDirection == -1 ? MathHelper.Pi : 0;
                Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Owner.HandPosition.Value.DirectionTo(Projectile.Center).ToRotation() + (Projectile.spriteDirection == -1 ? MathHelper.Pi + 1.4f : -1.4f));
                Owner.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Owner.HandPosition.Value.DirectionTo(Projectile.Center).ToRotation() + (Projectile.spriteDirection == -1 ? MathHelper.Pi + 1.4f : -1.4f));
            }
            if (Owner.controlUseItem)
            {
                if (Projectile.ai[0] <= 0 && Owner.CheckMana(6, true))
                {
                    Projectile.ai[0] = totalTime;
                }
                else
                {
                    Owner.SetDummyItemTime(2);
                }
            }
            Projectile.ai[0]--;
            if (Projectile.ai[0] < 0)
                Projectile.ai[0] = 0;

            int animationSpeed = 6;
            int totalLength = 59;

            if (Projectile.ai[0] == totalLength - animationSpeed * 2)
            {
                SoundEngine.PlaySound(SoundID.AbigailCry with { Pitch = 1f, Volume = 0.6f }, Projectile.Center);
                if (Owner == Main.LocalPlayer)
                {
                    int projCount = Main.rand.Next(3, 7);
                    int randomness = 5;
                    int speed = 11;
                    for (int i = 0; i < projCount; i++)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.DirectionTo(Main.MouseWorld) * speed + Main.rand.NextVector2Circular(randomness, randomness), ModContent.ProjectileType<BrimstoneHomer>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI);
                    }
                }
                Projectile.frame = 1;
            }
            else if (Projectile.ai[0] <= totalLength - animationSpeed * 4)
            {
                Projectile.frame = 0;
            }

        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
    }
}
