using UnityEngine;

public class Probabilidad : MonoBehaviour {

    /// <summary>
    /// Requiere un arreglo de float ej:{ 0.30f, 0.25f,0.45f }
    /// </summary>
    /// <param name="probs"></param>
    /// <returns></returns>
    public float DameAleatorio(float[] probs)
    {
        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
}
