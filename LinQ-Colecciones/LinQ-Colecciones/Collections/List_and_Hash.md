using System;
using System.Collections.Generic;

public class ListDemo
{
    private List<string> nombres;

    public ListDemo()
    {
        nombres = ["Juan", "María", "Pedro"];
    }

    public void MostrarNombres()
    {
        foreach (string nombre in nombres)
        {
            Console.WriteLine(nombre);
        }
    }
}