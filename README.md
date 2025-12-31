# Product Manager
**Aplikacja do zarządzania produktami (CRUD)**

Prosta aplikacja CRUD do zarządzania produktami, zaprojektowana z myślą o dalszym rozwoju oraz integracjach z zewnętrznymi systemami.

---

## 🚀 Uruchomienie aplikacji

1. W Visual Studio ustaw jednoczesne uruchamianie:
   - API
   - MVC

2. Uzupełnij poprawny **Connection String** w plikach konfiguracyjnych.

3. Uruchom migracje bazy danych.

4. Uruchom aplikację.

---

## 🏗 Architektura

Projekt oparty o:

- **Clean Architecture**
  - Domain
  - Application
  - Infrastructure
- **CQRS**
- Własna implementacja **Mediatora**

Zastosowana architektura:
- separuje odpowiedzialności pomiędzy warstwami,
- ułatwia rozwój i skalowanie aplikacji,
- upraszcza onboarding nowych programistów,
- jest przygotowana pod przyszłe integracje.

---

## 🎨 UI

- **.NET Core MVC**
- **JavaScript / jQuery**
- Operacje CRUD (dodawanie, edycja, usuwanie) realizowane w modalach

---

## 🔌 Integracje

Aplikacja przygotowana pod integracje z zewnętrznymi systemami (np. ERP: Dynamics 365, Comarch).

- Obsługa integracji realizowana przez `BackgroundService`
- Przetwarzanie danych oparte o **Channel** (obecnie w pamięci RAM)
- Docelowo możliwość integracji z szyną danych (np. Azure Service Bus)

> Implementacja integracji ma charakter koncepcyjny i prezentuje podejście architektoniczne.

---

## 🛠 Możliwe usprawnienia

- Dodanie **Outbox Pattern** dla niezawodnej obsługi eventów integracyjnych
- Bardziej restrykcyjna konfiguracja **CORS** (Origin / Headers / Methods)
- Wykorzystanie trwałej kolejki wiadomości (np. Azure Service Bus)

---

## 📌 Technologie

- .NET / .NET Core
- ASP.NET Core MVC
- Entity Framework Core
- JavaScript / jQuery
- Clean Architecture
- CQRS
