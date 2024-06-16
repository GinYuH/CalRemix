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
using CalRemix.Biomes;

namespace CalRemix.NPCs.PandemicPanic
{
    public class Malignant : ModNPC
    {
        Entity target = null;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Malignant");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 0.5f;
            NPC.aiStyle = -1;
            NPC.damage = 60;
            NPC.width = 80; //324
            NPC.height = 80; //216
            NPC.defense = 68;
            NPC.lifeMax = 600;
            NPC.knockBackResist = 0.75f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            AIType = -1;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.alpha = 128;
            NPC.HitSound = CalamityMod.NPCs.Perforator.PerforatorHeadMedium.HitSound;
            NPC.DeathSound = CalamityMod.NPCs.Perforator.PerforatorHeadMedium.DeathSound;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PandemicPanicBiome>().Type };
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void AI()
        {
            if (target == null || !target.active)
            {
                target = PandemicPanic.BioGetTarget(false, NPC);
            }
            int num1009 = (NPC.ai[0] == 0f) ? 1 : 2;
            int num1010 = (NPC.ai[0] == 0f) ? 60 : 80;
            for (int num1011 = 0; num1011 < 2; num1011++)
            {
                if (Main.rand.Next(3) < num1009)
                {
                    int num1012 = Dust.NewDust(NPC.Center - new Vector2((float)num1010), num1010 * 2, num1010 * 2, 173, NPC.velocity.X * 0.5f, NPC.velocity.Y * 0.5f, 90, default(Color), 1.5f);
                    Main.dust[num1012].noGravity = true;
                    Main.dust[num1012].velocity *= 0.2f;
                    Main.dust[num1012].fadeIn = 1f;
                }
            }
            if (target != null && target.active && !(target is NPC ne && ne.life <= 0))
            {
                NPC.ai[2] += 0.01f;
                if (NPC.ai[2] > 1)
                    NPC.ai[2] = 0;
                NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(target.Center) * 8, 0.5f);
            }
            else
            {
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
                new FlavorTextBestiaryInfoElement("A dark mass of matter that has incredible self-replicating abilities.")
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 position = NPC.Center - Main.screenPosition;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 8);
            Color color = NPC.GetAlpha(Color.Red * 0.6f);
            float scale = NPC.scale;
            for (int i = 0; i < 10; i++)
            {
                Vector2 vector2 = (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() + (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() * 6 * Math.Abs((float)Math.Sin(Main.GlobalTimeWrappedHourly));
                Main.spriteBatch.Draw(texture, position + vector2, NPC.frame, color, NPC.rotation, origin, scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, position, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, origin, scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ShadowbeamStaff, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ShadowbeamStaff, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
