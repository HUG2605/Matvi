self.addEventListener('push', event => {

    if (!event.data) return;

    const { titulo, cuerpo, datos } = event.data.json();

    const options = {
            body: cuerpo,
            icon: '/assets/images/logo-sm.png',
            data: datos,
            requireInteraction: true
        };

    event.waitUntil(
        self.registration.showNotification(titulo, options)
    );
});

// Al tocar la notificación → navegar al pedido
self.addEventListener('notificationclick', event => {
    event.notification.close();

    const datos = event.notification.data;

    let url = '/';

    if (datos?.tipo === 'PEDIDO_EN_PREPARACION')
        url = `/Ventas/Pedido`;

    event.waitUntil(
        clients.matchAll({ type: 'window', includeUncontrolled: true })
            .then(clientList => {
                // Si ya hay una pestaña abierta → navegar ahí
                const cliente = clientList.find(c => c.focused || c.visibilityState === 'visible');
                if (cliente) return cliente.navigate(url);

                // Si no hay pestaña abierta → abrir una nueva
                return clients.openWindow(url);
            })
    );
});