﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.GameContent;

namespace CalRemix.Content.NPCs.Bosses.Phytogen
{
    public class PhytogenShield : ModNPC
    {
        Vector2 OriginalSize = new Vector2(216, 206);
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Phytogen's Shield");
            this.HideFromBestiary();
            NPCID.Sets.MustAlwaysDraw[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.damage = 200;
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.width = 216;
            NPC.height = 206;
            NPC.defense = 20;
            NPC.lifeMax = 8000;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath8;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
        }

        public override void AI()
        {
            NPC.dontTakeDamage = true;
            if (Main.rand.NextBool(22))
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.JungleSpore);
            NPC carci = Main.npc[(int)NPC.ai[0]];
            if (carci != null && carci.active && carci.type == ModContent.NPCType<Phytogen>())
            {
                NPC.position = carci.Center - NPC.Size / 2;
                NPC.rotation += 0.1f;
                switch (carci.ai[0])
                {
                    case (int)Phytogen.PhaseType.Passive:
                        {
                            NPC.damage = 0;
                            NPC.ai[3] = 0.7f;
                            break;
                        }
                    case (int)Phytogen.PhaseType.Idle:
                        {
                            NPC.damage = 100;
                            NPC.ai[3] = 0.75f;
                            break;
                        }
                    case (int)Phytogen.PhaseType.SapBlobs:
                        {
                            NPC.damage = 100;
                            NPC.ai[3] = 0.8f;
                            break;
                        }
                    case (int)Phytogen.PhaseType.Moving:
                        {
                            NPC.damage = 100;
                            NPC.ai[3] = 1f;
                            break;
                        }
                    case (int)Phytogen.PhaseType.Burrow:
                        {
                            NPC.damage = 100;
                            NPC.ai[3] = 0.5f;
                            break;
                        }
                    case (int)Phytogen.PhaseType.LastStand:
                        {
                            NPC.damage = 0;
                            NPC.ai[3] = 12f;
                            break;
                        }
                }
            }
            else
            {
                NPC.active = false;
            }
            NPC.Size = Vector2.Lerp(NPC.Size, OriginalSize * NPC.ai[3], 0.075f);
            NPC.scale = MathHelper.Lerp(NPC.scale, NPC.ai[3], 0.075f);
        }


        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.JungleSpore, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.JungleSpore, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC carci = Main.npc[(int)NPC.ai[0]];
            if (carci == null || !carci.active || carci.type != ModContent.NPCType<Phytogen>())
                return false;

            Asset<Texture2D> sprite = TextureAssets.Npc[Type];
            Asset<Texture2D> sprite2 = ModContent.Request<Texture2D>(Texture + 2);
            Vector2 npcOffset = NPC.Center - screenPos;
            spriteBatch.Draw(sprite2.Value, npcOffset, null, NPC.GetAlpha(Lighting.GetColor(new Point((int)carci.position.X / 16, (int)carci.position.Y / 16))), -NPC.rotation * 0.8f - 0.3f, sprite.Size() / 2, NPC.scale, SpriteEffects.None, 0);
            spriteBatch.Draw(sprite.Value, npcOffset, null, NPC.GetAlpha(Lighting.GetColor(new Point((int)carci.position.X / 16, (int)carci.position.Y / 16))), NPC.rotation, sprite.Size() / 2, NPC.scale, SpriteEffects.None, 0);
            return false;
        }

        public override bool CheckActive()
        {
            return !NPC.AnyNPCs(ModContent.NPCType<Phytogen>());
        }
    }
}
