using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;

namespace CalRemix.Core.World
{
    public class RemixDowned : ModSystem
    {
        public static bool DownedGens => downedOrigen && downedCarcinogen && downedPhytogen && downedHydrogen && downedOxygen && downedIonogen && downedPathogen;

        public static bool downedCalamity = false;
        public static bool downedExcavator = false;
        public static bool downedAcidsighter = false;
        public static bool downedOrigen = false;
        public static bool downedCarcinogen = false;
        public static bool downedPhytogen = false;
        public static bool downedIonogen = false;
        public static bool downedHydrogen = false;
        public static bool downedOxygen = false;
        public static bool downedPathogen = false;
        public static bool downedPyrogen = false;
        public static bool downedDerellect = false;
        public static bool downedPolyphemalus = false;
        public static bool downedHypnos = false;
        public static bool downedSealedOne = false;

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

        public static bool downedGale = false;
        public override void OnWorldLoad()
        {
            downedCalamity = false;
            downedExcavator = false;
            downedAcidsighter = false;
            downedOrigen = false;
            downedCarcinogen = false;
            downedPhytogen = false;
            downedIonogen = false;
            downedHydrogen = false;
            downedOxygen = false;
            downedPathogen = false;
            downedPyrogen = false;
            downedDerellect = false;
            downedPolyphemalus = false;
            downedHypnos = false;
            downedSealedOne = false;

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

            downedGale = false;
        }
        public override void OnWorldUnload()
        {
            downedCalamity = false;
            downedExcavator = false;
            downedAcidsighter = false;
            downedOrigen = false;
            downedCarcinogen = false;
            downedPhytogen = false;
            downedIonogen = false;
            downedHydrogen = false;
            downedOxygen = false;
            downedPyrogen = false;
            downedPathogen = false;
            downedDerellect = false;
            downedPolyphemalus = false;
            downedHypnos = false;
            downedSealedOne = false;

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

            downedGale = false;
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag["downedCalamity"] = downedCalamity;
            tag["downedExcavator"] = downedExcavator;
            tag["downedAcidsighter"] = downedAcidsighter;
            tag["downedOrigen"] = downedOrigen;
            tag["downedCarcinogen"] = downedCarcinogen;
            tag["downedPhytogen"] = downedPhytogen;
            tag["downedIonogen"] = downedIonogen;
            tag["downedHydrogen"] = downedHydrogen;
            tag["downedOxygen"] = downedOxygen;
            tag["downedPathogen"] = downedPathogen;
            tag["downedPyrogen"] = downedPyrogen;
            tag["downedDerellect"] = downedDerellect;
            tag["downedPolyphemalus"] = downedPolyphemalus;
            tag["downedHypnos"] = downedHypnos;
            tag["downedSealedOne"] = downedSealedOne;

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

            tag["downedGale"] = downedGale;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            downedCalamity = tag.Get<bool>("downedCalamity");
            downedDerellect = tag.Get<bool>("downedDerellect");
            downedExcavator = tag.Get<bool>("downedExcavator");
            downedAcidsighter = tag.Get<bool>("downedAcidsighter");
            downedOrigen = tag.Get<bool>("downedOrigen");
            downedCarcinogen = tag.Get<bool>("downedCarcinogen");
            downedPhytogen = tag.Get<bool>("downedPhytogen");
            downedIonogen = tag.Get<bool>("downedIonogen");
            downedHydrogen = tag.Get<bool>("downedHydrogen");
            downedOxygen = tag.Get<bool>("downedOxygen");
            downedPathogen = tag.Get<bool>("downedPathogen");
            downedPyrogen = tag.Get<bool>("downedPyrogen");
            downedDerellect = tag.Get<bool>("downedDerellect");
            downedPolyphemalus = tag.Get<bool>("downedPolyphemalus");
            downedHypnos = tag.Get<bool>("downedHypnos");
            downedSealedOne = tag.Get<bool>("downedSealedOne");

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
            downedGale = tag.Get<bool>("downedGale");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(downedCalamity);
            writer.Write(downedExcavator);
            writer.Write(downedAcidsighter);
            writer.Write(downedOrigen);
            writer.Write(downedCarcinogen);
            writer.Write(downedPhytogen);
            writer.Write(downedIonogen);
            writer.Write(downedHydrogen);
            writer.Write(downedOxygen);
            writer.Write(downedPathogen);
            writer.Write(downedPyrogen);
            writer.Write(downedDerellect);
            writer.Write(downedPolyphemalus);
            writer.Write(downedHypnos);
            writer.Write(downedSealedOne);

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

            writer.Write(downedGale);
        }
        public override void NetReceive(BinaryReader reader)
        {
            downedCalamity = reader.ReadBoolean();
            downedExcavator = reader.ReadBoolean();
            downedAcidsighter = reader.ReadBoolean();
            downedOrigen = reader.ReadBoolean();
            downedCarcinogen = reader.ReadBoolean();
            downedPhytogen = reader.ReadBoolean();
            downedIonogen = reader.ReadBoolean();
            downedHydrogen = reader.ReadBoolean();
            downedOxygen = reader.ReadBoolean();
            downedPathogen = reader.ReadBoolean();
            downedPyrogen = reader.ReadBoolean();
            downedDerellect = reader.ReadBoolean();
            downedPolyphemalus = reader.ReadBoolean();
            downedHypnos = reader.ReadBoolean();
            downedSealedOne = reader.ReadBoolean();

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

            downedGale = reader.ReadBoolean();
        }
    }
}