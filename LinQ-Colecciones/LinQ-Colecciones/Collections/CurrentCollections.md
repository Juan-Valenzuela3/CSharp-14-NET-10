3. ConcurrentCollections (Para procesamiento paralelo)
Cuando usas todos los núcleos de tu CPU para procesar esos millones de registros, las colecciones normales (List, Dictionary) fallan porque no son "Thread-Safe" (varios hilos intentando escribir a la vez corrompen la memoria).

ConcurrentDictionary: Para inserciones masivas desde múltiples hilos.

BlockingCollection: Para patrones Productor-Consumidor (un hilo descarga datos, otros los procesan).