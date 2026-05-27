# PLAN_CORRECCION_ENDPOINTS_MIDDLEWARE

## Objetivo

Alinear estrictamente el contrato expuesto por `Middleware.HotelJJ` con los contratos reales de:

1. `Servicio.Hotel` (monolito de referencia contractual)
2. `Microservicio.Reservas`
3. `Microservicio.Alojamiento`
4. `Microservicio.Hospedaje`
5. `Microservicio.Facturacion`
6. `Microservicio.Seguridad`

Sin inventar rutas, sin eliminar rutas existentes, y sin dejar `POST`/`PUT`/`PATCH` sin `requestBody` cuando en origen existe.

---

## Hallazgo raíz

La mayoría de endpoints internos del middleware están en controllers `Gateway` tipo proxy (`ProxyAsync`) con firmas sin modelos de entrada (solo `CancellationToken`), por lo que Swagger muestra endpoints incompletos (`No parameters` / sin `requestBody`) aunque el proxy sí reenvía body en runtime.

---

## Endpoints correctos por módulo y diferencias detectadas

## Reservas (obligatorio prioritario)

### Controllers de referencia
- `Microservicio.Reservas/Reservas.API/Controllers/V1/Internal/Reservas/ReservaController.cs`
- `Microservicio.Reservas/Reservas.API/Controllers/V1/Booking/ReservasPublicWriteController.cs`
- `Microservicio.Reservas/Reservas.API/Controllers/V1/Internal/Reservas/ClienteController.cs`
- `Microservicio.Reservas/Reservas.API/Controllers/V1/Booking/ClientesPublicWriteController.cs`
- Referencia equivalente en monolito:
  - `Servicio.Hotel.API/Controllers/V1/Internal/Reservas/ReservaController.cs`
  - `Servicio.Hotel.API/Controllers/V1/Booking/ReservasPublicWriteController.cs`
  - `Servicio.Hotel.API/Controllers/V1/Internal/Reservas/ClienteController.cs`
  - `Servicio.Hotel.API/Controllers/V1/Booking/ClientesPublicWriteController.cs`

### Endpoints a corregir en Middleware

- **`GET /api/v1/internal/clientes`**
  - Actual middleware: sin query params documentados.
  - Esperado: query `page`, `pageSize`.
  - Servicio destino: Reservas.
  - Controller: `ReservasGatewayController`.

- **`POST /api/v1/internal/clientes`**
  - Actual middleware: sin `requestBody`.
  - Esperado: body `ClienteCreateRequest`.
  - Servicio destino: Reservas.
  - Controller: `ReservasGatewayController`.

- **`PUT /api/v1/internal/clientes/{id}`**
  - Actual middleware: sin `requestBody`.
  - Esperado: body `ClienteUpdateRequest`.
  - Servicio destino: Reservas.
  - Controller: `ReservasGatewayController`.

- **`GET /api/v1/public/clientes/by-email`**
  - Actual middleware: sin query params documentados.
  - Esperado: query `correo`.
  - Servicio destino: Reservas.
  - Controller: `ReservasGatewayController`.

- **`GET /api/v1/public/reservas`**
  - Actual middleware: sin query params documentados.
  - Esperado: query `page`, `limit`, `estado`.
  - Servicio destino: Reservas.
  - Controller: `ReservasGatewayController`.

- **`POST /api/v1/internal/reservas`**
  - Actual middleware: sin `requestBody` en Swagger.
  - Esperado: body `InternalReservaCreateRequest`.
  - DTO esperado: create interno de Reservas (cliente/sucursal/habitaciones por GUID).
  - Servicio destino: Reservas.
  - Controller: `ReservasGatewayController`.

- **`POST /api/v1/internal/reservas/calcular-precio`**
  - Actual middleware: sin `requestBody`.
  - Esperado: body `ReservaPrecioRequest`.
  - Servicio destino: Reservas.
  - Controller: `ReservasGatewayController`.

- **`PUT /api/v1/internal/reservas/{id}`**
  - Actual middleware: sin `requestBody`.
  - Esperado: body `ReservaUpdateRequest`.
  - Servicio destino: Reservas.
  - Controller: `ReservasGatewayController`.

- **`PATCH /api/v1/internal/reservas/{id}/cancelar`**
  - Actual middleware: sin `requestBody`.
  - Esperado: body `CancelarReservaRequest`.
  - Servicio destino: Reservas.
  - Controller: `ReservasGatewayController`.

### Endpoints de Reservas que deben mantenerse
- `GET /api/v1/internal/reservas`
- `GET /api/v1/internal/reservas/{id}`
- `PATCH /api/v1/internal/reservas/{id}/confirmar` (sin body en contrato origen)
- `GET /api/v1/public/reservas/{reservaGuid}`
- `POST /api/v1/public/reservas`
- `PATCH /api/v1/public/reservas/{reservaGuid}/cancelar`
- `POST /api/v1/public/reservas/calcular-precio`
- `POST /api/v1/accommodations/reservas`
- `GET /api/v1/accommodations/reservas/{reservaGuid}`

---

## Alojamiento

### Controllers de referencia
- `Alojamiento.API/Controllers/V1/Internal/Alojamiento/*`
- `Alojamiento.API/Controllers/V1/Internal/Valoraciones/ValoracionController.cs`
- `Alojamiento.API/Controllers/V1/Booking/*`
- Referencia equivalente en `Servicio.Hotel.API`.

### Endpoints a corregir en Middleware (`AlojamientoGatewayController`)
- `GET /api/v1/public/habitaciones`: agregar query `fechaInicio`, `fechaFin`, `sucursalGuid`.
- `GET /api/v1/internal/sucursales`: agregar query `estado`.
- `POST /api/v1/internal/sucursales`: agregar body `SucursalUpsertRequest`.
- `PUT /api/v1/internal/sucursales/{id}`: agregar body `SucursalUpsertRequest`.
- `PUT /api/v1/internal/sucursales/{sucursalGuid}`: agregar body `SucursalUpsertRequest`.
- `PATCH /api/v1/internal/sucursales/{sucursalGuid}/politicas`: agregar body `SucursalPoliticasPatchRequest`.
- `PATCH /api/v1/internal/sucursales/{sucursalGuid}/inhabilitar`: agregar body `InhabilitarRequest`.

- `GET /api/v1/internal/habitaciones`: agregar query `estado`.
- `POST /api/v1/internal/habitaciones`: agregar body `HabitacionCreateRequest`.
- `PUT /api/v1/internal/habitaciones/{id}`: agregar body `HabitacionUpdateRequest`.
- `PATCH /api/v1/internal/habitaciones/{id}/estado`: agregar body `HabitacionEstadoRequest`.

- `GET /api/v1/internal/tipos-habitacion`: agregar query `estado`.
- `POST /api/v1/internal/tipos-habitacion`: agregar body `TipoHabitacionUpsertRequest`.
- `PUT /api/v1/internal/tipos-habitacion/{id}`: agregar body `TipoHabitacionUpsertRequest`.
- `PUT /api/v1/internal/tipos-habitacion/{tipoGuid}`: agregar body `TipoHabitacionUpsertRequest`.

- `POST /api/v1/internal/tarifas`: agregar body `TarifaUpsertRequest`.
- `PUT /api/v1/internal/tarifas/{id}`: agregar body `TarifaUpsertRequest`.

- `POST /api/v1/internal/catalogo-servicios`: agregar body `CatalogoServicioUpsertRequest`.
- `PUT /api/v1/internal/catalogo-servicios/{id}`: agregar body `CatalogoServicioUpsertRequest`.

- `POST /api/v1/internal/valoraciones`: agregar body `ValoracionCreateRequest`.
- `PATCH /api/v1/internal/valoraciones/{id}/moderar`: agregar body `ValoracionModeracionRequest`.
- `PATCH /api/v1/internal/valoraciones/{id}/responder`: agregar body `ValoracionRespuestaRequest`.

---

## Hospedaje

### Controllers de referencia
- `Hospedaje.API/Controllers/V1/Internal/Hospedaje/EstadiaController.cs`
- `Hospedaje.API/Controllers/V1/Internal/Hospedaje/CargoEstadiaController.cs`
- Referencia equivalente en `Servicio.Hotel.API`.

### Endpoints a corregir en Middleware (`HospedajeGatewayController`)
- Ya tiene body en:
  - `PATCH /api/v1/internal/estadias/{id}/checkout`
  - `POST /api/v1/internal/estadias/{id}/cargos`
- No requiere body en:
  - `POST /api/v1/internal/estadias/checkin/{id_reserva}`
  - `PATCH /api/v1/internal/cargos-estadia/{id}/anular`

### Acción
- Validar que no se pierda documentación de path params y mantener contratos existentes.

---

## Facturación y Pagos

### Controllers de referencia
- `Facturacion.API/Controllers/V1/Internal/Facturacion/FacturaController.cs`
- `Facturacion.API/Controllers/V1/Internal/Pagos/PagoController.cs`
- `Facturacion.API/Controllers/V1/Booking/PagosPublicController.cs`
- Referencia equivalente en `Servicio.Hotel.API`.

### Endpoints a corregir en Middleware (`FacturacionGatewayController`)
- `GET /api/v1/internal/pagos`: agregar query `page`, `pageSize`.
- `GET /api/v1/internal/pagos/factura/{facturaId}`: agregar query `page`, `pageSize`.
- **Validación de endpoint existente ya en middleware**:
  - `POST /api/v1/internal/pagos` ya existe en `FacturacionIntegrationController` (no en gateway).
  - Acción: mantenerlo ahí para evitar conflicto Swagger por ruta/método duplicado.

### Endpoints con body ya correcto
- `PATCH /api/v1/internal/facturas/{id}/anular` -> `AnularFacturaRequest`.
- `PATCH /api/v1/internal/pagos/{id}/estado` -> `PagoEstadoRequest`.
- `POST /api/v1/pagos/simular` -> `PagoSimularRequest` interno.
- `POST /api/v1/public/pagos/simular` -> `PublicPagoSimularRequest`.

---

## Auth

### Controllers de referencia
- `Seguridad.API/Controllers/V1/Auth/AuthController.cs`
- Referencia equivalente en `Servicio.Hotel.API`.

### Estado
- `AuthIntegrationController` ya expone bodies correctos para:
  - `login`, `refresh`, `register-cliente`, `logout`, `cambiar-password`.
- Acción: solo validar Swagger y seguridad JWT.

---

## Seguridad (Usuarios, Roles, Permisos, Auditoría)

### Controllers de referencia
- `Seguridad.API/Controllers/V1/Internal/Seguridad/UsuarioController.cs`
- `Seguridad.API/Controllers/V1/Internal/Seguridad/RolController.cs`
- `Seguridad.API/Controllers/V1/Internal/Seguridad/RolPermisosController.cs`
- `Seguridad.API/Controllers/V1/Internal/Seguridad/PermisosController.cs`
- `Seguridad.API/Controllers/V1/Internal/Seguridad/AuditoriaController.cs`
- Referencia equivalente en `Servicio.Hotel.API`.

### Endpoints a corregir en Middleware (`SeguridadGatewayController`)
- `GET /api/v1/internal/usuarios`: agregar query `page`, `pageSize`, `estado`.
- `POST /api/v1/internal/usuarios`: agregar body `UsuarioCreateRequest`.
- `PUT /api/v1/internal/usuarios/{id}`: agregar body `UsuarioUpdateRequest`.
- `PUT /api/v1/internal/usuarios/{usuarioGuid}`: agregar body `UsuarioUpdateRequest`.
- `PATCH /api/v1/internal/usuarios/{id}/inhabilitar`: agregar body `InhabilitarRequest`.

- `GET /api/v1/internal/roles`: agregar query `estado`.
- `POST /api/v1/internal/roles`: agregar body `RolUpsertRequest`.
- `PUT /api/v1/internal/roles/{id}`: agregar body `RolUpsertRequest`.
- `PUT /api/v1/internal/roles/{rolGuid}`: agregar body `RolUpsertRequest`.
- `POST /api/v1/internal/roles/{rolGuid}/permisos`: agregar body `RolPermisosUpsertRequest`.

- `GET /api/v1/internal/auditoria`: agregar query `tabla`.

---

## Flujos integrados

### Controller
- `HotelJJ.API/Controllers/V1/Flujos/IntegratedFlowController.cs`

### Acción
- Mantener sin cambios contractuales (orquestación propia), solo validar que no interfiera con rutas de microservicios.

---

## Servicio/cliente HTTP a consumir (por endpoint)

- Endpoints bajo `/api/v1/internal|public|accommodations` de Reservas Gateway -> `IMicroserviceProxy` con destino `"Reservas"`.
- Endpoints de Alojamiento Gateway -> destino `"Alojamiento"`.
- Endpoints de Hospedaje Gateway -> destino `"Hospedaje"`.
- Endpoints de Facturación Gateway -> destino `"Facturacion"`.
- Endpoints de Seguridad Gateway -> destino `"Seguridad"`.
- Validación de URLs base en:
  - `HotelJJ.API/appsettings.json`
  - `HotelJJ.API/appsettings.Development.json`
  - Resolver: `HotelJJ.API/Infrastructure/Proxy/ProxyRouteResolver.cs`.

---

## Controllers y archivos a modificar

### Controllers
- `HotelJJ.API/Controllers/V1/Gateway/ReservasGatewayController.cs`
- `HotelJJ.API/Controllers/V1/Gateway/AlojamientoGatewayController.cs`
- `HotelJJ.API/Controllers/V1/Gateway/FacturacionGatewayController.cs`
- `HotelJJ.API/Controllers/V1/Gateway/SeguridadGatewayController.cs`
- (si aplica documentación fina) `HotelJJ.API/Controllers/V1/Gateway/HospedajeGatewayController.cs`

### Modelos/DTO request a crear o ampliar
- `HotelJJ.API/Models/Requests/Reservas/ReservationRequests.cs` (ampliar con contratos internos/publicos faltantes del gateway)
- `HotelJJ.API/Models/Requests/Alojamiento/AlojamientoGatewayRequests.cs` (nuevo)
- `HotelJJ.API/Models/Requests/Seguridad/SeguridadGatewayRequests.cs` (nuevo)
- `HotelJJ.API/Models/Requests/Facturacion/FacturacionRequests.cs` (ya existe, reutilizar)
- `HotelJJ.API/Models/Requests/Hospedaje/HospedajeRequests.cs` (ya existe)

### Configuración Swagger (si hiciera falta)
- `HotelJJ.API/Program.cs` (verificar que no se requiere ajuste adicional; prioridad en firmas tipadas de acción).

---

## Riesgos de romper compatibilidad

1. **Cambio de esquema Swagger**: clientes generados automáticamente podrían detectar campos nuevos (esperado y deseado).
2. **Ambigüedad de clases Request**: si se duplican nombres, puede romper compilación; se debe resolver con namespaces explícitos.
3. **Rutas solapadas**: mantener exactamente las rutas actuales para no romper consumidores.
4. **Seguridad**: no agregar `AllowAnonymous` en endpoints internos; mantenerlo solo en públicos.
5. **Proxy de body**: no alterar el mecanismo de forwarding, solo tipar firmas para documentación.

---

## Fases de implementación

### Fase 1 - Tipado de contratos faltantes
- Crear/ajustar request models para Alojamiento y Seguridad.
- Completar models de Reservas para endpoints internos del gateway.

### Fase 2 - Corrección de controllers Gateway
- Agregar `[FromBody]` en todos los `POST`/`PUT`/`PATCH` que lo requieren según microservicio.
- Agregar `[FromQuery]` en endpoints GET que lo exigen.
- Agregar endpoint faltante `POST /internal/pagos`.

### Fase 3 - Verificación de seguridad
- Confirmar `Authorize` para internos y `AllowAnonymous` para públicos según contrato real.

### Fase 4 - Compilación y validación Swagger
- Compilar solución/proyecto `HotelJJ.API`.
- Ejecutar API y revisar Swagger:
  - path params visibles
  - query params visibles
  - requestBody en `POST/PUT/PATCH` cuando corresponde
  - schemas de entrada/salida.

### Fase 5 - Prueba de trazabilidad de ruteo
- Verificar que cada endpoint corregido sigue llamando al microservicio correcto (`ProxyRouteResolver` + `IMicroserviceProxy`).

