# ============================================================
# Generador de clases Cfg para EF Core basado en entidades
# Alineado con la convención de colecciones del Ett
# Normalizado a CRLF
# ============================================================

# Parámetros
$server = "LOCALHOST\SQLEXPRESS"
$database = "MatviDB"
$username = "sa"
$password = "Sql_P3r50@1"

#$server = "LOCALHOST\SQL_EXP_2025"
#$database = "MatviDB"
#$username = "sa"
#$password = "Sql_P3r50@1"

$etdDir = "C:\GIT\Matvi\Matvi.Back.Infrastructure\Persistence\Entities"
$cfgDir = "C:\GIT\Matvi\Matvi.Back.Infrastructure\Persistence\Configurations"

$etdNamespace = "Matvi.Back.Infrastructure.Persistence.Entities"
$cfgNamespace = "Matvi.Back.Infrastructure.Persistence.Configurations"

$suffix = "Ett"

# Conexión
$connectionString = "Server=$server;Database=$database;User Id=$username;Password=$password;TrustServerCertificate=true;"
$connection = New-Object System.Data.SqlClient.SqlConnection $connectionString
$connection.Open()

# ============================================================
# Obtener PKs
# ============================================================
$pkMap = @{}
$pkCmd = $connection.CreateCommand()
$pkCmd.CommandText = @"
SELECT KU.TABLE_NAME, KU.COLUMN_NAME
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU 
    ON TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME
WHERE TC.CONSTRAINT_TYPE = 'PRIMARY KEY'
AND TC.TABLE_SCHEMA = 'dbo'
ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION
"@
$reader = $pkCmd.ExecuteReader()
while ($reader.Read()) {
    $tbl = $reader["TABLE_NAME"]
    if (-not $pkMap.ContainsKey($tbl)) { $pkMap[$tbl] = @() }
    $pkMap[$tbl] += $reader["COLUMN_NAME"]
}
$reader.Close()

# ============================================================
# Columnas
# ============================================================
$colList = @()
$colCmd = $connection.CreateCommand()
$colCmd.CommandText = @"
SELECT  
    c.TABLE_NAME,
    c.COLUMN_NAME,
    c.DATA_TYPE,
    c.CHARACTER_MAXIMUM_LENGTH,
    c.IS_NULLABLE,
    c.ORDINAL_POSITION,
    col.is_identity,
    col.precision,
    col.scale
FROM INFORMATION_SCHEMA.COLUMNS c
JOIN sys.columns col 
    ON col.object_id = OBJECT_ID(c.TABLE_SCHEMA + '.' + c.TABLE_NAME)
    AND col.name = c.COLUMN_NAME
WHERE c.TABLE_SCHEMA = 'dbo'
ORDER BY c.TABLE_NAME, c.ORDINAL_POSITION
"@
$reader = $colCmd.ExecuteReader()
while ($reader.Read()) {
    $colList += @{
        Tabla     = $reader["TABLE_NAME"]
        Nombre    = $reader["COLUMN_NAME"]
        Tipo      = $reader["DATA_TYPE"]
        Longitud  = $reader["CHARACTER_MAXIMUM_LENGTH"]
        Nullable  = $reader["IS_NULLABLE"]
        Posicion  = [int]$reader["ORDINAL_POSITION"]
        Identity  = $reader["is_identity"]
        Precision = $reader["precision"]
        Scale     = $reader["scale"]
    }
}
$reader.Close()

# ============================================================
# Relaciones FK
# ============================================================
$relMap = @{}
$relCmd = $connection.CreateCommand()
$relCmd.CommandText = @"
SELECT 
    fk_tab.name AS ForeignTable,
    fk_col.name AS ForeignColumn,
    pk_tab.name AS PrincipalTable
FROM sys.foreign_key_columns fkc
JOIN sys.tables fk_tab ON fkc.parent_object_id = fk_tab.object_id
JOIN sys.columns fk_col ON fkc.parent_object_id = fk_col.object_id AND fkc.parent_column_id = fk_col.column_id
JOIN sys.tables pk_tab ON fkc.referenced_object_id = pk_tab.object_id
"@
$reader = $relCmd.ExecuteReader()
while ($reader.Read()) {
    $child  = $reader["ForeignTable"]
    $col    = $reader["ForeignColumn"]
    $parent = $reader["PrincipalTable"]

    if (-not $relMap.ContainsKey($child)) { $relMap[$child] = @{} }
    $relMap[$child][$col] = $parent
}
$reader.Close()

# ============================================================
# Crear directorio destino
# ============================================================
if (!(Test-Path $cfgDir)) { New-Item -ItemType Directory -Path $cfgDir | Out-Null }

# Limpiar archivos existentes
Get-ChildItem -Path $cfgDir -File | Remove-Item -Force
Write-Host "🧹 Archivos previos eliminados en: $cfgDir"

# ============================================================
# Procesar entidades
# ============================================================
$files = Get-ChildItem -Path $etdDir -Filter *.cs
foreach ($file in $files) {

    $baseName = [System.IO.Path]::GetFileNameWithoutExtension($file.Name)

    if ($baseName -match "^(?<name>.+?)Ett$") {
        $tblName = $matches["name"]
    } else {
        continue
    }

    $cfgName = "${tblName}Cfg"
    $etdType = "${tblName}$suffix"

    $lines = @()
    $lines += "using Microsoft.EntityFrameworkCore;"
    $lines += "using Microsoft.EntityFrameworkCore.Metadata.Builders;"
    $lines += "using $etdNamespace;"
    $lines += ""
    $lines += "namespace $cfgNamespace;"
    $lines += ""
    $lines += "public class $cfgName : IEntityTypeConfiguration<$etdType>"
    $lines += "{"
    $lines += "    public void Configure(EntityTypeBuilder<$etdType> builder)"
    $lines += "    {"
    $lines += "        builder.ToTable(`"$tblName`");"
    $lines += ""

    # ============================================================
    # PK
    # ============================================================
    if ($pkMap.ContainsKey($tblName)) {
        $pkCols = $pkMap[$tblName]
        if ($pkCols.Count -eq 1) {
            $lines += "        builder.HasKey(e => e.$($pkCols[0]));"
        } else {
            $pkList = ($pkCols | ForEach-Object { "e.$_" }) -join ", "
            $lines += "        builder.HasKey(e => new { $pkList });"
        }
        $lines += ""
    }

    # ============================================================
    # Columnas
    # ============================================================
    $orderedCols = $colList | Where-Object { $_.Tabla -eq $tblName } | Sort-Object Posicion
    foreach ($col in $orderedCols) {

        $colName    = $col.Nombre
        $tipo       = $col.Tipo
        $max        = $col.Longitud
        $nullable   = $col.Nullable
        $isIdentity = $col.Identity
        $precision  = $col.Precision
        $scale      = $col.Scale

        if ($tipo -in @("varchar","nvarchar","char")) {
            if ($max -eq -1) { $colType = "$tipo(max)" }
            else { $colType = "$tipo($max)" }
        }
        elseif ($tipo -eq "decimal") {
            $colType = "decimal($precision,$scale)"
        }
        else {
            $colType = $tipo
        }

        $conf  = "        builder.Property(e => e.$colName)"
        $conf += "`r`n            .HasColumnType(`"$colType`")"

        if ($tipo -eq "decimal") {
            $conf += "`r`n            .HasPrecision($precision, $scale)"
        }

        if ($tipo -in @("varchar","nvarchar","char") -and $max -ne -1) {
            $conf += "`r`n            .HasMaxLength($max)"
        }

        if ($nullable -eq "NO") {
            $conf += "`r`n            .IsRequired()"
        }

        if ($isIdentity -eq $true) {
            $conf += "`r`n            .ValueGeneratedOnAdd()"
        }

        $conf += ";"
        $lines += $conf
    }

    # ============================================================
    # Relaciones FK
    # ============================================================
    if ($relMap.ContainsKey($tblName)) {
        foreach ($fkCol in $relMap[$tblName].Keys) {

            $parentTbl  = $relMap[$tblName][$fkCol]
            $fkColInfo  = $orderedCols | Where-Object { $_.Nombre -eq $fkCol }
            $isOptional = $fkColInfo.Nullable -eq "YES"

            # ⭐ Convención simple: Nav_ + columna FK
            $navName  = "Nav_$fkCol"

            # ⭐ Convención simple: Coll_ + tabla hija + _ + columna FK
            $childNav = "Coll_${tblName}_$fkCol"

            $lines += ""
            $lines += "        builder.HasOne(e => e.$navName)"
            $lines += "            .WithMany(p => p.$childNav)"
            $lines += "            .HasForeignKey(e => e.$fkCol)"

            if ($isOptional) {
                $lines += "            .IsRequired(false)"
            } else {
                $lines += "            .IsRequired()"
            }

            $lines += "            .OnDelete(DeleteBehavior.Restrict);"
        }
    }

    $lines += "    }"
    $lines += "}"

    $content  = ($lines -join "`r`n")
    $content  = $content -replace "`r?`n", "`r`n"
    $filePath = Join-Path $cfgDir "$cfgName.cs"

    $utf8NoBom = New-Object System.Text.UTF8Encoding $false
    $writer    = New-Object System.IO.StreamWriter($filePath, $false, $utf8NoBom)
    $writer.Write($content)
    $writer.Close()

    Write-Host "✅ Generado: $cfgName.cs"
}

$connection.Close()
Write-Host "🎉 Generación de configuraciones completada"