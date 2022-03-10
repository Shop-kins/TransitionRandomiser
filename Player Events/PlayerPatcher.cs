using System;
using HarmonyLib;
using TransitionRandomiser.UI;
using UnityEngine;

namespace TransitionRandomiser.Player_Events
{

    class PlayerPatcher
    {
        private static Boolean isDead = true;
        private static Boolean biomeChangeDone = true;

        private static Biome lastFrameBiome = BiomeHandler.SAFESHALLOWS;
        private static long stabilisationCounter = 0;

        private static int stabilisationTime = 300;

        private static String yourBiomeText = "";
        private static int yourBiomeTextCounter = 0;

        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("Update")]
        internal class Player_Update_Patch
        {

            [HarmonyPostfix]
            public static void Postfix()
            {
                if (!TransitionHandler.IsInitialised())
                {
                    TransitionHandler.GenerateRandomTransitionMap();
                }

                CustomUI.SetBigText("");
                CustomUI.SetFirstText("Current biome: " + TransitionHandler.GetCurrentBiome().GetName());
                CustomUI.SetSecondText("Previous biome: " + TransitionHandler.GetPreviousBiome().GetName());
                if (yourBiomeTextCounter <= 0)
                {
                    yourBiomeText = "";
                }
                yourBiomeTextCounter--;
                CustomUI.SetBiomeText(yourBiomeText);

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
                        if (newBiome.GetName() == TransitionHandler.GetCurrentBiome().GetName())
                        {
                            Player.main.UnfreezeStats();
                        }
                        else
                        {
                            Player.main.FreezeStats();
                        }
                        stabilisationCounter = 0;
                        lastFrameBiome = newBiome;
                    }

                    // Biome change worked?
                    if (!biomeChangeDone && newBiome.GetName() == TransitionHandler.GetCurrentBiome().GetName() && Player.main.playerController.inputEnabled)
                    {
                        biomeChangeDone = true;
                        Player.main.UnfreezeStats();
                    }

                    // Death stuff
                    if (isDead && main.GetBiomeString().ToLower() == "lifepod" && stabilisationCounter > stabilisationTime)
                    {
                        TransitionHandler.ResetCurrentBiome();
                        Console.WriteLine("HANDLED DEATH");
                        isDead = false;
                    }
                    // Normal update stuff
                    else if (!isDead)
                    {
                        if (TransitionHandler.GetCurrentBiome().GetName() != newBiome.GetName() && biomeChangeDone)
                        {
                            if (stabilisationCounter > stabilisationTime)
                            {
                                Console.WriteLine("CHANGE FROM " + TransitionHandler.GetCurrentBiome().GetName() + " TO " + newBiome.GetName());
                                TeleportLocation teleportLocation = TransitionHandler.getTeleportPositionForBiomeTransfer(newBiome, main.lastPosition);
                                stabilisationCounter = 0;

                                if (teleportLocation == null)
                                {
                                    Console.WriteLine("NOT VALID; IGNORING");
                                    TransitionHandler.SetCurrentBiome(newBiome);
                                    return;
                                }

                                Player.main.playerController.inputEnabled = false;
                                Player.main.playerController.SetEnabled(false);
                                Player.main.transform.position = teleportLocation.GetPosition();
                                Player.main.isUnderwaterForSwimming.Update(true);
                                MainCameraControl.main.rotationX = 0;
                                MainCameraControl.main.rotationY = 0;
                                Player.main.transform.rotation = Quaternion.Euler(teleportLocation.GetRotation());
                                Player.main.WaitForTeleportation();
                                biomeChangeDone = false;

                                Console.WriteLine("TELEPORTING TO " + teleportLocation.GetBiome().GetName());

                                yourBiomeText = "New biome: " + teleportLocation.GetBiome().GetName() + " (from " + teleportLocation.GetOrigin().GetName() + ")";
                                yourBiomeTextCounter = 600;

                                TransitionHandler.SetCurrentBiome(teleportLocation.GetBiome());
                            } else
                            {
                                CustomUI.SetBigText("Teleporting in " + Math.Round((stabilisationTime - stabilisationCounter) / 60.0, 0));
                            }
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
