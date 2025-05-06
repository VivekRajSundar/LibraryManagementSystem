# LibraryManagementSystem
A simple console-based Library Management System developed in C#, .NET 8, utilizing SQLite for data storage and Spectre.Console for rich console UI.

## 🛠️ Technologies Used
<p align="left">
  <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/csharp/csharp-original.svg" alt="C#" width="40" height="40" />
  <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/dotnetcore/dotnetcore-original.svg" alt=".NET" width="40" height="40" />
  <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/sqlite/sqlite-original.svg" alt="SQLite" width="40" height="40" />
</p>

## 📂 Folder Structure
```
LibraryManagementSystem/
├── LibraryManagementSystem.sln # Solution file
├── Program.cs               # Entry point of the application
├── Enums/
│ ├── AdminActivity.cs
│ ├── MemberActivity.cs
├── Models/                  # Models (e.g., Book, User)
│ ├── Book.cs
│ ├── User.cs
├── Services/                # Core business logic and services
│ ├── BookService.cs
│ ├── UserService.cs
│ ├── SessionManager.cs
├── Data/                    # Data access layer
│ ├── BookRepository.cs
│ ├── UserRepository.cs
│ ├── DbHelper.cs
├── Helpers/
│ ├── OutputHelper.cs        # Using Spectre Console Library
├── appsettings.json         # Configuration file
└── README.md                # Project documentation
```
## 🚀 Features
* User Registration & Login: Secure user authentication system.
* Book Management: Add, view, and manage books in the library.
* Borrow & Return Books: Users can borrow available books and return them.
* Interactive Console UI: Enhanced user experience using Spectre.Console.


