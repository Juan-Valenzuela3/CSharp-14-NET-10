
# Son clases genéricas que permiten almacenar una colección de elementos del mismo tipo. Proporcionan métodos para agregar, eliminar, buscar y ordenar elementos.
 
## LIST<T> VS HASHSET<T>

Las estructuras subyacentes (la memoria, el acceso y la complejidad)?

1. List<T> = Arreglos: Es una estructura secuencial. Recorre cada uno de los elementos para encontrar el valor buscado. En una lista de milones de elementos, el tiempo de búsqueda puede ser considerable. Complejidad: $O(n)
 
Métodos clave:
- Add(T item): Inserta al final. Si la capacidad se llena, C# crea un arreglo el doble de grande y copia todo ($O(n)$ en el momento del crecimiento, $O(1)$ normalmente)
- this[int index]: El indexador para acceso directo por posición ($O(1)$).       
- Remove(T item): Muy lento en listas grandes, porque debe desplazar todos los elementos posteriores para cerrar el hueco ($O(n)$).

2. HashSet<T>: Asigna una clave hash a cada elemento. En vez de recorrer toda la colección, calcula el hash del elemento buscado y accede directamente a la ubicación correspondiente.
 
Métodos clave:
- Add(T item): Calcula el hash, busca el balde y guarda. Si ya existe, devuelve false y no lo agrega.
- Contains(T item): La estrella del rendimiento. Calcula el hash y va directo al balde ($O(1)$).
- UnionWith / IntersectWith: Operaciones de conjuntos matemáticos optimizadas.
 
List<string> listaUsuarios = ["Ana", "Pedro", "Luis"];
HashSet<string> setUsuarios = ["Ana", "Pedro", "Luis"];
