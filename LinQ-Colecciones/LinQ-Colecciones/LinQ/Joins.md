## Join
- Estandar - Inner Join
- Devuelve los elementos que tienen una coincidencia en ambas colecciones. Si un elemento no tiene pareja en la otra tabla, desaparece del resultado.

## Left Join
- Prioridad a la izquierda

Imagina que tienes dos listas en papel:
Lista A (Izquierda): Una lista de Personas.
Lista B (Derecha): Una lista de Entradas de Cine.

El "Left Join" dice: "Quiero ver a TODAS las personas de mi lista, tengan entrada o no".
Si la persona tiene una entrada, pones el número de la entrada a su lado.
Si la persona no tiene entrada, dejas el espacio en blanco (eso es el null en programación).

Resultado: Nunca vas a perder a una persona de tu lista original, pero verás muchos espacioes vacíos en la columna de entradas.

## Right Join
- Prioridad a la derecha
Dice: quiero ver toda las entradas de cine que compré, tengan dueño o no.
Si la entrada ya se le dio a alguien, pones el nombre de la persona al lado.
Si la entrada está guardada y nadie la tiene, pones "Sin dueño al lado (otro null).

Resultado: Nunca vas a perder una entrada, pero verás algunas entradas que no tienen ninguna persona asociada

## Zip
- Une por posición. Toma el primer elemnto de A y el primero de B, luego el segundo de A con el segundo de B, y aí sucesivamente. Es extremadamenteo rápido para compbinar listas paralelas de igual tamaño.

# OPTIMIZADORES

## CountBy
- permite hacer el conteo sin crear grupos nuevos, ahorrando muchísima RAM

IntersectBy, ExceptBy y UnionBy??

## Span
- Es como un bisturí: permite mirar un pedazo pequeño de un arreglo gigante sin copiarlo