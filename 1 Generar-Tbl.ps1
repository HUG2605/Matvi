# Generador de clases Etd desde SQL Server con orden de columnas
# Autor: Gabriel

$server = "LOCALHOST\SQLEXPRESS"
$database = "MatviDB"
$username = "sa"
$password = "Sql_P3r50@1"

#$server = "LOCALHOST\SQL_EXP_2025"
#$database = "MatviDB"
#$username = "sa"
#$password = "Sql_P3r50@1"

$outputDir = "C:\GIT\Matvi\Matvi.Back.Infrastructure\Persistence\Entities"
$namespace = "Matvi.Back.Infrastructure.Persistence.Entities"
$suffix = "Ett"

$excluirAuditable = @("BitacoraCambioTbl")

$connectionString = "Server=$server;Database=$database;User Id=$username;Password=$password;TrustServerCertificate=true;"
$connection = New-Object System.Data.SqlClient.SqlConnection $connectionString
$connection.Open()

# ============================================================
# LEER TABLAS
# ============================================================
$tablesCmd = $connection.CreateCommand()
$tablesCmd.CommandText = @"
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE' 
  AND TABLE_SCHEMA = 'dbo'
"@

$tables = @()
$reader = $tablesCmd.ExecuteReader()
while ($reader.Read()) { 
    $tables += $reader["TABLE_NAME"].ToString().Trim()
}
$reader.Close()

# ============================================================
# LEER RELACIONES FK
# ============================================================
$relationsCmd = $connection.CreateCommand()
$relationsCmd.CommandText = @"
SELECT  
    fk_tab.name AS ForeignTable,
    fk_col.name AS ForeignColumn,
    pk_tab.name AS PrincipalTable
FROM sys.foreign_key_columns fkc
JOIN sys.tables fk_tab ON fkc.parent_object_id = fk_tab.object_id
JOIN sys.columns fk_col ON fkc.parent_object_id = fk_col.object_id AND fkc.parent_column_id = fk_col.column_id
JOIN sys.tables pk_tab ON fkc.referenced_object_id = pk_tab.object_id
"@

$reader = $relationsCmd.ExecuteReader()
$fkRelations = @{}
$inverseMap  = @{}

while ($reader.Read()) {

    $foreignReal   = $reader["ForeignTable"].ToString().Trim()
    $foreignNorm   = $foreignReal.ToUpper()
    $principalReal = $reader["PrincipalTable"].ToString().Trim()
    $foreignColumn = $reader["ForeignColumn"].ToString().Trim()

    if (-not $fkRelations.ContainsKey($foreignNorm)) {
        $fkRelations[$foreignNorm] = @()
    }
    $fkRelations[$foreignNorm] += @{
        Column = $foreignColumn
        Table  = $principalReal
    }

    $principalNorm = $principalReal.ToUpper()
    if (-not $inverseMap.ContainsKey($principalNorm)) {
        $inverseMap[$principalNorm] = @()
    }
    $inverseMap[$principalNorm] += @{
        Table  = $foreignReal
        Column = $foreignColumn
    }
}
$reader.Close()

# Crear carpeta si no existe
if (!(Test-Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir | Out-Null
}

# ============================================================
# GENERAR ENTIDADES
# ============================================================

# Limpiar archivos existentes
Get-ChildItem -Path $outputDir -File | Remove-Item -Force
Write-Host "🧹 Archivos previos eliminados en: $outputDir"

foreach ($tableReal in $tables) {

    $tableNorm = $tableReal.ToUpper()

    # Diccionario de nulabilidad por columna
    $columnNullability = @{}

    $colsCmd = $connection.CreateCommand()
    $colsCmd.CommandText = @"
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, ORDINAL_POSITION
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = '$tableReal'
  AND TABLE_SCHEMA = 'dbo'
ORDER BY ORDINAL_POSITION
"@

    $reader = $colsCmd.ExecuteReader()
    $props  = @()

    while ($reader.Read()) {
        $name       = $reader["COLUMN_NAME"]
        $sqlType    = $reader["DATA_TYPE"]
        $isNullable = $reader["IS_NULLABLE"]

        $columnNullability[$name] = ($isNullable -eq "YES")

        $csType = switch ($sqlType) {
            "tinyint"  { "byte" }
            "smallint" { "short" }
            "int"      { "int" }
            "bigint"   { "long" }
            "bit"      { "bool" }
            "varchar"  { "string" }
            "nvarchar" { "string" }
            "char"     { "string" }
            "datetime" { "DateTime" }
            "decimal"  { "decimal" }
            "float"    { "double" }
            default    { "string" }
        }

        $nullable = ""
        $default  = ""

        if ($csType -eq "string") {
            if ($isNullable -eq "YES") { $nullable = "?" }
            else { $default = " = null!;" }
        }
        elseif ($isNullable -eq "YES") {
            $nullable = "?"
        }

        $props += "    public $csType$nullable $name { get; set; }$default"
    }
    $reader.Close()

    # ============================================================
    # MANY-TO-ONE
    # ============================================================
    if ($fkRelations.ContainsKey($tableNorm)) {

        foreach ($fk in $fkRelations[$tableNorm]) {

            $typeName     = "$($fk.Table)$suffix"
            $isNullableFK = $columnNullability[$fk.Column]

            # ⭐ Convención simple: Nav_ + nombre columna FK
            $navName = "Nav_$($fk.Column)"

            if ($isNullableFK) {
                $props += "    public ${typeName}? $navName { get; set; }"
            }
            else {
                $props += "    public $typeName $navName { get; set; } = null!;"
            }
        }
    }

    # ============================================================
    # ONE-TO-MANY
    # ============================================================
    if ($inverseMap.ContainsKey($tableNorm)) {

        foreach ($inv in $inverseMap[$tableNorm]) {

            $childReal = $inv.Table
            $fkColumn  = $inv.Column

            # ⭐ Convención simple: Coll_ + tabla hija + _ + columna FK
            $collectionName = "Coll_${childReal}_$fkColumn"

            $props += "    public ICollection<${childReal}${suffix}> $collectionName { get; set; } = new List<${childReal}${suffix}>();"
        }
    }

    $className = "${tableReal}${suffix}"
    $herencia  = if ($excluirAuditable -contains $className) { "" } else { " : IAuditable" }

    $content = @"
using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace $namespace;

public class $className$herencia
{
$($props -join "`r`n")
}
"@

    $content  = $content -replace "`r?`n", "`r`n"
    $filePath = Join-Path $outputDir "$className.cs"

    $utf8NoBom = New-Object System.Text.UTF8Encoding $false
    $writer    = New-Object System.IO.StreamWriter($filePath, $false, $utf8NoBom)
    $writer.Write($content)
    $writer.Close()

    Write-Host "✅ Generado: $filePath"
}

$connection.Close()
Write-Host "🎉 Generación de entidades completada"