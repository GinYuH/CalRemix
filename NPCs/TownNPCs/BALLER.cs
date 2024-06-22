using System;
using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Events;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalRemix.NPCs.TownNPCs
{
    [AutoloadHead]
    public class BALLER : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Archwitch");
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPC.Happiness
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Dryad, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Dislike);
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.lavaImmune = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 20000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.8f;
            AnimationType = NPCID.Guide;
            NPC.noGravity = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("This powerful witch used her ability to manipulate winds to turn the tide of battle during the war with Yharim. Being able to create powerful drafts with just a wave of her hands, she lifted arships into the sky to cover ground much quicker and carry troops to their stations.\r\n")
            });
        }

        public override void AI()
        {
            Microsoft.Xna.Framework.Point p = NPC.Bottom.ToSafeTileCoordinates();
            int groundHeight = 0;
            for (int i = 0; i < 10; i++)
            {
                if (CalamityUtils.ParanoidTileRetrieval(p.X, p.Y + i).HasTile)
                {
                    groundHeight++;
                }
            }
            NPC.position.Y -= groundHeight * 10;
            NPC.rotation = NPC.spriteDirection * -MathHelper.PiOver4;
            NPC.spriteDirection = -NPC.direction;
            NPC.velocity.X = Main.windSpeedCurrent;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs) => RemixDowned.downedOxygen;

        public override List<string> SetNPCNameList() => new List<string>() { "Tempest"};

        public override string GetChat()
        {
            WeightedRandom<string> dialogue = new WeightedRandom<string>();

            dialogue.Add("Feel the breeze, it whispers secrets of the past and murmurs hints of the future. Listen closely, and you may learn something valuable.");
            dialogue.Add("Power is like the wind: invisible, yet capable of reshaping the world. Harness it wisely, and you can achieve greatness.");
            dialogue.Add("Every breeze carries whispers of rebellion. Can you hear them?");

            if (!Main.dayTime)
            {
                dialogue.Add("The night air carries a chill, but also a serenity. In the darkness, we can find peace... if we are brave enough to seek it.");
                dialogue.Add("Under the cover of darkness, the winds seem to have a mind of their own. Stay vigilant; not all that moves in the night is friendly.");
            }

            if (BirthdayParty.PartyIsUp)
                dialogue.Add("Even in times of celebration, the winds carry news. Stay vigilant, but enjoy this respite.");

            if (Main.bloodMoon)
            {
                dialogue.Add("Foul night! The very air is tainted with malevolence. We must endure this crimson curse.");
                dialogue.Add("The Blood Moon rises! Even the winds recoil in fear. Brace yourself, or be swept away!");
            }

            if (Main._shouldUseWindyDayMusic)
            {
                dialogue.Add("A perfect day to harness the gale. The winds favor us; let's use their might wisely.");
                dialogue.Add("The breeze today is invigorating. Perfect for scouting and swift maneuvers.");
            }

            if (CalRemixWorld.oxydayTime > 1)
            {
                dialogue.Add("The gales are relentless today. Even I must tread carefully in this storm.");
                dialogue.Add("The wind's fury is unmatched. Harness it if you can, but beware—it's as wild as it is powerful.");
            }

            return dialogue;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Increase Wind (1 gold)";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                if (Main.LocalPlayer.BuyItem(Item.buyPrice(gold: 1)))
                {
                    Main.windSpeedTarget += 1;
                    SoundEngine.PlaySound(SoundID.CoinPickup);
                }
            }
        }

        public override bool CanGoToStatue(bool toKingStatue) => !toKingStatue;

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 9f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 10;
            randExtraCooldown = 50;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<Tornado>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 2f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Microsoft.Xna.Framework.Color drawColor)
        {
            Vector2 npcOffset = NPC.Center - screenPos + Vector2.UnitY * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3) * 4;
            Texture2D balloons = ModContent.Request<Texture2D>("Terraria/Images/Item_1164").Value;
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D npctex = ModContent.Request<Texture2D>(Texture).Value;
            spriteBatch.Draw(balloons, npcOffset - Vector2.UnitY * 26 - Vector2.UnitX * NPC.spriteDirection * -10, null, NPC.GetAlpha(drawColor), 0f, balloons.Size() / 2, 1f, fx, 0);
            spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, npcOffset, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(npctex.Width / 2, npctex.Height / 2 / Main.npcFrameCount[Type]), NPC.scale, fx, 0);
            return false;
        }
    }
}
