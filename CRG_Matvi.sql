DECLARE @EsProd BIT = 0;

-- Detección de ambiente:
-- Producción = Contenedor Linux o servidor 'matvi-sqlserver'
IF SERVERPROPERTY('IsLinux') = 1 
   OR SERVERPROPERTY('MachineName') = 'matvi-sqlserver'
   OR @@SERVERNAME LIKE '%matvi-sqlserver%'
BEGIN
	SET @EsProd = 1;
END
ELSE IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'MatViDB')
BEGIN
	SET @EsProd = 0;
END
ELSE IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'MatviDB_PROD')
BEGIN
	SET @EsProd = 1;
END

IF @EsProd = 1
	USE [MatviDB_PROD];
ELSE
	USE [MatViDB];
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO






DECLARE @EsProduccion BIT = 0;

-- Si deseas omitir los datos demo en producción real, cambia @EsProduccion = 1 arriba.
-- IF DB_NAME() LIKE '%PROD%'
-- BEGIN
-- 	SET @EsProduccion = 1;
-- END


IF @EsProduccion = 1
BEGIN
	PRINT '===================================================='
	PRINT '>> AMBIENTE: PRODUCCIÓN (BD: ' + DB_NAME() + ')'
	PRINT '>> Omitiendo inserción de datos de prueba y demo.'
	PRINT '===================================================='
END
ELSE
BEGIN
	PRINT '===================================================='
	PRINT '>> AMBIENTE: DESARROLLO / LOCAL (BD: ' + DB_NAME() + ')'
	PRINT '>> Cargando datos de prueba y demo...'
	PRINT '===================================================='

-- #################################################### --
-- MÓDULO: ESCENCIALES
-- #################################################### --
BEGIN 
	PRINT '===================================================='
	PRINT 'Inicia: MÓDULO: ESCENCIALES'



	PRINT 'Termina: MÓDULO: ESCENCIALES'
	PRINT '===================================================='
END

-- 2
-- #################################################### --
-- MÓDULO: CONFIGURACIÓN
-- #################################################### --
BEGIN
	PRINT '===================================================='
	PRINT 'Inicia: MÓDULO: CONFIGURACIÓN'

	-- CtEmpresa y CtSucursal están definidas en EST_Matvi.sql

	PRINT 'Termina: MÓDULO: CONFIGURACIÓN'
	PRINT '===================================================='
END

-- 3
-- #################################################### --
-- MÓDULO: SEGURIDAD
-- #################################################### --
BEGIN 
	PRINT '===================================================='
	PRINT 'Inicia: MÓDULO: SEGURIDAD'

	-- ====================================================
	-- CtUsuario  (usuarios de demo — SYSTEM/Gabriel está en EST)
	-- ====================================================
	BEGIN

		SET IDENTITY_INSERT CtUsuario ON;

		INSERT INTO CtUsuario (
			Id,				IdCtRol,
			Nombre,			PrimerApellido,		SegundoApellido,
			Direccion,		Telefono,			Usuario,
			Contrasenia,	Activo)
		SELECT v.Id, v.IdCtRol, v.Nombre, v.PrimerApellido, v.SegundoApellido, v.Direccion, v.Telefono, v.Usuario, v.Contrasenia, v.Activo
		FROM (VALUES
			(1, 1, 'CARLOS', 'RAMÍREZ', 'LÓPEZ', 'AV. REFORMA 100, COL. CENTRO', '2491112233', 'ADM001', 'MTdAZG0xbg==', 1),
			(2, 2, 'VALERIA', 'ESPINOSA', 'NOLASCO', 'CALLE HIDALGO 45, COL. CENTRO', '2492223344', 'VEN001', 'MTdAZG0xbg==', 1),
			(3, 3, 'MARCOS', 'CARRILLO', 'MENDOZA', 'AV. EMILIANO ZAPATA 12, COL. CENTRO', '2493334455', 'ALM001', 'MTdAZG0xbg==', 1),
			(4, 4, 'RODRIGO', 'FLORES', 'GARCÍA', 'CALLE 3 SUR 89, COL. CENTRO', '2494445566', 'REP001', 'MTdAZG0xbg==', 1)
		) v(Id, IdCtRol, Nombre, PrimerApellido, SegundoApellido, Direccion, Telefono, Usuario, Contrasenia, Activo)
		WHERE NOT EXISTS (SELECT 1 FROM CtUsuario u WHERE u.Id = v.Id);

		SET IDENTITY_INSERT CtUsuario OFF;

	END

	-- ====================================================
	-- RlUsuarioSucursal  (asignaciones de usuarios de demo)
	-- ====================================================
	BEGIN
		INSERT INTO RlUsuarioSucursal (IdCtUsuario, IdCtSucursal)
		SELECT v.IdCtUsuario, v.IdCtSucursal
		FROM (VALUES
			(1, 1), (1, 2),
			(2, 1), (2, 2),
			(3, 1), (3, 2),
			(4, 1), (4, 2)
		) v(IdCtUsuario, IdCtSucursal)
		WHERE NOT EXISTS (SELECT 1 FROM RlUsuarioSucursal r WHERE r.IdCtUsuario = v.IdCtUsuario AND r.IdCtSucursal = v.IdCtSucursal);
	END

	PRINT 'Termina: MÓDULO: SEGURIDAD'
	PRINT '===================================================='
END

-- 4
-- #################################################### --
-- MÓDULO: PRODUCTOS
-- #################################################### --
BEGIN
	PRINT '===================================================='
	PRINT 'Inicia: MÓDULO: PRODUCTOS'

	-- CtSeccion y CtGrupo están definidos en EST_Matvi.sql


	-- ====================================================
	-- CtProducto										
	-- ====================================================
	BEGIN
		SET IDENTITY_INSERT [dbo].[CtProducto] ON 
	
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (1, 3, N'GRAVA', N'1/2"', NULL, 4, NULL, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (2, 3, N'GRAVA', N'2"', NULL, 4, 1, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (3, 3, N'ARENA', NULL, NULL, 4, 1, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (4, 2, N'BLOCK', N'12 CM', NULL, 6, 1, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (5, 2, N'BLOCK', N'14 CM', NULL, 6, 1, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (6, 2, N'TABIQUE ROJO', NULL, NULL, 6, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (7, 1, N'CEMENTO GRIS 50 KG', N'CPC 30R', N'CRUZ AZUL', 1, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (8, 1, N'CEMENTO GRIS 50 KG', N'CPC 40', N'CRUZ AZUL', 1, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (9, 1, N'MORTERO 50 KG', NULL, N'CRUZ AZUL', 1, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (10, 1, N'CEMENTO BLANCO 50 KG', NULL, N'CRUZ AZUL', 1, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (11, 1, N'CEMENTO BLANCO 25 KG', NULL, N'CRUZ AZUL', 1, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (12, 1, N'CEMENTO TOLTECA EXTRA 50 KG', N'CPC 30R', N'TOLTECA', 1, 1, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (13, 1, N'CEMENTO GRIS 50 KG', N'CPC 30R', N'MOCTEZUMA', 1, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (14, 1, N'CEMENTO GRIS 50 KG', N'CPO 40', N'MOCTEZUMA', 1, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (15, 5, N'ALAMBRON', N'1/4"', N'TERNIUM', 1, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (16, 5, N'ALAMBRE RECOCIDO', N'CAL. 16', N'TERNIUM', 1, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (17, 12, N'ABRAZADERA OMEGA GALVANIZADO', N'1/2"', N'ARGOS', 6, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (18, 28, N'TAQUETE PLASTICO', N'1/4"', N'ANCLO', 6, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (19, 5, N'CLAVO ESTÁNDAR', N'1"', N'DEACERO', 1, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (20, 5, N'CLAVO CONCRETO', N'2.5"', N'TRUPER', 1, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (21, 10, N'CODO 90°', N'2" CED40', N'TUBOPLUS', 6, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (22, 10, N'TEE SENCILLA', N'2" CÉD40', N'TIGRE', 6, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (23, 10, N'COPLE (UNIÓN)', N'2"', N'DURMAN', 6, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (24, 10, N'ADAPTADOR MACHO', N'2"', N'CHARLOTTE PIPE', 6, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (25, 10, N'CODO 45°', N'2" SANITARIO', N'PAVCO', 6, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (26, 10, N'REDUCCIÓN BUSHING', N'2"', N'DURMAN', 6, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (27, 25, N'LIJA DE AGUA (A-99)', N'60', N'FANDELI', 6, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (28, 25, N'LIJA PARA MADERA', N'40', N'3M', 5, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (29, 17, N'CABLE THW-2', N'14', N'IUSA', 5, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (30, 17, N'CABLE VINANEL XXI', N'14', N'CONDUMEX', 5, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (31, 17, N'CABLE USO RUDO (SJTO)', N'10, 3HILOS, DOBLE CAPA PVC', N'CONDUCTORES MONTERREY', 5, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (32, 8, N'LAZO DE POLIPROPILENO', N'1/8"', N'TRUPER', 1, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (33, 36, N'PISO CERÁMICO C32342', N'40 X 40', N'INTERCERAMIC', 3, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (34, 36, N'PORCELANATO GFDTRGET', N'60 X 60', N'VITROMEX', 3, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (35, 37, N'AZULEJO PARA MURO', N'20 X 30', N'PORCELANITE', 3, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (36, 36, N'PISO TIPO MADERA GDFGSTRFSD', N'18 X 50', N'LAMOSA', 3, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (37, 52, N'MANGUERA JARDÍN (REFORZADA)', N'1/2"', N'TRUPER', 5, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (38, 19, N'POLIDUCTO CORRUGADO R4', N'1/2', N'POLIFLEX', 5, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (39, 38, N'ADHESIVO  FESTERBOND', N'P/CONCRETO', N'FESTER', 2, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (40, 41, N'ÁCIDO MURIÁTICO ESTÁNDAR', N'HCI 12', N'SULTÁN', 2, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (41, 5, N'VARILLA #3', N'3/8"', N'TERNIUM', 6, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (42, 5, N'VARILLA #4', N'1/2', N'ACEROMEX', 6, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (43, 9, N'TUBO PVC HIDRÁULICO', N'1"', N'DURMAN', 5, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (44, 9, N'TUBO DE COBRE', N'1/2"', N'NACUBRE', 6, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (45, 9, N'TUBOPLUS', N'PP-R', N'ROTOPLAS', 5, 0, 0, NULL, 1, 0, 1)
		INSERT [dbo].[CtProducto] ([Id], [IdCtGrupo], [Nombre], [EspecificacionTecnica], [Marca], [IdCtUnidadBaseInventario], [Imagen], [CodigoBarrasInterno], [CodigoBarras], [Activo], [BajoPedido], [EntregaMostrador]) VALUES (46, 5, N'MALLA ELECTROSOLDADA 50M2', N'10-10', N'DEACERO', 3, 0, 0, NULL, 1, 0, 1)
	
		SET IDENTITY_INSERT [dbo].[CtProducto] OFF
	END

	BEGIN

		INSERT INTO RlProductoSucursal (
			IdCtProducto, 
			IdCtSucursal, 
			StockMinimoUBI,
			StockMaximoUBI,
			Regularizado, 
			Ubicacion)
		SELECT 
			p.Id,
			s.Id,
			0,
			0,
			0,
			NULL
		FROM CtProducto p
		CROSS JOIN CtSucursal s
		WHERE NOT EXISTS (
			SELECT 1 
			FROM RlProductoSucursal r
			WHERE r.IdCtProducto = p.Id
			  AND r.IdCtSucursal = s.Id
		);

	END

	-- ====================================================
	-- CtPresentacionProducto										
	-- ====================================================
	BEGIN 
		SET IDENTITY_INSERT [dbo].[CtPresentacionProducto] ON 
		
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (1, 1, N'METRO CÚBICO', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(200.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (2, 1, N'VIAJE', CAST(12.00000 AS Decimal(18, 5)), 1, 1, CAST(2400.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (3, 1, N'VIAJE', CAST(6.00000 AS Decimal(18, 5)), 1, 1, CAST(2000.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (4, 1, N'BOTE', CAST(0.02000 AS Decimal(18, 5)), 0, 1, CAST(30.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (5, 2, N'METRO CÚBICO', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(200.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (6, 2, N'VIAJE', CAST(12.00000 AS Decimal(18, 5)), 1, 1, CAST(2400.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (7, 2, N'VIAJE', CAST(6.00000 AS Decimal(18, 5)), 1, 1, CAST(1200.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (8, 3, N'METRO CÚBICO', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(250.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (9, 3, N'VIAJE', CAST(12.00000 AS Decimal(18, 5)), 1, 1, CAST(3000.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (10, 3, N'VIAJE', CAST(6.00000 AS Decimal(18, 5)), 1, 1, CAST(1500.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (11, 3, N'BOTE', CAST(0.02000 AS Decimal(18, 5)), 0, 1, CAST(30.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (12, 4, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(13.00000 AS Decimal(18, 5)), CAST(11.00000 AS Decimal(18, 5)), 500)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (13, 5, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(14.00000 AS Decimal(18, 5)), CAST(12.00000 AS Decimal(18, 5)), 500)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (14, 6, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(5.00000 AS Decimal(18, 5)), CAST(4.00000 AS Decimal(18, 5)), 250)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (15, 7, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(5.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (16, 7, N'BULTO', CAST(50.00000 AS Decimal(18, 5)), 1, 1, CAST(250.00000 AS Decimal(18, 5)), CAST(240.00000 AS Decimal(18, 5)), 20)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (17, 8, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(5.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (18, 8, N'BULTO', CAST(50.00000 AS Decimal(18, 5)), 1, 1, CAST(260.00000 AS Decimal(18, 5)), CAST(250.00000 AS Decimal(18, 5)), 20)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (19, 9, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(5.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (20, 9, N'BULTO', CAST(50.00000 AS Decimal(18, 5)), 1, 1, CAST(150.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (21, 10, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(5.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (22, 10, N'BULTO', CAST(50.00000 AS Decimal(18, 5)), 1, 1, CAST(450.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (23, 11, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(5.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (24, 11, N'BULTO', CAST(25.00000 AS Decimal(18, 5)), 1, 1, CAST(150.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (25, 12, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(5.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (26, 12, N'BULTO', CAST(50.00000 AS Decimal(18, 5)), 1, 1, CAST(250.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (27, 13, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(5.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (28, 13, N'BULTO', CAST(50.00000 AS Decimal(18, 5)), 1, 1, CAST(200.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (29, 14, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(5.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (30, 14, N'BULTO', CAST(50.00000 AS Decimal(18, 5)), 1, 1, CAST(260.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (31, 15, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(20.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (32, 16, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(25.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (33, 17, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(10.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (34, 17, N'BOLSA', CAST(2.00000 AS Decimal(18, 5)), 1, 0, CAST(0.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (35, 18, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(5.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (36, 18, N'BOLSA', CAST(50.00000 AS Decimal(18, 5)), 1, 0, CAST(0.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (37, 19, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(25.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (38, 19, N'CAJA', CAST(25.00000 AS Decimal(18, 5)), 1, 1, CAST(2000.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (39, 20, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(40.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (40, 21, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(15.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (41, 21, N'BOLSA', CAST(10.00000 AS Decimal(18, 5)), 1, 0, CAST(0.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (42, 22, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(15.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (43, 22, N'CAJA', CAST(25.00000 AS Decimal(18, 5)), 1, 0, CAST(0.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (44, 23, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(12.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (45, 24, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(15.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (46, 24, N'PAQUETE', CAST(10.00000 AS Decimal(18, 5)), 1, 0, CAST(0.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (47, 25, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(25.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (48, 26, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(33.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (49, 27, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(10.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (50, 27, N'PAQUETE', CAST(50.00000 AS Decimal(18, 5)), 1, 0, CAST(0.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (51, 28, N'METRO LINEAL', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(10.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (52, 28, N'ROLLO', CAST(50.00000 AS Decimal(18, 5)), 1, 0, CAST(0.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (53, 29, N'METRO LINEAL', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(10.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (54, 29, N'ROLLO', CAST(100.00000 AS Decimal(18, 5)), 1, 1, CAST(1500.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (55, 30, N'METRO LINEAL', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(20.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (56, 30, N'ROLLO', CAST(100.00000 AS Decimal(18, 5)), 1, 1, CAST(2500.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (57, 31, N'METRO LINEAL', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(30.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (58, 31, N'ROLLO', CAST(100.00000 AS Decimal(18, 5)), 1, 1, CAST(3000.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (59, 32, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(15.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (60, 33, N'METRO CUADRADO', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(255.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (61, 34, N'METRO CUADRADO', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(450.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (62, 35, N'METRO CUADRADO', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(152.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (63, 36, N'METRO CUADRADO', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(290.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (64, 37, N'METRO LINEAL', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(25.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (65, 38, N'METRO LINEAL', CAST(1.00000 AS Decimal(18, 5)), 0, 0, CAST(0.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (66, 39, N'LITRO', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(35.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (67, 39, N'CUBETA', CAST(16.00000 AS Decimal(18, 5)), 1, 1, CAST(56.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (68, 40, N'LITRO', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(45.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (69, 41, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(250.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (70, 41, N'TONELADA', CAST(150.00000 AS Decimal(18, 5)), 1, 1, CAST(15000.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (71, 42, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(300.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (72, 42, N'TONELADA', CAST(120.00000 AS Decimal(18, 5)), 1, 1, CAST(15600.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (73, 43, N'METRO LINEAL', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(12.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (74, 43, N'TUBO', CAST(6.00000 AS Decimal(18, 5)), 1, 1, CAST(250.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (75, 44, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), 1, 1, CAST(350.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (76, 44, N'METRO', CAST(0.16600 AS Decimal(18, 5)), 0, 1, CAST(55.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (77, 45, N'METRO LINEAL', CAST(1.00000 AS Decimal(18, 5)), 0, 1, CAST(85.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (78, 45, N'PIEZA', CAST(6.00000 AS Decimal(18, 5)), 1, 1, CAST(850.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (79, 46, N'METRO CUADRADO', CAST(1.00000 AS Decimal(18, 5)), 0, 0, CAST(0.00000 AS Decimal(18, 5)), NULL, NULL)
		INSERT [dbo].[CtPresentacionProducto] ([Id], [IdCtProducto], [Nombre], [FactorUBI], [EsParaCompra], [EsParaVenta], [PrecioVenta], [PrecioMayoreo], [UnidadesMayoreo]) VALUES (80, 46, N'TRAMO', CAST(2.50000 AS Decimal(18, 5)), 0, 1, CAST(350.00000 AS Decimal(18, 5)), NULL, NULL)
		
		SET IDENTITY_INSERT [dbo].[CtPresentacionProducto] OFF
		
	END

	PRINT 'Termina: MÓDULO: PRODUCTOS'
	PRINT '===================================================='
END 

-- 5
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
		INSERT INTO CtProveedor (Nombre, Direccion, Telefono, NombreContacto, TelefonoContacto)
		VALUES
			('CEMEX MÉXICO', 'AV. RICARDO MARGAIN 444, SAN PEDRO GARZA GARCÍA, NL', '8188888888', 'ATENCIÓN CLIENTES', '8001234567'),
			('HOLCIM MÉXICO', 'AV. INSURGENTES SUR 1602, COL. CRÉDITO CONSTRUCTOR, CDMX', '5555555555', 'DEPARTAMENTO COMERCIAL', '8007654321'),
			('TERNIUM MÉXICO', 'AV. VICENTE GUERRERO 400, MONTERREY, NL', '8184000000', 'ATENCIÓN COMERCIAL', '8184000001'),
			('CONDUMEX', 'AV. RÍO SAN JOAQUÍN 498, COL. AMPLIACIÓN GRANADA, CDMX', '5553000000', 'VENTAS INDUSTRIALES', '5553000001'),
			('COMEX', 'AV. EJÉRCITO NACIONAL 843, COL. GRANADA, CDMX', '5552000000', 'ATENCIÓN CLIENTES', '5552000001'),
			('FEGUSA MAYOREO', 'MARTÍN CARRERA #1005 PONIENTE, COL. HIDALGO, MONTERREY, NL', '8181149589', 'SUCURSAL HIDALGO', '8121659797'),
			('PLOMECSA', 'AV. CENTRAL 200, COL. INDUSTRIAL, CDMX', '5556000000', 'ATENCIÓN PLOMERÍA', '5556000001'),
			('GRUMOSA', 'AV. MOCTEZUMA 150, COL. INDUSTRIAL, CDMX', '5584934280', 'VENTAS FERRETERÍA', '5584934281'),
			('MACERE MÉXICO', 'DR. VÉRTIZ 1334, COL. PORTALES, BENITO JUÁREZ, CDMX', '5556013333', 'ATENCIÓN COMERCIAL', '7222105570'),
			('DISTRIBUIDORA FERRETERA ANDRADE', 'CALZ. IGNACIO ZARAGOZA 1200, IZTAPALAPA, CDMX', '5557000000', 'VENTAS FERRETERÍA', '5557000001'),
			('GRUPO ACERERO S.A.', 'CARRETERA A APODACA KM 12, MONTERREY, NL', '8189000000', 'ATENCIÓN COMERCIAL', '8189000001'),
			('IMPERQUIMIA', 'AV. TLÁHUAC 7900, CDMX', '5558000000', 'DEPARTAMENTO TÉCNICO', '5558000001'),
			('ROTOPLAS', 'AV. EJÉRCITO NACIONAL 843, COL. GRANADA, CDMX', '5559000000', 'ATENCIÓN CLIENTES', '5559000001'),
			('DISTRIBUIDORA ELÉCTRICA DEL CENTRO', 'AV. MORELOS 345, PUEBLA, PUE', '2222000000', 'VENTAS ELÉCTRICAS', '2222000001'),
			('GRUPO LAMOSA', 'AV. LÁZARO CÁRDENAS 2225, MONTERREY, NL', '8187000000', 'ATENCIÓN PISOS Y AZULEJOS', '8187000001');
	END

	-- ============================================
	-- RlProductoProveedor
	-- ============================================
	BEGIN
	
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (32, 1)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (3, 2)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (5, 2)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (6, 2)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (8, 2)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (11, 2)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (15, 2)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (29, 2)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (43, 2)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (21, 4)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (26, 4)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (28, 4)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (31, 4)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (36, 4)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (37, 4)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (38, 4)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (41, 4)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (42, 4)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (23, 5)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (8, 6)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (17, 6)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (24, 6)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (13, 8)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (16, 8)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (25, 8)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (33, 8)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (35, 8)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (44, 8)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (45, 8)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (1, 9)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (2, 9)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (4, 9)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (10, 10)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (18, 10)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (19, 10)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (22, 10)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (39, 10)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (12, 11)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (20, 11)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (30, 11)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (9, 12)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (14, 13)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (7, 14)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (46, 14)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (27, 15)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (34, 15)
		INSERT [dbo].[RlProductoProveedor] ([IdCtProducto], [IdCtProveedor]) VALUES (40, 15)

	END

	PRINT 'Termina: MÓDULO: COMPRAS'
	PRINT '===================================================='
END 

-- 6
-- #################################################### --
-- MÓDULO: INVENTARIOS
-- #################################################### --
BEGIN
	PRINT '===================================================='
	PRINT 'Inicia: MÓDULO: INVENTARIOS'

	-- ====================================================
	-- OpLote
	-- ====================================================
	BEGIN

		DECLARE @IdSucursalLote TINYINT;

		DECLARE cSucursalLote CURSOR FOR
		SELECT Id
		FROM CtSucursal;

		OPEN cSucursalLote;
		FETCH NEXT FROM cSucursalLote INTO @IdSucursalLote;

		WHILE @@FETCH_STATUS = 0
		BEGIN
			-- Asegurar fila en BtConsecutivo para LOTE (IdCtTipoConsecutivo = 2)
			IF NOT EXISTS (SELECT 1 FROM BtConsecutivo WHERE IdCtSucursal = @IdSucursalLote AND IdCtTipoConsecutivo = 2)
			BEGIN
				INSERT INTO BtConsecutivo (IdCtSucursal, IdCtTipoConsecutivo, UltimoNumero)
				VALUES (@IdSucursalLote, 2, 0);
			END

			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 1, 1, 1, N'METRO CÚBICO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-10T15:04:26.407' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 2, 1, 2, N'METRO CÚBICO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-10T15:12:55.123' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 3, 1, 3, N'METRO CÚBICO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-10T15:39:09.517' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 4, 1, 4, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-10T15:53:51.520' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 5, 1, 5, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-10T15:55:03.273' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 6, 1, 6, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-10T15:56:10.320' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 7, 1, 7, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-10T15:57:56.663' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 8, 1, 8, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-10T16:00:52.020' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 9, 1, 9, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-10T16:02:55.813' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 10, 1, 10, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-10T16:05:41.467' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 11, 1, 11, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-10T16:11:19.450' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 12, 1, 12, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-10T16:13:55.717' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 13, 1, 13, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-10T16:16:22.180' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 14, 1, 14, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-10T16:19:00.487' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 15, 1, 15, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-11T15:52:31.697' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 16, 1, 16, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-11T15:55:21.100' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 17, 1, 17, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-11T15:58:54.870' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 18, 1, 18, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-11T16:22:36.633' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 19, 1, 19, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T11:22:58.427' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 20, 1, 20, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T11:26:07.313' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 21, 1, 21, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T11:28:42.357' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 22, 1, 22, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T11:30:27.873' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 23, 1, 23, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T11:32:23.437' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 24, 1, 24, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T11:34:10.623' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 25, 1, 25, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T11:54:34.360' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 26, 1, 26, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T11:56:04.100' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 27, 1, 27, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T11:58:28.147' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 28, 1, 28, N'METRO LINEAL', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T12:00:25.777' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 29, 1, 29, N'METRO LINEAL', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T12:08:11.893' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 30, 1, 30, N'METRO LINEAL', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T12:13:42.880' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 31, 1, 31, N'METRO LINEAL', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T12:15:32.890' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 32, 1, 32, N'KILOGRAMO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T12:18:48.987' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 33, 1, 33, N'METRO CUADRADO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T12:21:19.577' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 34, 1, 34, N'METRO CUADRADO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T12:22:56.200' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 35, 1, 35, N'METRO CUADRADO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T12:24:15.320' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 36, 1, 36, N'METRO CUADRADO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T12:26:29.047' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 37, 1, 37, N'METRO LINEAL', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T13:08:15.440' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 38, 1, 38, N'METRO LINEAL', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T13:10:32.787' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 39, 1, 39, N'LITRO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T13:13:29.757' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 40, 1, 40, N'LITRO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T13:18:22.547' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 41, 1, 41, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T13:21:27.103' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 42, 1, 42, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T13:23:30.650' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 43, 1, 43, N'METRO LINEAL', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T13:28:33.713' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 44, 1, 44, N'PIEZA', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T16:01:08.257' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 45, 1, 45, N'METRO LINEAL', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T16:04:30.193' AS DateTime))
			INSERT [dbo].[OpLote] ([IdCtSucursal], [Numero], [IdCtTipoLote], [Cpp_IdCtProducto], [Cpp_Nombre], [Cpp_FactorUBI], [CantidadInicialUBI], [CantidadDisponibleUBI], [CostoUP], [CostoUBI], [Fecha]) VALUES (@IdSucursalLote, 46, 1, 46, N'METRO CUADRADO', CAST(1.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), CAST(N'2026-03-12T16:07:02.517' AS DateTime))

			UPDATE BtConsecutivo
			SET UltimoNumero = 46
			WHERE IdCtSucursal = @IdSucursalLote
			  AND IdCtTipoConsecutivo = 2; -- LOTE

			FETCH NEXT FROM cSucursalLote INTO @IdSucursalLote;
		END

		CLOSE cSucursalLote;
		DEALLOCATE cSucursalLote;

	END

	PRINT 'Termina: MÓDULO: INVENTARIOS'
	PRINT '===================================================='
END 

-- 7
-- #################################################### --
-- MÓDULO: VENTAS
-- #################################################### --
BEGIN
	PRINT '===================================================='
	PRINT 'Inicia: MÓDULO: VENTAS'

    -- ====================================================
	-- CtCliente									
	-- ====================================================
	BEGIN
		INSERT INTO CtCliente 
		( Nombre, Alias, Direccion, Telefono, RFCFiscal, IdCtRegimenFiscal, CalleFiscal, NumeroExteriorFiscal, NumeroInteriorFiscal, CodigoPostalFiscal, ColoniaFiscal, LocalidadFiscal, MunicipioFiscal, EntidadFederativaFiscal) VALUES
		('CONSTRUCCIONES HERNÁNDEZ',NULL,'CALLE 5 DE MAYO #123','2491234567','COSX800101AB1', 601,'5 DE MAYO','123','', '75150','CENTRO','ACATZINGO','ACATZINGO','PUEBLA'),
		('MATERIALES EL PROGRESO',NULL,'AV. INDEPENDENCIA #45','2499876543','MEPX800101AB2',603,'INDEPENDENCIA','45','', '75160','CENTRO','SAN SALVADOR EL SECO','SAN SALVADOR EL SECO','PUEBLA'),
		('FERRETERÍA LA ECONÓMICA',NULL,'CALLE REFORMA #210','2494567890','FEXX800101AB3', 601,'REFORMA','210','', '75480','CENTRO','TECAMACHALCO','TECAMACHALCO','PUEBLA'),
		('ARQ. LUIS MORALES',NULL,'PRIV. LAS FLORES #12','2493217890','MORL800101AB4',601,'LAS FLORES','12','', '75460','CENTRO','QUECHOLAC','QUECHOLAC','PUEBLA'),
		('ING. RAMÍREZ OBRAS CIVILES',NULL,'BLVD. MORELOS #500','2496541230','ROCX800101AB5', 603,'MORELOS','500','', '75200','CENTRO','TEPEACA','TEPEACA','PUEBLA'),
		('CARNICERÍA SAN JOSÉ',NULL,'CALLE HIDALGO #77','2491122334','CSJX800101AB6', 601,'HIDALGO','77','', '75150','CENTRO','ACATZINGO','ACATZINGO','PUEBLA'),
		('ABARROTES LUPITA',NULL,'AV. JUÁREZ #33','2495566778','ALPX800101AB7', 601,'JUÁREZ','33','', '75210','CENTRO','LOS REYES DE JUÁREZ','LOS REYES DE JUÁREZ','PUEBLA'),
		('TALLER MECÁNICO EL CAMINANTE',NULL,'CALLE 3 SUR #89','2496677889','TMEX800101AB8', 601,'3 SUR','89','', '75150','CENTRO','ACATZINGO','ACATZINGO','PUEBLA'),
		('PAPELERÍA LA ESTRELLA',NULL,'CALLE 2 ORIENTE #14','2497788990','PLEX800101AB9', 601,'2 ORIENTE','14','', '75480','CENTRO','TECAMACHALCO','TECAMACHALCO','PUEBLA');
	END

	PRINT 'Termina: MÓDULO: VENTAS'
	PRINT '===================================================='

END

-- 8
-- #################################################### --
-- MÓDULO: FINANZAS
-- #################################################### --
BEGIN
	PRINT '===================================================='
	PRINT 'Inicia: MÓDULO: FINANZAS'

	-- CtTipoGasto está definido en EST_Matvi.sql


	-- ====================================================
	-- CtCuentaBancaria
	-- ====================================================
	BEGIN
		INSERT INTO CtCuentaBancaria (Id, Nombre, Banco, NumeroCuenta, Activa) VALUES
		(1, 'Nómina BBVA',          'BBVA',         '0123456789', 1),
		(2, 'Caja Chica Banorte',   'Banorte',       '0987654321', 1),
		(3, 'Operativa Citibanamex','Citibanamex',   '1122334455', 1);
	END

	PRINT 'Termina: MÓDULO: FINANZAS'
	PRINT '===================================================='
END 

END -- Fin del bloque ELSE (Ambiente Desarrollo/Local)


