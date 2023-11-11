using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardManager_sc : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText, scoreText, winText;
    [SerializeField] TMP_InputField playerNameInput; // Necesitarás un InputField para que el jugador ingrese su nombre.
    [SerializeField] GameObject win;
    bool isNewHighScore;
    RandomNum_sc randomNum;

    private struct PlayerScore
    {
        public string playerName;
        public int attempts;
    }

    private List<PlayerScore> playerScores = new List<PlayerScore>();

    void Start()
    {
        LoadPlayerScores();
        UpdateScoreboard();
        randomNum = FindObjectOfType<RandomNum_sc>();
    }

    public void AddPlayerScore()
    {
        //obtiene los datos del jugador
        string playerName = playerNameInput.text;
        int attempt = randomNum.Tries - randomNum.MaxTries;

        int attempts = Mathf.Abs(attempt); //convierte en positivo 
        win.SetActive(true);

        if (IsHighScore(attempts))
        {            
            //si el puntaje supero algun record activa el go y pide al usuario que ponga su nombre
            winText.text = "Ingresa tu nombre para registrar tu record";
            isNewHighScore = true;
            playerNameInput.enabled=true;
        }
        else
        {
            winText.text = "El puntaje no supera a ninguno de los cinco mejores, no puedes registrar tu nombre";
            playerNameInput.enabled = false;
        }
    }
    public void SavePlayerRecord()
    {
        if (isNewHighScore)
        {
            string playerName = playerNameInput.text;
            int attempt = randomNum.Tries - randomNum.MaxTries;
            int attempts = Mathf.Abs(attempt);

            PlayerScore playerScore = new PlayerScore
            {
                playerName = playerName,
                attempts = attempts
            };

            // Agrega el nuevo puntaje a la lista.
            playerScores.Add(playerScore);

            // Ordenar la lista segun el puntaje.
            playerScores.Sort((a, b) => a.attempts.CompareTo(b.attempts));

            // Recortar la lista si tiene más de 5 elementos.
            if (playerScores.Count > 5)
            {
                playerScores.RemoveAt(playerScores.Count - 1);
            }

            SavePlayerScores();// Guardar la lista actualizada de puntajes.
            UpdateScoreboard();// Actualizar la interfaz del marcador.

            // Restablecer el indicador de nuevo récord y desactivar playerNameInput.
            isNewHighScore = false;
            playerNameInput.interactable = false;

            Debug.Log("Record guardado");
        }
    }
    private bool IsHighScore(int newAttempts)
    {
        // Verifica si el nuevo intento es lo suficientemente alto para entrar en la tabla de clasificacion.

        if (playerScores.Count < 5)
        {
            return true;
        }

        return newAttempts < playerScores[4].attempts;
    }

    private void UpdateScoreboard()
    {
        nameText.text = ""; //limpia el texto para que no se repita
        scoreText.text = "";

        foreach (var playerScore in playerScores)//actualiza la lista
        {            
            nameText.text += $"\n{playerScore.playerName}\n"; 
            scoreText.text += $"\n{playerScore.attempts} intentos\n";
        }
    }

    private void SavePlayerScores()
    {
        //Guarda la lista de los puntajes
        for (int i = 0; i < playerScores.Count; i++)
        {
            PlayerPrefs.SetString($"PlayerName{i}", playerScores[i].playerName);
            PlayerPrefs.SetInt($"Attempts{i}", playerScores[i].attempts);
        }

        PlayerPrefs.Save();
    }

    private void LoadPlayerScores()
    {
        playerScores.Clear();

        bool anyPlayerExists = false;

        //bucle para cargar los datos guardados
        for (int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.HasKey($"PlayerName{i}") && PlayerPrefs.HasKey($"Attempts{i}"))
            {
                string playerName = PlayerPrefs.GetString($"PlayerName{i}");
                int attempts = PlayerPrefs.GetInt($"Attempts{i}");

                PlayerScore playerScore = new PlayerScore
                {
                    playerName = playerName,
                    attempts = attempts
                };

                playerScores.Add(playerScore);

                anyPlayerExists = true; //valida que ya existe un jugador
            }
        }
        if (!anyPlayerExists) //si no hay nungun jugador añade 5 
        {
            for (int i = 0; i < 5; i++)
            {
                AddDefaultPlayer();
            }
        }
    }
    private void AddDefaultPlayer()
    {
        // Agrega un jugador predeterminado con nombre "Jugador" y 9 intentos, esto con el fin de que si se ejecuta en un pc nuevo
        // la tabla no aparezca vacia.
        PlayerScore defaultPlayer = new PlayerScore
        {
            playerName = "Jugador",
            attempts = 9
        };

        playerScores.Add(defaultPlayer);
        SavePlayerScores();
    }
}
