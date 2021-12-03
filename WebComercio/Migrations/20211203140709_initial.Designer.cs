﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebComercio.Data;

namespace WebComercio.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20211203140709_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebComercio.Carro", b =>
                {
                    b.Property<int>("CarroId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("CarroId");

                    b.HasIndex("UsuarioId")
                        .IsUnique();

                    b.ToTable("carro");
                });

            modelBuilder.Entity("WebComercio.Carro_productos", b =>
                {
                    b.Property<int>("Id_Producto")
                        .HasColumnType("int");

                    b.Property<int>("Id_Carro")
                        .HasColumnType("int");

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("Carro_productos_Id")
                        .HasColumnType("int");

                    b.HasKey("Id_Producto", "Id_Carro");

                    b.HasIndex("Id_Carro");

                    b.ToTable("Carro_productos");
                });

            modelBuilder.Entity("WebComercio.Categoria", b =>
                {
                    b.Property<int>("CatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CatId");

                    b.ToTable("categorias");
                });

            modelBuilder.Entity("WebComercio.Compra", b =>
                {
                    b.Property<int>("CompraId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.Property<int>("idUsuario")
                        .HasColumnType("int");

                    b.HasKey("CompraId");

                    b.HasIndex("idUsuario");

                    b.ToTable("compras");
                });

            modelBuilder.Entity("WebComercio.Producto", b =>
                {
                    b.Property<int>("ProductoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("CatId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Precio")
                        .HasColumnType("float");

                    b.HasKey("ProductoId");

                    b.HasIndex("CatId");

                    b.ToTable("productos");
                });

            modelBuilder.Entity("WebComercio.Productos_compra", b =>
                {
                    b.Property<int>("Id_producto")
                        .HasColumnType("int");

                    b.Property<int>("Id_compra")
                        .HasColumnType("int");

                    b.Property<int>("Cantidad_producto")
                        .HasColumnType("int");

                    b.HasKey("Id_producto", "Id_compra");

                    b.HasIndex("Id_compra");

                    b.ToTable("productos_compra");
                });

            modelBuilder.Entity("WebComercio.Usuario", b =>
                {
                    b.Property<int>("UsuarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Apellido")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Cuil")
                        .HasColumnType("int");

                    b.Property<string>("Mail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MiCarro")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TipoUsuario")
                        .HasColumnType("int");

                    b.HasKey("UsuarioId");

                    b.ToTable("usuarios");
                });

            modelBuilder.Entity("WebComercio.Carro", b =>
                {
                    b.HasOne("WebComercio.Usuario", "Usuario")
                        .WithOne("Carro")
                        .HasForeignKey("WebComercio.Carro", "UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("WebComercio.Carro_productos", b =>
                {
                    b.HasOne("WebComercio.Carro", "Carro")
                        .WithMany("Carro_productos")
                        .HasForeignKey("Id_Carro")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebComercio.Producto", "Producto")
                        .WithMany("Carro_productos")
                        .HasForeignKey("Id_Producto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Carro");

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("WebComercio.Compra", b =>
                {
                    b.HasOne("WebComercio.Usuario", "Usuario")
                        .WithMany("Compra")
                        .HasForeignKey("idUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("WebComercio.Producto", b =>
                {
                    b.HasOne("WebComercio.Categoria", "Cat")
                        .WithMany("Productos")
                        .HasForeignKey("CatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cat");
                });

            modelBuilder.Entity("WebComercio.Productos_compra", b =>
                {
                    b.HasOne("WebComercio.Compra", "Compra")
                        .WithMany("Productos_compra")
                        .HasForeignKey("Id_compra")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebComercio.Producto", "Producto")
                        .WithMany("Productos_compras")
                        .HasForeignKey("Id_producto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Compra");

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("WebComercio.Carro", b =>
                {
                    b.Navigation("Carro_productos");
                });

            modelBuilder.Entity("WebComercio.Categoria", b =>
                {
                    b.Navigation("Productos");
                });

            modelBuilder.Entity("WebComercio.Compra", b =>
                {
                    b.Navigation("Productos_compra");
                });

            modelBuilder.Entity("WebComercio.Producto", b =>
                {
                    b.Navigation("Carro_productos");

                    b.Navigation("Productos_compras");
                });

            modelBuilder.Entity("WebComercio.Usuario", b =>
                {
                    b.Navigation("Carro");

                    b.Navigation("Compra");
                });
#pragma warning restore 612, 618
        }
    }
}
