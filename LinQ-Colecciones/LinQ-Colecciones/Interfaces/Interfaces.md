# INTERFACES

## IEnumerable<T> (Iterador)

- Es la interfaz más básica y la madre de todas.
Solo permite una cosa: ir hacia adelante un elemento a la vez (foreach)
- Comportamiento: usa ejecuión diferida, no sabe cuántos elementos hay ni permite modificarlos
- Uso en millones de registros: es la mejor para streaming (transmitir) de datos desde un archivo o API, porque no carga todo en RAM

## ICollecion<T> (Manipulador)

- Hereda de IEnumerable, pero añade músculo
- Permite saber el conteo (count), agregar (add) y eliminar (Remove) elementos.
- Comportamiento: Normalmente implica que los datos ya están en memoria (RAM)
- Uso en millones de registros: Úsarla cuando necesite modificar la estructura de los datos que ya se descargaron.

## IQueryable<T> (Traductor)

- Hereda de IEnumerable, pero en lugar de ejecutar el código en C#, traduce  la consulta LINQ a otro lenguaje (como SQL).
- Comportamiento: la ejecución ocurre en el servidor de base de datos, no en mi aplicación
- Uso en ñmillones de registros: Si se filtra un millón de registros con IEnumerable, C# descarga el millón y luego filtra. Con IQueryable, el servidor filtra y solo envía los que se necesitan.

public class DataService(MyDbContext context)
{
    // IQueryable: El filtro "Where" se convierte en SQL. 
    // La base de datos hace el trabajo pesado.
    public IQueryable<User> GetActiveUsers() => 
        context.Users.Where(u => u.IsActive);

    // IEnumerable: El filtro se ejecuta en la RAM de tu App.
    // ¡Peligro si hay millones de registros!
    public IEnumerable<User> FilterInRam(IEnumerable<User> users) => 
        users.Where(u => u.Name.StartsWith("A"));

    // ICollection: Para cuando necesitas modificar la lista
    public void ProcessCollection(ICollection<User> users)
    {
        if (users.Count > 0) // ICollection sabe su tamaño
            users.Add(new User("New User"));
    }
}

## IAsyncEnumerable

- Permite que el foreach sea asíncrono, así que mientras se espera a que la base de datos envíe el siugiente bloque de 1000 registros, tu servidor puede atender otras peticiones.