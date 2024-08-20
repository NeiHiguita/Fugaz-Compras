using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace fugaz_retro.Models;

public partial class FugazContext : IdentityDbContext
{
    public FugazContext()
    {
    }

    public FugazContext(DbContextOptions<FugazContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriaInsumo> CategoriaInsumos { get; set; }
    public virtual DbSet<Cliente> Clientes { get; set; }
    public virtual DbSet<Compra> Compras { get; set; }
    public virtual DbSet<DetalleCompra> DetalleCompras { get; set; }
    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }
    public virtual DbSet<DetalleProducto> DetalleProductos { get; set; }
    public virtual DbSet<DetalleInsumo> DetalleInsumos { get; set; }
    public virtual DbSet<Insumo> Insumos { get; set; }
    public virtual DbSet<CostoEnvio> CostoEnvios { get; set; }
    public virtual DbSet<Pedido> Pedidos { get; set; }
    public virtual DbSet<PerdidaInsumo> PerdidaInsumos { get; set; }
    public virtual DbSet<Permiso> Permisos { get; set; }
    public virtual DbSet<Producto> Productos { get; set; }
    public virtual DbSet<Proveedor> Proveedors { get; set; }
    public virtual DbSet<RolPermiso> RolPermisos { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<Venta> Ventas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriaInsumo>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__CD54BC5A65CCC751");

            entity.ToTable("Categoria_Insumo");

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.EstadoCategoria).HasColumnName("estado_categoria");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre_categoria");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Cliente");

            entity.Property(e => e.IdCliente)
                .HasColumnName("id_cliente")
                .UseIdentityColumn()
                .IsRequired();
            entity.Property(e => e.Telefono)
                .HasColumnName("telefono")
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsRequired();
            entity.Property(e => e.IdUsuario).HasColumnName("idusuario");

            entity.HasKey(e => e.IdCliente)
                .HasName("PK_Cliente");

        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra).HasName("PK__Compra__C4BAA60461F0D5E6");

            entity.ToTable("Compra");

            entity.Property(e => e.IdCompra).HasColumnName("id_compra");
            entity.Property(e => e.Descuento).HasColumnName("descuento");
            entity.Property(e => e.FechaCompra)
                .HasColumnType("date")
                .HasColumnName("fecha_compra");
            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.PrecioTotal).HasColumnName("precio_total");
            entity.Property(e => e.Subtotal).HasColumnName("subtotal");
            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK__Compra__id_prove__4E88ABD4");
        });

        modelBuilder.Entity<DetalleCompra>(entity =>
        {
            entity.HasKey(e => e.IdDetalleCompra).HasName("PK_Detalle__BD16E279ABE746F9");

            entity.ToTable("Detalle_Compra");

            entity.Property(e => e.IdDetalleCompra).HasColumnName("id_detalle_compra");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.UnidadMedida)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("unidad_medida");
            entity.Property(e => e.IdCompra).HasColumnName("id_compra");
            entity.Property(e => e.IdInsumo).HasColumnName("id_insumo");
            entity.Property(e => e.PrecioUnitario).HasColumnName("precio_unitario");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("FK_Detalle_Cid_co_5165187F");

            entity.HasOne(d => d.IdInsumoNavigation).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.IdInsumo)
                .HasConstraintName("FK_Detalle_Cid_in_52593CB8");
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.IdDetallePedido).HasName("PK__Detalle___C08768CF39E5F374");

            entity.ToTable("Detalle_Pedido");
            entity.Property(e => e.talla)
               .HasMaxLength(250)
               .IsUnicode(false)
               .HasColumnName("talla");
            entity.Property(e => e.color)
               .HasMaxLength(250)
               .IsUnicode(false)
               .HasColumnName("color");
            entity.Property(e => e.IdDetallePedido).HasColumnName("id_detalle_pedido");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdPedido).HasColumnName("id_pedido");
            entity.Property(e => e.IdDetalleProducto).HasColumnName("id_detalle_producto");
            entity.Property(e => e.Subtotal).HasColumnName("subtotal");

            entity.HasOne(d => d.IdPedidoNavigation)
                       .WithMany(p => p.DetallePedidos)
                       .HasForeignKey(d => d.IdPedido)
                       .HasConstraintName("FK_Detalle_Pedido_Pedido");

            entity.HasOne(d => d.IdDetalleProductoNavigation)
                .WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdDetalleProducto)
                .HasConstraintName("FK_Detalle_Pedido_Detalle_Producto");
        });


        modelBuilder.Entity<Insumo>(entity =>
        {
            entity.HasKey(e => e.IdInsumo).HasName("PK_Insumo_D4F202B193BC63C8");

            entity.ToTable("Insumo");

            entity.Property(e => e.IdInsumo).HasColumnName("id_insumo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.CategoriaInsumo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("categoria_insumo");
            entity.Property(e => e.NombreInsumo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_insumo");
            modelBuilder.Entity<Insumo>()
        .Property(i => i.Stock)
        .HasColumnType("decimal(3,1)")
        .HasColumnName("stock");
            entity.Property(e => e.UnidadMedida)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("unidad_medida");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.PrecioUnitario).HasColumnName("precio_unitario");
            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Insumos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK_Insumoid_categ_48CFD27E");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__Pedido__6FF01489E3A55BCA");

            entity.ToTable("Pedido");

            entity.Property(e => e.IdPedido).HasColumnName("id_pedido");
            entity.Property(e => e.CostoEnvio).HasColumnName("costo_envio");
            entity.Property(e => e.CostoPedido).HasColumnName("costo_pedido");
            entity.Property(e => e.Descuento).HasColumnName("descuento");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechaEntrega)
                .HasColumnType("date")
                .HasColumnName("fecha_entrega");
            entity.Property(e => e.FechaPedido)
                .HasColumnType("date")
                .HasColumnName("fecha_pedido");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("metodo_pago");
            entity.Property(e => e.TipoTransferencia)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo_transferencia");
            entity.Property(e => e.TotalPedido).HasColumnName("total_pedido");
            entity.Property(e => e.ComprobantePago)
                .HasColumnName("comprobante_pago")
                .HasColumnType("varbinary(max)");
            entity.Property(e => e.Direccion)
                .HasColumnName("direccion")
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Ciudad)
                .HasColumnName("ciudad")
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.IdClienteNavigation)
                .WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK_Pedido_Cliente");
        });

        modelBuilder.Entity<PerdidaInsumo>(entity =>
        {
            entity.HasKey(e => e.IdPerdidaInsumo).HasName("PK__Perdida___49FE1C56F8B3D1F3");

            entity.ToTable("Perdida_Insumo");

            entity.Property(e => e.IdPerdidaInsumo).HasColumnName("id_perdida_insumo");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Fecha)
                .HasColumnType("date")
                .HasColumnName("fecha");
            entity.Property(e => e.IdInsumo).HasColumnName("id_insumo"); 
            entity.Property(e => e.TipoInsumo).HasColumnName("tipo_insumo");
            entity.Property(e => e.UnidadMedida).HasColumnName("unidad_medida");

            entity.HasOne(d => d.IdInsumoNavigation).WithMany(p => p.PerdidaInsumos)
                .HasForeignKey(d => d.IdInsumo)
                .HasConstraintName("FK__Perdida_I__id_pr__5FB337D6");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("PK__Permiso__5239C83E7AF3E9B1");

            entity.ToTable("Permisos");

            entity.Property(e => e.IdPermiso).HasColumnName("id_permiso");
            entity.Property(e => e.NombrePermiso)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_permiso");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__FF341C0D503CEFA9");

            entity.ToTable("Producto");

            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_producto");
            entity.Property(e => e.PrecioVenta).HasColumnName("precio_venta");
            entity.Property(e => e.CategoriaProducto)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("categoria_producto");
            entity.Property(e => e.CategoriaProducto).HasColumnName("categoria_producto");
            entity.Property(e => e.Foto)
                .HasColumnName("foto")
                .HasColumnType("varbinary(max)");
        });


        modelBuilder.Entity<DetalleProducto>(entity =>
        {
            entity.HasKey(e => e.IdDetalleProducto).HasName("PK__DetalleProducto__");

            entity.ToTable("Detalle_Producto");

            entity.Property(e => e.IdDetalleProducto).HasColumnName("id_detalle_producto");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Talla).HasColumnName("talla");
            entity.Property(e => e.Color).HasColumnName("color");

            entity.HasOne(d => d.Producto)
                  .WithMany(p => p.DetalleProductos)
                  .HasForeignKey(d => d.IdProducto)
                  .HasConstraintName("FK_DetalleProducto_Producto");
        });

        modelBuilder.Entity<DetalleInsumo>(entity =>
        {
            entity.HasKey(e => e.IdDetalleInsumo).HasName("PK_DetalleInsumo");

            entity.ToTable("Detalle_Insumo");

            entity.Property(e => e.IdDetalleInsumo).HasColumnName("Id_Detalle_Insumo");
            entity.Property(e => e.IdDetalleProducto).HasColumnName("Id_Detalle_Producto");
            entity.Property(e => e.IdInsumo).HasColumnName("Id_Insumo");
            entity.Property(e => e.Cantidad).HasColumnName("Cantidad");

            entity.HasOne(d => d.DetalleProducto)
                  .WithMany(p => p.DetalleInsumos)
                  .HasForeignKey(d => d.IdDetalleProducto)
                  .HasConstraintName("FK_DetalleInsumo_DetalleProducto");

            entity.HasOne(d => d.Insumo)
                  .WithMany()
                  .HasForeignKey(d => d.IdInsumo)
                  .HasConstraintName("FK_DetalleInsumo_Insumo");
        });



        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PK__Proveedo__8D3DFE284BF63AE3");

            entity.ToTable("Proveedor");

            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");

            // Campo para dirección alternativa
            entity.Property(e => e.DireccionAlternativa)
                .HasMaxLength(100) // Ajusta el tamaño si necesitas permitir direcciones más largas
                .IsUnicode(false)
                .HasColumnName("direccion_alternativa");

            entity.Property(e => e.Documento)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("documento");

            entity.Property(e => e.Estado)
                .HasColumnName("estado");

            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("nombre_completo");

            entity.Property(e => e.Rut)
                .HasColumnName("rut");

            entity.Property(e => e.RepresentanteLegal)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("representante_legal");

            entity.Property(e => e.Telefono)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("telefono");

            entity.Property(e => e.TipoProveedor)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tipo_proveedor");
        });

        modelBuilder.Entity<RolPermiso>(entity =>
        {
            entity.HasKey(e => e.IdRolPermiso).HasName("PK__Rol_Perm__BA448703B40BBE2A");

            entity.ToTable("Rol_Permiso");

            entity.Property(e => e.IdRolPermiso).HasColumnName("id_rol_permiso");
            entity.Property(e => e.IdPermiso).HasColumnName("id_permiso");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");

            entity.HasOne(d => d.IdPermisoNavigation).WithMany(p => p.RolPermisos)
                .HasForeignKey(d => d.IdPermiso)
                .HasConstraintName("FK__Rol_Permi__id_pe__3C69FB99");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolPermisos)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Rol_Permi__id_ro__3B75D760");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__6ABCB5E059F1FCBB");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre_rol");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__4E3E04AD9CC295B8");

            entity.ToTable("Usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Contraseña).HasColumnName("contraseña");
            entity.Property(e => e.Correo)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.DateRegister)
                .HasColumnType("date")
                .HasColumnName("date_register");
            entity.Property(e => e.Document)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("document");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre_usuario");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Usuario__id_rol___3F466844");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("PK__Ventas__459533BFC72708FB");

            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.IdPedido).HasColumnName("id_pedido");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("FK__Ventas__id_pedid__6383C8BA");

            base.OnModelCreating(modelBuilder);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
