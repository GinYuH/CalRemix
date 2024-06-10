using CalamityMod.Dusts;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalamityMod.BiomeManagers;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System;

namespace CalRemix.NPCs.BioWar
{
    public class Platelet : ModNPC
    {
        Entity target = null;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Platlet");
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 0.05f;
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 26;
            NPC.height = 26;
            NPC.defense = 10;
            NPC.lifeMax = 600;
            NPC.knockBackResist = 0.75f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            AIType = -1;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.HitSound = CalamityMod.NPCs.Perforator.PerforatorHeadMedium.HitSound;
            NPC.DeathSound = CalamityMod.NPCs.Perforator.PerforatorHeadMedium.DeathSound;
        }

        public override void AI()
        {
            if (target == null || !target.active || NPC.justHit)
            {
                target = BioWar.BioGetTarget(true, NPC);
            }
            if (NPC.ai[0] == 0)
            {
                NPC.ai[1] = Main.rand.Next(0, 3);
                NPC.velocity = Main.rand.NextVector2Circular(22, 22).SafeNormalize(Vector2.UnitY) * Main.rand.NextFloat(-8, 8);
                NPC.ai[0] = 1;
            }
            else if (NPC.ai[0] == 2)
            {
                NPC.velocity *= 0.97f;
            }
            for (int i = 0; i < 2; i++)
            {
                if (Main.rand.NextBool(3))
                {
                    int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Corruption, NPC.velocity.X * 0.5f, NPC.velocity.Y * 0.5f, 90, default(Color), 1.5f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 0.2f;
                    Main.dust[d].fadeIn = 1f;
                }
            }
            foreach (NPC n in Main.npc)
            {
                if (n == null)
                    continue;
                if (!n.active)
                    continue;
                if (n.whoAmI == NPC.whoAmI)
                    continue;
                if (n.type != NPC.type)
                    continue;
                if (n.getRect().Intersects(NPC.getRect()))
                {
                    float pushForce = 2f;
                    if (n.Center.X < NPC.Center.X)
                    {
                        NPC.position.X += pushForce;
                    }
                    else
                    {
                        NPC.position.X -= pushForce;
                    }
                    if (n.Center.Y < NPC.Center.Y)
                    {
                        NPC.position.Y += pushForce;
                    }
                    else
                    {
                        NPC.position.Y -= pushForce;
                    }
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("When things start tearing apart, you can count on these good ol' globs to glue stuff together.")
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;
            Texture2D texture = NPC.ai[1] == 2 ? ModContent.Request<Texture2D>(Texture+2).Value : NPC.ai[2] == 1 ? ModContent.Request<Texture2D>(Texture+3).Value : TextureAssets.Npc[NPC.type].Value;
            Vector2 position = NPC.Center - Main.screenPosition;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 8);
            Color color = NPC.GetAlpha(Color.Lime * 0.6f);
            Vector2 scale = Vector2.One;
            for (int i = 0; i < 10; i++)
            {
                Vector2 vector2 = (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() + (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() * 2 * Math.Abs((float)Math.Sin(Main.GlobalTimeWrappedHourly));
                Main.spriteBatch.Draw(texture, position + vector2, NPC.frame, color, NPC.rotation, origin, scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, position, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, origin, scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            NPC.ai[0] = 2;
        }
    }
}
