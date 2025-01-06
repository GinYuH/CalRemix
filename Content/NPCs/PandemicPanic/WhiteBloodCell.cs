using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System;
using CalRemix.Core.Biomes;

namespace CalRemix.Content.NPCs.PandemicPanic
{
    public class WhiteBloodCell : ModNPC
    {
        Entity target = null;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("White Blood Cell");
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 0.5f;
            NPC.aiStyle = -1;
            NPC.damage = 60;
            NPC.width = 40; //324
            NPC.height = 40; //216
            NPC.defense = 68;
            NPC.lifeMax = 1000;
            NPC.knockBackResist = 0.75f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            AIType = -1;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.HitSound = CalamityMod.NPCs.Perforator.PerforatorHeadMedium.HitSound;
            NPC.DeathSound = CalamityMod.NPCs.Perforator.PerforatorHeadMedium.DeathSound;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PandemicPanicBiome>().Type };
        }

        public override void AI()
        {
            if (target == null || !target.active)
            {
                target = PandemicPanic.BioGetTarget(true, NPC);
            }
            if (target != null && target.active && !(target is NPC ne && ne.life <= 0))
            {
                NPC.ai[2] += 0.03f;
                if (NPC.ai[2] > 18)
                    NPC.ai[2] = 0;
                NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(target.Center) * MathHelper.Clamp(NPC.ai[2], 4, 18), 0.5f);
            }
            else
            {
                NPC.ai[2] -= 0.3f;
                NPC.velocity *= 0.98f;
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
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
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
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().phd)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_Graypng").Value, position, NPC.frame, Color.Lime, NPC.rotation, origin, scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
