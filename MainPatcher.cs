using QModManager.API.ModLoading;
using System.Reflection;
using HarmonyLib;

namespace TransitionRandomiser
{
    [QModCore]
    public class MainPatcher
    {

        internal static Assembly myAssembly = Assembly.GetExecutingAssembly();

        [QModPatch]
        public static void Patch()
        {            
            Harmony.CreateAndPatchAll(myAssembly, "subnautica.mod.twitchinteraction");
        }

    }
}
