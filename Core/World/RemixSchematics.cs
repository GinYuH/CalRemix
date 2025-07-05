using CalamityMod;
using CalamityMod.Schematics;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria.ModLoader;

namespace CalRemix.Core.World
{
    public class RemixSchematics : ModSystem
    {
        internal const string IonAltarName = "Core/World/ionaltar.csch";
        internal const string OrigenWorkshopName = "Core/World/origenworkshop.csch";
        internal const string HallowShrineName = "Core/World/hallowshrine.csch";
        internal const string FrozenStrongholdName = "Core/World/FrozenStrongholdLocked.csch";
        internal const string CrimsonHeartName = "Core/World/CrimsonHeart.csch";
        internal const string PiggyStrawName = "Core/World/PiggyStraw.csch";
        internal const string PiggyStickName = "Core/World/PiggyStick.csch";
        internal const string PiggyBrickName = "Core/World/PiggyBrick.csch";

        internal static Dictionary<string, SchematicMetaTile[,]> TileMaps =>
            typeof(SchematicManager).GetField("TileMaps", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) as Dictionary<string, SchematicMetaTile[,]>;

        internal static readonly MethodInfo ImportSchematicMethod = typeof(CalamitySchematicIO).GetMethod("ImportSchematic", BindingFlags.NonPublic | BindingFlags.Static);


        public override void PostSetupContent()
        {
            TileMaps.Add("Ion Altar", LoadSchematic(IonAltarName).ShaveOffEdge());
            TileMaps.Add("Origen Workshop", LoadSchematic(OrigenWorkshopName).ShaveOffEdge());
            TileMaps.Add("Hallow Shrine", LoadSchematic(HallowShrineName).ShaveOffEdge());
            TileMaps.Add("Frozen Stronghold", LoadSchematic(FrozenStrongholdName).ShaveOffEdge());
            TileMaps.Add("Crimson Heart", LoadSchematic(CrimsonHeartName).ShaveOffEdge());
            TileMaps.Add("Piggy Straw", LoadSchematic(PiggyStrawName));
            TileMaps.Add("Piggy Stick", LoadSchematic(PiggyStickName));
            TileMaps.Add("Piggy Brick", LoadSchematic(PiggyBrickName));
        }


        public static SchematicMetaTile[,] LoadSchematic(string filename)
        {
            SchematicMetaTile[,] ret = null;
            using (Stream st = CalRemix.instance.GetFileStream(filename, true))
                ret = (SchematicMetaTile[,])ImportSchematicMethod.Invoke(null, [st]);

            return ret;
        }

    }
}