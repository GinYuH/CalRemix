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
using CalRemix.Projectiles.Hostile;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using CalamityMod.Graphics.Primitives;
using CalRemix.NPCs.Bosses.Phytogen;

namespace CalRemix.NPCs.PandemicPanic
{
    public class Dendritiator : ModNPC
    {
        Entity target = null;
        public Entity targeto = null;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dendritiator");
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 0.5f;
            NPC.aiStyle = -1;
            NPC.damage = 60;
            NPC.width = 34; //324
            NPC.height = 34; //216
            NPC.defense = 5;
            NPC.lifeMax = 20000;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            AIType = -1;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.HitSound = CalamityMod.NPCs.Perforator.PerforatorHeadMedium.HitSound;
            NPC.DeathSound = CalamityMod.NPCs.Perforator.PerforatorHeadMedium.DeathSound;
        }

        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 16; i++)
            {
                int x = (int)Main.rand.NextVector2CircularEdge(300, 300).X;
                int y = (int)Main.rand.NextVector2CircularEdge(300, 300).Y;
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + x, (int)NPC.position.Y + y, ModContent.NPCType<DendtritiatorArm>(), ai0: NPC.whoAmI, ai3: Main.rand.Next(120, 240));
            }
        }

        public override void AI()
        {
            Main.npcFrameCount[NPC.type] = 1;
            if (targeto == null || !targeto.active)
            {
                targeto = PandemicPanic.BioGetTarget(true, NPC);
            }
            if (targeto != null && targeto.active && !(targeto is NPC n && n.life <= 0))
            {
                NPC.velocity = NPC.DirectionTo(targeto.Center) * 4;
            }
            else
            {
                NPC.velocity *= 0.98f;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("The one who alerts the rest of the immune system of foreign intruders. It uses its tentacles to drag in invaders too their doom or call in more defenders.")
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
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None;
            for (int i = 0; i < 10; i++)
            {
                Vector2 vector2 = (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() + (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() * 2 * Math.Abs((float)Math.Sin(Main.GlobalTimeWrappedHourly));
                Main.spriteBatch.Draw(texture, position + vector2, NPC.frame, color, NPC.rotation + MathHelper.Pi, origin, scale, fx, 0f);
            }
            Main.spriteBatch.Draw(texture, position, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation + MathHelper.Pi, origin, scale, fx, 0f);
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SpectreStaff, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SpectreStaff, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
