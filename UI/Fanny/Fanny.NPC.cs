using CalamityMod.NPCs.AcidRain;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.Perforator;
using CalRemix.Items.Materials;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        public static void LoadNPCMessages()
        {
            fannyMessages.Add(new FannyMessage("Bee", "According to all known laws of aviation, there is no way that a bee should be able to fly. Its wings are too small to get its fat little body off the ground. The bee, of course, flies anyways. Because bees don't care what humans think is impossible.",
                "Nuhuh", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.Bee || n.type == NPCID.BeeSmall)));

            fannyMessages.Add(new FannyMessage("WulfrumPylone", "Woah, you hear that? No? Well it sounded like something big... we should get it's attention. A dose of some of that tower over there's energy in a special chest might be just the motivation it needs to come to the surface!",
                "Idle",
                (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<WulfrumAmplifier>() && !RemixDowned.downedExcavator), onlyPlayOnce: false, cooldown: 2400).AddItemDisplay(ItemID.LivingWoodChest));

            fannyMessages.Add(new FannyMessage("Cysts", "That pimple thing looks useless, but it drops a very useful material. Please kill it!",
                "Awooga",
                (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<HiveTumor>() || n.type == ModContent.NPCType<PerforatorCyst>())));

            fannyMessages.Add(new FannyMessage("Anauwu", "I sense an ominous presence. I think the best course of action here would be to kill everything you see. If something is dead it can't hurt you!",
                "Idle", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<LeviathanStart>())));

            fannyMessages.Add(new FannyMessage("Fairy", "That thing is hurting my eyes! Kill it, quick!",
                "Sob", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.EmpressButterfly)));

            fannyMessages.Add(new FannyMessage("Cultists", "Looks like some blue robe-wearing hooligans are worshiping a coin! Try not to interrupt them, they seem to be having a good time.",
                "Nuhuh", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.CultistDevote)));

            fannyMessages.Add(new FannyMessage("AncientDom", "Who is this guy???",
                "Sob", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.AncientCultistSquidhead)));

            fannyMessages.Add(new FannyMessage("Bloodworm", "Crush it under your boot.",
                "Idle", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<BloodwormNormal>() || n.type == ModContent.NPCType<BloodwormFleeing>())));

            fannyMessages.Add(new FannyMessage("Wolf", "Aw look a cute wolf! You can extract valuable Coyote Venom from their lifeless corpses in order to make some neat ice items.",
                "Idle", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.Wolf)).AddItemDisplay(ModContent.ItemType<CoyoteVenom>()));

            fannyMessages.Add(new FannyMessage("Dungeondie", "Oh, it appears my hack didn't work.",
                "Sob", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.DungeonGuardian) && Main.LocalPlayer.dead));

            fannyMessages.Add(new FannyMessage("AnglerTalk", "Look at his shit eating grin. He knows there is nothing you can do to him. He's bullying you, and you are helpless. Kill him. Kill him now. He won't see death coming.",
                "EvilIdle", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.Angler && Main.LocalPlayer.TalkNPC == n)).SetHoverTextOverride("... what?"));

            fannyMessages.Add(new FannyMessage("Deimos", "That \"Deimos\" over there. She has some delicious Mars Bars, you should go buy some!",
                "Idle", (FannySceneMetrics scene) => CrossModNPC(scene, "EverquartzAdventure", "StarbornPrincess")).SetHoverTextOverride("Thanks Fanny! I'll buy you plenty of Mars Bars!"));

            fannyMessages.Add(new FannyMessage("MutantNPC", "Hey, you see that... fleshy, blue winged guy? I've got a bad feeling about him, he looks real strong and he could DEFINETLY crush you in a fight. Not like that would ever happen, of course!",
                "Idle", (FannySceneMetrics scene) => CrossModNPC(scene, "FargowiltasSouls", "Mutant")).SetHoverTextOverride("I'll keep him in mind, Fanny!"));
        }
    }
}