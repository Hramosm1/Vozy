# Vozy Fase 1

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
SqlServer = {
  BaseDeDatos: {
    nombre: 'Vozy',
    tablas: [
      {
        nombre: 'VozyEndpoint',
        columnas: [
          { nombre: 'id', tipo: 'uniqueidentifier', default: 'newid()' },
          { nombre: 'jsonData', tipo: 'varchar(max)' },
          { nombre: 'campaignId', tipo: 'varchar(50)' },
          { nombre: 'contactId', tipo: 'varchar(50)' },
          { nombre: 'sessionId', tipo: 'varchar(50)' },
          { nombre: 'agentName', tipo: 'varchar(50)' },
          { nombre: 'insertDate', tipo: 'DateTime', default: 'getdate()' }
        ]
      }
    ]
  }
}
```