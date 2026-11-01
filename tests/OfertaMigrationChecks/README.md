# Verificación de la unificación de tipos de oferta

Ejecutar en Windows con SQL Server LocalDB instalado:

```powershell
dotnet run --project tests/OfertaMigrationChecks --configuration Release
```

El programa crea bases con nombres aleatorios `BolsaTrabajo_MigrationChecks_*` y las elimina al terminar. No utiliza la conexión de la aplicación. Verifica la migración desde el esquema anterior, categorías personalizadas, práctica profesional nueva o preexistente, respaldo de discrepancias, altas y modificaciones de ofertas, filtros, protección de categorías en uso y reversión/reaplicación de la migración.

La migración `20261101120000_UnificarTipoOferta` se aplica al iniciar la nueva WebAPI mediante `MigrateAsync`. Conserva `TipoOfertaId`, excepto para ofertas cuyo antiguo vínculo era `PracticaProfesional`: esas ofertas pasan a la categoría “Práctica profesional”. La categoría se busca por nombre y se crea con identidad si falta, sin reservar un ID que pueda estar ocupado.

La tabla histórica `RespaldoVinculosOferta` conserva el ID de oferta, el tipo anterior y el vínculo original para revisar discrepancias o revertir la migración. Se administra exclusivamente desde la migración y no se expone en la API. En la reversión, las categorías personalizadas sin equivalente en la enumeración antigua reciben el vínculo `Pasantia`; las ofertas con respaldo recuperan su vínculo original. No se elimina “Práctica profesional”, ya que podría tener ofertas nuevas o haber existido antes de migrar.
