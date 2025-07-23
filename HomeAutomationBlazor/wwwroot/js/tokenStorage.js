window.authToken = {
    set: function(token) {
        if (token) {
            localStorage.setItem('authToken', token);
        }
    },
    get: function() {
        return localStorage.getItem('authToken');
    },
    clear: function() {
        localStorage.removeItem('authToken');
    }
};
