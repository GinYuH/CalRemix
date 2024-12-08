using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using CalRemix.Content.NPCs.Bosses.Pyrogen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class PyrogenTool : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pyro Tool");
            Main.projFrames[Projectile.type] = 6;
        }
        public override string Texture => "CalRemix/Content/NPCs/Bosses/Pyrogen/PyrogenShield";

        public override void SetDefaults()
        {
            Projectile.Calamity().DealsDefenseDamage = true;
            Projectile.width = 20;
            Projectile.height = 10;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240; //killed by hostile projectiles being despawned, as opposed to on timer
            CooldownSlot = ImmunityCooldownID.Bosses;
        }
        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.frame = Main.rand.Next(0, 6);
                Projectile.localAI[0] = 1;
            }


            if (Main.zenithWorld)
            {
                Lighting.AddLight(Projectile.Center, 0.2f, 1.6f, 1.6f);
            }
            else
            {
                Lighting.AddLight(Projectile.Center, 1f, 1.6f, 0f);
            }
            int d = Main.zenithWorld ? DustID.IceTorch : DustID.Torch;
            if (!Main.dedServ)
            {
                if (Main.rand.NextBool(10))
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, d, 0f, 0f);
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 120);
        }
        public override void OnKill(int timeLeft)
        {
            int du = Main.zenithWorld ? DustID.IceTorch : DustID.Torch;
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, du, 0f, 0f);
                d.velocity = new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
            }
        }

        public override bool PreDraw(ref Color drawColor)
        {
            Texture2D sprite = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 npcOffset = Projectile.Center - Main.screenPosition;
            Rectangle frame = sprite.Frame(1, Main.projFrames[Type], 0, Projectile.frame);
            float height = Main.zenithWorld ? 2 : 12;
            float extraRot = Main.zenithWorld ? MathHelper.PiOver4 : 0;

            if (Main.zenithWorld)
            {
                sprite = Projectile.frame switch
                {
                    0 or 1 => TextureAssets.Item[ModContent.ItemType<SnowstormStaff>()].Value,
                    2 or 3 => TextureAssets.Item[ModContent.ItemType<Icebreaker>()].Value,
                    _ => TextureAssets.Item[ModContent.ItemType<Avalanche>()].Value
                };

            }
            if (!Main.zenithWorld)
            Main.EntitySpriteDraw(PyrogenShield.BloomTexture.Value, npcOffset, Main.zenithWorld ? null : frame, Color.White with { A = 0 }, Projectile.rotation, new Vector2(sprite.Width / 2, sprite.Height / height), 1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(sprite, npcOffset, Main.zenithWorld ? null : frame, Projectile.GetAlpha(drawColor), Projectile.rotation - extraRot, new Vector2(sprite.Width / 2, sprite.Height / height), 1f, SpriteEffects.None, 0);

            if (!Main.zenithWorld)
                Main.EntitySpriteDraw(PyrogenShield.Glow.Value, npcOffset, Main.zenithWorld ? null : frame, Color.White, Projectile.rotation, new Vector2(sprite.Width / 2, sprite.Height / height), 1f, SpriteEffects.None, 0);
            return false;
        }
    }
}