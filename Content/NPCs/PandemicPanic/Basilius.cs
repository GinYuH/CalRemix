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
    public class Basilius : ModNPC
    {
        Entity target = null;
        Vector2 prowlPoint = Vector2.Zero;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Basilius");
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 0.5f;
            NPC.aiStyle = -1;
            NPC.damage = 60;
            NPC.width = 160; //324
            NPC.height = 160; //216
            NPC.defense = 68;
            NPC.lifeMax = 10000;
            NPC.knockBackResist = 0f;
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
                target = PandemicPanic.BioGetTarget(false, NPC);
            }
            if (NPC.ai[0] == 0)
            {
                prowlPoint = NPC.position;
                NPC.ai[3] = NPC.whoAmI;
                NPC.realLife = NPC.whoAmI;
                int num4 = 0;
                int num5 = NPC.whoAmI;
                for (int m = 0; m < 22; m++)
                {
                    num4 = NPC.NewNPC(NPC.GetSource_FromThis(), (int)(NPC.position.X + (float)(NPC.width / 2)), (int)(NPC.position.Y + (float)NPC.height), ModContent.NPCType<BasiliusBody>(), NPC.whoAmI);
                    Main.npc[num4].ai[3] = NPC.whoAmI;
                    Main.npc[num4].realLife = NPC.whoAmI;
                    Main.npc[num4].ai[1] = num5;
                    Main.npc[num5].ai[0] = num4;
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num4);
                    num5 = num4;
                }
            }
            CalRemixNPC.WormAI(NPC, 12, 0.25f, target, canFlyByDefault: true, prowlPoint: prowlPoint);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("Vicious strains of bacteria, these chains are known for their ability to heat up bodies to critical degrees.")
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
            Vector2 scale = Vector2.One;
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None;
            for (int i = 0; i < 10; i++)
            {
                Vector2 vector2 = (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() + (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() * 2 * Math.Abs((float)Math.Sin(Main.GlobalTimeWrappedHourly));
                Main.spriteBatch.Draw(texture, position + vector2, NPC.frame, color, NPC.rotation, origin, scale, fx, 0f);
            }
            Main.spriteBatch.Draw(texture, position, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, origin, scale, fx, 0f);
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().phd)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_Gray").Value, position, NPC.frame, Color.Red, NPC.rotation, origin, scale, fx, 0f);
            }
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GemSapphire, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GemSapphire, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.075f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }
        public override bool CheckActive() => !PandemicPanic.IsActive;
    }
}
