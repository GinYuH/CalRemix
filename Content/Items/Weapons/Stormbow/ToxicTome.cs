using System;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Magic;
using CalRemix.Content.DamageClasses;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public class ToxicTome : ModItem, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.UseSound = SlimeGodCore.ShotSound;
            Item.autoReuse = true;
            Item.shootSpeed = 1f;

            Item.width = 20;
            Item.height = 46;
            Item.damage = 67;
            Item.crit = 6;
            Item.useTime = 52;
            Item.useAnimation = 52;

            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.rare = ItemRarityID.LightRed;
            Item.shoot = ModContent.ProjectileType<ChlorineSlammer>();
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // big govt secret: this is actually just a really edited undines retribution. but dont tell anyone that
            // u can edit the i < whatever for extra arrows lool. lol. haha lol
            for (int i = 0; i < 6; i++)
            {
                Vector2 cursorPos = Main.MouseWorld;
                cursorPos.X = player.Center.X + (Main.MouseWorld.X - player.Center.X);
                cursorPos.Y = player.Center.Y - 800 - (100 * (i));
                float speedX = 0;
                float speedY = 3;

                // arrow position noise pass
                cursorPos.X += Main.rand.Next(-60, 61);
                cursorPos.Y += Main.rand.Next(-60, 61);

                int projectile = Projectile.NewProjectile(source, cursorPos.X, cursorPos.Y, speedX, speedY, type, damage, knockback, player.whoAmI, 0.0f);
            }
            return false;
        }
    }
    public class ChlorineSlammer : ModProjectile
    {
        public ref float Mode => ref Projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);

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
            //Projectile.rotation = Projectile.velocity.ToRotation() * MathHelper.PiOver2;
            Projectile.velocity.Y *= 1.1f;
            Particle smoke = new HeavySmokeParticle(Projectile.Center, Projectile.velocity * Main.rand.NextFloat(-0.2f, -0.6f), Color.DarkCyan * 0.65f, 22, Main.rand.NextFloat(0.4f, 0.55f), 0.3f, Main.rand.NextFloat(-0.2f, 0.2f), false, required: true);
            GeneralParticleHandler.SpawnParticle(smoke);
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCHit1, Projectile.position);

            Particle pulse = new DirectionalPulseRing(Projectile.Center + new Vector2(0, Projectile.height / 2), Vector2.Zero, Color.DarkCyan, new Vector2(4f, 2f), Main.rand.NextFloat(-0.2f, 0.2f), 0.01f, 0.25f, 22);
            GeneralParticleHandler.SpawnParticle(pulse);
        }
        public override bool? CanDamage()
        {
            if (Mode == 1)
                return false;

            return null;
        }
    }
}