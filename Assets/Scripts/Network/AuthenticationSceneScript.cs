using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationSceneScript : MonoBehaviour
{
    public Button toggleAuthenButton;
    public GameObject[] loginComponents;
    public GameObject[] signInComponents;

    private bool isLogin = true;

    void Start()
    {
        if (toggleAuthenButton == null) 
        {
            toggleAuthenButton = GameObject.Find("ToggleAuthenButton").GetComponent<Button>();
        }
        toggleAuthenButton.GetComponentInChildren<TMP_Text>().text = "Sign In";

        if (loginComponents == null || loginComponents.Length == 0)
        {
            loginComponents = GameObject.FindGameObjectsWithTag("LoginComponents");
        }

        if (signInComponents == null || signInComponents.Length == 0)
        {
            signInComponents = GameObject.FindGameObjectsWithTag("SignInComponents");
        }

        foreach (GameObject signInComponent in signInComponents)
        {
            signInComponent.SetActive(false);
        }
    }

    void Update()
    {
        
    }

    public void ToggleAuthenticationState()
    {
        if (isLogin)
        {
            isLogin = false;
            toggleAuthenButton.GetComponentInChildren<TMP_Text>().text = "Login";
            foreach (GameObject loginComponent in loginComponents)
            {
                loginComponent.SetActive(false);
            }
            foreach (GameObject signInComponent in signInComponents)
            {
                signInComponent.SetActive(true);
            }
        }
        else
        {
            isLogin = true;
            toggleAuthenButton.GetComponentInChildren<TMP_Text>().text = "Sign In";
            foreach (GameObject loginComponent in loginComponents)
            {
                loginComponent.SetActive(true);
            }
            foreach (GameObject signInComponent in signInComponents)
            {
                signInComponent.SetActive(false);
            }
        }
    }

    public void LoginWithPlayFab()
    {
        var accountText = loginComponents.First(x => x.name == "LoginAccountInput").GetComponent<TMP_InputField>().text;
        var passwordText = loginComponents.First(x => x.name == "LoginPasswordInput").GetComponent<TMP_InputField>().text;

        PlayFabManager.Instance.LoginWithEmailAndPassword(accountText, passwordText);
    }

    public void SignInWithPlayFab()
    {
        var displayNameText = signInComponents.First(x => x.name == "SignInDisplayNameInput").GetComponent<TMP_InputField>().text;
        var accountText = signInComponents.First(x => x.name == "SignInAccountInput").GetComponent<TMP_InputField>().text;
        var passwordText = signInComponents.First(x => x.name == "SignInPasswordInput").GetComponent<TMP_InputField>().text;
        var confirmPasswordText = signInComponents.First(x => x.name == "SignInConfirmPasswordInput").GetComponent<TMP_InputField>().text;

        if (passwordText != confirmPasswordText)
        {
            Debug.LogWarning("Password and confirm password do not match.");
            return;
        }

        PlayFabManager.Instance.RegisterWithEmailAndPassword(displayNameText, accountText, passwordText);
    }

}
