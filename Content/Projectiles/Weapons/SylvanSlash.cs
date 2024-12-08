using System;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalRemix.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class SylvanSlash : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 4;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            Main.projFrames[Type] = 28;
        }

        public override void SetDefaults()
        {
            Projectile.width = 85;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.timeLeft = 10000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.noEnchantmentVisuals = true;
        }

        public ref float Time => ref Projectile.ai[0];
        public ref float Mode => ref Projectile.ai[1];
        public ref float StickHost => ref Projectile.ai[2];

        public ref Player Owner => ref Main.player[Projectile.owner];

        public override void AI()
        {
            if (Owner.HeldItem.type != ModContent.ItemType<SylvanSlasher>() || !Owner.active || Owner.dead || Owner.noItems || Owner.CCed)
            {
                Projectile.active = false;
            }

            Owner.heldProj = Projectile.whoAmI;

            Projectile.frame++;
            if (Projectile.frame >= 28)
            {
                Projectile.frame = 1;
            }

            // epic dust (code ripped from the og sylvan slasher bcuz im lazy)
            Vector2 vector14 = Projectile.Center + Projectile.velocity * 3f;
            if (Main.rand.NextBool(3))
            {
                int num30 = Dust.NewDust(vector14 - Projectile.Size / 2f, Projectile.width, Projectile.height, DustID.KryptonMoss, Projectile.rotation * 2f, Projectile.velocity.Y, 100, default, 2f);
                Main.dust[num30].noGravity = true;
                Main.dust[num30].position -= Projectile.velocity;
            }
            Lighting.AddLight(vector14, 0.2f, 2f, 3f);

            // the position we want our wand to go to
            Vector2 idealPosition = Owner.Center - Owner.DirectionTo(Main.MouseWorld) * Owner.Distance(Main.MouseWorld);
            // move towards the ideal position snappy
            // safety applied because you can put ur cursor over the players center and the projectile gets nan'd
            Projectile.velocity += Projectile.SafeDirectionTo(idealPosition) * (Projectile.Distance(idealPosition) * 0.1f);
            Projectile.velocity *= 0.7f;
            // account for player movement a little bit so it doesnt lag behind as much
            Projectile.velocity += Owner.velocity * 0.2f;
            // point towards the cursor, relative to the player 
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Owner.MountedCenter.DirectionTo(idealPosition).ToRotation(), 0.5f);

            Time++;
        }

        // also stolen from calamity modzing zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            CalamityPlayer modPlayer = Owner.Calamity();
            if (Projectile.owner == Main.myPlayer)
            {
                if (Projectile.ai[0] <= 0f)
                {
                    if ((target.damage > 5 || target.boss) && !target.SpawnedFromStatue)
                    {
                        if (modPlayer.wearingRogueArmor && modPlayer.rogueStealthMax != 0)
                        {
                            if (modPlayer.rogueStealth < modPlayer.rogueStealthMax)
                            {
                                modPlayer.rogueStealth += 0.05f;
                                if (modPlayer.rogueStealth > modPlayer.rogueStealthMax)
                                    modPlayer.rogueStealth = modPlayer.rogueStealthMax;
                            }
                        }
                    }
                }
            }
        }
    }
}