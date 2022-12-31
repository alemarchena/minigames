
using UnityEngine;

public class Movimientos : MonoBehaviour
{
  /// <summary>
  /// Mueve un objeto desde su lugar a un destino
  /// </summary>
  /// <param name="objetoAmover"></param>
  /// <param name="destino"></param>
  /// <param name="Velocidad"></param>
  /// <param name="deltaT"></param>
    public void MoverDesdeHasta(Transform objetoAmover, Transform destino, float Velocidad, float deltaT)
    {
        float step = Velocidad * deltaT;
        objetoAmover.position = Vector3.MoveTowards(objetoAmover.position, destino.position, step);
    }
}
