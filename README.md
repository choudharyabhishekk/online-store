# Online Electronics Store

The Online Electronics Store is a simple e-commerce platform built with ASP.NET Core MVC. It includes a user-friendly customer interface, an admin panel for store management, user authentication via ASP.NET Identity, and a mobile-responsive design using Bootstrap.

## Screenshots:


Homepage:


![image](https://github.com/user-attachments/assets/b57d5984-bc36-48af-af6b-f828307b723f)


Products Page:


![image](https://github.com/user-attachments/assets/f9c545ba-2166-4097-b97c-3ff65a5140b0)


## Features

### **Customer Features**
- **Browse Products**: View products by category, with details, images, and prices.
- **User Accounts**: Register, log in, and manage your account.
- **Shopping Cart**: Add, remove, and update items.
- **Checkout**: Complete purchases securely.
- **Responsive Design**: Mobile-friendly layout with Bootstrap.

### **Admin Features**
- **Dashboard**: View store metrics.
- **Manage Products**: Add, edit, and delete products.
- **Category Management**: Create and manage product categories.
- **User Management**: Manage user roles and accounts.
- **Order Management**: Track and update customer orders.

---

## **Technologies Used**
- **Framework**: ASP.NET Core MVC
- **Authentication**: ASP.NET Identity
- **Frontend**: HTML, CSS, JavaScript, and Bootstrap
- **Database**: SQL Server (via Entity Framework Core)
- **Server**: Kestrel Web Server
- **Tools**: Visual Studio, Git, and Postman (for testing)

---

## **Installation and Setup**

1. **Clone the repo:**
   ```bash
   git clone https://github.com/username/online-electronics-store.git
   cd online-electronics-store
   ```

2. **Set up the database:**
   - Update `appsettings.json` with your SQL Server connection.
   - Run:
     ```bash
     dotnet ef database update
     ```

3. **Run the app:**
   ```bash
   dotnet run
   ```
   Visit `https://localhost:5001/` or `http://localhost:5000/`.

---

## **Usage**

### **For Customers**
1. Register or log in.
2. Browse products, add to cart, and check out.

### **For Admins**
1. Log in with an admin account.
2. Access the admin panel to manage products, categories, users, and orders.

---

## **Folder Structure**
```
📁 Online-Electronics-Store
├── 📁 Controllers       # Handles requests
├── 📁 Models            # Database entities
├── 📁 Views             # Razor view files
├── 📁 wwwroot           # Static files (CSS, JS, images)
├── 📄 appsettings.json  # Configuration
├── 📄 Program.cs        # Application entry point
├── 📄 Startup.cs        # Configures middleware and services
```

---

## **Roles and Permissions**
- **Admin**: Full access to admin features.
- **User**: Can browse, shop, and check out.

---

## **Security**
- **Authentication**: Secure logins with ASP.NET Identity.
- **Authorization**: Role-based access for users and admins.
- **Password Protection**: Passwords are encrypted.



