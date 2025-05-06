# LibraryManagementSystem
A simple console-based Library Management System developed in C#, .NET 8, utilizing SQLite for data storage and Spectre.Console for rich console UI.

🛠️ Technologies Used
<p align="left"> 
  <a href="https://learn.microsoft.com/en-us/dotnet/csharp/"> 
    <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/csharp/csharp-original.svg" alt="C#" width="40" height="40"/>
  </a>&nbsp; 
  <a href="https://dotnet.microsoft.com/en-us/"> 
    <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/dot-net/dot-net-original.svg" alt=".NET" width="40" height="40"/> 
  </a>&nbsp; 
  <a href="https://www.sqlite.org/index.html"> 
    <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/sqlite/sqlite-original.svg" alt="SQLite" width="40" height="40"/> 
  </a>&nbsp; 
  <a href="https://spectreconsole.net/"> 
    <img src="https://spectreconsole.net/img/logo.png" alt="Spectre.Console" width="40" height="40"/> 
  </a> 
</p>

## 📂 Folder Structure
LibraryManagementSystem/
├── LibraryManagementSystem.sln # Solution file
├── Program.cs # Entry point of the application
├── Models/ # Entity classes (e.g., Book, User)
│ ├── Book.cs
│ ├── User.cs
│ └── BorrowRecord.cs
├── Data/ # Database context and seed data
│ └── LibraryContext.cs
├── Services/ # Core business logic and services
│ ├── BookService.cs
│ ├── UserService.cs
│ └── BorrowService.cs
├── Repositories/ # Data access layer
│ ├── BookRepository.cs
│ ├── UserRepository.cs
│ └── BorrowRepository.cs
├── Migrations/ # EF Core migrations (auto-generated)
│ └── [timestamp]_InitialCreate.cs
├── appsettings.json # Configuration file
└── README.md # Project documentation

🚀 Features
* User Registration & Login: Secure user authentication system.
* Book Management: Add, view, and manage books in the library.
* Borrow & Return Books: Users can borrow available books and return them.
* Interactive Console UI: Enhanced user experience using Spectre.Console.


