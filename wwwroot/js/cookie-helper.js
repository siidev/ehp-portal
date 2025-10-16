// Cookie helper functions for Blazor
window.setCookie = function (name, value, hours) {
    const expires = new Date();
    expires.setTime(expires.getTime() + (hours * 60 * 60 * 1000));
    document.cookie = `${name}=${encodeURIComponent(value)};expires=${expires.toUTCString()};path=/;SameSite=Lax`;
    console.log(`Cookie set: ${name} = ${value}`);
};

window.getCookie = function (name) {
    const nameEQ = name + "=";
    const ca = document.cookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0) {
            const value = decodeURIComponent(c.substring(nameEQ.length, c.length));
            console.log(`Cookie retrieved: ${name} = ${value}`);
            return value;
        }
    }
    console.log(`Cookie not found: ${name}`);
    return null;
};

window.removeCookie = function (name) {
    document.cookie = `${name}=;expires=Thu, 01 Jan 1970 00:00:00 UTC;path=/;`;
    console.log(`Cookie removed: ${name}`);
};