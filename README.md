# Vozy Fase 1 recuperacion

Publicada en: ***http://62.171.184.240:9431***.
La fase 1 de vozy tiene 4 
funciones bien definidas:

* Autenticar por medio de token.
* Verificar el agente de la gestion para poder procesarlo de la forma correcta.
* Transformar el cuerpo del request a un formato que pueda consumir la fase 2.
* Almacenar la gestion en una base de datos local para asi no perder ninguna gestion aunque falle al momento de subir la gestion al sistema en la fase 2.

## Base de datos local
Actualmente la base de datos se encuentra unicamente en el servidor Ubuntu
Con una estructura de esta forma
```javascript
mariadb = {
  BaseDeDatos: {
    nombre: 'Vozy',
    tablas: [
      {
        nombre: 'VozyEndpoint',
        columnas: [
          { nombre: 'id', tipo: 'UUID', PrimaryKey:true },
          { nombre: 'jsonData', tipo: 'text', notNull:true },
          { nombre: 'campaignId', tipo: 'varchar(50)' },
          { nombre: 'contactId', tipo: 'varchar(50)' },
          { nombre: 'sessionId', tipo: 'varchar(50)' },
          { nombre: 'agentName', tipo: 'varchar(50)' },
          { nombre: 'insertDate', tipo: 'DateTime', default: 'getdate()' },
          { nombre: 'sic', tipo: 'boolean', default: 0}
        ]
      }
    ]
  }
}
```

Para almacenar gestiones se utiliza el proceso almacenado **SP_InsertGestion**
```sql
call SP_InsertGestion('jsonData','campaignId','contactId','sessionId','agentName')
```