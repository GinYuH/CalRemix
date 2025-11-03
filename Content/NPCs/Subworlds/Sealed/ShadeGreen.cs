using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.UI;
using CalRemix.Content.Particles;
using Terraria.ID;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Items.Materials;
using CalRemix.Core.World;
using Microsoft.Xna.Framework.Input;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Sounds;
using CalRemix.Content.Items.Misc;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    // The dialogue code for these guys is fucking atrocious and I keep thinking about Undertale, remind me to never make a NPC-dependent dialogue system that supports multiple characters ever again
    public class ShadeGreen : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];

        public NPC BlueShade
        {
            get => Main.npc[(int)NPC.ai[2]];
            set => NPC.ai[2] = value.whoAmI;
        }

        public NPC YellowShade
        {
            get => Main.npc[(int)NPC.ai[3]];
            set => NPC.ai[3] = value.whoAmI;
        }

        public static Vector2 texOffset = new Vector2();


        public static SoundStyle talkSound = new SoundStyle("CalRemix/Assets/Sounds/BrightMind") with { PitchVariance = 0.75f, Pitch = -2 };

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = Texture + "_Bestiary",
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 100;
            NPC.height = 200;
            NPC.lifeMax = 2000;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.friendly = true;
            NPC.noGravity = true;
            NPC.HitSound = null;
            NPC.DeathSound = null;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = false;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VoidForestBiome>().Type };
        }

        public static void IncrementShadeQuest()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                CalRemixWorld.shadeQuestLevel++;

                ModPacket packet = CalRemix.instance.GetPacket();
                packet.Write((byte)RemixMessageType.ShadeQuestIncrement);
                packet.Write(CalRemixWorld.shadeQuestLevel + 1);
                packet.Send();
            }
            else
            {
                CalRemixWorld.shadeQuestLevel++;
            }
        }

        public static string CheckForItem(Player player, int type, int questLevelRequired, string newDialogue, string repeatDialogue)
        {
            bool inc = false;
            if ((player.HasItem(type) && CalRemixWorld.shadeQuestLevel == questLevelRequired) || (CalRemixWorld.shadeQuestLevel == questLevelRequired + 1))
            {
                if (player.HasItem(type))
                {
                    inc = true;
                }
                player.ConsumeItem(ModContent.ItemType<AbnormalEye>());
                string ret = newDialogue;
                if (NPCDialogueUI.HasReadDialogue(player, "ShadeGreen", repeatDialogue))
                {
                    ret = repeatDialogue;
                }
                if (CalRemixWorld.shadeQuestLevel == questLevelRequired && inc)
                    IncrementShadeQuest();
                return ret;
            }
            return "";
        }


        public override void AI()
        {
            NPC.TargetClosest();
            NPC.spriteDirection = NPC.direction;
            bool blueExists = true;
            bool yellowExists = true;
            int blueIdx = NPC.FindFirstNPC(ModContent.NPCType<ShadeBlue>());
            int yellowIdx = NPC.FindFirstNPC(ModContent.NPCType<ShadeYellow>());

            bool readDialogueOne = NPCDialogueUI.HasReadDialogue(Target, "ShadeGreen.Intro4");
            bool readDialogueTwo = NPCDialogueUI.HasReadDialogue(Target, "ShadeGreen.Mind2");
            bool readDialogueThree = NPCDialogueUI.HasReadDialogue(Target, "ShadeGreen.Cultist2");

            // Repeat dialogue if clicked on
            if (NPC.type == ModContent.NPCType<ShadeGreen>())
            {
                Rectangle maus = Utils.CenteredRectangle(Main.MouseWorld, Vector2.One * 10);
                if (maus.Intersects(NPC.getRect()))
                {
                    if (Target.whoAmI == Main.myPlayer && Main.LocalPlayer.controlUseTile && State == 0 && Main.LocalPlayer.Remix().talkedNPC == -1 && Main.LocalPlayer.Distance(NPC.Center) < 600)
                    {
                        string key = readDialogueOne ? "Intro4" : "Intro1";
                        string newd = CheckForItem(Target, ModContent.ItemType<AbnormalSample>(), 0, "Mind1", "Mind2");
                        key = newd == "" ? key : newd;
                        string newd2 = CheckForItem(Target, ModContent.ItemType<TanMatter>(), 1, "Cultist1", "Cultist2");
                        key = newd2 == "" ? key : newd2;
                        string newd3 = CheckForItem(Target, ModContent.ItemType<PusSac>(), 2, "QuestEnd", "QuestEnd");
                        key = newd3 == "" ? key : newd3;
                        NPCDialogueUI.StartDialogue(NPC.whoAmI, key);

                        State = 1;
                    }
                }
            }

            #region Spwan Shades
            if (!NPC.AnyNPCs(ModContent.NPCType<ShadeBlue>()))
            {
                blueExists = false;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    BlueShade = Main.npc[NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X - 200, (int)NPC.Bottom.Y, ModContent.NPCType<ShadeBlue>())];
                }
            }
            else if (BlueShade.whoAmI != blueIdx)
            {
                BlueShade = Main.npc[blueIdx];
            }
            if (!NPC.AnyNPCs(ModContent.NPCType<ShadeYellow>()))
            {
                yellowExists = false;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    YellowShade = Main.npc[NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X + 200, (int)NPC.Bottom.Y, ModContent.NPCType<ShadeYellow>())];
                }
            }
            else if (YellowShade.whoAmI != yellowIdx)
            {
                YellowShade = Main.npc[yellowIdx];
            }
            #endregion


            if (blueExists && yellowExists)
            {
                if (NPC.Distance(Main.player[NPC.target].Center) < 600)
                {
                    if (State == 0 && !readDialogueOne)
                    {
                        State = 1;
                        NPCDialogueUI.StartDialogue(NPC.whoAmI, "Intro1");
                    }
                }
                else
                {
                    State = 0;
                }

                if (State >= 1)
                {
                    Timer++;
                    if (NPCDialogueUI.NotFinishedTalking(NPC) && Timer % 7 == 0)
                    {
                        SoundEngine.PlaySound(talkSound, NPC.Center);
                    }
                    if (NPCDialogueUI.NotFinishedTalking(YellowShade) && Timer % 7 == 0)
                    {
                        SoundEngine.PlaySound(talkSound with { Pitch = -3 }, NPC.Center);
                    }
                    if (NPCDialogueUI.NotFinishedTalking(BlueShade) && Timer % 7 == 0)
                    {
                        SoundEngine.PlaySound(talkSound with { Pitch = -1.5f }, NPC.Center);
                    }

                    NPC GreenShade = NPC;
                    // Dialogue flow
                    if (!readDialogueOne)
                    {
                        switch (State)
                        {
                            case 1: SwitchShade(YellowShade, "Intro1"); break;
                            case 2: SwitchShade(BlueShade, "Intro1"); break;
                            case 3: SwitchShade(GreenShade, "Intro2"); break;
                            case 4: SwitchShade(BlueShade, "Intro2"); break;
                            case 5: SwitchShade(GreenShade, "Intro3"); break;
                            case 6: SwitchShade(BlueShade, "Intro3"); break;
                            case 7: SwitchShade(YellowShade, "Intro1"); break;
                            case 8: SwitchShade(GreenShade, "Intro4"); break;
                            case 9:
                                {
                                    if (Target.Remix().talkedNPC == -1)
                                    {
                                        Target.QuickSpawnItem(NPC.GetSource_FromThis(), ModContent.ItemType<RustedShard>());
                                        State = 0;
                                    }
                                }
                                break;
                        }
                    }
                    else if (!readDialogueTwo && CalRemixWorld.shadeQuestLevel == 1)
                    {
                        switch (State)
                        {
                            case 1: SwitchShade(BlueShade, "Mind1"); break;
                            case 2: SwitchShade(YellowShade, "Intro1"); break;
                            case 3: SwitchShade(GreenShade, "Mind2"); break;
                            case 4:
                                {
                                    if (Target.Remix().talkedNPC == -1)
                                    {
                                        Target.QuickSpawnItem(NPC.GetSource_FromThis(), ModContent.ItemType<RustedShard>());
                                        State = 0;
                                    }
                                }
                                break;
                        }
                    }
                    else if (!readDialogueThree && CalRemixWorld.shadeQuestLevel == 2)
                    {
                        switch (State)
                        {
                            case 1: SwitchShade(BlueShade, "Cultist1"); break;
                            case 2: SwitchShade(YellowShade, "Cultist1"); break;
                            case 3: SwitchShade(GreenShade, "Cultist2"); break;
                            case 4:
                                {
                                    if (Target.Remix().talkedNPC == -1)
                                    {
                                        Target.QuickSpawnItem(NPC.GetSource_FromThis(), ModContent.ItemType<RustedShard>());
                                        State = 0;
                                    }
                                }
                                break;
                        }
                    }
                    else if (CalRemixWorld.shadeQuestLevel == 3)
                    {
                        switch (State)
                        {
                            case 1: SwitchShade(BlueShade, "QuestEnd"); break;
                            case 2: SwitchShade(YellowShade, "Intro1"); break;
                            case 3: SwitchShade(GreenShade, "QuestEnd"); break;
                            case 4:
                                {
                                    SoundEngine.PlaySound(CommonCalamitySounds.LightningSound);
                                    for (int i = 0; i < 110; i++)
                                    {
                                        VoidMetaballYellow.SpawnParticle(NPC.Center, Main.rand.NextVector2Circular(1f, 1f) * Main.rand.NextFloat(16, 26) * 2, Main.rand.NextFloat(50, 130));
                                    }
                                    for (int i = 0; i < 110; i++)
                                    {
                                        VoidMetaballYellow.SpawnParticle(BlueShade.Center, Main.rand.NextVector2Circular(1f, 1f) * Main.rand.NextFloat(16, 26) * 2, Main.rand.NextFloat(50, 130));
                                    }
                                    BlueShade.StrikeInstantKill();
                                    YellowShade.ai[1] = 1;
                                    NPC.StrikeInstantKill();
                                }
                                break;
                        }
                    }
                    // Reset if repeat dialogue
                    else if (!NPCDialogueUI.IsBeingTalkedTo(NPC))
                    {
                        Timer = 0;
                        State = 0;
                    }
                }
                else
                {
                    Timer = 0;
                }
            }

            if (NPC.frameCounter++ % 8 == 0)
            {
                texOffset = Main.rand.NextVector2Circular(100, 100);
            }
            int iterationAmt = 5;
            for (int i = 0; i < iterationAmt; i++)
            {
                float comp = i / (float)(iterationAmt - 1);
                VoidMetaballGreen.SpawnParticle(NPC.Center + Vector2.UnitY.RotatedBy(NPC.rotation) * MathHelper.Lerp(0, 140, comp) + Main.rand.NextVector2Circular(30, 30), Main.rand.NextVector2Circular(2, 2), Main.rand.NextFloat(100, 150) * MathHelper.Lerp(1, 0.10f, comp));
                if (Main.rand.NextBool(10))
                {

                    VoidMetaballGreen.SpawnParticle(NPC.Center + Vector2.UnitY.RotatedBy(NPC.rotation) * MathHelper.Lerp(0, 140, comp) + Main.rand.NextVector2Circular(30, 30), Main.rand.NextVector2Circular(8, 8), Main.rand.NextFloat(10, 30), NPC.whoAmI);
                }
            }
        }
        
        public void SwitchShade(NPC nextShade, string key)
        {
            if (Target.Remix().talkedNPC == -1 && nextShade.whoAmI != -1)
            {
                State++;
                NPCDialogueUI.StartDialogue(nextShade.whoAmI, key);
                if (Main.BestiaryTracker.Kills.GetKillCount(nextShade) <= 1)
                {
                    for (int i = 0; i < 50; i++)
                    Main.BestiaryTracker.Kills.RegisterKill(nextShade);
                }
            }
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Vector2 offset = Vector2.Zero;
            spriteBatch.Draw(tex, NPC.Center - screenPos + offset, null, Color.White, 0, tex.Size() / 2, NPC.scale, 0, 0);

            return false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
    }

    public class ShadeYellow : ShadeGreen
    {

        public static Vector2 texOffsetYellow = new Vector2();
        public override void AI()
        {
            if (NPC.frameCounter++ % 8 == 0)
            {
                texOffsetYellow = Main.rand.NextVector2Circular(100, 100);
            }
            NPC.TargetClosest();
            NPC.spriteDirection = NPC.direction;
            int iterationAmt = 5;
            float ex = State == 1 ? 3 : 1;
            for (int i = 0; i < iterationAmt; i++)
            {
                float comp = i / (float)(iterationAmt - 1);
                VoidMetaballYellow.SpawnParticle(NPC.Center + Vector2.UnitY.RotatedBy(NPC.rotation) * MathHelper.Lerp(0, 140, comp) + Main.rand.NextVector2Circular(30, 30), Main.rand.NextVector2Circular(2, 2) * ex, Main.rand.NextFloat(100, 150) * MathHelper.Lerp(1, 0.10f, comp));
                if (Main.rand.NextBool(10))
                {

                    VoidMetaballYellow.SpawnParticle(NPC.Center + Vector2.UnitY.RotatedBy(NPC.rotation) * MathHelper.Lerp(0, 140, comp) + Main.rand.NextVector2Circular(30, 30), Main.rand.NextVector2Circular(8, 8) * ex, Main.rand.NextFloat(10, 30), NPC.whoAmI);
                }
            }
            if (State == 1)
            {
                Timer++;
                if (Timer == 120)
                {
                    NPCDialogueUI.StartDialogue(NPC.whoAmI, "QuestEnd");
                }
                if (Timer > 120)
                {
                    if (!NPCDialogueUI.IsBeingTalkedTo(NPC))
                    {
                        NPC.active = false;
                    }
                }
            }
        }
    }

    public class ShadeBlue : ShadeGreen
    {

        public static Vector2 texOffsetBlue = new Vector2();
        public override void AI()
        {
            if (NPC.frameCounter++ % 8 == 0)
            {
                texOffsetBlue = Main.rand.NextVector2Circular(100, 100);
            }
            NPC.TargetClosest();
            NPC.spriteDirection = NPC.direction;
            int iterationAmt = 5;
            for (int i = 0; i < iterationAmt; i++)
            {
                float comp = i / (float)(iterationAmt - 1);
                VoidMetaballBlue.SpawnParticle(NPC.Center + Vector2.UnitY.RotatedBy(NPC.rotation) * MathHelper.Lerp(0, 140, comp) + Main.rand.NextVector2Circular(30, 30), Main.rand.NextVector2Circular(2, 2), Main.rand.NextFloat(100, 150) * MathHelper.Lerp(1, 0.10f, comp));
                if (Main.rand.NextBool(10))
                {

                    VoidMetaballBlue.SpawnParticle(NPC.Center + Vector2.UnitY.RotatedBy(NPC.rotation) * MathHelper.Lerp(0, 140, comp) + Main.rand.NextVector2Circular(30, 30), Main.rand.NextVector2Circular(8, 8), Main.rand.NextFloat(10, 30), NPC.whoAmI);
                }
            }
        }
    }
}
