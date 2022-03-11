using System;
using System.IO;
using System.Reflection;
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
                CustomUI.SetBigText("");
                CustomUI.SetFirstText("Current biome: " + TransitionHandler.GetCurrentBiome().GetName());
                if (TransitionHandler.GetUndoLocation() != null)
                {
                    CustomUI.SetSecondText("Press U to undo the last teleport");
                } else
                {
                    CustomUI.SetSecondText("No undo available");
                }
                if (yourBiomeTextCounter <= 0)
                {
                    yourBiomeText = "";
                }
                yourBiomeTextCounter--;
                CustomUI.SetBiomeText(yourBiomeText);

                try
                {
                    Biome newBiome = BiomeHandler.GetBiomeByGameID(Player.main.GetBiomeString().ToLower());
                    if (newBiome == null) return;

                    if (Input.GetKeyDown(KeyCode.U))
                    {
                        Teleport(TransitionHandler.GetUndoLocation());
                        TransitionHandler.ClearUndoLocation();
                        return;
                    }

                    // Stabilisation Counter
                    if (Player.main.playerController.inputEnabled)
                    {
                        if (lastFrameBiome.GetName() == newBiome.GetName())
                        {
                            stabilisationCounter++;
                        }
                        else
                        {
                            if (newBiome.GetName() == TransitionHandler.GetCurrentBiome().GetName() && biomeChangeDone)
                            {
                                UnfreezeStats();
                            }
                            else
                            {
                                FreezeStats();
                            }
                            stabilisationCounter = 0;
                            lastFrameBiome = newBiome;
                        }
                    }

                    // Biome change worked?
                    if (!biomeChangeDone && newBiome.GetName() == TransitionHandler.GetCurrentBiome().GetName() && Player.main.playerController.inputEnabled)
                    {
                        biomeChangeDone = true;
                        UnfreezeStats();
                        //Player.main.isUnderwater.Update(true);
                        Player.main.UpdateMotorMode();
                        Player.main.OnPlayerPositionCheat();
                        //Player.main.SetMotorMode(Player.MotorMode.Dive);
                    }

                    // Death stuff
                    if (isDead && Player.main.playerController.inputEnabled)
                    {
                        TransitionHandler.SetCurrentBiome(newBiome);
                        UnfreezeStats();
                        Console.WriteLine("HANDLED DEATH");
                        isDead = false;
                    }
                    // Normal update stuff
                    else if (!isDead && Player.main.playerController.inputEnabled)
                    {
                        if (TransitionHandler.GetCurrentBiome().GetName() != newBiome.GetName() && biomeChangeDone)
                        {
                            if (stabilisationCounter > stabilisationTime)
                            {
                                Console.WriteLine("CHANGE FROM " + TransitionHandler.GetCurrentBiome().GetName() + " TO " + newBiome.GetName());
                                TeleportLocation teleportLocation = TransitionHandler.getTeleportPositionForBiomeTransfer(newBiome, Player.main.lastPosition);
                                stabilisationCounter = 0;

                                if (teleportLocation == null)
                                {
                                    Console.WriteLine("NOT VALID; IGNORING");
                                    TransitionHandler.SetCurrentBiome(newBiome);
                                    return;
                                }

                                Teleport(teleportLocation);
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

            public static void Teleport(TeleportLocation location)
            {
                if (location == null) return;

                Player.main.playerController.inputEnabled = false;
                Player.main.playerController.SetEnabled(false);
                Player.main.transform.position = location.GetPosition().ToVector3();
                MainCameraControl.main.rotationX = 0;
                MainCameraControl.main.rotationY = 0;
                Player.main.transform.rotation = Quaternion.Euler(location.GetRotation().ToVector3());
                Player.main.WaitForTeleportation();
                Player.main.OnPlayerPositionCheat();
                biomeChangeDone = false;

                Console.WriteLine("TELEPORTING TO " + location.GetBiome().GetName());

                yourBiomeText = "New biome: " + location.GetBiome().GetName() + " (from " + location.GetOrigin().GetName() + ")";
                yourBiomeTextCounter = 600;

                TransitionHandler.SetCurrentBiome(location.GetBiome());
            }

            public static void FreezeStats()
            {
                Player.main.FreezeStats();
            }

            public static void UnfreezeStats()
            {
                while (Player.main.IsFrozenStats())
                {
                    Player.main.UnfreezeStats();
                }
            }

        }

        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("Awake")]
        internal class Player_Awake_Patch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                isDead = true;

                Console.WriteLine("PLAYER AWAKE");
                String baseDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
                String slot = SaveLoadManager.main.GetCurrentSlot();
                String filePath = Path.Combine(baseDirectory, slot + "-data.dat");

                if (SaveLoadManager.main.GetGameInfo(slot) == null || !File.Exists(filePath))
                {
                    // New save, regenerate
                    //Console.WriteLine("GENERATING " + filePath);
                    TransitionHandler.GenerateRandomTransitionMap();
                    String base64str = TransitionHandler.ToBase64String();
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    File.Create(filePath).Close();
                    File.WriteAllText(filePath, base64str);
                }
                else
                {
                    // Load
                    //Console.WriteLine("LOADING " + filePath);
                    String base64str = File.ReadAllText(filePath);
                    TransitionHandler.FromBase64String(base64str);
                }
                //TransitionHandler.WriteTransitionLog();
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
