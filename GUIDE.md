# Análisis y posibles tests

Análisis de comportamientos y capacidades que debe tener un ascensor y un sistema de ascensores.
(un ascensor inicia con las puertas abiertas)

1. Un ascensor puede abrir las puertas
2. Un ascensor puede cerrar las puertas
3. Un ascensor puede moverse entre pisos
4. Un ascensor no puedo moverse fuera de los limites de pisos
5. Un ascensor puede reportar el piso actual
6. Un ascensor no puede moversi si las puertas están abiertas
7. Un ascensor puede recibir un request (solicitud desde dentro del ascensor)
8. Un ascensor puede recibir más de un request
8. Un ascensor puede recibir un call (una solicitud de desplazamiento hacia un piso con una intención de dirección (arriba o abajo))
9. Un ascensor puede recibir más de un call
9. Un ascensor termina un request cuando llega al piso solicitado y abre las puertas

Necesitamos una manera de simular el paso del tiempo, por lo general esto es llamado "tick" una unidad de desplazamiento temporal.

¿Un ascensor recibe llamados directos, los botones de los pisos están conectados directamente al ascensor o hay un "sistema" que lo administra?
¿Este posible actor (sistema) debe manejar las unidades de tiempo?
¿Un ascensor se mueve solo, sin poleas o un motor (sistema) que es quien lo empuja?
¿Este sistema es el encargado de mover a los ascensores?
¿El sistema es quien hace el ring?