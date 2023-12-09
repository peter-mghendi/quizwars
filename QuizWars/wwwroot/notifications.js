/**
 * Represents details of a user.
 *
 * @typedef {Object} User
 * @property {string} userId - The unique identifier for the user.
 * @property {string} email - The email address of the user.
 */

/**
 * Represents details of a topic.
 *
 * @typedef {Object} Topic
 * @property {number} id - The unique identifier for the topic.
 * @property {string} title - The title of the topic.
 * @property {string} description - The description of the topic.
 */

/**
 * Represents details of a game associated with a notification.
 *
 * @typedef {Object} Game
 * @property {number} id - The unique identifier for the game.
 * @property {string} identifier - The unique identifier for the game instance.
 * @property {Topic} topic - The details of the topic associated with the game.
 * @property {User} playerOne - Details of the first player (challenger).
 * @property {User} playerTwo - Details of the second player (challenged).
 */

/**
 * Represents a sample payload for a notification.
 *
 * @typedef {Object} Notification
 * @property {number} id - The unique identifier for the notification.
 * @property {number} action - The action code associated with the notification. 0: Play, 1: Results
 * @property {string} sentAt - The timestamp indicating when the notification was sent.
 * @property {string|null} readAt - The timestamp indicating when the notification was read (null if not yet read).
 * @property {Game} game - Details of the game associated with the invitation.
 * @property {boolean} isRead - Indicates whether the notification has been read (true) or not (false).
 * @property {string} text - The text content of the notification.
 * @property {string} url - The URL to initiate gameplay for the given invitation.
 */


/**
 * Sample payload:
 *
 * <pre>
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
 * </pre>
 */

export function showNotification(event) {
    /** @type {Notification} */
    const payload = event.data.json();
    console.log(payload);

    event.waitUntil(
        self.registration.showNotification('QuizWars', {
            body: payload.text,
            icon: 'icon-512.png',
            vibrate: [100, 50, 100],
            data: {url: payload.url}
        })
    );
}

export function openNotificationAction(event) {
    event.notification.close();

    /** @type {Notification} */
    const data = event.notification.data;
    event.waitUntil(clients.openWindow(data.url));
}