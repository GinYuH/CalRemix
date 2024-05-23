using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace CalRemix.NPCs
{
    public class GlisteningSpirit : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Mag => ref NPC.ai[0];
        public override void SetStaticDefaults()
        {
            NPCID.Sets.ImmuneToAllBuffs[Type] = true;
        }
        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 48;
            NPC.lifeMax = 2000;
            NPC.damage = 150;
            NPC.defense = 0;
            NPC.knockBackResist = 0f;
            NPC.value = 0;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit36;
            NPC.DeathSound = SoundID.NPCDeath39;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
            if (Target != null)
            {
                if (Mag < 18)
                    Mag += 0.04f;
                NPC.velocity = NPC.DirectionFrom(Target.Center) * Mag;
            }
        }
        public override void DrawEffects(ref Color drawColor)
        {
            if (Main.rand.NextBool())
            {
                Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.GoldCoin, Scale: 2.5f);
                dust.noGravity = true;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
		        new FlavorTextBestiaryInfoElement("The soul of a greedy person, or maybe the spirit of a gold coin looking for a body. Who cares, money!!!!")
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<Necroplasm>(), 1, 25, 35);
            npcLoot.Add(ItemID.GoldCoin, 1, 70, 90);
            npcLoot.Add(ItemID.PlatinumCoin, 1, 50, 75);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY), null, new Color(255, 255, 255, 100), NPC.rotation, texture.Size() / 2f, NPC.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
