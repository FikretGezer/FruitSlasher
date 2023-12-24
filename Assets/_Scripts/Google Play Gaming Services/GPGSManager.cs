using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using TMPro;

public class GPGSManager : MonoBehaviour
{
    public TMP_Text statusTxt;
    public TMP_Text descriptionTxt;
    public GameObject achievementButton;
    public GameObject leaderboardButton;

    private void Start() {
        SignInToGPGS();
    }
    internal void SignInToGPGS()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(code => {
            statusTxt.text = "Autenticating...";
            if(code == SignInStatus.Success)
            {
                statusTxt.text = "Successfully Authenticated.";
                descriptionTxt.text = "Hello " + Social.localUser.userName + ". You have an ID of " + Social.localUser.id;
                achievementButton.SetActive(true);
                leaderboardButton.SetActive(true);
            }
            else{
                statusTxt.text = "Failed to Authenticate";
                descriptionTxt.text = "Failed to Authenticate, reason for failure is " + code;
            }
        });
    }
}
/*
Hey all Just just going to pin this here quickly. If anyone experiences issues when either upgrading GPGS to the newest version 0.11.01 or is starting the
project after that point they've made some major changes, primarily that you no longer need to Initialize or Activate in order to call the authentication.

Note: this is for the messages about PlayGamesClientConfiguration not existing, it is no longer required!

Note for 0.11.1

The new SDK contains four major changes to increase sign-in success which you
should be aware of:

1. Sign-in is triggered automatically when your game is launched. Creating a
PlayGamesClientConfiguration instance, the initialization and activation of
PlayGamesPlatform are not needed. Just calling
`PlayGamesPlatform.Instance.Authenticate()` will fetch the result of automatic
sign-in.
2. Authentication tokens are now provided using
`PlayGamesPlatform.Instance.requestServerSideAccess()`.
3. Sign-out method is removed, and we will no longer require an in-game button
to sign-in or sign-out of Play Games Services.
4. Extra scopes cannot be requested.

Before importing the unity package for v0.11.1, delete everything under
`Assets/GooglePlayGames`. You can then follow the regular integration process.


Hope that helps anyone getting stuck with it, the tutorial should still work the same if you dont do those steps.
*/
