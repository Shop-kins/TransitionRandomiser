using System;
using HarmonyLib;
using TransitionRandomiser.UI;

namespace TransitionRandomiser.Player_Events
{

    class PlayerPatcher
    {
        private static Boolean isDead = true;

        private static Biome lastFrameBiome = BiomeHandler.LIFEPOD;
        private static long stabilisationCounter = 0;

        private static int stabilisationTime = 300;

        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("Update")]
        internal class Player_Update_Patch
        {

            [HarmonyPostfix]
            public static void Postfix()
            {
                CustomUI.SetBigText("");
                CustomUI.SetFirstText("Current biome: " + TransitionHandler.GetCurrentBiome().GetName());
                CustomUI.SetSecondText("Previous biome: " + TransitionHandler.GetPreviousBiome().GetName());

                try
                {
                    var main = Player.main;
                    Biome newBiome = BiomeHandler.GetBiomeByGameID(main.GetBiomeString().ToLower());
                    if (newBiome == null) return;

                    // Stabilisation Counter
                    if (lastFrameBiome.GetName() == newBiome.GetName())
                    {
                        stabilisationCounter++;
                    }
                    else
                    {
                        stabilisationCounter = 0;
                        lastFrameBiome = newBiome;
                    }

                    // Death stuff
                    if (isDead && newBiome.GetName() == "lifepod" && stabilisationCounter > stabilisationTime)
                    {
                        TransitionHandler.SetCurrentBiome(BiomeHandler.LIFEPOD);
                        isDead = false;
                    }
                    // Normal update stuff
                    else
                    {
                        if (TransitionHandler.GetCurrentBiome().GetName() != newBiome.GetName() && newBiome.GetName() != "lifepod" && TransitionHandler.GetCurrentBiome().GetName() != "lifepod")
                        {
                            if (stabilisationCounter > stabilisationTime)
                            {
                                Console.WriteLine("CHANGE FROM " + TransitionHandler.GetCurrentBiome().GetName() + " TO " + newBiome.GetName());
                                TeleportPosition teleportPosition = TransitionHandler.getTeleportPositionForBiomeTransfer(newBiome);
                                Biome teleportBiome = BiomeHandler.GetBiomeByGameID(teleportPosition.name);

                                Player.main.SetPosition(teleportPosition.position);
                                Player.main.OnPlayerPositionCheat();
                                stabilisationCounter = 0;

                                Console.WriteLine("TELEPORTING TO " + teleportBiome.GetName());

                                TransitionHandler.SetCurrentBiome(teleportBiome);
                            } else
                            {
                                CustomUI.SetBigText("Teleporting in " + Math.Round((stabilisationTime - stabilisationCounter) / 60.0, 0));
                            }
                        }
                        else if (stabilisationCounter > stabilisationTime && TransitionHandler.GetCurrentBiome().GetName() != newBiome.GetName())
                        {
                            // Something to do with the lifepod, so just ignore
                            TransitionHandler.SetCurrentBiome(newBiome);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to invoke action " + e.Message);
                    Console.WriteLine(e.StackTrace);
                }
                CustomUI.Update();
            }
        }

        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("OnKill")]
        internal class Player_Death_Patch
        {

            [HarmonyPostfix]
            public static void Postfix()
            {
                isDead = true;
            }
        }
    }

}
