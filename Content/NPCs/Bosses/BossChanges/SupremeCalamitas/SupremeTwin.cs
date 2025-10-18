using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;
using CalamityMod.Projectiles.Boss;

namespace CalRemix.Content.NPCs.Bosses.BossChanges.SupremeCalamitas
{
    public class SupremeTwin : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float Twin => ref NPC.ai[1];
        public ref float Timer2 => ref NPC.ai[2];
        public enum TwinType
        {
            Retinazer = 0,
            Spaztismazm = 1
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 142;
            NPC.height = 186;
            NPC.lifeMax = 500000;
            NPC.damage = 50;
            NPC.defense = 16;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.boss = true;
            NPC.noTileCollide = false;
            NPC.HitSound = CalamityMod.NPCs.SupremeCalamitas.SupremeCalamitas.BrotherHit;
            NPC.DeathSound = CalamityMod.NPCs.SupremeCalamitas.SupremeCalamitas.BrotherDeath;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            // spawn the other guy if one guy is already alive
            if (NPC.CountNPCS(ModContent.NPCType<SupremeTwin>()) > 1)
            {
                Twin = (int)TwinType.Spaztismazm;
            }
            else
            {
                Twin = (int)TwinType.Retinazer;
            }

            // awesome random timer start
            Timer = Main.rand.Next(240, 360);
            Timer2 = 120;
        }
        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            if (NPC.FindFirstNPC(ModContent.NPCType<CalamityMod.NPCs.SupremeCalamitas.SupremeCalamitas>()) == -1)
            {
                NPC.defense = 0;
                NPC.takenDamageMultiplier = 1f;
            }
            else 
            {
                NPC.defense = 999999999;
                NPC.takenDamageMultiplier = 0f;
            }

            NPC.rotation = NPC.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver2;

            #region Attack
            if (Timer <= 0)
            {
                Vector2 velocity = NPC.DirectionTo(player.Center) * 4;
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, velocity, ModContent.ProjectileType<BrimstoneHellblast>(), 200, 0, Main.myPlayer, 0f, 2f);

                Timer = Main.rand.Next(120, 240);
            }

            if (Timer2 <= 0)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0, 4), ModContent.ProjectileType<BrimstoneHellblast>(), 200, 0, Main.myPlayer, 0f, 2f);

                Timer2 = 120;
            }

            Timer--;
            Timer2--;
            #endregion

            #region Movement
            Vector2 desiredPosition = new Vector2(player.Center.X + 550, player.Center.Y - 450f);

            // code procured from the original supreme cataclysm using a dark and cursed spell
            // it is incomprehensible to the mortal man, and to myself
            float num676 = 60f;
            float num677 = 1f;
            Vector2 vector83 = new Vector2(NPC.Center.X, NPC.Center.Y);
            float num678 = player.Center.X - vector83.X + 550f;
            if (Twin == (int)TwinType.Retinazer)
            {
                num678 = player.Center.X - vector83.X - 550f;
                desiredPosition.X = player.Center.X - 550;
            }
            float num679 = player.Center.Y - vector83.Y - 450f;
            float num680 = (float)Math.Sqrt((double)(num678 * num678 + num679 * num679));
            num680 = num676 / num680;
            num678 *= num680;
            num679 *= num680;
            if (NPC.velocity.X < num678)
            {
                NPC.velocity.X = NPC.velocity.X + num677;
                if (NPC.velocity.X < 0f && num678 > 0f)
                {
                    NPC.velocity.X = NPC.velocity.X + num677;
                }
            }
            else if (NPC.velocity.X > num678)
            {
                NPC.velocity.X = NPC.velocity.X - num677;
                if (NPC.velocity.X > 0f && num678 < 0f)
                {
                    NPC.velocity.X = NPC.velocity.X - num677;
                }
            }
            if (NPC.velocity.Y < num679)
            {
                NPC.velocity.Y = NPC.velocity.Y + num677;
                if (NPC.velocity.Y < 0f && num679 > 0f)
                {
                    NPC.velocity.Y = NPC.velocity.Y + num677;
                }
            }
            else if (NPC.velocity.Y > num679)
            {
                NPC.velocity.Y = NPC.velocity.Y - num677;
                if (NPC.velocity.Y > 0f && num679 < 0f)
                {
                    NPC.velocity.Y = NPC.velocity.Y - num677;
                }
            }

            if (NPC.Distance(desiredPosition) > 500f)
            {
                NPC.noTileCollide = true;
            }
            if (NPC.noTileCollide == true && NPC.Distance(desiredPosition) < 50f)
            {
                NPC.noTileCollide = false;
            }
            #endregion
        }

        public override void ModifyTypeName(ref string typeName)
        {
            typeName = Twin == (int)TwinType.Retinazer ? "Guarflamis" : "Guarluxis";
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Texture2D glowmask = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/BossChanges/SupremeCalamitas/SupremeTwinGlow").Value;
            int whichTwin = Twin == (int)TwinType.Retinazer ? 0 : 1;
            spriteBatch.Draw(texture, NPC.Center - screenPos, texture.Frame(2, 1, whichTwin, 0), drawColor, NPC.rotation, new Vector2(texture.Width / 4, texture.Height / 2), NPC.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(glowmask, NPC.Center - screenPos, glowmask.Frame(2, 1, whichTwin, 0), Color.White, NPC.rotation, new Vector2(glowmask.Width / 4, glowmask.Height / 2), NPC.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
