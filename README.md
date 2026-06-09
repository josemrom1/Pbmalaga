# Peña Bética de Málaga - Landing Page

Bienvenido al repositorio de la página web de la **Peña Bética de Málaga**, una organización de aficionados al Real Betis Balompié.

## 📋 Descripción

Esta es una landing page responsiva y moderna para la Peña Bética de Málaga que incluye:

- 🏠 Página principal con secciones informativas
- 📋 Formulario de registro con validación
- 🎨 Diseño responsivo (móvil, tablet, desktop)
- ✅ Validación de campos en tiempo real
- 💚⚪ Colores del Real Betis Balompié

## 🎯 Características principales

### Formulario de Registro
El formulario incluye dos campos obligatorios con validaciones específicas:

#### 1. **DNI**
- Longitud: exactamente 9 caracteres
- Formato: 8 números + 1 letra (ejemplo: 12345678X)
- Validación en tiempo real
- Acepta letras mayúsculas y minúsculas

#### 2. **Teléfono**
- Longitud: exactamente 9 dígitos
- Solo acepta números
- Validación en tiempo real
- Máximo 9 dígitos

### Validaciones
- ✓ Validación de longitud (min/max = 9)
- ✓ Validación de formato (DNI: números + letra, Teléfono: solo números)
- ✓ Mensajes de error en tiempo real
- ✓ Validación al perder el foco
- ✓ Validación al enviar el formulario

## 📁 Estructura del proyecto

```
Pbmalaga/
├── index.html      # Página principal HTML
├── styles.css      # Estilos CSS
├── script.js       # JavaScript para validaciones e interactividad
└── README.md       # Este archivo
```

## 🚀 Uso

1. Descarga o clona el repositorio
2. Abre el archivo `index.html` en un navegador web
3. Completa el formulario con:
   - Tu DNI (8 números + 1 letra)
   - Tu teléfono (9 dígitos)

## 🎨 Secciones de la página

### 1. **Encabezado (Header)**
- Logo del Real Betis
- Nombre de la Peña
- Menú de navegación

### 2. **Sección Hero**
- Imagen del escudo del Real Betis
- Mensaje de bienvenida
- Descripción de la Peña

### 3. **Sobre Nosotros**
- Información sobre la Peña
- Valores principales (Pasión, Comunidad, Tradición)

### 4. **Registro**
- Formulario con validación
- DNI: 8 números + 1 letra
- Teléfono: 9 números
- Mensajes de error y éxito

### 5. **Contacto**
- Información de contacto
- Email
- Ubicación

### 6. **Pie de página (Footer)**
- Información de copyright
- Lema del club

## 💻 Tecnologías utilizadas

- **HTML5**: Estructura semántica
- **CSS3**: Estilos responsivos con Grid y Flexbox
- **JavaScript**: Validaciones y interactividad

## 📱 Responsividad

La página es completamente responsiva con breakpoints en:
- 📱 Móviles: < 480px
- 📱 Tablets: 768px
- 🖥️ Desktop: > 768px

## ✨ Características de validación

### DNI
```javascript
// Formato: 8 dígitos + 1 letra
Ejemplo válido: 12345678A
Ejemplo válido: 87654321Z
```

### Teléfono
```javascript
// Formato: 9 dígitos
Ejemplo válido: 123456789
Ejemplo válido: 987654321
```

## 🔒 Validaciones en tiempo real

- Al escribir: Se filtran caracteres no permitidos
- Al perder el foco: Se valida el formato completo
- Al enviar: Se valida nuevamente antes de procesar

## 📝 Notas

- La longitud mínima y máxima de ambos campos es **9 caracteres**
- El teléfono solo acepta **números**
- El DNI acepta **8 números y 1 letra**
- Los mensajes de error son específicos para cada tipo de validación

## 🎯 Próximas mejoras

- [ ] Integración con backend para guardar datos
- [ ] Confirmación por email
- [ ] Galería de eventos
- [ ] Blog con noticias
- [ ] Sección de fotos y vídeos

## 👥 Contribuciones

Las contribuciones son bienvenidas. Por favor, abre un issue o pull request para sugerir cambios.

## 📞 Contacto

**Peña Bética de Málaga**
- 📧 Email: info@penabetics.com
- 📍 Ubicación: Málaga, Andalucía, España

## ⚖️ Licencia

Este proyecto es de uso interno para la Peña Bética de Málaga.

---

**¡Viva el Real Betis Balompié! 💚⚪**
