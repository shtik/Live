var Shtik;
(function (Shtik) {
    var AutoNav;
    (function (AutoNav) {
        function loadSlide(url) {
            return fetch(url, { method: "GET" }).then(function (response) { return response.text(); });
        }
        function transition() {
            var url = window.location.href + "/partial";
            loadSlide(url)
                .then(function (json) {
                var partial = JSON.parse(json);
                document.querySelector("div.slide").innerHTML = partial.slide.html;
            });
        }
        function go(href) {
            history.pushState(null, null, href);
            transition();
        }
        document.addEventListener("DOMContentLoaded", function () {
            var protocol = window.location.protocol === "https:" ? "wss:" : "ws:";
            var path = window.location.pathname.substr(5).replace(/\/[0-9]+$/, "");
            var wsUri = protocol + "//" + window.location.host + path;
            var socket = new WebSocket(wsUri);
            socket.onmessage = function (e) {
                var data = JSON.parse(e.data);
                if (data.slide) {
                    go(window.location.pathname.replace(/\/[0-9]+$/, "" + data.slide));
                }
            };
            window.addEventListener("popstate", transition);
        });
    })(AutoNav = Shtik.AutoNav || (Shtik.AutoNav = {}));
})(Shtik || (Shtik = {}));
