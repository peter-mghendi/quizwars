// In development, always fetch from the network and do not enable offline support.
// This is because caching would make development more difficult (changes would not
// be reflected on the first load after each change).
self.addEventListener('fetch', () => { });
/**
 * Represents a sample payload for a game invitation notification.
 *
 * @typedef {Object} GameInvitationPayload
 * @property {number} id - The unique identifier for the notification.
 * @property {number} action - The action code associated with the notification.
 * @property {string} sentAt - The timestamp indicating when the notification was sent.
 * @property {string|null} readAt - The timestamp indicating when the notification was read (null if not yet read).
 * @property {Object} game - The details of the game associated with the invitation.
 * @property {number} game.id - The unique identifier for the game.
 * @property {string} game.identifier - The unique identifier for the game instance.
 * @property {Object} game.topic - The topic details of the game.
 * @property {number} game.topic.id - The unique identifier for the topic.
 * @property {string} game.topic.title - The title of the game topic.
 * @property {string} game.topic.description - The description of the game topic.
 * @property {Object} game.playerOne - Details of the first player (challenger).
 * @property {string} game.playerOne.userId - The unique identifier for the first player.
 * @property {string} game.playerOne.email - The email address of the first player.
 * @property {Object} game.playerTwo - Details of the second player (challenged).
 * @property {string} game.playerTwo.userId - The unique identifier for the second player.
 * @property {string} game.playerTwo.email - The email address of the second player.
 * @property {boolean} isRead - Indicates whether the notification has been read (true) or not (false).
 * @property {string} text - The text content of the notification.
 * @property {string} url - The URL to initiate gameplay for the given invitation.
 */

/**
 * Sample payload:
 * 
 * <pre><code>
 * {
 *   "id": 29,
 *   "action": 0,
 *   "sentAt": "2023-12-07T21:03:30.2757486+00:00",
 *   "readAt": null,
 *   "game": {
 *     "id": 25,
 *     "identifier": "31827102-0e9e-4474-a7c5-365ff8e13d06",
 *     "topic": {
 *       "id": 2,
 *       "title": "Harry Potter",
 *       "description": "Delve into the magical universe of wizards and spells."
 *     },
 *     "playerOne": {
 *       "userId": "dedd16a8-4544-46c4-b3f8-d9abddc592eb",
 *       "email": "person@mail.com"
 *     },
 *     "playerTwo": {
 *       "userId": "a8d6f1a2-3542-44de-b4e4-42cf5908c609",
 *       "email": "user@mail.com"
 *     }
 *   },
 *   "isRead": false,
 *   "text": "person@mail.com has challenged you in Harry Potter. Play now!",
 *   "url": "game/31827102-0e9e-4474-a7c5-365ff8e13d06/play"
 * }
 * </code></pre>
 */

self.addEventListener('push', event => {
    /** @type {GameInvitationPayload} */
    const payload = event.data.json();
    console.log(payload);
    
    event.waitUntil(
        self.registration.showNotification('QuizWars', {
            body: payload.text,
            icon: 'icon-512.png',
            vibrate: [100, 50, 100],
            data: { url: payload.url }
        })
    );
});

self.addEventListener('notificationclick', event => {
    event.notification.close();
    event.waitUntil(clients.openWindow(event.notification.data.url));
});