async function suscribirWebPush(idUsuario) {

    // Verificar soporte del navegador
    if (!('Notification' in window) || !('serviceWorker' in navigator)) {
        console.warn('Este navegador no soporta notificaciones.');
        return;
    }

    // Solicitar permiso
    const permiso = await Notification.requestPermission();

    if (permiso !== 'granted') {
        console.warn('Permiso de notificaciones denegado.');
        return;
    }

    try {
        const registration = await navigator.serviceWorker.ready;

        // Suscribir al navegador
        const subscription = await registration.pushManager.subscribe({
            userVisibleOnly: true,
            applicationServerKey: urlBase64ToUint8Array(vapidPublicKey)
        });

        const sub = subscription.toJSON();

        // Enviar suscripción al backend
        //await fetch('/webpush/suscribir', {
        await fetch('/Notifications/api/webpush/suscribir', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                ctUsua_Id: idUsuario,
                endpoint: sub.endpoint,
                keys: {
                    p256dh: sub.keys.p256dh,
                    auth: sub.keys.auth
                }
            })
        });

        console.log('Suscripción Web Push registrada correctamente.');

    } catch (err) {
        console.error('Error al suscribir Web Push:', err);
    }
}

// Convierte la VAPID key a formato que entiende el navegador
function urlBase64ToUint8Array(base64String) {
    const padding = '='.repeat((4 - base64String.length % 4) % 4);
    const base64 = (base64String + padding).replace(/-/g, '+').replace(/_/g, '/');
    const rawData = atob(base64);
    return Uint8Array.from([...rawData].map(c => c.charCodeAt(0)));
}