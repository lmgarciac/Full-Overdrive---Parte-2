using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ControlSemaforo : MonoBehaviour
{
    public float _tiempoAbierto = 8f;
    public float _tiempoAlerta = 3f;
    public Semaforo[] _semaforos = new Semaforo[2];

    private int _señalActual = 0;
    private float _controlTiempo = 0;
    private bool _emitirAlerta = false;

    void Start()
    {
        AlterarSeñal();
    }

    // Update is called once per frame
    void Update()
    {
        if(_controlTiempo >= _tiempoAlerta && !_emitirAlerta)
        {
            _emitirAlerta = true;

            _semaforos[_señalActual].AlertaSeñal();
        }
        else if (_controlTiempo >= _tiempoAbierto)
        {
            _emitirAlerta = false;
            _controlTiempo = 0;
            _semaforos[_señalActual].CerrarSeñal();
            AlterarSeñal();
        }

        _controlTiempo += (1 * Time.deltaTime);
    }

    private void AlterarSeñal()
    {
        ProximaSeñal();
        for (int i = 0; i<_semaforos.Length;i++)
        {
            if (i == _señalActual)
            {
                _semaforos[i].AbrirSeñal();
            }
            else
            {
                _semaforos[i].CerrarSeñal();
            }
        }
    }

    private void ProximaSeñal ()
    {
        if(_señalActual < 1)
        {
            _señalActual++;
        }
        else
        {
            _señalActual = 0;
        }
    }
}
