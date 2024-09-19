using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Buffs.StatDebuffs;
using System.IO;
using Microsoft.Xna.Framework;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using CalamityMod.Sounds;
using CalRemix.Core.Biomes;

namespace CalRemix.Content.NPCs.Bosses.Losbaf
{
    [AutoloadBossHead]
    public class Losbaf : ModNPC
    {
        private Player Target => Main.player[NPC.target];
        public ref float Phase => ref NPC.ai[0];
        public ref float Timer => ref NPC.ai[1];
        public ref float Attack => ref NPC.ai[2];
        public ref float Step => ref NPC.ai[3];
        public int eyeFrame = 0;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(eyeFrame);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            eyeFrame = reader.ReadInt32();
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 3;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<PearlAura>()] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers nPCBestiaryDrawModifiers = new()
            {
                Scale = 0.5f
            };
            NPCID.Sets.NPCBestiaryDrawModifiers value = nPCBestiaryDrawModifiers;
        }
        public override void SetDefaults()
        {
            NPC.boss = true;
            NPC.aiStyle = -1;
            NPC.width = 110;
            NPC.height = 110;
            NPC.lifeMax = 4000;
            NPC.defense = 10;
            NPC.damage = 25;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(platinum: 20);
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = CommonCalamitySounds.ExoHitSound;
            NPC.DeathSound = CommonCalamitySounds.ExoDeathSound;
            NPC.Calamity().canBreakPlayerDefense = true;
            SpawnModBiomes = [ModContent.GetInstance<ExosphereBiome>().Type];
            if (!Main.dedServ && ModLoader.HasMod("CalamityModMusic"))
                Music = MusicLoader.GetMusicSlot("CalamityModMusic/Sounds/Music/ExoMechs");
        }
        public override void AI()
        {
            NPC.localAI[0]++;
            if (NPC.localAI[0] >= 6)
            {
                if (eyeFrame > 4 || eyeFrame < 0)
                    eyeFrame = 0;
                else
                    eyeFrame++;
                NPC.localAI[0] = 0;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 120);
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 6)
            {
                if (NPC.frame.Y > frameHeight || NPC.frame.Y < 0)
                    NPC.frame.Y = 0;
                else
                    NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("A mech designed after the Fabrication Quartet. Despite its low intelligence, its raw power proves to exceed its predecessors.")
            });
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Rectangle rect = new(0, NPC.frame.Y, texture.Width, texture.Height / Main.npcFrameCount[Type]);
            spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY), rect, drawColor, NPC.rotation, rect.Size() / 2f, NPC.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D glow = ModContent.Request<Texture2D>($"{nameof(CalRemix)}/NPCs/Bosses/Losbaf/LosbafGlow", AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(glow, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY), null, new Color(255, 255, 255, 255), NPC.rotation, glow.Size() / 2f, NPC.scale, SpriteEffects.None, 0f);

            Texture2D eyes = ModContent.Request<Texture2D>($"{nameof(CalRemix)}/NPCs/Bosses/Losbaf/LosbafEyes", AssetRequestMode.ImmediateLoad).Value;
            Rectangle rect = new(0, eyeFrame * (eyes.Height / 6), eyes.Width / 2, eyes.Height / 6);
            spriteBatch.Draw(eyes, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY), rect, new Color(255, 255, 255, 255), NPC.rotation, rect.Size() / 2f, NPC.scale, SpriteEffects.None, 0f);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
        }
        public override void OnKill()
        {
        }
    }
}
