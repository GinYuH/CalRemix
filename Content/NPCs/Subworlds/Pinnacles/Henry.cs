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
using CalRemix.UI;
using CalRemix.Content.Items.Misc;
using CalamityMod;
using Terraria.ID;
using CalamityMod.Sounds;
using CalRemix.Core.Subworlds;

namespace CalRemix.Content.NPCs.Subworlds.Pinnacles
{
    [AutoloadHead]
    public class Henry : DialogueNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];

        public static SoundStyle talkSound = new SoundStyle("CalRemix/Assets/Sounds/HenryTalk") with { PitchVariance = 0.75f };

        public override SoundStyle TextSound => talkSound;

        public override int TextSpeed => 7;

        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 54;
            NPC.height = 60;
            NPC.lifeMax = 2000;
            NPC.damage = 0;
            NPC.defense = 8;
            NPC.friendly = true;
            NPC.noGravity = false;
            NPC.HitSound = CommonCalamitySounds.WulfrumNPCDeathSound with { Pitch = 0.5f };
            NPC.DeathSound = CommonCalamitySounds.WulfrumNPCDeathSound;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = false;
        }

        public override string GetDialogue()
        {
            if (Main.LocalPlayer.Remix().wapUnlocked)
                return "Repeat";
            return "Dialogue";
        }

        public override void OnEnd(string key)
        {
            Main.LocalPlayer.Remix().wapUnlocked = true;
        }

        public override void AI()
        {
            base.AI();
            NPC.TargetClosest(false);
            NPC.spriteDirection = NPC.direction;
            Timer++;
            if (NPC.ai[1] > 0)
            {
                if (Timer > 60)
                {
                    PinnaclesSubworldData.frogPos = new Vector2(-2222, -2222);
                    NPC.active = false;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Texture2D glow = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Pinnacles/Henry_Glow").Value;
            Texture2D eye = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Pinnacles/Henry_Eyes").Value;

            float eyeOff = MathHelper.Lerp(-4, 4, Utils.GetLerpValue(NPC.Left.X, NPC.Right.X, Target.Center.X, true));

            SpriteEffects fx = NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            float speed = 20f;
            Vector2 scale = Vector2.One + new Vector2(MathF.Cos(Main.GlobalTimeWrappedHourly * speed), MathF.Sin(Main.GlobalTimeWrappedHourly * speed)) * 0.025f;
            if (!NPCDialogueUI.NotFinishedTalking(NPC))
                scale = Vector2.One;
            if (NPC.ai[1] > 0)
            {
                scale = Vector2.Lerp(Vector2.One, Vector2.UnitY, Timer / 60f);
            }
            spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY + texture.Height / 2), null, drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height), scale * NPC.scale, fx, 0f);
            spriteBatch.Draw(glow, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY + texture.Height / 2), null, Color.White, NPC.rotation, new Vector2(texture.Width / 2, texture.Height), scale * NPC.scale, fx, 0f);
            spriteBatch.Draw(eye, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY + texture.Height / 2) + eyeOff * Vector2.UnitX, null, Color.White, NPC.rotation, new Vector2(texture.Width / 2, texture.Height), scale * NPC.scale, fx, 0f);
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<RustedShardProjectile>())
                return true;
            return false;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            modifiers.SetMaxDamage(1);
            NPC.ai[1] = 1;
            Timer = 0;
            NPC.dontTakeDamage = true;
            NPC.netUpdate = true;
        }

        public override bool NeedSaving()
        {
            return true;
        }

        public override bool CheckActive()
        {
            return false;
        }
    }
}
