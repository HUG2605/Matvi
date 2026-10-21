
--	USE MASTER
--	DROP DATABASE MatViDB

USE master;
GO

-- Limpiar objetos huérfanos que hayan quedado por error en master
IF OBJECT_ID('master.dbo.BtCambio', 'U') IS NOT NULL DROP TABLE master.dbo.BtCambio;
GO

IF OBJECT_ID('tempdb..##MatviTargetDB') IS NOT NULL DROP TABLE ##MatviTargetDB;
CREATE TABLE ##MatviTargetDB (IsProd BIT NOT NULL);

-- Detectar y recrear la base de datos objetivo:
-- Producción (VPS Docker Linux / matvi-sqlserver) -> MatviDB_PROD
-- Local (Windows / SQLEXPRESS) -> MatViDB
DECLARE @DetectarProd BIT = 0;
IF SERVERPROPERTY('IsLinux') = 1 
   OR SERVERPROPERTY('MachineName') = 'matvi-sqlserver'
   OR @@SERVERNAME LIKE '%matvi-sqlserver%'
BEGIN
	SET @DetectarProd = 1;
END

IF @DetectarProd = 1
BEGIN
	INSERT INTO ##MatviTargetDB VALUES (1);
	IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'MatviDB_PROD')
	BEGIN
		ALTER DATABASE [MatviDB_PROD] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
		DROP DATABASE [MatviDB_PROD];
	END
	CREATE DATABASE [MatviDB_PROD] COLLATE Modern_Spanish_CI_AI;

	-- Asegurar existencia de MatViDB para evitar error de compilación 911 en SSMS
	IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'MatViDB')
		CREATE DATABASE [MatViDB] COLLATE Modern_Spanish_CI_AI;
END
ELSE
BEGIN
	INSERT INTO ##MatviTargetDB VALUES (0);
	IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'MatViDB')
	BEGIN
		ALTER DATABASE [MatViDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
		DROP DATABASE [MatViDB];
	END
	CREATE DATABASE [MatViDB] COLLATE Modern_Spanish_CI_AI;

	-- Asegurar existencia de MatviDB_PROD para evitar error de compilación 911 en SSMS
	IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'MatviDB_PROD')
		CREATE DATABASE [MatviDB_PROD] COLLATE Modern_Spanish_CI_AI;
END
GO

DECLARE @EsProd BIT = 0;
IF OBJECT_ID('tempdb..##MatviTargetDB') IS NOT NULL
	SELECT TOP 1 @EsProd = IsProd FROM ##MatviTargetDB;

IF @EsProd = 1
	USE [MatviDB_PROD];
ELSE
	USE [MatViDB];
GO

IF OBJECT_ID('tempdb..##MatviTargetDB') IS NOT NULL DROP TABLE ##MatviTargetDB;
GO







-- #################################################### --
-- MÓDULO: ESCENCIALES
-- #################################################### --
BEGIN 
	PRINT '===================================================='
	PRINT 'Inicia: MÓDULO: ESCENCIALES'

	-- ====================================================
	-- BtCambio										
	-- ====================================================
	BEGIN
		-- DROP TABLE BtCambio
		CREATE TABLE BtCambio
		(
			Id				INT				NOT NULL IDENTITY,
			Tabla			NVARCHAR(100)	NOT NULL,
			LlavePrimaria	NVARCHAR(100)	NOT NULL,
			Campo			NVARCHAR(100)	NOT NULL,
			ValorAnterior	NVARCHAR(MAX)	NULL,
			ValorNuevo		NVARCHAR(MAX)	NULL,
			TipoOperacion	TINYINT			NOT NULL, -- 1 = INSERT, 2 = UPDATE, 3 = DELETE
			IdCtUsuario		SMALLINT		NOT NULL,
			Fecha			DATETIME		NOT NULL DEFAULT GETDATE(),
			CONSTRAINT PK_BtCambio PRIMARY KEY (Id),
		);
	END

	-- ============================================
	-- CtTipoConsecutivo
	-- ============================================
	BEGIN
		-- DROP TABLE CtTipoConsecutivo
		CREATE TABLE CtTipoConsecutivo
		(
			Id			TINYINT			NOT NULL,
			Nombre		VARCHAR(25)		NOT NULL,
			CONSTRAINT PK_CtTipoConsecutivo PRIMARY KEY (Id),
			CONSTRAINT UK_CtTipoConsecutivo UNIQUE (Nombre)
		);

	END

	BEGIN
		INSERT INTO CtTipoConsecutivo (Id, Nombre) 
		VALUES
			(1, 'COMPRA'),
			(2, 'LOTE'),
			(3, 'MOVIMIENTO DE INVENTARIO'),
			(4, 'PEDIDO'),
			(5, 'PAGO'),
			(6, 'COBRO'),
			(7, 'GASTO'),
			(8, 'CORTE');
	END	

	-- ====================================================
	-- BtConsecutivo										
	-- ====================================================
	BEGIN
		-- DROP TABLE BtConsecutivo
		CREATE TABLE BtConsecutivo
		(
			IdCtSucursal			TINYINT		NOT NULL,
			IdCtTipoConsecutivo		TINYINT		NOT NULL,
			UltimoNumero			INT			NOT NULL,
			CONSTRAINT PK_BtConsecutivo PRIMARY KEY (IdCtSucursal, IdCtTipoConsecutivo),
			CONSTRAINT FK_BtConsecutivo_CtTipoConsecutivo 
			FOREIGN KEY(IdCtTipoConsecutivo) REFERENCES CtTipoConsecutivo(Id)
		);
	END

	-- ====================================================
	-- BtConsecutivo										
	-- ====================================================
	BEGIN
		-- DROP TABLE CtUsuarioWebPushSuscripcion
		CREATE TABLE CtUsuarioWebPushSuscripcion 
		(
			Id              INT             NOT NULL IDENTITY(1,1),
			IdCtUsuario     SMALLINT        NOT NULL,
			Endpoint        VARCHAR(500)    NOT NULL,
			P256dh          VARCHAR(500)    NOT NULL,
			Auth            VARCHAR(100)    NOT NULL,
			FechaRegistro   DATETIME        NOT NULL    DEFAULT GETDATE(),
			Activa          BIT             NOT NULL    DEFAULT 1,
			CONSTRAINT PK_CtUsuarioWebPushSuscripcion PRIMARY KEY (Id),
			--CONSTRAINT FK_CtUsuarioWebPushSuscripcion_CtUsuario 
			--FOREIGN KEY (IdCtUsuario) REFERENCES CtUsuario(Id)
		);

	END

	PRINT 'Termina: MÓDULO: ESCENCIALES'
	PRINT '===================================================='
END


-- #################################################### --
-- MÓDULO: CONFIGURACIÓN
-- #################################################### --
BEGIN
	PRINT '===================================================='
	PRINT 'Inicia: MÓDULO: CONFIGURACIÓN'

	-- ====================================================
	-- CtEmpresa										
	-- ====================================================
	BEGIN
		-- DROP TABLE CtEmpresa
		CREATE TABLE CtEmpresa
		(
			Id			TINYINT			NOT NULL,
			Nombre		VARCHAR(50)		NOT NULL,
			Logotipo	VARCHAR(MAX)	NULL,
			Telefono	VARCHAR(10)		NULL,
			CONSTRAINT PK_CtEmpresa PRIMARY KEY (Id),
			CONSTRAINT UK_CtEmpresa UNIQUE (Nombre)
		);
	END

	BEGIN
		INSERT INTO CtEmpresa (Id, Nombre, Logotipo, Telefono) 
		VALUES 
			(1, 'MATERIALES VILLANUEVA', NULL, NULL)
	END


	-- ====================================================
	-- CtSucursal										
	-- ====================================================
	BEGIN
		-- DROP TABLE CtSucursal
		CREATE TABLE CtSucursal
		(
			Id				TINYINT			NOT NULL IDENTITY,
			IdCtEmpresa		TINYINT			NOT NULL,
			Nombre			VARCHAR(20)		NOT NULL,
			Clave			CHAR(4)			NOT NULL,
			Direccion		VARCHAR(250)	NOT NULL,
			Telefono		VARCHAR(10)		NOT NULL,
			CONSTRAINT PK_CtSucursal PRIMARY KEY (Id),
			CONSTRAINT UK_CtSucursal UNIQUE (Nombre),
			CONSTRAINT FK_CtSucursal_CtEmpresa 
			FOREIGN KEY(IdCtEmpresa) REFERENCES CtEmpresa(Id)
		);

		ALTER TABLE BtConsecutivo
		ADD CONSTRAINT FK_BtConsecutivo_CtSucursal
		FOREIGN KEY (IdCtSucursal) REFERENCES CtSucursal(Id) 

	END

	BEGIN
		INSERT INTO CtSucursal (IdCtEmpresa, Nombre, Clave, Direccion, Telefono) 
		VALUES 
			(1, 'VILLANUEVA', 'SSEB', N'AV. RODOLFO SÁNCHEZ TABOADA S/N, SAN SEBASTIÁN VILLANUEVA, PUEBLA', ''),
			(1, 'SAN MATEO',  'SMAT', N'AV. EMILIANO ZAPATA 543, COL. CENTRO, SAN MATEO SOLTEPEC PUEBLA', N'2452545555')

		-- Inicializar consecutivo en 0 para todas las sucursales estructurales y todos los tipos
		INSERT INTO BtConsecutivo (IdCtSucursal, IdCtTipoConsecutivo, UltimoNumero)
		SELECT s.Id, t.Id, 0
		FROM CtSucursal s
		CROSS JOIN CtTipoConsecutivo t;
	END

	PRINT 'Termina: MÓDULO: CONFIGURACIÓN'
	PRINT '===================================================='
END


-- #################################################### --
-- MÓDULO: SEGURIDAD
-- #################################################### --
BEGIN 
	PRINT '===================================================='
	PRINT 'Inicia: MÓDULO: SEGURIDAD'

	-- ====================================================
	-- CtRol										
	-- ====================================================
	BEGIN
		-- DROP TABLE CtRol
		CREATE TABLE CtRol
		(
			Id			TINYINT		NOT NULL,
			Nombre		VARCHAR(50)	NOT NULL,
			Acronimo	CHAR(3)		NOT NULL,
			CONSTRAINT PK_CtRol PRIMARY KEY (Id),
			CONSTRAINT UK_CtRol UNIQUE (Nombre)
		);
	END

	BEGIN
		INSERT INTO CtRol VALUES 
			(0, 'SYSTEM', 'STM'),
			(1, 'ADMINISTRADOR', 'ADM'),
			(2, 'VENDEDOR', 'VND'),
			(3, 'ALMACENISTA', 'ALM'),
			(4, 'REPARTIDOR', 'RPT');
	END

	-- ====================================================
	-- CtUsuario								
	-- ====================================================
	BEGIN
		-- DROP TABLE CtUsuario
		CREATE TABLE CtUsuario
		(
			Id					SMALLINT		NOT NULL IDENTITY,
			IdCtRol				TINYINT			NOT NULL,
			Nombre				VARCHAR(25)		NOT NULL,
			PrimerApellido		VARCHAR(25)		NOT NULL,
			SegundoApellido		VARCHAR(25)		NOT NULL,
			Direccion			VARCHAR(250)	NOT NULL,
			Telefono			VARCHAR(10)		NOT NULL,
			Usuario				VARCHAR(8)		NOT NULL,
			Contrasenia			VARCHAR(100)	NULL,
			Activo				BIT				NOT NULL,
			CONSTRAINT PK_CtUsuario PRIMARY KEY (Id),
			CONSTRAINT UK_CtUsuario UNIQUE (Usuario),
			CONSTRAINT FK_CtUsuario_CtRol 
			FOREIGN KEY(IdCtRol) REFERENCES CtRol(Id)
		);

		ALTER TABLE CtUsuarioWebPushSuscripcion
		ADD CONSTRAINT FK_CtUsuarioWebPushSuscripcion_CtUsuario 
		FOREIGN KEY (IdCtUsuario) REFERENCES CtUsuario(Id)

	END

	BEGIN

		SET IDENTITY_INSERT CtUsuario ON;

		INSERT INTO CtUsuario (
			Id,				IdCtRol,
			Nombre,			PrimerApellido,		SegundoApellido,
			Direccion,		Telefono,			Usuario,	
			Contrasenia,	Activo)
		VALUES 
		(
			0,			0, 
			'GABRIEL'	,'JUÁREZ'	,'SANDOVAL',	
			'AV. RODOLFO SÁNCHEZ TABOADA 11, SAN SEBASTIÁN VILLANUEVA PUEBLA',	'5522496817', 'STM000',	
			'MTdAZG0xbg==', 1
		);

		--	Usuario base del sistema (Contraseña: 17@dm1n -> Base64 MTdAZG0xbg==):
		--	0: STM000 (SYSTEM)
		--	El resto de usuarios de demo se encuentran en CRG_Matvi.sql

		SET IDENTITY_INSERT CtUsuario OFF;

	END

	-- ====================================================
	-- RlUsuarioSucursal								
	-- ====================================================
	BEGIN
		-- DROP TABLE RlUsuarioSucursal
		CREATE TABLE RlUsuarioSucursal
		(
			IdCtUsuario		SMALLINT	NOT NULL,
			IdCtSucursal	TINYINT		NOT NULL,
			CONSTRAINT PK_RlUsuarioSucursal PRIMARY KEY (IdCtUsuario, IdCtSucursal),
			CONSTRAINT FK_RlUsuarioSucursal_CtUsuario 
			FOREIGN KEY(IdCtUsuario) REFERENCES CtUsuario(Id),
			CONSTRAINT FK_RlUsuarioSucursal_CtSucursal 
			FOREIGN KEY(IdCtSucursal) REFERENCES CtSucursal(Id),
		);

	END

	BEGIN
		-- Solo usuario SYSTEM (Id=0) — los demás usuarios están en CRG_Matvi.sql
		INSERT INTO RlUsuarioSucursal (IdCtUsuario,IdCtSucursal) 
		VALUES 
			(0, 1), (0, 2);
	END

	-- ====================================================
	-- CtModulo									
	-- ====================================================
	BEGIN
		-- DROP TABLE CtModulo
		CREATE TABLE CtModulo
		(
			Id			TINYINT		NOT NULL,
			Nombre		VARCHAR(25)	NOT NULL,
			IconName	VARCHAR(50) NOT NULL, 
			Area		VARCHAR(25)	NOT NULL, 
			CONSTRAINT PK_CtModulo PRIMARY KEY (Id),
			CONSTRAINT UK_CtModulo UNIQUE (Nombre)
		);
	END

	BEGIN
		INSERT INTO CtModulo VALUES
			(1, 'SEGURIDAD', '', 'Seguridad'),
			(2, 'CONFIGURACIÓN', '', 'Configuracion'),
			(3, 'PRODUCTOS', '', 'Productos'),
			(4, 'COMPRAS', '', 'Compras'),
			(5, 'INVENTARIOS', '', 'Inventarios'),
			(6, 'VENTAS', '', 'Ventas'),
			(7, 'FINANZAS', '', 'Finanzas');

		--UPDATE CtModulo SET IconName = 'bx bx-lock' WHERE Id = 1
		--UPDATE CtModulo SET IconName = 'bx bx-cog' WHERE Id = 2
		--UPDATE CtModulo SET IconName = 'bx bx-wrench' WHERE Id = 3
		--UPDATE CtModulo SET IconName = 'bx bx-receipt' WHERE Id = 4
		--UPDATE CtModulo SET IconName = 'bx bx-package' WHERE Id = 5
		--UPDATE CtModulo SET IconName = 'bx bx-cart' WHERE Id = 6
		--UPDATE CtModulo SET IconName = 'bx bx-wallet' WHERE Id = 7

		UPDATE CtModulo SET IconName = 'mdi mdi-lock' WHERE Id = 1;
		UPDATE CtModulo SET IconName = 'mdi mdi-cog' WHERE Id = 2;
		UPDATE CtModulo SET IconName = 'mdi mdi-wrench' WHERE Id = 3;
		UPDATE CtModulo SET IconName = 'mdi mdi-receipt' WHERE Id = 4;
		UPDATE CtModulo SET IconName = 'mdi mdi-package-variant' WHERE Id = 5;
		UPDATE CtModulo SET IconName = 'mdi mdi-cart' WHERE Id = 6;
		UPDATE CtModulo SET IconName = 'mdi mdi-wallet' WHERE Id = 7;

		--SELECT * FROM CatModulo
		--DELETE FROM CatModulo
	END

	-- ====================================================
	-- CtFuncion										
	-- ====================================================
	BEGIN
		-- DROP TABLE CtFuncion
		CREATE TABLE CtFuncion
		(
			Id			TINYINT			NOT NULL, 
			IdCtModulo	TINYINT			NOT NULL, 
			Nombre		VARCHAR(50)		NOT NULL, 
			Ruta		VARCHAR(100)	NOT NULL, 
			CONSTRAINT PK_CtFuncion PRIMARY KEY (Id), 
			CONSTRAINT UK_CtFuncion UNIQUE (IdCtModulo, Nombre), 
			CONSTRAINT FK_CtFuncion_CtModulo 
			FOREIGN KEY (IdCtModulo) REFERENCES CtModulo(Id)
		);
	END

	BEGIN
		INSERT INTO CtFuncion VALUES 
		--	1	SEGURIDAD
		(01,	1,  'Usuarios',					'/Usuario'	 ),
		(02,	1,  'Roles',					'/Rol'		 ),
		--	2	CONFIGURACIÓN
		(03,	2,  'Empresa',					'/Empresa'	 ),
		(04,	2,  'Sucursales',				'/Sucursal'	 ),
		--	3	PRODUCTOS
		(05,	3,  'Secciones',				'/Seccion'	 ),
		(06,	3,  'Grupos',					'/Grupo'	 ),
		(07,	3,  'Productos',				'/Producto'	 ),
		--	4	COMPRAS
		(08,	4,  'Proveedores',				'/Proveedor' ),
		(09,	4,  'Compras',					'/Compra'	 ),
		--	5	INVENTARIOS
		(10,	5,  'Lotes',					'/Lote'		 ),
		(11,	5,  'Movimientos',				'/Movimiento'),
		(12,	5,  'Inventario',				'/Inventario'),
		--	6	VENTAS
		(13,	6,  'Clientes',					'/Cliente'	 ),
		(14,	6,  'Pedidos',					'/Pedido'	 ),
		--	7	FINANZAS
		(15,	7,  'Tipos de Gastos',			'/TipoGasto' ),
		(16,	7,  'Gastos',					'/Gasto'	 ),
		(17,	7,  'Pagos',					'/Pago'		 ),
		(18,	7,  'Cobros',					'/Cobro'	 ),
		(19,	7,  'Cortes de caja',			'/CorteCaja' ),
		(20,	7,  'Movimientos Bancarios',	'/MovimientoBancario' ),
		(21,	7,  'Movimientos Efectivo',		'/MovimientoEfectivo' );
		--SELECT * FROM CtFuncion
	END

	-- ====================================================
	-- CtAccion										
	-- ====================================================
	BEGIN
		-- DROP TABLE CtAccion
		CREATE TABLE CtAccion 
		(
			Id			SMALLINT	NOT NULL	IDENTITY,
			IdCtFuncion	TINYINT		NOT NULL,
			Nombre		VARCHAR(50)	NOT NULL,
			CONSTRAINT PK_CtAccion PRIMARY KEY (Id), 
			CONSTRAINT FK_CtAccion_CtFuncion 
			FOREIGN KEY (IdCtFuncion) REFERENCES CtFuncion(Id)
		);
	END

	BEGIN
		INSERT INTO CtAccion VALUES 
		----	1	SEGURIDAD
		--(01,	1,  'Usuarios',			'/Usuario'	 ),
		(1,	'Crear'),
		(1,	'Editar'),
		(1,	'Eliminar'),
		--(02,	1,  'Roles',			'/Rol'		 ),
		(2,	'Editar'),
		----	2	CONFIGURACIÓN
		--(03,	2,  'Empresa',			'/Empresa'	 ),
		(3,	'Editar'),
		--(04,	2,  'Sucursales',		'/Sucursal'	 ),
		(4,	'Crear'),
		(4,	'Editar'),
		(4,	'Eliminar'),
		----	3	PRODUCTOS
		--(05,	3,  'Secciones',		'/Seccion'	 ),
		(5,	'Crear'),
		(5,	'Editar'),
		(5,	'Eliminar'),
		--(06,	3,  'Grupos',			'/Grupo'	 ),
		(6,	'Crear'),
		(6,	'Editar'),
		(6,	'Eliminar'),
		--(07,	3,  'Productos',		'/Producto'	 ),
		(7,	'Crear'),
		(7,	'Editar'),
		(7,	'Eliminar'),
		(7,	'Crear Presentación de Producto'),
		(7,	'Editar Presentación de Producto'),
		(7,	'Eliminar Presentación de Producto'),
		----	4	COMPRAS
		--(08,	4,  'Proveedores',		'/Proveedor' ),
		(8, 'Crear'),
		(8, 'Editar'),
		(8, 'Eliminar'),
		--(09,	4,  'Compras',			'/Compra'	 ),
		(9, 'Crear'),
		(9, 'Editar'),
		(9, 'Eliminar'),
		----	5	INVENTARIOS
		--(10,	5,  'Lotes',			'/Lote'	 ),
		(10, 'Regularizar'),
		--(11,	5,  'Movimientos',		'/Movimiento'	 ),
		(11, 'Crear'),
		--(12,	5,  'Inventarios',		'/Inventario'),
		(12, 'Editar'),
		----	6	VENTAS
		--(13,	6,  'Clientes',			'/Cliente'	 ),
		(13, 'Crear'),
		(13, 'Editar'),
		(13, 'Eliminar'),
		--(14,	6,  'Pedidos',			'/Pedido'	 ),
		(14, 'Crear'),
	    (14, 'Surtir'),
		(14, 'Entregar'),
		(14, 'Cobrar'),
		(14, 'Cancelar'),
		(14, 'AjustarRechazo'),
		----	7	FINANZAS
		--(15,	7,  'Tipos de Gastos',	'/TipoGasto' ),
		(15, 'Crear'),
		(15, 'Editar'),
		(15, 'Eliminar'),
		--(16,	7,  'Gastos',			'/Gasto'	 ),
		(16, 'Crear'),
		(16, 'Editar'),
		(16, 'Eliminar'),
		--(17,	7,  'Pagos',			'/Pago'		 ),
		(17, 'Crear'),
		--(18,	7,  'Cobros',			'/Cobro'	 ),
		(18, 'Crear'),
		--(19,	7,  'Cortes de caja',	'/CorteCaja' )
		(19, 'Crear'),
		--(20,	7,  'Movimientos Bancarios',	'/MovimientoBancario' )
		(20, 'Conciliar')
	END

	-- ====================================================
	-- RlRolFuncion
	-- ====================================================
	BEGIN
		-- DROP TABLE RlRolFuncion
		CREATE TABLE RlRolFuncion 
		(
			Id				INT			NOT NULL	IDENTITY,
			IdCtRol			TINYINT		NOT NULL,
			IdCtFuncion		TINYINT		NOT NULL,
			Consultar		BIT			NOT NULL,
			VerSensibles	BIT			NOT NULL,
			CONSTRAINT PK_RlRolFuncion PRIMARY KEY (Id), 
			CONSTRAINT UK_RlRolFuncion UNIQUE (IdCtRol, IdCtFuncion),
			CONSTRAINT FK_RlRolFuncion_CtRol 
			FOREIGN KEY (IdCtRol) REFERENCES CtRol(Id),
			CONSTRAINT FK_RlRolFuncion_CtFuncion 
			FOREIGN KEY (IdCtFuncion) REFERENCES CtFuncion(Id)
		);
	END

	BEGIN
		-- SYSTEM
		INSERT INTO RlRolFuncion
		SELECT 0, Id, 1, 1 FROM CtFuncion ORDER BY Id;

		-- ADMINISTRADOR
		INSERT INTO RlRolFuncion
		SELECT 1, Id, 1, 1 FROM CtFuncion ORDER BY Id;

		-- VEDNDEDOR
		INSERT INTO RlRolFuncion
		SELECT 2, Id, 0, 0 FROM CtFuncion ORDER BY Id;

		-- ALMACENISTA
		INSERT INTO RlRolFuncion
		SELECT 3, Id, 0, 0 FROM CtFuncion ORDER BY Id;

		--REPARTIDOR  (solo consultar Pedidos; sin sensibles)
		INSERT INTO RlRolFuncion
		SELECT 4, Id, CASE WHEN Id = 14 THEN 1 ELSE 0 END, 0 FROM CtFuncion ORDER BY Id;
	END

	-- ====================================================
	-- RlRolAccion
	-- ====================================================
	BEGIN
		-- DROP TABLE RlRolAccion
		CREATE TABLE RlRolAccion 
		(
			Id			INT			NOT NULL	IDENTITY,
			IdCtRol		TINYINT		NOT NULL,
			IdCtAccion	SMALLINT	NOT NULL,
			TieneAcceso	BIT			NOT NULL,
			CONSTRAINT PK_RlRolAccion PRIMARY KEY (Id), 
			CONSTRAINT UK_RlRolAccion UNIQUE (IdCtRol, IdCtAccion),
			CONSTRAINT FK_RlRolAccion_CtRol 
			FOREIGN KEY (IdCtRol) REFERENCES CtRol(Id),
			CONSTRAINT FK_RlRolAccion_CtAccion 
			FOREIGN KEY (IdCtAccion) REFERENCES CtAccion(Id)
		);
	END

	BEGIN
		-- SYSTEM
		INSERT INTO RlRolAccion
		SELECT 0, Id, 1 FROM CtAccion ORDER BY Id

		-- ADMINISTRADOR
		INSERT INTO RlRolAccion
		SELECT 1, Id, 1 FROM CtAccion ORDER BY Id

		-- VENDEDOR
		INSERT INTO RlRolAccion
		SELECT 2, Id, 0 FROM CtAccion ORDER BY Id

		-- ALMACENISTA
		INSERT INTO RlRolAccion
		SELECT 3, Id, 0 FROM CtAccion ORDER BY Id

		--REPARTIDOR  (solo acción Entregar de Pedidos)
		INSERT INTO RlRolAccion
		SELECT 4, Id, CASE WHEN IdCtFuncion = 14 AND Nombre = 'Entregar' THEN 1 ELSE 0 END FROM CtAccion ORDER BY Id
	END

	PRINT 'Termina: MÓDULO: SEGURIDAD'
	PRINT '===================================================='
END


-- #################################################### --
-- MÓDULO: PRODUCTOS
-- #################################################### --
BEGIN
	PRINT '===================================================='
	PRINT 'Inicia: MÓDULO: PRODUCTOS'

	-- ====================================================
	-- CtSeccion										
	-- ====================================================
	BEGIN
		-- DROP TABLE CtSeccion
		CREATE TABLE CtSeccion
		(
			Id		TINYINT			NOT NULL IDENTITY,
			Nombre	VARCHAR(50)		NOT NULL,
			CONSTRAINT PK_CtSeccion PRIMARY KEY (Id),
			CONSTRAINT UK_CtSeccion UNIQUE (Nombre)
		);
	END

	BEGIN
		INSERT INTO CtSeccion (Nombre)
		VALUES 
			(UPPER('Materiales de Obra Negra')),		-- 1
			(UPPER('Plomería e Hidráulicos')),			-- 2
			(UPPER('Agua y Energía')),					-- 3
			(UPPER('Electricidad')),					-- 4
			(UPPER('Herramientas Manuales')),			-- 5
			(UPPER('Herramientas Eléctricas')),			-- 6
			(UPPER('Acabados')),						-- 7
			(UPPER('Pisos y Recubrimientos')),			-- 8
			(UPPER('Carpintería y Ferretería')),		-- 9
			(UPPER('Tlapalería y Adhesivos')),			-- 10
			(UPPER('Seguridad y Equipo de Trabajo')),	-- 11
			(UPPER('Baño y Cocina')),					-- 12
			(UPPER('Jardín y Exterior'));				-- 13
	END

	-- ====================================================
	-- CtGrupo
	-- ====================================================
	BEGIN
		-- DROP TABLE CtGrupo
		CREATE TABLE CtGrupo
		(
			Id			SMALLINT		NOT NULL	IDENTITY,
			IdCtSeccion	TINYINT			NOT NULL,
			Nombre		VARCHAR(50)		NOT NULL,
			Descripcion	VARCHAR(500)	NOT NULL,
			CONSTRAINT PK_CtGrupo PRIMARY KEY (Id),
			CONSTRAINT UK_CtGrupo UNIQUE (Nombre),
			CONSTRAINT FK_CtGrupo_CtSeccion 
			FOREIGN KEY(IdCtSeccion) REFERENCES CtSeccion(Id)
		);
	END

	BEGIN
		INSERT INTO CtGrupo (IdCtSeccion, Nombre, Descripcion)
		VALUES

		-- ============================================================
		-- 1. MATERIALES DE OBRA NEGRA
		-- ============================================================
		(1, UPPER('Cementos y morteros'),
			UPPER('Materiales base para mezclas estructurales y de acabado. Incluye: cemento gris CPC 30/40, cemento blanco, mortero seco preparado, mezcla para block. Orientado a fundaciones, castillos, cadenas y aplanados base.')),

		(1, UPPER('Block y tabiques'),
			UPPER('Elementos prefabricados para construcción de muros. Incluye: block hueco 10x20x40, block sólido, tabique rojo recocido, tabique prensado, tabique de barro. Indicar dimensiones y resistencia al dar de alta.')),

		(1, UPPER('Agregados pétreos'),
			UPPER('Materiales de relleno y mezcla. Incluye: arena fina, arena gruesa, grava 3/4, piedra braza, tepetate, granzón, polvo de piedra. Se vende comúnmente por m³ o viaje.')),

		(1, UPPER('Cal y yeso'),
			UPPER('Insumos para aplanados, estucados y recubrimientos base. Incluye: cal hidratada en polvo, cal en pasta, yeso en saco. Ideal para primera y segunda mano de muros.')),

		(1, UPPER('Varilla y acero de refuerzo'),
			UPPER('Productos de acero para estructuras de concreto. Incluye: varilla corrugada 3/8", 1/2", 5/8", 3/4", alambrón, malla electrosoldada, malla de gallinero, clavos de construcción. Indicar calibre y longitud.')),

		(1, UPPER('Madera para cimbra'),
			UPPER('Madera de uso temporal en obra para moldear concreto y estructuras. Incluye: tabla de pino cimbra, tablón, polín 4x4, triplay 9mm/12mm. Registrar medidas (grosor, ancho y largo).')),

		(1, UPPER('Perfiles metálicos y estructurales'),
			UPPER('Elementos metálicos para estructuras, marcos y soportes. Incluye: PTR cuadrado y rectangular, ángulo de acero, canal, solera, placa de acero, tubería estructural. Indicar sección y calibre.')),

		(1, UPPER('Impermeabilizantes de obra negra'),
			UPPER('Aditivos y productos para proteger estructuras desde la etapa de obra negra. Incluye: impermeabilizante para cimientos, aditivo impermeabilizante para mezclas, polietileno para losas.')),

		(1, UPPER('Complementos de obra'),
			UPPER('Materiales auxiliares para amarre y sujeción en obra. Incluye: alambre recocido, alambre galvanizado, lazo de henequén, mecate, costales, cubetas para mezcla.')),


		-- ============================================================
		-- 2. PLOMERÍA E HIDRÁULICOS
		-- ============================================================
		(2, UPPER('Tubería PVC hidráulica'),
			UPPER('Tubos de PVC para instalaciones de agua fría a presión. Incluye: tubo PVC cédula 40 en 1/2", 3/4", 1", 1.5", 2". Registrar diámetro nominal, longitud y presión de trabajo.')),

		(2, UPPER('Tubería PVC sanitaria'),
			UPPER('Tubos de PVC para desagüe y drenaje. Incluye: tubo sanitario 2", 3", 4", 6". Registrar diámetro y longitud. No apto para agua a presión.')),

		(2, UPPER('Tubería CPVC y cobre'),
			UPPER('Tubería para agua caliente. Incluye: tubo CPVC 1/2" y 3/4", tubo de cobre rígido tipo M/L, tubo de cobre flexible. Indicar si es para agua fría/caliente y diámetro.')),

		(2, UPPER('Tubería galvanizada'),
			UPPER('Tubería de acero galvanizado para instalaciones de gas y agua en zonas de alta presión. Incluye: tubería galvanizada 1/2", 3/4", 1", 1.5". Indicar diámetro y longitud.')),

		(2, UPPER('Conexiones hidráulicas PVC'),
			UPPER('Piezas para unir y direccionar tubería PVC hidráulica. Incluye: codos 45°/90°, tees, coples, reducciones, tapones, adaptadores macho/hembra. Indicar diámetro.')),

		(2, UPPER('Conexiones sanitarias PVC'),
			UPPER('Accesorios para unir tubería de desagüe. Incluye: codos sanitarios, yees, tees sanitarias, coples, trampas tipo P, sifones. Indicar diámetro.')),

		(2, UPPER('Conexiones de cobre y CPVC'),
			UPPER('Piezas para tubería de cobre y CPVC. Incluye: codos, tees, coples, adaptadores soldables y de compresión. Indicar material y diámetro.')),

		(2, UPPER('Llaves y válvulas'),
			UPPER('Dispositivos para controlar el flujo de agua. Incluye: llave de esfera 1/2"-2", válvula de globo, válvula de ángulo, válvula check, llave de nariz, flotador de tinaco. Indicar diámetro y tipo de conexión.')),

		(2, UPPER('Registros y coladeras'),
			UPPER('Accesorios para captación y acceso en drenaje. Incluye: coladera de piso, coladera de techo, contra-tapa de registro, tapón de limpieza. Indicar medida y material.')),

		(2, UPPER('Accesorios de instalación hidráulica'),
			UPPER('Complementos para montaje y sellado de tuberías. Incluye: teflón, soldadura líquida para PVC, abrazaderas metálicas y plásticas, empaques, manguera flexible, kuplex. Indicar diámetro o uso.')),

		(2, UPPER('Mangueras de uso general'),
			UPPER('Mangueras flexibles para abasto y riego. Incluye: manguera de jardín 1/2" y 3/4", manguera de nivel, manguera cristal, conectores y pistolas. Indicar diámetro interior y longitud.')),


		-- ============================================================
		-- 3. AGUA Y ENERGÍA
		-- ============================================================
		(3, UPPER('Calentadores de agua'),
			UPPER('Equipos para suministro de agua caliente. Incluye: calentadores de gas de 6/10/20 litros, boilers solares, calentadores de paso instantáneo eléctrico. Indicar capacidad y tipo de energía.')),

		(3, UPPER('Tinacos y cisternas'),
			UPPER('Almacenamiento de agua potable. Incluye: tinacos rotoplas 450/750/1100 L, cisternas prefabricadas, tapas y accesorios. Indicar capacidad en litros y si es tinaco aéreo o cisterna enterrada.')),

		(3, UPPER('Bombas de agua'),
			UPPER('Equipos para impulsar y presurizar agua. Incluye: bombas centrífugas, periféricas, presurizadoras, sistemas hidroneumáticos. Indicar HP, voltaje y uso (doméstico/industrial).')),

		(3, UPPER('Gas LP y accesorios'),
			UPPER('Accesorios para instalaciones de gas. Incluye: reguladores de gas, manguera de gas, válvulas de paso, detectores de fugas. No incluye tanques. Indicar presión y diámetro.')),


		-- ============================================================
		-- 4. ELECTRICIDAD
		-- ============================================================
		(4, UPPER('Cables y alambres'),
			UPPER('Conductores eléctricos para instalaciones residenciales e industriales. Incluye: cable THW calibre 12, 14, 10, 8, 6; alambre duplex, tripolar, cable de uso rudo. Indicar calibre AWG y número de conductores.')),

		(4, UPPER('Conduits y canaletas'),
			UPPER('Protección y conducción del cableado eléctrico. Incluye: conduit PVC 3/4" y 1", conduit metálico, canaleta plástica ranurada, uniones, curvas y cajas de registro. Indicar diámetro o sección.')),

		(4, UPPER('Centros de carga y protección'),
			UPPER('Distribución y protección de circuitos eléctricos. Incluye: centros de carga 4/8/12/20 circuitos, pastillas de 15/20/30 A, interruptores termomagnéticos, fusibles. Indicar número de circuitos y amperaje.')),

		(4, UPPER('Interruptores y contactos'),
			UPPER('Dispositivos de control y conexión eléctrica. Incluye: apagadores sencillos, dobles, de 3 vías, contactos polarizados, contactos GFCI, placas y tapas. Indicar voltaje y amperaje.')),

		(4, UPPER('Clavijas y extensiones'),
			UPPER('Distribución temporal de energía eléctrica. Incluye: extensiones domésticas, industriales, clavijas macho/hembra, multicontactos con protector de voltaje. Indicar longitud y calibre.')),

		(4, UPPER('Iluminación LED'),
			UPPER('Sistemas de iluminación eficiente para interior y exterior. Incluye: focos LED A19/PAR/GU10, lámparas de techo, paneles LED, reflectores, tiras LED, lámparas de emergencia. Indicar watts y temperatura de color.')),

		(4, UPPER('Accesorios eléctricos'),
			UPPER('Complementos para instalaciones eléctricas. Incluye: cintas aislantes, conectores tipo mariposa, capuchones, cajas octagonales y de paso, amarres de cables, bridas. Indicar uso o calibre compatible.')),


		-- ============================================================
		-- 5. HERRAMIENTAS MANUALES
		-- ============================================================
		(5, UPPER('Medición y nivelación'),
			UPPER('Instrumentos para medir y nivelar en obra. Incluye: cinta métrica 3/5/8 m, nivel de burbuja, escuadra de carpintero, plomada, flexómetro, manguera de nivel, nivel láser. Indicar longitud o tipo.')),

		(5, UPPER('Golpe y demolición'),
			UPPER('Herramientas de impacto manual. Incluye: martillos de carpintero/bola/albañil, marros de 3/5/8 kg, mazos de hule, cinceles, punteros, mandarrias. Indicar peso y tipo de mango.')),

		(5, UPPER('Corte manual'),
			UPPER('Herramientas para cortar madera y metal. Incluye: seguetas con hojas reemplazables, serruchos de punta, cúter, navajas, guillotinas para tubería. Indicar material a cortar.')),

		(5, UPPER('Sujeción y ajuste'),
			UPPER('Herramientas para sujetar y ajustar piezas. Incluye: pinzas de presión, llaves españolas y de tubo, llave perico, desarmadores planos y de cruz, juegos de llaves Allen. Indicar medida o juego.')),

		(5, UPPER('Albañilería y mezcla'),
			UPPER('Herramientas para preparar y aplicar mezclas. Incluye: cucharas de albañil, llanas de acero y plástico, espátulas de distintos anchos, paletas, codales. Indicar medida.')),

		(5, UPPER('Pintura y acabados manuales'),
			UPPER('Herramientas para aplicar recubrimientos. Incluye: brochas planas 1"/2"/3"/4", brochas de corte, rodillos de pelo corto/largo, charolas, extensiones para rodillo, espátulas de empapelado.')),

		(5, UPPER('Lijas y abrasivos'),
			UPPER('Materiales para lijar y preparar superficies. Incluye: lija de madera grano 60/100/150/220, lija de agua, lija al seco, lija en rollo, esponjas abrasivas. Indicar grano y soporte (hoja/rollo).')),

		(5, UPPER('Excavación y campo'),
			UPPER('Herramientas para mover tierra y materiales. Incluye: palas cuadradas y de punta, picos, tlalachos, barretas, azadones, rastrillo de campo. Indicar tipo y si incluye mango o va sin él. Aplica también cabos de repuesto.')),

		(5, UPPER('Transporte de materiales'),
			UPPER('Herramientas para mover y trasladar materiales en obra. Incluye: carretillas de construcción de 1 llanta, carretillas de 2 ruedas, cubetas de 18 L, costales de rafia. Indicar capacidad de carga.')),

		(5, UPPER('Preparación y cribado'),
			UPPER('Herramientas para filtrar y separar materiales. Incluye: arneros/cribas de distintas mallas (finas, medianas, gruesas), cedazos de mano, armazones metálicos. Indicar apertura de malla.')),

		(5, UPPER('Plomería manual'),
			UPPER('Herramientas para instalaciones hidráulicas y sanitarias. Incluye: cortatubos para PVC/cobre/galvanizado, dobladores de cobre, soplete de gas, estañador, llave stilson, llave de cadena. Indicar diámetro compatible.')),


		-- ============================================================
		-- 6. HERRAMIENTAS ELÉCTRICAS Y EQUIPO
		-- ============================================================
		(6, UPPER('Taladros y rotomartillos'),
			UPPER('Perforación eléctrica para distintos materiales. Incluye: taladros eléctricos de 1/2", rotomartillos SDS, taladros inalámbricos. Registrar potencia en watts, voltaje y tipo de broca compatible.')),

		(6, UPPER('Brocas y puntas'),
			UPPER('Accesorios de perforación y atornillado. Incluye: brocas para concreto, madera, metal; brocas de diamante, brocas TCT, puntas de impacto Phillips/plana/Torx. Indicar diámetro, longitud y material.')),

		(6, UPPER('Esmeriles y discos'),
			UPPER('Corte, desbaste y pulido. Incluye: esmeril angular 4.5"/7", discos de corte para metal y concreto, discos de desbaste, discos diamantados. Indicar diámetro y tipo de trabajo.')),

		(6, UPPER('Sierras eléctricas'),
			UPPER('Corte de madera, metal y derivados. Incluye: sierra circular, sierra caladora, sierra de banco. Indicar potencia y tipo de hoja compatible.')),

		(6, UPPER('Pulidoras y lijadoras eléctricas'),
			UPPER('Acabados superficiales con motor. Incluye: pulidora eléctrica, lijadora orbital, lijadora de banda, detalladoras. Indicar potencia y tipo de lija compatible.')),

		(6, UPPER('Soldadura'),
			UPPER('Equipos y consumibles para unión de metales. Incluye: soldadora de arco eléctrico, electrodos 6011/6013, careta de soldar, guantes de carnaza. Indicar amperaje y tipo de electrodo.')),

		(6, UPPER('Compresores y pistolas'),
			UPPER('Equipos de aire comprimido para pintura y trabajo con pistola. Incluye: compresores 1/2-3 HP, pistola de gravedad, pistola touch-up, manguera de aire, accesorios de compresor. Indicar litros/min y PSI.')),


		-- ============================================================
		-- 7. ACABADOS
		-- ============================================================
		(7, UPPER('Pinturas vinílicas e interior'),
			UPPER('Pinturas para interiores en muros y techos. Incluye: vinílica blanca y de color, pintura lavable, pintura económica de cubeta. Indicar rendimiento m²/L y presentación (cubeta/galón/cuarto).')),

		(7, UPPER('Pinturas de aceite y esmalte'),
			UPPER('Pinturas de alto rendimiento y brillo para madera y metal. Incluye: esmalte alkídico, pintura de aceite, esmalte para piso, pintura anticorrosiva. Indicar color y presentación.')),

		(7, UPPER('Pinturas para fachada y exterior'),
			UPPER('Pinturas resistentes a intemperie para exteriores. Incluye: pintura elastomérica, acrílica exterior, pintura de caucho, fachadas texturizadas. Indicar rendimiento y tipo de superficie.')),

		(7, UPPER('Selladores y primers'),
			UPPER('Preparación de superficie antes de pintar. Incluye: sellador vinílico, sellador de yeso, primer anticorrosivo, fijador para pintura antigua. Indicar tipo de superficie a aplicar.')),

		(7, UPPER('Barnices y lacas'),
			UPPER('Protección y acabado para madera. Incluye: barniz brillante y semimate, barniz marino, sellador para madera, laca nitrocelulósica. Indicar tipo de madera y uso interior/exterior.')),

		(7, UPPER('Impermeabilizantes'),
			UPPER('Protección contra humedad en techos y muros. Incluye: impermeabilizante acrílico en cubeta, impermeabilizante de poliuretano, membrana impermeabilizante, sellador de juntas y grietas. Indicar m² de cobertura.')),

		(7, UPPER('Disolventes y limpieza'),
			UPPER('Productos para diluir pinturas y limpiar herramientas. Incluye: thinner acrílico, thinner laca, aguarrás, acetona, removedor de pintura. Indicar uso compatible (tipo de pintura).')),

		(7, UPPER('Aplanados y texturas'),
			UPPER('Recubrimientos decorativos para muros interiores y exteriores. Incluye: estuco, pasta texturizada, yeso acabado, pasta lista para usar, pintura texturizada. Indicar rendimiento y tipo de acabado.')),

		(7, UPPER('Techados y láminas'),
			UPPER('Materiales para cubiertas de techo. Incluye: lámina galvanizada cal. 26/28, lámina pintro, lámina traslúcida de policarbonato, lámina de fibrocemento (Plycem). Indicar calibre, longitud y ancho.')),

		(7, UPPER('Molduras y elementos decorativos'),
			UPPER('Elementos de decoración para interiores. Incluye: molduras de unicel y poliestireno, casetones para techo, cornisas, marcos decorativos. Indicar medidas y perfil.')),


		-- ============================================================
		-- 8. PISOS Y RECUBRIMIENTOS
		-- ============================================================
		(8, UPPER('Cerámica y porcelanato'),
			UPPER('Revestimientos para pisos y muros de alta resistencia. Incluye: loseta cerámica, porcelanato rectificado y rústico, pisos de pasta. Indicar medidas (ej. 33x33, 60x60), acabado y uso (piso/muro).')),

		(8, UPPER('Azulejo y mosaico'),
			UPPER('Revestimientos para muros en baño y cocina. Incluye: azulejo blanco, azulejo decorado, mosaico de vidrio, mosaico de pasta, cenefas. Indicar medidas y si es para muro o piso.')),

		(8, UPPER('Piedra natural y artificial'),
			UPPER('Acabados con materiales pétreos. Incluye: cantera, tezontle, piedra laja, mármol, travertino, piedra reconstituida. Indicar si es para interior/exterior y espesor.')),

		(8, UPPER('Adoquín y concreto'),
			UPPER('Pisos para exteriores y andadores. Incluye: adoquín de concreto en distintas formas y colores, concreto estampado. Indicar resistencia y m² de cobertura.')),

		(8, UPPER('Pegamentos y adhesivos para piso'),
			UPPER('Productos para instalación de pisos y azulejos. Incluye: pegazulejo blanco y gris, adhesivo epóxico, bondex, adhesivo de contacto, mezcla para granito. Indicar rendimiento kg/m².')),

		(8, UPPER('Boquillas y lechadas'),
			UPPER('Materiales para rellenar juntas entre piezas. Incluye: boquilla (junta) en distintos colores, lechada de cemento, lechada epóxica. Indicar color y ancho de junta recomendado.')),

		(8, UPPER('Zoclos y tapajuntas'),
			UPPER('Elementos de remate entre piso y muro. Incluye: zoclo cerámico, zoclo de PVC, tapajuntas de aluminio y PVC, perfil de transición. Indicar medida y material.')),


		-- ============================================================
		-- 9. CARPINTERÍA Y FERRETERÍA
		-- ============================================================
		(9, UPPER('Tornillería y fijación'),
			UPPER('Elementos de sujeción para todo tipo de materiales. Incluye: tornillos para madera, tornillos autorroscantes, tornillos para concreto, taquetes de plástico y metálicos, anclas expansivas, pijas, remaches pop. Indicar diámetro, longitud y material.')),

		(9, UPPER('Clavos'),
			UPPER('Clavos para carpintería, construcción y acabados. Incluye: clavos de acero para concreto, clavos de cabeza plana y redonda para madera, clavos para clavadora neumática, grapas. Indicar calibre y longitud en pulgadas.')),

		(9, UPPER('Bisagras y herrajes'),
			UPPER('Elementos metálicos para puertas, ventanas y muebles. Incluye: bisagras de acero y latón, bisagras de piano, pasadores, aldabas, armellas, ganchos, escuadras metálicas, ángulos. Indicar medida y material.')),

		(9, UPPER('Cerraduras y candados'),
			UPPER('Seguridad y acceso para puertas y portones. Incluye: chapas de pomo, chapas de palanca, cerraduras embutidas, candados de arco, candados de alta seguridad, cierrapuertas. Indicar tipo de puerta y nivel de seguridad.')),

		(9, UPPER('Cadenas y cables de acero'),
			UPPER('Elementos de sujeción y seguridad de alta resistencia. Incluye: cadena de acero galvanizado por metro, cable de acero, eslabones, grilletes, seguros de cable. Indicar diámetro y resistencia en kg.')),

		(9, UPPER('Tableros y maderas'),
			UPPER('Materiales de madera para carpintería general. Incluye: triplay de 4x8 en 6/9/12 mm, MDF, aglomerado, melamina, tabla de pino. Indicar espesor, dimensión y si está acabado.')),

		(9, UPPER('Perfiles y acabados de aluminio'),
			UPPER('Perfiles metálicos para cancelería y acabados. Incluye: ángulo de aluminio, perfil T, perfil U, jaladores, riel para puertas corredizas. Indicar longitud y sección.')),

		(9, UPPER('Puertas y marcos'),
			UPPER('Puertas para interiores y exteriores. Incluye: puertas de madera sólida y entablerada, puertas de MDF, puertas metálicas, marcos de madera y metálicos. Indicar dimensiones, material y si incluye marco.')),


		-- ============================================================
		-- 10. TLAPALERÍA Y ADHESIVOS
		-- ============================================================
		(10, UPPER('Adhesivos y pegamentos'),
			UPPER('Pegamentos para distintos materiales. Incluye: pegamento blanco (carpintero), adhesivo de contacto (amarillo), pegamento epóxico bicomponente, pegamento para PVC, cianocrilato (Super Bonder). Indicar materiales a unir y tiempo de fraguado.')),

		(10, UPPER('Siliconas y selladores'),
			UPPER('Sellado de juntas y grietas en distintos materiales. Incluye: silicona transparente, silicona blanca, silicona de alta temperatura, sellador de poliuretano, sellador acrílico (backer rod). Indicar tipo de superficie y temperatura de trabajo.')),

		(10, UPPER('Lubricantes y antioxidantes'),
			UPPER('Productos para mantenimiento de piezas metálicas. Incluye: WD-40, aceite 3-en-1, grasa de litio, antioxidante, lubricante en spray. Indicar uso (cadenas, bisagras, pernos) y presentación.')),

		(10, UPPER('Cintas adhesivas'),
			UPPER('Cintas para sellado, protección y señalización. Incluye: cinta canela de embalaje, masking tape, cinta doble cara, cinta de tela, cinta de teflón, cinta de aluminio para ductos. Indicar ancho y largo del rollo.')),

		(10, UPPER('Estopas y materiales de sellado'),
			UPPER('Materiales para sellar juntas en instalaciones. Incluye: estopa blanca, estopa negra, teflón en rollo, cuerda de yute para rosca, empaque de hule. Indicar diámetro o uso en plomería/gas.')),

		(10, UPPER('Desengrasantes y limpiadores'),
			UPPER('Productos químicos para limpieza industrial. Incluye: desengrasante en spray, cloro industrial, ácido muriático, desincrustante, limpiador multiusos concentrado. Indicar superficie de aplicación y dilución.')),


		-- ============================================================
		-- 11. SEGURIDAD Y EQUIPO DE TRABAJO
		-- ============================================================
		(11, UPPER('Equipo de protección personal'),
			UPPER('Protección para trabajadores en obra. Incluye: cascos de seguridad, lentes de protección, guantes de carnaza, guantes de látex, tapones auditivos, cubrebocas. Indicar talla y tipo de riesgo.')),

		(11, UPPER('Calzado de seguridad'),
			UPPER('Calzado con protección para obra y trabajo pesado. Incluye: botas de casquillo, botas de hule, zapatos dieléctricos. Indicar talla y tipo de protección (casquillo/suela antiderrapante).')),

		(11, UPPER('Escaleras y andamios'),
			UPPER('Acceso seguro a alturas en obra. Incluye: escaleras de aluminio de 3/4/6/8 peldaños, escaleras de extensión, caballetes de pintura, andamios modulares. Indicar altura máxima y capacidad de carga.')),

		(11, UPPER('Ropa de trabajo'),
			UPPER('Ropa para protección y comodidad en obra. Incluye: overoles, chalecos de trabajo, pantalones de mezclilla reforzados, camisas de trabajo. Indicar talla y material.')),

		(11, UPPER('Señalización de obra'),
			UPPER('Elementos para delimitar y advertir en zonas de trabajo. Incluye: cintas de precaución, conos de tráfico, señales de advertencia, mallas de seguridad, casetas de seguridad. Indicar uso y dimensiones.')),


		-- ============================================================
		-- 12. BAÑO Y COCINA
		-- ============================================================
		(12, UPPER('Sanitarios'),
			UPPER('Aparatos sanitarios principales. Incluye: inodoro de una pieza y dos piezas, inodoro de fluxómetro, lavabo de sobreponer y empotrar, mingitorio. Indicar color, marca y tipo de instalación.')),

		(12, UPPER('Grifería y mezcladores'),
			UPPER('Llaves y mezcladores para baño y cocina. Incluye: llave de lavabo, mezcladoras de baño, llaves de cocina, manerales de regadera, regaderas de mano y de techo. Indicar acabado (cromado, níquel, bronce).')),

		(12, UPPER('Regaderas y accesorios de baño'),
			UPPER('Accesorios decorativos y funcionales para baño. Incluye: regaderas de mano, barras de baño, portarrollos, toalleros, jaboneras, espejos. Indicar material y acabado.')),

		(12, UPPER('Muebles de baño'),
			UPPER('Mobiliario para organización en baño. Incluye: vanities con lavabo integrado, gabinetes de pared, espejos con repisa, organizadores de esquina. Indicar dimensiones y material.')),

		(12, UPPER('Tarjas y fregaderos'),
			UPPER('Tarjas para cocina y lavandería. Incluye: tarja de acero inoxidable 1/2 tinas, fregadero de barro, tarja de granito, tarja de cocina empotrada. Indicar número de tinas y dimensiones.')),

		(12, UPPER('Accesorios de cocina e instalación'),
			UPPER('Complementos para instalación en cocina y baño. Incluye: conexiones flexibles, válvulas de ángulo, coladeras para fregadero, sifones de tarja, kit de montaje de lavabo. Indicar diámetro y uso.')),


		-- ============================================================
		-- 13. JARDÍN Y EXTERIOR
		-- ============================================================
		(13, UPPER('Mangueras y riego'),
			UPPER('Sistemas de riego y abasto de agua para jardín. Incluye: manguera de jardín 1/2" y 3/4", manguera expandible, aspersores, pistolas de riego, conectores y adaptadores. Indicar longitud y diámetro.')),

		(13, UPPER('Herramienta de jardín'),
			UPPER('Herramientas para mantenimiento de áreas verdes. Incluye: azadón, rastrillo, pala jardinera, tijeras de podar, machete, pala de mano. Indicar si incluye mango y material.')),

		(13, UPPER('Fumigación y control de plagas'),
			UPPER('Productos para control de plagas en jardín y hogar. Incluye: insecticidas, herbicidas, fungicidas, bombas de fumigación manual. Indicar plaga objetivo y tipo de aplicación.')),

		(13, UPPER('Mallas y cercos'),
			UPPER('Materiales para cercar y delimitar áreas. Incluye: malla de gallinero, malla ciclónica, malla sombra, alambre de púas, postes metálicos. Indicar calibre, apertura de malla y altura.')),

		(13, UPPER('Macetas y jardinería decorativa'),
			UPPER('Contenedores y elementos decorativos para jardín. Incluye: macetas de plástico y barro en distintos tamaños, jardineras de piso y pared, tierra de hoja, sustrato. Indicar material y capacidad en litros.'));

	END

	-- ====================================================
	-- CtUnidadBaseInventario
	-- ====================================================
	BEGIN
		-- DROP TABLE CtUnidadBaseInventario
		CREATE TABLE CtUnidadBaseInventario
		(
			Id			TINYINT			NOT NULL,
			Nombre		VARCHAR(50)		NOT NULL,
			Abreviatura	VARCHAR(10)		NOT NULL,
			CONSTRAINT PK_CtUnidadMedida PRIMARY KEY (Id),
			CONSTRAINT UK_CtUnidadMedida UNIQUE (Nombre)
		);
	END

	BEGIN
		INSERT INTO CtUnidadBaseInventario (Id, Nombre, Abreviatura) 
		VALUES
			(1, 'KILOGRAMO', 'KG'),
			(2, 'LITRO', 'LT'),
			(3, 'METRO CUADRADO', 'M2'),
			(4, 'METRO CÚBICO', 'M3'),
			(5, 'METRO LINEAL', 'M'),
			(6, 'PIEZA', 'PZ');

	END	

	-- ====================================================
	-- CtProducto										
	-- ====================================================
	BEGIN
		--DROP TABLE CtProducto
		CREATE TABLE CtProducto
		(
			Id							INT				NOT NULL	IDENTITY,
			IdCtGrupo					SMALLINT		NOT NULL,
			Nombre						VARCHAR(50)		NOT NULL,
			EspecificacionTecnica		VARCHAR(50)		NULL,
			Marca						VARCHAR(50)		NULL,
			IdCtUnidadBaseInventario	TINYINT			NOT NULL,	-- unidad de inventario
			Imagen						VARCHAR(MAX)	NULL,		-- ruta o base64
			CodigoBarras				VARCHAR(25)		NULL,
			CodigoBarrasInterno			BIT				NOT NULL,
			BajoPedido					BIT				NOT NULL,
			EntregaMostrador			BIT				NOT NULL,
			Activo						BIT				NOT NULL,
			CONSTRAINT PK_CtProducto PRIMARY KEY (Id),
			CONSTRAINT UK_CtProducto UNIQUE (Nombre, EspecificacionTecnica, Marca),
			CONSTRAINT FK_CtProducto_CtGrupo 
			FOREIGN KEY (IdCtGrupo) REFERENCES CtGrupo (Id),
			CONSTRAINT FK_CtProducto_CtUnidadBaseInventario 
			FOREIGN KEY (IdCtUnidadBaseInventario) REFERENCES CtUnidadBaseInventario (Id)
		);
		
		CREATE UNIQUE INDEX UK_CtProducto_CodigoBarras 
		ON CtProducto(CodigoBarras)
		WHERE CodigoBarras IS NOT NULL;

	END	

	-- ====================================================
	-- CtPresentacionProducto										
	-- ====================================================
	BEGIN 
	
		--DROP TABLE CtPresentacionProducto
		CREATE TABLE CtPresentacionProducto
		(
			Id					INT				NOT NULL IDENTITY,
			IdCtProducto		INT				NOT NULL,
			Nombre				VARCHAR(50)		NOT NULL,
			FactorUBI			DECIMAL(18,5)	NOT NULL,	-- cuántas unidades base de inventario representa
			EsParaCompra		BIT				NOT NULL,
			EsParaVenta			BIT				NOT NULL,
			PrecioVenta			DECIMAL(18,5)	NOT NULL,	-- precio específico de esta presentación
			PrecioMayoreo		DECIMAL(18,5)	NULL,		-- opcional
			UnidadesMayoreo		SMALLINT		NULL,		-- aplica solo a presentaciones completas
			CONSTRAINT PK_CtPresentacionProducto PRIMARY KEY (Id),
			CONSTRAINT UK_CtPresentacionProducto UNIQUE (IdCtProducto, Nombre, FactorUBI),
			CONSTRAINT FK_CtPresentacionProducto_CtProducto 
			FOREIGN KEY (IdCtProducto) REFERENCES CtProducto (Id),
		);

	END

	-- ====================================================
	-- RlProductoSucursal										
	-- ====================================================
	BEGIN
		--DROP TABLE RlProductoSucursal
		CREATE TABLE RlProductoSucursal
		(
			IdCtProducto		INT				NOT NULL,
			IdCtSucursal		TINYINT			NOT NULL,
			StockMinimoUBI		DECIMAL(18,5)   NOT NULL,   -- En unidad base de Inventario
			StockMaximoUBI		DECIMAL(18,5)   NOT NULL,   -- En unidad base de Inventario
			Regularizado		BIT				NOT NULL DEFAULT 0,
			Ubicacion			VARCHAR(50)		NULL,
			StockReservadoUBI	DECIMAL(18,5)	NOT NULL DEFAULT 0,
			CONSTRAINT PK_RlProductoSucursal 
				PRIMARY KEY (IdCtProducto, IdCtSucursal),
			CONSTRAINT FK_RlProductoSucursal_CtProducto 
				FOREIGN KEY (IdCtProducto) REFERENCES CtProducto(Id),
			CONSTRAINT FK_RlProductoSucursal_CtSucursal 
				FOREIGN KEY (IdCtSucursal) REFERENCES CtSucursal(Id)
		);
	END

	PRINT 'Termina: MÓDULO: PRODUCTOS'
	PRINT '===================================================='
END 


-- #################################################### --
-- MÓDULO: COMPRAS
-- #################################################### --
BEGIN
	PRINT '===================================================='
	PRINT 'Inicia: MÓDULO: COMPRAS'

	-- ====================================================
	-- CtProveedor										
	-- ====================================================
	BEGIN
		-- DROP TABLE CtProveedor
		CREATE TABLE CtProveedor
		(
			Id					INT				NOT NULL IDENTITY,
			Nombre				VARCHAR(50)		NOT NULL,
			Direccion			VARCHAR(250)	NOT NULL,
			Telefono			VARCHAR(10)		NOT NULL,
			NombreContacto		VARCHAR(50)		NOT NULL,
			TelefonoContacto	VARCHAR(50)		NOT NULL,
			CONSTRAINT PK_CtProveedor PRIMARY KEY (Id),
			CONSTRAINT UK_CtProveedor UNIQUE (Nombre)
		);
	END

	-- ============================================
	-- RlProductoProveedor
	-- ============================================
	BEGIN
		--DROP TABLE RlProductoProveedor
		CREATE TABLE RlProductoProveedor
		(
			IdCtProducto	INT	NOT NULL,
			IdCtProveedor	INT	NOT NULL,
			CONSTRAINT PK_RlProductoProveedor PRIMARY KEY (IdCtProveedor, IdCtProducto),
			CONSTRAINT FK_RlProductoProveedor_CtProducto 
			FOREIGN KEY (IdCtProducto) REFERENCES CtProducto (Id),
			CONSTRAINT FK_RlProductoProveedor_CtProveedor 
			FOREIGN KEY (IdCtProveedor) REFERENCES CtProveedor (Id)
		)
	END
	
	-- ===============================================
	-- OpCompra
	-- ===============================================
	BEGIN
		-- DROP TABLE OpCompra
		CREATE TABLE OpCompra
		(
			Id					INT             NOT NULL IDENTITY,
			Fecha               DATETIME        NOT NULL,
			-- Ubicación y cliente
			IdCtSucursal        TINYINT			NOT NULL,
			Numero				INT				NOT NULL,
			IdCtProveedor       INT             NOT NULL,
			Total               DECIMAL(18,5)   NOT NULL,
			TotalPagado			DECIMAL(18,5)	NOT NULL,
			CONSTRAINT PK_OpCompra PRIMARY KEY (Id),
			CONSTRAINT UK_OpCompra UNIQUE (IdCtSucursal, Numero),
			CONSTRAINT FK_OpCompra_CtSucursal 
			FOREIGN KEY (IdCtSucursal) REFERENCES CtSucursal(Id),
			CONSTRAINT FK_OpCompra_CtProveedor 
			FOREIGN KEY (IdCtProveedor) REFERENCES CtProveedor(Id)
		);
	END

	-- ===============================================
	-- OpCompraDetalle
	-- ===============================================
	BEGIN
		CREATE TABLE OpCompraDetalle
		(
			Id							INT             NOT NULL IDENTITY,
			IdOpCompra					INT             NOT NULL,
			-- Identificación de la presentación del producto (La presentación exacta con la que se compró)
			Cpp_IdCtProducto			INT				NOT NULL,	-- CtPresentacionProducto.IdCtProducto
			Cpp_Nombre					VARCHAR(50)		NOT NULL,	-- CtPresentacionProducto.Nombre
			Cpp_FactorUBI				DECIMAL(18,5)	NOT NULL,	-- CtPresentacionProducto.FactorUBI
			-- Cantidades
			Cantidad					DECIMAL(18,5)   NOT NULL,   -- Cantidad comprada en la presentación
			-- Costos
			CostoUP						DECIMAL(18,5)   NOT NULL,   -- Costo por unidad de presentación
			CostoUBI					DECIMAL(18,5)   NOT NULL,   -- Costo por unidad base de inventario
			Importe						DECIMAL(18,5)   NOT NULL,
			-- Lote generado por esta compra
			IdOpLote					INT             NOT NULL,
			CONSTRAINT PK_OpCompraDetalle PRIMARY KEY (Id),
			CONSTRAINT FK_OpCompraDetalle_OpCompra 
			FOREIGN KEY (IdOpCompra) REFERENCES OpCompra(Id),
			CONSTRAINT FK_OpCompraDetalle_CtProducto 
			FOREIGN KEY (Cpp_IdCtProducto) REFERENCES CtProducto(Id),
			-- FK OpLote EN CREATE OpLote
		);
	END

	PRINT 'Termina: MÓDULO: COMPRAS'
	PRINT '===================================================='
END 


-- #################################################### --
-- MÓDULO: INVENTARIOS
-- #################################################### --
BEGIN
	PRINT '===================================================='
	PRINT 'Inicia: MÓDULO: INVENTARIOS'

	-- ============================================
	-- CtTipoLote
	-- ============================================
	BEGIN
		-- DROP TABLE CtTipoLote
		CREATE TABLE CtTipoLote
		(
			Id		TINYINT			NOT NULL,
			Nombre	VARCHAR(25)		NOT NULL,
			CONSTRAINT PK_CtTipoLote PRIMARY KEY (Id),
			CONSTRAINT UK_CtTipoLote UNIQUE (Nombre)
		);

	END

	BEGIN
		INSERT INTO CtTipoLote (Id, Nombre) 
		VALUES
			(1, 'INICIAL'),
			(2, 'COMPRA'),
			(3, 'TRASPASO');
	END	

	-- ============================================
	-- OpLote
	-- ============================================
	BEGIN
		--DROP TABLE OpLote
		CREATE TABLE OpLote
		(
			Id							INT             NOT NULL IDENTITY,
			-- Ubicación del lote
			IdCtSucursal				TINYINT			NOT NULL,
			Numero						INT				NOT NULL,
			IdCtTipoLote				TINYINT			NOT NULL,
			-- Identificación de la presentación del producto (La presentación exacta con la que se compró o generó el lote.)
			Cpp_IdCtProducto			INT				NOT NULL,	-- CtPresentacionProducto.IdCtProducto
			Cpp_Nombre					VARCHAR(50)		NOT NULL,	-- CtPresentacionProducto.Nombre
			Cpp_FactorUBI				DECIMAL(18,5)	NOT NULL,	-- CtPresentacionProducto.FactorUBI
			-- Cantidades en UBIs
			CantidadInicialUBI			DECIMAL(18,5)   NOT NULL,   -- En unidad base de Inventario (cantidad ingresada × FactorUBI)
			CantidadDisponibleUBI		DECIMAL(18,5)   NOT NULL,   -- En unidad base de Inventario
			-- Costos
			CostoUP						DECIMAL(18,5)   NOT NULL,   -- Costo por unidad de presentación
			CostoUBI					DECIMAL(18,5)   NOT NULL,   -- Costo por unidad base de inventario
			-- Información del lote
			Fecha						DATETIME        NOT NULL,
			CONSTRAINT PK_OpLote PRIMARY KEY (Id),
			CONSTRAINT UK_OpLote UNIQUE (IdCtSucursal, Numero),
			CONSTRAINT FK_OpLote_CtSucursal 
			FOREIGN KEY (IdCtSucursal) REFERENCES CtSucursal(Id),
			CONSTRAINT FK_OpLote_CtTipoLote 
			FOREIGN KEY (IdCtTipoLote) REFERENCES CtTipoLote(Id),
			CONSTRAINT FK_OpLote_CtProducto 
			FOREIGN KEY (Cpp_IdCtProducto) REFERENCES CtProducto(Id),
		);

		ALTER TABLE OpCompraDetalle
		ADD CONSTRAINT FK_OpCompraDetalle_OpLote
		FOREIGN KEY (IdOpLote) REFERENCES OpLote(Id)

	END


	-- ====================================================
	-- CtTipoMovimiento
	-- ====================================================
	BEGIN
		--DROP TABLE CtTipoMovimiento
		CREATE TABLE CtTipoMovimiento
		(
			Id		TINYINT		NOT NULL,
			Tipo	TINYINT		NOT NULL,	-- 1=Entrada, 2=Salida
			Nombre	VARCHAR(50)	NOT NULL,
			CONSTRAINT PK_CtTipoMovimiento PRIMARY KEY (Id),
			CONSTRAINT UK_CtTipoMovimiento UNIQUE (Tipo, Nombre),
		);

	END

	BEGIN
		INSERT INTO CtTipoMovimiento (Id, Tipo, Nombre) VALUES
		(1, 1, 'COMPRA'),
		(2, 2, 'VENTA'),
		(3, 1, 'REINGRESO'),
		(4, 2, 'TRASPASO DE SALIDA'),
		(5, 1, 'TRASPASO DE ENTRADA'),
		(6, 1, 'AJUSTE POSITIVO'),
		(7, 2, 'AJUSTE NEGATIVO');
	END

	-- ====================================================
	-- OpMovimientoInventario										
	-- ====================================================
	BEGIN
		--DROP TABLE OpMovimientoInventario
		CREATE TABLE OpMovimientoInventario 
		(
			Id						INT             NOT NULL IDENTITY,
			-- Ubicación y producto
			IdCtSucursal			TINYINT			NOT NULL,
			Numero					INT				NOT NULL,
			-- Identificación de la presentación del producto (La presentación exacta con la que se movió)
			Cpp_IdCtProducto		INT				NOT NULL,	-- CtPresentacionProducto.IdCtProducto
			Cpp_Nombre				VARCHAR(50)		NOT NULL,	-- CtPresentacionProducto.Nombre
			Cpp_FactorUBI			DECIMAL(18,5)	NOT NULL,	-- CtPresentacionProducto.FactorUBI
			-- Cantidades
			CantidadUBI				DECIMAL(18,5)   NOT NULL,	-- Cantidad en Unidad Base de Inventario
			-- Momento exacto del movimiento
			Fecha					DATETIME        NOT NULL,
			-- Tipo de movimiento
			IdCtTipoMovimiento		TINYINT         NOT NULL,
				-- Ejemplos:
				-- 1 = COMPRA
				-- 2 = VENTA
				-- 3 = REINGRESO
				-- 4 = TRASPASO_SALIDA
				-- 5 = TRASPASO_ENTRADA
				-- 6 = AJUSTE_POSITIVO
				-- 7 = AJUSTE_NEGATIVO
			-- Relación con operaciones
			IdOpPedidoDetalleEvento INT				NULL,   -- ventas (evento SURTIDO) y reingresos (evento RECHAZADO); 1 evento -> N movimientos (uno por lote)
			IdOpCompraDetalle       INT             NULL,   -- solo para COMPRA
			IdOpTraspasoDetalle     INT             NULL,   -- solo para TRASPASO
			-- Relación con lote
			IdOpLote                INT             NOT NULL,   -- lote del que sale o al que entra
			-- Costos (solo para salidas)
			CostoUBI				DECIMAL(18,5)   NULL,
			CostoTotal              DECIMAL(18,5)   NULL,
			-- Auditoría
			IdCtUsuario             SMALLINT        NOT NULL,
			CONSTRAINT PK_OpMovimientoInventario PRIMARY KEY (Id),
			CONSTRAINT UK_OpMovimientoInventario UNIQUE (IdCtSucursal, Numero),
			CONSTRAINT FK_OpMovimientoInventario_CtSucursal
			FOREIGN KEY (IdCtSucursal) REFERENCES CtSucursal(Id),
			CONSTRAINT FK_OpMovimientoInventario_CtProducto
			FOREIGN KEY (Cpp_IdCtProducto) REFERENCES CtProducto(Id),
			CONSTRAINT FK_OpMovimientoInventario_CtTipoMovimiento
			FOREIGN KEY (IdCtTipoMovimiento) REFERENCES CtTipoMovimiento(Id),
			--CONSTRAINT FK_OpMovimientoInventario_OpPedidoDetalleEvento
			--FOREIGN KEY (IdOpPedidoDetalleEvento) REFERENCES OpPedidoDetalleEvento(Id),
			CONSTRAINT FK_OpMovimientoInventario_OpCompraDetalle
			FOREIGN KEY (IdOpCompraDetalle) REFERENCES OpCompraDetalle(Id),
			--CONSTRAINT FK_OpMovimientoInventario_OpTraspasoDetalle
			--FOREIGN KEY (IdOpTraspasoDetalle) REFERENCES OpTraspasoDetalle(Id),
			CONSTRAINT FK_OpMovimientoInventario_OpLote
			FOREIGN KEY (IdOpLote) REFERENCES OpLote(Id),
			CONSTRAINT FK_OpMovimientoInventario_CtUsuario
			FOREIGN KEY (IdCtUsuario) REFERENCES CtUsuario(Id)
		);
	END
	
	-- ====================================================
	-- OpTraspaso
	-- ====================================================
	BEGIN
		--DELETE FROM OpTraspaso
		CREATE TABLE OpTraspaso
		(
			Id                  INT         NOT NULL IDENTITY,
			IdCtSucursalOrigen  TINYINT     NOT NULL,
			IdCtSucursalDestino TINYINT		NOT NULL,
			Fecha               DATETIME    NOT NULL,
			IdCtUsuario         SMALLINT			NOT NULL,
			CONSTRAINT PK_OpTraspaso PRIMARY KEY (Id),
			CONSTRAINT FK_OpTraspaso_CtSucursal_Origen
			FOREIGN KEY (IdCtSucursalOrigen) REFERENCES CtSucursal(Id),
			CONSTRAINT FK_OpTraspaso_CtSucursal_Destino
			FOREIGN KEY (IdCtSucursalDestino) REFERENCES CtSucursal(Id),
			CONSTRAINT FK_OpTraspaso_CtUsuario
			FOREIGN KEY (IdCtUsuario) REFERENCES CtUsuario(Id)
		);

	END

	-- ====================================================
	-- OpTraspasoDetalle
	-- ====================================================
	BEGIN
		--DELETE FROM OpTraspasoDetalle
		CREATE TABLE OpTraspasoDetalle
		(
			Id									INT             NOT NULL IDENTITY,
			IdOpTraspaso						INT             NOT NULL,
			-- Identificación de la presentación del producto (La presentación exacta con la que se traspasó)
			Cpp_IdCtProducto					INT				NOT NULL,	-- CtPresentacionProducto.IdCtProducto
			Cpp_Nombre							VARCHAR(50)		NOT NULL,	-- CtPresentacionProducto.Nombre
			Cpp_FactorUBI						DECIMAL(18,5)	NOT NULL,	-- CtPresentacionProducto.FactorUBI
			-- Cantidad
			Cantidad							DECIMAL(18,5)   NOT NULL,	-- Cantidad traspasada en la presentación
			-- Este campo permite ligar el lote creado en destino
			IdOpLote_Destino					INT             NULL,
			CONSTRAINT PK_OpTraspasoDetalle PRIMARY KEY (Id),
			CONSTRAINT FK_OpTraspasoDetalle_OpTraspaso
			FOREIGN KEY (IdOpTraspaso) REFERENCES OpTraspaso(Id),
			CONSTRAINT FK_OpTraspasoDetalle_CtProducto
			FOREIGN KEY (Cpp_IdCtProducto) REFERENCES CtProducto(Id),
			CONSTRAINT FK_OpTraspasoDetalle_LoteDestino
			FOREIGN KEY (IdOpLote_Destino) REFERENCES OpLote(Id)
		);

		ALTER TABLE OpMovimientoInventario
		ADD CONSTRAINT FK_OpMovimientoInventario_OpTraspasoDetalle
		FOREIGN KEY (IdOpTraspasoDetalle) REFERENCES OpTraspasoDetalle(Id);

	END

	PRINT 'Termina: MÓDULO: INVENTARIOS'
	PRINT '===================================================='
END 


-- #################################################### --
-- MÓDULO: VENTAS
-- #################################################### --
BEGIN
	PRINT '===================================================='
	PRINT 'Inicia: MÓDULO: VENTAS'

	-- ====================================================
	-- CTRegimenFiscal
	-- ====================================================

	BEGIN
	    -- DROP TABLE CtRegimenFiscal
		CREATE TABLE CtRegimenFiscal
		(
		    Id        SMALLINT			NOT NULL,
			Nombre    VARCHAR(100)      NOT NULL,
			Fisica    BIT               NOT NULL,
			Moral     BIT               NOT NULL,
			CONSTRAINT PK_CtRegimenFiscal PRIMARY KEY (Id),
			CONSTRAINT UK_CtRegimenFiscal UNIQUE (Nombre)
		);
	END 

	BEGIN 
		INSERT INTO CtRegimenFiscal (Id, Nombre, Fisica, Moral) VALUES 
		(601, 'GENERAL DE LEY PERSONAS MORALES', 0, 1),
		(603, 'PERSONAS MORALES CON FINES NO LUCRATIVOS', 0, 1),
		(605, 'SUELDOS Y SALARIOS E INGRESOS ASIMILADOS A SALARIOS', 1, 0),
		(606, 'ARRENDAMIENTO', 1, 0),
		(607, 'RÉGIMEN DE ENAJENACIÓN O ADQUISICIÓN DE BIENES', 1, 0),
		(608, 'DEMÁS INGRESOS', 1, 0),
		(610, 'RESIDENTES EN EL EXTRANJERO SIN ESTABLECIMIENTO PERMANENTE EN MÉXICO', 1, 1),
		(611, 'INGRESOS POR DIVIDENDOS (SOCIOS Y ACCIONISTAS)', 1, 0),
		(612, 'PERSONAS FÍSICAS CON ACTIVIDADES EMPRESARIALES Y PROFESIONALES', 1, 0),
		(614, 'INGRESOS POR INTERESES', 1, 0),
		(615, 'RÉGIMEN DE LOS INGRESOS POR OBTENCIÓN DE PREMIOS', 1, 0),
		(616, 'SIN OBLIGACIONES FISCALES', 1, 0),
		(620, 'SOCIEDADES COOPERATIVAS DE PRODUCCIÓN QUE OPTAN POR DIFERIR SUS INGRESOS', 0, 1),
		(621, 'INCORPORACIÓN FISCAL', 1, 0),
		(622, 'ACTIVIDADES AGRÍCOLAS, GANADERAS, SILVÍCOLAS Y PESQUERAS', 0, 1),
		(623, 'OPCIONAL PARA GRUPOS DE SOCIEDADES', 0, 1),
		(624, 'COORDINADOS', 0, 1),
		(625, 'RÉGIMEN DE LAS ACTIVIDADES EMPRESARIALES CON INGRESOS A TRAVÉS DE PLATAFORMAS TECNOLÓGICAS', 1, 0),
		(626, 'RÉGIMEN SIMPLIFICADO DE CONFIANZA', 1, 1);		
	END

	-- ====================================================
	-- CtCliente									
	-- ====================================================
	BEGIN
		-- DROP TABLE CtCliente
		CREATE TABLE CtCliente
		(
			Id						INT				NOT NULL IDENTITY,
			Nombre					VARCHAR(50)		NOT NULL,
			Alias					VARCHAR(25)		NULL,
			Direccion				VARCHAR(100)	NOT NULL,
			Telefono				CHAR(10)	   	NOT NULL,
			RFCFiscal				VARCHAR(13)		NULL,
			IdCtRegimenFiscal	 	SMALLINT		NULL,
			CalleFiscal				VARCHAR(100)	NULL,
			NumeroExteriorFiscal	VARCHAR(25)		NULL,
			NumeroInteriorFiscal	VARCHAR(25)		NULL,
			CodigoPostalFiscal		CHAR(5)			NULL,
			ColoniaFiscal			VARCHAR(50)		NULL,
			LocalidadFiscal			VARCHAR(50)		NULL,
			MunicipioFiscal			VARCHAR(50)		NULL,
			EntidadFederativaFiscal	VARCHAR(50)		NULL,
			SaldoMonedero			DECIMAL(18,5)	NOT NULL DEFAULT 0,
			LimiteCreditoSemanal	DECIMAL(18,2)	NOT NULL DEFAULT 0,
			CONSTRAINT PK_CtCliente PRIMARY KEY (Id),
			CONSTRAINT UK_CtCliente UNIQUE (Nombre,Telefono),
			CONSTRAINT FK_CtCliente_CtRegimenFiscal FOREIGN KEY (IdCtRegimenFiscal) REFERENCES CtRegimenFiscal (Id)
		);
	END

	BEGIN
		INSERT INTO CtCliente 
		( Nombre, Alias, Direccion, Telefono, RFCFiscal, IdCtRegimenFiscal, CalleFiscal, NumeroExteriorFiscal, NumeroInteriorFiscal, CodigoPostalFiscal, ColoniaFiscal, LocalidadFiscal, MunicipioFiscal, EntidadFederativaFiscal, SaldoMonedero) VALUES
		('MOSTRADOR',NULL,'NO DEFINIDO','0000000000','XAXX010101000', 616,'NO DEFINIDO','S/N','', '00000','CENTRO','ACATZINGO','ACATZINGO','PUEBLA', 0);
	END

	-- ====================================================
	-- CtCanalCaptura									
	-- ====================================================
	BEGIN
		-- DROP TABLE CtCanalCaptura
		CREATE TABLE CtCanalCaptura
		(
			Id		TINYINT		NOT NULL,
			Nombre	VARCHAR(25)	NOT NULL,
			CONSTRAINT PK_CtCanalCaptura PRIMARY KEY (Id),
			CONSTRAINT UK_CtCanalCaptura UNIQUE (Nombre)
		);
	END

	BEGIN
		INSERT INTO CtCanalCaptura (Id, Nombre) VALUES
		(1, 'PRESENCIAL'),
		(2, 'TELÉFONO');
	END

	-- ====================================================
	-- CtEstatusPedido							
	-- ====================================================
	BEGIN
		-- DROP TABLE CtEstatusPedido
		CREATE TABLE CtEstatusPedido
		(
			Id			TINYINT			NOT NULL,
			Nombre		VARCHAR(25)		NOT NULL,
			Descripcion	VARCHAR(300)	NOT NULL,
			CONSTRAINT PK_CtEstatusPedido PRIMARY KEY (Id),
			CONSTRAINT UK_CtEstatusPedido UNIQUE (Nombre)
		)
	END

	BEGIN
		INSERT INTO CtEstatusPedido (Id, Nombre, Descripcion) VALUES
			(1, 'CREADO', 'Pedido recién capturado; ninguna línea se ha empezado a surtir. Todo pedido pasa por aquí, incluso los que terminarán en Apartado; el cobro es una operación separada.'),
			(2, 'APARTADO', 'Pedido pagado al 100% al momento de la captura; el cliente lo recogerá o recibirá posteriormente. No se valida ni descuenta inventario mientras permanezca en este estatus. Exclusivo para clientes registrados.'),
			(3, 'EN PROCESO', 'Al menos una línea ya tiene cantidad surtida y el pedido aún no se cierra. Agrupa las etapas de surtido y entrega; el detalle de cada línea está en sus estatus y eventos.'),
			(4, 'ENTREGADO', 'El 100% de las líneas del pedido fue entregado completamente al cliente.'),
			(5, 'ENTREGADO PARCIAL', 'El pedido se cerró con al menos una línea no entregada en su totalidad, ya sea por rechazo del cliente o por cierre administrativo de productos no surtidos.'),
			(6, 'RECHAZADO', 'El cliente rechazó la totalidad del pedido o ninguna línea fue entregada antes del cierre.'),
			(7, 'CANCELADO', 'El pedido fue cancelado antes de que cualquier línea fuera surtida.');
	END

	-- ====================================================
	-- OpPedido									
	-- ====================================================
	BEGIN
		-- DROP TABLE OpPedido
		CREATE TABLE OpPedido 
		(
			Id							INT				NOT NULL IDENTITY,
			-- Ubicación y cliente
			IdCtSucursal				TINYINT			NOT NULL,
			Numero						INT				NOT NULL,
			IdCtCliente					INT				NOT NULL,
			-- Datos del cliente mostrador
			NombreClienteMostrador		VARCHAR(50)		NULL,
			TelefonoClienteMostrador	VARCHAR(10)		NULL,
			-- Datos de captura
			IdCtCanalCaptura			TINYINT			NOT NULL,       
			IdCtEstatusPedido			TINYINT			NOT NULL, 
			-- Entrega
			EntregaDomicilio			BIT				NOT NULL,
			DireccionEntrega			VARCHAR(250)	NULL,
			FechaEntregaEstimada		DATETIME		NULL,
			Notas						VARCHAR(250)	NOT NULL, --Notas
			-- Entrega a repartidor
			IdCtUsuarioRepartidor		SMALLINT		NULL,
			-- Totales
			Total						DECIMAL(18,5)	NOT NULL,
			TotalPagado					DECIMAL(18,5)	NOT NULL,
			CONSTRAINT PK_OpPedido PRIMARY KEY (Id),
			CONSTRAINT UK_OpPedido UNIQUE (IdCtSucursal, Numero),
			CONSTRAINT FK_OpPedido_CtSucursal 
			FOREIGN KEY (IdCtSucursal) REFERENCES CtSucursal (Id),
			CONSTRAINT FK_OpPedido_CtCliente 
			FOREIGN KEY (IdCtCliente) REFERENCES CtCliente (Id),
			CONSTRAINT FK_OpPedido_CtCanalCaptura 
			FOREIGN KEY (IdCtCanalCaptura) REFERENCES CtCanalCaptura (Id),
			CONSTRAINT FK_OpPedido_CtEstatusPedido 
			FOREIGN KEY (IdCtEstatusPedido) REFERENCES CtEstatusPedido (Id),
			CONSTRAINT FK_OpPedido_CtUsuarioRepartidor 
			FOREIGN KEY (IdCtUsuarioRepartidor) REFERENCES CtUsuario (Id),
		);
	END
	
	-- ====================================================
	-- CtEstatusPedidoDetalle							
	-- ====================================================
	BEGIN
		CREATE TABLE CtEstatusPedidoDetalle
		(
			Id						TINYINT			NOT NULL,
			Nombre					VARCHAR(30)		NOT NULL,
			Descripcion				VARCHAR(300)	NOT NULL,
			CONSTRAINT PK_CtEstatusPedidoDetalle PRIMARY KEY (Id),
			CONSTRAINT UK_CtEstatusPedidoDetalle UNIQUE (Nombre)
		)
	END

	BEGIN
		INSERT INTO CtEstatusPedidoDetalle (Id, Nombre, Descripcion) VALUES
		(1, 'PENDIENTE', 'Línea guardada sin ninguna cantidad surtida. Puede tener cantidad apartada o no tenerla (por ejemplo, BajoPedido sin existencia suficiente). Si se entrega en mostrador al guardar el pedido, la línea nace directamente en CERRADO.'),
		(2, 'EN PROCESO', 'La línea ya tiene cantidad surtida y aún queda cantidad por resolver. El detalle de cada surtido, entrega y rechazo se registra en OpPedidoDetalleEvento.'),
		(3, 'CERRADO', 'La línea ya no tiene cantidad pendiente: toda su Cantidad quedó distribuida entre CantidadEntregada, CantidadRechazada y CantidadCancelada, con al menos un surtido. No indica cómo terminó; eso lo dicen las cantidades.'),
		(4, 'CANCELADO', 'La línea se canceló completa antes de surtir cualquier cantidad; no hubo movimiento de inventario. Si ya se había surtido algo y se cancela el remanente, la línea queda CERRADO.');
	END

	-- ====================================================
	-- OpPedidoDetalle									
	-- ====================================================
	BEGIN
		CREATE TABLE OpPedidoDetalle 
		(
			Id									INT				NOT NULL IDENTITY,
			IdOpPedido							INT				NOT NULL,
			IdCtEstatusPedidoDetalle			TINYINT			NOT NULL,
			 -- Producto y presentación usados en el momento de la venta
			Cpp_IdCtProducto					INT				NOT NULL,	-- CtPresentacionProducto.IdCtProducto
			Cpp_Nombre							VARCHAR(50)		NOT NULL,	-- CtPresentacionProducto.Nombre
			Cpp_FactorUBI						DECIMAL(18,5)	NOT NULL,	-- CtPresentacionProducto.FactorUBI
			 -- Cantidades (en la presentación)
			Cantidad							DECIMAL(18,5)	NOT NULL,	-- Cantidad vendida en la presentación
			-- Acumulados: caché de OpPedidoDetalleEvento, se actualizan en la misma transacción que el evento
			CantidadSurtida						DECIMAL(18,5)	NOT NULL,	-- Suma de eventos SURTIDO 
			CantidadEntregada					DECIMAL(18,5)	NOT NULL,		-- Cantidad entregada en la presentación
			CantidadRechazada					DECIMAL(18,5)	NOT NULL,		-- Cantidad rechazada en la presentación
			CantidadCancelada					DECIMAL(18,5)	NOT NULL,	-- Suma de enventos CANCELADO
			 -- Precios
			Precio								DECIMAL(18,5)	NOT NULL,
			Importe								DECIMAL(18,5)	NOT NULL,
			-- Relación con inventario (SOLO la salida principal)
			IdOpMovimientoInventario_Salida     INT             NULL,   -- salida principal
			IdOpMovimientoInventario_Reingreso  INT             NULL,   -- si hay rechazo
			CONSTRAINT PK_OpPedidoDetalle PRIMARY KEY (Id),
			CONSTRAINT FK_OpPedidoDetalle_OpPedido 
			FOREIGN KEY (IdOpPedido) REFERENCES OpPedido (Id),
			CONSTRAINT FK_OpPedidoDetalle_CtEstatusPedidoDetalle
			FOREIGN KEY (IdCtEstatusPedidoDetalle) REFERENCES CtEstatusPedidoDetalle(Id),
			CONSTRAINT FK_OpPedidoDetalle_CtProducto 
			FOREIGN KEY (Cpp_IdCtProducto) REFERENCES CtProducto (Id),
		)

	END

	-- ====================================================
	-- CtTipoEventoPedidoDetalle						
	-- ====================================================

	BEGIN
		CREATE TABLE CtTipoEventoPedidoDetalle
		(
			Id			TINYINT			NOT NULL,
			Nombre		VARCHAR(30)     NOT NULL,
			Descripcion	VARCHAR(300)	NOT NULL,
			CONSTRAINT PK_CtTipoEventoPedidoDetalle PRIMARY KEY (Id)
		);

	END

	BEGIN
		INSERT INTO CtTipoEventoPedidoDetalle (Id, Nombre, Descripcion) VALUES
			(1, 'RESERVADO', 'Se aparta cantidad de la línea para este pedido, para que otro pedido no la considere disponible. No descuenta el lote ni genera movimiento. No se registra cuando no hay existencia que apartar (por ejemplo, BajoPedido sin existencia suficiente).'),
			(2, 'SURTIDO', 'El producto sale del inventario para ser entregado: el almacenista o el vendedor lo pasa al repartidor (domicilio) o directo al cliente (recoge en tienda). Descuenta el lote, consume la cantidad apartada si la hay, y puede registrarse varias veces por línea (surtido parcial).'),
			(3, 'ENTREGADO', 'El cliente recibió en sus manos una cantidad (parcial o total) de la línea. Lo registra quien se la dio: vendedor, almacenista o repartidor. No genera movimiento de inventario; la salida ya ocurrió en SURTIDO.'),
			(4, 'RECHAZADO', 'El cliente no aceptó una cantidad ya surtida y esta regresa físicamente al almacén o al mostrador. Genera movimiento de reingreso al lote. Solo se permite si el pedido no tiene ningún abono (TotalPagado = 0).'),
			(5, 'CANCELADO', 'Se cancela una cantidad de la línea que no se ha surtido (toda la línea o su remanente). Libera la cantidad apartada, si la había. No genera movimiento de inventario.');
	END

	-- ====================================================
	-- OpPedidoDetalleEvento								
	-- ====================================================
	-- Bitácora append-only: un renglón por cada acción sobre una línea.
	--   RESERVADO : aparta cantidad (sube StockReservadoUBI), sin movimiento de inventario
	--   SURTIDO   : sale del inventario (1 movimiento de salida por cada lote usado); IdCtUsuarioRecibe = repartidor, o NULL si lo recibe el cliente directo
	--   ENTREGADO : el cliente lo recibe en sus manos, sin movimiento de inventario
	--   RECHAZADO : reingreso al lote (1 movimiento de entrada por cada lote al que regresa)
	--   CANCELADO : cancela cantidad sin surtir y libera la reserva, sin movimiento de inventario
	BEGIN
		CREATE TABLE OpPedidoDetalleEvento
		(
			Id								INT				NOT NULL IDENTITY,
			IdOpPedidoDetalle				INT				NOT NULL,
			IdCtTipoEventoPedidoDetalle		TINYINT			NOT NULL,
			Cantidad						DECIMAL(18,5)	NOT NULL,	-- Cantidad de ESTE evento, no el acumulado
			Fecha							DATETIME		NOT NULL,
			IdCtUsuario						SMALLINT		NOT NULL,	-- Quien ejecuta la acción
			IdCtUsuarioRecibe				SMALLINT		NULL,		-- Solo SURTIDO: repartidor que recibe; NULL = cliente directo
			Notas							VARCHAR(300)	NULL,
			CONSTRAINT PK_OpPedidoDetalleEvento PRIMARY KEY (Id),
			CONSTRAINT FK_OpPedidoDetalleEvento_OpPedidoDetalle
			FOREIGN KEY (IdOpPedidoDetalle) REFERENCES OpPedidoDetalle(Id),
			CONSTRAINT FK_OpPedidoDetalleEvento_CtTipoEventoPedidoDetalle
			FOREIGN KEY (IdCtTipoEventoPedidoDetalle) REFERENCES CtTipoEventoPedidoDetalle(Id),
			CONSTRAINT FK_OpPedidoDetalleEvento_CtUsuario
			FOREIGN KEY (IdCtUsuario) REFERENCES CtUsuario(Id),
			CONSTRAINT FK_OpPedidoDetalleEvento_CtUsuarioRecibe
			FOREIGN KEY (IdCtUsuarioRecibe) REFERENCES CtUsuario (Id)
		);
		
		-- Ahora sí se puede crear el FK del movimiento hacia el evento
		ALTER TABLE OpMovimientoInventario
		ADD CONSTRAINT FK_OpMovimientoInventario_OpPedidoDetalleEvento
		FOREIGN KEY (IdOpPedidoDetalleEvento) REFERENCES OpPedidoDetalleEvento (Id);

	END

	-- ====================================================
	-- OpPedidoEstatus									
	-- ====================================================
	BEGIN
		CREATE TABLE OpPedidoEstatus
		(
			Id                  INT             NOT NULL IDENTITY,
			IdOpPedido          INT             NOT NULL,
			IdCtEstatusPedido   TINYINT         NOT NULL,
			Fecha               DATETIME        NOT NULL,
			IdCtUsuario         SMALLINT        NOT NULL,
			Notas               VARCHAR(250)    NOT NULL,
			CONSTRAINT PK_OpPedidoEstatus PRIMARY KEY (Id),
			CONSTRAINT UK_OpPedidoEstatus UNIQUE (IdOpPedido, IdCtEstatusPedido),
			CONSTRAINT FK_OpPedidoEstatus_OpPedido
				FOREIGN KEY (IdOpPedido) REFERENCES OpPedido (Id),
			CONSTRAINT FK_OpPedidoEstatus_CtEstatusPedido
				FOREIGN KEY (IdCtEstatusPedido) REFERENCES CtEstatusPedido (Id),
			CONSTRAINT FK_OpPedidoEstatus_CtUsuario
				FOREIGN KEY (IdCtUsuario) REFERENCES CtUsuario (Id)
		)
	END

	-- ====================================================
	-- CtConfiguracionMonedero								
	-- ====================================================
	BEGIN
		CREATE TABLE CtConfiguracionMonedero
		(
			Id							INT					NOT NULL,
			PorcentajeAcumulacion		DECIMAL(6,5)		NOT NULL,
			CostoPremio					DECIMAL(18,2)		NOT NULL,
			NombrePremio				VARCHAR(100)		NOT NULL,
			Activo						BIT					NOT NULL DEFAULT 1,
			CONSTRAINT PK_CtConfiguracionMondero PRIMARY KEY (Id)
		)
	END
	
	-- ====================================================
	-- BtMonenederoMovimiento						
	-- ====================================================
	BEGIN
		CREATE TABLE BtMonederoMovimiento
		(
			Id					INT					NOT NULL IDENTITY,
			IdCtCliente			INT					NOT NULL,
			IdOpPedido			INT					NULL,
			Tipo				TINYINT				NOT NULL, -- 1 = Acumulación, 2 = Canje
			Monto				DECIMAL(18,5)		NOT NULL,
			SaldoRestante		DECIMAL(18,5)		NOT NULL,
			Fecha				DATETIME			NOT NULL,
			CONSTRAINT PK_BtMonederoMovimiento PRIMARY KEY (Id),
			CONSTRAINT FK_BtMonederoMovimiento_CtCliente
			FOREIGN KEY (IdCtCliente) REFERENCES CtCliente (Id),
			CONSTRAINT FK_BtMonederoMovimiento_OpPedido
			FOREIGN KEY (IdOpPedido) REFERENCES OpPedido (Id)		
		)
	END 

	PRINT 'Termina: MÓDULO: VENTAS'
	PRINT '===================================================='
END 


-- #################################################### --
-- MÓDULO: FINANZAS
-- #################################################### --
BEGIN
	PRINT '===================================================='
	PRINT 'Inicia: MÓDULO: FINANZAS'
	
	-- ====================================================
	-- CtTipoGasto									
	-- ====================================================
	BEGIN
		-- DROP TABLE CtTipoGasto
		CREATE TABLE CtTipoGasto
		(
			Id		TINYINT		NOT NULL IDENTITY,
			Nombre	VARCHAR(50)	NOT NULL,
			CONSTRAINT PK_CtTipoGasto PRIMARY KEY (Id),
			CONSTRAINT UK_CtTipoGasto UNIQUE (Nombre)
		);
	END

	BEGIN
		INSERT INTO CtTipoGasto (Nombre) VALUES 
			('RENTA DE SUCURSAL'),
			('PAGO DE NÓMINA'),
			('PAGO DE REPARTO A DOMICILIO'),
			('COMBUSTIBLE PARA VEHÍCULOS'),
			('MANTENIMIENTO DE VEHÍCULOS'),
			('COMPRA DE MATERIAL DE OFICINA'),
			('SERVICIOS DE AGUA'),
			('SERVICIOS DE LUZ ELÉCTRICA'),
			('SERVICIOS DE INTERNET'),
			('PUBLICIDAD Y MARKETING'),
			('GASTOS DE COBRANZA'),
			('PAPELERÍA Y FACTURACIÓN'),
			('MANTENIMIENTO DE MAQUINARIA'),
			('MANTENIMIENTO DE MONTACARGAS'),
			('REPARACIÓN DE HERRAMIENTAS'),
			('UNIFORMES Y EQUIPO DE SEGURIDAD'),
			('GASTOS DE LIMPIEZA'),
			('PAGO DE CONTABILIDAD'),
			('PAGO DE SOFTWARE ADMINISTRATIVO'),
			('PAGO DE IMPUESTOS Y DERECHOS'),
			('GASTOS BANCARIOS'),
			('INTERESES POR CRÉDITO'),
			('GASTOS DE MENSAJERÍA'),
			('GASTOS DE VIÁTICOS'),
			('COMPRA DE EMPAQUES Y BOLSAS'),
			('GASTOS POR DEVOLUCIONES'),
			('GASTOS POR MERMA'),
			('CAPACITACIÓN DE PERSONAL'),
			('SEGUROS DE LOCAL'),
			('SEGUROS DE VEHÍCULOS');
	END


	-- ====================================================
	-- CtMedioPago									
	-- ====================================================
	BEGIN
		-- DROP TABLE CtMedioPago
		CREATE TABLE CtMedioPago
		(
			Id				TINYINT			NOT NULL,
			Nombre			VARCHAR(50)		NOT NULL,
			CONSTRAINT PK_CtMedioPago PRIMARY KEY (Id),
			CONSTRAINT UK_CtMedioPago UNIQUE (Nombre)
		);
	END

	BEGIN
		INSERT INTO CtMedioPago (Id, Nombre) VALUES 
		(1, 'EFECTIVO'),
		(2, 'TRANSFERENCIA'),
		(3, 'TARJETA DE DÉBITO'),
		(4, 'TARJETA DE CRÉDITO'),
		(5, 'MERCADOPAGO');
	END

	-- ====================================================
	-- CtCuentaBancaria								
	-- ====================================================
	BEGIN
		--DROP TABLE CtCuentaBancaria
		CREATE TABLE CtCuentaBancaria
		(
			Id				TINYINT			NOT NULL,
			Nombre			VARCHAR(50)		NOT NULL,
			Banco			VARCHAR(50)		NOT NULL,
			NumeroCuenta	VARCHAR(20)		NOT NULL,
			Activa			BIT				NOT NULL DEFAULT 1,
			CONSTRAINT PK_CtCuentaBancaria PRIMARY KEY (Id),
			CONSTRAINT UK_CtCuentaBancaria UNIQUE (NumeroCuenta)
		);
	END

	-- CtCuentaBancaria: datos de cuentas reales van en CRG_Matvi.sql

	-- ====================================================
	-- OpCorteCaja								
	-- ====================================================
	BEGIN
		--DROP TABLE OpCorteCaja
		CREATE TABLE OpCorteCaja
		(
			Id							INT				NOT NULL IDENTITY,
			IdCtSucursal				TINYINT			NOT NULL,
			Numero						INT				NOT NULL,
			IdCtUsuario					SMALLINT		NOT NULL,
			FechaApertura				DATETIME		NOT NULL,
			FechaCierre					DATETIME		NULL,

			FondoInicial				DECIMAL(18,2)	NOT NULL,
			TotalCobros					DECIMAL(18,2)	NULL,
			TotalCobrosEfectivo			DECIMAL(18,2)	NULL,
			TotalCobrosBancario			DECIMAL(18,2)	NULL,
			TotalEntregasRepartidor		DECIMAL(18,2)	NULL,
			TotalGastos					DECIMAL(18,2)	NULL,
			TotalGastosEfectivo			DECIMAL(18,2)	NULL,
			TotalGastosBancario			DECIMAL(18,2)	NULL,
			TotalPagos					DECIMAL(18,2)	NULL,
			TotalPagosEfectivo			DECIMAL(18,2)	NULL,
			TotalPagosBancario			DECIMAL(18,2)	NULL,
			FondoFinalSistema			DECIMAL(18,2)	NULL,
			FondoFinalContado			DECIMAL(18,2)	NULL,
			Diferencia					DECIMAL(18,2)	NULL,

			TotalVentas					DECIMAL(18,2)	NULL,
			CostoVentasReal				DECIMAL(18,2)	NULL,
			CostoVentasRegularizado		DECIMAL(18,2)	NULL,
			VentasSinCostear			DECIMAL(18,2)	NULL,
			UtilidadBruta				DECIMAL(18,2)	NULL,
			UtilidadNeta				DECIMAL(18,2)	NULL,
			PorcentajeVentasSinCostear	DECIMAL(5,2)	NULL,
			PorcentajeCoberturaGastos	DECIMAL(5,2)	NULL,
			GastosAbsorbidosDeCapital	DECIMAL(18,2)	NULL,

			Notas						VARCHAR(500)	NULL,
			CONSTRAINT PK_OpCorteCaja PRIMARY KEY (Id),
			CONSTRAINT UK_OpCorteCaja UNIQUE (IdCtSucursal, Numero),
			CONSTRAINT FK_OpCorteCaja_CtSucursal
			FOREIGN KEY (IdCtSucursal) REFERENCES CtSucursal(Id),
			CONSTRAINT FK_OpCorteCaja_CtUsuario
			FOREIGN KEY (IdCtUsuario) REFERENCES CtUsuario(Id)
		);
	END

	-- ====================================================
	-- OpGasto							
	-- ====================================================

	BEGIN
        CREATE TABLE OpGasto 
        (
            Id              INT                NOT NULL IDENTITY,
            IdCtSucursal    TINYINT            NOT NULL,
            Numero          INT                NOT NULL,
            FechaPago       DATETIME           NOT NULL,
            IdCtTipoGasto   TINYINT            NOT NULL,
            Proveedor       VARCHAR(100)       NOT NULL,
            Concepto        VARCHAR(100)       NOT NULL,
            Importe         DECIMAL(18,5)      NOT NULL,
            IdCtMedioPago   TINYINT            NOT NULL,
            IdCtUsuario     SMALLINT           NOT NULL,
            IdOpCorteCaja   INT                NULL,
            Notas           VARCHAR(500)       NULL,
            CONSTRAINT PK_OpGasto PRIMARY KEY (Id),
            CONSTRAINT UK_OpGasto UNIQUE (IdCtSucursal, Numero),
			CONSTRAINT FK_OpGasto_CtSucursal 
			FOREIGN KEY (IdCtSucursal) REFERENCES CtSucursal(Id),
            CONSTRAINT FK_OpGasto_CtTipoGasto 
            FOREIGN KEY (IdCtTipoGasto) REFERENCES CtTipoGasto(Id),
            CONSTRAINT FK_OpGasto_CtUsuario 
            FOREIGN KEY (IdCtUsuario) REFERENCES CtUsuario(Id),
            CONSTRAINT FK_OpGasto_CtMedioPago
            FOREIGN KEY (IdCtMedioPago) REFERENCES CtMedioPago(Id),
            CONSTRAINT FK_OpGasto_OpCorteCaja
            FOREIGN KEY (IdOpCorteCaja) REFERENCES OpCorteCaja(Id)
        );
    END

	-- ====================================================
	-- OpCobro									
	-- ====================================================
	BEGIN
		CREATE TABLE OpCobro 
		(
			Id					INT				NOT NULL IDENTITY,
			IdCtSucursal		TINYINT			NOT NULL,
			Numero				INT				NOT NULL,
			Fecha				DATETIME		NOT NULL,
			IdCtCliente			INT				NOT NULL,
			MontoTotal			DECIMAL(18,5)	NOT NULL,
			IdCtMedioPago		TINYINT			NOT NULL,
			IdCtUsuarioRecibe	SMALLINT		NOT NULL,
			Notas				VARCHAR(500)	NULL,
			IdOpCorteCaja		INT				NULL,
			CONSTRAINT PK_OpCobro PRIMARY KEY (Id),
			CONSTRAINT UK_OpCobro UNIQUE (IdCtSucursal, Numero),
			CONSTRAINT FK_OpCobro_CtSucursal
			FOREIGN KEY (IdCtSucursal) REFERENCES CtSucursal(Id),
			CONSTRAINT FK_OpCobro_CtCliente 
			FOREIGN KEY (IdCtCliente) REFERENCES CtCliente (Id),
			CONSTRAINT FK_OpCobro_CtMedioPago 
			FOREIGN KEY (IdCtMedioPago) REFERENCES CtMedioPago (Id),
			CONSTRAINT FK_OpCobro_CtUsuarioRecibe
			FOREIGN KEY (IdCtUsuarioRecibe) REFERENCES CtUsuario(Id),
			CONSTRAINT FK_OpCobro_OpCorteCaja
			FOREIGN KEY (IdOpCorteCaja) REFERENCES OpCorteCaja(Id)

		);
	END

	-- ====================================================
	-- OpCobroDetalle									
	-- ====================================================
	BEGIN
		CREATE TABLE OpCobroDetalle 
		(
			Id				INT				NOT NULL IDENTITY,
			IdOpCobro		INT				NOT NULL,
			IdOpPedido		INT				NOT NULL,
			MontoAplicado	DECIMAL(18,5)	NOT NULL,
			CONSTRAINT PK_OpCobroDetalle PRIMARY KEY (Id),
			CONSTRAINT FK_OpCobroDetalle_OpCobro 
			FOREIGN KEY (IdOpCobro) REFERENCES OpCobro(Id),
			CONSTRAINT FK_OpCobroDetalle_OpPedido 
			FOREIGN KEY (IdOpPedido) REFERENCES OpPedido (Id),
		);
	END

	-- ====================================================
	-- OpPago									
	-- ====================================================
	BEGIN
		-- DROP TABLE OpPago
		CREATE TABLE OpPago 
		(
			Id                  INT				NOT NULL IDENTITY,
			IdCtSucursal		TINYINT			NOT NULL,
			Numero				INT				NOT NULL,
			Fecha				DATETIME		NOT NULL,
			IdCtProveedor		INT				NOT NULL,
			MontoTotal          DECIMAL(18,5)	NOT NULL,
			IdCtMedioPago       TINYINT			NOT NULL,
			IdCtUsuarioPaga     SMALLINT		NOT NULL,
			Notas               VARCHAR(500)	NULL,
			IdOpCorteCaja		INT				NULL,
			CONSTRAINT PK_OpPago PRIMARY KEY (Id),
			CONSTRAINT UK_OpPago UNIQUE (IdCtSucursal, Numero),
			CONSTRAINT FK_OpPago_CtSucursal 
			FOREIGN KEY (IdCtSucursal) REFERENCES CtSucursal(Id),
			CONSTRAINT FK_OpPago_CtProveedor 
			FOREIGN KEY (IdCtProveedor) REFERENCES CtProveedor(Id),
			CONSTRAINT FK_OpPago_CtMedioPago 
			FOREIGN KEY (IdCtMedioPago) REFERENCES CtMedioPago(Id),
			CONSTRAINT FK_OpPago_CtUsuario 
			FOREIGN KEY (IdCtUsuarioPaga) REFERENCES CtUsuario(Id),
			CONSTRAINT FK_OpPago_OpCorteCaja
			FOREIGN KEY (IdOpCorteCaja) REFERENCES OpCorteCaja(Id)
		);
	END
	
	-- ====================================================
	-- OpPagoDetalle									
	-- ====================================================
	BEGIN
		-- DROP TABLE OpPagoDetalle
		CREATE TABLE OpPagoDetalle 
		(
			Id              INT				NOT NULL IDENTITY,
			IdOpPago        INT				NOT NULL,
			IdOpCompra      INT				NOT NULL,
			MontoAplicado   DECIMAL(18,5)	NOT NULL,
			CONSTRAINT PK_OpPagoDetalle PRIMARY KEY (Id),
			CONSTRAINT FK_OpPagoDetalle_OpPago 
			FOREIGN KEY (IdOpPago) REFERENCES OpPago(Id),
			CONSTRAINT FK_OpPagoDetalle_OpCompra 
			FOREIGN KEY (IdOpCompra) REFERENCES OpCompra(Id)
		);

	END

	-- ====================================================
	-- OpMovimientoBancario								
	-- ====================================================
	BEGIN
		--DROP TABLE OpMovimientoBancario
		CREATE TABLE OpMovimientoBancario
		(
			Id					INT				NOT NULL IDENTITY,
			IdCtCuentaBancaria	TINYINT			NOT NULL,
			Fecha				DATETIME		NOT NULL,
			Tipo				TINYINT			NOT NULL,
			Monto				DECIMAL(18,2)	NOT NULL,
			Referencia			VARCHAR(100)	NULL,
			Conciliado			BIT				NOT NULL DEFAULT 0,
			IdOpCobro			INT				NULL,
			IdOpGasto			INT				NULL,
			IdOpPago			INT				NULL,
			CONSTRAINT PK_OpMovimientoBancario PRIMARY KEY (Id),
			CONSTRAINT FK_OpMovimientoBancario_CtCuentaBancaria
			FOREIGN KEY (IdCtCuentaBancaria) REFERENCES CtCuentaBancaria(Id),
			CONSTRAINT FK_OpMovimientoBancario_OpCobro
			FOREIGN KEY (IdOpCobro) REFERENCES OpCobro(Id),
			CONSTRAINT FK_OpMovimientoBancario_OpGasto
			FOREIGN KEY (IdOpGasto) REFERENCES OpGasto(Id),
			CONSTRAINT FK_OpMovimientoBancario_OpPago
			FOREIGN KEY (IdOpPago) REFERENCES OpPago(Id)
		);
	END

	PRINT 'Termina: MÓDULO: FINANZAS'
	PRINT '===================================================='
END 

