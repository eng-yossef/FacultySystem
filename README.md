# 🎓 Faculty System

A full-featured Faculty Management System built with ASP.NET Core MVC, Entity Framework Core, and Identity. This system allows administrators to manage **Instructors, Courses, Departments**, and **Trainees**, with full CRUD functionality and secure user authentication and role management.

---

## ✨ Features

- ✅ Instructor Management (Create, Edit, Delete, View)
- 📸 Image Upload for Instructors (stored in `/wwwroot/images`)
- 🏛 Department & Course Management
- 👨‍🎓 Assign Students to Courses
- 🔐 Authentication & Authorization using ASP.NET Identity
- 👥 Role-Based Access Control (Admin, Instructor, Trainee, etc.)
- 🖼 Beautiful Bootstrap UI with Card-Based Layout
- 🔗 One-to-One Relationship between ApplicationUser and Instructor

---

## 🔧 Technologies Used

- ASP.NET Core MVC
- Entity Framework Core (Code-First)
- SQL Server / LocalDB
- ASP.NET Core Identity
- Bootstrap 5 (for styling)
- LINQ & Repository Pattern
- AutoMapper (optional)
- .NET 7 or 8

---

## 🗂 Project Structure

```plaintext
FacultySystem/
│
├── Models/                  # EF Models (Instructor, Course, Department, Trainee)
├── Views/                   # Razor Views for all entities
├── Controllers/             # MVC Controllers (CRUD + Auth)
├── wwwroot/images/          # Instructor uploaded images
├── Data/                    # ApplicationDbContext & Identity setup
├── Migrations/              # EF Core migration files
├── Program.cs               # App setup & configuration
├── appsettings.json         # Connection string & Identity settings
└── README.md                # This file
