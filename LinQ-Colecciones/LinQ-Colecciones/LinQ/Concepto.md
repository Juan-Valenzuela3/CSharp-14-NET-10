# LinQ (Language integrated Query)

- Una consulta Linq NO se ejecuta en el momento en que se escribe
- Ejecución diferida: no se procesan los datos cuando se define la consulta, sino solo cuando se itera sobre ellos (foreach - ToList)
- Si se intenta filtrar 10 millones de registros con Where, Select, etc; LINQ crearaá una lista nueva en cada paso. En lugar de eso LINQ, crea un objeto IEnumerable, los datos solo fluyen a través de ese plano cuando alguien los pide.
- Solo usar ToList o ToArray al final de cada solicitud y solo si es estrictamente necesario.

# Métodos

## Count

- Lee todos los elementos de una lista y devuelve la cantidad solo hasta haberlos contado todos.

## Any

- Lee solo el primer elemento de una colección y si existe uno, devuelve true

## Chunck

- Uno de los métodos de extensión más potentes y modernos. Añadido en .NET 6 y perfeccionado en las versiones actuales .NET 10
- Procesamiento por lotes, mejor aliado
- Cuando se tienen millones de registros, procesarlos uno por uno es ineficientes por la cantidad de llamadas a la DB. Procesarlos todos a la vez puede agotar la memoria. Chunck(size) divide una secuencia masiva en trozos (arreglos) de un tamaño fijo
- Chunc es lineal, recorre la lista una sola vez y va entregando los pedazos
- La RAM solo ocupará lo que mida el Chunk
- Paralelismo: se puede combinar .Chunk con Parallel.ForEachAsync para procesar múltiples lotes al mismo tiempo en diferentes núcles del CPU
- Retorno: cada trozo de devuelve .Chunk es un Arreglo, lo cua es ideal proque los arreglos son las estructuras más rápidas para iterar en C#

## Where

- Filtrar elementos

## Select

- Transformar la data

## Take

- Es un limitador de seguridad. Dice simplemente los elementos que deseo obtener y una vez obtenidos, detiene la ejecución por completo.
- En versiones modernas se pueden utilizar rangos
Take (50..100) / Take(^10..)
- TakeWhile(condition): sigue toamdno elementos mientras se cumpla una condición. En el momento en que un elemento NO la cumple, se detiene (aunque haya más elementos válidos después)

## Distinc()

- Eliminar duplicados

## SelectMany()

- Si se tiene una lista de listas, selectMany aplana todo en una sola lista

## OrderBy

- Es una de las operaciones más costosas en LINQ, pues necesita ver todos los datos antes de poder entregar el primero. 

## Operadores de Materialización

- Con las interfaces solo se tienen promesas de datos. Cuando se llaman a estos operadores, la promesa se rompe y los datos tienen que ser cargados en la RAM sí o sí.

### ToList
- Es el más común, crea una lista en memoria.
- ¿Cuándo usarlo?: cuando se necesita modificar la colección (añadir o quitar elementos) o cuando necesitamos acceder a los datos varias veces y no queremos que LINQ vuelva a buscarlos a la base de datos cada vez.
- Costo: Reserva espacio extra en memoria (capacidad) para permitir el crecimiento de la lista

### ToArray
- Crea un arreglo de tamaño fijo T[]
- ¿Cuándo usarlo?: cuando sabemos que los datos no van a cambiar de tamaño y queremos el máximo rendimiento de lectura. Los arreglos son más ligeros que las listas proque no tienen la sobrecarga de capacidad extra.
- Úsarlo para pasar datos a métodos que solo van a leer

### ToDictionary
- ¿Cuándo usarlo?: Cuando tenemos millones de registros y necesitamos buscar uno específico por ID miles de veces
- Impacto: Es el más pesado en Ram, pero transforma una búsqueda lenta en una búsqueda instantánea.