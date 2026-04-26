# BirthdayTracker 🎂
### Agenda de Cumpleaños – Proyecto Final Programación I (C#)
**Autor:** Luis Angel Guillen  
**Tecnología:** C# 13 / .NET 10 / SQL Server Express 2022

---

## 📁 Estructura del Proyecto

```
BirthdayTracker/
├── BirthdayTracker.Models/        → Clases de dominio (BaseEntity, Contact, VipContact...)
├── BirthdayTracker.DataAccess/    → Repositorios ADO.NET (conexión a SQL Server)
├── BirthdayTracker.Business/      → Servicios de negocio y validaciones
├── BirthdayTracker.Presentation/  → Menús de consola y punto de entrada
├── Database_BirthdayTrackerDB.sql → Script SQL para crear la base de datos
└── BirthdayTracker.sln            → Solución de Visual Studio
```

## ⚙️ Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server Express 2022 (instancia `.\SQLEXPRESS`)
- Visual Studio 2022 o VS Code

## 🚀 Pasos para Ejecutar

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/TuUsuario/BirthdayTracker.git
   cd BirthdayTracker
   ```

2. **Crear la base de datos:**  
   Abre SQL Server Management Studio (SSMS), conéctate a `.\SQLEXPRESS`  
   y ejecuta el archivo `Database_BirthdayTrackerDB.sql`.

3. **Restaurar paquetes y compilar:**
   ```bash
   dotnet restore
   dotnet build
   ```

4. **Ejecutar:**
   ```bash
   dotnet run --project BirthdayTracker.Presentation
   ```

## 🧩 Conceptos de POO Aplicados

| Concepto | Dónde se aplica |
|---|---|
| Clases y Objetos | `Contact`, `BirthdayNote`, `ReminderLog` |
| Clase Abstracta | `BaseEntity` con método abstracto `GetDisplayName()` |
| Herencia | `VipContact : Contact` |
| Constructores | Múltiples constructores en `Contact` y `BirthdayNote` |
| Sobrecargas | `ContactService.Search()` × 3 variantes |
| Encapsulamiento | Propiedades con validaciones en `ContactService` |

## 🔗 Enlaces

- **Repositorio:** https://github.com/TuUsuario/BirthdayTracker
- **Presentación:** [Agregar enlace]
- **Video:** [Agregar enlace]
