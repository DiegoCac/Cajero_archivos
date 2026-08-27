# Cajero_archivos
Proyecto final de archivos 2026
# Proyecto Cajero Automático

## 📌 Descripción
Este proyecto consiste en la implementación de un **Cajero Automático** en lenguaje **Java o C#**, diseñado para simular operaciones de retiro, depósito y administración de usuarios. El objetivo principal es que el estudiante aprenda a resolver problemas de programación, organizar sus ideas y almacenar información en archivos de texto.

---

## 🎯 Objetivos
- Desarrollar habilidades en programación orientada a objetos.
- Implementar almacenamiento y control de información mediante archivos.
- Simular operaciones reales de un cajero automático.
- Aplicar buenas prácticas de diseño y modularidad.

---

## 🏗️ Funcionalidades

### Área Administrativa
- **Creación de usuarios**: Registrar nombre, número de tarjeta (16 dígitos), PIN (4 dígitos), saldo y límite diario de retiro.
- **Inicializar cajero**: Ingresar cantidad de billetes disponibles (máx. Q10,000.00).
- **Agregar efectivo**: Añadir billetes al cajero (máx. Q30,000.00).
- **Modificar número de tarjeta**: Actualizar tarjeta manteniendo el límite diario.
- **Modificar límite de retiro**: Cambiar el monto máximo diario de un usuario.
- **Consulta de usuario**: Mostrar saldo, límite, cantidad retirada y último acceso.
- **Control de usuarios**: Reportes diarios de retiros, depósitos, cambios de PIN y último acceso.

### Área de Usuario
- **Cambiar PIN**: Actualizar el PIN ingresando tarjeta, PIN y token correctos.
- **Retiros**: Retirar cualquier cantidad (ej. Q123, Q239) verificando saldo y disponibilidad de billetes.
- **Depósitos**: Ingresar billetes por denominación y actualizar saldo.
- **Ver últimas transacciones**: Mostrar las últimas 5 operaciones (retiros y depósitos).
- **Ver saldo**: Consultar saldo actual y límite disponible para el día.

---

## 📂 Consideraciones Generales
- Todas las operaciones se registran en **archivos de texto** para control y auditoría.
- El proyecto debe incluir:
  - Documento de análisis y diseño (entradas, procesos, salidas, diagramas).
  - Código fuente en **Java o C#**.
  - Manual técnico y manual de usuario.
  - Documentación completa.
---

## 🚀 Tecnologías
- Lenguaje: **Java** o **C#**
- IDE recomendado: **Visual Studio** o **NetBeans**
- Almacenamiento: Archivos de texto (`.txt`)

---

## 📖 Ejecución
1. Clonar el repositorio:
   ```bash
   git clone https://github.com/DiegoCac/Cajero_archivos.git
