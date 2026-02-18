using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalRemix.Content.Items.Potions;
using CalamityMod.NPCs.Abyss;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Boss;

namespace CalRemix.Content.NPCs.Subworlds.Carboniforest
{
    public class LargeStinkbug : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = NPCAIStyleID.Herpling;
            NPC.damage = 200;
            NPC.width = 32;
            NPC.height = 36;
            NPC.defense = 12;
            NPC.lifeMax = 5000;
            NPC.knockBackResist = 0.8f;
            NPC.value = Item.buyPrice(silver: 50);
            NPC.noGravity = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath17;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToCold = true;
        }

        public override void AI()
        {
            NPC.spriteDirection = NPC.velocity.X.DirectionalSign();
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 8)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y > frameHeight)
            {
                NPC.frame.Y = 0;
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
            npcLoot.Add(ItemID.Stinkbug, 1, 1, 3);
        }

        public override void OnKill()
        {
            for (int i = 0; i < 16; i++)
            {
                Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(0.1f, 0.3f), ModContent.ProjectileType<SandPoisonCloud>(), CalRemixHelper.ProjectileDamage(200, 320), 1);
            }
        }
    }
}
