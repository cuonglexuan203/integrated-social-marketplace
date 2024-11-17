export class Helper {
    static clearLocalStorage() {
        try {
            window.localStorage.clear();
        } catch (ex) {
            console.error(ex);
        }
    }
}
