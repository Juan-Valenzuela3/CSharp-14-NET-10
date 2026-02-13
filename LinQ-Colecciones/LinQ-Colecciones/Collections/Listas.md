## Arreglos
- Bloque de memoria estático
- Siempre tendrá el mismo tamaño de elementos, no puede crecer ni encogerse
- Es el más eficiente proque solo tiene los datos.
- Ideal cuando sabemos exáctamente cuántos datos tendremos

## Listas
- Bloque elástico
- Tiene tamaño dinámico
- La lista tiene un arreglo interno; cuando se llena, la lista crea un arreglo nuevo (normalmente del doble de tamaño), copia todos los elementos del viejo al nuevo y desecha el anterior.
- Uso: el estándar cuando no sabemos cúantos datos llegarán
- Se pueden optimizar para que reserve un espacio de un # determinado
var listaOptimizada = new List<int>(1_000_000);

## Tuplas
- Suelen confundirse con las colecciones porque agrupan datos, pero en realidad son estructuras de transporte
- Una lista puede llevar muchos vagones del mismo tipo, ua tupla es una mochila donde podemos introducir objetos de diferentes tipos para llevarlos de un punto A a un punto B rápidamente.
- Estructura ligera que permite múltiples valores de diferentes tipos en una sola unidad, sin tener que crear una clase formal
- Estructura fija, definimos cuántos elementos tiene al inicio
- Datos: heterogénea
- Propósito: transportar un grupo pequeño de datos relacionados
- Ligeros
- Nunca usar tuplas para mover datos a través de muchas capas de la aplicación, para esto están los DTOs. Las tuplas son para uso interno y rápido dentro de los métodos.


## Diccionario
- Dinámica, puede crecer
- Datos: homogéneos
- Propósito: almacenar y organizar muchos datos
- Pesados

