using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class BrightMind : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];

        public static SoundStyle talkSound = new SoundStyle("CalRemix/Assets/Sounds/BrightMind") with { PitchVariance = 0.75f };

        public static SoundStyle hitSound = new SoundStyle("CalRemix/Assets/Sounds/BrightMindHit") with { PitchVariance = 0.75f };

        public static SoundStyle deathSound = new SoundStyle("CalRemix/Assets/Sounds/BrightMindDeath");

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 54;
            NPC.height = 80;
            NPC.lifeMax = 2000;
            NPC.damage = 0;
            NPC.defense = 8;
            NPC.friendly = true;
            NPC.noGravity = false;
            NPC.HitSound = hitSound;
            NPC.DeathSound = deathSound;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = false;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<BadlandsBiome>().Type };
        }
        public override void AI()
        {
            NPC.TargetClosest();
            NPC.spriteDirection = NPC.direction;
            if (State == 1)
            {
                Timer++;
                if (Main.mouseLeft && Main.mouseLeftRelease)
                {
                    SoundEngine.PlaySound(talkSound, NPC.Center);
                    CombatText.NewText(NPC.getRect(), Color.Tan, "EFFSAF");
                }
            }
            else
            {
                Timer = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            SpriteEffects fx = NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            float speed = 40f;
            Vector2 scale = Vector2.One + new Vector2(MathF.Cos(Main.GlobalTimeWrappedHourly * speed), MathF.Sin(Main.GlobalTimeWrappedHourly * speed)) * 0.1f;
            if (State != 1)
                scale = Vector2.One;
            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY + texture.Height / 2), null, drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height), scale * NPC.scale, fx, 0f);
            return false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override bool NeedSaving()
        {
            return true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<RustedShardProjectile>())
                return true;
            return false;
        }
    }
}
