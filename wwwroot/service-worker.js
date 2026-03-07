// Standard service worker for a Blazor Server app
// This basically allows the app to be installed and provides a fallback if offline.

self.addEventListener('install', (event) => {
    console.log('Service worker installing...');
    self.skipWaiting();
});

self.addEventListener('activate', (event) => {
    console.log('Service worker activating...');
});

self.addEventListener('fetch', (event) => {
    // For Blazor Server, we generally just pass through the fetch requests
    // since the app logic resides on the server.
});
