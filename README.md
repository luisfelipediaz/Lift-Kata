# Lift Kata - Práctica de TDD

Una kata diseñada para practicar **Test-Driven Development (TDD)** construyendo un sistema de ascensores paso a paso.

## Sobre TDD y esta Kata

**Test-Driven Development** es una técnica de desarrollo donde escribes las pruebas ANTES que el código de producción. El ciclo es:

1. **🔴 Red**: Escribe un test que falla
2. **🟢 Green**: Escribe el código mínimo para que pase
3. **🔵 Refactor**: Mejora el código manteniendo los tests en verde

### ¿Por qué esta kata es ideal para TDD?

- **Requisitos claros e incrementales**: Cada característica del ascensor es pequeña y bien definida
- **Feedback inmediato**: Sabes cuando algo funciona o no
- **Diseño emergente**: El código evoluciona naturalmente desde lo simple hacia lo complejo
- **Errores de estado**: El ascensor tiene muchos estados (piso actual, puertas, dirección) que son perfectos para practicar TDD

## Requisitos del Ascensor

### Funcionalidad Básica

- Un ascensor se mueve entre varios pisos
- Tiene un panel de botones que los pasajeros presionan para solicitar pisos (requests)
- Las personas pueden llamar al ascensor desde otros pisos. Una llamada (call) tiene un piso y una dirección deseada
- Tiene puertas que pueden estar abiertas o cerradas
- Cumple un **request** cuando se mueve al piso solicitado y abre las puertas
- Cumple un **call** cuando se mueve al piso correcto, está por ir en la dirección solicitada, y abre las puertas
- **Solo puede moverse entre pisos si las puertas están cerradas**

### Características Adicionales

Cuando tengas un ascensor funcionando, puedes abordar estas características:

- Puede haber más de un ascensor
- Solo un ascensor necesita responder a cada llamada
- En cada piso hay un monitor sobre cada puerta. Mientras el ascensor se mueve, muestra en qué piso está
- Cuando el ascensor se detiene para responder una llamada, el monitor muestra en qué dirección irá
- Al cumplir una llamada, el ascensor relevante hace un 'DING' al abrir las puertas

## Cómo Empezar con TDD

### Primer Test: Lo más simple posible

```csharp
[Fact]
public void Lift_CanOpenDoors()
{
    var lift = new Lift();

    lift.OpenDoors();

    lift.AreDoorsOpen.Should().BeTrue();
}
```

**🔴 Red**: Este test fallará porque `Lift` no existe
**🟢 Green**: Crea la clase `Lift` con `OpenDoors()` y `AreDoorsOpen`
**🔵 Refactor**: ¿Hay algo que mejorar? Aún no

### Progresión Sugerida (Baby Steps)

1. El ascensor puede abrir puertas
2. El ascensor puede cerrar puertas
3. El ascensor puede subir un piso
4. El ascensor puede bajar un piso
5. El ascensor no puede moverse con puertas abiertas
6. El ascensor puede recibir requests (botones internos)
7. El ascensor se mueve al piso solicitado
8. El ascensor puede recibir calls (botones externos)
9. El ascensor respeta la dirección de las llamadas
10. El ascensor maneja múltiples requests en cola
11. El sistema tiene límites de pisos
12. ... y así sucesivamente

## Convención de Commits para TDD

Este proyecto usa emojis para marcar cada fase del ciclo TDD:

- **🔴 Red**: Test fallando
  ```
  🔴 El ascensor no puede moverse con puertas abiertas
  ```

- **🟢 Green**: Test pasando
  ```
  🟢 El ascensor no puede moverse con puertas abiertas
  ```

- **🔵 Refactor**: Mejora sin cambiar comportamiento
  ```
  🔵 Extraer lógica de validación de puertas a método privado
  ```

### ¿Por qué hacer commit de cada fase?

- **Historial legible**: Puedes ver exactamente cómo evolucionó el código
- **Seguridad**: Si un refactor sale mal, vuelves al último 🟢
- **Aprendizaje**: Puedes revisar tu proceso y mejorarlo
- **Evidencia**: Demuestra que seguiste TDD disciplinadamente

## Ejemplo de un Ciclo Completo Red-Green-Refactor

### 🔴 Red: Escribir el test que falla

```csharp
[Fact]
public void Lift_CantMoveUpIfTheDoorsAreOpen()
{
    var lift = new Lift();
    lift.OpenDoors();

    var action = () => lift.MoveUp();

    action.Should().Throw<InvalidOperationException>();
}
```

Ejecuta el test → **Falla** porque `MoveUp()` no valida las puertas.

```bash
git add .
git commit -m "🔴 El ascensor no puede moverse con puertas abiertas"
```

### 🟢 Green: Hacer que pase (código mínimo)

```csharp
public void MoveUp()
{
    if (AreDoorsOpen)
        throw new InvalidOperationException("Cannot move with doors open");

    CurrentFloor++;
}
```

Ejecuta el test → **Pasa** ✓

```bash
git add .
git commit -m "🟢 El ascensor no puede moverse con puertas abiertas"
```

### 🔵 Refactor: Mejorar el diseño

```csharp
public void MoveUp()
{
    ValidateDoorsAreClosed();
    CurrentFloor++;
}

public void MoveDown()
{
    ValidateDoorsAreClosed();
    CurrentFloor--;
}

private void ValidateDoorsAreClosed()
{
    if (AreDoorsOpen)
        throw new InvalidOperationException("Cannot move with doors open");
}
```

Ejecuta los tests → **Todos pasan** ✓

```bash
git add .
git commit -m "🔵 Extraer validación de puertas a método privado"
```

## Estructura del Proyecto

```
Lift-Kata/
├── Domain/
│   ├── Lift.cs              # Clase principal del ascensor
│   ├── LiftSystem.cs        # Sistema que maneja múltiples ascensores
│   └── Extensions/          # Métodos de extensión para mejorar semántica
├── Lift.Tests/
│   ├── LiftTests.cs         # Tests unitarios del Lift
│   └── LiftSystemTests.cs   # Tests de integración del sistema
└── Cosmos.Katas.sln
```

### Tecnologías y Dependencias

- **.NET 9**: Framework principal
- **xUnit**: Framework de testing
- **AwesomeAssertions**: Assertions fluidas (`.Should().BeTrue()`)

### Ejecutar los Tests

```bash
dotnet test
```

Para ver tests específicos:
```bash
dotnet test --filter "FullyQualifiedName~LiftUnitTests"
```

## Tips Específicos de TDD para esta Kata

### 1. Manejo de Estado

El ascensor tiene múltiples estados interdependientes:
- Piso actual
- Estado de las puertas
- Dirección
- Cola de requests/calls

**Tip**: Empieza con el estado más simple (puertas) antes de abordar la cola de requests.

### 2. Testing de Excepciones

Muchas reglas se expresan como "no puede hacer X si Y":

```csharp
var action = () => lift.MoveUp();
action.Should().Throw<InvalidOperationException>();
```

**Tip**: Valida el mensaje de la excepción para hacer tests más específicos.

### 3. Organización de Tests

- **Tests Unitarios** (`LiftTests.cs`): Prueban comportamientos individuales
- **Tests de Sistema** (`LiftSystemTests.cs`): Prueban escenarios completos

**Tip**: Empieza con tests unitarios. Los tests de sistema vienen después.

### 4. Evita estos Anti-Patrones

❌ **Escribir mucho código antes de testear**
```csharp
// NO hagas esto: implementar 5 métodos antes de escribir tests
```

✅ **Un test, un cambio mínimo**
```csharp
// SÍ: un test → código mínimo → siguiente test
```

❌ **Tests que prueban demasiado**
```csharp
// NO: un test que valida 10 cosas diferentes
```

✅ **Tests enfocados**
```csharp
// SÍ: cada test valida una sola regla de negocio
```

## Progresión de la Kata (Niveles)

### 📗 Nivel Básico
- [ ] El ascensor tiene puertas que abren/cierran
- [ ] El ascensor se mueve un piso arriba/abajo
- [ ] No puede moverse con puertas abiertas
- [ ] Acepta requests de pisos
- [ ] Se mueve al piso solicitado y abre puertas

### 📘 Nivel Intermedio
- [ ] Maneja múltiples requests en cola
- [ ] Acepta calls con dirección
- [ ] Respeta la dirección de las llamadas
- [ ] Tiene límites de pisos (no puede ir más allá)
- [ ] Optimiza el recorrido (no va y viene innecesariamente)

### 📕 Nivel Avanzado
- [ ] Sistema con múltiples ascensores
- [ ] Solo un ascensor responde cada llamada
- [ ] Monitores que muestran piso actual
- [ ] Monitores que muestran dirección
- [ ] Hace 'DING' al cumplir una llamada

## Preguntas para Reflexionar

Después de completar la kata, reflexiona sobre:

1. **Diseño**: ¿Cómo emergió el diseño? ¿Fue diferente de lo que imaginabas al inicio?
2. **Tests**: ¿Cuántos tests tienes? ¿Cuántas líneas de código de producción por test?
3. **Confianza**: ¿Qué tan seguro te sientes haciendo cambios? ¿Por qué?
4. **Baby Steps**: ¿En qué momentos diste pasos demasiado grandes? ¿Qué pasó?
5. **Refactoring**: ¿Cuándo refactorizaste? ¿Por qué en esos momentos?
6. **Fallos**: ¿Qué tests fallaron inesperadamente? ¿Qué aprendiste?

## Recursos Adicionales

- [Kata-Log: Lift Kata](https://kata-log.rocks/lift-kata)
- [Samman Coaching: Lift](https://sammancoaching.org/kata_descriptions/lift.html)
- [Test-Driven Development by Kent Beck](https://www.amazon.com/Test-Driven-Development-Kent-Beck/dp/0321146530)

## Agradecimientos

Esta kata está descrita en [Kata-Log](https://kata-log.rocks/lift-kata), aunque ha sido modificada ligeramente para este proyecto.

---

**¡Feliz práctica de TDD!** Recuerda: 🔴 → 🟢 → 🔵 → Repetir