using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.Summon;
using CalamityMod.CalPlayer;
using CalRemix.Content.Buffs;
using CalRemix.Content.NPCs.Bosses.Pyrogen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class MoltenTools : ModProjectile
    {
        public override string Texture => "CalRemix/Content/NPCs/Bosses/Pyrogen/PyrogenShield";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 52;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.frame = Main.rand.Next(0, 6);
                Projectile.localAI[0] = 1;
            }
            Lighting.AddLight(Projectile.Center, TorchID.Orange);
            Player player = Main.player[Projectile.owner];
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            bool isMinion = Projectile.type == ModContent.ProjectileType<MoltenTools>();
            player.AddBuff(ModContent.BuffType<MoltenToolsBuff>(), 3600);
            if (isMinion)
            {
                if (player.dead)
                {
                    modPlayer.moltool = false;
                }
                if (modPlayer.moltool)
                {
                    Projectile.timeLeft = 2;
                }
            }
            Projectile.rotation += 0.075f;

            Projectile.ChargingMinionAI(2500f, 2800f, 3000f, 500f, 1, 20f, 30f, 17f, new Vector2(0f, -60f), 20f, 18f, true, true);
        }


        public override bool PreDraw(ref Color drawColor)
        {
            Texture2D sprite = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 npcOffset = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(PyrogenShield.BloomTexture.Value, npcOffset, sprite.Frame(1, Main.projFrames[Type], 0, Projectile.frame), Color.White with { A = 0 }, Projectile.rotation, new Vector2(sprite.Width / 2, sprite.Height / 12), 1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(sprite, npcOffset, sprite.Frame(1, Main.projFrames[Type], 0, Projectile.frame), Projectile.GetAlpha(drawColor), Projectile.rotation, new Vector2(sprite.Width / 2, sprite.Height / 12), 1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(PyrogenShield.Glow.Value, npcOffset, sprite.Frame(1, Main.projFrames[Type], 0, Projectile.frame), Color.White, Projectile.rotation, new Vector2(sprite.Width / 2, sprite.Height / 12), 1f, SpriteEffects.None, 0);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 60);
        }
    }
}
