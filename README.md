# README — Simulador Sistémico de Agua (MVP)

## 1. Descripción general
Este sistema es un **simulador simple basado en dinámica de sistemas** para analizar la gestión de agua en barrios periurbanos.  
Modela el sistema hídrico como:

- **Depósito (Stock):** nivel de agua del tanque comunal.  
- **Flujos (Flows):** entrada periódica por cisterna y salida por consumo del barrio.  
- **Retroalimentaciones (Feedback Loops):**
  - **Bucle reforzador:** acaparamiento (aumento de consumo cuando hay escasez).
  - **Bucle balanceador:** racionamiento (reducción de consumo cuando el nivel está crítico).

El simulador permite observar **comportamiento dinámico emergente**, como estabilidad, ciclos de escasez o colapso, según los parámetros ingresados.

## 2. Cómo usar el sistema (flujo básico)
1. Ingresar los parámetros de simulación.
2. Activar o no los bucles sistémicos.
3. Presionar **Simular**.
4. Revisar tabla y gráfico.
5. Exportar o importar parámetros si se desea.

## 3. Explicación de cada campo

### Depósito (Stock)
- **Nivel inicial (L):** estado inicial del tanque.
- **Capacidad tanque (L):** límite físico del depósito.

### Flujos
- **Consumo base (L/día):** flujo de salida normal.
- **Frecuencia cisterna (días):** intervalo entre recargas.
- **Volumen cisterna (L):** cantidad agregada por recarga.

### Horizonte temporal
- **Duración simulación (días):** cantidad total de días simulados.

### Retroalimentación
- **Umbral reforzador (%):** nivel donde inicia acaparamiento.
- **Umbral balanceador (%):** nivel donde inicia racionamiento.
- **Incremento acaparamiento (%):** intensidad reforzadora.
- **Reducción racionamiento (%):** intensidad balanceadora.

### Checkboxes
- **Activar reforzador**
- **Activar balanceador**

## 4. Funciones

### Simular
Ejecuta:
Nivel(t+1) = Nivel(t) + Entrada(t) - Consumo(t)

### Exportar Inputs
Guarda parámetros actuales en un archivo JSON.

### Importar Inputs
Carga parámetros desde un archivo JSON.

## 5. Interpretación de resultados
- Curva estable → equilibrio dinámico.  
- Caídas crecientes → reforzador dominante.  
- Recuperación tras caída → balanceador efectivo.  
- Nivel 0 repetido → escasez estructural.

## 6. Recomendaciones
- Probar escenarios con y sin bucles.
- Cambiar un parámetro a la vez.
- Guardar escenarios con nombres descriptivos.

