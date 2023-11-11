using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomNum_sc : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hintText, triesText, loseText;
    [SerializeField] Image hpBar;
    [SerializeField] GameObject lose;
    [SerializeField] int num, maxTries;
    int tries;
    
    TMP_InputField inputField;
    ScoreboardManager_sc scoreboardManager;

    public int Tries { get => tries; set => tries = value; }
    public int MaxTries { get => maxTries; set => maxTries = value; }

    void Start()
    {
        num = Random.Range(1, 101); // genera el numero random entre 1 y 100
        inputField = GetComponent<TMP_InputField>();
        Tries = MaxTries;

        scoreboardManager=FindObjectOfType<ScoreboardManager_sc>();
    }

    public void CheckNumber() // Funcion que se llama cuando el jugador llena el input
    {
        if (int.TryParse(inputField.text, out int i))
        {
            Tries--; //se restan los intentos

            if (i == num) //Cuando adivina el numero activa la interfaz de victoria
            {
                hintText.text = "ADIVINASTE!!";
                if (scoreboardManager != null)
                {
                    scoreboardManager.AddPlayerScore();
                }
            }
            else if (i > 100 || i <= 0)
            {
                hintText.text = "El numero esta entre 1 y 100 ";
            }
            else if (i > num)
            {
                hintText.text = "El numero es menor a " + i;
            }
            else
            {
                hintText.text = "El numero es mayor a " + i;
            }
            triesText.text = Tries.ToString(); //muestra los intentos en pantalla
            hpBar.fillAmount = (float)Tries / MaxTries; //convierte los intentos en un rango entre 0 y 1
        }
        else
        {
            hintText.text = "Ingresa solo numeros"; //Pide que ingrese unicamente numeros enteros
        }

        if (Tries <= 0)
        {
            lose.SetActive(true);// Activa interfaz derrota
            loseText.text = "Agotaste tus oportunidades\r\nEl numero era: " + num;
        }
    }   
}
