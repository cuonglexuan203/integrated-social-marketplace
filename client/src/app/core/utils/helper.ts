export class Helper {
    static clearLocalStorage() {
        try {
            window.localStorage.clear();
        } catch (ex) {
            console.error(ex);
        }
    }

    static getUserFromLocalStorage() {
        try {
            const userData = window.localStorage.getItem('user');
            return userData ? JSON.parse(userData) : null;
        } catch (ex) {
            console.error(ex);
            return null;
        }
    }

    static getCurrentTime() {
        return new Date().getTime();
    }
}
