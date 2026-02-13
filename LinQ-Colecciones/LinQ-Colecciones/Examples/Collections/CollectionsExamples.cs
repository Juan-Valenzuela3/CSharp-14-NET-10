using System.Collections.Frozen;

public class CollectionsExample
{
    #region Listas    
    public static void Run()
    {
        List<string> frutas = ["Manzana", "Banana", "Cereza"];
        Console.WriteLine($"Frutas: {frutas[1]}"); // Acceder por índice

        frutas.Add("Naranja"); // Agregar elemento
        frutas.AddRange(new[] { "Pera", "Uva" }); // Agregar varios elementos

        frutas.Insert(2, "Sandía"); // Insertar en posición específica

        frutas.Remove("Banana"); // Eliminar por valor
        frutas.RemoveAt(0); // Eliminar por índice

        Console.WriteLine("Lista actual: " + string.Join(", ", frutas));

        Console.WriteLine("¿Contiene 'Pera'? " + frutas.Contains("Pera")); // Buscar elemento
        Console.WriteLine("Índice de 'Uva': " + frutas.IndexOf("Uva")); // Buscar índice

        frutas.Sort(); // Ordenar alfabéticamente
        Console.WriteLine("Ordenada: " + string.Join(", ", frutas));

        frutas.Reverse(); // Invertir orden
        Console.WriteLine("Invertida: " + string.Join(", ", frutas));

        frutas.Clear(); // Vaciar la lista
        Console.WriteLine("Elementos después de Clear: " + frutas.Count);
    }
    #endregion
    
    #region Arreglos
    public static void RunArreglos()
    {
        // Si se ve un tipo de colección con [] es un arreglo, no una lista
        string[] frutas = ["Manzana", "Banana", "Cereza"];

        // Acceder por índice
        Console.WriteLine($"Primera fruta: {frutas[0]}");

        // Longitud del arreglo
        Console.WriteLine($"Cantidad de frutas: {frutas.Length}");

        // Recorrer el arreglo
        Console.WriteLine("Todas las frutas:");
        foreach (var fruta in frutas)
            Console.WriteLine(fruta);

        // Ordenar el arreglo
        Array.Sort(frutas);
        Console.WriteLine("Ordenadas: " + string.Join(", ", frutas));

        // Invertir el arreglo
        Array.Reverse(frutas);
        Console.WriteLine("Invertidas: " + string.Join(", ", frutas));
    }
    #endregion

    #region Diccionarios
    public static void RunDiccionarios()
    {
        Dictionary<int, string> capitales = new()
        {
            { 1, "Buenos Aires" },
            { 2, "Santiago" },
            { 3, "Lima" }
        };

        // Agregar un elemento
        capitales.Add(4, "Quito");

        // Modificar un valor
        capitales[2] = "Valparaíso";

        // Eliminar un elemento
        capitales.Remove(3);

        // Verificar si existe una clave
        Console.WriteLine("¿Contiene la clave 1? " + capitales.ContainsKey(1));

        // Recorrer solo claves
        Console.WriteLine("Claves:");
        foreach (var clave in capitales.Keys)
            Console.WriteLine(clave);

        // Recorrer solo valores
        Console.WriteLine("Valores:");
        foreach (var valor in capitales.Values)
            Console.WriteLine(valor);

        // Cantidad de elementos
        Console.WriteLine("Cantidad de elementos: " + capitales.Count);

        // Limpiar el diccionario
        capitales.Clear();
        Console.WriteLine("Elementos después de Clear: " + capitales.Count);
    }
    #endregion

    #region Tuplas
    public static void RunTuplas()
    {
        // Definir una tupla con nombres
        (int Id, string Nombre, decimal Precio) producto = (1, "Laptop", 1200.50m);

        // Acceder a los elementos por nombre
        Console.WriteLine($"ID: {producto.Id}, Nombre: {producto.Nombre}, Precio: {producto.Precio}");

        // Desempaquetar la tupla en variables
        var (id, nombre, precio) = producto;
        Console.WriteLine($"Desempaquetado - ID: {id}, Nombre: {nombre}, Precio: {precio}");

        // Tupla sin nombres (posicional)
        var tuplaSimple = (10, "Texto", true);
        Console.WriteLine($"Tupla simple: {tuplaSimple.Item1}, {tuplaSimple.Item2}, {tuplaSimple.Item3}");

        // Comparar tuplas
        var t1 = (x: 5, y: 7);
        var t2 = (x: 5, y: 7);
        Console.WriteLine($"¿Son iguales? {t1 == t2}");
    }
    #endregion

    #region Hashsets
    public static void RunHashsets()
    {
        HashSet<int> numeros = [1, 2, 3, 4, 5, 3]; // HashSet no permite duplicados

        // Agregar elementos
        numeros.Add(6);
        numeros.Add(3); // No se agrega porque ya existe

        // Eliminar elemento
        numeros.Remove(2);

        // Verificar si contiene un elemento
        Console.WriteLine("¿Contiene el 4? " + numeros.Contains(4));

        // Recorrer el HashSet
        Console.WriteLine("Elementos del HashSet:");
        foreach (var n in numeros)
            Console.WriteLine(n);

        // Unión de conjuntos
        var otros = new HashSet<int> { 4, 5, 6, 7 };
        numeros.UnionWith(otros); // Une ambos conjuntos
        Console.WriteLine("Unión: " + string.Join(", ", numeros));

        // Limpiar el HashSet
        numeros.Clear();
        Console.WriteLine("Elementos después de Clear: " + numeros.Count);
    }
    #endregion

    #region FrozenCollections
    public static void RunFrozenCollections()
    {
        // FrozenSet<T> es una colección inmutable (no se puede modificar después de crearla)
        var frutas = new[] { "Manzana", "Banana", "Cereza" };
        var frozen = frutas.ToFrozenSet();
        Console.WriteLine("Frutas en FrozenSet:");
        foreach (var fruta in frozen)
            Console.WriteLine(fruta);

        // Intentar modificar (esto no compila):
        // frozen.Add("Pera"); // Error: FrozenSet es inmutable

        // Verificar existencia
        Console.WriteLine("¿Contiene 'Banana'? " + frozen.Contains("Banana"));
    }
    #endregion
}