using CalamityMod;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Rogue;
using CalRemix.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class GuardiansWrathProj : ModProjectile
    {
        public Vector2 relativePosition = Vector2.Zero;
        public override string Texture => "CalRemix/Content/Items/Weapons/GuardiansWrath";

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.tileCollide = true;
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            relativePosition = reader.ReadVector2();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(relativePosition);
        }

        public override void AI()
        {
            Projectile.timeLeft = 22;
            Player p = Main.player[Projectile.owner];
            if (Projectile.velocity != Vector2.Zero)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (Projectile.ai[1] > 60)
            {
                Projectile.rotation = 0;
            }
            if (p != null && p.active)
            {
                if (Projectile.velocity == Vector2.Zero)
                {
                    if (p.Hitbox.Intersects(Projectile.Hitbox))
                    {
                        Projectile.Kill();
                    }
                }
                if (p.controlUseTile && !p.CCed && p.HeldItem.type == ModContent.ItemType<GuardiansWrath>() && (Projectile.ai[2] == 1 || Projectile.ai[0] > 0))
                {
                    Projectile.netUpdate = true;
                    int lightningDamage = (int)(Projectile.damage * 5);
                    Vector2 lightningSpawnPosition = Projectile.Center - Vector2.UnitY.RotatedByRandom(0.2f) * 1000f;
                    Vector2 lightningShootVelocity = (Projectile.Center - lightningSpawnPosition + Projectile.velocity * 7.5f).SafeNormalize(Vector2.UnitY) * 15f;
                    int lightning = Projectile.NewProjectile(Projectile.GetSource_FromThis(), lightningSpawnPosition, lightningShootVelocity, ModContent.ProjectileType<StormfrontLightning>(), lightningDamage, 0f, Projectile.owner);
                    SoundEngine.PlaySound(SoundID.Thunder, Projectile.Center);
                    if (Main.projectile.IndexInRange(lightning))
                    {
                        Main.projectile[lightning].CritChance = Projectile.CritChance;
                        Main.projectile[lightning].ai[0] = lightningShootVelocity.ToRotation();
                        Main.projectile[lightning].ai[1] = Main.rand.Next(100);
                        Main.projectile[lightning].DamageType = DamageClass.Melee;
                    }
                    Projectile.ai[0] = 0;
                    Projectile.ai[2] = 2;
                }
                if (Projectile.ai[2] == 2)
                {
                    Projectile.ai[1]++;
                }
                if (Projectile.Distance(p.Center) > 3000)
                {
                    Projectile.ai[2] = 2;
                }
                if (Projectile.ai[1] > 60)
                {
                    Projectile.velocity = Vector2.Lerp(Projectile.Center, p.Center, 0.1f) - Projectile.Center;
                    if (p.Hitbox.Intersects(Projectile.Hitbox))
                    {
                        Projectile.Kill();
                    }
                }
            }
            if (Projectile.ai[0] > 0 && Projectile.ai[2] == 0)
            {
                NPC n = Main.npc[(int)Projectile.ai[0] - 1];
                if (!n.active || n.life < 0 || n.dontTakeDamage)
                {
                    Projectile.ai[2] = 2;
                }
                else
                {
                    Projectile.Center = n.Center - relativePosition;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.netUpdate = true;
                Projectile.ai[0] = target.whoAmI + 1;
                relativePosition = target.Center - Projectile.Center;
                SoundEngine.PlaySound(GuardiansWrath.HitSound, Projectile.Center);
                Projectile.tileCollide = false;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[2] == 0)
            {
                Projectile.netUpdate = true;
                Projectile.ai[2] = 1;
                SoundEngine.PlaySound(GuardiansWrath.HitSound, Projectile.Center);
            }
            Projectile.velocity = Vector2.Zero;
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
            Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition + (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 50, null, Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(value.Width, 0), Projectile.scale, SpriteEffects.None);
            return false;
        }
    }
}