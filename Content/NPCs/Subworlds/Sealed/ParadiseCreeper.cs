using CalamityMod;
using CalRemix.Content.Items.Potions;
using CalRemix.Content.Items.Weapons;
using CalRemix.Core.Biomes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
//using CalamityMod.CalPlayer;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class ParadiseCreeper : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.width = 52;
            NPC.height = 38;
            AIType = -1;
            NPC.lavaImmune = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.lifeMax = 500;
            NPC.defense = 3;
            NPC.damage = 20;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToHeat = true;
            SpawnModBiomes = [GetInstance<CarnelianForestBiome>().Type];
        }

        public override void AI()
        {
            NPC.target = -1;
            if (NPC.direction == 0)
            {
                NPC.direction = (int)Main.rand.NextFloatDirection();
            }
            NPC.spriteDirection = NPC.direction = NPC.velocity.X.DirectionalSign();

            if (NPC.velocity.Y == 0)
            {
                NPC.velocity.X = 0;
                if (Main.rand.NextBool(30))
                {
                    NPC.velocity.X = Main.rand.NextFloatDirection() * 6;
                    NPC.velocity.Y = -4;
                }
                if (Main.rand.NextBool(15))
                {
                    NPC.direction *= -1;
                }
            }
            
            if (NPC.velocity.Y < 0)
            {
                NPC.rotation = MathHelper.Min(NPC.rotation + 0.1f, MathHelper.PiOver4);
            }
            else if (NPC.velocity.Y > 0)
            {
                NPC.rotation = MathHelper.Max(NPC.rotation - 0.1f, -MathHelper.PiOver4);
            }
            else
            {
                NPC.rotation = Utils.AngleLerp(NPC.rotation, 0, 0.05f);
            }
            if (Main.rand.NextBool(6000))
            {
                SoundEngine.PlaySound(BetterSoundID.ItemBubblePop with { Volume = 3f }, NPC.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Item.NewItem(NPC.GetSource_FromThis(), NPC.getRect(), ItemType<Butter>());
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
            Texture2D tex = TextureAssets.Npc[Type].Value;
            float rotOff = 0; // NPC.spriteDirection == -1 ? MathHelper.Pi : 0;
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, NPC.GetAlpha(drawColor), NPC.rotation + rotOff, tex.Size() / 2, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            return false;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.AddIf(() => NPC.buffTime[0] == 0, ItemType<Butter>());
            npcLoot.AddIf(() => NPC.buffTime[0] > 0, ItemType<BurntButter>());
            npcLoot.Add(ItemType<GildedGauntlet>(), 10);
        }
    }
}