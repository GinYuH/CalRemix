using CalamityMod;
using CalRemix.UI;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System;
using Microsoft.Xna.Framework.Graphics;
//using CalamityMod.CalPlayer;

namespace CalRemix.Content.NPCs.Dinosaurs
{
    public class Mammoth : ModNPC
    {
        public ref float Timer => ref NPC.ai[2];
        public ref float StretchXMult => ref NPC.localAI[0];
        public ref float StretchYMult => ref NPC.localAI[1];

        public static readonly SoundStyle MammothRoar = new("CalRemix/Assets/Sounds/Mammoth")
        {
            PitchVariance = 0.6f,
            MaxInstances = 0
        };
        public static readonly SoundStyle MammothInjured = new("CalRemix/Assets/Sounds/Mammoth")
        {
            PitchRange = (0.6f, 0.9f),
            MaxInstances = 0
        };
        public static readonly SoundStyle MammothDie = new("CalRemix/Assets/Sounds/Mammoth")
        {
            Pitch = -1.3f,
            MaxInstances = 0
        };
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.CountsAsCritter[NPC.type] = true;
            if (Main.dedServ)
                return;
            HelperMessage.New("Mammoth",
                "My goodness, is that a Wooly Mammoth?!? This world must be in the Stone Age! Go get a look while you can, before they go extinct!",
                "FannyAwooga",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type));
        }

        [JITWhenModsEnabled("CalamityMod")]
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Bunny);
            NPC.width = 50;
            NPC.height = 50;
            NPC.lavaImmune = false;
            AIType = NPCID.Bunny;
            NPC.HitSound = MammothInjured;
            NPC.DeathSound = MammothDie;
            NPC.lifeMax = 350000;
            NPC.chaseable = false;
            NPC.knockBackResist = 0.98f;
            NPC.rarity = 5;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
        }
        public override bool? CanBeHitByItem(Player player, Item item) => null;

        public override bool? CanBeHitByProjectile(Projectile projectile) => null;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type], quickUnlock: true);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override bool PreAI()
        {
            // no invisible mammoths
            if (StretchXMult == 0 || StretchYMult == 0)
            {
                StretchXMult = 1;
                StretchYMult = 1;
            }
            
            // do the roar
            if (Main.rand.NextBool(500))
            {
                SoundEngine.PlaySound(MammothRoar, NPC.position);
            }

            // animate if moving
            if (NPC.velocity.X != 0)
            {
                Timer++;
                if ((int)Timer % 2 == 0)
                {
                    StretchXMult = Main.rand.NextFloat(1.2f, 0.5f);
                    StretchYMult = Main.rand.NextFloat(1.2f, 0.5f);
                }
            }

            // face where moving
            int leftOrRight = Math.Sign(NPC.velocity.X);
            if (leftOrRight != 0f)
            {
                NPC.direction = NPC.spriteDirection = leftOrRight;
            }

            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                StretchXMult = 1;
                StretchYMult = 1;
            }
            
            ReLogic.Content.Asset<Texture2D> texture = ModContent.Request<Texture2D>(Texture);
            SpriteEffects flip = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.EntitySpriteDraw(texture.Value, NPC.Bottom - Main.screenPosition, null, drawColor, NPC.rotation, texture.Size() * new Vector2(0.5f, 1f), NPC.scale * new Vector2(StretchXMult, StretchYMult), flip, 0);
            return false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneSnow)
            {
                return Terraria.ModLoader.Utilities.SpawnCondition.OverworldDaySnowCritter.Chance * 0.4f;
            }
            return 0f;
        }
    }
}