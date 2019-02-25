window.addEventListener("load", function () {
    var a = document.querySelector("a.PostLogoutRedirectUri");
    if (a) {
        var countdown = 5;
        var interval = setInterval(function () {
            if (countdown == 0) {
                clearInterval();
                window.location = a.href;
            }
            countdown--;
            document.getElementById('countdown').innerText = countdown;
        }, 1000);
    }
});
