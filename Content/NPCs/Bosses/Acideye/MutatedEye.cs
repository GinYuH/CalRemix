using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using CalamityMod.BiomeManagers;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using ReLogic.Content;
using CalamityMod;

namespace CalRemix.Content.NPCs.Bosses.Acideye
{
    public class MutatedEye : ModNPC
    {
        private Player Target => Main.player[NPC.target];
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<PearlAura>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Irradiated>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<SulphuricPoisoning>()] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers nPCBestiaryDrawModifiers = new()
            {
                Scale = 0.5f
            };
            NPCID.Sets.NPCBestiaryDrawModifiers value = nPCBestiaryDrawModifiers;
            value.Frame = 1;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 25;
            NPC.height = 25;
            NPC.LifeMaxNERB(30, 75, 3000);
            NPC.defense = 3;
            NPC.damage = 14;
            NPC.knockBackResist = 0f;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SulphurousSeaBiome>().Type };
        }
        public override void AI()
        {
            NPC.TargetClosest();
            NPC.rotation = NPC.velocity.ToRotation();
            NPC.velocity = (NPC.velocity * 179f + NPC.DirectionTo(Target.Center) * 8f / 1.5f) / 180f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] 
            {
                new FlavorTextBestiaryInfoElement("Babies of the acidsighter. Do not pet them as they're very messy and gross.")
            });
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY), null, drawColor, NPC.rotation, texture.Size() / 2f, NPC.scale, SpriteEffects.FlipHorizontally, 0f);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Acideye/MutatedEye_Glow").Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY), null, new Color(255, 255, 255, 255), NPC.rotation, texture.Size() / 2f, NPC.scale, SpriteEffects.FlipHorizontally, 0f);
        }
    }
}
