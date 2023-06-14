using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    //Firebase Variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public TMP_InputField userNameRegisterField;
    public TMP_InputField userSurnameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    [Header("Change")]
    public TMP_InputField emailChangePassword;
    public TMP_InputField newPassword;
    public TMP_Text warningChangePassText;

    [Header("Tela")]
    public GameObject loginTela;
    public GameObject registroTela, senhaTela, emailTela;
    private GameObject atualTela;

    private void Awake()
    {
        //Verifica se todas as depedencias do firebase estão incluidas no sistema
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //Se elas estão incluidas inicializa o firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Não foi possivel resolver todas as dependencias do firebase " + dependencyStatus);
            }
        });

        atualTela = loginTela;
    }

    private void InitializeFirebase()
    {
        Debug.Log("FirebaseAuth Configurado!");
        //Muda o autenticador
        auth = FirebaseAuth.DefaultInstance;
    }
    //Função para o botão de login
    public void LoginButton()
    {
        //Chama a corrotina login passando o email e senha
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    //função para o botão de registro
    public void RegisterButton()
    {
        //chama a corrotina registro passand o email, senha e nome de usuário
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, userNameRegisterField.text + " " + userSurnameRegisterField.text));
    }

    public void TelaLogin()
    {
        MudarTela(loginTela);
    }

    public void TelaRegistro()
    {
        MudarTela(registroTela);
    }

    public void TelaSenha()
    {
        MudarTela(senhaTela);
    }

    void MudarTela(GameObject tela)
    {
        tela.SetActive(true);
        print(atualTela);
        if (atualTela != null)
            atualTela.SetActive(false);
        atualTela = tela;
    }

    public void ChangePassButton()
    {
        StartCoroutine(ChangePassword(emailChangePassword.text));
    }

    private IEnumerator Login(string email, string password)
    {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Falha na tarefa de registro com {LoginTask.Exception}");
            FirebaseException firebaseException = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseException.ErrorCode;

            string message = "Login Falhou!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Email não encontrado";
                    break;
                case AuthError.MissingPassword:
                    message = "Senha não encontrada!";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong password!";
                    break;
                case AuthError.InvalidEmail:
                    message = "Email invalido!";
                    break;
                case AuthError.UserNotFound:
                    message = "Conta não existe!";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            User = LoginTask.Result.User;
            Debug.LogFormat("Usuário logado com sucesso: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logado!";
            LoadSceneManager.instance.Load(1);
        }
    }
    private IEnumerator Register(string _email, string password, string username)
    {
        Debug.Log(_email);
        if (username == "")
        {
            warningRegisterText.text = "Usuario está vazio";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            warningRegisterText.text = "Senha não combina!";
        }
        else
        {
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, password);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                Debug.LogWarning(message: $"Falha no registro com {RegisterTask.Exception}");
                FirebaseException firebaseException = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseException.ErrorCode;

                string message = "Registro Falhou";

                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Email não preenchido aqui";
                        break;
                    case AuthError.MissingPassword:
                        message = "Senha não preenchido!";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak password!";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email ja em uso!";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                User = RegisterTask.Result.User;
                if (User != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = username };
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);
                    if (ProfileTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Falha na tarefa de registro com {ProfileTask.Exception}");
                        FirebaseException firebaseException = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)(firebaseException.ErrorCode);
                        warningRegisterText.text = "Nove de usuario falhou";
                    }
                    else
                    {
                        //UIManager.instance.LoginScreen();
                        warningRegisterText.text = "";
                    }
                    var sendEmailTask = User.SendEmailVerificationAsync().ContinueWith(task =>
                    {
                        if (task.IsCanceled)
                        {
                            print("A tarefa de envio de email foi cancelada");
                            return;
                        }
                        if (task.IsFaulted)
                        {
                            print("Ocorreu um erro no envio do email de verificação: " + task.Exception.ToString());
                            return;
                        }
                        print("Email foi enviado com sucesso!");
                    });
                }
            }
        }
    }

    private IEnumerator ChangePassword(string email)
    {
        if (emailChangePassword.text == "")
        {
            warningChangePassText.text = "Campo de email não pode estar vazio";
        }

        else
        {
            var task = auth.SendPasswordResetEmailAsync(email).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("Password reset email sent successfully.");
            });

            yield return new WaitUntil(predicate: () => task.IsCompleted);

            TelaLogin();
        }
    }
}
