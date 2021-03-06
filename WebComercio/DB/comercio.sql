USE [master]
GO
/****** Object:  Database [comercio]    Script Date: 11/12/2021 17:24:20 ******/
CREATE DATABASE [comercio]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'comercio', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS02\MSSQL\DATA\comercio.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'comercio_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS02\MSSQL\DATA\comercio_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [comercio] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [comercio].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [comercio] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [comercio] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [comercio] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [comercio] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [comercio] SET ARITHABORT OFF 
GO
ALTER DATABASE [comercio] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [comercio] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [comercio] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [comercio] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [comercio] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [comercio] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [comercio] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [comercio] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [comercio] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [comercio] SET  DISABLE_BROKER 
GO
ALTER DATABASE [comercio] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [comercio] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [comercio] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [comercio] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [comercio] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [comercio] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [comercio] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [comercio] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [comercio] SET  MULTI_USER 
GO
ALTER DATABASE [comercio] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [comercio] SET DB_CHAINING OFF 
GO
ALTER DATABASE [comercio] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [comercio] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [comercio] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [comercio] SET QUERY_STORE = OFF
GO
USE [comercio]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 11/12/2021 17:24:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Carro]    Script Date: 11/12/2021 17:24:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Carro](
	[CarroId] [int] IDENTITY(1,1) NOT NULL,
	[UsuarioId] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Carro_productos]    Script Date: 11/12/2021 17:24:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Carro_productos](
	[Id_Carro] [int] NOT NULL,
	[Id_Producto] [int] NOT NULL,
	[Cantidad] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categorias]    Script Date: 11/12/2021 17:24:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categorias](
	[CatId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Compras]    Script Date: 11/12/2021 17:24:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Compras](
	[CompraId] [int] IDENTITY(1,1) NOT NULL,
	[idUsuario] [int] NOT NULL,
	[Total] [float] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Productos]    Script Date: 11/12/2021 17:24:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Productos](
	[ProductoId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NULL,
	[Precio] [float] NULL,
	[Cantidad] [int] NULL,
	[CatId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Productos_compra]    Script Date: 11/12/2021 17:24:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Productos_compra](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Id_compra] [int] NOT NULL,
	[Id_producto] [int] NOT NULL,
	[Cantidad_producto] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 11/12/2021 17:24:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[UsuarioId] [int] IDENTITY(1,1) NOT NULL,
	[Cuil] [bigint] NULL,
	[Nombre] [varchar](50) NULL,
	[Apellido] [varchar](50) NULL,
	[Mail] [varchar](50) NULL,
	[Password] [char](64) NULL,
	[MiCarro] [int] NULL,
	[TipoUsuario] [int] NULL,
	[Intentos] [int] NULL,
	[Bloqueado] [bit] NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Carro] ON 

INSERT [dbo].[Carro] ([CarroId], [UsuarioId]) VALUES (1, 1)
INSERT [dbo].[Carro] ([CarroId], [UsuarioId]) VALUES (2, 2)
INSERT [dbo].[Carro] ([CarroId], [UsuarioId]) VALUES (4, 4)
SET IDENTITY_INSERT [dbo].[Carro] OFF
GO
INSERT [dbo].[Carro_productos] ([Id_Carro], [Id_Producto], [Cantidad]) VALUES (4, 1, 20)
GO
SET IDENTITY_INSERT [dbo].[Categorias] ON 

INSERT [dbo].[Categorias] ([CatId], [Nombre]) VALUES (1, N'Alimentos')
INSERT [dbo].[Categorias] ([CatId], [Nombre]) VALUES (2, N'Bebidas')
INSERT [dbo].[Categorias] ([CatId], [Nombre]) VALUES (3, N'Congelados')
INSERT [dbo].[Categorias] ([CatId], [Nombre]) VALUES (4, N'Almacen')
INSERT [dbo].[Categorias] ([CatId], [Nombre]) VALUES (5, N'Golosinas')
SET IDENTITY_INSERT [dbo].[Categorias] OFF
GO
SET IDENTITY_INSERT [dbo].[Compras] ON 

INSERT [dbo].[Compras] ([CompraId], [idUsuario], [Total]) VALUES (1, 2, 1108.7230000000002)
SET IDENTITY_INSERT [dbo].[Compras] OFF
GO
SET IDENTITY_INSERT [dbo].[Productos] ON 

INSERT [dbo].[Productos] ([ProductoId], [Nombre], [Precio], [Cantidad], [CatId]) VALUES (1, N'Aceite de Girasol', 277.85, 19, 1)
INSERT [dbo].[Productos] ([ProductoId], [Nombre], [Precio], [Cantidad], [CatId]) VALUES (2, N'Arroz Molinos Ala Gran Selecto Largo Fino 1 Kg', 89.5, 29, 1)
INSERT [dbo].[Productos] ([ProductoId], [Nombre], [Precio], [Cantidad], [CatId]) VALUES (3, N'Sidra Saenz Briones 1888 750 Ml.', 449, 49, 2)
INSERT [dbo].[Productos] ([ProductoId], [Nombre], [Precio], [Cantidad], [CatId]) VALUES (4, N'Coca-Cola 1.5L', 174.45, 50, 2)
INSERT [dbo].[Productos] ([ProductoId], [Nombre], [Precio], [Cantidad], [CatId]) VALUES (5, N'Medallón de Carne Express Paty 4 Un.', 185.85, 50, 3)
INSERT [dbo].[Productos] ([ProductoId], [Nombre], [Precio], [Cantidad], [CatId]) VALUES (6, N'Papas Npisettes Dia 500gr', 199, 50, 3)
INSERT [dbo].[Productos] ([ProductoId], [Nombre], [Precio], [Cantidad], [CatId]) VALUES (7, N'Mantecol 111Gr.', 99.95, 199, 5)
SET IDENTITY_INSERT [dbo].[Productos] OFF
GO
SET IDENTITY_INSERT [dbo].[Productos_compra] ON 

INSERT [dbo].[Productos_compra] ([Id], [Id_compra], [Id_producto], [Cantidad_producto]) VALUES (1, 1, 1, 1)
INSERT [dbo].[Productos_compra] ([Id], [Id_compra], [Id_producto], [Cantidad_producto]) VALUES (2, 1, 2, 1)
INSERT [dbo].[Productos_compra] ([Id], [Id_compra], [Id_producto], [Cantidad_producto]) VALUES (3, 1, 3, 1)
INSERT [dbo].[Productos_compra] ([Id], [Id_compra], [Id_producto], [Cantidad_producto]) VALUES (4, 1, 7, 1)
SET IDENTITY_INSERT [dbo].[Productos_compra] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuarios] ON 

INSERT [dbo].[Usuarios] ([UsuarioId], [Cuil], [Nombre], [Apellido], [Mail], [Password], [MiCarro], [TipoUsuario], [Intentos], [Bloqueado]) VALUES (1, 20322428082, N'Sebastian', N'Lopez Sosa', N'sebastiang.lopez@davinci.edu.ar', N'db0284fc80b4881fa545953441a21958470c49d73508714a84a259cb62a5dbb5', 1, 1, 0, 0)
INSERT [dbo].[Usuarios] ([UsuarioId], [Cuil], [Nombre], [Apellido], [Mail], [Password], [MiCarro], [TipoUsuario], [Intentos], [Bloqueado]) VALUES (2, 27342166291, N'Clara', N'Sosa', N'cla@sosa.com', N'db0284fc80b4881fa545953441a21958470c49d73508714a84a259cb62a5dbb5', 2, 2, 0, 0)
INSERT [dbo].[Usuarios] ([UsuarioId], [Cuil], [Nombre], [Apellido], [Mail], [Password], [MiCarro], [TipoUsuario], [Intentos], [Bloqueado]) VALUES (4, 20456543122, N'Horacio', N'Calleja', N'horaciocalleja@gmail.com', N'41a20cef9347cf9246491ee8092dbc388692bc3ada20cfa64fe1447a77bca0b5', 4, 2, 0, 0)
SET IDENTITY_INSERT [dbo].[Usuarios] OFF
GO
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_Intentos]  DEFAULT ((0)) FOR [Intentos]
GO
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_Bloqueado]  DEFAULT ((0)) FOR [Bloqueado]
GO
USE [master]
GO
ALTER DATABASE [comercio] SET  READ_WRITE 
GO
