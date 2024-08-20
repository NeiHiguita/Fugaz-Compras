using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fugaz_retro.Migrations
{
   // / <inheritdoc />
    public partial class Fugaz : Migration
    {
      //  / <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categoria_Insumo",
                columns: table => new
                {
                    id_categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_categoria = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    estado_categoria = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__CD54BC5A65CCC751", x => x.id_categoria);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                    idusuario = table.Column<int>(type: "int", nullable: false),
                    direccion = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    telefono = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    ciudad = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true)
                 },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.id_cliente);
                    table.ForeignKey(
                        name: "FK_Cliente_Usuario",
                        column: x => x.idusuario,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Restrict); 
                });
            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    id_permiso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_permiso = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    estado_permiso = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Permisos__228F224F4D60E8D9", x => x.id_permiso);
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    id_producto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_producto = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    precioVenta = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Producto__FF341C0D503CEFA9", x => x.id_producto);
                });

            migrationBuilder.CreateTable(
                name: "Proveedor",
                columns: table => new
                {
                    id_proveedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipo_proveedor = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
					empresa = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
					rut = table.Column<int>(type: "int", nullable: true),
                    representante_legal = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    nombre_completo = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: true),
                    documento = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    telefono = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    direccion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Proveedo__8D3DFE284BF63AE3", x => x.id_proveedor);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_rol = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false)  
                },
                constraints: table =>
            {
                table.PrimaryKey("PK__Roles__6ABCB5E059F1FCBB", x => x.id_rol);
            });

            migrationBuilder.CreateTable(
            name: "AspNetRoleClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Insumo",
                columns: table => new
                {
                    id_insumo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_categoria = table.Column<int>(type: "int", nullable: true),
                    nombre_insumo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    unidad_medida = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: true),
                    descripcion = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    stock = table.Column<int>(type: "int", nullable: false),
                    precio_unitario = table.Column<double>(type: "float", nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Insumo__D4F202B193BC63C8", x => x.id_insumo);
                    table.ForeignKey(
                        name: "FK__Insumo__id_categ__48CFD27E",
                        column: x => x.id_categoria,
                        principalTable: "Categoria_Insumo",
                        principalColumn: "id_categoria");
                });

            migrationBuilder.CreateTable(
                name: "Perdida_Insumo",
                columns: table => new
                {
                    id_perdida_insumo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_insumo = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    fecha = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Perdida___49FE1C56F8B3D1F3", x => x.id_perdida_insumo);
                    table.ForeignKey(
                        name: "FK__Perdida_I__id_in__2A164134",
                        column: x => x.id_insumo,
                        principalTable: "Insumo",
                        principalColumn: "id_insumo");
                });

            migrationBuilder.CreateTable(
                name: "Rol_Permiso",
                columns: table => new
                {
                    id_rol_permiso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_rol = table.Column<int>(type: "int", nullable: true),
                    id_permiso = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Rol_Perm__BA448703B40BBE2A", x => x.id_rol_permiso);
                    table.ForeignKey(
                        name: "FK__Rol_Permi__id_pe__3C69FB99",
                        column: x => x.id_permiso,
                        principalTable: "Permisos",
                        principalColumn: "id_permiso");
                    table.ForeignKey(
                        name: "FK__Rol_Permi__id_ro__3B75D760",
                        column: x => x.id_rol,
                        principalTable: "Roles",
                        principalColumn: "id_rol");
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_rol = table.Column<int>(type: "int", nullable: true),
                    nombre_usuario = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    correo = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    contraseña = table.Column<int>(type: "varchar(40)", nullable: false),
					estado = table.Column<bool>(type: "bit", nullable: false),
                    date_register = table.Column<DateTime>(type: "date", nullable: false),
                    document = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuario__4E3E04AD9CC295B8", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK__Usuario__id_rol___3F466844",
                        column: x => x.id_rol,
                        principalTable: "Roles",
                        principalColumn: "id_rol");
                });

            migrationBuilder.CreateTable(
                name: "Compra",
                columns: table => new
                {
                    id_compra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: true),
                    id_proveedor = table.Column<int>(type: "int", nullable: true),
                    fecha_compra = table.Column<DateTime>(type: "date", nullable: false),
                    metodo_pago = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
					tipo_transferencia = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
					subtotal = table.Column<double>(type: "float", nullable: false),
                    iva = table.Column<double>(type: "float", nullable: false),
                    descuento = table.Column<double>(type: "float", nullable: true),
                    precio_total = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Compra__C4BAA60461F0D5E6", x => x.id_compra);
                    table.ForeignKey(
                        name: "FK__Compra__id_prove__4E88ABD4",
                        column: x => x.id_proveedor,
                        principalTable: "Proveedor",
                        principalColumn: "id_proveedor");
                    table.ForeignKey(
                        name: "FK__Compra__id_usuar__4D94879B",
                        column: x => x.id_usuario,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Detalle_Compra",
                columns: table => new
                {
                    id_detalle_compra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_compra = table.Column<int>(type: "int", nullable: true),
                    id_insumo = table.Column<int>(type: "int", nullable: true),
                    cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Detalle___BD16E279ABE746F9", x => x.id_detalle_compra);
                    table.ForeignKey(
                        name: "FK__Detalle_C__id_co__5165187F",
                        column: x => x.id_compra,
                        principalTable: "Compra",
                        principalColumn: "id_compra");
                    table.ForeignKey(
                        name: "FK__Detalle_C__id_in__52593CB8",
                        column: x => x.id_insumo,
                        principalTable: "Insumo",
                        principalColumn: "id_insumo");
                });

            migrationBuilder.CreateTable(
                name: "Pedido",
                columns: table => new
                {
                    id_pedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: true),
                    id_cliente = table.Column<int>(type: "int", nullable: true),
                    estado = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    metodo_pago = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    fecha_pedido = table.Column<DateTime>(type: "date", nullable: false),
                    fecha_entrega = table.Column<DateTime>(type: "date", nullable: false),
                    costo_pedido = table.Column<double>(type: "float", nullable: false),
                    costo_envio = table.Column<double>(type: "float", nullable: false),
                    iva = table.Column<double>(type: "float", nullable: false),
                    descuento = table.Column<double>(type: "float", nullable: false),
                    total_pedido = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pedido__6FF01489E3A55BCA", x => x.id_pedido);
                    table.ForeignKey(
                        name: "FK__Pedido__id_clien__5629CD9C",
                        column: x => x.id_cliente,
                        principalTable: "Cliente",
                        principalColumn: "id_cliente");
                    table.ForeignKey(
                        name: "FK__Pedido__id_usuar__5535A963",
                        column: x => x.id_usuario,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Detalle_Pedido",
                columns: table => new
                {
                    id_detalle_pedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_pedido = table.Column<int>(type: "int", nullable: true),
                    id_producto = table.Column<int>(type: "int", nullable: true),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precio_unitario = table.Column<double>(type: "float", nullable: false),
                    subtotal = table.Column<double>(type: "float", nullable: false)

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Detalle___C08768CF39E5F374", x => x.id_detalle_pedido);
                    table.ForeignKey(
                        name: "FK__Detalle_P__id_pe__5BE2A6F2",
                        column: x => x.id_pedido,
                        principalTable: "Pedido",
                        principalColumn: "id_pedido");
                    table.ForeignKey(
                        name: "FK__Detalle_P__id_pr__5CD6CB2B",
                        column: x => x.id_producto,
                        principalTable: "Producto",
                        principalColumn: "id_producto");
                });

            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    id_venta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: true),
                    id_pedido = table.Column<int>(type: "int", nullable: true),
                    Subtotal = table.Column<double>(type: "float", nullable: false),
                    iva = table.Column<double>(type: "float", nullable: false),
                    decuento = table.Column<double>(type: "float", nullable: true),
                    total_venta = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ventas__459533BFC72708FB", x => x.id_venta);
                    table.ForeignKey(
                        name: "FK__Ventas__id_pedid__6383C8BA",
                        column: x => x.id_pedido,
                        principalTable: "Pedido",
                        principalColumn: "id_pedido");
                    table.ForeignKey(
                        name: "FK__Ventas__id_usuar__628FA481",
                        column: x => x.id_usuario,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_id_proveedor",
                table: "Compra",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_id_usuario",
                table: "Compra",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Compra_id_compra",
                table: "Detalle_Compra",
                column: "id_compra");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Compra_id_insumo",
                table: "Detalle_Compra",
                column: "id_insumo");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Pedido_id_pedido",
                table: "Detalle_Pedido",
                column: "id_pedido");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Pedido_id_producto",
                table: "Detalle_Pedido",
                column: "id_producto");


            migrationBuilder.CreateIndex(
                name: "IX_Insumo_id_categoria",
                table: "Insumo",
                column: "id_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_id_cliente",
                table: "Pedido",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_id_usuario",
                table: "Pedido",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Perdida_Insumo_id_insumo",
                table: "Perdida_insumo",
                column: "id_insumo");

            migrationBuilder.CreateIndex(
                name: "IX_Rol_Permiso_id_permiso",
                table: "Rol_Permiso",
                column: "id_permiso");

            migrationBuilder.CreateIndex(
                name: "IX_Rol_Permiso_id_rol",
                table: "Rol_Permiso",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_id_rol",
                table: "Usuario",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_id_pedido",
                table: "Ventas",
                column: "id_pedido");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_id_usuario",
                table: "Ventas",
                column: "id_usuario");
        }

        // <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Detalle_Compra");

            migrationBuilder.DropTable(
                name: "Detalle_Pedido");

            migrationBuilder.DropTable(
                name: "Perdida_Insumo");

            migrationBuilder.DropTable(
                name: "Ventas");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Compra");

            migrationBuilder.DropTable(
                name: "Insumo");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "Pedido");

            migrationBuilder.DropTable(
                name: "Proveedor");

            migrationBuilder.DropTable(
                name: "Categoria_Insumo");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Rol_Permiso");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
