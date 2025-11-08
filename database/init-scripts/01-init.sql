-- Crear tablas según tu esquema Northwind
CREATE TABLE Categories (
    CategoryID SERIAL PRIMARY KEY,
    CategoryName VARCHAR(255) NOT NULL,
    Description TEXT,
    Picture TEXT
);

CREATE TABLE Suppliers (
    SupplierID SERIAL PRIMARY KEY,
    CompanyName VARCHAR(255) NOT NULL,
    ContactName VARCHAR(255),
    ContactTitle VARCHAR(255),
    Address TEXT,
    City VARCHAR(100),
    Region VARCHAR(100),
    PostalCode VARCHAR(20),
    Country VARCHAR(100),
    Phone VARCHAR(50),
    Fax VARCHAR(50),
    HomePage VARCHAR(500)
);

CREATE TABLE Products (
    ProductID SERIAL PRIMARY KEY,
    ProductName VARCHAR(255) NOT NULL,
    SupplierID INTEGER NOT NULL,
    CategoryID INTEGER NOT NULL,
    QuantityPerUnit VARCHAR(100),
    UnitPrice DECIMAL(10,2),
    UnitsInStock INTEGER NOT NULL,
    UnitsOnOrder INTEGER NOT NULL,
    ReorderLevel SMALLINT NOT NULL,
    Discontinued BOOLEAN NOT NULL,
    
    CONSTRAINT fk_products_categories 
        FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),
    CONSTRAINT fk_products_suppliers 
        FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID)
);

CREATE TABLE Employees (
    EmployeeID SERIAL PRIMARY KEY,
    LastName VARCHAR(255) NOT NULL,
    FirstName VARCHAR(255) NOT NULL,
    Title VARCHAR(255),
    TitleOfCourtesy VARCHAR(64),
    BirthDate DATE,
    HireDate DATE,
    Address TEXT,
    City VARCHAR(100),
    Region VARCHAR(100),
    PostalCode VARCHAR(20),
    Country VARCHAR(100),
    HomePhone VARCHAR(50),
    Extension VARCHAR(10),
    Photo TEXT,
    Notes TEXT,
    ReportsTo INTEGER,

    CONSTRAINT fk_employees_reportsto 
        FOREIGN KEY (ReportsTo) REFERENCES Employees(EmployeeID)
        ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE Shippers (
    ShipperID SERIAL PRIMARY KEY,
    CompanyName VARCHAR(255) NOT NULL,
    Phone VARCHAR(50)
);

CREATE TABLE Customers (
    CustomerID SERIAL PRIMARY KEY,
    CompanyName VARCHAR(255) NOT NULL,
    ContactName VARCHAR(255),
    ContactTitle VARCHAR(255),
    Address TEXT,
    City VARCHAR(100),
    Region VARCHAR(100),
    PostalCode VARCHAR(20),
    Country VARCHAR(100),
    Phone VARCHAR(50),
    Fax VARCHAR(50)
);

CREATE TABLE Orders (
    OrderID SERIAL PRIMARY KEY,
    CustomerID INTEGER,
    EmployeeID INTEGER,
    OrderDate DATE,
    RequiredDate DATE,
    ShippedDate DATE,
    ShipVia INTEGER,
    Freight DECIMAL(10,2),
    ShipName VARCHAR(255),
    ShipAddress TEXT,
    ShipCity VARCHAR(100),
    ShipRegion VARCHAR(100),
    ShipPostalCode VARCHAR(20),
    ShipCountry VARCHAR(100),
    
    CONSTRAINT fk_orders_customers 
        FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    CONSTRAINT fk_orders_employees 
        FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID),
    CONSTRAINT fk_orders_shippers 
        FOREIGN KEY (ShipVia) REFERENCES Shippers(ShipperID)
);

CREATE TABLE Order_Details (
    OrderID INTEGER,
    ProductID INTEGER,
    UnitPrice DECIMAL(10,2),
    Quantity SMALLINT,
    Discount REAL,
    
    PRIMARY KEY (OrderID, ProductID),
    CONSTRAINT fk_order_details_orders 
        FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    CONSTRAINT fk_order_details_products 
        FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- -- Insertar categorías iniciales
INSERT INTO Categories (CategoryName, Description) VALUES
-- ('SERVIDORES', 'Productos relacionados con servidores y infraestructura'),
-- ('CLOUD', 'Soluciones y servicios en la nube'),
('ELECTRONICS', 'Dispositivos electrónicos y gadgets'),
 ('SOFTWARE', 'Aplicaciones y programas informáticos');

-- -- Insertar algunos suppliers de ejemplo
 INSERT INTO Suppliers (CompanyName, ContactName, ContactTitle, City, Country) VALUES
 ('Tech Supply Co.', 'Juan Pérez', 'Gerente de Ventas', 'Bogotá', 'Colombia'),
 ('Server Experts', 'María García', 'Directora Técnica', 'Medellín', 'Colombia'),
 ('Cloud Solutions', 'Carlos López', 'CEO', 'Cali', 'Colombia');

-- Crear índices para mejorar performance
CREATE INDEX idx_products_category ON Products(CategoryID);
CREATE INDEX idx_products_supplier ON Products(SupplierID);
CREATE INDEX idx_products_name ON Products(ProductName);
CREATE INDEX idx_orders_customer ON Orders(CustomerID);
CREATE INDEX idx_orders_employee ON Orders(EmployeeID);

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "AspNetRoles" (
    "Id" text NOT NULL,
    "Name" character varying(256) NULL,
    "NormalizedName" character varying(256) NULL,
    "ConcurrencyStamp" text NULL,
    CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetUsers" (
    "Id" text NOT NULL,
    "Name" text NOT NULL,
    "LastName" text NOT NULL,
    "IsActive" boolean NOT NULL,
    "UserName" character varying(256) NULL,
    "NormalizedUserName" character varying(256) NULL,
    "Email" character varying(256) NULL,
    "NormalizedEmail" character varying(256) NULL,
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text NULL,
    "SecurityStamp" text NULL,
    "ConcurrencyStamp" text NULL,
    "PhoneNumber" text NULL,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone NULL,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL,
    CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetRoleClaims" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "RoleId" text NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserClaims" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "UserId" text NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text NULL,
    "UserId" text NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
    "UserId" text NOT NULL,
    "RoleId" text NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserTokens" (
    "UserId" text NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text NULL,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "RefreshTokens" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "UserId" text NULL,
    "Token" text NULL,
    "JwtId" text NULL,
    "IsUsed" boolean NOT NULL,
    "IsRevoked" boolean NOT NULL,
    "ExpireDate" timestamp with time zone NOT NULL,
    "CreatedBy" integer NULL,
    "CreatedDate" timestamp with time zone NULL,
    "LastModifiedBy" integer NULL,
    "LastModifiedDate" timestamp with time zone NULL,
    CONSTRAINT "PK_RefreshTokens" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RefreshTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id")
);

INSERT INTO "AspNetRoles" ("Id", "ConcurrencyStamp", "Name", "NormalizedName")
VALUES ('df8d34db-8932-45f9-83b9-1f81e96fd1ee', 'e39bf853-1df8-4dd1-9522-c23b87017f40', 'ADMINISTRADOR', 'ADMINISTRADOR');

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");

CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");

CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");

CREATE INDEX "IX_RefreshTokens_UserId" ON "RefreshTokens" ("UserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251107025119_InitIdentity', '7.0.14');

COMMIT;

