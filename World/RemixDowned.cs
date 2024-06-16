using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;

namespace CalRemix
{
    public class RemixDowned : ModSystem
    {
        public static bool downedCalamity = false;
        public static bool downedExcavator = false;
        public static bool downedAcidsighter = false;
        public static bool downedCarcinogen = false;
        public static bool downedPhytogen = false;
        public static bool downedIonogen = false;
        public static bool downedHydrogen = false;
        public static bool downedOxygen = false;
        public static bool downedPathogen = false;
        public static bool downedDerellect = false;
        public static bool downedPolyphemalus = false;

        public static bool downedEarthElemental = false;
        public static bool downedLifeSlime = false;
        public static bool downedClamitas = false;
        public static bool downedCyberDraedon = false;
        public static bool downedOnyxKinsman = false;
        public static bool downedYggdrasilEnt = false;
        public static bool downedPlagueEmperor = false;
        public static bool downedLaRuga = false;
        public static bool downedKingMinnowsPrime = false;
        public static bool downedDend = false;
        public static bool downedMaser = false;
        public override void OnWorldLoad()
        {
            downedCalamity = false;
            downedExcavator = false;
            downedAcidsighter = false;
            downedCarcinogen = false;
            downedPhytogen = false;
            downedIonogen = false;
            downedHydrogen = false;
            downedOxygen = false;
            downedPathogen = false;
            downedDerellect = false;
            downedPolyphemalus = false;

            downedEarthElemental = false;
            downedLifeSlime = false;
            downedClamitas = false;
            downedCyberDraedon = false;
            downedOnyxKinsman = false;
            downedYggdrasilEnt = false;
            downedPlagueEmperor = false;
            downedLaRuga = false;
            downedKingMinnowsPrime = false;
            downedDend = false;
            downedMaser = false;
        }
        public override void OnWorldUnload()
        {
            downedCalamity = false;
            downedExcavator = false;
            downedAcidsighter = false;
            downedCarcinogen = false;
            downedPhytogen = false;
            downedIonogen = false;
            downedHydrogen = false;
            downedOxygen = false;
            downedPathogen = false;
            downedDerellect = false;
            downedPolyphemalus = false;

            downedEarthElemental = false;
            downedLifeSlime = false;
            downedClamitas = false;
            downedCyberDraedon = false;
            downedOnyxKinsman = false;
            downedYggdrasilEnt = false;
            downedPlagueEmperor = false;
            downedLaRuga = false;
            downedKingMinnowsPrime = false;
            downedDend = false;
            downedMaser = false;
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag["downedCalamity"] = downedCalamity;
            tag["downedExcavator"] = downedExcavator;
            tag["downedAcidsighter"] = downedAcidsighter;
            tag["downedCarcinogen"] = downedCarcinogen;
            tag["downedPhytogen"] = downedPhytogen;
            tag["downedIonogen"] = downedIonogen;
            tag["downedHydrogen"] = downedHydrogen;
            tag["downedOxygen"] = downedOxygen;
            tag["downedPathogen"] = downedPathogen;
            tag["downedDerellect"] = downedDerellect;
            tag["downedPolyphemalus"] = downedPolyphemalus;

            tag["downedEarthElemental"] = downedEarthElemental;
            tag["downedLifeSlime"] = downedLifeSlime;
            tag["downedClamitas"] = downedClamitas;
            tag["downedCyberDraedon"] = downedCyberDraedon;
            tag["downedOnyxKinsman"] = downedOnyxKinsman;
            tag["downedYggdrasilEnt"] = downedYggdrasilEnt;
            tag["downedPlagueEmperor"] = downedPlagueEmperor;
            tag["downedLaRuga"] = downedLaRuga;
            tag["downedKingMinnowsPrime"] = downedKingMinnowsPrime;
            tag["downedDend"] = downedDend;
            tag["downedMaser"] = downedMaser;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            downedCalamity = tag.Get<bool>("downedCalamity");
            downedDerellect = tag.Get<bool>("downedDerellect");
            downedExcavator = tag.Get<bool>("downedExcavator");
            downedAcidsighter = tag.Get<bool>("downedAcidsighter");
            downedCarcinogen = tag.Get<bool>("downedCarcinogen");
            downedPhytogen = tag.Get<bool>("downedPhytogen");
            downedIonogen = tag.Get<bool>("downedIonogen");
            downedHydrogen = tag.Get<bool>("downedHydrogen");
            downedOxygen = tag.Get<bool>("downedOxygen");
            downedPathogen = tag.Get<bool>("downedPathogen");
            downedDerellect = tag.Get<bool>("downedDerellect");
            downedPolyphemalus = tag.Get<bool>("downedPolyphemalus");

            downedEarthElemental = tag.Get<bool>("downedEarthElemental");
            downedLifeSlime = tag.Get<bool>("downedLifeSlime");
            downedClamitas = tag.Get<bool>("downedClamitas");
            downedCyberDraedon = tag.Get<bool>("downedCyberDraedon");
            downedOnyxKinsman = tag.Get<bool>("downedOnyxKinsman");
            downedYggdrasilEnt = tag.Get<bool>("downedYggdrasilEnt");
            downedPlagueEmperor = tag.Get<bool>("downedPlagueEmperor");
            downedLaRuga = tag.Get<bool>("downedLaRuga");
            downedKingMinnowsPrime = tag.Get<bool>("downedKingMinnowsPrime");
            downedDend = tag.Get<bool>("downedDend");
            downedMaser = tag.Get<bool>("downedMaser");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(downedCalamity);
            writer.Write(downedExcavator);
            writer.Write(downedAcidsighter);
            writer.Write(downedCarcinogen);
            writer.Write(downedPhytogen);
            writer.Write(downedIonogen);
            writer.Write(downedHydrogen);
            writer.Write(downedOxygen);
            writer.Write(downedPathogen);
            writer.Write(downedDerellect);
            writer.Write(downedPolyphemalus);

            writer.Write(downedEarthElemental);
            writer.Write(downedLifeSlime);
            writer.Write(downedClamitas);
            writer.Write(downedCyberDraedon);
            writer.Write(downedOnyxKinsman);
            writer.Write(downedYggdrasilEnt);
            writer.Write(downedPlagueEmperor);
            writer.Write(downedLaRuga);
            writer.Write(downedKingMinnowsPrime);
            writer.Write(downedDend);
            writer.Write(downedMaser);
        }
        public override void NetReceive(BinaryReader reader)
        {
            downedCalamity = reader.ReadBoolean();
            downedExcavator = reader.ReadBoolean();
            downedAcidsighter = reader.ReadBoolean();
            downedCarcinogen = reader.ReadBoolean();
            downedPhytogen = reader.ReadBoolean();
            downedIonogen = reader.ReadBoolean();
            downedHydrogen = reader.ReadBoolean();
            downedOxygen = reader.ReadBoolean();
            downedPathogen = reader.ReadBoolean();
            downedDerellect = reader.ReadBoolean();
            downedPolyphemalus = reader.ReadBoolean();

            downedEarthElemental = reader.ReadBoolean();
            downedLifeSlime = reader.ReadBoolean();
            downedClamitas = reader.ReadBoolean();
            downedCyberDraedon = reader.ReadBoolean();
            downedOnyxKinsman = reader.ReadBoolean();
            downedYggdrasilEnt = reader.ReadBoolean();
            downedPlagueEmperor = reader.ReadBoolean();
            downedLaRuga = reader.ReadBoolean();
            downedKingMinnowsPrime = reader.ReadBoolean();
            downedDend = reader.ReadBoolean();
            downedMaser = reader.ReadBoolean();
        }
    }
}