using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalRemix.Content.Items.Potions;
using CalamityMod.Projectiles.Summon;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class TheBealed : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];

        public override void SetStaticDefaults()
        {
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.CountsAsCritter[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 42;
            NPC.height = 24;
            NPC.lifeMax = 5;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.noGravity = false;
            NPC.HitSound = CnidarianJellyfishOnTheString.SlapSound with { Pitch = -0.4f };
            NPC.DeathSound = BetterSoundID.ItemSlapHandSmack;
            NPC.noTileCollide = false;
            NPC.value = Item.buyPrice(silver: 1);
            NPC.friendly = true;
            NPC.chaseable = false;
            NPC.catchItem = (short)ModContent.ItemType<Items.Critters.TheBealed>();
            SpawnModBiomes = new int[5] { ModContent.GetInstance<SealedFieldsBiome>().Type, ModContent.GetInstance<TurnipBiome>().Type, ModContent.GetInstance<BarrensBiome>().Type, ModContent.GetInstance<BadlandsBiome>().Type, ModContent.GetInstance<DarnwoodSwampBiome>().Type };
        }

        public override void AI()
        {
            if (NPC.direction == 0)
            {
                NPC.direction = (int)Main.rand.NextFloatDirection();
            }
            NPC.spriteDirection = NPC.direction;

            if (!NPC.wet)
            {
                Timer++;
                if (Timer > 300)
                {
                    if (Timer % 60 == 0)
                    {
                        NPC.SimpleStrikeNPC(1, 0, noPlayerInteraction: true);
                    }
                }
            }
            else
            {
                Timer = 0;
                NPC.velocity.Y = -1;
            }

            if (Main.rand.NextBool(300))
            {
                NPC.direction *= -1;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<Crimtato>());
        }
    }
}
