﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.GameContent;
using CalamityMod.World;
using CalRemix.Content.Projectiles.Hostile;
using Terraria.Audio;

namespace CalRemix.Content.NPCs.Bosses.Carcinogen
{
    public class CarcinogenShield : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Carcinogen's Shield");
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.damage = 30;
            NPC.width = 170;
            NPC.height = 166;
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
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch);
            NPC.dontTakeDamage = true;
            NPC carci = Main.npc[(int)NPC.ai[0]];
            if (carci != null && carci.active && carci.type == ModContent.NPCType<Carcinogen>())
            {
                NPC.position = carci.Center - NPC.Size / 2;
                NPC.rotation += 0.1f;

                bool blender = carci.ai[0] == 2 && carci.ai[2] == 1; // If Carcinogen is doing his fire spin attack
                int cinderSpeed = blender ? 10 : 6; // Speed of cinder projectiles, increases during blender
                int cinderRate = blender ? 10 : 60; // Rate at which cinders are spawned
                if (carci.ai[0] != 1)
                {
                    // If Carci is doing the blender, he is below 50% health, or it's revengeance mode, randomly fling out cinders
                    if ((CalamityWorld.revenge || carci.life / carci.lifeMax <= 0.5f) || (carci.ai[0] == 2 && carci.ai[2] == 1))
                    {
                        if (Main.rand.NextBool(cinderRate))
                        {
                            SoundEngine.PlaySound(SoundID.Item20 with { Volume = 0.2f, Pitch = 0.4f }, NPC.Center);
                            Vector2 square = new Vector2(Main.rand.Next((int)NPC.position.X, (int)NPC.position.X + NPC.width), Main.rand.Next((int)NPC.position.Y, (int)NPC.position.Y + NPC.height));
                            int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), square, new Vector2(Main.rand.Next(-cinderSpeed, cinderSpeed), Main.rand.Next(-cinderSpeed, 0)), ModContent.ProjectileType<CigarCinder>(), (int)(NPC.damage * 0.25f), 0f, Main.myPlayer);
                            Main.projectile[p].scale = Main.rand.NextFloat(1f, 2f);
                        }
                    }
                }
            }
            // Die if the parent is not an active Carcinogen
            else
            {
                NPC.active = false;
            }
        }


        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Dirt, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Dirt, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC carci = Main.npc[(int)NPC.ai[0]];
            if (carci == null || !carci.active || carci.type != ModContent.NPCType<Carcinogen>())
                return false;

            Asset<Texture2D> sprite = TextureAssets.Npc[Type];
            Texture2D cigars = ModContent.Request<Texture2D>(Texture + "Cigars").Value;
            Vector2 npcOffset = NPC.Center - screenPos;
            // <3 additive
            spriteBatch.EnterShaderRegion(BlendState.Additive);
            spriteBatch.Draw(sprite.Value, npcOffset, null, NPC.GetAlpha(Lighting.GetColor(new Point((int)carci.position.X / 16, (int)carci.position.Y / 16))), NPC.rotation, sprite.Size() / 2, 1f, SpriteEffects.None, 0);
            spriteBatch.ExitShaderRegion();
            spriteBatch.Draw(cigars, npcOffset, null, NPC.GetAlpha(Lighting.GetColor(new Point((int)carci.position.X / 16, (int)carci.position.Y / 16))), NPC.rotation, sprite.Size() / 2, 1f, SpriteEffects.None, 0);
            return false;
        }
        public override bool CheckActive() => !(Main.npc[(int)NPC.ai[0]] != null && Main.npc[(int)NPC.ai[0]].active && Main.npc[(int)NPC.ai[0]].type == ModContent.NPCType<Carcinogen>());
    }
}
