// In development, always fetch from the network and do not enable offline support.
// This is because caching would make development more difficult (changes would not
// be reflected on the first load after each change).

self.addEventListener("install", (event) => {
    event.waitUntil(
        caches.open(cacheName).then((cache) => cache.addAll(
            [
                "/css/bootstrap/bootstrap.min.css",
                "/css/app.css",
                "/WordleHelper.styles.css",
                "/data/English (International).dic",
                "/data/English%20(International).dic",
                "/data/Espanol.dic"
            ]
        ))
    );
});

self.addEventListener("fetch", (event) => {
    event.respondWith(
        caches.open("mysite-dynamic").then((cache) => fetch(event.request)
            .then((response) => {
                cache.put(event.request, response.clone());
                return response;
            }))
    );
});
