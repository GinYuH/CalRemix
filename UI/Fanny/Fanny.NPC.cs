using CalamityMod.NPCs.AcidRain;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.TownNPCs;
using CalRemix.Content.Items.Materials;
using CalRemix.Core.World;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadNPCMessages()
        {
            HelperMessage.New("God", "Holy sugar honey iced tea! I-is that.. No! It can’t be.. Is that a real GOD?! Oh flapjacks.. you better watch out, $0! That one is a real toughie!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(1500000) && scene.onscreenNPCs.Any(n => !n.friendly && !n.townNPC && !n.dontTakeDamage && !n.SpawnedFromStatue)).SetHoverTextOverride("Oh my! I'll make sure to watch out, Fanny!").AddDynamicText(HelperMessage.GetPlayerName);

            HelperMessage.New("Bee", "According to all known laws of aviation, there is no way that a bee should be able to fly. Its wings are too small to get its fat little body off the ground. The bee, of course, flies anyways. Because bees don't care what humans think is impossible.",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.Bee || n.type == NPCID.BeeSmall));

            screenHelperMessages.Add(HelperMessage.New("WulfrumPylone", "Woah, you hear that? No? Well it sounded like something big... we should get it's attention. A dose of some of that tower over there's energy in a special chest might be just the motivation it needs to come to the surface!",
                "FannyIdle",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<WulfrumAmplifier>() && !RemixDowned.downedExcavator), onlyPlayOnce: false, cooldown: 2400).AddItemDisplay(ItemID.LivingWoodChest));

            screenHelperMessages.Add(HelperMessage.New("Cysts", "That pimple thing looks useless, but it drops a very useful material. Please kill it!",
                "FannyAwooga",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<HiveTumor>() || n.type == ModContent.NPCType<PerforatorCyst>())));

            HelperMessage.New("Anauwu", "I sense an ominous presence. I think the best course of action here would be to kill everything you see. If something is dead it can't hurt you!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<LeviathanStart>()));

            HelperMessage.New("Fairy", "That thing is hurting my eyes! Kill it, quick!",
                "FannySob", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.EmpressButterfly));

            HelperMessage.New("Cultists", "Looks like some blue robe-wearing hooligans are worshiping a coin! Try not to interrupt them, they seem to be having a good time.",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.CultistDevote));

            HelperMessage.New("AncientDom", "Who is this guy???",
                "FannySob", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.AncientCultistSquidhead));

            HelperMessage.New("Crawltipede", "Did you know that those big worms can only target you when you're on the ground? Keep flying and you're untouchable!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.SolarCrawltipedeHead));

            HelperMessage.New("Bloodworm", "Crush it under your boot.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<BloodwormNormal>() || n.type == ModContent.NPCType<BloodwormFleeing>()));

            HelperMessage.New("Wolf", "Aw look a cute wolf! You can extract valuable Coyote Venom from their lifeless corpses in order to make some neat ice items.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.Wolf)).AddItemDisplay(ModContent.ItemType<CoyoteVenom>());

            HelperMessage.New("Dungeondie", "Oh, it appears my hack didn't work.",
                "FannySob", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.DungeonGuardian) && Main.LocalPlayer.dead);

            HelperMessage.New("AmidiasTalk", "What's this guy babbling about? His advice sure doesn't sound useful, though! Besides, you've got good ol' me! Who else could you possibly need?",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<SeaKing>() && Main.LocalPlayer.TalkNPC == n));

            HelperMessage.New("AnglerTalk", "Look at his shit eating grin. He knows there is nothing you can do to him. He's bullying you, and you are helpless. Kill him. Kill him now. He won't see death coming.",
                "EvilFannyMiffed", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.Angler && Main.LocalPlayer.TalkNPC == n)).SetHoverTextOverride("... what?").SpokenByEvilFanny();

            HelperMessage.New("Deimos", "That \"Deimos\" over there. She has some delicious Mars Bars, you should go buy some!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => CrossModNPC(scene, "EverquartzAdventure", "StarbornPrincess")).SetHoverTextOverride("Thanks Fanny! I'll buy you plenty of Mars Bars!");

            HelperMessage.New("MutantNPC", "Hey, you see that... fleshy, blue winged guy? I've got a bad feeling about him, he looks real strong and he could DEFINETLY crush you in a fight. Not like that would ever happen, of course!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => CrossModNPC(scene, "Fargowiltas", "Mutant")).SetHoverTextOverride("I'll keep him in mind, Fanny!");

            HelperMessage.New("SirNautilus", "That fish guy seems a bit confused. He claims his name is \"Sir Nautilus\", but I can't help but notice he's more.. fish-like, no? Someone oughtta teach this guy his marine biology!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => CrossModNPC(scene, "CalamityFables", "SirNautilusPassive"));


            int num = Main.rand.Next(0, int.MaxValue);
            int dateTypes = Main.rand.Next(10);
            // thank you javyz
            string timeType = dateTypes switch
            {
                0 => "milliseconds",
                1 => "seconds",
                2 => "minutes",
                3 => "hours",
                4 => "days",
                5 => "weeks",
                6 => "months",
                7 => "years",
                8 => "decades",
                9 => "centuries",
                _ => "millenia"
            };
            HelperMessage.New("UpdateTime", "Wondering when the next update is? I know! I know! It's in " + num + " " + timeType + "!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Cnidrion>())).SetHoverTextOverride("Wow! That sure is a while Fanny!");
        }
    }
}